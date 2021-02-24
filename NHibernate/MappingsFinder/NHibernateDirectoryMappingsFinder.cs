using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Itb.DalCore.NHibernate.Mappings {
	/// <summary> NHibernate mappings finder
	/// </summary>
	public class NHibernateDirectoryMappingsFinder : INHibernateMappingsFinder
    {
        private readonly ILogger logger;
        private List<System.IO.DirectoryInfo> _searchPath = null;

		/// <summary> Create directory NHibernate mappings finder object
		/// </summary>
        /// <param name="logger">logger</param>
		/// <param name="searchPath">directory path</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="searchPath"/> is null
		/// </exception>
		/// <exception cref="System.Security.SecurityException">
		/// The caller does not have the required permission.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Path contains invalid characters such as <![CDATA[", <, >, or |.]]>.
		/// </exception>
		/// <exception cref="System.IO.PathTooLongException"/>
		public NHibernateDirectoryMappingsFinder(ILogger logger, params string[] searchPath) {
			_searchPath = new List<System.IO.DirectoryInfo>(searchPath.Length);
			foreach(var item in searchPath) {
				if(!string.IsNullOrEmpty(item)) {
					_searchPath.Add(new System.IO.DirectoryInfo(item));
				}
			}

            this.logger = logger;
		}

        /// <summary> Create directory NHibernate mappings finder object
		/// </summary>
		/// <param name="searchPath">directory path</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="searchPath"/> is null
		/// </exception>
		/// <exception cref="System.Security.SecurityException">
		/// The caller does not have the required permission.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Path contains invalid characters such as <![CDATA[", <, >, or |.]]>.
		/// </exception>
		/// <exception cref="System.IO.PathTooLongException"/>
        public NHibernateDirectoryMappingsFinder(params string[] searchPath)
            : this(null, searchPath)
        {
        }
	
#pragma warning disable 1591

		public void AddMappings(ref global::NHibernate.Cfg.Configuration config) {
			foreach(var dir in _searchPath) {
				config.AddDirectory(dir);
				if(this.logger?.IsEnabled(LogLevel.Information) == true) {
					this.logger.LogInformation("Mappings added from directory: " + dir.FullName);
				}
			}
		}

#pragma warning restore 1591
	}
}
