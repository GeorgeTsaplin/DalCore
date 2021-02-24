using System.Threading;
using System.Threading.Tasks;

namespace Itb.DalCore
{
    /// <summary> Repository interface (read operations)
    /// </summary>
    /// <typeparam name="T">entity interface</typeparam>
    /// <typeparam name="TImpl">entity data type</typeparam>
    /// <typeparam name="ID">entity ID data type</typeparam>
    public interface IRepositoryRead<T, TImpl, ID> : IRepository where TImpl : T where T : Domain.IPrimaryKey<ID> {
		/// <summary> Get entity by id
		/// </summary>
		/// <param name="id">id value</param>
		/// <returns>entity with specified id.
		/// NULL - if not exists
		/// </returns>
		TImpl Get(ID id);

        /// <summary> Get entity by id async
        /// </summary>
        /// <param name="id">id value</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>entity with specified id.
        /// NULL - if not exists
        /// </returns>
        Task<TImpl> GetAsync(ID id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary> Get entity by id
        /// </summary>
        /// <param name="id">id value</param>
        /// <param name="lockMode">lock mode</param>
        /// <returns>entity with specified id.
        /// NULL - if not exists
        /// </returns>
        TImpl Get(ID id, LockMode lockMode);

        /// <summary> Get entity by id async
        /// </summary>
        /// <param name="id">id value</param>
        /// <param name="lockMode">lock mode</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>entity with specified id.
        /// NULL - if not exists
        /// </returns>
        Task<TImpl> GetAsync(ID id, LockMode lockMode, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary> Get entity by id using lazy-loading.
        /// </summary>
        /// <param name="id">id value</param>
        /// <returns>proxy object of entity with specified id.
        /// NULL - throw exception
        /// </returns>
        //TODO: choose exception type
        TImpl Load(ID id);

        /// <summary> Get entity by id using lazy-loading.
        /// </summary>
        /// <param name="id">id value</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>proxy object of entity with specified id.
        /// NULL - throw exception
        /// </returns>
        //TODO: choose exception type
        Task<TImpl> LoadAsync(ID id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary> Get entity by id using lazy-loading.
        /// </summary>
        /// <param name="id">id value</param>
        /// <param name="lockMode">lock mode</param>
        /// <returns>proxy object of entity with specified id.
        /// NULL - throw exception
        /// </returns>
        //TODO: choose exception type
        TImpl Load(ID id, LockMode lockMode);

        /// <summary> Get entity by id using lazy-loading async.
        /// </summary>
        /// <param name="id">id value</param>
        /// <param name="lockMode">lock mode</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>proxy object of entity with specified id.
        /// NULL - throw exception
        /// </returns>
        //TODO: choose exception type
        Task<TImpl> LoadAsync(ID id, LockMode lockMode, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary> Refresh specified entity by re-read it from DB
        /// </summary>
        /// <param name="entity">entity</param>
        void Refresh(T entity);

        /// <summary> Refresh specified entity by re-read it from DB async
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns></returns>
        Task RefreshAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary> Refresh specified entity by re-read it from DB
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="lockMode">lock mode</param>
        void Refresh(T entity, LockMode lockMode);

        /// <summary> Refresh specified entity by re-read it from DB async
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="lockMode">lock mode</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns></returns>
        Task RefreshAsync(T entity, LockMode lockMode, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary> Remove specified entity from cache
        /// </summary>
        /// <param name="entity">entity</param>
        void Evict(T entity);

        /// <summary> Remove specified entity from cache async
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns></returns>
        Task EvictAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary> Get list of entities
        /// </summary>
        /// <param name="top">max results</param>
        /// <returns>list of entities</returns>
        System.Collections.Generic.IList<TImpl> List(int top);

        /// <summary> Get list of entities async
		/// </summary>
		/// <param name="top">max results</param>
        /// <param name="cancellationToken">cancellation token</param>
		/// <returns>list of entities</returns>
		Task<System.Collections.Generic.IList<TImpl>> ListAsync(int top, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary> Get list of all entities
        /// </summary>
        /// <returns>list of all entities</returns>
        System.Collections.Generic.IList<TImpl> GetAll();

        /// <summary> Get list of all entities async
        /// </summary>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>list of all entities</returns>
        Task<System.Collections.Generic.IList<TImpl>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary> Returns <see cref="System.Linq.IQueryable{T}"/> object
        /// </summary>
        /// <returns><see cref="System.Linq.IQueryable{T}"/> object for type of T</returns>
        System.Linq.IQueryable<T> Query();
	}
}
