using System;
using System.Data;
using Itb.DalCore.UnitOfWork;

namespace Itb.DalCore.NHibernate.UnitOfWork
{
    /// <summary> NHibernate unit of work factory
    /// </summary>
    public sealed class NHibernateUnitOfWorkFactory : IUnitOfWorkFactory {

		readonly ISessionProvider _sessionProvider;
		/// <summary> Creates new Unit of Work factory
		/// </summary>
		/// <param name="sessionProvider">session provider</param>
		public NHibernateUnitOfWorkFactory(ISessionProvider sessionProvider)
		{
			if(null == sessionProvider)
			{
				throw new ArgumentNullException("sessionProvider");
			}
			_sessionProvider = sessionProvider;
		}

#pragma warning disable 1591

		public IUnitOfWork Create(IsolationLevel isolationLevel) {
			global::NHibernate.ISession session = _sessionProvider.GetSessionForUoW();
			return new NHibernateUnitOfWork(session, isolationLevel);
		}

		public IUnitOfWork Create() {
			return Create(IsolationLevel.ReadCommitted);
		}

#pragma warning restore 1591
	}
}