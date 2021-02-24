using System;

namespace Itb.DalCore.Exceptions
{
    /// <summary> Thrown then object has stale data. Used with versioning feature.
    /// </summary>
    [Serializable]
	public class StaleObjectException : ApplicationException
	{
		/// <summary> Object name (get)
		/// </summary>
		public string ObjectName { get; private set; }
		/// <summary> Object identifier (get)
		/// </summary>
		public object Identifier { get; private set; }

		/// <summary> Creates new stale object exception
		/// </summary>
		/// <param name="objectName">object name</param>
		/// <param name="identifier">object identifier</param>
		/// <param name="message">exception message</param>
        /// <param name="innerException">inner exception</param>
		public StaleObjectException(string objectName, object identifier, string message, Exception innerException) : base(message, innerException)
		{
			this.ObjectName = objectName;
			this.Identifier = identifier;
		}

        /// <inheritdoc/>
        public override string ToString()
		{
			return string.Format(@"ObjectName = '{1}' (Identifier = '{2}')
{0}"
				, base.ToString(), this.ObjectName, this.Identifier
			);
		}
	}
}
