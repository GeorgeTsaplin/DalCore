using System;

namespace Itb.DalCore.Exceptions
{
    /// <summary> Data source exception
    /// </summary>
    [Serializable]
	public class DataSourceException : ApplicationException
	{
		/// <summary> Creates new data source exception
		/// </summary>
		/// <param name="message">exception message</param>
		/// <param name="innerException">inner exception</param>
		public DataSourceException(string message, Exception innerException) : base(message, innerException) { }
	}
}
