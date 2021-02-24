using System.Data;

namespace Itb.DalCore.UnitOfWork {
	/// <summary> Unit of work factory interface
	/// </summary>
	public interface IUnitOfWorkFactory {
		/// <summary> Create UoW
		/// </summary>
		/// <param name="isolationLevel">transaction isolation level</param>
		/// <returns>Unit of work</returns>
		IUnitOfWork Create(IsolationLevel isolationLevel);
		/// <summary> Create UoW
		/// </summary>
		/// <returns>Unit of work</returns>
		IUnitOfWork Create();
	}
}
