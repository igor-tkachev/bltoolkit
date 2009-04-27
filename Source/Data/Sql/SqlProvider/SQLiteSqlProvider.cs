using System;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class SQLiteSqlProvider : BasicSqlProvider
	{
		public SQLiteSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

				switch (be.Operation)
				{
					case "+": return be.Type == typeof(string)? new SqlBinaryExpression(be.Expr1, "||", be.Expr2, be.Type, be.Precedence): expr;
					case "^": // (a + b) - (a & b) * 2
						return Sub(
							Add(be.Expr1, be.Expr2, be.Type),
							Mul(new SqlBinaryExpression(be.Expr1, "&", be.Expr2, be.Type), 2), be.Type);
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "Substring" : return new SqlFunction("Substr", func.Parameters);
					case "Left"      : return BuildComplexLeft (func);
					case "Right"     : return BuildComplexRight(func);
				}
			}

			return expr;
		}
	}
}
