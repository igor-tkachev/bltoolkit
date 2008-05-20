/***

 * FdpDataProvider
needed FirebirdClient http://sourceforge.net/project/showfiles.php?group_id=9028&package_id=62107
tested with FirebirdClient 2.1.0 Beta 3

Known troubles:
1) Some tests fails due to Fb SQL-syntax specific
2) ResultSet mapping doesn't work - not supported by client
3) UnitTests.CS.DataAccess.OutRefTest tests: Test2 && TestNullable2 doesnt work:
	parameters directions should be provided correctly to functions run, that's why
	output parameterd would be mapped to Entity e, so asserts should be same a in Test1.

"Features"
1) Type conversation due to http://www.firebirdsql.org/manual/migration-mssql-data-types.html
	BUT! for Binary types BLOB is used! not CHAR!
2) InOut parameters faking: InOut parameters are not suppotred by Fb, but they could be
	emulated: each InOut parameter should be defined in RETURNS() section, and allso has a mirror 
	in parameter section with name [prefix][inOutParameterName], see OutRefTest SP. Faking settings:
	FdpDataProvider.InOutInputParameterPrefix = "in_";
	FdpDataProvider.IsInOutParameterEmulation = true;
3) Returned values faking. Each parameter with "magic name" woul be treated as ReturnValue.
	see Scalar_ReturnParameter SP. Faking settings:
	FdpDataProvider.ReturnParameterName = "RETURN_VALUE";
	FdpDataProvider.IsReturnValueEmulation = true;

 */

using System;
using System.Data;
using System.Data.Common;

using FirebirdSql.Data.FirebirdClient;

using BLToolkit.Mapping;

namespace BLToolkit.Data.DataProvider
{
	public class FdpDataProvider : DataProviderBase
	{
		public FdpDataProvider()
		{
			MappingSchema = new FbMappingSchema();
		}

		#region InOut & ReturnValue emulation

		public static string InOutInputParameterPrefix = "in_";
		public static string ReturnParameterName = "RETURN_VALUE";

		public static bool IsReturnValueEmulation    = true;
		public static bool IsInOutParameterEmulation = true;
		public static bool QuoteIdentifiers          = false;

		#endregion

		#region Overloads

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

				if (IsReturnValueEmulation)
					foreach (IDbDataParameter par in command.Parameters)
						if (IsReturnValue(par))
							par.Direction = ParameterDirection.ReturnValue;

				return true;
			}
			return false;
		}

		public override object Convert(object value, ConvertType convertType)
		{
			switch (convertType)
			{
				case ConvertType.NameToQueryField:
				case ConvertType.NameToQueryTable:
					if (QuoteIdentifiers)
					{
						string name = value.ToString();

						if (name.Length > 0 && name[0] == '"')
							return value;

						return '"' + name + '"';
					}
					break;

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

				case ConvertType.ExceptionToErrorNumber:
					if (value is FbException)
					{
						FbException ex = (FbException)value;
						if (ex.Errors.Count > 0)
							return ex.Errors[0].Number;
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

		public override bool IsValueParameter(IDbDataParameter parameter)
		{
			return parameter.Direction != ParameterDirection.ReturnValue
				&& parameter.Direction != ParameterDirection.Output;
		}

		private string GetInputParameterName(string ioParameterName)
		{
			return (string)Convert(
				InOutInputParameterPrefix + (string)Convert(ioParameterName, ConvertType.ParameterToName),
				ConvertType.NameToParameter);
		}
		private static IDbDataParameter GetParameter(string parameterName, IDbDataParameter[] commandParameters)
		{
			foreach (IDbDataParameter par in commandParameters)
				if (string.Compare(parameterName, par.ParameterName, true) == 0)
					return par;
			return null;
		}

		private bool IsReturnValue(IDbDataParameter parameter)
		{
			if (string.Compare(parameter.ParameterName,
					(string)Convert(ReturnParameterName, ConvertType.NameToParameter), true) == 0
				)
				return true;

			return false;
		}

		public override void PrepareCommand(ref CommandType commandType, ref string commandText, ref IDbDataParameter[] commandParameters)
		{
			#region "smart" input-output parameter detection
			if (commandType == CommandType.StoredProcedure && IsInOutParameterEmulation)
			{
				foreach (IDbDataParameter par in commandParameters)
				{
					if (par.Direction == ParameterDirection.InputOutput
						|| par.Direction == ParameterDirection.Output)
					{
						string           iParameterName  = GetInputParameterName(par.ParameterName);
						IDbDataParameter fakeIOParameter = GetParameter(iParameterName, commandParameters);

						if (fakeIOParameter == null)
							if (par.Direction == ParameterDirection.InputOutput)
								throw new DataException(string.Format(
										"Input parameter {0} for input-output parameter {1} is not found",
										iParameterName, par.ParameterName));
							else
								continue;

						fakeIOParameter.Value = par.Value;

						// direction should be output, or parameter mistmath for procedure exception
						// would be thrown
						par.Direction = ParameterDirection.Output;
						// direction should be Input
						fakeIOParameter.Direction = ParameterDirection.Input;
					}

				}
			}
			#endregion

			base.PrepareCommand(ref commandType, ref commandText, ref commandParameters);
		}

		#endregion

		#region FbMappingSchema

		public class FbMappingSchema : MappingSchema
		{
			public byte[] ConvertToByteArray(string value)
			{
				return System.Text.Encoding.UTF8.GetBytes(value);
			}

			public override byte[] ConvertToByteArray(object value)
			{
				if (value is string)
					return ConvertToByteArray((string)value);

				return base.ConvertToByteArray(value);
			}

			public bool ConvertToBoolean(string value)
			{
				value = value.ToLower();
				if (value == "1" || value == "0"
					|| value == "t" || value == "f")
					return value == "1" || value == "t";
				return Common.Convert.ToBoolean(value);
			}

			public override bool ConvertToBoolean(object value)
			{
				if (value is string)
					return ConvertToBoolean((string)value);

				return base.ConvertToBoolean(value);
			}


			public System.IO.Stream ConvertToStream(string value)
			{
				return new System.IO.MemoryStream(ConvertToByteArray(value));
			}

			public override System.IO.Stream ConvertToStream(object value)
			{
				if (value is string)
					return ConvertToStream((string)value);

				return base.ConvertToStream(value);
			}

			public System.Data.SqlTypes.SqlBinary ConvertToSqlBinary(string value)
			{
				return BLToolkit.Common.Convert.ToSqlBinary(ConvertToByteArray(value));
			}

			public override System.Data.SqlTypes.SqlBinary ConvertToSqlBinary(object value)
			{
				if (value is string)
					return ConvertToSqlBinary((string)value);
				return base.ConvertToSqlBinary(value);
			}

			public System.Data.SqlTypes.SqlBytes ConvertToSqlBytes(string value)
			{
				return BLToolkit.Common.Convert.ToSqlBytes(ConvertToByteArray(value));
			}

			public override System.Data.SqlTypes.SqlBytes ConvertToSqlBytes(object value)
			{
				if (value is string)
					return ConvertToSqlBytes((string)value);

				return base.ConvertToSqlBytes(value);
			}

			public override bool? ConvertToNullableBoolean(object value)
			{
				if (value is string)
					return ConvertToBoolean((string)value);
				return base.ConvertToNullableBoolean(value);
			}

			public override System.Data.SqlTypes.SqlGuid ConvertToSqlGuid(object value)
			{
				if (value is string)
					return new System.Data.SqlTypes.SqlGuid(new Guid((string)value));
				return base.ConvertToSqlGuid(value);
			}

			protected override object MapInternal(Reflection.InitContext initContext)
			{
				FbDataReader dr = initContext.SourceObject as FbDataReader;

				// Fb's SP returns single row with nulls if selected object doesn't exists
				// so for all DBNull's (null) should be returned, instead of object instance
				//
				if (dr != null)
				{
					int i = dr.FieldCount;
					while (--i >= 0)
						if (!dr.IsDBNull(i))
							break;

					// All field are DBNull.
					//
					if (i < 0)
						return null;
				}
				return base.MapInternal(initContext);
			}
		}

		#endregion
	}
}