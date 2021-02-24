namespace Itb.DalCore {
	/// <summary> Entity lock mode
	/// </summary>
	public enum LockMode {
		/// <summary> No lock required 
		/// </summary>
		/// <remarks>
		/// If an object is requested with this lock mode, a Read lock might be obtained if necessary.
		/// </remarks>
		None,
		/// <summary> A shared lock
		/// </summary>
		/// <remarks>
		/// Objects are loaded in Read mode by default
		/// </remarks>
		Read,
		/// <summary> An upgrade lock
		/// </summary>
		/// <remarks>
		/// Objects loaded in this lock mode are materialized using an SQL SELECT ... FOR UPDATE
		/// </remarks>
		Upgrade,
	 	/// <summary> Similar to <see cref="LockMode.Upgrade"/> except that, for versioned entities,
		/// it results in a forced version increment
		/// </summary>
		Force,
		/// <summary> Attempt to obtain an upgrade lock, using an Oracle-style SELECT ... FOR UPGRADE NOWAIT
		/// </summary>
		/// <remarks> 
		/// The semantics of this lock mode, once obtained, are the same as Upgrade
		/// </remarks>
		UpgradeNoWait,
		/// <summary> A Write lock is obtained when an object is updated or inserted
		/// </summary>
		/// <remarks>
		/// This is not a valid mode for Load() or Lock()
		/// </remarks>
		Write
	}
}