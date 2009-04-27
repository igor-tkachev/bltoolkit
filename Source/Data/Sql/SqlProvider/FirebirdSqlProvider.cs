using System;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class FirebirdSqlProvider : BasicSqlProvider
	{
		public FirebirdSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
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
				sb.Append("FROM rdb$database").AppendLine();
			}
			else
				base.BuildSelectClause(sb);
		}

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

				switch (be.Operation)
				{
					case "%": return new SqlFunction("Mod",     be.Expr1, be.Expr2);
					case "&": return new SqlFunction("Bin_And", be.Expr1, be.Expr2);
					case "|": return new SqlFunction("Bin_Or",  be.Expr1, be.Expr2);
					case "^": return new SqlFunction("Bin_Xor", be.Expr1, be.Expr2);
					case "+": return be.Type == typeof(string)? new SqlBinaryExpression(be.Expr1, "||", be.Expr2, be.Type, be.Precedence): expr;
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction)expr;

				switch (func.Name)
				{
					case "Length":    return new SqlFunction  ("Char_Length", func.Parameters);
					case "Substring": return new SqlExpression("Substring({0} from {1} for {2})", Precedence.Primary, func.Parameters);
				}
			}

			return expr;
		}
	}
}
