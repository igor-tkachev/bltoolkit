using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;

#if FW2
using System.Collections.Generic;
using ConfigManager = System.Configuration.ConfigurationManager;
#else
using ConfigManager = System.Configuration.ConfigurationSettings;
#endif

using BLToolkit.Common;
using BLToolkit.Data.DataProvider;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace BLToolkit.Data
{
	/// <summary>
	/// The <b>DbManager</b> is a primary class of the <see cref="BLToolkit.Data"/> namespace
	/// that can be used to execute commands of different database providers.
	/// </summary>
	/// <remarks>
	/// When the <b>DbManager</b> goes out of scope, it does not close the internal connection object.
	/// Therefore, you must explicitly close the connection by calling <see cref="Close"/> or 
	/// <see cref="Dispose(bool)"/>. Also, you can use the C# <b>using</b> statement.
	/// </remarks>
	/// <include file="Examples.xml" path='examples/db[@name="DbManager"]/*' />
	[System.ComponentModel.DesignerCategory("Code")]
	public class DbManager : Component
	{
		#region Public Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DbManager"/> class 
		/// and opens a database connection.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This constructor uses a configuration, which has been used first in your application. 
		/// If there has been no connection used before, an empty string is applied as a default configuration.
		/// </para>
		/// <para>
		/// See the <see cref="ConfigurationString"/> property 
		/// for an explanation and use of the default configuration.
		/// </para>
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="ctor"]/*' />
		/// <seealso cref="AddConnectionString(string)">AddConnectionString Method</seealso>
		/// <returns>An instance of the database manager class.</returns>
		[System.Diagnostics.DebuggerStepThrough]
		public DbManager() : this((IDbConnection)null, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DbManager"/> class 
		/// and opens a database connection for the provided configuration.
		/// </summary>
		/// <remarks>
		/// See the <see cref="ConfigurationString"/> property 
		/// for an explanation and use of the configuration string.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="ctor(string)"]/*' />
		/// <param name="configurationString">Configuration string.</param>
		/// <returns>An instance of the <see cref="DbManager"/> class.</returns>
		[System.Diagnostics.DebuggerStepThrough]
		public DbManager(string configurationString)
			: this((IDbConnection)null, configurationString)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DbManager"/> class 
		/// and opens a database connection for the provided configuration.
		/// </summary>
		/// <remarks>
		/// See the <see cref="ConfigurationString"/> property 
		/// for an explanation and use of the configuration string.
		/// </remarks>
		/// <param name="configuration">Configuration string not containing provider name.</param>
		/// <param name="providerName">Provider configuration name.</param>
		/// <returns>An instance of the <see cref="DbManager"/> class.</returns>
		[System.Diagnostics.DebuggerStepThrough]
		public DbManager(string providerName, string configuration)
			: this((IDbConnection)null, providerName + ProviderNameDivider + configuration)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DbManager"/> class for the provided connection.
		/// </summary>
		/// <remarks>
		/// This constructor tries to open the connection if the connection state equals 
		/// <see cref="System.Data.ConnectionState">ConnectionState.Closed</see>. 
		/// In this case the <see cref="IDbConnection.ConnectionString"/> property of the connection 
		/// must be set before colling the constructor.
		/// Otherwise, it neither opens nor closes the connection. 
		/// </remarks>
		/// <exception cref="DataException">
		/// Type of the connection could not be recognized.
		/// </exception>
		/// <include file="Examples.xml" path='examples/db[@name="ctor(IDbConnection)"]/*' />
		/// <param name="connection">An instance of the <see cref="IDbConnection"/> class.</param>
		/// <returns>An instance of the <see cref="DbManager"/> class.</returns>
		[System.Diagnostics.DebuggerStepThrough]
		public DbManager(IDbConnection connection)
		{
			if (connection != null)
			{
				Init(connection);
		
				if (_connection.State == ConnectionState.Closed)
					OpenConnection();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DbManager"/> class for the provided transaction.
		/// </summary>
		/// <include file="Examples.xml" path='examples/db[@name="ctor(IDbTransaction)"]/*' />
		/// <param name="transaction"></param>
		[System.Diagnostics.DebuggerStepThrough]
		public DbManager(IDbTransaction transaction)
			: this(transaction != null? transaction.Connection: null)
		{
			_transaction      = transaction;
			_closeTransaction = false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DbManager"/> class 
		/// and opens a database connection for the provided configuration and database connection.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This constructor opens the connection only if the connection state equals 
		/// <see cref="System.Data.ConnectionState">ConnectionState.Closed</see>. 
		/// Otherwise, it neither opens nor closes the connection.
		/// </para>
		/// <para>
		/// See the <see cref="ConfigurationString"/> property 
		/// for an explanation and use of the configuration string.
		/// </para>
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="ctor(IDbConnection,string)"]/*' />
		/// <param name="connection">An instance of the <see cref="IDbConnection"/>.</param>
		/// <param name="configurationString">The configuration string.</param>
		/// <returns>An instance of the <see cref="DbManager"/> class.</returns>
		[System.Diagnostics.DebuggerStepThrough]
		public DbManager(
			IDbConnection connection,
			string        configurationString)
		{
			if (connection == null)
			{
				Init(configurationString);

				if (configurationString != null)
					OpenConnection(configurationString);
			}
			else
			{
				Init(connection);

				_configurationString = configurationString;
				_connection.ConnectionString = GetConnectionString(configurationString);

				if (_connection.State == ConnectionState.Closed)
					OpenConnection();
			}
		}

		#endregion

		#region Public Properties

		private MappingSchema _mappingSchema = Map.DefaultSchema;
		/// <summary>
		/// Gets the <see cref="BLToolkit.Mapping.MappingSchema"/> 
		/// used by this instance of the <see cref="DbManager"/>.
		/// </summary>
		/// <value>
		/// A mapping schema.
		/// </value>
		public MappingSchema MappingSchema
		{
			get { return _mappingSchema; }
			set { _mappingSchema = value != null? value: Map.DefaultSchema; }
		}

		private DataProviderBase _dataProvider;
		/// <summary>
		/// Gets the <see cref="BLToolkit.Data.DataProvider.DataProviderBase"/> 
		/// used by this instance of the <see cref="DbManager"/>.
		/// </summary>
		/// <value>
		/// A data provider.
		/// </value>
		/// <include file="Examples.xml" path='examples/db[@name="DataProvider"]/*' />
		public DataProviderBase DataProvider
		{
			[System.Diagnostics.DebuggerStepThrough]
			get { return _dataProvider; }
		}

		private bool          _isExternalConnection;
		private bool          _closeConnection;
		private IDbConnection _connection;
		/// <summary>
		/// Gets or sets the <see cref="IDbConnection"/> used by this instance of the <see cref="DbManager"/>.
		/// </summary>
		/// <value>
		/// The connection to the data source.
		/// </value>
		/// <remarks>
		/// Then you set a connection object, it has to match the data source type.
		/// </remarks>
		/// <exception cref="DataException">
		/// A connection does not match the data source type.
		/// </exception>
		/// <include file="Examples.xml" path='examples/db[@name="Connection"]/*' />
		public IDbConnection Connection
		{
			[System.Diagnostics.DebuggerStepThrough]
			get
			{
				if (_connection == null)
					OpenConnection(_configurationString);
				return _connection;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				else if (_dataProvider != null)
				{
					if (value.GetType() == _dataProvider.ConnectionType)
					{
						_connection = value;
					}
					else
					{
						throw new DataException("The connection does not match the data provider type.");
					}
				}
				else
				{
					Init(value);
				}

				
				_isExternalConnection = true;
				_closeConnection      = false;
			}
		}

		private IDbCommand _selectCommand;
		/// <summary>
		/// Gets the <see cref="IDbCommand"/> used by this instance of the <see cref="DbManager"/>.
		/// </summary>
		/// <value>
		/// A <see cref="IDbCommand"/> used during executing query.
		/// </value>
		/// <remarks>
		/// The <b>Command</b> can be used to access command parameters.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="Command"]/*' />
		public IDbCommand Command
		{
			[System.Diagnostics.DebuggerStepThrough]
			get { return SelectCommand; }
		}

		/// <summary>
		/// Gets the select <see cref="IDbCommand"/> used by this instance of the <see cref="DbManager"/>.
		/// </summary>
		/// <value>
		/// A <see cref="IDbCommand"/> used during executing query.
		/// </value>
		/// <remarks>
		/// The <b>SelectCommand</b> can be used to access select command parameters.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="Command"]/*' />
		public IDbCommand SelectCommand
		{
			[System.Diagnostics.DebuggerStepThrough]
			get { return _selectCommand = InitCommand(_selectCommand); }
		}

		private IDbCommand _insertCommand;
		/// <summary>
		/// Gets the insert <see cref="IDbCommand"/> used by this instance of the <see cref="DbManager"/>.
		/// </summary>
		/// <value>
		/// A <see cref="IDbCommand"/> used during executing query.
		/// </value>
		/// <remarks>
		/// The <b>InsertCommand</b> can be used to access insert command parameters.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="Command"]/*' />
		public IDbCommand InsertCommand
		{
			[System.Diagnostics.DebuggerStepThrough]
			get { return _insertCommand = InitCommand(_insertCommand); }
		}

		private IDbCommand _updateCommand;
		/// <summary>
		/// Gets the update <see cref="IDbCommand"/> used by this instance of the <see cref="DbManager"/>.
		/// </summary>
		/// <value>
		/// A <see cref="IDbCommand"/> used during executing query.
		/// </value>
		/// <remarks>
		/// The <b>UpdateCommand</b> can be used to access update command parameters.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="Command"]/*' />
		public IDbCommand UpdateCommand
		{
			[System.Diagnostics.DebuggerStepThrough]
			get { return _updateCommand = InitCommand(_updateCommand); }
		}

		private IDbCommand _deleteCommand;
		/// <summary>
		/// Gets the delete <see cref="IDbCommand"/> used by this instance of the <see cref="DbManager"/>.
		/// </summary>
		/// <value>
		/// A <see cref="IDbCommand"/> used during executing query.
		/// </value>
		/// <remarks>
		/// The <b>DeleteCommand</b> can be used to access delete command parameters.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="Command"]/*' />
		public IDbCommand DeleteCommand
		{
			[System.Diagnostics.DebuggerStepThrough]
			get { return _deleteCommand = InitCommand(_deleteCommand); }
		}

		private bool           _closeTransaction = true;
		private IDbTransaction _transaction;
		/// <summary>
		/// Gets the <see cref="IDbTransaction"/> used by this instance of the <see cref="DbManager"/>.
		/// </summary>
		/// <value>
		/// The <see cref="IDbTransaction"/>. The default value is a null reference.
		/// </value>
		/// <remarks>
		/// You have to call the <see cref="BeginTransaction()"/> method to begin a transaction.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="Transaction"]/*' />
		/// <seealso cref="BeginTransaction()">BeginTransaction Method</seealso>
		public IDbTransaction Transaction
		{
			[System.Diagnostics.DebuggerStepThrough]
			get { return _transaction; }
		}

		private string _configurationString;
		/// <summary>
		/// Gets the string used to open a database.
		/// </summary>
		/// <value>
		/// A string containing configuration settings.
		/// </value>
		/// <remarks>
		/// <para>
		/// An actual database connection string is read from the <i>appSettings</i> section 
		/// of application configuration file (App.config, Web.config, or Machine.config) 
		/// according to the follow rule:
		/// </para>
		/// <code>
		/// &lt;appSettings&gt;
		///     &lt;add 
		///         key   = "ConnectionString.<b>configurationString</b>" 
		///         va<i></i>lue = "Server=(local);Database=Northwind;Integrated Security=SSPI" /&gt;
		/// &lt;/appSettings&gt;
		/// </code>
		/// <para>
		/// If the configuration string is empty, the following rule is applied:
		/// </para>
		/// <code>
		/// &lt;appSettings&gt;
		///     &lt;add 
		///         key   = "ConnectionString" 
		///         va<i></i>lue = "Server=(local);Database=Northwind;Integrated Security=SSPI" /&gt;
		/// &lt;/appSettings&gt;
		/// </code>
		/// <para>
		/// If you don't want to use a configuration file, you can add a database connection string 
		/// using the <see cref="AddConnectionString(string)"/> method.
		/// </para>
		/// <para>
		/// The configuration string may have a prefix used to define a data provider. The following table
		/// contains prefixes for all supported data providers:
		/// <list type="table">
		/// <listheader><term>Prefix</term><description>Provider</description></listheader>
		/// <item><term>Sql</term><description>Data Provider for SQL Server</description></item>
		/// <item><term>OleDb</term><description>Data Provider for OLE DB</description></item>
		/// <item><term>Odbc</term><description>Data Provider for ODBC</description></item>
		/// <item><term>Oracle</term><description>Data Provider for Oracle</description></item>
		/// </list>
		/// </para>
		/// </remarks>
		/// <seealso cref="AddConnectionString(string)">AddConnectionString Method</seealso>
		public string ConfigurationString
		{
			[System.Diagnostics.DebuggerStepThrough]
			get { return _configurationString; }
		}

		#endregion

		#region IDisposable interface

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="DbManager"/> and 
		/// optionally releases the managed resources.
		/// </summary>
		/// <remarks>
		/// This method is called by the public <see cref="IDisposable.Dispose()"/> method 
		/// and the Finalize method.
		/// </remarks>
		/// <param name="disposing"><b>true</b> to release both managed and unmanaged resources; <b>false</b> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
				Close();
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Closes the connection to the database.
		/// </summary>
		/// <remarks>
		/// The <b>Close</b> method rolls back any pending transactions
		/// and then closes the connection.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="Close()"]/*' />
		/// <seealso cref="Dispose(bool)">Dispose Method</seealso>
		public void Close()
		{
			if (_selectCommand != null) { _selectCommand.Dispose(); _selectCommand = null; }
			if (_insertCommand != null) { _insertCommand.Dispose(); _insertCommand = null; }
			if (_updateCommand != null) { _updateCommand.Dispose(); _updateCommand = null; }
			if (_deleteCommand != null) { _deleteCommand.Dispose(); _deleteCommand = null; }

			if (_transaction != null && _closeTransaction)
			{
				try
				{
					OnBeforeOperation(OperationType.DisposeTransaction);
					_transaction.Dispose();
					OnAfterOperation (OperationType.DisposeTransaction);
					_transaction = null;
				}
				catch (Exception ex)
				{
					OnOperationException(OperationType.DisposeTransaction, ex);
					throw;
				}
			}

			if (_connection != null && _closeConnection)
			{
				try
				{
					OnBeforeOperation(OperationType.CloseConnection);
					_connection.Dispose();
					OnAfterOperation (OperationType.CloseConnection);
					_connection = null;
				}
				catch (Exception ex)
				{
					OnOperationException(OperationType.CloseConnection, ex);
					throw;
				}
			}
		}

		/// <summary>
		/// Begins a database transaction.
		/// </summary>
		/// <remarks>
		/// Once the transaction has completed, you must explicitly commit or roll back the transaction
		/// by using the <see cref="System.Data.IDbTransaction.Commit">Commit</see> or 
		/// <see cref="System.Data.IDbTransaction.Rollback">Rollback</see> methods.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="BeginTransaction()"]/*' />
		/// <returns>This instance of the <see cref="DbManager"/>.</returns>
		/// <seealso cref="Transaction">Transaction Property</seealso>
		public DbManager BeginTransaction()
		{
			return BeginTransaction(IsolationLevel.ReadCommitted);
		}

		/// <summary>
		/// Begins a database transaction with the specified <see cref="IsolationLevel"/> value.
		/// </summary>
		/// <remarks>
		/// Once the transaction has completed, you must explicitly commit or roll back the transaction
		/// by using the <see cref="System.Data.IDbTransaction.Commit">Commit</see> or 
		/// <see cref="System.Data.IDbTransaction.Rollback">Rollback</see> methods.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="BeginTransaction(IsolationLevel)"]/*' />
		/// <param name="il">One of the <see cref="IsolationLevel"/> values.</param>
		/// <returns>This instance of the <see cref="DbManager"/>.</returns>
		public virtual DbManager BeginTransaction(IsolationLevel il)
		{
			// If transaction is open, we dispose it, it will rollback all changes.
			//
			if (_transaction != null)
			{
				try
				{
					OnBeforeOperation(OperationType.DisposeTransaction);
					_transaction.Dispose();
					OnAfterOperation (OperationType.DisposeTransaction);
				}
				catch (Exception ex)
				{
					OnOperationException(OperationType.DisposeTransaction, ex);
					throw;
				}
			}

			// Create new transaction object.
			//
			try
			{
				OnBeforeOperation(OperationType.BeginTransaction);
				_transaction      = Connection.BeginTransaction(il);
				OnAfterOperation (OperationType.BeginTransaction);
				_closeTransaction = true;
			}
			catch (Exception ex)
			{
				OnOperationException(OperationType.BeginTransaction, ex);
				throw;
			}

			// If the active command exists.
			//
			if (_selectCommand != null) _selectCommand.Transaction = _transaction;
			if (_insertCommand != null) _insertCommand.Transaction = _transaction;
			if (_updateCommand != null) _updateCommand.Transaction = _transaction;
			if (_deleteCommand != null) _deleteCommand.Transaction = _transaction;

			return this;
		}

		/// <summary>
		/// Commits the database transaction.
		/// </summary>
		/// <returns>This instance of the <see cref="DbManager"/>.</returns>
		public DbManager CommitTransaction()
		{
			if (_transaction != null)
			{
				try
				{
					OnBeforeOperation(OperationType.CommitTransaction);
					_transaction.Commit();
					OnAfterOperation (OperationType.CommitTransaction);
				}
				catch (Exception ex)
				{
					OnOperationException(OperationType.CommitTransaction, ex);
					throw;
				}

				if (_closeTransaction)
					_transaction = null;
			}

			return this;
		}

		/// <summary>
		/// Rolls back a transaction from a pending state.
		/// </summary>
		/// <returns>This instance of the <see cref="DbManager"/>.</returns>
		public DbManager RollbackTransaction()
		{
			if (_transaction != null)
			{
				try
				{
					OnBeforeOperation(OperationType.RollbackTransaction);
					_transaction.Rollback();
					OnAfterOperation (OperationType.RollbackTransaction);
				}
				catch (Exception ex)
				{
					OnOperationException(OperationType.RollbackTransaction, ex);
					throw;
				}

				if (_closeTransaction)
					_transaction = null;
			}

			return this;
		}

		#endregion

		#region Protected Methods

		static DbManager()
		{
			AddDataProvider(new SqlDataProvider());
			AddDataProvider(new AccessDataProvider());
			AddDataProvider(new OleDbDataProvider());
			AddDataProvider(new OdbcDataProvider());
			AddDataProvider(new SybaseAdoDataProvider());

			string dataProviders = ConfigManager.AppSettings.Get("BLToolkit.DataProviders");
			if (null != dataProviders)
			{
				foreach (string dataProviderTypeName in dataProviders.Split(';'))
					AddDataProvider(Type.GetType(dataProviderTypeName, true));
			}

			_defaultConfiguration = ConfigManager.AppSettings.Get("BLToolkit.DefaultConfiguration");
		}

		private static string           _firstConfiguration;
		private static DataProviderBase _firstProvider;
		private static Hashtable        _configurationList = Hashtable.Synchronized(new Hashtable());

		private void Init(string configurationString)
		{
			if (configurationString == null)
				configurationString = DefaultConfiguration;

			if (configurationString == _firstConfiguration)
			{
				_dataProvider = _firstProvider;
			}
			else
			{
				_dataProvider = (DataProviderBase)_configurationList[configurationString];

				if (_dataProvider == null)
				{
					_dataProvider = GetDataProvider(configurationString);
					_configurationList[configurationString] = _dataProvider;
				}

				if (_firstConfiguration == null)
				{
					lock (_configurationList.SyncRoot)
					{
						if (_firstConfiguration == null)
						{
							_firstConfiguration = configurationString;
							_firstProvider      = _dataProvider;
						}
					}
				}
			}

			_dataProvider.InitDbManager(this);
		}

		protected virtual string GetConnectionString(string configurationString)
		{
			// Use default configuration.
			//
			if (configurationString == null) 
				configurationString = DefaultConfiguration;

			// Check cached strings first.
			//
			object o = _connectionStringList[configurationString];

			// Connection string is not in cache.
			//
			if (o == null)
			{
				string key = string.Format(
					"ConnectionString{0}{1}",
					configurationString.Length == 0? "": ".",
					configurationString);

				o = ConfigManager.AppSettings.Get(key);

				if (o == null)
				{
					throw new DataException(string.Format(
						"The '{0}' key does not exist in the configuration file.", key));
				}

				// Store the result in cache.
				//
				_connectionStringList[configurationString] = o;
			}

			return o.ToString();
		}

		protected virtual string GetConnectionString(IDbConnection connection)
		{
			return connection.ConnectionString;
		}

		private static DataProviderBase GetDataProvider(string configurationString)
		{
			// configurationString can be:
			// ''        : default provider,   default configuration;
			// '.'       : default provider,   default configuration;
			// 'foo.bar' :   'foo' provider,     'bar' configuration;
			// 'foo.'    :   'foo' provider,   default configuration;
			// 'foo'     : default provider,     'foo' configuration;
			// '.foo'    : default provider,     'foo' configuration;
			// '.foo.bar': default provider, 'foo.bar' configuration;
			//
			// Default provider is SqlDataProvider
			//
			string cs  = configurationString.ToUpper();
			string key = "SQL";

			if (cs.Length > 0)
			{
				cs += ProviderNameDivider;

				foreach (string k in _dataProviderNameList.Keys)
				{
					if (cs.StartsWith(k + ProviderNameDivider))
					{
						key = k;
						break;
					}
				}
			}

			return (DataProviderBase)_dataProviderNameList[key];
		}

		private void Init(IDbConnection connection)
		{
			_dataProvider = (DataProviderBase)_dataProviderTypeList[connection.GetType()];

			if (_dataProvider != null)
			{
				Connection = connection;
			}
			else
			{
				throw new DataException(string.Format(
					"The '{0}' type of the connection could not be recognized.",
					connection.GetType().FullName));
			}

			_dataProvider.InitDbManager(this);
		}

		private void OpenConnection()
		{
			try
			{
				OnBeforeOperation(OperationType.OpenConnection);
				_connection.Open();
				OnAfterOperation (OperationType.OpenConnection);
			}
			catch (Exception ex)
			{
				OnOperationException(OperationType.OpenConnection, ex);
				throw;
			}

			_closeConnection = true;
		}

		private void OpenConnection(string configurationString)
		{
			// If connection is already opened, we close it and open again.
			//
			if (_connection != null)
			{
				Dispose();
				GC.ReRegisterForFinalize(this);
			}

			// Store the configuration string.
			//
			_configurationString = configurationString;

			// Create and open the connection object.
			//
			_connection = _dataProvider.CreateConnectionObject();
			_connection.ConnectionString = GetConnectionString(ConfigurationString);

			OpenConnection();
		}

		private IDbDataParameter[] CreateSpParameters(string spName, object[] parameterValues)
		{
			// Pull the parameters for this stored procedure from 
			// the parameter cache (or discover them & populate the cache)
			//
			IDbDataParameter[] commandParameters = GetSpParameters(spName, true);

			if (parameterValues == null || parameterValues.Length == 0)
				return commandParameters;

			if (commandParameters == null)
			{
				commandParameters = new IDbDataParameter[parameterValues.Length];

				if (parameterValues[0] is IDbDataParameter)
				{
					parameterValues.CopyTo(commandParameters, 0);
				}
				else
				{
					for (int i = 0; i < parameterValues.Length; i++)
						commandParameters[i] = Parameter("?", parameterValues[i]);
				}

				return commandParameters;
			}

			if (parameterValues[0] is IDbDataParameter)
			{
				// If we receive an array of IDbDataParameter, 
				// we need to copy parameters to the IDbDataParameter[].
				//
				for (int i = 0; i < commandParameters.Length; i++)
				{
					IDbDataParameter param = commandParameters[i];
					string           name  = param.ParameterName;
					bool             found = false;

					foreach (IDbDataParameter p in parameterValues)
					{
						if (string.Compare(name, p.ParameterName, true) == 0)
						{
							if (param.Direction != p.Direction)
							{
								System.Diagnostics.Debug.WriteLine(string.Format(
									"Stored Procedure '{0}'. Parameter '{1}' has different direction '{2}'. Should be '{3}'.",
										spName, name, param.Direction, p.Direction),
									typeof(DbManager).Namespace);

								param.Direction = p.Direction;
							}

							if (param.Direction != ParameterDirection.Output)
								param.Value = p.Value;

							p.ParameterName = name;

							found = true;

							break;
						}
					}

					if (found == false && (
						param.Direction == ParameterDirection.Input || 
						param.Direction == ParameterDirection.InputOutput))
					{
						System.Diagnostics.Debug.WriteLine(string.Format(
							"Stored Procedure '{0}'. Parameter '{1}' not assigned.", spName, name),
							typeof(DbManager).Namespace);

						param.SourceColumn = _dataProvider.Convert(name, ConvertType.ParameterToName).ToString();
					}
				}
			}
			else
			{
				// Assign the provided values to the parameters based on parameter order.
				//
				AssignParameterValues(commandParameters, parameterValues);
			}

			return commandParameters;
		}

		protected virtual IDbCommand InitCommand(IDbCommand command)
		{
			if (command == null) 
			{
				// Create a command object.
				//
				command = _dataProvider.CreateCommandObject(Connection);

				// If an active transaction exists.
				//
				if (Transaction != null)
				{
					command.Transaction = Transaction;
				}
			}

			return command;
		}

		/// <summary>
		/// Helper function. Creates the command object and sets command type and command text.
		/// </summary>
		/// <param name="commandAction">Command action.</param>
		/// <param name="commandType">The <see cref="System.Data.CommandType">CommandType</see> (stored procedure, text, etc.)</param>
		/// <param name="sql">The SQL statement.</param>
		/// <returns>The command object.</returns>
		private IDbCommand GetCommand(CommandAction commandAction, CommandType commandType, string sql)
		{
			IDbCommand command = GetCommand(commandAction);

			command.Parameters.Clear();
			command.CommandType = commandType;
			command.CommandText = sql;

			return command;
		}

		/// <summary>
		/// This method is used to attach array of IDbDataParameter to a IDbCommand.
		/// </summary>
		/// <param name="command">The command to which the parameters will be added</param>
		/// <param name="commandParameters">An array of IDbDataParameters tho be added to command</param>
		private void AttachParameters(IDbCommand command, IDbDataParameter[] commandParameters)
		{
			command.Parameters.Clear();

			foreach (IDbDataParameter p in commandParameters)
			{
				_dataProvider.AttachParameter(command, p);
			}
		}

		private static Hashtable _paramCache = Hashtable.Synchronized(new Hashtable());

		/// <summary>
		/// Resolve at run time the appropriate set of parameters for a stored procedure.
		/// </summary>
		/// <param name="spName">The name of the stored procedure.</param>
		/// <param name="includeReturnValueParameter">Whether or not to include their return value parameter.</param>
		/// <returns></returns>
		protected virtual IDbDataParameter[] DiscoverSpParameters(string spName, bool includeReturnValueParameter)
		{
			IDbConnection con;

			if (_isExternalConnection)
				con = _dataProvider.CloneConnection(_connection);
			else
			{
				con = _dataProvider.CreateConnectionObject();
				con.ConnectionString = GetConnectionString(ConfigurationString);
			}

			using (con)
			{
				try
				{
					OnBeforeOperation(OperationType.OpenConnection);
					con.Open();
					OnAfterOperation (OperationType.OpenConnection);
				}
				catch (Exception ex)
				{
					OnOperationException(OperationType.OpenConnection, ex);
					throw;
				}

				using (IDbCommand cmd = con.CreateCommand())
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.CommandText = spName;

					bool res;

					try
					{
						OnBeforeOperation(OperationType.DeriveParameters);
						res = _dataProvider.DeriveParameters(cmd);
						OnAfterOperation (OperationType.DeriveParameters);
					}
					catch (Exception ex)
					{
						OnOperationException(OperationType.DeriveParameters, ex);
						throw;
					}

					if (res == false)
						return null;

					if (includeReturnValueParameter == false)
					{
						cmd.Parameters.RemoveAt(0);
					}

					IDbDataParameter[] discoveredParameters = 
						new IDbDataParameter[cmd.Parameters.Count];

					cmd.Parameters.CopyTo(discoveredParameters, 0);

					return discoveredParameters;
				}
			}
		}

		/// <summary>
		/// Copies cached parameter array.
		/// </summary>
		/// <param name="originalParameters">The original parameter array.</param>
		/// <returns>The result array.</returns>
		private IDbDataParameter[] CloneParameters(IDbDataParameter[] originalParameters)
		{
			IDbDataParameter[] clonedParameters = new IDbDataParameter[originalParameters.Length];

			for (int i = 0, j = originalParameters.Length; i < j; i++)
			{
				clonedParameters[i] = _dataProvider.CloneParameter(originalParameters[i]);
			}

			return clonedParameters;
		}

		/// <summary>
		/// Retrieves the set of parameters appropriate for the stored procedure.
		/// </summary>
		/// <remarks>
		/// This method will query the database for this information, 
		/// and then store it in a cache for future requests.
		/// </remarks>
		/// <param name="spName">The name of the stored procedure.</param>
		/// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results.</param>
		/// <returns>An array of the <see cref="IDbDataParameter"/>.</returns>
		public IDbDataParameter[] GetSpParameters(string spName, bool includeReturnValueParameter)
		{
			string key = 
				string.Format("{0}:{1}:{2}", ConfigurationString, spName, includeReturnValueParameter);

			IDbDataParameter[] cachedParameters = (IDbDataParameter[])_paramCache[key];

			if (cachedParameters == null)
			{
				if (_paramCache.ContainsKey(key))
					return null;

				cachedParameters = DiscoverSpParameters(spName, includeReturnValueParameter);
				_paramCache[key] = cachedParameters;
			}
		
			return cachedParameters == null? null: CloneParameters(cachedParameters);
		}

		/// <summary>
		/// This method assigns an array of values to an array of parameters.
		/// </summary>
		/// <param name="commandParameters">array of IDbDataParameters to be assigned values</param>
		/// <param name="parameterValues">array of objects holding the values to be assigned</param>
		private void AssignParameterValues(IDbDataParameter[] commandParameters, object[] parameterValues)
		{
			if (commandParameters == null || parameterValues == null)
			{
				// Do nothing if we get no data.
				//
				return;
			}

			int nValues = 0;

			// Iterate through the parameters, assigning the values from 
			// the corresponding position in the value array.
			//
			for (int i = 0; i < commandParameters.Length; i++)
			{
				IDbDataParameter parameter = commandParameters[i];

				if (_dataProvider.IsValueParameter(parameter))
				{
					if (nValues >= parameterValues.Length)
						throw new ArgumentException("Parameter count does not match Parameter Value count.");

					object value = parameterValues[nValues++];

					parameter.Value = value == null ? DBNull.Value : value;
					//_dataProvider.SetParameterType(parameter, value);
				}
			}

			// We must have the same number of values as we pave parameters to put them in.
			//
			if (nValues != parameterValues.Length)
				throw new ArgumentException("Parameter count does not match Parameter Value count.");
		}

		private IDataReader ExecuteReaderInternal()
		{
			return ExecuteReader(CommandBehavior.Default);
		}

		private IDataReader ExecuteReaderInternal(CommandBehavior commandBehavior)
		{
			IDataReader dr = null;

			try
			{
				OnBeforeOperation(OperationType.ExecuteReader);
				dr = SelectCommand.ExecuteReader(commandBehavior);
				OnAfterOperation (OperationType.ExecuteReader);
			}
			catch (Exception ex)
			{
				if (dr != null)
					dr.Dispose();

				OnOperationException(OperationType.ExecuteReader, ex);
				throw;
			}

			return dr;
		}

		private int ExecuteNonQueryInternal()
		{
			try
			{
				OnBeforeOperation(OperationType.ExecuteNonQuery);
				int result = SelectCommand.ExecuteNonQuery();
				OnAfterOperation (OperationType.ExecuteNonQuery);

				return result;
			}
			catch (Exception ex)
			{
				OnOperationException(OperationType.ExecuteNonQuery, ex);
				throw;
			}
		}

		protected virtual void OnBeforeOperation(OperationType op)
		{
		}

		protected virtual void OnAfterOperation(OperationType op)
		{
		}

		protected virtual void OnOperationException(OperationType op, Exception ex)
		{
			throw new DataException(ex);
		}

		#endregion

		#region Public Static Methods

		private static Hashtable _dataProviderNameList = Hashtable.Synchronized(new Hashtable(4));
		private static Hashtable _dataProviderTypeList = Hashtable.Synchronized(new Hashtable(4));

		private const string ProviderNameDivider = ".";

		/// <summary>
		/// Adds a new data provider.
		/// </summary>
		/// <remarks>
		/// The method can be used to register a new data provider for further use.
		/// </remarks>
		/// <include file="Examples1.xml" path='examples/db[@name="AddDataProvider(DataProvider.IDataProvider)"]/*' />
		/// <seealso cref="AddConnectionString(string)">AddConnectionString Method.</seealso>
		/// <seealso cref="BLToolkit.Data.DataProvider.DataProviderBase.Name">DataProviderBase.Name Property.</seealso>
		/// <param name="dataProvider">An instance of the <see cref="BLToolkit.Data.DataProvider.DataProviderBase"/> interface.</param>
		public static void AddDataProvider(DataProviderBase dataProvider)
		{
			if (null == dataProvider)
				throw new ArgumentNullException("dataProvider");

			if (null == dataProvider.Name || 0 == dataProvider.Name.Length)
				throw new ArgumentException("dataProvider.Name must be a valid string");

			_dataProviderNameList[dataProvider.Name.ToUpper()] = dataProvider;
			_dataProviderTypeList[dataProvider.ConnectionType] = dataProvider;
		}

		/// <summary>
		/// Adds a new data provider witch a specified name.
		/// </summary>
		/// <remarks>
		/// The method can be used to register a new data provider for further use.
		/// </remarks>
		/// <include file="Examples1.xml" path='examples/db[@name="AddDataProvider(DataProvider.IDataProvider)"]/*' />
		/// <seealso cref="AddConnectionString(string)">AddConnectionString Method.</seealso>
		/// <seealso cref="BLToolkit.Data.DataProvider.DataProviderBase.Name">DataProviderBase.Name Property.</seealso>
		/// <param name="providerName">The data provider name.</param>
		/// <param name="dataProvider">An instance of the <see cref="BLToolkit.Data.DataProvider.DataProviderBase"/> interface.</param>
		public static void AddDataProvider(string providerName, DataProviderBase dataProvider)
		{
			if (null == dataProvider)
				throw new ArgumentNullException("dataProvider");

			if (null == providerName || 0 == providerName.Length)
				throw new ArgumentException("providerName must be a valid string");

			_dataProviderNameList[providerName.ToUpper()] = dataProvider;
			_dataProviderTypeList[dataProvider.ConnectionType] = dataProvider;
		}

		/// <summary>
		/// Adds a new data provider.
		/// </summary>
		/// <remarks>
		/// The method can be used to register a new data provider for further use.
		/// </remarks>
		/// <seealso cref="AddConnectionString(string)">AddConnectionString Method.</seealso>
		/// <seealso cref="BLToolkit.Data.DataProvider.DataProviderBase.Name">DataProviderBase.Name Property.</seealso>
		/// <param name="dataProviderType">A data provider type.</param>
		public static void AddDataProvider(Type dataProviderType)
		{
			AddDataProvider((DataProviderBase)Activator.CreateInstance(dataProviderType));
		}

		/// <summary>
		/// Adds a new data provider witch a specified name.
		/// </summary>
		/// <remarks>
		/// The method can be used to register a new data provider for further use.
		/// </remarks>
		/// <seealso cref="AddConnectionString(string)">AddConnectionString Method.</seealso>
		/// <seealso cref="BLToolkit.Data.DataProvider.DataProviderBase.Name">DataProviderBase.Name Property.</seealso>
		/// <param name="providerName">The data provider name.</param>
		/// <param name="dataProviderType">A data provider type.</param>
		public static void AddDataProvider(string providerName, Type dataProviderType)
		{
			AddDataProvider(providerName, (DataProviderBase)Activator.CreateInstance(dataProviderType));
		}

		/// <summary>
		/// Adds a new connection string or replaces existing one.
		/// </summary>
		/// <remarks>
		/// Use this method when you use only one configuration and 
		/// you don't want to use a configuration file.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="AddConnectionString(string)"]/*' />
		/// <param name="connectionString">A valid database connection string.</param>
		public static void AddConnectionString(string connectionString)
		{
			AddConnectionString(string.Empty, connectionString);
		}

		/// <summary>
		/// Adds a new connection string or replaces existing one.
		/// </summary>
		/// <remarks>
		/// Use this method when you use multiple configurations and 
		/// you don't want to use a configuration file.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="AddConnectionString(string,string)"]/*' />
		/// <param name="configurationString">The configuration string.</param>
		/// <param name="connectionString">A valid database connection string.</param>
		public static void AddConnectionString(string configurationString, string connectionString)
		{
			_connectionStringList[configurationString] = connectionString;
		}

		/// <summary>
		/// Adds a new connection string or replaces existing one.
		/// </summary>
		/// <remarks>
		/// Use this method when you use multiple configurations and 
		/// you don't want to use a configuration file.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="AddConnectionString(string,string)"]/*' />
		/// <param name="providerName">The data provider name.</param>
		/// <param name="configurationString">The configuration string.</param>
		/// <param name="connectionString">A valid database connection string.</param>
		public static void AddConnectionString(
			string providerName, string configurationString, string connectionString)
		{
			AddConnectionString(providerName + ProviderNameDivider + configurationString, connectionString);
		}

		#endregion

		#region Public Static Properties

		/// <summary>
		/// This table caches connection strings which were already read.
		/// </summary>
		private static Hashtable _connectionStringList = Hashtable.Synchronized(new Hashtable());
		private static string    _defaultConfiguration;

		/// <summary>
		/// Gets and sets the default configuration string.
		/// </summary>
		/// <remarks>
		/// See the <see cref="ConfigurationString"/> property 
		/// for an explanation and use of the default configuration.
		/// </remarks>
		/// <value>
		/// A string containing default configuration settings.
		/// </value>
		/// <seealso cref="ConfigurationString">ConfigurationString Property</seealso>
		public static string DefaultConfiguration
		{
			get 
			{
				if (_defaultConfiguration == null)
				{
					foreach (DictionaryEntry de in _connectionStringList)
					{
						_defaultConfiguration = de.Key.ToString();
						break;
					}

					if (_defaultConfiguration == null)
						_defaultConfiguration = string.Empty;
				}

				return _defaultConfiguration;
			}

			set 
			{
				_defaultConfiguration = value;
			}
		}

		#endregion

		#region Parameters

		private static Array SortArray(Array array, IComparer comparer)
		{
			if (array == null)
				return null;

			Array arrayClone = (Array)array.Clone();

			Array.Sort(arrayClone, comparer);
			return arrayClone;
		}

		/// <summary>
		/// Creates an array of parameters from the DataRow object.
		/// </summary>
		/// <remarks>
		/// The method can take an additional parameter list, 
		/// which can be created by using the same method.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="CreateParameters(DataRow,IDbDataParameter[])"]/*' />
		/// <param name="dataRow">The <see cref="DataRow"/> to create parameters.</param>
		/// <param name="commandParameters">An array of paramters to be added to the result array.</param>
		/// <returns>An array of parameters.</returns>
		public IDbDataParameter[] CreateParameters(
			DataRow dataRow, params IDbDataParameter[] commandParameters)
		{
			return CreateParameters(dataRow, null, null, null, commandParameters);
		}

		/// <summary>
		/// Creates an array of parameters from the DataRow object.
		/// </summary>
		/// <remarks>
		/// The method can take an additional parameter list, 
		/// which can be created by using the same method.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="CreateParameters(DataRow,IDbDataParameter[])"]/*' />
		/// <param name="dataRow">The <see cref="DataRow"/> to create parameters.</param>
		/// <param name="outputParameters">Output parameters names.</param>
		/// <param name="inputOutputParameters">InputOutput parameters names.</param>
		/// <param name="ignoreParameters">Parameters names to skip.</param>
		/// <param name="commandParameters">An array of paramters to be added to the result array.</param>
		/// <returns>An array of parameters.</returns>
		public IDbDataParameter[] CreateParameters(
			DataRow                   dataRow,
			string[]                  outputParameters,
			string[]                  inputOutputParameters,
			string[]                  ignoreParameters,
			params IDbDataParameter[] commandParameters)
		{
			ArrayList paramList = new ArrayList();
			IComparer comparer  = CaseInsensitiveComparer.Default;

			outputParameters      = (string[])SortArray(outputParameters,      comparer);
			inputOutputParameters = (string[])SortArray(inputOutputParameters, comparer);
			ignoreParameters      = (string[])SortArray(ignoreParameters,      comparer);

			foreach (DataColumn c in dataRow.Table.Columns)
			{
				if (ignoreParameters != null && Array.BinarySearch(ignoreParameters, c.ColumnName, comparer) >= 0)
					continue;

				if (c.AutoIncrement || c.ReadOnly)
					continue;

				string           name      = _dataProvider.Convert(c.ColumnName, ConvertType.NameToParameter).ToString();
				IDbDataParameter parameter = c.AllowDBNull?
					NullParameter(name, dataRow[c.ColumnName]):
					Parameter    (name, dataRow[c.ColumnName]);

				if (outputParameters != null && Array.BinarySearch(outputParameters, c.ColumnName, comparer) >= 0)
					parameter.Direction = ParameterDirection.Output;
				else if (inputOutputParameters != null && Array.BinarySearch(inputOutputParameters, c.ColumnName, comparer) >= 0)
					parameter.Direction = ParameterDirection.InputOutput;

				paramList.Add(parameter);
			}

			if (commandParameters != null)
				paramList.AddRange(commandParameters);

			return (IDbDataParameter[])paramList.ToArray(typeof(IDbDataParameter));
		}

		/// <summary>
		/// Creates an array of parameters from a business object.
		/// </summary>
		/// <remarks>
		/// The method can take an additional parameter list, 
		/// which can be created by using the same method.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="CreateParameters(object,IDbDataParameter[])"]/*' />
		/// <param name="obj">An object.</param>
		/// <param name="commandParameters">An array of paramters to be added to the result array.</param>
		/// <returns>An array of parameters.</returns>
		public IDbDataParameter[] CreateParameters(
			object                    obj,
			params IDbDataParameter[] commandParameters)
		{
			return CreateParameters(obj, null, null, null, commandParameters);
		}

		/// <summary>
		/// Creates an array of parameters from a business object.
		/// </summary>
		/// <remarks>
		/// The method can take an additional parameter list, 
		/// which can be created by using the same method.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="CreateParameters(object,IDbDataParameter[])"]/*' />
		/// <param name="obj">An object.</param>
		/// <param name="outputParameters">Output parameters names.</param>
		/// <param name="inputOutputParameters">InputOutput parameters names.</param>
		/// <param name="ignoreParameters">Parameters names to skip.</param>
		/// <param name="commandParameters">An array of paramters to be added to the result array.</param>
		/// <returns>An array of parameters.</returns>
		public IDbDataParameter[] CreateParameters(
			object                    obj,
			string[]                  outputParameters,
			string[]                  inputOutputParameters,
			string[]                  ignoreParameters,
			params IDbDataParameter[] commandParameters)
		{
			ObjectMapper om        = _mappingSchema.GetObjectMapper(obj.GetType());
			ArrayList    paramList = new ArrayList();
			IComparer    comparer  = CaseInsensitiveComparer.Default;

			outputParameters       = (string[])SortArray(outputParameters,      comparer);
			inputOutputParameters  = (string[])SortArray(inputOutputParameters, comparer);
			ignoreParameters       = (string[])SortArray(ignoreParameters,      comparer);

			foreach (MemberMapper mm in om)
			{
				if (ignoreParameters != null && Array.BinarySearch(ignoreParameters, mm.Name, comparer) >= 0)
					continue;
				
				object value = mm.GetValue(obj);
				string name  = _dataProvider.Convert(mm.Name, ConvertType.NameToParameter).ToString();

				IDbDataParameter parameter   = mm.MapMemberInfo.Nullable || value == null?
					NullParameter(name, value, mm.MapMemberInfo.NullValue):
					Parameter    (name, value);

				if (outputParameters != null && Array.BinarySearch(outputParameters, mm.Name, comparer) >= 0)
					parameter.Direction = ParameterDirection.Output;
				else if (inputOutputParameters != null && Array.BinarySearch(inputOutputParameters, mm.Name, comparer) >= 0)
					parameter.Direction = ParameterDirection.InputOutput;

				paramList.Add(parameter);
			}

			if (commandParameters != null)
				paramList.AddRange(commandParameters);

			return (IDbDataParameter[])paramList.ToArray(typeof(IDbDataParameter));
		}

		/// <summary>
		/// Maps all parameters returned from the server to all given objects.
		/// </summary>
		/// <param name="returnValueMember">Name of a <see cref="MemberMapper"/> to map return value.</param>
		/// <param name="obj">An <see cref="System.Object"/> to map from command parameters.</param>
		public void MapOutputParameters(
			string returnValueMember,
			object obj)
		{
			IMapDataDestination dest = _mappingSchema.GetDataDestination(obj);

			foreach (IDbDataParameter parameter in Command.Parameters)
			{
				int ordinal = -1;

				switch (parameter.Direction)
				{
					case ParameterDirection.InputOutput:
					case ParameterDirection.Output:
						ordinal = dest.GetOrdinal(
							_dataProvider.Convert(parameter.ParameterName, ConvertType.ParameterToName).ToString());
						break;

					case ParameterDirection.ReturnValue:

						if (returnValueMember != null)
						{
							if (!returnValueMember.StartsWith("@") && dest is ObjectMapper)
							{
								ObjectMapper   om = (ObjectMapper)dest;
								MemberAccessor ma = om.TypeAccessor[returnValueMember];

								if (ma != null)
								{
									ma.SetValue(obj, parameter.Value);
									continue;
								}
							}
							else
							{
								returnValueMember = returnValueMember.Substring(1);
							}

							ordinal = dest.GetOrdinal(returnValueMember);
						}

						break;
				}

				if (ordinal >= 0)
					dest.SetValue(obj, ordinal, parameter.Value);
			}
		}

		/// <summary>
		/// Maps all parameters returned from the server to an object.
		/// </summary>
		/// <param name="obj">An <see cref="System.Object"/> to map from command parameters.</param>
		/// from command parameters.</param>
		public void MapOutputParameters(object obj)
		{
			MapOutputParameters(null, obj);
		}

		/// <summary>
		/// Maps all parameters returned from the server to all given objects.
		/// </summary>
		/// <param name="returnValueMember">Name of the member used to map the
		/// return value. Can be null.</param>
		/// <param name="objects">An array of <see cref="System.Object"/> to map
		/// from command parameters.</param>
		public void MapOutputParameters(
			string returnValueMember,
			params object[] objects)
		{
			foreach (object obj in objects)
				MapOutputParameters(returnValueMember, obj);
		}

		/// <summary>
		/// Maps all parameters returned from the server to an object.
		/// </summary>
		/// <param name="objects">An array of <see cref="System.Object"/> to map
		/// from command parameters.</param>
		public void MapOutputParameters(params object[] objects)
		{
			MapOutputParameters(null, objects);
		}

		/// <overloads>
		/// Assigns a business object to command parameters.
		/// </overloads>
		/// <summary>
		/// Assigns the <see cref="DataRow"/> to command parameters.
		/// </summary>
		/// <include file="Examples1.xml" path='examples/db[@name="AssignParameterValues(DataRow)"]/*' />
		/// <remarks>
		/// The method is used in addition to the <see cref="CreateParameters(object,IDbDataParameter[])"/> method.
		/// </remarks>
		/// <param name="dataRow">The <see cref="DataRow"/> to assign.</param>
		/// <returns>This instance of the <see cref="DbManager"/>.</returns>
		public DbManager AssignParameterValues(DataRow dataRow)
		{
			foreach (DataColumn c in dataRow.Table.Columns)
			{
				if (c.AutoIncrement == false && c.ReadOnly == false)
				{
					object o    = dataRow[c.ColumnName];
					string name = _dataProvider.Convert(c.ColumnName, ConvertType.NameToParameter).ToString();

					Parameter(name).Value =
						c.AllowDBNull && _mappingSchema.IsNull(o)? DBNull.Value: o;
				}
			}

			return this;
		}

		/// <summary>
		/// Assigns a business object to command parameters.
		/// </summary>
		/// <remarks>
		/// The method is used in addition to the <see cref="CreateParameters(object,IDbDataParameter[])"/> method.
		/// </remarks>
		/// <include file="Examples1.xml" path='examples/db[@name="AssignParameterValues(object)"]/*' />
		/// <param name="obj">An object to assign.</param>
		/// <returns>This instance of the <see cref="DbManager"/>.</returns>
		public DbManager AssignParameterValues(object obj)
		{
			ObjectMapper om = _mappingSchema.GetObjectMapper(obj.GetType());

			foreach (MemberMapper mm in om)
			{
				string name  = _dataProvider.Convert(mm.Name, ConvertType.NameToParameter).ToString();

				if (Command.Parameters.Contains(name))
				{
					object value = mm.GetValue(obj);

					Parameter(name).Value =
						value == null || mm.MapMemberInfo.Nullable && _mappingSchema.IsNull(value)?
							DBNull.Value: value;
				}
			}

			return this;
		}

		/// <overloads>
		/// Adds a parameter to the <see cref="Command"/> or returns existing one.
		/// </overloads>
		/// <summary>
		/// Returns an existing parameter.
		/// </summary>
		/// <remarks>
		/// The method can be used to retrieve return and output parameters.
		/// </remarks>
		/// <include file="Examples1.xml" path='examples/db[@name="Parameter(string)"]/*' />
		/// <param name="parameterName">The name of the parameter.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter Parameter(string parameterName)
		{
			return _dataProvider.GetParameter(Command, parameterName);
		}

		/// <summary>
		/// Adds an input parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the
		/// <see cref="System.Data.ParameterDirection">ParameterDirection.Input</see> type.
		/// </remarks>
		/// <include file="Examples1.xml" path='examples/db[@name="Parameter(string,object)"]/*' />
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">The <see cref="System.Object">System.Object.</see>
		/// that is the value of the parameter.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter Parameter(string parameterName, object value)
		{
			return Parameter(ParameterDirection.Input, parameterName, value);
		}

		/// <summary>
		/// Adds an input parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the
		/// <see cref="System.Data.ParameterDirection">ParameterDirection.Input</see> type.
		/// </remarks>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="dbType">One of the <see cref="DbType"/> values.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter Parameter(string parameterName, DbType dbType)
		{
			return Parameter(ParameterDirection.Input, parameterName, dbType);
		}

		/// <summary>
		/// Adds an input parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the
		/// <see cref="System.Data.ParameterDirection">ParameterDirection.Input</see> type.
		/// </remarks>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="dbType">One of the <see cref="DbType"/> values.</param>
		/// <param name="size">Size of the parameter.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter Parameter(string parameterName, DbType dbType, int size)
		{
			return Parameter(ParameterDirection.Input, parameterName, dbType, size);
		}

		/// <summary>
		/// Adds an input parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the
		/// <see cref="System.Data.ParameterDirection">ParameterDirection.Input</see> type.
		/// If the parameter is null, it's converted to <see cref="DBNull.Value">DBNull.Value</see>.
		/// </remarks>
		/// <include file="Examples1.xml" path='examples/db[@name="NullParameter(string,object)"]/*' />
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">The <see cref="System.Object">System.Object.</see>
		/// that is the value of the parameter.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter NullParameter(string parameterName, object value)
		{
			if (_mappingSchema.IsNull(value))
				@value = DBNull.Value;

			return Parameter(ParameterDirection.Input, parameterName, value);
		}

		public IDbDataParameter NullParameter(string parameterName, object value, object nullValue)
		{
			if (value == null || value.Equals(nullValue))
				@value = DBNull.Value;

			return Parameter(ParameterDirection.Input, parameterName, value);
		}

		/// <summary>
		/// Adds an input parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the
		/// <see cref="System.Data.ParameterDirection">ParameterDirection.Input</see> type.
		/// </remarks>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">The <see cref="System.Object">System.Object.</see>
		/// that is the value of the parameter.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter InputParameter(string parameterName, object value)
		{
			return Parameter(ParameterDirection.Input, parameterName, value);
		}

		/// <summary>
		/// Adds an output parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the
		/// <see cref="System.Data.ParameterDirection">ParameterDirection.Output</see> type.
		/// </remarks>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">The <see cref="System.Object">System.Object.</see>
		/// that is the value of the parameter.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter OutputParameter(string parameterName, object value)
		{
			return Parameter(ParameterDirection.Output, parameterName, value);
		}

		/// <summary>
		/// Adds an output parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the
		/// <see cref="System.Data.ParameterDirection">ParameterDirection.Output</see> type.
		/// </remarks>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="dbType">One of the <see cref="DbType"/> values.</param>
		/// that is the value of the parameter.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter OutputParameter(string parameterName, DbType dbType)
		{
			return Parameter(ParameterDirection.Output, parameterName, dbType);
		}

		/// <summary>
		/// Adds an input-output parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the
		/// <see cref="System.Data.ParameterDirection">ParameterDirection.InputOutput</see> type.
		/// </remarks>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">The <see cref="System.Object">System.Object.</see>
		/// that is the value of the parameter.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter InputOutputParameter(string parameterName, object value)
		{
			return Parameter(ParameterDirection.InputOutput,parameterName, value);
		}

		/// <summary>
		/// Adds a return value parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the
		/// <see cref="System.Data.ParameterDirection">ParameterDirection.ReturnValue</see> type.
		/// </remarks>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter ReturnValue(string parameterName)
		{
			return Parameter(ParameterDirection.ReturnValue, parameterName, null);
		}

		/// <summary>
		/// Adds a parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the specified
		/// <see cref="System.Data.ParameterDirection">ParameterDirection</see> type.
		/// </remarks>
		/// <param name="parameterDirection">One of the <see cref="System.Data.ParameterDirection"/> values.</param>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">The <see cref="System.Object">System.Object.</see>
		/// that is the value of the parameter.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter Parameter(
			ParameterDirection parameterDirection,
			string parameterName,
			object value)
		{
			IDbDataParameter parameter = _dataProvider.CreateParameterObject(Command);

			parameter.ParameterName = parameterName;
			parameter.Direction     = parameterDirection;

			//_dataProvider.SetParameterType(parameter, value);

			parameter.Value = value != null? value: DBNull.Value;

			return parameter;
		}

		public IDbDataParameter Parameter(
			ParameterDirection parameterDirection,
			string parameterName,
			object value,
			DbType dbType)
		{
			IDbDataParameter parameter = _dataProvider.CreateParameterObject(Command);

			parameter.ParameterName = parameterName;
			parameter.Direction     = parameterDirection;
			parameter.DbType        = dbType;
			parameter.Value         = value;

			return parameter;
		}

		public IDbDataParameter Parameter(
			string parameterName,
			object value,
			DbType dbType)
		{
			return Parameter(ParameterDirection.Input, parameterName, value, dbType);
		}

		public IDbDataParameter Parameter(
			ParameterDirection parameterDirection,
			string parameterName,
			object value,
			DbType dbType,
			int    size)
		{
			IDbDataParameter parameter = _dataProvider.CreateParameterObject(Command);

			parameter.ParameterName = parameterName;
			parameter.Direction     = parameterDirection;
			parameter.DbType        = dbType;
			parameter.Size          = size;
			parameter.Value         = value;

			return parameter;
		}

		public IDbDataParameter Parameter(
			string parameterName,
			object value,
			DbType dbType,
			int    size)
		{
			return Parameter(ParameterDirection.Input, parameterName, value, dbType, size);
		}

		/// <summary>
		/// Adds a parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the specified
		/// <see cref="System.Data.ParameterDirection">ParameterDirection</see> type.
		/// </remarks>
		/// <param name="parameterDirection">One of the <see cref="System.Data.ParameterDirection"/> values.</param>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="dbType">One of the <seealso cref="DbType"/> values.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter Parameter(
			ParameterDirection parameterDirection,
			string parameterName,
			DbType dbType)
		{
			IDbDataParameter parameter = _dataProvider.CreateParameterObject(Command);
		
			parameter.ParameterName = parameterName;
			parameter.Direction     = parameterDirection;
			parameter.DbType        = dbType;

			return parameter;
		}

		/// <summary>
		/// Adds a parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the specified
		/// <see cref="System.Data.ParameterDirection">ParameterDirection</see> type.
		/// </remarks>
		/// <param name="parameterDirection">One of the <see cref="System.Data.ParameterDirection"/> values.</param>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="dbType">One of the <seealso cref="DbType"/> values.</param>
		/// <param name="size">Size of the parameter.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter Parameter(
			ParameterDirection parameterDirection,
			string parameterName,
			DbType dbType,
			int    size)
		{
			IDbDataParameter parameter = _dataProvider.CreateParameterObject(Command);
		
			parameter.ParameterName = parameterName;
			parameter.Direction     = parameterDirection;
			parameter.DbType        = dbType;
			parameter.Size          = size;

			return parameter;
		}

		/// <summary>
		/// Cretes an input parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the
		/// <see cref="System.Data.ParameterDirection">ParameterDirection.Input</see> type
		/// and <see cref="System.Data.DataRowVersion">DataRowVersion.Current</see> type.
		/// </remarks>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="dbType">One of the <see cref="DbType"/> values.</param>
		/// <param name="size">Size of the parameter.</param>
		/// <param name="sourceColumn">Source column for a parameter in DataTable.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter Parameter(
			string parameterName,
			DbType dbType,
			int    size,
			string sourceColumn)
		{
			IDbDataParameter param = Parameter(ParameterDirection.Input, parameterName, dbType, size);
			
			param.SourceColumn  = sourceColumn;
			param.SourceVersion = DataRowVersion.Current;

			return param;
		}

		/// <summary>
		/// Cretes an input parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the
		/// <see cref="System.Data.ParameterDirection">ParameterDirection.Input</see> type
		/// and <see cref="System.Data.DataRowVersion">DataRowVersion.Current</see> type.
		/// </remarks>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="dbType">One of the <see cref="DbType"/> values.</param>
		/// <param name="sourceColumn">Source column for a parameter in DataTable.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter Parameter(
			string parameterName,
			DbType dbType,
			string sourceColumn)
		{
			IDbDataParameter param = Parameter(ParameterDirection.Input, parameterName, dbType);

			param.SourceColumn  = sourceColumn;
			param.SourceVersion = DataRowVersion.Current;

			return param;
		}

		/// <summary>
		/// Cretes an input parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the
		/// <see cref="System.Data.ParameterDirection">ParameterDirection.Input</see> type
		/// and <see cref="System.Data.DataRowVersion">DataRowVersion.Current</see> type.
		/// </remarks>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="dbType">One of the <see cref="DbType"/> values.</param>
		/// <param name="size">Size of the parameter.</param>
		/// <param name="sourceColumn">Source column for a parameter in DataTable.</param>
		/// <param name="dataRowVersion">Version of data to use for a parameter in DataTable.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter Parameter(
			string parameterName,
			DbType dbType,
			int    size,
			string sourceColumn,
			DataRowVersion dataRowVersion)
		{
			IDbDataParameter param = Parameter(ParameterDirection.Input, parameterName, dbType, size);

			param.SourceColumn  = sourceColumn;
			param.SourceVersion = dataRowVersion;

			return param;
		}

		/// <summary>
		/// Cretes an input parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the
		/// <see cref="System.Data.ParameterDirection">ParameterDirection.Input</see> type
		/// and <see cref="System.Data.DataRowVersion">DataRowVersion.Current</see> type.
		/// </remarks>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="dbType">One of the <see cref="DbType"/> values.</param>
		/// <param name="sourceColumn">Source column for a parameter in DataTable.</param>
		/// <param name="dataRowVersion">Version of data to use for a parameter in DataTable.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter Parameter(
			string parameterName,
			DbType dbType,
			string sourceColumn,
			DataRowVersion dataRowVersion)
		{
			IDbDataParameter param = Parameter(ParameterDirection.Input, parameterName, dbType);

			param.SourceColumn  = sourceColumn;
			param.SourceVersion = dataRowVersion;

			return param;
		}

		#endregion

		#region SetCommand

		/// <summary>
		/// Specifies the action that command is supposed to perform, i.e. Select, Insert, Update, Delete.
		/// It is used in Execute methods of the <see cref="DbManager"/> class to identify command instance 
		/// to be used.
		/// </summary>
		private enum CommandAction
		{
			Select,
			Insert,
			Update,
			Delete
		}

		private bool _executed;
		private bool _prepared;

		private IDbDataParameter[] _selectCommandParameters;
		private IDbDataParameter[] _insertCommandParameters;
		private IDbDataParameter[] _updateCommandParameters;
		private IDbDataParameter[] _deleteCommandParameters;

		private void SetCommand(CommandAction commandAction, IDbCommand command)
		{
			switch (commandAction)
			{
				case CommandAction.Select: _selectCommand = command; break;
				case CommandAction.Insert: _insertCommand = command; break;
				case CommandAction.Update: _updateCommand = command; break;
				case CommandAction.Delete: _deleteCommand = command; break;
			}
		}

		private IDbCommand GetCommand(CommandAction commandAction)
		{
			switch (commandAction)
			{
				default:
				case CommandAction.Select: return SelectCommand;
				case CommandAction.Insert: return InsertCommand;
				case CommandAction.Update: return UpdateCommand;
				case CommandAction.Delete: return DeleteCommand;
			}
		}

		private void SetCommandParameters(CommandAction commandAction, IDbDataParameter[] commandParameters)
		{
			switch (commandAction)
			{
				case CommandAction.Select: _selectCommandParameters = commandParameters; break;
				case CommandAction.Insert: _insertCommandParameters = commandParameters; break;
				case CommandAction.Update: _updateCommandParameters = commandParameters; break;
				case CommandAction.Delete: _deleteCommandParameters = commandParameters; break;
			}
		}

		private IDbDataParameter[] GetCommandParameters(CommandAction commandAction)
		{
			switch (commandAction)
			{
				default:
				case CommandAction.Select: return _selectCommandParameters;
				case CommandAction.Insert: return _insertCommandParameters;
				case CommandAction.Update: return _updateCommandParameters;
				case CommandAction.Delete: return _deleteCommandParameters;
			}
		}

		private DbManager SetCommand(
			CommandAction commandAction,
			CommandType   commandType,
			string        commandText,
			params        IDbDataParameter[] commandParameters)
		{
			if (_executed)
			{
				_executed = false;
				_prepared = false;
			}

			PrepareCommand(commandAction, commandType, commandText, commandParameters);
			
			return this;
		}

		private DbManager SetSpCommand(
			CommandAction   commandAction,
			string          spName,
			params object[] parameterValues)
		{
			// http://rsdn.ru/?forum/message.1613538.aspx
			//

			//// If we receive parameter values, we need to figure out where they go.
			////
			//if (parameterValues != null && parameterValues.Length > 0)
			//{
				return SetCommand(
					commandAction,
					CommandType.StoredProcedure,
					spName,
					CreateSpParameters(spName, parameterValues));
			//}
			//// Otherwise we can just call the SP without params.
			////
			//else 
			//{
			//    return SetCommand(
			//        commandAction,
			//        CommandType.StoredProcedure,
			//        spName,
			//        (IDbDataParameter[])null);
			//}
		}

		#region Select

		/// <summary>
		/// Creates a SQL statement.
		/// </summary>
		/// <param name="commandText">The command text to execute.</param>
		/// <returns>Current instance.</returns>
		public DbManager SetCommand(
			string commandText)
		{
			return SetCommand(
				CommandAction.Select, CommandType.Text, commandText, (IDbDataParameter[])null);
		}

		/// <summary>
		/// Creates a SQL statement.
		/// </summary>
		/// <param name="commandType">The <see cref="System.Data.CommandType">CommandType</see> (stored procedure, text, etc.)</param>
		/// <param name="commandText">The command text to execute.</param>
		/// <returns>Current instance.</returns>
		public DbManager SetCommand(
			CommandType commandType,
			string      commandText)
		{
			return SetCommand(
				CommandAction.Select, commandType, commandText, (IDbDataParameter[])null);
		}

		/// <summary>
		/// Creates a SQL statement.
		/// </summary>
		/// <remarks>
		/// The method can be used to create the <i>INSERT</i>, <i>UPDATE</i>, and <i>DELETE</i> SQL statements.
		/// </remarks>
		/// <param name="commandText">The command text to execute.</param>
		/// <param name="commandParameters">An array of paramters used to executes the command.</param>
		/// <returns>Current instance.</returns>
		public DbManager SetCommand(
			string commandText,
			params IDbDataParameter[] commandParameters)
		{
			return SetCommand(
				CommandAction.Select, CommandType.Text, commandText, commandParameters);
		}

		/// <summary>
		/// Creates a SQL statement.
		/// </summary>
		/// <param name="commandType">The <see cref="System.Data.CommandType">CommandType</see> (stored procedure, text, etc.)</param>
		/// <param name="commandText">The command text to execute.</param>
		/// <param name="commandParameters">An array of paramters used to executes the command.</param>
		/// <returns>Current instance.</returns>
		public DbManager SetCommand(
			CommandType commandType,
			string      commandText,
			params      IDbDataParameter[] commandParameters)
		{
			return SetCommand(
				CommandAction.Select, commandType, commandText, commandParameters);
		}

		/// <summary>
		/// Creates a command to be executed as a stored procedure using the provided parameter values.
		/// </summary>
		/// <remarks>
		/// The method queries the database to discover the parameters for the stored procedure 
		/// (the first time each stored procedure is called), 
		/// and assign the values based on parameter order.
		/// </remarks>
		/// <param name="spName">The name of the stored prcedure</param>
		/// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>Current instance.</returns>
		public DbManager SetSpCommand(
			string spName,
			params object[] parameterValues)
		{
			return SetSpCommand(CommandAction.Select, spName, parameterValues);
		}

		#endregion

		#region Insert

		/// <summary>
		/// Creates an Insert SQL statement.
		/// </summary>
		/// <remarks>
		/// The method can be used to create the <i>INSERT</i>, <i>UPDATE</i>, and <i>DELETE</i> SQL statements.
		/// </remarks>
		/// <param name="commandText">The command text to execute.</param>
		/// <param name="commandParameters">An array of paramters used to executes the command.</param>
		/// <returns>Current instance.</returns>
		public DbManager SetInsertCommand(
			string commandText,
			params IDbDataParameter[] commandParameters)
		{
			return SetCommand(
				CommandAction.Insert, CommandType.Text, commandText, commandParameters);
		}

		/// <summary>
		/// Creates an Insert SQL statement.
		/// </summary>
		/// <param name="commandType">The <see cref="System.Data.CommandType">CommandType</see> (stored procedure, text, etc.)</param>
		/// <param name="commandText">The command text to execute.</param>
		/// <param name="commandParameters">An array of paramters used to executes the command.</param>
		/// <returns>Current instance.</returns>
		public DbManager SetInsertCommand(
			CommandType commandType,
			string      commandText,
			params      IDbDataParameter[] commandParameters)
		{
			return SetCommand(
				CommandAction.Insert, commandType, commandText, commandParameters);
		}

		/// <summary>
		/// Creates an Insert command to be executed as a stored procedure using the provided parameter values.
		/// </summary>
		/// <remarks>
		/// The method queries the database to discover the parameters for the stored procedure 
		/// (the first time each stored procedure is called), 
		/// and assign the values based on parameter order.
		/// </remarks>
		/// <param name="spName">The name of the stored prcedure</param>
		/// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>Current instance.</returns>
		public DbManager SetInsertSpCommand(
			string spName,
			params object[] parameterValues)
		{
			return SetSpCommand(CommandAction.Insert, spName, parameterValues);
		}

		#endregion

		#region Update

		/// <summary>
		/// Creates an Update SQL statement.
		/// </summary>
		/// <remarks>
		/// The method can be used to create the <i>INSERT</i>, <i>UPDATE</i>, and <i>DELETE</i> SQL statements.
		/// </remarks>
		/// <param name="commandText">The command text to execute.</param>
		/// <param name="commandParameters">An array of paramters used to executes the command.</param>
		/// <returns>Current instance.</returns>
		public DbManager SetUpdateCommand(
			string commandText,
			params IDbDataParameter[] commandParameters)
		{
			return SetCommand(
				CommandAction.Update, CommandType.Text, commandText, commandParameters);
		}

		/// <summary>
		/// Creates an Update SQL statement.
		/// </summary>
		/// <param name="commandType">The <see cref="System.Data.CommandType">CommandType</see> (stored procedure, text, etc.)</param>
		/// <param name="commandText">The command text to execute.</param>
		/// <param name="commandParameters">An array of paramters used to executes the command.</param>
		/// <returns>Current instance.</returns>
		public DbManager SetUpdateCommand(
			CommandType commandType,
			string      commandText,
			params      IDbDataParameter[] commandParameters)
		{
			return SetCommand(
				CommandAction.Update, commandType, commandText, commandParameters);
		}

		/// <summary>
		/// Creates an Update command to be executed as a stored procedure using the provided parameter values.
		/// </summary>
		/// <remarks>
		/// The method queries the database to discover the parameters for the stored procedure 
		/// (the first time each stored procedure is called), 
		/// and assign the values based on parameter order.
		/// </remarks>
		/// <param name="spName">The name of the stored prcedure</param>
		/// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>Current instance.</returns>
		public DbManager SetUpdateSpCommand(
			string spName,
			params object[] parameterValues)
		{
			return SetSpCommand(CommandAction.Update, spName, parameterValues);
		}

		#endregion

		#region Delete

		/// <summary>
		/// Creates a Delete SQL statement.
		/// </summary>
		/// <remarks>
		/// The method can be used to create the <i>INSERT</i>, <i>UPDATE</i>, and <i>DELETE</i> SQL statements.
		/// </remarks>
		/// <param name="commandText">The command text to execute.</param>
		/// <param name="commandParameters">An array of paramters used to executes the command.</param>
		/// <returns>Current instance.</returns>
		public DbManager SetDeleteCommand(
			string commandText,
			params IDbDataParameter[] commandParameters)
		{
			return SetCommand(
				CommandAction.Delete, CommandType.Text, commandText, commandParameters);
		}

		/// <summary>
		/// Creates a Delete SQL statement.
		/// </summary>
		/// <param name="commandType">The <see cref="System.Data.CommandType">CommandType</see> (stored procedure, text, etc.)</param>
		/// <param name="commandText">The command text to execute.</param>
		/// <param name="commandParameters">An array of paramters used to executes the command.</param>
		/// <returns>Current instance.</returns>
		public DbManager SetDeleteCommand(
			CommandType commandType,
			string      commandText,
			params      IDbDataParameter[] commandParameters)
		{
			return SetCommand(
				CommandAction.Delete, commandType, commandText, commandParameters);
		}

		/// <summary>
		/// Creates a Delete command to be executed as a stored procedure using the provided parameter values.
		/// </summary>
		/// <remarks>
		/// The method queries the database to discover the parameters for the stored procedure 
		/// (the first time each stored procedure is called), 
		/// and assign the values based on parameter order.
		/// </remarks>
		/// <param name="spName">The name of the stored prcedure</param>
		/// <param name="parameterValues">An array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>Current instance.</returns>
		public DbManager SetDeleteSpCommand(
			string spName,
			params object[] parameterValues)
		{
			return SetSpCommand(CommandAction.Delete, spName, parameterValues);
		}

		#endregion

		#endregion

		#region Prepare

		private IDbCommand PrepareCommand(
			CommandAction      commandAction,
			CommandType        commandType,
			string             commandText,
			IDbDataParameter[] commandParameters)
		{
			DataProvider.PrepareCommand(ref commandType, ref commandText, ref commandParameters);

			IDbCommand command = GetCommand(commandAction, commandType, commandText);

			SetCommand          (commandAction, command);
			SetCommandParameters(commandAction, commandParameters);

			if (commandParameters != null)
			{
				AttachParameters(command, commandParameters);
			}

			return command;
		}

		/// <summary>
		/// Prepares a command for execution.
		/// </summary>
		/// <returns>Current instance.</returns>
		public DbManager Prepare()
		{
			IDbCommand command = GetCommand(CommandAction.Select);

			if (InitParameters(CommandAction.Select) == false)
			{
				try
				{
					OnBeforeOperation(OperationType.PrepareCommand);
					command.Prepare();
					OnAfterOperation (OperationType.PrepareCommand);
				}
				catch (Exception ex)
				{
					OnOperationException(OperationType.PrepareCommand, ex);
					throw;
				}
			}

			_prepared = true;

			return this;
		}

		private bool InitParameters(CommandAction commandAction)
		{
			bool prepare = false;

			IDbDataParameter[] commandParameters = GetCommandParameters(commandAction);

			if (commandParameters != null)
			{
				foreach (IDbDataParameter p in commandParameters)
				{
					p.DbType = p.DbType;
			
					if (p.Value is string)
					{
						int len = ((string)p.Value).Length;

						if (p.Size < len)
						{
							p.Size  = len;
							prepare = true;
						}
					}
					else if (p.Value is DBNull)
					{
						p.Size = 1;
					}
					else if (p.Value is byte[])
					{
						int len = ((byte[])p.Value).Length;

						if (p.Size < len)
						{
							p.Size  = len;
							prepare = true;
						}
					}
				}

				if (prepare)
				{
					IDbCommand command = GetCommand(commandAction);

					AttachParameters(command, commandParameters);
					command.Prepare();
				}
			}

			return prepare;
		}

		#endregion

		#region ExecuteForEach

		/// <summary>
		/// Executes a SQL statement for a given collection of objects and 
		/// returns the number of rows affected.
		/// </summary>
		/// <remarks>
		/// The method prepares the <see cref="Command"/> object 
		/// and calls the <see cref="ExecuteNonQuery()"/> method for each item of the list.
		/// </remarks>
		/// <include file="Examples1.xml" path='examples/db[@name="Execute(CommandType,string,IList)"]/*' />
		/// <param name="collection">The list of objects used to execute the command.</param>
		/// <returns>The number of rows affected by the command.</returns>
		public int ExecuteForEach(ICollection collection)
		{
			int rows = 0;

			if (collection != null && collection.Count != 0)
			{
				bool initParameters = true;

				foreach (object o in collection)
				{
					if (initParameters)
					{
						initParameters = false;

						IDbDataParameter[] parameters = GetCommandParameters(CommandAction.Select);

						if (parameters == null || parameters.Length == 0)
						{
							parameters = CreateParameters(o);

							SetCommandParameters(CommandAction.Select, parameters);
							AttachParameters(SelectCommand, parameters);
							Prepare();
						}
					}

					AssignParameterValues(o);
					rows += ExecuteNonQueryInternal();
				}
			}
		
			return rows;
		}

		/// <summary>
		/// Executes a SQL statement for the <see cref="DataTable"/> and 
		/// returns the number of rows affected.
		/// </summary>
		/// <remarks>
		/// The method prepares the <see cref="Command"/> object 
		/// and calls the <see cref="ExecuteNonQuery()"/> method for each item 
		/// of the <see cref="DataTable"/>.
		/// </remarks>
		/// <include file="Examples1.xml" path='examples/db[@name="Execute(CommandType,string,DataTable)"]/*' />
		/// <param name="table">An instance of the <see cref="DataTable"/> class to execute the command.</param>
		/// <returns>The number of rows affected by the command.</returns>
		public int ExecuteForEach(DataTable table)
		{
			int rows = 0;

			if (table != null && table.Rows.Count != 0)
			{
				IDbDataParameter[] parameters = GetCommandParameters(CommandAction.Select);

				if (parameters == null || parameters.Length == 0)
				{
					parameters = CreateParameters(table.Rows[0]);

					SetCommandParameters(CommandAction.Select, parameters);
					AttachParameters(SelectCommand, parameters);
					Prepare();
				}

				foreach (DataRow dr in table.Rows)
				{
					AssignParameterValues(dr);
					rows += ExecuteNonQueryInternal();
				}
			}
		
			return rows;
		}

		/// <summary>
		/// Executes a SQL statement for the first table of the <see cref="DataSet"/> 
		/// and returns the number of rows affected.
		/// </summary>
		/// <remarks>
		/// The method prepares the <see cref="Command"/> object
		/// and calls the <see cref="ExecuteNonQuery()"/> method for each item of the first table.
		/// </remarks>
		/// <include file="Examples1.xml" path='examples/db[@name="Execute(CommandType,string,DataSet)"]/*' />
		/// <param name="dataSet">An instance of the <see cref="DataSet"/> class to execute the command.</param>
		/// <returns>The number of rows affected by the command.</returns>
		public int ExecuteForEach(DataSet dataSet)
		{
			return ExecuteForEach(dataSet.Tables[0]);
		}

		/// <summary>
		/// Executes a SQL statement for the specified table of the <see cref="DataSet"/> 
		/// and returns the number of rows affected.
		/// </summary>
		/// <remarks>
		/// The method prepares the <see cref="Command"/> object
		/// and calls the <see cref="ExecuteNonQuery()"/> method for each item of the first table.
		/// </remarks>
		/// <include file="Examples1.xml" path='examples/db[@name="Execute(CommandType,string,DataSet,string)"]/*' />
		/// <param name="dataSet">An instance of the <see cref="DataSet"/> class to execute the command.</param>
		/// <param name="nameOrIndex">The table name or index.
		/// name/index.</param>
		/// <returns>The number of rows affected by the command.</returns>
		public int ExecuteForEach(DataSet dataSet, NameOrIndexParameter nameOrIndex)
		{
			return nameOrIndex.ByName ? ExecuteForEach(dataSet.Tables[nameOrIndex.Name])
				: ExecuteForEach(dataSet.Tables[nameOrIndex.Index]);
		}

		#endregion

		#region ExecuteNonQuery

		/// <summary>
		/// Executes a SQL statement and returns the number of rows affected.
		/// </summary>
		/// <remarks>
		/// The method can be used to execute the <i>INSERT</i>, <i>UPDATE</i>, and <i>DELETE</i> SQL statements.
		/// </remarks>
		/// <include file="Examples1.xml" path='examples/db[@name="ExecuteNonQuery()"]/*' />
		/// <returns>The number of rows affected by the command.</returns>
		public int ExecuteNonQuery()
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			return ExecuteNonQueryInternal();
		}

		/// <summary>
		/// Executes a SQL statement and returns the number of rows affected.
		/// </summary>
		/// <remarks>
		/// The method can be used to execute the <i>INSERT</i>, <i>UPDATE</i>, and <i>DELETE</i> SQL statements.
		/// </remarks>
		/// <param name="returnValueMember">Name of a <see cref="MemberMapper"/> to map return value.</param>
		/// <param name="obj">An <see cref="System.Object"/> to map from command parameters.</param>
		/// <returns>The number of rows affected by the command.</returns>
		public int ExecuteNonQuery(
			string returnValueMember,
			object obj)
		{
			int rowsAffected = ExecuteNonQuery();

			MapOutputParameters(returnValueMember, obj);
			return rowsAffected;
		}

		/// <summary>
		/// Executes a SQL statement and returns the number of rows affected.
		/// </summary>
		/// <remarks>
		/// The method can be used to execute the <i>INSERT</i>, <i>UPDATE</i>, and <i>DELETE</i> SQL statements.
		/// </remarks>
		/// <param name="obj">An <see cref="System.Object"/> to map from command parameters.</param>
		/// <returns>The number of rows affected by the command.</returns>
		public int ExecuteNonQuery(object obj)
		{
			int rowsAffected = ExecuteNonQuery();

			MapOutputParameters(null, obj);
			return rowsAffected;
		}

		/// <summary>
		/// Executes a SQL statement and returns the number of rows affected.
		/// </summary>
		/// <remarks>
		/// The method can be used to execute the <i>INSERT</i>, <i>UPDATE</i>, and <i>DELETE</i> SQL statements.
		/// </remarks>
		/// <param name="returnValueMember">Name of a <see cref="MemberMapper"/> to map return value.</param>
		/// <param name="objects">An array of <see cref="System.Object"/> to map
		/// from command parameters.</param>
		/// <returns>The number of rows affected by the command.</returns>
		public int ExecuteNonQuery(
			string          returnValueMember,
			params object[] objects)
		{
			int rowsAffected = ExecuteNonQuery();

			MapOutputParameters(returnValueMember, objects);
			return rowsAffected;
		}

		/// <summary>
		/// Executes a SQL statement and returns the number of rows affected.
		/// </summary>
		/// <remarks>
		/// The method can be used to execute the <i>INSERT</i>, <i>UPDATE</i>, and <i>DELETE</i> SQL statements.
		/// </remarks>
		/// <param name="objects">An array of <see cref="System.Object"/> to map
		/// from command parameters.</param>
		/// <returns>The number of rows affected by the command.</returns>
		public int ExecuteNonQuery(params object[] objects)
		{
			int rowsAffected = ExecuteNonQuery();

			MapOutputParameters(null, objects);
			return rowsAffected;
		}

		#endregion

		#region ExecuteScalar

		/// <summary>
		/// Executes the query, and returns the first column of the first row
		/// in the resultset returned by the query. Extra columns or rows are
		/// ignored.
		/// </summary>
		/// <returns>The first column of the first row in the resultset.</returns>
		/// <seealso cref="ExecuteScalar(ScalarSourceType, NameOrIndexParameter)"/>
		public object ExecuteScalar()
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			try
			{
				OnBeforeOperation(OperationType.ExecuteScalar);
				object result = SelectCommand.ExecuteScalar();
				OnAfterOperation (OperationType.ExecuteScalar);

				return result;
			}
			catch (Exception ex)
			{
				OnOperationException(OperationType.ExecuteScalar, ex);
				throw;
			}
		}

		/// <summary>
		/// Executes the query, and returns the value with specified scalar
		/// source type.
		/// </summary>
		/// <param name="sourceType">The method used to return the scalar
		/// value.</param>
		/// <returns><list type="table">
		/// <listheader>
		///  <term>ScalarSourceType</term>
		///  <description>Return value</description>
		/// </listheader>
		/// <item>
		///  <term>DataReader</term>
		///  <description>The first column of the first row in the resultset.
		///  </description>
		/// </item>
		/// <item>
		///  <term>OutputParameter</term>
		///  <description>The value of the first output or input/output
		///  parameter returned.</description>
		/// </item>
		/// <item>
		///  <term>ReturnValue</term>
		///  <description>The value of the "return value" parameter returned.
		///  </description>
		/// </item>
		/// <item>
		///  <term>AffectedRows</term>
		///  <description>The number of rows affected.</description>
		/// </item>
		/// </list>
		/// </returns>
		/// <seealso cref="ExecuteScalar(ScalarSourceType, NameOrIndexParameter)"/>
		public object ExecuteScalar(ScalarSourceType sourceType)
		{
			return ExecuteScalar(sourceType, new NameOrIndexParameter());
		}

		/// <summary>
		/// Executes the query, and returns the value with specified scalar
		/// source type.
		/// </summary>
		/// <param name="sourceType">The method used to return the scalar value.</param>
		/// <param name="nameOrIndex">The column name/index or output parameter name/index.</param>
		/// <returns><list type="table">
		/// <listheader>
		///  <term>ScalarSourceType</term>
		///  <description>Return value</description>
		/// </listheader>
		/// <item>
		///  <term>DataReader</term>
		///  <description>The column with specified name or at specified index
		///  of the first row in the resultset.</description>
		/// </item>
		/// <item>
		///  <term>OutputParameter</term>
		///  <description>The value of the output or input/output parameter
		///  returned with specified name or at specified index.</description>
		/// </item>
		/// <item>
		///  <term>ReturnValue</term>
		///  <description>The value of the "return value" parameter returned.
		///  The index parameter is ignored.</description>
		/// </item>
		/// <item>
		///  <term>AffectedRows</term>
		///  <description>The number of rows affected. The index parameter is
		///  ignored.</description>
		/// </item>
		/// </list>
		/// </returns>
		public object ExecuteScalar(ScalarSourceType sourceType, NameOrIndexParameter nameOrIndex)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			switch (sourceType)
			{
				case ScalarSourceType.DataReader:
					using (IDataReader reader = ExecuteReaderInternal())
						if (reader.Read())
							return reader.GetValue(nameOrIndex.ByName ? reader.GetOrdinal(nameOrIndex.Name) : nameOrIndex.Index);

					break;

				case ScalarSourceType.OutputParameter:
					ExecuteNonQueryInternal();

					if (nameOrIndex.ByName)
					{
						string name = (string)_dataProvider.Convert(nameOrIndex.Name,
							ConvertType.NameToParameter);
						
						return Parameter(name).Value;
					}
					else
					{
						int index = nameOrIndex.Index;
						foreach (IDataParameter p in SelectCommand.Parameters)
						{
							// Skip the return value parameter.
							//
							if (p.Direction == ParameterDirection.ReturnValue)
								continue;

							if (0 == index)
								return p.Value;

							--index;
						}
					}
					break;

				case ScalarSourceType.ReturnValue:
					ExecuteNonQueryInternal();

					foreach (IDataParameter p in SelectCommand.Parameters)
						if (p.Direction == ParameterDirection.ReturnValue)
							return p.Value;

					break;

				case ScalarSourceType.AffectedRows:
					return ExecuteNonQueryInternal();

				default:
					throw new InvalidEnumArgumentException("sourceType",
						(int)sourceType, typeof(ScalarSourceType));
			}

			return null;
		}

#if FW2
		// I need partial specialization :crash:
		
		/// <summary>
		/// Executes the query, and returns the first column of the first row
		/// in the resultset returned by the query. Extra columns or rows are
		/// ignored.
		/// </summary>
		/// <returns>
		/// The first column of the first row in the resultset.</returns>
		/// <seealso cref="ExecuteScalar{T}(ScalarSourceType, NameOrIndexParameter)"/>
		public T ExecuteScalar<T>()
		{
			return (T)_mappingSchema.ConvertChangeType(ExecuteScalar(), typeof(T));
		}

		/// <summary>
		/// Executes the query, and returns the value with specified scalar
		/// source type.
		/// </summary>
		/// <param name="sourceType">The method used to return the scalar
		/// value.</param>
		/// <returns><list type="table">
		/// <listheader>
		///  <term>ScalarSourceType</term>
		///  <description>Return value</description>
		/// </listheader>
		/// <item>
		///  <term>DataReader</term>
		///  <description>The first column of the first row in the resultset.
		///  </description>
		/// </item>
		/// <item>
		///  <term>OutputParameter</term>
		///  <description>The value of the first output or input/output
		///  parameter returned.</description>
		/// </item>
		/// <item>
		///  <term>ReturnValue</term>
		///  <description>The value of the "return value" parameter returned.
		///  </description>
		/// </item>
		/// <item>
		///  <term>AffectedRows</term>
		///  <description>The number of rows affected.</description>
		/// </item>
		/// </list>
		/// </returns>
		/// <seealso cref="ExecuteScalar{T}(ScalarSourceType, NameOrIndexParameter)"/>
		public T ExecuteScalar<T>(ScalarSourceType sourceType)
		{
			return ExecuteScalar<T>(sourceType, new NameOrIndexParameter());
		}

		/// <summary>
		/// Executes the query, and returns the value with specified scalar
		/// source type.
		/// </summary>
		/// <param name="sourceType">The method used to return the scalar value.</param>
		/// <param name="nameOrIndex">The column name/index or output parameter name/index.</param>
		/// <returns><list type="table">
		/// <listheader>
		///  <term>ScalarSourceType</term>
		///  <description>Return value</description>
		/// </listheader>
		/// <item>
		///  <term>DataReader</term>
		///  <description>The column with specified name or at specified index
		///  of the first row in the resultset.</description>
		/// </item>
		/// <item>
		///  <term>OutputParameter</term>
		///  <description>The value of the output or input/output parameter
		///  returned with specified name or at specified index.</description>
		/// </item>
		/// <item>
		///  <term>ReturnValue</term>
		///  <description>The value of the "return value" parameter returned.
		///  The index parameter is ignored.</description>
		/// </item>
		/// <item>
		///  <term>AffectedRows</term>
		///  <description>The number of rows affected. The index parameter is
		///  ignored.</description>
		/// </item>
		/// </list>
		/// </returns>
		public T ExecuteScalar<T>(ScalarSourceType sourceType, NameOrIndexParameter nameOrIndex)
		{
			return (T)_mappingSchema.ConvertChangeType(ExecuteScalar(sourceType, nameOrIndex), typeof(T));
		}
		
#endif

		#endregion

		#region ExecuteScalarList

		/// <summary>
		/// Executes the query, and returns the array list of values of the
		/// specified column of  the every row in the resultset returned by the
		/// query. Extra columns are ignored.
		/// </summary>
		/// <param name="list">The array to fill in.</param>
		/// <param name="nameOrIndex">The column name/index or output parameter name/index.</param>
		/// <param name="type">The type of the each element.</param>
		/// <returns>Array list of values of the specified column of the every
		/// row in the resultset.</returns>
		public IList ExecuteScalarList(
			IList                list,
			Type                 type,
			NameOrIndexParameter nameOrIndex)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				return _mappingSchema.MapDataReaderToScalarList(dr, nameOrIndex, list, type);
			}
		}

		/// <summary>
		/// Executes the query, and returns the array list of values of first
		/// column of the every row in the resultset returned by the query.
		/// Extra columns are ignored.
		/// </summary>
		/// <param name="list">The array to fill in.</param>
		/// <param name="type">The type of the each element.</param>
		/// <returns>Array list of values of first column of the every row in
		/// the resultset.</returns>
		public IList ExecuteScalarList(IList list, Type type)
		{
			return ExecuteScalarList(list, type, 0);
		}

		/// <summary>
		/// Executes the query, and returns the array list of values of the
		/// specified column of  the every row in the resultset returned by the
		/// query. Extra columns are ignored.
		/// </summary>
		/// <param name="nameOrIndex">The column name/index.</param>
		/// <param name="type">The type of the each element.</param>
		/// <returns>Array list of values of the specified column of the every
		/// row in the resultset.</returns>
		public ArrayList ExecuteScalarList(Type type, NameOrIndexParameter nameOrIndex)
		{
			ArrayList list = new ArrayList();

			ExecuteScalarList(list, type, nameOrIndex);

			return list;
		}

		/// <summary>
		/// Executes the query, and returns the array list of values of first
		/// column of the every row in the resultset returned by the query.
		/// Extra columns are ignored.
		/// </summary>
		/// <param name="type">The type of the each element.</param>
		/// <returns>Array list of values of first column of the every row in
		/// the resultset.</returns>
		public ArrayList ExecuteScalarList(Type type)
		{
			ArrayList list = new ArrayList();

			ExecuteScalarList(list, type, 0);

			return list;
		}

		
#if FW2
		/// <summary>
		/// Executes the query, and returns the array list of values of the
		/// specified column of  the every row in the resultset returned by the
		/// query. Extra columns are ignored.
		/// </summary>
		/// <param name="list">The array to fill in.</param>
		/// <param name="nameOrIndex">The column name/index or output parameter
		/// name/index.</param>
		/// <typeparam name="T">The type of the each element.</typeparam>
		/// <returns>Array list of values of the specified column of the every
		/// row in the resultset.</returns>
		public IList<T> ExecuteScalarList<T>(
			IList<T>             list,
			NameOrIndexParameter nameOrIndex)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				return _mappingSchema.MapDataReaderToScalarList<T>(dr, nameOrIndex, list);
			}
		}

		/// <summary>
		/// Executes the query, and returns the array list of values of first
		/// column of the every row in the resultset returned by the query.
		/// Extra columns are ignored.
		/// </summary>
		/// <param name="list">The array to fill in.</param>
		/// <typeparam name="T">The type of the each element.</typeparam>
		/// <returns>Array list of values of first column of the every row in
		/// the resultset.</returns>
		public IList<T> ExecuteScalarList<T>(IList<T> list)
		{
			return ExecuteScalarList(list, 0);
		}

		/// <summary>
		/// Executes the query, and returns the array list of values of the
		/// specified column of the every row in the resultset returned by the
		/// query. Extra columns are ignored.
		/// </summary>
		/// <param name="nameOrIndex">The column name/index or output parameter name/index.</param>
		/// <typeparam name="T">The type of the each element.</typeparam>
		/// <returns>Array list of values of the specified column of the every
		/// row in the resultset.</returns>
		public List<T> ExecuteScalarList<T>(NameOrIndexParameter nameOrIndex)
		{
			List<T> list = new List<T>();

			ExecuteScalarList<T>(list, nameOrIndex);

			return list;
		}

		/// <summary>
		/// Executes the query, and returns the array list of values of first
		/// column of the every row in the resultset returned by the query.
		/// Extra columns are ignored.
		/// </summary>
		/// <typeparam name="T">The type of the each element.</typeparam>
		/// <returns>Array list of values of first column of the every row in
		/// the resultset.</returns>
		public List<T> ExecuteScalarList<T>()
		{
			List<T> list = new List<T>();

			ExecuteScalarList<T>(list, 0);

			return list;
		}
#endif

		#endregion

		#region ExecuteScalarDictionary

		public IDictionary ExecuteScalarDictionary(
			IDictionary dic,
			NameOrIndexParameter keyField,   Type keyFieldType,
			NameOrIndexParameter valueField, Type valueFieldType)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			//object nullValue = _mappingSchema.GetNullValue(type);

			if (keyField.ByName && keyField.Name.Length > 0 && keyField.Name[0] == '@')
				keyField = keyField.Name.Substring(1);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				if (dr.Read())
				{
					int keyIndex = keyField.ByName ? dr.GetOrdinal(keyField.Name) : keyField.Index;
					int valueIndex = valueField.ByName ? dr.GetOrdinal(valueField.Name) : valueField.Index;

					do
					{
						object value = dr[valueIndex];
						object key = dr[keyIndex];

						if (key == null || key.GetType() != keyFieldType)
							key = key is DBNull ? null : _mappingSchema.ConvertChangeType(key, keyFieldType);

						if (value == null || value.GetType() != valueFieldType)
							value = value is DBNull ? null : _mappingSchema.ConvertChangeType(value, valueFieldType);

						dic.Add(key, value);
					}
					while (dr.Read());
				}
			}

			return dic;
		}

		public Hashtable ExecuteScalarDictionary(
			NameOrIndexParameter keyField, Type keyFieldType,
			NameOrIndexParameter valueField, Type valueFieldType)
		{
			Hashtable table = new Hashtable();

			ExecuteScalarDictionary(table, keyField, keyFieldType, valueField, valueFieldType);

			return table;
		}

#if FW2
		public IDictionary<K,T> ExecuteScalarDictionary<K,T>(
			IDictionary<K, T> dic,
			NameOrIndexParameter keyField,
			NameOrIndexParameter valueField)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			//object nullValue = _mappingSchema.GetNullValue(type);

			Type keyFieldType   = typeof(K);
			Type valueFieldType = typeof(T);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				if (dr.Read())
				{
					int keyIndex = keyField.ByName ? dr.GetOrdinal(keyField.Name) : keyField.Index;
					int valueIndex = valueField.ByName ? dr.GetOrdinal(valueField.Name) : valueField.Index;

					do
					{
						object value = dr[valueIndex];
						object key = dr[keyIndex];

						if (key == null || key.GetType() != keyFieldType)
							key = key is DBNull ? null : _mappingSchema.ConvertChangeType(key, keyFieldType);

						if (value == null || value.GetType() != valueFieldType)
							value = value is DBNull ? null : _mappingSchema.ConvertChangeType(value, valueFieldType);

						dic.Add((K)key, (T)value);
					}
					while (dr.Read());
				}
			}

			return dic;
		}

		public Dictionary<K,T> ExecuteScalarDictionary<K,T>(
			NameOrIndexParameter keyField,
			NameOrIndexParameter valueField)
		{
			Dictionary<K,T> dic = new Dictionary<K,T>();

			ExecuteScalarDictionary<K,T>(dic, keyField, valueField);

			return dic;
		}
#endif

		#endregion

		#region ExecuteScalarDictionary (Index)

		public IDictionary ExecuteScalarDictionary(
			IDictionary dic, MapIndex index,
			NameOrIndexParameter valueField, Type valueFieldType)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			//object nullValue = _mappingSchema.GetNullValue(type);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				if (dr.Read())
				{
					int valueIndex = valueField.ByName ? dr.GetOrdinal(valueField.Name) : valueField.Index;
					int[] keyIndex = new int[index.Fields.Length];

					for (int i = 0; i < keyIndex.Length; i++)
						keyIndex[i] = index.Fields[i].ByName ?
							dr.GetOrdinal(index.Fields[i].Name) : index.Fields[i].Index;

					do
					{
						object value = dr[valueIndex];

						if (value == null || value.GetType() != valueFieldType)
							value = value is DBNull ? null : _mappingSchema.ConvertChangeType(value, valueFieldType);

						object[] key = new object[keyIndex.Length];

						for (int i = 0; i < keyIndex.Length; i++)
							key[i] = dr[keyIndex[i]];

						dic.Add(new CompoundValue(key), value);
					}
					while (dr.Read());
				}
			}

			return dic;
		}

		public Hashtable ExecuteScalarDictionary(
			MapIndex index, NameOrIndexParameter valueField, Type valueFieldType)
		{
			Hashtable table = new Hashtable();

			ExecuteScalarDictionary(table, index, valueField, valueFieldType);

			return table;
		}

#if FW2
		public IDictionary<CompoundValue,T> ExecuteScalarDictionary<T>(
			IDictionary<CompoundValue, T> dic, MapIndex index, NameOrIndexParameter valueField)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			//object nullValue = _mappingSchema.GetNullValue(type);

			Type valueFieldType = typeof(T);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				if (dr.Read())
				{
					int valueIndex = valueField.ByName ? dr.GetOrdinal(valueField.Name) : valueField.Index;
					int[] keyIndex = new int[index.Fields.Length];

					for (int i = 0; i < keyIndex.Length; i++)
						keyIndex[i] = index.Fields[i].ByName ?
							dr.GetOrdinal(index.Fields[i].Name) : index.Fields[i].Index;

					do
					{
						object value = dr[valueIndex];

						if (value == null || value.GetType() != valueFieldType)
							value = value is DBNull ? null : _mappingSchema.ConvertChangeType(value, valueFieldType);

						object[] key = new object[keyIndex.Length];

						for (int i = 0; i < keyIndex.Length; i++)
							key[i] = dr[keyIndex[i]];

						dic.Add(new CompoundValue(key), (T)value);
					}
					while (dr.Read());
				}
			}

			return dic;
		}

		public Dictionary<CompoundValue,T> ExecuteScalarDictionary<T>(
			MapIndex index, NameOrIndexParameter valueField)
		{
			Dictionary<CompoundValue,T> dic = new Dictionary<CompoundValue,T>();

			ExecuteScalarDictionary<T>(dic, index, valueField);

			return dic;
		}
#endif

		#endregion

		#region ExecuteReader

		/// <summary>
		/// Executes the command and builds an <see cref="IDataReader"/>.
		/// </summary>
		/// <returns>An instance of the <see cref="IDataReader"/> class.</returns>
		public IDataReader ExecuteReader()
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			return ExecuteReaderInternal();
		}

		/// <summary>
		/// Executes the command and builds an <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="commandBehavior">One of the <see cref="CommandBehavior"/> values.</param>
		/// <returns>An instance of the <see cref="IDataReader"/> class.</returns>
		public IDataReader ExecuteReader(CommandBehavior commandBehavior)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			return ExecuteReaderInternal(commandBehavior);
		}

		#endregion

		#region ExecuteDataSet

		/// <summary>
		/// Executes a SQL statement using the provided parameters.
		/// </summary>
		/// <remarks>
		/// See the <see cref="ExecuteDataSet(NameOrIndexParameter)"/> method
		/// to find an example.
		/// </remarks>
		/// <returns>The <see cref="DataSet"/>.</returns>
		public DataSet ExecuteDataSet()
		{
			return ExecuteDataSet(null, 0, 0, "Table");
		}

		/// <summary>
		/// Executes a SQL statement using the provided parameters.
		/// </summary>
		/// <remarks>
		/// See the <see cref="ExecuteDataSet(NameOrIndexParameter)"/> method
		/// to find an example.
		/// </remarks>
		/// <param name="dataSet">The input DataSet object.</param>
		/// <returns>The <see cref="DataSet"/>.</returns>
		public DataSet ExecuteDataSet(
			DataSet dataSet)
		{
			return ExecuteDataSet(dataSet, 0, 0, "Table");
		}

		/// <summary>
		/// Executes a SQL statement using the provided parameters.
		/// </summary>
		/// <remarks>
		/// See the <see cref="ExecuteDataSet(NameOrIndexParameter)"/> method
		/// to find an example.
		/// </remarks>
		/// <param name="table">The name or index of the populating table.</param>
		/// <returns>The <see cref="DataSet"/>.</returns>
		public DataSet ExecuteDataSet(
			NameOrIndexParameter table)
		{
			return ExecuteDataSet(null, 0, 0, table);
		}

		/// <summary>
		/// Executes a SQL statement using the provided parameters.
		/// </summary>
		/// <param name="dataSet">The DataSet object to populate.</param>
		/// <param name="table">The name or index of the populating table.</param>
		/// <returns>The <see cref="DataSet"/>.</returns>
		public DataSet ExecuteDataSet(
			DataSet              dataSet,
			NameOrIndexParameter table)
		{
			return ExecuteDataSet(dataSet, 0, 0, table);
		}

		/// <summary>
		/// Executes a SQL statement using the provided parameters.
		/// </summary>
		/// <param name="dataSet">The DataSet object to populate.</param>
		/// <param name="table">The name or index of the populating table.</param>
		/// <param name="startRecord">The zero-based record number to start with.</param>
		/// <param name="maxRecords">The maximum number of records to retrieve.</param>
		/// <returns>The <see cref="DataSet"/>.</returns>
		public DataSet ExecuteDataSet(
			DataSet              dataSet,
			int                  startRecord,
			int                  maxRecords,
			NameOrIndexParameter table)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			if (dataSet == null)
				dataSet = new DataSet();

			DbDataAdapter da = _dataProvider.CreateDataAdapterObject();

			((IDbDataAdapter)da).SelectCommand = SelectCommand;

			try
			{
				OnBeforeOperation(OperationType.Fill);
				if (table.ByName)
					da.Fill(dataSet, startRecord, maxRecords, table.Name);
				else
#if FW2
					da.Fill(startRecord, maxRecords, dataSet.Tables[table.Index]);
#else
					da.Fill(dataSet, startRecord, maxRecords, dataSet.Tables[table.Index].TableName);
#endif
				OnAfterOperation(OperationType.Fill);

				return dataSet;
			}
			catch (Exception ex)
			{
				OnOperationException(OperationType.Fill, ex);
				throw;
			}
		}

		#endregion

		#region ExecuteDataTable

		/// <summary>
		/// Executes a SQL statement using the provided parameters.
		/// </summary>
		/// <returns>The <see cref="DataTable"/>.</returns>
		public DataTable ExecuteDataTable()
		{
			return ExecuteDataTable(null);
		}

		/// <summary>
		/// Executes a SQL statement using the provided parameters.
		/// </summary>
		/// <param name="dataTable">The DataTable object to populate.</param>
		/// <returns>The <see cref="DataTable"/>.</returns>
		public DataTable ExecuteDataTable(DataTable dataTable)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			if (dataTable == null)
				dataTable = new DataTable();

			DbDataAdapter da = _dataProvider.CreateDataAdapterObject();
			((IDbDataAdapter)da).SelectCommand = SelectCommand;

			try
			{
				OnBeforeOperation(OperationType.Fill);
				da.Fill(dataTable);
				OnAfterOperation (OperationType.Fill);

				return dataTable;
			}
			catch (Exception ex)
			{
				OnOperationException(OperationType.Fill, ex);
				throw;
			}
		}

		public void ExecuteDataTables(params DataTable[] tableList)
		{
			if (tableList == null || tableList.Length == 0)
				return;

			using (IDataReader dr = ExecuteReader())
			{
				int i = 0;

				do 
				{
					_mappingSchema.MapDataReaderToDataTable(dr, tableList[i]);

					tableList[i].AcceptChanges();

					i++;
				}
				while (dr.NextResult() && i < tableList.Length);
			}
		}

		#endregion

		#region ExecuteObject

		/// <summary>
		/// Executes a SQL statement and maps resultset to an object.
		/// </summary>
		/// <param name="entity">An object to populate.</param>
		/// <returns>A business object.</returns>
		public object ExecuteObject(object entity)
		{
			if (null == entity)
				throw new ArgumentNullException("entity");

			return ExecuteObjectInternal(entity, entity.GetType());
		}

		/// <summary>
		/// Executes a SQL statement and maps resultset to an object.
		/// </summary>
		/// <param name="entity">An object to populate.</param>
		/// <param name="type">The System.Type of the object.</param>
		/// <returns>A business object.</returns>
		private object ExecuteObjectInternal(object entity, Type type)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal(CommandBehavior.SingleRow))
			{
				if (dr.Read()) 
				{
					return entity == null?
						_mappingSchema.MapDataReaderToObject(dr, type,   null):
						_mappingSchema.MapDataReaderToObject(dr, entity, null);
				}

				return null;
			}
		}

		/// <summary>
		/// Executes a SQL statement and maps resultset to an object.
		/// </summary>
		/// <param name="type">Type of an object.</param>
		/// <returns>A business object.</returns>
		public object ExecuteObject(Type type)
		{
			return ExecuteObjectInternal(null, type);
		}

#if FW2
		/// <summary>
		/// Executes a SQL statement and maps resultset to an object.
		/// </summary>
		/// <typeparam name="T">Type of an object.</typeparam>
		/// <returns>A business object.</returns>
		public T ExecuteObject<T>()
		{
			return (T)ExecuteObjectInternal(null, typeof(T));
		}
#endif

		#endregion

		#region ExecuteList

		private IList ExecuteListInternal(IList list, Type type, params object[] parameters)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				return _mappingSchema.MapDataReaderToList(dr, list, type, parameters);
			}
		}

#if FW2
		private IList<T> ExecuteListInternal<T>(IList<T> list, params object[] parameters)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				return _mappingSchema.MapDataReaderToList<T>(dr, list, parameters);
			}
		}
#endif

		/// <summary>
		/// Executes the query, and returns an array of business entities using the provided parameters.
		/// </summary>
		/// <param name="type">Type of the business object.</param>
		/// <returns>An array of business objects.</returns>
		public ArrayList ExecuteList(Type type)
		{
			ArrayList arrayList = new ArrayList();

			ExecuteListInternal(arrayList, type, (object[])null);

			return arrayList;
		}

#if FW2
		/// <summary>
		/// Executes the query, and returns an array of business entities.
		/// </summary>
		/// <typeparam name="T">Type of an object.</typeparam>
		/// <returns>Populated list of mapped business objects.</returns>
		public List<T> ExecuteList<T>()
		{
			List<T> list = new List<T>();

			ExecuteListInternal(list, typeof(T), (object[])null);

			return list;
		}
#endif

		/// <summary>
		/// Executes the query, and returns an array of business entities using the provided parameters.
		/// </summary>
		/// <param name="type">Type of the business object.</param>
		/// <param name="parameters"></param>
		/// <returns>An array of business objects.</returns>
		public ArrayList ExecuteList(Type type, params object[] parameters)
		{
			ArrayList arrayList = new ArrayList();

			ExecuteListInternal(arrayList, type, parameters);

			return arrayList;
		}

#if FW2
		/// <summary>
		/// Executes the query, and returns an array of business entities.
		/// </summary>
		/// <typeparam name="T">Type of an object.</typeparam>
		/// <param name="parameters"></param>
		/// <returns>Populated list of mapped business objects.</returns>
		public List<T> ExecuteList<T>(params object[] parameters)
		{
			List<T> list = new List<T>();

			ExecuteListInternal(list, typeof(T), parameters);

			return list;
		}
#endif

		/// <summary>
		/// Executes the query, and returns an array of business entities.
		/// </summary>
		/// <param name="list">The list of mapped business objects to populate.</param>
		/// <param name="type">Type of an object.</param>
		/// <returns>Populated list of mapped business objects.</returns>
		public IList ExecuteList(IList list, Type type)
		{
			return ExecuteListInternal(list, type, (object[])null);
		}

#if FW2
		/// <summary>
		/// Executes the query, and returns an array of business entities.
		/// </summary>
		/// <typeparam name="T">Type of an object.</typeparam>
		/// <param name="list">The list of mapped business objects to populate.</param>
		/// <returns>Populated list of mapped business objects.</returns>
		public IList<T> ExecuteList<T>(IList<T> list) 
		{
			ExecuteListInternal<T>(list, (object[])null);

			return list;
		}
#endif

		/// <summary>
		/// Executes the query, and returns an array of business entities.
		/// </summary>
		/// <param name="list">The list of mapped business objects to populate.</param>
		/// <param name="type">Type of an object.</param>
		/// <param name="parameters"></param>
		/// <returns>Populated list of mapped business objects.</returns>
		public IList ExecuteList(IList list, Type type, params object[] parameters)
		{
			return ExecuteListInternal(list, type, parameters);
		}

#if FW2
		/// <summary>
		/// Executes the query, and returns an array of business entities.
		/// </summary>
		/// <typeparam name="T">Type of an object.</typeparam>
		/// <param name="list">The list of mapped business objects to populate.</param>
		/// <param name="parameters"></param>
		/// <returns>Populated list of mapped business objects.</returns>
		public IList<T> ExecuteList<T>(IList<T> list, params object[] parameters)
		{
			ExecuteListInternal<T>(list, parameters);

			return list;
		}

		public L ExecuteList<L, T>(L list, params object[] parameters)
			where L : IList<T>
		{
			ExecuteListInternal(list, typeof(T), parameters);

			return list;
		}
#endif

		#endregion

		#region ExecuteDictionary

		/// <summary>
		/// Executes the query, and returns the <see cref="Hashtable"/> of business entities 
		/// using the provided parameters.
		/// </summary>
		/// <include file="Examples.xml" path='examples/db[@name="ExecuteDictionary(string,Type)"]/*' />
		/// <param name="keyField">The field name or index that is used as a key to populate <see cref="Hashtable"/>.</param>
		/// <param name="keyFieldType">Business object type.</param>
		/// <param name="parameters">Any additional parameters passed to the constructor with <see cref="InitContext"/> parameter.</param>
		/// <returns>An instance of the <see cref="Hashtable"/> class.</returns>
		public Hashtable ExecuteDictionary(
			NameOrIndexParameter keyField,
			Type                 keyFieldType,
			params object[]      parameters)
		{
			Hashtable hash = new Hashtable();

			ExecuteDictionary(hash, keyField, keyFieldType, parameters);

			return hash;
		}

		/// <summary>
		/// Executes the query, and returns the <see cref="Hashtable"/> of business entities.
		/// </summary>
		/// <include file="Examples.xml" path='examples/db[@name="ExecuteDictionary(Hashtable,string,Type)"]/*' />
		/// <param name="dictionary">A dictionary of mapped business objects to populate.</param>
		/// <param name="keyField">The field name or index that is used as a key to populate <see cref="IDictionary"/>.</param>
		/// <param name="type">Business object type.</param>
		/// <param name="parameters">Any additional parameters passed to the constructor with <see cref="InitContext"/> parameter.</param>
		/// <returns>An instance of the <see cref="IDictionary"/>.</returns>
		public IDictionary ExecuteDictionary(
			IDictionary          dictionary,
			NameOrIndexParameter keyField,
			Type                 type,
			params object[]      parameters)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				return _mappingSchema.MapDataReaderToDictionary(dr, dictionary, keyField, type, parameters);
			}
		}

#if FW2
		/// <summary>
		/// Executes the query, and returns a dictionary of business entities.
		/// </summary>
		/// <typeparam name="TKey">Key's type.</typeparam>
		/// <typeparam name="TValue">Value's type.</typeparam>
		/// <param name="keyField">The field name or index that is used as a key to populate the dictionary.</param>
		/// <param name="parameters">Any additional parameters passed to the constructor with <see cref="InitContext"/> parameter.</param>
		/// <returns>An instance of the dictionary.</returns>
		public Dictionary<TKey, TValue> ExecuteDictionary<TKey, TValue>(
			NameOrIndexParameter keyField,
			params object[]      parameters)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				return _mappingSchema.MapDataReaderToDictionary<TKey,TValue>(
					dr, keyField, parameters);
			}
		}

		public IDictionary<TKey, TValue> ExecuteDictionary<TKey, TValue>(
			IDictionary<TKey, TValue> dictionary,
			NameOrIndexParameter      keyField,
			Type                      destObjectType,
			params object[]           parameters)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				return _mappingSchema.MapDataReaderToDictionary<TKey,TValue>(
					dr, dictionary, keyField, destObjectType, parameters);
			}
		}

		public IDictionary<TKey, TValue> ExecuteDictionary<TKey, TValue>(
			IDictionary<TKey, TValue> dictionary,
			NameOrIndexParameter      keyField,
			params object[]           parameters)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				return _mappingSchema.MapDataReaderToDictionary<TKey,TValue>(
					dr, dictionary, keyField, parameters);
			}
		}
#endif

		#endregion

		#region ExecuteDictionary (Index)

		/// <summary>
		/// Executes the query, and returns the <see cref="Hashtable"/> of business entities 
		/// using the provided parameters.
		/// </summary>
		/// <include file="Examples.xml" path='examples/db[@name="ExecuteDictionary(string,Type)"]/*' />
		/// <param name="index">Dictionary key fields.</param>
		/// <param name="type">Business object type.</param>
		/// <param name="parameters">Any additional parameters passed to the constructor with <see cref="InitContext"/> parameter.</param>
		/// <returns>An instance of the <see cref="Hashtable"/> class.</returns>
		public Hashtable ExecuteDictionary(
			MapIndex        index,
			Type            type,
			params object[] parameters)
		{
			Hashtable hash = new Hashtable();

			ExecuteDictionary(hash, index, type, parameters);

			return hash;
		}

		/// <summary>
		/// Executes the query, and returns the <see cref="Hashtable"/> of business entities.
		/// </summary>
		/// <include file="Examples.xml" path='examples/db[@name="ExecuteDictionary(Hashtable,string,Type)"]/*' />
		/// <param name="dictionary">A dictionary of mapped business objects to populate.</param>
		/// <param name="index">Dictionary key fields.</param>
		/// <param name="type">Business object type.</param>
		/// <param name="parameters">Any additional parameters passed to the constructor with <see cref="InitContext"/> parameter.</param>
		/// <returns>An instance of the <see cref="IDictionary"/>.</returns>
		public IDictionary ExecuteDictionary(
			IDictionary     dictionary,
			MapIndex        index,
			Type            type,
			params object[] parameters)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				return _mappingSchema.MapDataReaderToDictionary(dr, dictionary, index, type, parameters);
			}
		}

#if FW2
		/// <summary>
		/// Executes the query, and returns a dictionary of business entities.
		/// </summary>
		/// <typeparam name="TValue">Value's type.</typeparam>
		/// <returns>An instance of the dictionary.</returns>
		public Dictionary<CompoundValue, TValue> ExecuteDictionary<TValue>(
			MapIndex        index,
			params object[] parameters)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				return _mappingSchema.MapDataReaderToDictionary<TValue>(dr, index, parameters);
			}
		}

		public IDictionary<CompoundValue, TValue> ExecuteDictionary<TValue>(
			IDictionary<CompoundValue, TValue> dictionary,
			MapIndex                           index,
			Type                               destObjectType,
			params object[]                    parameters)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				return _mappingSchema.MapDataReaderToDictionary<TValue>(
					dr, dictionary, index, destObjectType, parameters);
			}
		}

		public IDictionary<CompoundValue, TValue> ExecuteDictionary<TValue>(
			IDictionary<CompoundValue, TValue> dictionary,
			MapIndex                           index,
			params object[]                    parameters)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				return _mappingSchema.MapDataReaderToDictionary<TValue>(
					dr, dictionary, index, parameters);
			}
		}
#endif

		#endregion

		#region ExecuteResultSet

		public MapResultSet[] ExecuteResultSet(MapResultSet[] resultSets)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				_mappingSchema.MapDataReaderToResultSet(dr, resultSets);
			}

			return resultSets;
		}

		public MapResultSet[] ExecuteResultSet(
			Type masterType, params MapNextResult[] nextResults)
		{
			return ExecuteResultSet(_mappingSchema.ConvertToResultSet(masterType, nextResults));
		}

		#endregion

		#region Update

		private DbDataAdapter CreateDataAdapter()
		{
			DbDataAdapter da = _dataProvider.CreateDataAdapterObject();

			if (_insertCommand != null) ((IDbDataAdapter)da).InsertCommand = InsertCommand;
			if (_updateCommand != null) ((IDbDataAdapter)da).UpdateCommand = UpdateCommand;
			if (_deleteCommand != null) ((IDbDataAdapter)da).DeleteCommand = DeleteCommand;

			return da;
		}

		/// <summary>
		/// Calls the corresponding INSERT, UPDATE, or DELETE statements for each inserted, updated, or 
		/// deleted row in the specified <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> used to update the data source.</param>
		/// <returns>The number of rows successfully updated from the <see cref="DataSet"/>.</returns>
		public int Update(DataSet dataSet)
		{
			return Update(dataSet, "Table");
		}

		/// <summary>
		/// Calls the corresponding INSERT, UPDATE, or DELETE statements for each inserted, updated, or 
		/// deleted row in the <see cref="DataSet"/> with the specified <see cref="DataTable"/> name.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> used to update the data source.</param>
		/// <param name="table">The name or index of the source table to use for table mapping.</param>
		/// <returns>The number of rows successfully updated from the <see cref="DataSet"/>.</returns>
		public int Update(
			DataSet              dataSet,
			NameOrIndexParameter table)
		{
			if (dataSet == null)
				throw new ArgumentNullException(
					"dataSet",
					"DataSet must be initialized before calling Update routine. Cannot update database from a null dataset.");

			DbDataAdapter da = CreateDataAdapter();

			try
			{
				OnBeforeOperation(OperationType.Update);
				int result = (table.ByName) ?
					da.Update(dataSet, table.Name) :
					da.Update(dataSet.Tables[table.Index]);
				OnAfterOperation(OperationType.Update);

				return result;
			}
			catch (Exception ex)
			{
				OnOperationException(OperationType.Update, ex);
				throw;
			}
		}

		/// <summary>
		/// Calls the corresponding INSERT, UPDATE, or DELETE statements for
		/// each inserted, updated, or deleted row in the specified
		/// <see cref="DataTable"/>.
		/// </summary>
		/// <param name="dataTable">The name or index of the source table to
		/// use for table mapping.</param>
		/// <returns>The number of rows successfully updated from the
		/// <see cref="DataTable"/>.</returns>
		public int Update(DataTable dataTable)
		{
			if (dataTable == null)
				throw new ArgumentNullException(
					"dataTable",
					"DataTable must be initialized before calling Update routine. Cannot update database from a null data table.");

			try
			{
				OnBeforeOperation(OperationType.Update);
				int result = CreateDataAdapter().Update(dataTable);
				OnAfterOperation(OperationType.Update);

				return result;
			}
			catch (Exception ex)
			{
				OnOperationException(OperationType.Update, ex);
				throw;
			}
		}

		#endregion
	}
}
