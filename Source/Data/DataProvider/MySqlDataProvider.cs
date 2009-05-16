// MySql Connector/Net
// http://dev.mysql.com/downloads/connector/net/
//
using System;
using System.Data;
using System.Data.Common;

using MySql.Data.MySqlClient;

namespace BLToolkit.Data.DataProvider
{
	using Sql.SqlProvider;
using System.Collections.Generic;

	public class MySqlDataProvider :  DataProviderBase
	{
		#region Static configuration


		public static char ParameterPrefix         { get; set; }
		public static bool TryConvertParameterName { get; set; }

		private static List<char> _convertParameterPrefixies;
		public  static List<char>  ConvertParameterPrefixies
		{
			get { return _convertParameterPrefixies; }
			set { _convertParameterPrefixies = value ?? new List<char>(); }
		}

		public static void ConfigureOldStyle()
		{
			ParameterPrefix           = '?';
			ConvertParameterPrefixies = new List<char>(new char[] { '@' });
			TryConvertParameterName   = true;
		}

		public static void ConfigureNewStyle()
		{
			ParameterPrefix           = '@';
			ConvertParameterPrefixies = null;
			TryConvertParameterName   = false;
		}

		static MySqlDataProvider()
		{
			ConfigureOldStyle();
		}
		
		#endregion

		public override IDbConnection CreateConnectionObject()
		{
			return new MySqlConnection();
		}

		public override DbDataAdapter CreateDataAdapterObject()
		{
			return new MySqlDataAdapter();
		}

		private void ConvertParameterNames(IDbCommand command)
		{
			foreach (IDataParameter p in command.Parameters)
			{
				if (p.ParameterName[0] != ParameterPrefix)
					p.ParameterName =
						Convert(
							Convert(p.ParameterName, ConvertType.ParameterToName),
							ConvertType.NameToParameter).ToString();
			}
		}

		public override bool DeriveParameters(IDbCommand command)
		{
			if (command is MySqlCommand)
			{
				MySqlCommandBuilder.DeriveParameters((MySqlCommand)command);
				if (TryConvertParameterName && ConvertParameterPrefixies.Count > 0)
					ConvertParameterNames(command);
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
					return ParameterPrefix + value.ToString();

				case ConvertType.ParameterToName:
					if (value != null)
					{
						string str = value.ToString();
						return (str.Length > 0 && (str[0] == ParameterPrefix || (TryConvertParameterName && ConvertParameterPrefixies.Contains(str[0])))) ? str.Substring(1) : str;
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
			get { return DataProvider.ProviderName.MySql; }
		}

		public override ISqlProvider CreateSqlProvider()
		{
			return new MySqlSqlProvider(this);
		}
	}
}
