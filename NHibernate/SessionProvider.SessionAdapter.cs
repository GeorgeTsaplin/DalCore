using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Stat;
using NHibernate.Type;

namespace Itb.DalCore.NHibernate
{
    partial class SessionProvider
	{
		internal class SessionAdapter : ISession
		{
			readonly ISession _original;
			bool _allowDisposeOriginalSession;
			public SessionAdapter(ISession session, bool allowDisposeOriginalSession)
			{
				if (null == session)
				{
					throw new ArgumentNullException("session");
				}
				_original = session;
				_allowDisposeOriginalSession = allowDisposeOriginalSession;
			}

			~SessionAdapter()
			{
				Dispose(false);
			}

			public void AllowDisposeOriginalSession(bool value)
			{
				_allowDisposeOriginalSession = value;
			}

			#region ISession
#pragma warning disable 1591

			public void Flush()
			{
				_original.Flush();
			}
			public DbConnection Disconnect()
			{
				return _original.Disconnect();
			}
			public void Reconnect()
			{
				_original.Reconnect();
			}
			public void Reconnect(DbConnection connection)
			{
				_original.Reconnect(connection);
			}
			public DbConnection Close()
			{
				return _original.Close();
			}
			public void CancelQuery()
			{
				_original.CancelQuery();
			}
			public bool IsDirty()
			{
				return _original.IsDirty();
			}
			public object GetIdentifier(object obj)
			{
				return _original.GetIdentifier(obj);
			}
			public bool Contains(object obj)
			{
				return _original.Contains(obj);
			}
			public void Evict(object obj)
			{
				_original.Evict(obj);
			}
			public object Load(Type theType, object id, global::NHibernate.LockMode lockMode)
			{
				return _original.Load(theType, id, lockMode);
			}
			public object Load(string entityName, object id, global::NHibernate.LockMode lockMode)
			{
				return _original.Load(entityName, id, lockMode);
			}
			public object Load(Type theType, object id)
			{
				return _original.Load(theType, id);
			}
			public T Load<T>(object id, global::NHibernate.LockMode lockMode)
			{
				return _original.Load<T>(id, lockMode);
			}
			public T Load<T>(object id)
			{
				return _original.Load<T>(id);
			}
			public object Load(string entityName, object id)
			{
				return _original.Load(entityName, id);
			}
			public void Load(object obj, object id)
			{
				_original.Load(obj, id);
			}
			public void Replicate(object obj, ReplicationMode replicationMode)
			{
				_original.Replicate(obj, replicationMode);
			}
			public void Replicate(string entityName, object obj, ReplicationMode replicationMode)
			{
				_original.Replicate(entityName, obj, replicationMode);
			}
			public object Save(object obj)
			{
				return _original.Save(obj);
			}
			public void Save(object obj, object id)
			{
				_original.Save(obj, id);
			}
			public object Save(string entityName, object obj)
			{
				return _original.Save(entityName, obj);
			}
			public void SaveOrUpdate(object obj)
			{
				_original.SaveOrUpdate(obj);
			}
			public void SaveOrUpdate(string entityName, object obj)
			{
				_original.SaveOrUpdate(entityName, obj);
			}
			public void Update(object obj)
			{
				_original.Update(obj);
			}
			public void Update(object obj, object id)
			{
				_original.Update(obj, id);
			}
			public void Update(string entityName, object obj)
			{
				_original.Update(entityName, obj);
			}
			public object Merge(object obj)
			{
				return _original.Merge(obj);
			}
			public object Merge(string entityName, object obj)
			{
				return _original.Merge(entityName, obj);
			}
			public void Persist(object obj)
			{
				_original.Persist(obj);
			}
			public void Persist(string entityName, object obj)
			{
				_original.Persist(entityName, obj);
			}

            public void Delete(object obj)
			{
				_original.Delete(obj);
			}
			public void Delete(string entityName, object obj)
			{
				_original.Delete(entityName, obj);
			}
			public int Delete(string query)
			{
				return _original.Delete(query);
			}
			public int Delete(string query, object value, IType type)
			{
				return _original.Delete(query, value, type);
			}
			public int Delete(string query, object[] values, IType[] types)
			{
				return _original.Delete(query, values, types);
			}
			public void Lock(object obj, global::NHibernate.LockMode lockMode)
			{
				_original.Load(obj, lockMode);
			}
			public void Lock(string entityName, object obj, global::NHibernate.LockMode lockMode)
			{
				_original.Load(entityName, obj, lockMode);
			}
			public void Refresh(object obj)
			{
				_original.Refresh(obj);
			}
			public void Refresh(object obj, global::NHibernate.LockMode lockMode)
			{
				_original.Refresh(obj, lockMode);
			}
			public global::NHibernate.LockMode GetCurrentLockMode(object obj)
			{
				return _original.GetCurrentLockMode(obj);
			}
			public ITransaction BeginTransaction()
			{
				return _original.BeginTransaction();
			}
			public ITransaction BeginTransaction(IsolationLevel isolationLevel)
			{
				return _original.BeginTransaction(isolationLevel);
			}
			public ICriteria CreateCriteria<T>() where T : class
			{
				return _original.CreateCriteria<T>();
			}
			public ICriteria CreateCriteria<T>(string alias) where T : class
			{
				return _original.CreateCriteria<T>(alias);
			}
			public ICriteria CreateCriteria(Type persistentClass)
			{
				return _original.CreateCriteria(persistentClass);
			}
			public ICriteria CreateCriteria(Type persistentClass, string alias)
			{
				return _original.CreateCriteria(persistentClass, alias);
			}
			public ICriteria CreateCriteria(string entityName)
			{
				return _original.CreateCriteria(entityName);
			}
			public ICriteria CreateCriteria(string entityName, string alias)
			{
				return _original.CreateCriteria(entityName, alias);
			}
			public IQueryOver<T, T> QueryOver<T>() where T : class
			{
				return _original.QueryOver<T>();
			}
			public IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias) where T : class
			{
				return _original.QueryOver<T>(alias);
			}
			public IQuery CreateQuery(string queryString)
			{
				return _original.CreateQuery(queryString);
			}
			public IQuery CreateFilter(object collection, string queryString)
			{
				return _original.CreateFilter(collection, queryString);
			}
			public IQuery GetNamedQuery(string queryName)
			{
				return _original.GetNamedQuery(queryName);
			}
			public ISQLQuery CreateSQLQuery(string queryString)
			{
				return _original.CreateSQLQuery(queryString);
			}
			public void Clear()
			{
				_original.Clear();
			}
			public object Get(Type clazz, object id)
			{
				return _original.Get(clazz, id);
			}
			public object Get(Type clazz, object id, global::NHibernate.LockMode lockMode)
			{
				return _original.Get(clazz, id, lockMode);
			}
			public object Get(string entityName, object id)
			{
				return _original.Get(entityName, id);
			}
			public T Get<T>(object id)
			{
				return _original.Get<T>(id);
			}
			public T Get<T>(object id, global::NHibernate.LockMode lockMode)
			{
				return _original.Get<T>(id, lockMode);
			}
			public string GetEntityName(object obj)
			{
				return _original.GetEntityName(obj);
			}
			public IFilter EnableFilter(string filterName)
			{
				return _original.EnableFilter(filterName);
			}
			public IFilter GetEnabledFilter(string filterName)
			{
				return _original.GetEnabledFilter(filterName);
			}
			public void DisableFilter(string filterName)
			{
				_original.DisableFilter(filterName);
			}
            [Obsolete("Use ISession.CreateQueryBatch instead.")]
            public IMultiQuery CreateMultiQuery()
			{
				return _original.CreateMultiQuery();
			}
			public ISession SetBatchSize(int batchSize)
			{
				return _original.SetBatchSize(batchSize);
			}
			public ISessionImplementor GetSessionImplementation()
			{
				return _original.GetSessionImplementation();
			}
            [Obsolete("Use ISession.CreateQueryBatch instead.")]
            public IMultiCriteria CreateMultiCriteria()
			{
                return _original.CreateMultiCriteria();
            }
            [Obsolete("Please use SessionWithOptions instead. Now requires to be flushed and disposed of.")]
            public ISession GetSession(EntityMode entityMode)
			{
                return _original.GetSession(entityMode);
            }

            public FlushMode FlushMode
			{
				get { return _original.FlushMode; }
				set { _original.FlushMode = value; }
			}
			public CacheMode CacheMode
			{
				get { return _original.CacheMode; }
				set { _original.CacheMode = value; }
			}
			public ISessionFactory SessionFactory
			{
				get { return _original.SessionFactory; }
			}
			public DbConnection Connection
			{
				get { return _original.Connection; }
			}
			public bool IsOpen
			{
				get { return _original.IsOpen; }
			}
			public bool IsConnected
			{
				get { return _original.IsConnected; }
			}
			public ITransaction Transaction
			{
				get { return _original.Transaction; }
			}
			public ISessionStatistics Statistics
			{
				get { return _original.Statistics; }
			}

#pragma warning restore 1591
			#endregion //ISession

			public void Dispose()
			{
				Dispose(true);
			}

			public void Dispose(bool disposing)
			{
				if (_allowDisposeOriginalSession && disposing)
				{
					_original.Dispose();
				}
			}

			#region Члены ISession


			public bool DefaultReadOnly
			{
				get
				{
					return _original.DefaultReadOnly;
				}
				set
				{
					_original.DefaultReadOnly = value;
				}
			}

            public bool IsReadOnly(object entityOrProxy)
			{
				return _original.IsReadOnly(entityOrProxy);
			}

			public T Merge<T>(string entityName, T entity) where T : class
			{
				return _original.Merge<T>(entityName, entity);
			}

			public T Merge<T>(T entity) where T : class
			{
				return _original.Merge<T>(entity);
			}

			public IQueryOver<T, T> QueryOver<T>(string entityName, Expression<Func<T>> alias) where T : class
			{
				return _original.QueryOver<T>(entityName, alias);
			}

			public IQueryOver<T, T> QueryOver<T>(string entityName) where T : class
			{
				return _original.QueryOver<T>(entityName);
			}

			public void SetReadOnly(object entityOrProxy, bool readOnly)
			{
				_original.SetReadOnly(entityOrProxy, readOnly);
			}

            public void Save(string entityName, object obj, object id)
                => _original.Save(entityName, obj, id);

            public void SaveOrUpdate(string entityName, object obj, object id)
                => _original.SaveOrUpdate(entityName, obj, id);

            public void Update(string entityName, object obj, object id)
                => _original.Update(entityName, obj, id);

            public Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.FlushAsync(cancellationToken);
            }

            public Task<bool> IsDirtyAsync(CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.IsDirtyAsync(cancellationToken);
            }

            public Task EvictAsync(object obj, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.EvictAsync(obj, cancellationToken);
            }

            public Task<object> LoadAsync(Type theType, object id, global::NHibernate.LockMode lockMode, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.LoadAsync(theType, id, lockMode, cancellationToken);
            }

            public Task<object> LoadAsync(string entityName, object id, global::NHibernate.LockMode lockMode, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.LoadAsync(entityName, id, lockMode, cancellationToken);
            }

            public Task<object> LoadAsync(Type theType, object id, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.LoadAsync(theType, id, cancellationToken);
            }

            public Task<T> LoadAsync<T>(object id, global::NHibernate.LockMode lockMode, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.LoadAsync<T>(id, lockMode, cancellationToken);
            }

            public Task<T> LoadAsync<T>(object id, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.LoadAsync<T>(id, cancellationToken);
            }

            public Task<object> LoadAsync(string entityName, object id, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.LoadAsync(entityName, id, cancellationToken);
            }

            public Task LoadAsync(object obj, object id, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.LoadAsync(obj, id, cancellationToken);
            }

            public Task ReplicateAsync(object obj, ReplicationMode replicationMode, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.ReplicateAsync(obj, replicationMode, cancellationToken);
            }

            public Task ReplicateAsync(string entityName, object obj, ReplicationMode replicationMode, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.ReplicateAsync(entityName, obj, replicationMode, cancellationToken);
            }

            public Task<object> SaveAsync(object obj, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.SaveAsync(obj, cancellationToken);
            }

            public Task SaveAsync(object obj, object id, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.SaveAsync(obj, id, cancellationToken);
            }

            public Task<object> SaveAsync(string entityName, object obj, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.SaveAsync(entityName, obj, cancellationToken);
            }

            public Task SaveAsync(string entityName, object obj, object id, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.SaveAsync(entityName, obj, id, cancellationToken);
            }

            public Task SaveOrUpdateAsync(object obj, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.SaveOrUpdateAsync(obj, cancellationToken);
            }

            public Task SaveOrUpdateAsync(string entityName, object obj, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.SaveOrUpdateAsync(entityName, obj, cancellationToken);
            }

            public Task SaveOrUpdateAsync(string entityName, object obj, object id, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.SaveOrUpdateAsync(entityName, obj, id, cancellationToken);
            }

            public Task UpdateAsync(object obj, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.UpdateAsync(obj, cancellationToken);
            }

            public Task UpdateAsync(object obj, object id, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.UpdateAsync(obj, id, cancellationToken);
            }

            public Task UpdateAsync(string entityName, object obj, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.UpdateAsync(entityName, obj, cancellationToken);
            }

            public Task UpdateAsync(string entityName, object obj, object id, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.UpdateAsync(entityName, obj, id, cancellationToken);
            }

            public Task<object> MergeAsync(object obj, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.MergeAsync(obj, cancellationToken);
            }

            public Task<object> MergeAsync(string entityName, object obj, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.MergeAsync(entityName, obj, cancellationToken);
            }

            public Task<T> MergeAsync<T>(T entity, CancellationToken cancellationToken = default(CancellationToken)) where T : class
            {
                return _original.MergeAsync(entity, cancellationToken);
            }

            public Task<T> MergeAsync<T>(string entityName, T entity, CancellationToken cancellationToken = default(CancellationToken)) where T : class
            {
                return _original.MergeAsync(entityName, entity, cancellationToken);
            }

            public Task PersistAsync(object obj, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.PersistAsync(obj, cancellationToken);
            }

            public Task PersistAsync(string entityName, object obj, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.PersistAsync(entityName, obj, cancellationToken);
            }

            public Task DeleteAsync(object obj, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.DeleteAsync(obj, cancellationToken);
            }

            public Task DeleteAsync(string entityName, object obj, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.DeleteAsync(entityName, obj, cancellationToken);
            }

            public Task<int> DeleteAsync(string query, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.DeleteAsync(query, cancellationToken);
            }

            public Task<int> DeleteAsync(string query, object value, IType type, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.DeleteAsync(query, value, type, cancellationToken);
            }

            public Task<int> DeleteAsync(string query, object[] values, IType[] types, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.DeleteAsync(query, values, types, cancellationToken);
            }

            public Task LockAsync(object obj, global::NHibernate.LockMode lockMode, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.LockAsync(obj, lockMode, cancellationToken);
            }

            public Task LockAsync(string entityName, object obj, global::NHibernate.LockMode lockMode, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.LockAsync(entityName, obj, lockMode, cancellationToken);
            }

            public Task RefreshAsync(object obj, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.RefreshAsync(obj, cancellationToken);
            }

            public Task RefreshAsync(object obj, global::NHibernate.LockMode lockMode, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.RefreshAsync(obj, lockMode, cancellationToken);
            }

            public Task<IQuery> CreateFilterAsync(object collection, string queryString, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.CreateFilterAsync(collection, queryString, cancellationToken);
            }

            public Task<object> GetAsync(Type clazz, object id, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.GetAsync(clazz, id, cancellationToken);
            }

            public Task<object> GetAsync(Type clazz, object id, global::NHibernate.LockMode lockMode, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.GetAsync(clazz, id, lockMode, cancellationToken);
            }

            public Task<object> GetAsync(string entityName, object id, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.GetAsync(entityName, id, cancellationToken);
            }

            public Task<T> GetAsync<T>(object id, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.GetAsync<T>(id, cancellationToken);
            }

            public Task<T> GetAsync<T>(object id, global::NHibernate.LockMode lockMode, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.GetAsync<T>(id, lockMode, cancellationToken);
            }

            public Task<string> GetEntityNameAsync(object obj, CancellationToken cancellationToken = default(CancellationToken))
            {
                return _original.GetEntityNameAsync(obj, cancellationToken);
            }

            public ISharedSessionBuilder SessionWithOptions()
            {
                return _original.SessionWithOptions();
            }

            public void JoinTransaction()
            {
                _original.JoinTransaction();
            }

            public IQueryable<T> Query<T>()
            {
                return _original.Query<T>();
            }

            public IQueryable<T> Query<T>(string entityName)
            {
                return _original.Query<T>(entityName);
            }

            #endregion
        }
	}
}
