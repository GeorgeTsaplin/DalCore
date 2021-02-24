using NUnit.Framework;
using System;
using Itb.DalCore.Domain;
using Itb.DalCore.NHibernate.UnitOfWork;
using Itb.DalCore.NHibernate.Mappings;
using System.Collections.Generic;
using Spring.Context.Support;

namespace Itb.DalCore.NHibernate.Tests {
#pragma warning disable 1591
	[TestFixture]
	[Explicit]
	public class BaseRepositoryTests : Itb.Tests.TestFixtureBase {
		protected override Itb.Tests.TestFixtureBase.MappingsFinderTypes MappingsFinderType {
			get { return MappingsFinderTypes.All; }
		}

		//private interface IStubRepo : IRepository {
		//	DateTime GetDate();
		//}
		private class ObjectRepo : BaseRepository<Object, Object, int> {
			public ObjectRepo(ISessionProvider session) : base(session) { }

			public new DateTime GetDate() {
				return base.GetDate();
			}
		}
		private ObjectRepo _objectRepo;
		[TestFixtureSetUp]
		public override void SetUp() {
			base.SetUp();
			_objectRepo = new ObjectRepo(SessionProvider);
		}
		[TestFixtureTearDown]
		public override void TearDown() {
			base.TearDown();
			_objectRepo = null;
		}

		[Test]
		//[Ignore("Turn on after changes had made")]
		public void GetDateTest() {
			DateTime actual = DateTime.MinValue;
			DateTime expected = DateTime.Now;
			Assert.DoesNotThrow(new TestDelegate(() => actual = _objectRepo.GetDate()));
			using(var uow = this.UnitOfWorkFactory.Create()) {
				Assert.DoesNotThrow(new TestDelegate(() => actual = _objectRepo.GetDate()));
			}
			Assert.AreEqual(expected.Date, actual.Date);
		}

		[Test]
		public void GetPropertyToColumnMapping()
		{
			var actual = _objectRepo.GetPropertyToColumnMapping();
			System.Collections.Specialized.StringDictionary expected = new System.Collections.Specialized.StringDictionary();
			expected.Add("Id", "id");
			expected.Add("Name", "name");

			NUnit.Framework.CollectionAssert.AreEqual(expected, actual);
		}
	}
#pragma warning restore 1591
}
