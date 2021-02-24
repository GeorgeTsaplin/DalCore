using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Itb.DalCore.NHibernate.Exceptions
{
	/// <summary> Data source exception translator via using DB query with one required parameter - [Original exception message]
	/// </summary>
	public class DataSourceExceptionViaQueryTranslator : global::NHibernate.Exceptions.ISQLExceptionConverter
	{
		readonly string _queryName;
		readonly ISessionProvider _sessionProvider;
		/// <summary> Creates data source via query translator
		/// </summary>
		/// <param name="sessionProvider">session provider</param>
		/// <param name="queryName">NHibernate query name</param>
		public DataSourceExceptionViaQueryTranslator(ISessionProvider sessionProvider, string queryName)
		{
			if(null == sessionProvider)
			{
				throw new ArgumentNullException("sessionProvider");
			}
			if(string.IsNullOrEmpty(queryName))
			{
				throw new ArgumentNullException("queryName");
			}
			_sessionProvider = sessionProvider;
			_queryName = queryName;
		}
#pragma warning disable 1591

		DalCore.Exceptions.DataSourceException Translate(Exception exception)
		{
			if(null == exception)
			{
				return null;
			}

			var mostInnerException = exception;

			var q = _sessionProvider.CurrentSession.GetNamedQuery(_queryName);
			if(null == q)
			{
				throw new NotImplementedException(string.Format("Specified query does not exist - '{0}'", _queryName));
			}

			q.SetParameter(0, mostInnerException.Message);
			try
			{
				q.UniqueResult();
			} catch(global::NHibernate.ADOException e)
			{
				mostInnerException = global::NHibernate.Exceptions.ADOExceptionHelper.ExtractDbException(e);
			}
			return new DalCore.Exceptions.DataSourceException(mostInnerException.Message, exception);
		}

        /*static Exception GetMostInnerException(Exception exception)
		{
			Exception mostInnerException = exception;
			while(null != mostInnerException.InnerException)
			{
				mostInnerException = mostInnerException.InnerException;
			}
			return mostInnerException;
		}*/

#pragma warning restore 1591

        #region ISQLExceptionConverter Members

        /// <summary>
        /// Returns translated ADO exception
        /// </summary>
        /// <param name="adoExceptionContextInfo">ADO exception to convert</param>
        /// <returns>translate ADO exception</returns>
        public Exception Convert(global::NHibernate.Exceptions.AdoExceptionContextInfo adoExceptionContextInfo)
		{
			var exception = global::NHibernate.Exceptions.ADOExceptionHelper.ExtractDbException(adoExceptionContextInfo.SqlException);
			return Translate(exception);
		}

		#endregion
	}
}
