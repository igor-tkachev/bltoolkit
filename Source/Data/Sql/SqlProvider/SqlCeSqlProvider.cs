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
					case "Length": return new SqlFunction("Len",       func.Parameters);
					case "Left"  : return new SqlFunction("Substring", func.Parameters[0], new SqlValue(1), func.Parameters[1]);
				}
			}

			return base.ConvertExpression(expr);
		}
	}
}
