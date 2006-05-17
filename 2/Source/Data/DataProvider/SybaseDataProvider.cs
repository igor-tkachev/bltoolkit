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

		public override void AttachParameter(IDbCommand command, IDbDataParameter parameter)
		{
			if (parameter.Value is string && parameter.DbType == DbType.Guid)
				parameter.DbType = DbType.AnsiString;

			base.AttachParameter(command, parameter);
			
			AseParameter p = (AseParameter)parameter;

			if (p.AseDbType == AseDbType.Unsupported && p.Value is DBNull)
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
