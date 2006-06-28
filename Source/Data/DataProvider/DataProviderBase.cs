using System;
using System.Data;
using System.Data.Common;

using BLToolkit.Common;

namespace BLToolkit.Data.DataProvider
{
	/// <summary>
	/// The <b>DataProviderBase</b> is a class that provides specific data provider information
	/// for the <see cref="DbManager"/> class. 
	/// </summary>
	/// <remarks>
	/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
	/// </remarks>
	/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
	public abstract class DataProviderBase
	{
		#region Abstract Properties

		/// <summary>
		/// Returns an actual type of the connection object used by this instance of the <see cref="DbManager"/>.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
		/// <value>An instance of the <see cref="Type"/> class.</value>
		public abstract Type ConnectionType { get; }

		/// <summary>
		/// Returns the data manager name.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
		/// <value>The data manager name.</value>
		public abstract string Name { get; }

		#endregion

		#region Abstract Methods

		/// <summary>
		/// Creates a new instance of the <see cref="IDbConnection"/>.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
		/// <returns>The <see cref="IDbConnection"/> object.</returns>
		public abstract IDbConnection CreateConnectionObject();

		/// <summary>
		/// Creates an instance of the <see cref="DbDataAdapter"/>.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
		/// <returns>The <see cref="DbDataAdapter"/> object.</returns>
		public abstract DbDataAdapter CreateDataAdapterObject();

		/// <summary>
		/// Populates the specified <see cref="IDbCommand"/> object's Parameters collection with 
		/// parameter information for the stored procedure specified in the <see cref="IDbCommand"/>.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
		/// <param name="command">The <see cref="IDbCommand"/> referencing the stored procedure 
		/// for which the parameter information is to be derived.
		/// The derived parameters will be populated into the Parameters of this command.</param>
		/// <returns>true - parameters can be derive.</returns>
		public abstract bool DeriveParameters(IDbCommand command);

		#endregion

		public virtual object Convert(object value, ConvertType convertType)
		{
			return value;
		}

		public virtual void AttachParameter(IDbCommand command, IDbDataParameter parameter)
		{
			command.Parameters.Add(parameter);
		}

		public virtual bool IsValueParameter(IDbDataParameter parameter)
		{
			return parameter.Direction != ParameterDirection.ReturnValue;
		}

		public virtual IDbDataParameter CloneParameter(IDbDataParameter parameter)
		{
			return (IDbDataParameter)((ICloneable)parameter).Clone();
		}

		public virtual IDbCommand CreateCommandObject(IDbConnection connection)
		{
			return connection.CreateCommand();
		}

#if EXPERIMENTAL
		// This method must be overriden to provide access to specific data types such a 'Xml' data type
		//
		public virtual DataReader<T>.GetMethod SelectDataReaderGetMethod<T>(IDataReader dr, int index)
		{
			return DataReader<T>.Get;
		}
#endif
	}
}
