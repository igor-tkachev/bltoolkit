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
			expr = base.ConvertExpression(expr);

			if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "Length"    : return new SqlFunction("Len", func.Parameters);
					case "CharIndex" :
						if (func.Parameters.Length == 3)
							return Add<int>(
								ConvertExpression(new SqlFunction("CharIndex",
									func.Parameters[0],
									ConvertExpression(new SqlFunction("Substring",
										func.Parameters[1],
										func.Parameters[2], new SqlFunction("Len", func.Parameters[1]))))),
								Sub(func.Parameters[2], 1));
						break;

					case "Stuff"     :
						if (func.Parameters[3] is SqlValue)
						{
							SqlValue value = (SqlValue)func.Parameters[3];

							if (value.Value is string && string.IsNullOrEmpty((string)value.Value))
								return new SqlFunction(
									func.Name,
									func.Precedence,
									func.Parameters[0],
									func.Parameters[1],
									func.Parameters[1],
									new SqlValue(null));
						}

						break;
				}
			}

			return expr;
		}
	}
}
