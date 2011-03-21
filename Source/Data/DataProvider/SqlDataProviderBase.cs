using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace BLToolkit.Data.DataProvider
{
	using Sql.SqlProvider;

	/// <summary>
	/// Implements access to the Data Provider for SQL Server.
	/// </summary>
	/// <remarks>
	/// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
	/// </remarks>
	/// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
	public abstract class SqlDataProviderBase : DataProviderBase
	{
		/// <summary>
		/// Creates the database connection object.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
		/// <returns>The database connection object.</returns>
		public override IDbConnection CreateConnectionObject()
		{
			return new SqlConnection();
		}

		/// <summary>
		/// Creates the data adapter object.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
		/// <returns>A data adapter object.</returns>
		public override DbDataAdapter CreateDataAdapterObject()
		{
			return new SqlDataAdapter();
		}

		/// <summary>
		/// Populates the specified <see cref="IDbCommand"/> object's Parameters collection with 
		/// parameter information for the stored procedure specified in the <see cref="IDbCommand"/>.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
		/// <param name="command">The <see cref="IDbCommand"/> referencing the stored procedure for which the parameter
		/// information is to be derived. The derived parameters will be populated into the 
		/// Parameters of this command.</param>
		public override bool DeriveParameters(IDbCommand command)
		{
			SqlCommandBuilder.DeriveParameters((SqlCommand)command);

			foreach (SqlParameter p in command.Parameters)
			{
				// We have to clear UDT type names.
				// Otherwise it will fail with error
				// "Database name is not allowed with a table-valued parameter"
				// but this is exactly the way how they are discovered.
				//
				if (p.SqlDbType == SqlDbType.Structured)
				{
					var firstDot = p.TypeName.IndexOf('.');
					if (firstDot >= 0)
						p.TypeName = p.TypeName.Substring(firstDot + 1);
				}
			}

			return true;
		}

		public override void PrepareCommand(ref CommandType commandType, ref string commandText, ref IDbDataParameter[] commandParameters)
		{
			base.PrepareCommand(ref commandType, ref commandText, ref commandParameters);

			if (commandParameters == null)
				return;

			foreach (var p in commandParameters)
			{
				var val = p.Value;

				if (val == null || !val.GetType().IsArray || val is byte[] || val is char[])
					continue;

				var dt = new DataTable();

				dt.Columns.Add("column_value", val.GetType().GetElementType());

				dt.BeginLoadData();

				foreach (object o in (Array)val)
				{
					var row = dt.NewRow();
					row[0] = o;
					dt.Rows.Add(row);
				}

				dt.EndLoadData();

				p.Value = dt;
			}
		}

		public override void SetUserDefinedType(IDbDataParameter parameter, string typeName)
		{
			if (!(parameter is SqlParameter))
				throw new ArgumentException("SqlParameter expected.", "parameter");

			((SqlParameter)parameter).TypeName = typeName;
		}

		public override object Convert(object value, ConvertType convertType)
		{
			switch (convertType)
			{
				case ConvertType.ExceptionToErrorNumber:
					if (value is SqlException)
						return ((SqlException)value).Number;
					break;
			}

			return SqlProvider.Convert(value, convertType);
		}

		/// <summary>
		/// Returns connection type.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
		/// <value>An instance of the <see cref="Type"/> class.</value>
		public override Type ConnectionType
		{
			get { return typeof(SqlConnection); }
		}

		public const string NameString = DataProvider.ProviderName.MsSql;

		/// <summary>
		/// Returns the data provider name.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataProvider Method</seealso>
		/// <value>Data provider name.</value>
		public override string Name
		{
			get { return NameString; }
		}

		public override ISqlProvider CreateSqlProvider()
		{
			return new MsSql2005SqlProvider();
		}

		public override int MaxParameters
		{
			get { return 2100 - 20; }
		}

		public override int MaxBatchSize
		{
			get { return 65536; }
		}

		#region GetDataReader

		public override IDataReader GetDataReader(Mapping.MappingSchema schema, IDataReader dataReader)
		{
			return dataReader is SqlDataReader?
				new SqlDataReaderEx((SqlDataReader)dataReader):
				base.GetDataReader(schema, dataReader);
		}

		class SqlDataReaderEx : DataReaderEx<SqlDataReader>
		{
			public SqlDataReaderEx(SqlDataReader rd): base(rd)
			{
			}

			public override DateTimeOffset GetDateTimeOffset(int i)
			{
				return DataReader.GetDateTimeOffset(i);
			}
		}

		#endregion
	}
}
