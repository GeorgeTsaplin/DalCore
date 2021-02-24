
namespace Itb.DalCore.NHibernate {
	/// <summary> NHibernate simple initializer
	/// </summary>
	/// <remarks>
	/// Initialize NHIbernate configuration by App.config
	/// </remarks>
	public class NHibernateSimpleInitializer : INHibernateInitializer {
		private global::NHibernate.Cfg.Configuration _config = null;
		/// <summary> Creates new NHibernate simple initializer
		/// </summary>
		public NHibernateSimpleInitializer()
		{
		}
#pragma warning disable 1591
		public global::NHibernate.Cfg.Configuration GetConfiguration() {
			if(null == _config) {
				_config = new global::NHibernate.Cfg.Configuration();
				Reconfigure(ref _config);
			}
			  
			return _config;
		}
#pragma warning restore 1591

		/// <summary> Reconfigures NHibernate config before it has been used
		/// </summary>
		/// <param name="config">NHibernate configuration</param>
		protected virtual void Reconfigure(ref global::NHibernate.Cfg.Configuration config)
		{

		}

	}
}
