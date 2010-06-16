using System;

namespace BLToolkit.Data.Linq
{
	using Data.Sql.SqlProvider;

	public interface ILinqDataProvider
	{
		ISqlProvider CreateSqlProvider();
		IDataContext CreateDataContext();
	}
}
