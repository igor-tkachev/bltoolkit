using System;
using System.Data;
using System.Data.Common;

using Npgsql;

namespace BLToolkit.Data.DataProvider
{
	using Sql.SqlProvider;
	using BLToolkit.Mapping;

	public class PostgreSQLDataProvider : DataProviderBase
	{
		#region Configurable

		public static bool UseUpperCase;
		public static bool UseLowerCase;

		public static bool QuoteIdentifiers
		{
			get { return PostgreSQLSqlProvider.QuoteIdentifiers; }
			set { PostgreSQLSqlProvider.QuoteIdentifiers = value; }
		}

		public override void Configure(System.Collections.Specialized.NameValueCollection attributes)
		{
			var quoteIdentifiers = attributes["QuoteIdentifiers"];

			if (quoteIdentifiers != null)
				QuoteIdentifiers = Common.Convert.ToBoolean(quoteIdentifiers);

			var useLowerCase = attributes["UseLowerCase"];
			if (useLowerCase != null)
				UseLowerCase = Common.Convert.ToBoolean(useLowerCase);

			var useUpperCase = attributes["UseUpperCase"];
			if (useUpperCase != null && !UseLowerCase)
				UseUpperCase = Common.Convert.ToBoolean(useUpperCase);

			base.Configure(attributes);
		}

		#endregion

		public override IDbConnection CreateConnectionObject()
		{
			return new NpgsqlConnection();
		}

		public override DbDataAdapter CreateDataAdapterObject()
		{
			return new NpgsqlDataAdapter();
		}

		public override bool DeriveParameters(IDbCommand command)
		{
			NpgsqlCommandBuilder.DeriveParameters((NpgsqlCommand)command);
			return true;
		}

		public override void SetParameterValue(IDbDataParameter parameter, object value)
		{
			if(value is Enum)
			{
				var type = Enum.GetUnderlyingType(value.GetType());
				value = (MappingSchema ?? Map.DefaultSchema).ConvertChangeType(value, type);

			}
			base.SetParameterValue(parameter, value);
		}

		public override object Convert(object value, ConvertType convertType)
		{
			switch (convertType)
			{
				case ConvertType.ExceptionToErrorNumber:
					if (value is NpgsqlException)
					{
						var ex = (NpgsqlException)value;

						foreach (NpgsqlError error in ex.Errors)
							return error.Code;

						return 0;
					}

					break;
			}

			return SqlProvider.Convert(value, convertType);
		}

		public override Type ConnectionType
		{
			get { return typeof(NpgsqlConnection); }
		}

		public override string Name
		{
			get { return DataProvider.ProviderName.PostgreSQL; }
		}

		public override int MaxBatchSize
		{
			get { return 0; }
		}

		public override ISqlProvider CreateSqlProvider()
		{
			return new PostgreSQLSqlProvider();
		}

		public override void PrepareCommand(ref CommandType commandType, ref string commandText, ref IDbDataParameter[] commandParameters)
		{
			if (UseLowerCase)
				commandText = commandText.ToLower();
			if (UseUpperCase)
				commandText = commandText.ToLower();

			base.PrepareCommand(ref commandType, ref commandText, ref commandParameters);
		}

	}
}
