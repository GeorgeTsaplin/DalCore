using System;
using System.Data;
using Itb.DalCore.NHibernate;
using Itb.DalCore.NHibernate.Mappings;
using Spring.Context.Support;
using Spring.Context;
using System.Collections.Generic;

namespace Itb.Tests {
	/// <summary> Base test class
	/// </summary>
	public abstract class TestFixtureBase {
#pragma warning disable 1591
		[Flags]
		protected enum MappingsFinderTypes {
			Assembly=0,
			Directory=1,
			All=Assembly | Directory
		}
		protected abstract MappingsFinderTypes MappingsFinderType { get; }
		protected virtual string[] MappingsPath {
			get {
				string path = System.IO.Path.GetDirectoryName((new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath);
				return new string[] { /*path + "\\Hbm\\MsSql2005", */path + "\\Tests\\Hbm\\MsSql2005" };
			}
		}

		public virtual void TearDown() {
			Spring.Context.Support.ContextRegistry.Clear();
			_sessionProvider.Dispose();
			_sessionProvider = null;
		}
		public virtual void SetUp()
		{
			log4net.Config.XmlConfigurator.Configure();
			
			StaticApplicationContext appContext = new StaticApplicationContext();
			RegisterObjects(appContext);
			ContextRegistry.RegisterContext(appContext);
		}
		ISessionProvider _sessionProvider;
		protected ISessionProvider SessionProvider
		{
			get { return _sessionProvider; }
		}
		Itb.DalCore.UnitOfWork.IUnitOfWorkFactory _uowFactory;
		protected Itb.DalCore.UnitOfWork.IUnitOfWorkFactory UnitOfWorkFactory
		{
			get { return _uowFactory; }
		}
		protected virtual void RegisterObjects(StaticApplicationContext appContext)
		{
			var initializer = new NHibernateSimpleInitializer();
			INHibernateMappingsFinderEnumerator mappingsFinder = null;
			{
				List<INHibernateMappingsFinder> list = new List<INHibernateMappingsFinder>(2);
				if((this.MappingsFinderType & MappingsFinderTypes.Assembly) == MappingsFinderTypes.Assembly)
				{
					list.Add(new NHibernateAssemblyMappingsFinder("CTI", "ITB"));
				}
				if((this.MappingsFinderType & MappingsFinderTypes.Directory) == MappingsFinderTypes.Directory)
				{
					list.Add(new NHibernateDirectoryMappingsFinder(this.MappingsPath));
				}
				mappingsFinder = new NHibernateMappingsFinderList(list);
			}
			_sessionProvider = new SessionProvider(initializer, mappingsFinder);
			//appContext.ObjectFactory.RegisterSingleton("SessionProvider", _sessionProvider);
			_uowFactory = new Itb.DalCore.NHibernate.UnitOfWork.NHibernateUnitOfWorkFactory(_sessionProvider);
			appContext.ObjectFactory.RegisterSingleton("UnitOfWorkFactory", _uowFactory);
		}

		public IDbCommand CreateDbCommand(NHibernate.ISession session) {
			var cmd = session.Connection.CreateCommand();
			if(null != session.Transaction) {
				session.Transaction.Enlist(cmd);
			}
			return cmd;
		}
#pragma warning restore 1591
	}
}
