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

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

				switch (be.Operation[0])
				{
					case '%': return new SqlFunction("MOD",    be.Expr1, be.Expr2);
					case '&': return new SqlFunction("BITAND", be.Expr1, be.Expr2);
					case '|': // (a + b) - BITAND(a, b)
						return new SqlBinaryExpression(
							new SqlBinaryExpression(be.Expr1, "+", be.Expr2),
							"-",
							new SqlFunction("BITAND", be.Expr1, be.Expr2),
							Precedence.Subtraction);

					case '^': // (a + b) - BITAND(a, b) * 2
						return new SqlBinaryExpression(
							new SqlBinaryExpression(be.Expr1, "+", be.Expr2),
							"-",
							new SqlBinaryExpression(
								new SqlFunction("BITAND", be.Expr1, be.Expr2),
								"*",
								new SqlValue(2)),
							Precedence.Subtraction);
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "COALESCE"        : return new SqlFunction("NVL",    func.Parameters);
					case "CHARACTER_LENGTH": return new SqlFunction("LENGTH", func.Parameters);
					case "IndexOf":
						return new SqlBinaryExpression(
							func.Parameters.Length == 2?
								new SqlFunction("INSTR", func.Parameters[0], func.Parameters[1]):
								new SqlFunction("INSTR",
									func.Parameters[0],
									func.Parameters[1],
									new SqlBinaryExpression(func.Parameters[2], "+", new SqlValue(1), Precedence.Additive)),
							"-",
							new SqlValue(1),
							Precedence.Subtraction);
				}
			}

			return base.ConvertExpression(expr);
		}
	}
}
