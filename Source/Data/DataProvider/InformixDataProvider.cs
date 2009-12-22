using System;
using System.Data;
using System.Data.Common;

using IBM.Data.Informix;

namespace BLToolkit.Data.DataProvider
{
	using Sql.SqlProvider;

	class InformixDataProvider :  DataProviderBase
	{
		public override IDbConnection CreateConnectionObject () { return new IfxConnection      ();     }
		public override DbDataAdapter CreateDataAdapterObject() { return new IfxDataAdapter     ();     }
		public override ISqlProvider  CreateSqlProvider      () { return new InformixSqlProvider(this); }

		public override Type   ConnectionType         { get { return typeof(IfxConnection);              } }
		public override string Name                   { get { return DataProvider.ProviderName.Informix; } }
		public override string EndOfSql               { get { return ";"; } }

		public override bool DeriveParameters(IDbCommand command)
		{
			if (command is IfxCommand)
			{
				IfxCommandBuilder.DeriveParameters((IfxCommand)command);
				return true;
			}

			return false;
		}

		public override object Convert(object value, ConvertType convertType)
		{
			switch (convertType)
			{
				case ConvertType.NameToQueryParameter:
					return "?";

				case ConvertType.NameToParameter:
					return ":" + value;

				case ConvertType.ParameterToName:
					if (value != null)
					{
						string str = value.ToString();
						return (str.Length > 0 && str[0] == ':')? str.Substring(1): str;
					}

					break;

				case ConvertType.ExceptionToErrorNumber:
					if (value is IfxException)
					{
						IfxException ex = (IfxException)value;

						foreach (IfxError error in ex.Errors)
							return error.NativeError;

						return 0;
					}

					break;
				 
			}

			return value;
		}

		public override void PrepareCommand(ref CommandType commandType, ref string commandText, ref IDbDataParameter[] commandParameters)
		{
			if (commandParameters != null) foreach (var p in commandParameters)
			{
				if (p.Value is Guid)
				{
					string value = p.Value.ToString().ToUpper();
					p.DbType = DbType.AnsiStringFixedLength;
					p.Value  = value;
					p.Size   = value.Length;
				}
			}
		}
	}
}
