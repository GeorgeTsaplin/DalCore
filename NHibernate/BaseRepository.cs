using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Persister.Entity;

namespace Itb.DalCore.NHibernate
{
    ///<summary> Base repository
    ///</summary>
    ///<typeparam name="T">entity interface</typeparam>
    ///<typeparam name="TImpl">entity data type</typeparam>
    ///<typeparam name="ID">primary key data type</typeparam>
    public abstract class BaseRepository<T, TImpl, ID> : IRepositoryRead<T, TImpl, ID>, IRepositoryWrite<T, TImpl, ID>
		where TImpl : class, T where T : Domain.IPrimaryKey<ID> {

		private readonly ISessionProvider _sessionProvider;
		/// <summary> current session (get)
		/// </summary>
		protected ISession Session {
			get { return _sessionProvider.CurrentSessionUoW; }
		}

		/// <summary> Exec some function using session for read only operations
		/// </summary>
		/// <typeparam name="TRes">return data type</typeparam>
		/// <param name="func">function</param>
		/// <returns>function result</returns>
		protected TRes ExecInSessionRO<TRes>(Func<ISession, TRes> func)
		{
			if(null == func)
			{
				throw new ArgumentNullException("func");
			}

			using(var session = _sessionProvider.CurrentSession)
			{
				return func(session);
			}
		}

        /// <summary> Exec some function using session for read only operations async
		/// </summary>
		/// <typeparam name="TRes">return data type</typeparam>
		/// <param name="func">function</param>
        /// <param name="cancellationToken">cancellation token</param>
		/// <returns>function result</returns>
		protected async Task<TRes> ExecInSessionROAsync<TRes>(
            Func<ISession, CancellationToken, Task<TRes>> func,
            CancellationToken cancellationToken)
        {
            if (null == func)
            {
                throw new ArgumentNullException("func");
            }

            using (var session = _sessionProvider.CurrentSession)
            {
                return await func(session, cancellationToken);
            }
        }

        /// <summary> Exec some action using session for read only operations
        /// </summary>
        /// <param name="action">action</param>
        protected void ExecInSessionRO(Action<ISession> action)
		{
			if(null == action)
			{
				throw new ArgumentNullException("action");
			}

			using(var session = _sessionProvider.CurrentSession)
			{
				action(session);
			}
		}

        /// <summary> Exec some action using session for read only operations
        /// </summary>
        /// <param name="action">action</param>
        /// <param name="cancellationToken">cancellation token</param>
        protected async Task ExecInSessionROAsync(
            Func<ISession, CancellationToken, Task> action,
            CancellationToken cancellationToken)
        {
            if (null == action)
            {
                throw new ArgumentNullException("action");
            }

            using (var session = _sessionProvider.CurrentSession)
            {
                await action(session, cancellationToken);
            }
        }

        ///<summary> Create base repository object
        ///</summary>
        ///<param name="sessionProvider">session provider</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sessionProvider"/> is null.
        /// </exception>
        protected BaseRepository(ISessionProvider sessionProvider)
		{
			if(null == sessionProvider)
			{
				throw new ArgumentNullException("sessionProvider");
			}
			_sessionProvider = sessionProvider;
		}

		/// <summary> Convert specified <paramref name="lockMode"/> to <seealso cref="global::NHibernate.LockMode"/>
		/// </summary>
		/// <param name="lockMode">lock mode</param>
		/// <returns><seealso cref="global::NHibernate.LockMode"/></returns>
		protected static global::NHibernate.LockMode GetLockMode(Itb.DalCore.LockMode lockMode) {
			global::NHibernate.LockMode nhibernateLockMode = global::NHibernate.LockMode.None;
			switch(lockMode) {
			case LockMode.None:
				nhibernateLockMode = global::NHibernate.LockMode.None;
				break;
			case LockMode.Read:
				nhibernateLockMode = global::NHibernate.LockMode.Read;
				break;
			case LockMode.Upgrade:
				nhibernateLockMode = global::NHibernate.LockMode.Upgrade;
				break;
			case LockMode.Force:
				nhibernateLockMode = global::NHibernate.LockMode.Force;
				break;
			case LockMode.UpgradeNoWait:
				nhibernateLockMode = global::NHibernate.LockMode.UpgradeNoWait;
				break;
			case LockMode.Write:
				nhibernateLockMode = global::NHibernate.LockMode.Write;
				break;
			default:
				//I18N
				throw new NotImplementedException($"Unknown NHibernate LockMode: {lockMode}");
			}
			return nhibernateLockMode;
		}
        /// <summary> Get named query
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="queryName">query name</param>
        /// <returns>named query</returns>
        /// <exception cref="ArgumentException">
        /// Named query <paramref name="queryName"/> not found loaded mappings.
        /// </exception>
        protected static IQuery GetNamedQuery(ISession session, string queryName) {
			var q = session.GetNamedQuery(queryName);
			if(null == q) {
				//I18N
				throw new ArgumentException("Named query not found", queryName);
			}
			return q;
		}
		/// <summary> Get current date
		/// </summary>
		/// <returns>current date</returns>
		/// <remarks>
		/// For DBMS return current date of data source server.
		/// </remarks>
		protected DateTime GetDate() {
			const string getDateQueryName = "GetDate";
			return ExecInSessionRO<DateTime>(session =>
				{
					var q = GetNamedQuery(session, getDateQueryName);
					return q.UniqueResult<DateTime>();
				}
			);
		}

		/// <summary> Create new criteria for the entity class
		/// </summary>
		/// <param name="session">session</param>
		/// <returns>new criteria</returns>
		protected static ICriteria CreateCriteria(ISession session)
		{
			return session.CreateCriteria<TImpl>();
		}
		/// <summary> Create new criteria for the entity class with a specified alias
		/// </summary>
		/// <param name="session">session</param>
		/// <param name="alias">alias</param>
		/// <returns>new criteria</returns>
		protected static ICriteria CreateCriteria(ISession session, string alias) {
			return session.CreateCriteria<TImpl>(alias);
		}

		StringDictionary _propertyToColumnMapping;

		/// <summary> Gets property mapping to data source columns
		/// </summary>
		/// <returns></returns>
		public StringDictionary GetPropertyToColumnMapping()
		{
			if(null != _propertyToColumnMapping)
			{
				return _propertyToColumnMapping;
			}

			_propertyToColumnMapping = new StringDictionary();
			return ExecInSessionRO<StringDictionary>(session =>
				{
					ISessionFactory sessionFactory = session.SessionFactory;
					var metaData = sessionFactory.GetClassMetadata(typeof(TImpl));
					var entityPersister = (AbstractEntityPersister)metaData;
					if(entityPersister.HasIdentifierProperty)
					{
						string idPropertyName = entityPersister.IdentifierPropertyName;
						_propertyToColumnMapping.Add(idPropertyName
							, (string.IsNullOrEmpty(entityPersister.IdentifierColumnNames[0]) ? idPropertyName : entityPersister.IdentifierColumnNames[0]));
					}
					var propertyNames = entityPersister.PropertyNames;
					foreach(var propertyName in propertyNames)
					{
						var columnNameArray = entityPersister.GetPropertyColumnNames(propertyName);
						if(columnNameArray.Count() > 0)
						{
							_propertyToColumnMapping.Add(propertyName, (string.IsNullOrEmpty(columnNameArray[0]) ? propertyName : columnNameArray[0]));
						}
						else
						{
							_propertyToColumnMapping.Add(propertyName, propertyName);
						}
					}
					return _propertyToColumnMapping;
				}
			);
		}

		#region IRepositoryRead
#pragma warning disable 1591

		public virtual TImpl Get(ID id) {
			return ExecInSessionRO<TImpl>(session => session.Get<TImpl>(id));
		}

        public virtual async Task<TImpl> GetAsync(ID id, CancellationToken cancellationToken = default(CancellationToken))
            => await ExecInSessionROAsync<TImpl>((session, ct) => session.GetAsync<TImpl>(id, ct), cancellationToken);

        public virtual TImpl Get(ID id, Itb.DalCore.LockMode lockMode) {
			return ExecInSessionRO<TImpl>(session => session.Get<TImpl>(id, GetLockMode(lockMode)));
		}

        public virtual async Task<TImpl> GetAsync(ID id, LockMode lockMode, CancellationToken cancellationToken = default(CancellationToken))
            => await ExecInSessionROAsync<TImpl>((session, ct) => session.GetAsync<TImpl>(id, GetLockMode(lockMode), ct), cancellationToken);

        public TImpl Load(ID id) {
			return ExecInSessionRO<TImpl>(session => session.Load<TImpl>(id));
		}

        public async Task<TImpl> LoadAsync(ID id, CancellationToken cancellationToken = default(CancellationToken))
            => await ExecInSessionROAsync<TImpl>((session, ct) => session.LoadAsync<TImpl>(id, ct), cancellationToken);

        public TImpl Load(ID id, Itb.DalCore.LockMode lockMode) {
			return ExecInSessionRO<TImpl>(session => session.Load<TImpl>(id, GetLockMode(lockMode)));
		}

        public async Task<TImpl> LoadAsync(ID id, LockMode lockMode, CancellationToken cancellationToken = default(CancellationToken))
            => await ExecInSessionROAsync<TImpl>((session, ct) => session.LoadAsync<TImpl>(id, GetLockMode(lockMode), ct), cancellationToken);

        public void Refresh(T entity) {
			ExecInSessionRO(session => session.Refresh(entity));
		}

        public async Task RefreshAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
            => await ExecInSessionROAsync((session, ct) => session.RefreshAsync(entity, ct), cancellationToken);

        public void Refresh(T entity, LockMode lockMode) {
			ExecInSessionRO(session => session.Refresh(entity, GetLockMode(lockMode)));
		}

        public async Task RefreshAsync(T entity, LockMode lockMode, CancellationToken cancellationToken = default(CancellationToken))
            => await ExecInSessionROAsync((session, ct) => session.RefreshAsync(entity, GetLockMode(lockMode), ct), cancellationToken);

        public void Evict(T entity) {
			ExecInSessionRO(session => session.Evict(entity));
		}

        public async Task EvictAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
            => await ExecInSessionROAsync((session, ct) => session.EvictAsync(entity, ct), cancellationToken);

        public IList<TImpl> List(int top)
            => ExecInSessionRO(session => CreateCriteria(session).SetMaxResults(top).List<TImpl>());

        public async Task<IList<TImpl>> ListAsync(int top, CancellationToken cancellationToken = default(CancellationToken))
            => await ExecInSessionROAsync((session, ct) => CreateCriteria(session).SetMaxResults(top).ListAsync<TImpl>(ct), cancellationToken);

        public IList<TImpl> GetAll()
            => ExecInSessionRO(session => CreateCriteria(session).List<TImpl>());

        public async Task<IList<TImpl>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
            => await ExecInSessionROAsync((session, ct) => CreateCriteria(session).ListAsync<TImpl>(ct), cancellationToken);

#pragma warning restore 1591
        #endregion //IRepositoryRead

        #region IRepositoryWrite
#pragma warning disable 1591

        public virtual void Save(T entity) {
			this.Session.SaveOrUpdate(entity);
		}

        public virtual async Task SaveAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
        => await this.Session.SaveOrUpdateAsync(entity, cancellationToken);

        public virtual void Flush() {
            this.Session.Flush();
        }

        public virtual async Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        => await this.Session.FlushAsync(cancellationToken);

        public virtual void Delete(T entity) {
			this.Session.Delete(entity);
		}

        public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
            => await this.Session.DeleteAsync(entity, cancellationToken);

        public virtual void Lock(T entity, Itb.DalCore.LockMode lockMode) {
			this.Session.Lock(entity, GetLockMode(lockMode));
		}

        public virtual async Task LockAsync(T entity, LockMode lockMode, CancellationToken cancellationToken = default(CancellationToken))
            => await this.Session.LockAsync(entity, GetLockMode(lockMode), cancellationToken);

#pragma warning restore 1591
        #endregion //IRepositoryWrite

        /// <summary> Safe method to call ISession.Query{T}()
        /// </summary>
        /// <returns><see cref="IQueryable{T}"/> object for type of T</returns>
        public IQueryable<T> Query() {
			return this.Session.Query<T>();
		}

		#region Execute native query

		/// <summary> Set query parameters delegate
		/// </summary>
		/// <param name="q">query</param>
		/// <returns>query</returns>
		protected delegate IQuery SetQueryParameters(IQuery q);
		/// <summary> Convert query results delegate
		/// </summary>
		/// <typeparam name="TRes">data type to convert to</typeparam>
		/// <param name="data">query results array</param>
		/// <returns>converted object</returns>
		protected delegate TRes ConvertData<TRes>(object[] data) where TRes : T;
		/// <summary> Execute native query and convert results.
		/// Multi-threaded result convertion
		/// </summary>
		/// <typeparam name="TRes">query results item data type</typeparam>
		/// <param name="queryString">native query text</param>
		/// <param name="setParamsMethod">quert parameters setter delegate</param>
		/// <param name="convertMethod">query result converter delegate</param>
		/// <param name="maxThreads">max number of threads for async convert query results (don't use less then 2)</param>
		/// <returns>converted native query result</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="queryString"/> is null.
		/// <paramref name="convertMethod"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="maxThreads"/> has not supported value.
		/// </exception>
		/// <remarks>
		/// Try to use HQL, Linq-To-NHibernate and other ORM services and tools.
		/// </remarks>
		protected IList<TRes> ExecuteNativeQuery<TRes>(string queryString, SetQueryParameters setParamsMethod
				, ConvertData<TRes> convertMethod, ushort maxThreads) where TRes : T {
			if(string.IsNullOrEmpty(queryString)) {
				throw new ArgumentNullException("queryString");
			}
			if(null == convertMethod) {
				throw new ArgumentNullException("convertMethod");
			}
			if(1 == maxThreads) {
				//I18N
				throw new ArgumentOutOfRangeException("maxThreads", maxThreads, "Don't use 1(one) for maxThreads parameter. It's not make sence");
			}
			IQuery q = this.Session.CreateSQLQuery(queryString);
			if(null != setParamsMethod) {
				q = setParamsMethod.Invoke(q);
			}
			var resList = q.List();
			object[] data;
			List<TRes> res = new List<TRes>(resList.Count);
			foreach(var item in resList) {
				data = (object[])item;
				//TODO: implement multi-threaded results convertion
				res.Add(convertMethod.Invoke(data));
			}
			return res;
		}
		/// <summary> Execute native query and convert results.
		/// Convert results in main thread
		/// </summary>
		/// <typeparam name="TRes">query results item data type</typeparam>
		/// <param name="queryString">native query text</param>
		/// <param name="setParamsMethod">quert parameters setter delegate</param>
		/// <param name="convertMethod">query result converter delegate</param>
		/// <returns>converted native query result</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="queryString"/> is null.
		/// <paramref name="convertMethod"/> is null.
		/// </exception>
		/// <remarks>
		/// Try to use HQL, Linq-To-NHibernate and other ORM services and tools
		/// </remarks>
		protected IList<TRes> ExecuteNativeQuery<TRes>(string queryString, SetQueryParameters setParamsMethod
				, ConvertData<TRes> convertMethod) where TRes : T {
			return ExecuteNativeQuery<TRes>(queryString, setParamsMethod, convertMethod, 0);
		}

		#endregion //Execute native query

		}
}