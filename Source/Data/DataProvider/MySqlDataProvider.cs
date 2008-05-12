// MySql Connector/Net
// http://dev.mysql.com/downloads/connector/net/
//
using System;
using System.Data;
using System.Data.Common;
using System.Text;

using MySql.Data.MySqlClient;

namespace BLToolkit.Data.DataProvider
{
	public class MySqlDataProvider :  DataProviderBase
	{
		public override IDbConnection CreateConnectionObject()
		{
			return new MySqlConnection();
		}

		public override DbDataAdapter CreateDataAdapterObject()
		{
			return new MySqlDataAdapter();
		}

		public override bool DeriveParameters(IDbCommand command)
		{
			if (command is MySqlCommand)
			{
				MySqlCommandBuilder.DeriveParameters((MySqlCommand)command);
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
					return "?" + value;

				case ConvertType.ParameterToName:
					if (value != null)
					{
						string str = value.ToString();
						return (str.Length > 0 && str[0] == '?')? str.Substring(1): str;
					}
					break;

				case ConvertType.ExceptionToErrorNumber:
					if (value is MySqlException)
						return ((MySqlException)value).Number;
					break;
				 
			}

			return value;
		}

		public override Type ConnectionType
		{
			get { return typeof(MySqlConnection); }
		}

		public override string Name
		{
			get { return "MySql"; }
		}
	}
}
