using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using System.Xml;

// System.Data.SQLite.dll must be referenced.
// http://sqlite.phxsoftware.com/
//
using System.Data.SQLite;

namespace BLToolkit.Data.DataProvider
{
	/// <summary>
	/// Implements access to the Data Provider for SQLite.
	/// </summary>
	/// <remarks>
	/// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
	/// </remarks>
	/// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
	public sealed class SQLiteDataProvider: DataProviderBase
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
			return new SQLiteConnection();
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
			return new SQLiteDataAdapter();
		}

		/// <summary>
		/// Populates the specified IDbCommand object's Parameters collection with 
		/// parameter information for the stored procedure specified in the IDbCommand.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
		/// <param name="command">The IDbCommand referencing the stored procedure for which the parameter information is to be derived. The derived parameters will be populated into the Parameters of this command.</param>
		public override bool DeriveParameters(IDbCommand command)
		{
			// SQLiteCommandBuilder does not implement DeriveParameters.
			// This is not surprising, since SQLite has no support for stored procs.
			//
			return false;
		}

		public override object Convert(object value, ConvertType convertType)
		{
			string name = (string)value;

			switch (convertType)
			{
				case ConvertType.NameToQueryParameter:
				case ConvertType.NameToParameter:
					return "@" + name;

				case ConvertType.NameToQueryField:
				case ConvertType.NameToQueryTable:

					if (name.Length > 0 && name[0] == '[')
						return value;

					if (name.IndexOf('.') > 0)
						value = string.Join("].[", name.Split('.'));

					return "[" + value + "]";

				case ConvertType.ParameterToName:
					return name.Length > 0 && name[0] == '@'? name.Substring(1): name;

				case ConvertType.ExceptionToErrorNumber:
					if (value is SQLiteException)
						return ((SQLiteException)value).ErrorCode;
					break;
			}

			return value;
		}

		public override void AttachParameter(IDbCommand command, IDbDataParameter parameter)
		{
			if (parameter.Direction == ParameterDirection.Input || parameter.Direction == ParameterDirection.InputOutput)
			{
				if (parameter.Value is XmlDocument)
				{
					parameter.Value = Encoding.UTF8.GetBytes(((XmlDocument) parameter.Value).InnerXml);
					parameter.DbType = DbType.Binary;
				}
			}

			base.AttachParameter(command, parameter);
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
			get { return typeof(SQLiteConnection); }
		}

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
			get { return "SQLite"; }
		}

		public class SQLiteMappingSchema : Mapping.MappingSchema
		{
			#region Convert

			public override XmlReader ConvertToXmlReader(object value)
			{
				if (value is byte[])
					value = Encoding.UTF8.GetString((byte[])value);

				return base.ConvertToXmlReader(value);
			}

			public override XmlDocument ConvertToXmlDocument(object value)
			{
				if (value is byte[])
					value = Encoding.UTF8.GetString((byte[])value);

				return base.ConvertToXmlDocument(value);
			}

			#endregion
		}

		/// <summary>
		/// SQLite built-in text processor is ANSI-only  Just override it.
		/// </summary>
		[SQLiteFunction(Name = "lower", Arguments = 1, FuncType = FunctionType.Scalar)]
		internal class LoverFunction : SQLiteFunction
		{
			public override object Invoke(object[] args)
			{
				Debug.Assert(args != null && args.Length == 1);
				object arg = args[0];

				Debug.Assert(arg is string || arg is DBNull || arg is byte[]);
				return
					arg is string? ((string)arg).ToLower():
					arg is byte[]? Encoding.UTF8.GetString((byte[])arg).ToLower():
					arg;
			}
		}

		/// <summary>
		/// SQLite built-in text processor is ANSI-only  Just override it.
		/// </summary>
		[SQLiteFunction(Name = "upper", Arguments = 1, FuncType = FunctionType.Scalar)]
		internal class UpperFunction : SQLiteFunction
		{
			public override object Invoke(object[] args)
			{
				Debug.Assert(args != null && args.Length == 1);
				object arg = args[0];

				Debug.Assert(arg is string || arg is DBNull || arg is byte[]);
				return
					arg is string? ((string)arg).ToUpper():
					arg is byte[]? Encoding.UTF8.GetString((byte[])arg).ToUpper():
					arg;
			}
		}
	}
}
