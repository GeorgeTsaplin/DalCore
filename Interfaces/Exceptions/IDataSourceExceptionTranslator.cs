using System;

namespace Itb.DalCore.Exceptions
{
    //HACK: used global::NHibernate.Exceptions.ISQLExceptionConverter instead
    public interface IDataSourceExceptionTranslator
	{
		DataSourceException Translate(Exception exception);
	}
}
