using System;

namespace Itb.DalCore {
	/// <summary> Cache levels
	/// </summary>
	[Flags]
	public enum CacheLevels : byte {
		/// <summary> without cache
		/// </summary>
		None=0,
		/// <summary> ORM cache кэш уровня ORM
		/// </summary>
		ORM=1,
		/// <summary> business logic cache
		/// </summary>
		BusinessLayer=2,
		/// <summary> business logic static cache
		/// </summary>
		BusinessLayerStatic=4,
	}
}