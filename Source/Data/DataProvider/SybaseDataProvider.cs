using System;
using System.Collections;
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

		public override object Convert(object value, ConvertType convertType)
		{
			switch (convertType)
			{
				case ConvertType.NameToQueryParameter:
				case ConvertType.NameToParameter:
					return "@" + value;

				case ConvertType.NameToQueryField:
					{
						string name = value.ToString();

						if (name.Length > 0 && name[0] == '[')
							return value;
					}

					return "[" + value + "]";

				case ConvertType.NameToQueryTable:
					{
						string name = value.ToString();

						if (name.Length > 0 && name[0] == '[')
							return value;

						if (name.IndexOf('.') > 0)
							value = string.Join("].[", name.Split('.'));
					}

					return "[" + value + "]";

				case ConvertType.ParameterToName:
					if (value != null)
					{
						string str = value.ToString();
						return str.Length > 0 && str[0] == '@'? str.Substring(1): str;
					}

					break;

				case ConvertType.ExceptionToErrorNumber:
					if (value is AseException)
					{
						AseException ex = (AseException)value;

						if (ex.Errors.Count > 0)
							return ex.Errors[0].MessageNumber;
					}

					break;
			}

			return value;
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

		public override void PrepareCommand(ref CommandType commandType, ref string commandText, ref IDbDataParameter[] commandParameters)
		{
			base.PrepareCommand(ref commandType, ref commandText, ref commandParameters);

			if (commandType == CommandType.Text && commandParameters != null)
			{
				ArrayList list = null;

				for (int i = 0; i < commandParameters.Length; i++)
				{
					IDbDataParameter p = commandParameters[i];

					if (commandText.IndexOf(p.ParameterName) < 0)
					{
						if (list == null)
						{
							list = new ArrayList(commandParameters.Length);

							for (int j = 0; j < i; j++)
								list.Add(commandParameters[j]);
						}
					}
					else
					{
						if (list != null)
							list.Add(p);
					}
				}

				if (list != null)
					commandParameters = (IDbDataParameter[])list.ToArray(typeof(IDbDataParameter));
			}
		}
	}
}
