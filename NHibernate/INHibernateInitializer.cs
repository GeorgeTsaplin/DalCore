using NHibernate.Cfg;

namespace Itb.DalCore.NHibernate {
	///<summary>
	/// Bootstrapper for NHibernate
	///</summary>
	public interface INHibernateInitializer {
		///<summary> Builds and returns NHibernate configuration
		///</summary>
		///<returns>NHibernate configuration object</returns>
		Configuration GetConfiguration();
	}
}