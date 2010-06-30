using System;
using System.Data;
using System.Data.Common;

using Npgsql;

namespace BLToolkit.Data.DataProvider
{
	using Sql.SqlProvider;

	public class PostgreSQLDataProvider : DataProviderBase
	{
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
	}
}
