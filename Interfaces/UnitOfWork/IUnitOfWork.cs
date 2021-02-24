using System;
using System.Threading;
using System.Threading.Tasks;

namespace Itb.DalCore.UnitOfWork {
	/// <summary> Unit of work (UoW) interface
	/// If user don't execute <see cref="IUnitOfWork.Commit"/> method, then transaction must be rollback
	/// </summary>
	public interface IUnitOfWork : IDisposable {
		/// <summary> Commit changes
		/// </summary>
		void Commit();

        /// <summary>
        /// Commit changes async
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
	}
}
