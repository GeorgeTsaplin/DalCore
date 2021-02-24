using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Itb.DalCore.Domain;

namespace Itb.DalCore.NHibernate.Tests
{
	internal class Object : IPrimaryKey<int>
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }

		#region IPrimaryKey<int> Members

		public virtual int __Pk
		{
			get { return Id; }
		}

		#endregion
	}
}
