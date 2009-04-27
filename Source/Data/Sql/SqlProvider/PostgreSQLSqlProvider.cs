using System;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class PostgreSQLSqlProvider : BasicSqlProvider
	{
		public PostgreSQLSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

				switch (be.Operation)
				{
					case "^": return new SqlBinaryExpression(be.Expr1, "#", be.Expr2, be.Type);
					case "+": return be.Type == typeof(string)? new SqlBinaryExpression(be.Expr1, "||", be.Expr2, be.Type, be.Precedence): expr;
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "Left"     : return new SqlFunction("Substring", func.Parameters[0], new SqlValue(1), func.Parameters[1]);
					case "CharIndex":
						return func.Parameters.Length == 2?
							new SqlExpression("Position({0} in {1})", Precedence.Primary, func.Parameters[0], func.Parameters[1]):
							Add<int>(
								new SqlExpression("Position({0} in {1})", Precedence.Primary, func.Parameters[0],
									new SqlFunction("Substring",
										func.Parameters[1],
										func.Parameters[2],
										Sub<int>(new SqlFunction("Length", func.Parameters[1]), func.Parameters[2]))),
								Sub(func.Parameters[2], 1));
				}
			}

			return base.ConvertExpression(expr);
		}
	}
}
