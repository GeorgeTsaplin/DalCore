using System;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;

namespace Itb.DalCore.NHibernate.UnitOfWork {
	/// <summary> NHibernate helper
	/// </summary>
	/// <remarks>
	/// For correct work you must add implementations of <see cref="Mappings.INHibernateMappingsFinder"/> (NHMappingsFinder) and <see cref="INHibernateInitializer"/> (NHInitializer)
	/// to Spring.NET
	/// </remarks>
	internal static class NHibernateHelper
    {
		private static readonly object _lockObject = new object();
		private static Configuration _configuration;
		private static ISessionFactory _sessionFactory;

		/// <summary> session factory (get)
		/// </summary>
		/// <exception cref="NotImplementedException">
		/// Not found implementation of <see cref="Itb.DalCore.NHibernate.INHibernateInitializer"/> (NHMappingsFinder) in Spring.NET.
		/// Not found implementation of <see cref="Itb.DalCore.NHibernate.Mappings.INHibernateMappingsFinderEnumerator"/> (NHInitializer) in Spring.NET.
		/// </exception>
		public static ISessionFactory SessionFactory {
			get {
				if(null == _sessionFactory) {
					lock(_lockObject) {
						if(null == _sessionFactory) {
							_sessionFactory = Configuration.BuildSessionFactory();
						}
					}
				}
				return _sessionFactory;
			}
		}
		private static Configuration Configuration {
			get {
				if(null == _configuration) {
					lock(_lockObject) {
						if(null == _configuration) {
							var mappingsFinderEnumerator = CTI.Spring.IoC.Get<Mappings.INHibernateMappingsFinderEnumerator>("NHMappingsFinder");
							if(null == mappingsFinderEnumerator)
							{
								throw new NotImplementedException(string.Format("You must specify NHMappingsFinder of type {0} in Spring.NET"
									, typeof(Mappings.INHibernateMappingsFinderEnumerator)));
							}
							var initializer = CTI.Spring.IoC.Get<INHibernateInitializer>("NHInitializer");
							if(null == initializer)
							{
								throw new NotImplementedException(string.Format("You must specify NHInitializer of type {0} in Spring.NET"
									, typeof(INHibernateInitializer)));
							}
							_configuration = initializer.GetConfiguration();
							// add mappings to NHibernate configuration to build SessionFactory
							foreach(var mappingsFinder in mappingsFinderEnumerator) {
								mappingsFinder.AddMappings(ref _configuration);
							}
						}
					}
				}
				return _configuration;
			}
		}
		/// <summary> Open session
		/// </summary>
        /// <param name="logger">logger</param>
		/// <returns>NHibernate session</returns>
		/// <remarks>For opening session with <seealso cref="global::NHibernate.IInterceptor"/> you must specify NHInterceptor item in Spring.NET</remarks>
		public static ISession OpenSession(Common.Logging.ILog logger = null) {
			ISession session = null;
			var interceptor = CTI.Spring.IoC.Get<global::NHibernate.IInterceptor>("NHInterceptor");
			if(null == interceptor) {
				session = SessionFactory.OpenSession();

				logger?.Debug("OpenSession()");
			}
			else {
				session = SessionFactory.OpenSession(interceptor);

				if(logger?.IsDebugEnabled == true) {
					logger.Debug($"OpenSession({interceptor.GetType().AssemblyQualifiedName})");
				}
			}
			return session;
		}
		/// <summary> Get session
		/// </summary>
		/// <returns>current session</returns>
		/// <exception cref="System.NotImplementedException">
		/// Not found implementation of <see cref="Itb.DalCore.NHibernate.INHibernateInitializer"/> in Spring.NET config.
		/// Not found implementation of <see cref="Itb.DalCore.NHibernate.Mappings.INHibernateMappingsFinderEnumerator"/> in Spring.NET config.
		/// </exception>
		/// <exception cref="System.InvalidOperationException">
		/// Session does not bind to context
		/// </exception>
		public static ISession GetSession() {
			if(CurrentSessionContext.HasBind(SessionFactory)) {
				return SessionFactory.GetCurrentSession();
			}
			throw new InvalidOperationException(@"Database access logic cannot be used, if session not opened.
Implicitly session usage not allowed now. Please open session explicitly through IUnitOfWorkFactory.Create method");
		}

		/// <summary> Destroy NHibernate objects and release all resources
		/// </summary>
		public static void Dispose()
		{
			lock(_lockObject)
			{
				if(null != _sessionFactory)
				{
					_sessionFactory.Close();
					_sessionFactory.Dispose();
					_sessionFactory = null;
				}
				_configuration = null;
			}
		}
	}
}