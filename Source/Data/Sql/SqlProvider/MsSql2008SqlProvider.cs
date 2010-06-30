using System;

namespace BLToolkit.Data.Sql.SqlProvider
{
	public class MsSql2008SqlProvider : MsSqlSqlProvider
	{
		protected override ISqlProvider CreateSqlProvider()
		{
			return new MsSql2008SqlProvider();
		}
	}
}
