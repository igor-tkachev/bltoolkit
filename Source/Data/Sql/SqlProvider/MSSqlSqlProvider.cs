using System;
using System.Collections.Generic;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public abstract class MsSqlSqlProvider : BasicSqlProvider
	{
		public MsSqlSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		protected override string FirstFormat
		{
			get { return SqlQuery.Select.SkipValue == null ? "TOP ({0})" : null; }
		}

		protected override void BuildSql(StringBuilder sb)
		{
			AlternativeBuildSql(sb, true, base.BuildSql);
		}

		protected override void BuildOrderByClause(StringBuilder sb)
		{
			if (!NeedSkip)
				base.BuildOrderByClause(sb);
		}

		protected override IEnumerable<SqlQuery.Column> GetSelectedColumns()
		{
			if (NeedSkip && !SqlQuery.OrderBy.IsEmpty)
				return AlternativeGetSelectedColumns(base.GetSelectedColumns);
			return base.GetSelectedColumns();
		}

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				/*
				switch (func.Name)
				{
					case "Length" : return new SqlFunction("Len", func.Parameters);
				}
				*/
			}

			return expr;
		}
	}
}
