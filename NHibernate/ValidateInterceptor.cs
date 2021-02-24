using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Itb.DalCore.NHibernate {
	/// <summary> Validate interceptor.
	/// Call Validate on <see cref="Itb.Common.IValidatable"/> entities before save
	/// </summary>
	public class ValidateInterceptor : global::NHibernate.EmptyInterceptor {
#pragma warning disable 1591

		public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, global::NHibernate.Type.IType[] types) {
			bool res = false;
			if(entity is Itb.Common.IValidatable) {
				((Itb.Common.IValidatable)entity).Validate();
				res = true;
			}
			return res;
		}
		public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, global::NHibernate.Type.IType[] types) {
			bool res = false;
			if(entity is Itb.Common.IValidatable) {
				((Itb.Common.IValidatable)entity).Validate();
				res = true;
			}
			return res;
		}

#pragma warning restore 1591
	}
}
