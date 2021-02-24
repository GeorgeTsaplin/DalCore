using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Itb.DalCore.NHibernate.Tests
{
#pragma warning disable 1591
	[Explicit]
	public class DataSourceExceptionViaQueryTranslatorTests : Itb.Tests.TestFixtureBase
	{
		protected override Itb.Tests.TestFixtureBase.MappingsFinderTypes MappingsFinderType
		{
			get { return MappingsFinderTypes.All; }
		}

		[TestFixtureSetUp]
		public override void SetUp()
		{
			base.SetUp();
		}
		[TestFixtureTearDown]
		public override void TearDown()
		{
			base.TearDown();
		}

		private class MyDbException : System.Data.Common.DbException
		{
			public MyDbException(string message) : base(message) { }
		}

		[TestCase(@"The INSERT statement conflicted with the FOREIGN KEY constraint 'FK_Channel_Recorder'. The conflict occurred in database 'Recording30_test1', table 'dbo.Recorder', column 'ID'."
			, "Can not delete recorder because there are channels on it!")]
		public void TranslateException(string exceptionMsg, string expected)
		{
			var translator = new Itb.DalCore.NHibernate.Exceptions.DataSourceExceptionViaQueryTranslator(SessionProvider, "ProcessErrorMessage");
			var exception = new Exception("wrapper exception", new MyDbException(exceptionMsg));
			var translatedEx = translator.Convert(new global::NHibernate.Exceptions.AdoExceptionContextInfo()
				{
					SqlException = exception
				}
			);
			Assert.AreEqual(expected, translatedEx.Message);
		}
		[TestCase(@"The INSERT statement conflicted with the FOREIGN KEY constraint 'FK_Channel_Recorder'. The conflict occurred in database 'Recording30_test1', table 'dbo.Recorder', column 'ID'."
			, "Could not find stored procedure 'dbo.__some_missing_object'.")]
		public void TranslateExceptionWithMissingDBObject(string exceptionMsg, string expected)
		{
			var translator = new Itb.DalCore.NHibernate.Exceptions.DataSourceExceptionViaQueryTranslator(SessionProvider, "MissingObject");
			var exception = new Exception("wrapper exception", new MyDbException(exceptionMsg));
			var translatedEx = translator.Convert(new global::NHibernate.Exceptions.AdoExceptionContextInfo()
				{
					SqlException = exception
				}
			);
			Assert.AreEqual(expected, translatedEx.Message);
		}
	}
#pragma warning restore 1591

}
