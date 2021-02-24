using System.Threading;
using System.Threading.Tasks;

namespace Itb.DalCore
{
    /// <summary> Repository interface (write operations)
    /// </summary>
    /// <typeparam name="T">entity interface</typeparam>
    /// <typeparam name="TImpl">entity data type</typeparam>
    /// <typeparam name="ID">entity ID data type</typeparam>
    public interface IRepositoryWrite<T, TImpl, ID> : IRepository where TImpl : T where T : Domain.IPrimaryKey<ID>
    {
		/// <summary> Save entity
		/// </summary>
		/// <param name="entity">entity for saving</param>
		void Save(T entity);

        /// <summary> Save entity async
		/// </summary>
		/// <param name="entity">entity for saving</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns></returns>
		Task SaveAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary> Force changes to flush to DB
		/// </summary>
        void Flush();

        /// <summary> Force changes to flush to DB async
		/// </summary>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns></returns>
        Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary> Delete entity
        /// </summary>
        /// <param name="entity">entity for delete</param>
        void Delete(T entity);

        /// <summary> Delete entity async
        /// </summary>
        /// <param name="entity">entity for delete</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns></returns>
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary> Lock specified entity
        /// </summary>
        /// <param name="entity">entity for locking</param>
        /// <param name="lockMode">lock mode</param>
        void Lock(T entity, LockMode lockMode);

        /// <summary> Lock specified entity async
        /// </summary>
        /// <param name="entity">entity for locking</param>
        /// <param name="lockMode">lock mode</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns></returns>
        Task LockAsync(T entity, LockMode lockMode, CancellationToken cancellationToken = default(CancellationToken));
    }
}
