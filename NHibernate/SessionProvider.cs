using System;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;

namespace Itb.DalCore.NHibernate
{
    ///<summary> Session provider
    ///</summary>
    public partial class SessionProvider : ISessionProvider {

		private readonly object _syncObj = new object();
		private ISessionFactory _sessionFactory;
		private readonly Configuration _configuration;
        private readonly ILogger logger;

        /// <summary>
        /// Gets session factory
        /// </summary>
        protected ISessionFactory SessionFactory
		{
			get
			{
				if (this._sessionFactory != null)
				{
					return this._sessionFactory;
				}

				lock (this._syncObj)
				{
					if (this._sessionFactory == null)
					{
						this._sessionFactory = this._configuration.BuildSessionFactory();
					}
				}

				return this._sessionFactory;
			}
		}
		readonly IInterceptor _interceptor;
		
		/// <summary> Creates new session provider
		/// </summary>
		/// <param name="initializer">NHibernate initializer</param>
		/// <param name="mappingsFinder">NHibernate mappings finder</param>
		/// <param name="interceptor">NHibernate session interceptor</param>
        /// <param name="logger">logger</param>
		public SessionProvider(
            INHibernateInitializer initializer, 
            Mappings.INHibernateMappingsFinderEnumerator mappingsFinder,
			IInterceptor interceptor = null,
            ILogger logger = null)
		{
			if(null == mappingsFinder)
			{
				throw new NotImplementedException(string.Format("You must specify NHMappingsFinder of type {0} in Spring.NET"
					, typeof(Mappings.INHibernateMappingsFinderEnumerator)));
			}
			if(null == initializer)
			{
				throw new NotImplementedException(string.Format("You must specify NHInitializer of type {0} in Spring.NET"
					, typeof(INHibernateInitializer)));
			}

			_interceptor = interceptor;
			var configuration = initializer.GetConfiguration();
			// add mappings to NHibernate configuration to build SessionFactory
			foreach(var item in mappingsFinder)
			{
				item.AddMappings(ref configuration);
			}

			this._configuration = configuration;

            this.logger = logger;
		}

#pragma warning disable 1591

		public ISession CurrentSessionUoW {
			get
			{
				CheckIfDisposed();
				if(CurrentSessionContext.HasBind(SessionFactory))
				{
					return new SessionAdapter(SessionFactory.GetCurrentSession(), false);
				}
				throw new InvalidOperationException(@"Database access logic cannot be used, if session not opened.
Implicitly session usage not allowed now. Please open session explicitly through IUnitOfWorkFactory.Create method");
			}
		}
		public ISession GetSessionForUoW()
		{
			var session = this.CurrentSession;
			var sessionUoW = session as SessionAdapter;
			if(null != sessionUoW)
			{
				sessionUoW.AllowDisposeOriginalSession(false);
			}
			return (sessionUoW??session);
		}
		public ISession CurrentSession {
			get {
				CheckIfDisposed();
				ISession session = null;
				try {
					session = this.CurrentSessionUoW;

					this.logger?.LogDebug("GetCurrentSession()");
				} catch(InvalidOperationException) {
					if(null == _interceptor)
					{
						session = SessionFactory.OpenSession();

						this.logger?.LogDebug("OpenSession()");
					}
					else
					{
						session = SessionFactory.WithOptions().Interceptor(_interceptor).OpenSession();

						if(this.logger?.IsEnabled(LogLevel.Debug) == true)
						{
							this.logger.LogDebug($"OpenSession({_interceptor.GetType()})");
						}
					}
					session = new SessionAdapter(session, true);
				}
				return session;
			}
		}

		int _disposedFlag = 0;

		void CheckIfDisposed()
		{
			if(1 == _disposedFlag)
			{
				throw new ObjectDisposedException(this.GetType().Name);
			}
		}
		public void Dispose()
		{
			if(1 == System.Threading.Interlocked.CompareExchange(ref _disposedFlag, 1, 0))
			{
				return;
			}
			GC.SuppressFinalize(this);

			if (this._sessionFactory != null)
			{
				this._sessionFactory.Close();
				this._sessionFactory.Dispose();
			}
		}

		void ISessionProvider.Evict(Type persistentClass, object id)
			=> this.SessionFactory.Evict(persistentClass, id);

		void ISessionProvider.Evict(Type persistentClass)
			=> this.SessionFactory.Evict(persistentClass);

		Task ISessionProvider.EvictAsync(Type persistentClass, CancellationToken cancellationToken)
			=> this.SessionFactory.EvictAsync(persistentClass, cancellationToken);

		Task ISessionProvider.EvictAsync(Type persistentClass, object id, CancellationToken cancellationToken)
			=> this.SessionFactory.EvictAsync(persistentClass, id, cancellationToken);

		void ISessionProvider.EvictCollection(string roleName, object id)
			=> this.SessionFactory.EvictCollection(roleName, id);

		void ISessionProvider.EvictCollection(string roleName)
			=> this.SessionFactory.EvictCollection(roleName);

		Task ISessionProvider.EvictCollectionAsync(string roleName, CancellationToken cancellationToken)
			=> this.SessionFactory.EvictCollectionAsync(roleName, cancellationToken);

		Task ISessionProvider.EvictCollectionAsync(string roleName, object id, CancellationToken cancellationToken)
			=> this.SessionFactory.EvictCollectionAsync(roleName, id, cancellationToken);

		void ISessionProvider.EvictEntity(string entityName, object id)
			=> this.SessionFactory.EvictEntity(entityName, id);

		void ISessionProvider.EvictEntity(string entityName)
			=> this.SessionFactory.EvictEntity(entityName);

		Task ISessionProvider.EvictEntityAsync(string entityName, CancellationToken cancellationToken)
			=> this.SessionFactory.EvictEntityAsync(entityName, cancellationToken);

		Task ISessionProvider.EvictEntityAsync(string entityName, object id, CancellationToken cancellationToken)
			=> this.SessionFactory.EvictEntityAsync(entityName, id, cancellationToken);

		void ISessionProvider.EvictQueries(string cacheRegion)
			=> this.SessionFactory.EvictQueries(cacheRegion);

		void ISessionProvider.EvictQueries()
			=> this.SessionFactory.EvictQueries();

		Task ISessionProvider.EvictQueriesAsync(CancellationToken cancellationToken)
			=> this.SessionFactory.EvictQueriesAsync(cancellationToken);

		Task ISessionProvider.EvictQueriesAsync(string cacheRegion, CancellationToken cancellationToken)
			=> this.SessionFactory.EvictQueriesAsync(cacheRegion, cancellationToken);

#pragma warning restore 1591
	}
}