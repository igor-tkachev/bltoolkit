using System;
using System.Data;

namespace BLToolkit.Data.DataProvider
{
	using Sql.SqlProvider;

	public sealed class Sql2005DataProvider : SqlDataProviderBase
	{
		public override string Name
		{
			get { return DataProvider.ProviderName.MsSql2005; }
		}

		public override ISqlProvider CreateSqlProvider()
		{
			return new MsSql2005SqlProvider();
		}

		public override void AttachParameter(IDbCommand command, IDbDataParameter parameter)
		{
			if (parameter.DbType == DbType.DateTime2)
				parameter.DbType = DbType.DateTime;

			base.AttachParameter(command, parameter);
		}
	}
}
