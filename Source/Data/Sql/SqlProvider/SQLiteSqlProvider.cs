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
			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

				switch (be.Operation[0])
				{
					case '^': // (a + b) - (a & b) * 2
						return Sub(Add(be.Expr1, be.Expr2), Mul(new SqlBinaryExpression(be.Expr1, "&", be.Expr2), 2));
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "Substring" : return new SqlFunction("Substr", func.Parameters);
				}
			}

			return base.ConvertExpression(expr);
		}
	}
}
