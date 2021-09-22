using NHibernate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Itb.DalCore.NHibernate
{
	///<summary> Session provider interface
	///</summary>
	public interface ISessionProvider : IDisposable
	{
		///<summary> current session binded to Unit of Work (get)
		///<remarks>transactional mode</remarks>
		///</summary>
		ISession CurrentSessionUoW { get; }
		///<summary> current session (get)
		///<remarks>preferred for read only operations</remarks>
		///</summary>
		ISession CurrentSession { get; }
		/// <summary> Gets or creates session for Unot of Work
		/// </summary>
		/// <returns>unit of work session</returns>
		ISession GetSessionForUoW();

        /// <summary>
        /// Evict an entry from the process-level cache. This method occurs outside of any transaction;
        /// it performs an immediate "hard" remove, so does not respect any transaction isolation semantics of the usage strategy.
        /// Use with care.
        /// </summary>
        /// <param name="persistentClass"></param>
        /// <param name="id"></param>
        void Evict(System.Type persistentClass, object id);
        /// <summary>
        /// Evict all entries from the process-level cache. This method occurs outside of
        /// any transaction; it performs an immediate "hard" remove, so does not respect
        /// any transaction isolation semantics of the usage strategy. Use with care.
        /// </summary>
        /// <param name="persistentClass"
        void Evict(System.Type persistentClass);

        /// <summary>
        /// Evict all entries from the process-level cache. This method occurs outside of
        /// any transaction; it performs an immediate "hard" remove, so does not respect
        /// any transaction isolation semantics of the usage strategy. Use with care.
        /// </summary>
        /// <param name="persistentClass"></param>
        /// <param name="cancellationToken"></param>
        Task EvictAsync(System.Type persistentClass, CancellationToken cancellationToken = default);

        /// <summary>
        /// Evict an entry from the process-level cache. This method occurs outside of any
        /// transaction; it performs an immediate "hard" remove, so does not respect any
        /// transaction isolation semantics of the usage strategy. Use with care.
        /// </summary>
        Task EvictAsync(System.Type persistentClass, object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Evict an entry from the process-level cache. This method occurs outside of any
        /// transaction; it performs an immediate "hard" remove, so does not respect any
        /// transaction isolation semantics of the usage strategy. Use with care.
        /// </summary>
        void EvictCollection(string roleName, object id);

        /// <summary>
        /// Evict all entries from the process-level cache. This method occurs outside of
        /// any transaction; it performs an immediate "hard" remove, so does not respect
        /// any transaction isolation semantics of the usage strategy. Use with care.
        /// </summary>
        void EvictCollection(string roleName);
        //
        /// <summary>
        /// Evict all entries from the process-level cache. This method occurs outside of
        /// any transaction; it performs an immediate "hard" remove, so does not respect
        /// any transaction isolation semantics of the usage strategy. Use with care.
        /// </summary>
        Task EvictCollectionAsync(string roleName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Evict an entry from the process-level cache. This method occurs outside of any
        /// transaction; it performs an immediate "hard" remove, so does not respect any
        /// transaction isolation semantics of the usage strategy. Use with care.
        /// </summary>
        Task EvictCollectionAsync(string roleName, object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Evict an entry from the second-level cache. This method occurs outside of any
        /// transaction; it performs an immediate "hard" remove, so does not respect any
        /// transaction isolation semantics of the usage strategy. Use with care.
        /// </summary>
        void EvictEntity(string entityName, object id);
        //
        /// <summary>
        /// Evict all entries from the second-level cache. This method occurs outside of
        /// any transaction; it performs an immediate "hard" remove, so does not respect
        /// any transaction isolation semantics of the usage strategy. Use with care.
        /// </summary>
        void EvictEntity(string entityName);
        /// <summary>
        /// Evict all entries from the second-level cache. This method occurs outside of
        /// any transaction; it performs an immediate "hard" remove, so does not respect
        /// any transaction isolation semantics of the usage strategy. Use with care.
        /// </summary>
        Task EvictEntityAsync(string entityName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Evict an entry from the second-level cache. This method occurs outside of any
        /// transaction; it performs an immediate "hard" remove, so does not respect any
        /// transaction isolation semantics of the usage strategy. Use with care.
        /// </summary>
        Task EvictEntityAsync(string entityName, object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Evict any query result sets cached in the named query cache region.
        /// </summary>
        void EvictQueries(string cacheRegion);

        /// <summary>
        /// Evict any query result sets cached in the default query cache region.
        /// </summary>
        void EvictQueries();

        /// <summary>
        /// Evict any query result sets cached in the default query cache region.
        /// </summary>
        Task EvictQueriesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Evict any query result sets cached in the named query cache region.
        /// </summary>
        Task EvictQueriesAsync(string cacheRegion, CancellationToken cancellationToken = default);

    }
}