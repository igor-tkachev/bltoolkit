using System;
using System.Data;
using System.Data.Common;

using FirebirdSql.Data.FirebirdClient;

namespace BLToolkit.Data.DataProvider
{
	public class FdpDataProvider : DataProviderBase
	{
		public override IDbConnection CreateConnectionObject()
		{
			return new FbConnection();
		}

		public override DbDataAdapter CreateDataAdapterObject()
		{
			return new FbDataAdapter();
		}

		public override bool DeriveParameters(IDbCommand command)
		{
			if (command is FbCommand)
			{
				FbCommandBuilder.DeriveParameters((FbCommand)command);
				return true;
			}
			return false;
		}

		public override object Convert(object value, ConvertType convertType)
		{
			switch (convertType)
			{
				case ConvertType.NameToQueryParameter:
				case ConvertType.NameToParameter:
					return "@" + value;

				case ConvertType.ParameterToName:
					if (value != null)
					{
						string str = value.ToString();
						return str.Length > 0 && str[0] == '@' ? str.Substring(1) : str;
					}
					break;
			}

			return value;
		}
		public override Type ConnectionType
		{
			get
			{
				return typeof(FbConnection);
			}
		}

		public override string Name
		{
			get { return "Fdp"; }
		}
	}
}