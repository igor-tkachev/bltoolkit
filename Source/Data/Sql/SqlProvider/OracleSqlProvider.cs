using System;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class OracleSqlProvider : BasicSqlProvider
	{
		public OracleSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		protected override void BuildSelectClause(StringBuilder sb)
		{
			if (SqlBuilder.From.Tables.Count == 0)
			{
				AppendIndent(sb);
				sb.Append("SELECT").AppendLine();
				BuildColumns(sb);
				AppendIndent(sb);
				sb.Append("FROM SYS.DUAL").AppendLine();
			}
			else
				base.BuildSelectClause(sb);
		}

		protected override void BuildBinaryExpression(StringBuilder sb, SqlBinaryExpression expr)
		{
			switch (expr.Operation[0])
			{
				case '%': BuildFunction(sb, "MOD",    expr);    break;
				case '&': BuildFunction(sb, "BITAND", expr);    break;
				case '|':
					sb.Append('(');
					BuildExpression(sb, expr.Expr1);
					sb.Append(" + ");
					BuildExpression(sb, expr.Expr2);
					sb.Append(") - ");
					BuildFunction(sb, "BITAND", expr);
					break;

				case '^':
					sb.Append('(');
					BuildExpression(sb, expr.Expr1);
					sb.Append(" + ");
					BuildExpression(sb, expr.Expr2);
					sb.Append(") - ");
					BuildFunction(sb, "BITAND", expr);
					sb.Append(" * 2");
					break;

				default : base.BuildBinaryExpression(sb, expr); break;
			}
		}


		protected override int GetPrecedence(ISqlExpression expr)
		{
			if (expr is SqlBinaryExpression)
			{
				switch (((SqlBinaryExpression)expr).Operation[0])
				{
					case '%':
					case '&': return Precedence.Primary;
					case '|':
					case '^': return Precedence.Additive - 1;
				}
			}

			return base.GetPrecedence(expr);
		}
	}
}
