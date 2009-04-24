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
						return Sub(Add(be.Expr1, be.Expr2), new SqlFunction("BITAND", be.Expr1, be.Expr2));

					case '^': // (a + b) - BITAND(a, b) * 2
						return Sub(Add(be.Expr1, be.Expr2), Mul(new SqlFunction("BITAND", be.Expr1, be.Expr2), 2));
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "Coalesce"  : return new SqlFunction("Nvl",    func.Parameters);
					case "Substring" : return new SqlFunction("Substr", func.Parameters);
					case "CharIndex" :
						return func.Parameters.Length == 2?
							new SqlFunction("InStr", func.Parameters[1], func.Parameters[0]):
							new SqlFunction("InStr", func.Parameters[1], func.Parameters[0], func.Parameters[2]);
				}
			}

			return base.ConvertExpression(expr);
		}
	}
}
