using NHibernate;
using System;

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
	 }
}