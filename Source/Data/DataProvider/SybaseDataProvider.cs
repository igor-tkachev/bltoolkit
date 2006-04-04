using System;
using System.Data;
using System.Data.Common;

using Sybase.Data.AseClient;

namespace BLToolkit.Data.DataProvider
{
	public class SybaseDataProvider : DataProviderBase
	{
		public override IDbConnection CreateConnectionObject()
		{
			return new AseConnection();
		}

		public override DbDataAdapter CreateDataAdapterObject()
		{
			return new AseDataAdapter();
		}

		public override bool DeriveParameters(IDbCommand command)
		{
			AseCommandBuilder.DeriveParameters((AseCommand)command);
			return true;
		}

		public override void SetParameterType(IDbDataParameter parameter, object value)
		{
			if (value is string)
				parameter.DbType = DbType.AnsiString;
		}

		public override Type ConnectionType
		{
			get { return typeof(AseConnection); }
		}

		public override string Name
		{
			get { return "Sybase"; }
		}
	}
}
