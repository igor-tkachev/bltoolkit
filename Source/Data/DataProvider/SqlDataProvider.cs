using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using BLToolkit.Mapping;

namespace BLToolkit.Data.DataProvider
{
	/// <summary>
	/// Implements access to the Data Provider for SQL Server.
	/// </summary>
	/// <remarks>
	/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
	/// </remarks>
	/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
	public sealed class SqlDataProvider: DataProviderBase
	{
		public SqlDataProvider()
		{
			//MappingSchema = new SqlMappingSchema();
		}

		/// <summary>
		/// Creates the database connection object.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
		/// <returns>The database connection object.</returns>
		public override IDbConnection CreateConnectionObject()
		{
			return new SqlConnection();
		}

		/// <summary>
		/// Creates the data adapter object.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
		/// <returns>A data adapter object.</returns>
		public override DbDataAdapter CreateDataAdapterObject()
		{
			return new SqlDataAdapter();
		}

		/// <summary>
		/// Populates the specified IDbCommand object's Parameters collection with 
		/// parameter information for the stored procedure specified in the IDbCommand.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
		/// <param name="command">The IDbCommand referencing the stored procedure for which the parameter information is to be derived. The derived parameters will be populated into the Parameters of this command.</param>
		public override bool DeriveParameters(IDbCommand command)
		{
			SqlCommandBuilder.DeriveParameters((SqlCommand)command);
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
			}

			return value;
		}

		/// <summary>
		/// Returns connection type.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
		/// <value>An instance of the <see cref="Type"/> class.</value>
		public override Type ConnectionType
		{
			get { return typeof(SqlConnection); }
		}

		/// <summary>
		/// Returns the data provider name.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataProvider Method</seealso>
		/// <value>Data provider name.</value>
		public override string Name
		{
			get { return "Sql"; }
		}

		public class SqlMappingSchema : MappingSchema
		{
			protected override DataReaderMapper CreateDataReaderMapper(IDataReader dataReader)
			{
				return new SqlDataReaderMapper(this, dataReader);
			}
		}

		public class SqlDataReaderMapper : DataReaderMapper
		{
			public SqlDataReaderMapper(MappingSchema mappingSchema, IDataReader dataReader)
				: base(mappingSchema, dataReader)
			{
				_dataReader = (SqlDataReader)dataReader;
			}

			private SqlDataReader _dataReader;

#if FW2
			public override Type GetFieldType(int index)
			{
				return _dataReader.GetProviderSpecificFieldType(index);
			}

			public override object GetValue(object o, int index)
			{
				return _dataReader.GetProviderSpecificValue(index);
			}
#endif

			public override SqlBoolean  GetSqlBoolean (object o, int index) { return _dataReader.GetSqlBoolean (index); }
			public override SqlByte     GetSqlByte    (object o, int index) { return _dataReader.GetSqlByte    (index); }
			public override SqlDateTime GetSqlDateTime(object o, int index) { return _dataReader.GetSqlDateTime(index); }
			public override SqlDecimal  GetSqlDecimal (object o, int index) { return _dataReader.GetSqlDecimal (index); }
			public override SqlDouble   GetSqlDouble  (object o, int index) { return _dataReader.GetSqlDouble  (index); }
			public override SqlGuid     GetSqlGuid    (object o, int index) { return _dataReader.GetSqlGuid    (index); }
			public override SqlInt16    GetSqlInt16   (object o, int index) { return _dataReader.GetSqlInt16   (index); }
			public override SqlInt32    GetSqlInt32   (object o, int index) { return _dataReader.GetSqlInt32   (index); }
			public override SqlInt64    GetSqlInt64   (object o, int index) { return _dataReader.GetSqlInt64   (index); }
			public override SqlMoney    GetSqlMoney   (object o, int index) { return _dataReader.GetSqlMoney   (index); }
			public override SqlSingle   GetSqlSingle  (object o, int index) { return _dataReader.GetSqlSingle  (index); }
			public override SqlString   GetSqlString  (object o, int index) { return _dataReader.GetSqlString  (index); }
		}
	}
}
