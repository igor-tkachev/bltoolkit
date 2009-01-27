using System;

namespace BLToolkit.Data.Sql.SqlProvider
{
	public interface ISqlProvider
	{
		string BuildSql(SqlBuilder sqlBuilder);
	}
}
