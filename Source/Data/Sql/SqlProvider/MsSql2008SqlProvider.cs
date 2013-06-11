using System;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	public class MsSql2008SqlProvider : MsSqlSqlProvider
	{
		protected override ISqlProvider CreateSqlProvider()
		{
			return new MsSql2008SqlProvider();
		}

		protected override void BuildFunction(StringBuilder sb, SqlFunction func)
		{
			func = ConvertFunctionParameters(func);
			base.BuildFunction(sb, func);
		}

		protected override void BuildInsertOrUpdateQuery(StringBuilder sb)
		{
			BuildInsertOrUpdateQueryAsMerge(sb, null);
			sb.AppendLine(";");
		}
	}
}
