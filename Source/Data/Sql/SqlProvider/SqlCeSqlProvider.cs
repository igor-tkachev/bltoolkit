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
			expr = base.ConvertExpression(expr);

			if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "Length": return new SqlFunction("Len", func.Parameters);
					case "Left"  : return BuildAlternativeLeft (func);
					case "Right" : return BuildAlternativeRight(func);
				}
			}

			return expr;
		}
	}
}
