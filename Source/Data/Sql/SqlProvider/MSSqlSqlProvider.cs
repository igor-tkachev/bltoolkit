using System;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class MSSqlSqlProvider : BasicSqlProvider
	{
		public MSSqlSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "Length": return new SqlFunction("Len", func.Parameters);
				}
			}

			return base.ConvertExpression(expr);
		}
	}
}
