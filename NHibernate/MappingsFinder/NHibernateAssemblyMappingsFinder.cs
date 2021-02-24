using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Itb.DalCore.NHibernate.Mappings {
	/// <summary> Assemblies NHibernate mappings finder
	/// </summary>
	/// <remarks>
	/// Loads all assemblies and add them to NHibernate config for scanning mapping resources (*.hbm.xml)
	/// </remarks>
	//TRANS
	public sealed class NHibernateAssemblyMappingsFinder : INHibernateMappingsFinder {
		private class IgnoreCaseComparer : IEqualityComparer<string> {
			public bool Equals(string x, string y) {
				return string.Equals(x, y, StringComparison.CurrentCultureIgnoreCase);
			}
			public int GetHashCode(string obj) {
				return obj.ToLower().GetHashCode();
			}
		}
		private readonly static IgnoreCaseComparer Comparer = new IgnoreCaseComparer();

		private string[] _availableCompanyNames;
        private readonly ILogger logger;

        /// <summary> Create new assembly mappings finder
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="availableCompanyNames">available company names from Assembly Info for which try to find mapping resources</param>
        public NHibernateAssemblyMappingsFinder(ILogger logger, params string[] availableCompanyNames)
        {
			_availableCompanyNames = availableCompanyNames;
            this.logger = logger;
		}

        /// <summary> Create new assembly mappings finder
        /// </summary>
        /// <param name="availableCompanyNames">available company names from Assembly Info for which try to find mapping resources</param>
        public NHibernateAssemblyMappingsFinder(params string[] availableCompanyNames)
            : this(null, availableCompanyNames)
        {
        }

#pragma warning disable 1591
        public void AddMappings(ref global::NHibernate.Cfg.Configuration config)
        {
			LoadAssemblies();
			foreach(var asm in AppDomain.CurrentDomain.GetAssemblies()) {
				// при любом обращении к динамической сборке появляется Exception
				try {
					var tmp = asm.Location;
				} catch(Exception) {
					continue;
				}
				if(IsSearchMappingsInAssembly(asm)) {
					config.AddAssembly(asm);
					if(this.logger?.IsEnabled(LogLevel.Information) == true) {
						this.logger.LogInformation("Mappings added from assembly: " + asm.FullName);
					}
				}
				else {
					if(this.logger?.IsEnabled(LogLevel.Debug) == true) {
						this.logger.LogDebug("Skip finding mappings from assembly: " + asm.FullName);
					}
				}
			}
		}
#pragma warning restore 1591

		private static string GetCompanyName(Assembly asm) {
			string companyName = string.Empty;
			object[] attr = asm.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
			if(attr.Length > 0) {
				companyName = ((AssemblyCompanyAttribute)attr[0]).Company;
			}
			return companyName;
		}
		private bool IsSearchMappingsInAssembly(Assembly asm) {
			bool res = true;
			if(null != _availableCompanyNames && _availableCompanyNames.Count() > 0) {
				string companyName = GetCompanyName(asm);
				res = _availableCompanyNames.Contains(companyName, Comparer);
			}
			return res;
		}
		/// <summary> Load all application assemblies
		/// </summary>
		/// <returns></returns>
		private void LoadAssemblies() {
			Queue<Assembly> assemblies = new Queue<Assembly>(AppDomain.CurrentDomain.GetAssemblies());
			List<string> asmNames = new List<string>();
			//помещаем в список имена всех загруженных сборок
			asmNames.AddRange(assemblies.Select(x => x.FullName));
			//UNDONE: очень трудоемкий процесс, найти способ избежать загрузку ВСЕХ dll
			while(assemblies.Count > 0) {
				Assembly asm = assemblies.Dequeue();
				if(IsSearchMappingsInAssembly(asm)) {
					//для каждой загруженной сборки просматриваем ReferencedAssemblies и загружаем, если они еще не загружены
					foreach(var refAsm in asm.GetReferencedAssemblies()) {
						if(!asmNames.Contains(refAsm.FullName) 
							//загружаем только неподписанные сборки (т.е. наши)
							&& refAsm.GetPublicKeyToken().Length == 0) {
							assemblies.Enqueue(Assembly.Load(refAsm.FullName));
							asmNames.Add(refAsm.FullName);
						}
					}
				}
			}
		}
	}
}
