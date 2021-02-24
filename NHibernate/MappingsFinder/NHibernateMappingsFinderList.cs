using System;
using System.Collections.Generic;

namespace Itb.DalCore.NHibernate.Mappings {
	/// <summary> NHibernate mappings finder list
	/// </summary>
	public class NHibernateMappingsFinderList : List<INHibernateMappingsFinder>, INHibernateMappingsFinderEnumerator {
		/// <summary> Create NHibernate mappings finder factory
		/// </summary>
		public NHibernateMappingsFinderList() : base() { }
		/// <summary> Create NHibernate mappings finder factory
		/// </summary>
		/// <param name="collection">collection to add in list</param>
		public NHibernateMappingsFinderList(IEnumerable<INHibernateMappingsFinder> collection) : base(collection) { }
	}
}