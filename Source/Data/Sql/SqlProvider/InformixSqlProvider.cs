using System;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class InformixSqlProvider : BasicSqlProvider
	{
		public InformixSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		protected override void BuildSelectClause(StringBuilder sb)
		{
			if (SqlBuilder.From.Tables.Count == 0)
			{
				AppendIndent(sb);
				sb.Append("SELECT FIRST 1").AppendLine();
				BuildColumns(sb);
				AppendIndent(sb);
				sb.Append("FROM SYSTABLES").AppendLine();
			}
			else
				base.BuildSelectClause(sb);
		}

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

				switch (be.Operation[0])
				{
					case '%': return new SqlFunction("MOD",    be.Expr1, be.Expr2);
					case '&': return new SqlFunction("BITAND", be.Expr1, be.Expr2);
					case '|': return new SqlFunction("BITOR",  be.Expr1, be.Expr2);
					case '^': return new SqlFunction("BITXOR", be.Expr1, be.Expr2);
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "COALESCE":         return new SqlFunction("NVL",    func.Parameters);
					case "CHARACTER_LENGTH": return new SqlFunction("LENGTH", func.Parameters);
				}
			}

			return base.ConvertExpression(expr);
		}

		protected override void BuildLikePredicate(StringBuilder sb, SqlBuilder.Predicate.Like predicate)
		{
			if (predicate.IsNot)
				sb.Append("NOT ");

			int precedence = GetPrecedence(predicate);

			BuildExpression(sb, precedence, predicate.Expr1);
			sb.Append(" LIKE ");
			BuildExpression(sb, precedence, predicate.Expr2);

			if (predicate.Escape != null)
			{
				sb.Append(" ESCAPE '");
				BuildExpression(sb, precedence, predicate.Escape);
				sb.Append("'");
			}
		}
	}
}
