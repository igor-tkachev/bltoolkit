using System;
using System.Data;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class MsSql2005SqlProvider : MsSqlSqlProvider
	{
		public MsSql2005SqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		protected override void BuildDataType(System.Text.StringBuilder sb, SqlDataType type)
		{
			switch (type.DbType)
			{
				case SqlDbType.DateTime2 : sb.Append("DateTime");        break;
				default                  : base.BuildDataType(sb, type); break;
			}
		}
	}
}
