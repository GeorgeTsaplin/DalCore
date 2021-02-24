
namespace Itb.DalCore.NHibernate.Mappings {
	/// <summary> NHibernate mappings finder interface
	/// </summary>
	public interface INHibernateMappingsFinder {
		/// <summary> Add mappings to HNibernate configuration
		/// </summary>
		/// <param name="config">NHibernate configuration</param>
		void AddMappings(ref global::NHibernate.Cfg.Configuration config);
	}
}
