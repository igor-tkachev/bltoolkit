using System;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	public class MsSql2012SqlProvider : MsSqlSqlProvider
	{
		protected override string LimitFormat         { get { return SqlQuery.Select.SkipValue != null ? "FETCH NEXT {0} ROWS ONLY" : null; } }
		protected override string OffsetFormat        { get { return "OFFSET {0} ROWS"; } }
		protected override bool   OffsetFirst         { get { return true;              } }
		protected override bool   BuildAlternativeSql { get { return false;             } }

		protected override ISqlProvider CreateSqlProvider()
		{
			return new MsSql2012SqlProvider();
		}

		protected override void BuildInsertOrUpdateQuery(StringBuilder sb)
		{
			BuildInsertOrUpdateQueryAsMerge(sb, null);
			sb.AppendLine(";");
		}

		protected override void BuildSql(StringBuilder sb)
		{
			if (NeedSkip && SqlQuery.OrderBy.IsEmpty)
			{
				for (var i = 0; i < SqlQuery.Select.Columns.Count; i++)
					SqlQuery.OrderBy.ExprAsc(new SqlValue(i + 1));
			}

			base.BuildSql(sb);
		}
	}
}
