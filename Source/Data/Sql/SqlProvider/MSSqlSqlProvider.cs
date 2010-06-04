using System;
using System.Collections.Generic;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;
	using Reflection;

	public abstract class MsSqlSqlProvider : BasicSqlProvider
	{
		protected MsSqlSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
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

		protected override void BuildGetIdentity(StringBuilder sb)
		{
			sb
				.AppendLine()
				.AppendLine("SELECT SCOPE_IDENTITY()");
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

			switch (expr.ElementType)
			{
				case QueryElementType.SqlBinaryExpression:
					{
						var be = (SqlBinaryExpression)expr;

						switch (be.Operation)
						{
							case "%":
								{
									var type1 = TypeHelper.GetUnderlyingType(be.Expr1.SystemType);

									if (type1 == typeof(double) || type1 == typeof(float))
									{
										return new SqlBinaryExpression(
											be.Expr2.SystemType,
											new SqlFunction(typeof(int), "Convert", SqlDataType.Int32, be.Expr1),
											be.Operation,
											be.Expr2);
									}

									break;
								}
						}

						break;
					}

				case QueryElementType.SqlFunction:
					{
						var func = (SqlFunction)expr;

						switch (func.Name)
						{
							case "Convert" :
								{
									if (TypeHelper.GetUnderlyingType(func.SystemType) == typeof(ulong) &&
										TypeHelper.IsFloatType(func.Parameters[1].SystemType))
										return new SqlFunction(
											func.SystemType,
											func.Name,
											func.Precedence,
											func.Parameters[0],
											new SqlFunction(func.SystemType, "Floor", func.Parameters[1]));

									break;
								}
						}

						break;
					}
			}

			return expr;
		}

		protected override void BuildDeleteClause(StringBuilder sb)
		{
			AppendIndent(sb)
				.Append("DELETE ")
				.Append(DataProvider.Convert(GetTableAlias(SqlQuery.From.Tables[0]), ConvertType.NameToQueryTableAlias))
				.AppendLine();
		}

		protected override void BuildUpdateTableName(StringBuilder sb)
		{
			sb.Append(DataProvider.Convert(GetTableAlias(SqlQuery.From.Tables[0]), ConvertType.NameToQueryTableAlias));
		}

		protected override void BuildUnicodeString(StringBuilder sb, string value)
		{
			sb
				.Append("N\'")
				.Append(value.Replace("'", "''"))
				.Append('\'');
		}
	}
}
