using System;

namespace BLToolkit.Data.DataProvider
{
	using Sql.SqlProvider;

	public sealed class Sql2000DataProvider : SqlDataProviderBase
	{
		public override string Name
		{
			get { return DataProvider.ProviderName.MsSql2000; }
		}

		public override ISqlProvider CreateSqlProvider()
		{
			return new MsSql2000SqlProvider();
		}
	}
}
