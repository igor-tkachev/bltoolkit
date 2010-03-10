using System;

namespace BLToolkit.Data.DataProvider
{
#if FW3
	using Sql.SqlProvider;
#endif

	public sealed class Sql2008DataProvider : SqlDataProviderBase
	{
		public override string Name
		{
			get { return DataProvider.ProviderName.MsSql2008; }
		}

#if FW3

		public override ISqlProvider CreateSqlProvider()
		{
			return new MsSql2008SqlProvider(this);
		}

#endif
	}
}
