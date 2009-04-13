using System;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class PostgreSQLSqlProvider : BasicSqlProvider
	{
		public PostgreSQLSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		protected override void BuildBinaryExpression(StringBuilder sb, SqlBinaryExpression expr)
		{
			switch (expr.Operation[0])
			{
				case '^': BuildBinaryExpression(sb, "#", expr); break;
				default : base.BuildBinaryExpression(sb, expr); break;;
			}
		}
	}
}
