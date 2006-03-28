using System;
using System.Data;
using System.Data.Common;

using Sybase.Data.AseClient;

namespace BLToolkit.Data.DataProvider
{
	public class SybaseDataProvider : IDataProvider
	{
		public IDbConnection CreateConnectionObject()
		{
			return new AseConnection();
		}

		public DbDataAdapter CreateDataAdapterObject()
		{
			return new AseDataAdapter();
		}

		public bool DeriveParameters(IDbCommand command)
		{
			AseCommandBuilder.DeriveParameters((AseCommand)command);
			return true;
		}

		public object Convert(object value, ConvertType convertType)
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
						return str.Length > 0 && str[0] == '@'? str.Substring(1): str;
					}

					break;
			}

			return value;
		}

		public void SetParameterType(IDbDataParameter parameter, object value)
		{
			if (value is string)
				parameter.DbType = DbType.AnsiString;
		}

		public Type ConnectionType
		{
			get { return typeof(AseConnection); }
		}

		public string Name
		{
			get { return "Sybase"; }
		}
	}
}
