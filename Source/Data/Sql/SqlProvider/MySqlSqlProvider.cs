using System;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class MySqlSqlProvider : BasicSqlProvider
	{
		public MySqlSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "CharIndex": return new SqlFunction("Locate", func.Parameters);
				}
			}

			return base.ConvertExpression(expr);
		}
	}
}
