using System;

namespace BLToolkit.Data.SqlBuilder.SqlProvider
{
	public interface ISqlProvider
	{
		string BuildSql(Sql sqlBuilder);
	}
}
