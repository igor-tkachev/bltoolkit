// MySql Connector/Net
// http://dev.mysql.com/downloads/connector/net/
//
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using MySql.Data.MySqlClient;

namespace BLToolkit.Data.DataProvider
{
	using Sql.SqlProvider;

	public class MySqlDataProvider :  DataProviderBase
	{
		#region Static configuration

		public static char ParameterSymbol           { get; set; }
		public static bool TryConvertParameterSymbol { get; set; }

		private static List<char> _convertParameterSymbols;
		public  static List<char>  ConvertParameterSymbols
		{
			get { return _convertParameterSymbols; }
			set { _convertParameterSymbols = value ?? new List<char>(); }
		}

		[Obsolete("Use CommandParameterPrefix or SprocParameterPrefix instead.")]
		public  static string  ParameterPrefix
		{
			get { return _sprocParameterPrefix; }
			set { _sprocParameterPrefix = _commandParameterPrefix = string.IsNullOrEmpty(value) ? string.Empty : value; }
		}

		private static string _commandParameterPrefix = "";
		public  static string  CommandParameterPrefix
		{
			get { return _commandParameterPrefix; }
			set { _commandParameterPrefix = string.IsNullOrEmpty(value) ? string.Empty : value; }
		}

		private static string _sprocParameterPrefix = "";
		public  static string  SprocParameterPrefix
		{
			get { return _sprocParameterPrefix; }
			set { _sprocParameterPrefix = string.IsNullOrEmpty(value) ? string.Empty : value; }
		}

		public static void ConfigureOldStyle()
		{
			ParameterSymbol           = '?';
			ConvertParameterSymbols   = new List<char>(new char[] { '@' });
			TryConvertParameterSymbol = true;
		}

		public static void ConfigureNewStyle()
		{
			ParameterSymbol           = '@';
			ConvertParameterSymbols   = null;
			TryConvertParameterSymbol = false;
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
				if (p.ParameterName[0] != ParameterSymbol)
					p.ParameterName =
						Convert(
							Convert(p.ParameterName, ConvertType.SprocParameterToName),
							command.CommandType == CommandType.StoredProcedure ? ConvertType.NameToSprocParameter : ConvertType.NameToCommandParameter).ToString();
			}
		}

		public override bool DeriveParameters(IDbCommand command)
		{
			if (command is MySqlCommand)
			{
				MySqlCommandBuilder.DeriveParameters((MySqlCommand)command);

				if (TryConvertParameterSymbol && ConvertParameterSymbols.Count > 0)
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
					return ParameterSymbol + value.ToString();

				case ConvertType.NameToCommandParameter:
					return ParameterSymbol + CommandParameterPrefix + value.ToString();

				case ConvertType.NameToSprocParameter:
					return ParameterSymbol + SprocParameterPrefix + value.ToString();

				case ConvertType.SprocParameterToName:
					if (value != null)
					{
						string str = value.ToString();
						str = (str.Length > 0 && (str[0] == ParameterSymbol || (TryConvertParameterSymbol && ConvertParameterSymbols.Contains(str[0])))) ? str.Substring(1) : str;

						if ((!string.IsNullOrEmpty(SprocParameterPrefix))
							&& str.StartsWith(SprocParameterPrefix))
							str = str.Substring(SprocParameterPrefix.Length);

						return str;
					}
					break;

				case ConvertType.ExceptionToErrorNumber:
					if (value is MySqlException)
						return ((MySqlException)value).Number;
					break;

				case ConvertType.ExceptionToErrorMessage:
					if (value is MySqlException)
						return ((MySqlException)value).Message;
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

		public override void Configure(System.Collections.Specialized.NameValueCollection attributes)
		{
			string paremeterPrefix = attributes["ParameterPrefix"];
			if (paremeterPrefix != null)
				CommandParameterPrefix = SprocParameterPrefix = paremeterPrefix;

			paremeterPrefix = attributes["CommandParameterPrefix"];
			if (paremeterPrefix != null)
				CommandParameterPrefix = paremeterPrefix;

			paremeterPrefix = attributes["SprocParameterPrefix"];
			if (paremeterPrefix != null)
				SprocParameterPrefix = paremeterPrefix;

			string configName = attributes["ParameterSymbolConfig"];
			if (configName != null)
			{
				switch (configName)
				{
					case "OldStyle":
						ConfigureOldStyle();
						break;
					case "NewStyle":
						ConfigureNewStyle();
						break;
				}
			}

			string parameterSymbol = attributes["ParameterSymbol"];
			if (parameterSymbol != null && parameterSymbol.Length == 1)
				ParameterSymbol = parameterSymbol[0];

			string convertParameterSymbols = attributes["ConvertParameterSymbols"];
			if (convertParameterSymbols != null)
				ConvertParameterSymbols = new List<char>(convertParameterSymbols.ToCharArray());

			string tryConvertParameterSymbol = attributes["TryConvertParameterSymbol"];
			if (tryConvertParameterSymbol != null)
				TryConvertParameterSymbol = BLToolkit.Common.Convert.ToBoolean(tryConvertParameterSymbol);

			base.Configure(attributes);
		}
	}
}
