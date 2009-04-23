using System;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class SqlCeSqlProvider : BasicSqlProvider
	{
		public SqlCeSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "CHARACTER_LENGTH": return new SqlFunction("LEN", func.Parameters);
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
