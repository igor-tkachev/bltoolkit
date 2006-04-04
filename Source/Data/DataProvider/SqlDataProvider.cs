using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace BLToolkit.Data.DataProvider
{
	/// <summary>
	/// Implements access to the Data Provider for SQL Server.
	/// </summary>
	/// <remarks>
	/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
	/// </remarks>
	/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
	public class SqlDataProvider: DataProviderBase
	{
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
	}
}
