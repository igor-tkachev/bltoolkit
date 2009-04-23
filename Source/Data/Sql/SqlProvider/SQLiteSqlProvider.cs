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
						return new SqlBinaryExpression(
							new SqlBinaryExpression(be.Expr1, "+", be.Expr2, Precedence.Additive),
							"-",
							new SqlBinaryExpression(
								new SqlBinaryExpression(be.Expr1, "&", be.Expr2),
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
					case "CHARACTER_LENGTH": return new SqlFunction("LENGTH", func.Parameters);
					case "IndexOf":
						return new SqlBinaryExpression(
							func.Parameters.Length == 2?
								new SqlFunction("CHARINDEX", func.Parameters[1], func.Parameters[0]):
								new SqlFunction("CHARINDEX",
									func.Parameters[1],
									func.Parameters[0],
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
