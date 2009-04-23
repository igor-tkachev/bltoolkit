using System;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class SybaseSqlProvider : BasicSqlProvider
	{
		public SybaseSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
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
						if (func.Parameters.Length == 2)
							return new SqlBinaryExpression(
								new SqlFunction("CHARINDEX", func.Parameters[1], func.Parameters[0]),
								"-",
								new SqlValue(1),
								Precedence.Subtraction);

						var n = new SqlBinaryExpression(func.Parameters[2], "+", new SqlValue(1), Precedence.Additive);

						return new SqlFunction("CHARINDEX",
							func.Parameters[1],
							new SqlFunction("SUBSTRING",
								func.Parameters[0],
								n,
								new SqlBinaryExpression(new SqlFunction("LEN", func.Parameters[0]), "-", n, Precedence.Subtraction)));
				}
			}

			return base.ConvertExpression(expr);
		}
	}
}
