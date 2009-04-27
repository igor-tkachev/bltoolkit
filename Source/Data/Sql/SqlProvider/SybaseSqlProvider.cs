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
					case "Length"   : return new SqlFunction("Len", func.Parameters);
					case "CharIndex":
						if (func.Parameters.Length == 3)
							return Add<int>(
								new SqlFunction("CharIndex",
									func.Parameters[0],
									new SqlFunction("Substring",
										func.Parameters[1],
										func.Parameters[2], new SqlFunction("Len", func.Parameters[1]))),
								Sub(func.Parameters[2], 1));

						break;
				}
			}

			return base.ConvertExpression(expr);
		}
	}
}
