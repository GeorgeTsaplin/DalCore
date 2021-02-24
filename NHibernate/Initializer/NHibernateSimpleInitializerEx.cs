using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Itb.DalCore.NHibernate
{
	/// <summary> NHibernate simple initializer
	/// </summary>
	/// <remarks>
	/// Initialize NHibernate configuration by specifing connection string and command timeout
	/// </remarks>
	public class NHibernateSimpleInitializerEx : NHibernateSimpleInitializer
	{
		string _connectionString;
		int _commandTimeout; 

		/// <summary> Creates new NHibernate initializer
		/// </summary>
		/// <param name="connectionString">connection string</param>
		/// <param name="commandTimeout">command timeout</param>
		public NHibernateSimpleInitializerEx(string connectionString, int commandTimeout)
		{
			_connectionString=connectionString;
			_commandTimeout = commandTimeout;
		}
		/// <summary> Reconfigures NHibernate config before it has been used by specifying connection string and command timeout
		/// </summary>
		/// <param name="config">NHibernate configuration</param>
		protected override void Reconfigure(ref global::NHibernate.Cfg.Configuration config)
		{
			config.SetProperty("connection.connection_string", _connectionString);
			config.SetProperty("command_timeout", _commandTimeout.ToString());
		}
	}
}
