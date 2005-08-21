/*
 * File:    DbManager.cs
 * Created: 12/30/2002
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

#if VER2
using System.Collections.Generic;
#endif

namespace Rsdn.Framework.Data
{
	/// <summary>
	/// The <b>DbManager</b> is a primary class of the <see cref="Rsdn.Framework.Data"/> namespace
	/// that can be used to execute commands of different database providers.
	/// </summary>
	/// <remarks>
	/// When the <b>DbManager</b> goes out of scope, it does not close the internal connection object.
	/// Therefore, you must explicitly close the connection by calling <see cref="Close"/> or 
	/// <see cref="Dispose()"/>. Also, you can use the C# <b>using</b> statement.
	/// </remarks>
	/// <include file="Examples.xml" path='examples/db[@name="DbManager"]/*' />
	public class DbManager :
#if DEFINE_COMPONENT
		Component,
#endif
		IDisposable
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
		public DbManager()
			: this((IDbConnection)null, null)
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
		/// <param name="provider">Provider configuration name.</param>
		/// <returns>An instance of the <see cref="DbManager"/> class.</returns>
		public DbManager(string configuration, string provider)
			: this((IDbConnection)null, configuration + "." + provider)
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
		/// <exception cref="RsdnDbManagerException">
		/// Type of the connection could not be recognized.
		/// </exception>
		/// <include file="Examples.xml" path='examples/db[@name="ctor(IDbConnection)"]/*' />
		/// <param name="connection">An instance of the <see cref="IDbConnection"/> class.</param>
		/// <returns>An instance of the <see cref="DbManager"/> class.</returns>
		public DbManager(IDbConnection connection)
		{
			if (connection != null)
			{
#if HANDLE_EXCEPTIONS
				try 
				{
#endif
					Init(connection);
			
					if (_connection.State == ConnectionState.Closed)
						OpenConnection();
#if HANDLE_EXCEPTIONS
				}
				catch (Exception ex)
				{
					HandleException(ex);
				}
#endif
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DbManager"/> class for the provided transaction.
		/// </summary>
		/// <include file="Examples.xml" path='examples/db[@name="ctor(IDbTransaction)"]/*' />
		/// <param name="transaction"></param>
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
		public DbManager(
			IDbConnection connection,
			string        configurationString)
		{
#if HANDLE_EXCEPTIONS
			try
			{
#endif
			if (connection == null)
				{
					Init(configurationString);

					// http://www.codeproject.com/dotnet/RsdnFrameworkData.asp?msg=640267#xx640267xx
					//
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
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
#endif
		}

		#endregion

		#region Public Properties

		private DataProvider.IDataProvider _dataProvider;
		/// <summary>
		/// Gets the <see cref="Rsdn.Framework.Data.DataProvider.IDataProvider"/> 
		/// used by this instance of the <see cref="DbManager"/>.
		/// </summary>
		/// <value>
		/// A data provider.
		/// </value>
		/// <include file="Examples.xml" path='examples/db[@name="DataProvider"]/*' />
		/// <seealso cref="AddDataProvider">AddDataProvider Method</seealso>
		public DataProvider.IDataProvider DataProvider
		{
			get { return _dataProvider; }
		}

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
		/// <exception cref="RsdnDbManagerException">
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
						throw new RsdnDbManagerException("The connection does not match the data provider type.");
					}
				}
				else
				{
					Init(value);
				}

				_closeConnection = false;
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
		/// The configuration string may have a postfix used to define a data provider. The following table
		/// contains postfixes for all supported data providers:
		/// <list type="table">
		/// <listheader><term>Postfix</term><description>Provider</description></listheader>
		/// <item><term>.Sql</term><description>Data Provider for SQL Server</description></item>
		/// <item><term>.OleDb</term><description>Data Provider for OLE DB</description></item>
		/// <item><term>.Odbc</term><description>Data Provider for ODBC</description></item>
		/// <item><term>.Oracle</term><description>Data Provider for Oracle</description></item>
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
		/// This method is called by the public <see cref="Component.Dispose()"/> method 
		/// and the <see cref="Component.Finalize"/> method.
		/// </remarks>
		/// <param name="disposing"><b>true</b> to release both managed and unmanaged resources; <b>false</b> to release only unmanaged resources.</param>
		protected 
#if DEFINE_COMPONENT
			override 
#else
			virtual
#endif
			void Dispose(bool disposing)
		{
			Close();
		}

#if !DEFINE_COMPONENT
		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 
		/// </summary>
		~DbManager()
		{
			Dispose(false);
		}
#endif

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
		/// <seealso cref="Dispose()">Dispose Method</seealso>
		public void Close()
		{
			if (_selectCommand != null) { _selectCommand.Dispose(); _selectCommand = null; }
			if (_insertCommand != null) { _insertCommand.Dispose(); _insertCommand = null; }
			if (_updateCommand != null) { _updateCommand.Dispose(); _updateCommand = null; }
			if (_deleteCommand != null) { _deleteCommand.Dispose(); _deleteCommand = null; }

			if (_transaction != null && _closeTransaction)
			{
				OnBeforeOperation(OperationType.DisposeTransaction);
				_transaction.Dispose();
				OnAfterOperation (OperationType.DisposeTransaction);
				_transaction = null;
			}

			if (_connection != null && _closeConnection)
			{
				OnBeforeOperation(OperationType.CloseConnection);
				_connection.Dispose();
				OnAfterOperation (OperationType.CloseConnection);
				_connection = null;
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
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				// If transaction is open, we dispose it, it will rollback all changes.
				//
				if (_transaction != null)
				{
					OnBeforeOperation(OperationType.DisposeTransaction);
					_transaction.Dispose();
					OnAfterOperation (OperationType.DisposeTransaction);
				}

				// Create new transaction object.
				//
				OnBeforeOperation(OperationType.BeginTransaction);
				_transaction      = Connection.BeginTransaction(il);
				OnAfterOperation (OperationType.BeginTransaction);
				_closeTransaction = true;

				// If the active command exists.
				//
				if (_selectCommand != null) _selectCommand.Transaction = _transaction;
				if (_insertCommand != null) _insertCommand.Transaction = _transaction;
				if (_updateCommand != null) _updateCommand.Transaction = _transaction;
				if (_deleteCommand != null) _deleteCommand.Transaction = _transaction;

				return this;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public DbManager CommitTransaction()
		{
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				if (_transaction != null)
				{
					OnBeforeOperation(OperationType.CommitTransaction);
					_transaction.Commit();
					OnAfterOperation (OperationType.CommitTransaction);

					if (_closeTransaction)
						_transaction = null;
				}

				return this;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
		}

		#endregion

		#region Protected Methods

		static DbManager ()
		{
			AddDataProvider(new DataProvider.SqlDataProvider());
			AddDataProvider(new DataProvider.OleDbDataProvider());
			AddDataProvider(new DataProvider.OdbcDataProvider());
			AddDataProvider(new DataProvider.OracleDataProvider());
		}

		private static string                     _firstConfiguration;
		private static DataProvider.IDataProvider _firstProvider;
		private static Hashtable _configurationList = Hashtable.Synchronized(new Hashtable());

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
				_dataProvider = (DataProvider.IDataProvider)_configurationList[configurationString];

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
		}

		private static DataProvider.IDataProvider GetDataProvider(string configurationString)
		{
			int idx = configurationString.LastIndexOf('.');

			string key = idx >= 0 && configurationString.Length > idx + 1?
				configurationString.Substring(idx + 1):
				"SQL";

			DataProvider.IDataProvider dataProvider = 
				(DataProvider.IDataProvider)_dataProviderNameList[key.ToUpper()];

			if (dataProvider == null)
				dataProvider = (DataProvider.IDataProvider)_dataProviderNameList["SQL"];

			return dataProvider;
		}

		private void Init(IDbConnection connection)
		{
			_dataProvider = (DataProvider.IDataProvider)_dataProviderTypeList[connection.GetType()];

			if (_dataProvider != null)
			{
				Connection = connection;
			}
			else
			{
				throw new RsdnDbManagerException(string.Format(
					"The '{0}' type of the connection could not be recognized.",
					connection.GetType().FullName));
			}
		}

		private void InitializeComponent()
		{
		}

		private void OpenConnection()
		{
			OnBeforeOperation(OperationType.OpenConnection);
			_connection.Open();
			OnAfterOperation (OperationType.OpenConnection);

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

			// Store the configuration string for future use.
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
			IDbDataParameter[] commandParameters = GetSpParameterSet(spName, true);

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
							param.Value     = p.Value;
							p.ParameterName = name;

							found = true;

							break;
						}
					}

					if (found == false && (
						param.Direction == ParameterDirection.Input || 
						param.Direction == ParameterDirection.InputOutput))
					{
						param.SourceColumn = name[0] == '@'? name.Substring(1): name;
					}
				}
			}
			else
			{
				// Assign the provided values to these parameters based on parameter order.
				//
				AssignParameterValues(commandParameters, parameterValues);
			}

			return commandParameters;
		}

#if HANDLE_EXCEPTIONS

		private static void HandleException(Exception ex)
		{
			if (ex is RsdnDataException || ex is ArgumentNullException)
			{
				throw ex;
			}
			else
			{
				throw new RsdnDbManagerException(ex.Message, ex);
			}
		}

#endif

		private IDbCommand InitCommand(IDbCommand command)
		{
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				IDbCommand cmd = command;

				if (cmd == null) 
				{
					// Create a command object.
					//
					cmd = Connection.CreateCommand();

					// If an active transaction exists.
					//
					if (Transaction != null)
					{
						cmd.Transaction = Transaction;
					}
				}

				return cmd;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
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
		private static void AttachParameters(IDbCommand command, IDbDataParameter[] commandParameters)
		{
			command.Parameters.Clear();

			foreach (IDbDataParameter p in commandParameters)
			{
				command.Parameters.Add(p);
			}
		}

		private static Hashtable _paramCache = Hashtable.Synchronized(new Hashtable());

		/// <summary>
		/// Resolve at run time the appropriate set of parameters for a stored procedure.
		/// </summary>
		/// <param name="spName">The name of the stored procedure.</param>
		/// <param name="includeReturnValueParameter">Whether or not to include their return value parameter.</param>
		/// <returns></returns>
		private IDbDataParameter[] DiscoverSpParameterSet(string spName, bool includeReturnValueParameter)
		{
			using (IDbConnection con = _dataProvider.CreateConnectionObject())
			{
				con.ConnectionString = _closeConnection || _connection == null?
					GetConnectionString(ConfigurationString):
					_connection.ConnectionString;

				OnBeforeOperation(OperationType.OpenConnection);
				con.Open();
				OnAfterOperation (OperationType.OpenConnection);

				using (IDbCommand cmd = con.CreateCommand())
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.CommandText = spName;

					OnBeforeOperation(OperationType.DeriveParameters);
					bool res = _dataProvider.DeriveParameters(cmd);
					OnAfterOperation(OperationType.DeriveParameters);

					if (res == false)
						return null;

					if (includeReturnValueParameter == false)
					{
						cmd.Parameters.RemoveAt(0);
					}

					IDbDataParameter[] discoveredParameters = 
						new IDbDataParameter[cmd.Parameters.Count];;

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
		private static IDbDataParameter[] CloneParameters(IDbDataParameter[] originalParameters)
		{
			IDbDataParameter[] clonedParameters = new IDbDataParameter[originalParameters.Length];

			for (int i = 0, j = originalParameters.Length; i < j; i++)
			{
				clonedParameters[i] = (IDbDataParameter)((ICloneable)originalParameters[i]).Clone();
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
		public IDbDataParameter[] GetSpParameterSet(string spName, bool includeReturnValueParameter)
		{
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				string key = 
					string.Format("{0}:{1}:{2}", ConfigurationString, spName, includeReturnValueParameter);

				IDbDataParameter[] cachedParameters = (IDbDataParameter[])_paramCache[key];

				if (cachedParameters == null)
				{
					if (_paramCache.ContainsKey(key))
						return null;

					cachedParameters = DiscoverSpParameterSet(spName, includeReturnValueParameter);
					_paramCache[key] = cachedParameters;
				}
			
				return cachedParameters == null? null: CloneParameters(cachedParameters);
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
		}

		/// <summary>
		/// This method assigns an array of values to an array of parameters.
		/// </summary>
		/// <param name="commandParameters">array of IDbDataParameters to be assigned values</param>
		/// <param name="parameterValues">array of objects holding the values to be assigned</param>
		private static void AssignParameterValues(IDbDataParameter[] commandParameters, object[] parameterValues)
		{
			if (commandParameters == null || parameterValues == null)
			{
				// Do nothing if we get no data.
				//
				return;
			}

			int len = commandParameters.Length;
			int dt  = 0;

			if (commandParameters[0].Direction == ParameterDirection.ReturnValue)
			{
				len--;
				dt = 1;
			}

			// We must have the same number of values as we pave parameters to put them in.
			//
			if (len != parameterValues.Length)
			{
				throw new ArgumentException("Parameter count does not match Parameter Value count.");
			}

			// Iterate through the parameters, assigning the values from 
			// the corresponding position in the value array.
			//
			for (int i = 0; i < len; i++)
			{
				object value = parameterValues[i];

				commandParameters[i + dt].Value = value == null? DBNull.Value: value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="op"></param>
		protected virtual void OnBeforeOperation(OperationType op)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="op"></param>
		protected virtual void OnAfterOperation(OperationType op)
		{
		}

		#endregion

		#region Public Static Methods.

		private static Hashtable _dataProviderNameList = Hashtable.Synchronized(new Hashtable(4));
		private static Hashtable _dataProviderTypeList = Hashtable.Synchronized(new Hashtable(4));

		/// <summary>
		/// Adds a new data manager.
		/// </summary>
		/// <remarks>
		/// The method can be used to register a new data manager for further use.
		/// </remarks>
		/// <include file="Examples1.xml" path='examples/db[@name="AddDataProvider(DataProvider.IDataProvider)"]/*' />
		/// <seealso cref="AddConnectionString(string)">AddConnectionString Method.</seealso>
		/// <seealso cref="Rsdn.Framework.Data.DataProvider.IDataProvider.Name">IDataProvider.Name Property.</seealso>
		/// <param name="dataProvider">An instance of the <see cref="Rsdn.Framework.Data.DataProvider.IDataProvider"/> interface.</param>
		public static void AddDataProvider(DataProvider.IDataProvider dataProvider)
		{
			_dataProviderNameList[dataProvider.Name.ToUpper()] = dataProvider;
			_dataProviderTypeList[dataProvider.ConnectionType] = dataProvider;
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

		private static string GetConnectionString(string configurationString)
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

#if VER2
				o = ConfigurationManager.AppSettings.Get(key);
#else
				o = ConfigurationSettings.AppSettings.Get(key);
#endif

				if (o == null)
				{
					throw new RsdnDbManagerException(string.Format(
						"The '{0}' key does not exist in the configuration file.", key));
				}

				// Store the result in cache.
				//
				_connectionStringList[configurationString] = o;
			}

			return o.ToString();
		}
		#endregion

		#region Parameters

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
			DataRow dataRow,
			params IDbDataParameter[] commandParameters)
		{
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				ArrayList paramList = new ArrayList();

				foreach (DataColumn c in dataRow.Table.Columns)
				{
					if (c.AutoIncrement == false && c.ReadOnly == false)
					{
						paramList.Add(
							c.AllowDBNull?
							NullParameter("@" + c.ColumnName, dataRow[c.ColumnName]):
							Parameter    ("@" + c.ColumnName, dataRow[c.ColumnName]));
					}
				}

				foreach (IDbDataParameter p in commandParameters)
					paramList.Add(p);

				return (IDbDataParameter[])paramList.ToArray(typeof(IDbDataParameter));
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
		}

		/// <summary>
		/// Creates an array of parameters from a business object.
		/// </summary>
		/// <remarks>
		/// The method can take an additional parameter list, 
		/// which can be created by using the same method.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="CreateParameters(object,IDbDataParameter[])"]/*' />
		/// <param name="entity">The business object.</param>
		/// <param name="commandParameters">An array of paramters to be added to the result array.</param>
		/// <returns>An array of parameters.</returns>
		public IDbDataParameter[] CreateParameters(
			object entity,
			params IDbDataParameter[] commandParameters)
		{
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				ParameterReader       pr = new ParameterReader(this);
				Mapping.MapDescriptor td = Mapping.MapDescriptor.GetDescriptor(entity.GetType());

				Mapping.Map.MapInternal(td, entity, pr, this);

				foreach (IDbDataParameter p in commandParameters)
					pr.ParamList.Add(p);

				return (IDbDataParameter[])pr.ParamList.ToArray(typeof(IDbDataParameter));
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
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
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				foreach (DataColumn c in dataRow.Table.Columns)
				{
					if (c.AutoIncrement == false && c.ReadOnly == false)
					{
						object o = dataRow[c.ColumnName];

						Parameter("@" + c.ColumnName).Value = 
							c.AllowDBNull && Mapping.Map.IsNull(o)? DBNull.Value: o;
					}
				}

				return this;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
		}

		/// <summary>
		/// Assigns a business object to command parameters.
		/// </summary>
		/// <remarks>
		/// The method is used in addition to the <see cref="CreateParameters(object,IDbDataParameter[])"/> method.
		/// </remarks>
		/// <include file="Examples1.xml" path='examples/db[@name="AssignParameterValues(object)"]/*' />
		/// <param name="entity">A business entity to assign.</param>
		/// <returns>This instance of the <see cref="DbManager"/>.</returns>
		public DbManager AssignParameterValues(object entity)
		{
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				ParameterReader       pr = new ParameterReader(this);
				Mapping.MapDescriptor td = Mapping.MapDescriptor.GetDescriptor(entity.GetType());

				Mapping.Map.MapInternal(td, entity, pr, this);

				foreach (IDbDataParameter p in pr.ParamList)
					if (Command.Parameters.Contains(p.ParameterName))
						Parameter(p.ParameterName).Value = p.Value;

				return this;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
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
#if HANDLE_EXCEPTIONS
			try 
			{
#endif
				return (IDbDataParameter)Command.Parameters[parameterName];
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
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
			if (Mapping.Map.IsNull(value))
			{
				@value = DBNull.Value;
			}

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
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				IDbDataParameter parameter = Command.CreateParameter();

				parameter.ParameterName = parameterName;
				parameter.Direction     = parameterDirection;

				if (value is DateTime && parameter is OleDbParameter)
					((OleDbParameter)parameter).OleDbType = OleDbType.Date;

				parameter.Value = value != null? value: DBNull.Value;

				return parameter;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterDirection"></param>
		/// <param name="parameterName"></param>
		/// <param name="value"></param>
		/// <param name="dbType"></param>
		/// <returns></returns>
		public IDbDataParameter Parameter(
			ParameterDirection parameterDirection,
			string parameterName,
			object value,
			DbType dbType)
		{
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				IDbDataParameter parameter = Command.CreateParameter();

				parameter.ParameterName = parameterName;
				parameter.Direction     = parameterDirection;
				parameter.DbType        = dbType;
				parameter.Value         = value;

				return parameter;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterName"></param>
		/// <param name="value"></param>
		/// <param name="dbType"></param>
		/// <returns></returns>
		public IDbDataParameter Parameter(
			string parameterName,
			object value,
			DbType dbType)
		{
			return Parameter(ParameterDirection.Input, parameterName, value, dbType);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterDirection"></param>
		/// <param name="parameterName"></param>
		/// <param name="value"></param>
		/// <param name="dbType"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public IDbDataParameter Parameter(
			ParameterDirection parameterDirection,
			string parameterName,
			object value,
			DbType dbType,
			int    size)
		{
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				IDbDataParameter parameter = Command.CreateParameter();

				parameter.ParameterName = parameterName;
				parameter.Direction     = parameterDirection;
				parameter.DbType        = dbType;
				parameter.Size          = size;
				parameter.Value         = value;

				return parameter;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterName"></param>
		/// <param name="value"></param>
		/// <param name="dbType"></param>
		/// <param name="size"></param>
		/// <returns></returns>
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
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				IDbDataParameter parameter = Command.CreateParameter();
			
				parameter.ParameterName = parameterName;
				parameter.Direction     = parameterDirection;
				parameter.DbType        = dbType;

				return parameter;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
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
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				IDbDataParameter parameter = Command.CreateParameter();
			
				parameter.ParameterName = parameterName;
				parameter.Direction     = parameterDirection;
				parameter.DbType        = dbType;
				parameter.Size          = size;

				return parameter;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
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

		private void SetCommand(
			CommandAction commandAction,
			IDbCommand    command)
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

		private void SetCommandParameters(
			CommandAction      commandAction,
			IDbDataParameter[] commandParameters)
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
			// If we receive parameter values, we need to figure out where they go.
			//
			if (parameterValues != null && parameterValues.Length > 0)
			{
				return SetCommand(
					commandAction,
					CommandType.StoredProcedure,
					spName,
					CreateSpParameters(spName, parameterValues));
			}
			// Otherwise we can just call the SP without params.
			//
			else 
			{
				return SetCommand(
					commandAction,
					CommandType.StoredProcedure,
					spName,
					(IDbDataParameter[])null);
			}
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
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				IDbCommand command = GetCommand(commandAction, commandType, commandText);

				SetCommand          (commandAction, command);
				SetCommandParameters(commandAction, commandParameters);

				if (commandParameters != null)
				{
					AttachParameters(command, commandParameters);
				}

				return command;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
		}

		/// <summary>
		/// Prepares a command for execution.
		/// </summary>
		/// <returns>Current instance.</returns>
		public DbManager Prepare()
		{
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				IDbCommand command = GetCommand(CommandAction.Select);

				if (InitParameters(CommandAction.Select) == false)
				{
					OnBeforeOperation(OperationType.PrepareCommand);
					command.Prepare();
					OnAfterOperation (OperationType.PrepareCommand);

					_prepared = true;
				}

				return this;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
		}

		private bool InitParameters(CommandAction commandAction)
		{
			bool prepare = false;

#if HANDLE_EXCEPTIONS
			try 
			{
#endif
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
					}

					if (prepare)
					{
						IDbCommand command = GetCommand(commandAction);

						AttachParameters(command, commandParameters);
						command.Prepare();
					}
				}
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
#endif

			return prepare;
		}

		#endregion

		#region Execute

		/// <summary>
		/// Executes a SQL statement for a given list of objects and 
		/// returns the number of rows affected.
		/// </summary>
		/// <remarks>
		/// The method prepares the <see cref="Command"/> object 
		/// and calls the <see cref="ExecuteNonQuery"/> method for each item of the list.
		/// </remarks>
		/// <include file="Examples1.xml" path='examples/db[@name="Execute(CommandType,string,IList)"]/*' />
		/// <param name="list">The list of objects used to execute the command.</param>
		/// <returns>The number of rows affected by the command.</returns>
		public int Execute(IList list)
		{
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				int rows = 0;

				if (list != null && list.Count != 0)
				{
					IDbDataParameter[] parameters = GetCommandParameters(CommandAction.Select);

					if (parameters == null || parameters.Length == 0)
					{
						parameters = CreateParameters(list[0]);

						SetCommandParameters(CommandAction.Select, parameters);
						AttachParameters(SelectCommand, parameters);
						Prepare();
					}

					foreach (object o in list)
					{
						OnBeforeOperation(OperationType.ExecuteNonQuery);
						rows += AssignParameterValues(o).ExecuteNonQuery();
						OnAfterOperation (OperationType.ExecuteNonQuery);
					}
				}
			
				return rows;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return 0;
			}
#endif
		}

		/// <summary>
		/// Executes a SQL statement for the <see cref="DataTable"/> and 
		/// returns the number of rows affected.
		/// </summary>
		/// <remarks>
		/// The method prepares the <see cref="Command"/> object 
		/// and calls the <see cref="ExecuteNonQuery"/> method for each item 
		/// of the <see cref="DataTable"/>.
		/// </remarks>
		/// <include file="Examples1.xml" path='examples/db[@name="Execute(CommandType,string,DataTable)"]/*' />
		/// <param name="table">An instance of the <see cref="DataTable"/> class to execute the command.</param>
		/// <returns>The number of rows affected by the command.</returns>
		public int Execute(DataTable table)
		{
#if HANDLE_EXCEPTIONS
			try
			{
#endif
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
						OnBeforeOperation(OperationType.ExecuteNonQuery);
						rows += AssignParameterValues(dr).ExecuteNonQuery();
						OnAfterOperation (OperationType.ExecuteNonQuery);
					}
				}
			
				return rows;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return 0;
			}
#endif
		}

		/// <summary>
		/// Executes a SQL statement for the first table of the <see cref="DataSet"/> 
		/// and returns the number of rows affected.
		/// </summary>
		/// <remarks>
		/// The method prepares the <see cref="Command"/> object
		/// and calls the <see cref="ExecuteNonQuery"/> method for each item of the first table.
		/// </remarks>
		/// <include file="Examples1.xml" path='examples/db[@name="Execute(CommandType,string,DataSet)"]/*' />
		/// <param name="dataSet">An instance of the <see cref="DataSet"/> class to execute the command.</param>
		/// <returns>The number of rows affected by the command.</returns>
		public int Execute(DataSet dataSet)
		{
			return Execute(dataSet.Tables[0]);
		}

		/// <summary>
		/// Executes a SQL statement for the specified table of the <see cref="DataSet"/> 
		/// and returns the number of rows affected.
		/// </summary>
		/// <remarks>
		/// The method prepares the <see cref="Command"/> object
		/// and calls the <see cref="ExecuteNonQuery"/> method for each item of the first table.
		/// </remarks>
		/// <include file="Examples1.xml" path='examples/db[@name="Execute(CommandType,string,DataSet,string)"]/*' />
		/// <param name="dataSet">An instance of the <see cref="DataSet"/> class to execute the command.</param>
		/// <param name="tableName">The table name.</param>
		/// <returns>The number of rows affected by the command.</returns>
		public int Execute(DataSet dataSet, string tableName)
		{
			return Execute(dataSet.Tables[tableName]);
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
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				if (_prepared)
					InitParameters(CommandAction.Select);

				OnBeforeOperation(OperationType.ExecuteNonQuery);
				int result = SelectCommand.ExecuteNonQuery();
				OnAfterOperation (OperationType.ExecuteNonQuery);

				return result;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return 0;
			}
#endif
		}

		#endregion

		#region ExecuteScalar

		/// <summary>
		/// Executes the query, and returns the first column of the first row in the resultset returned 
		/// by the query. Extra columns or rows are ignored.
		/// </summary>
		/// <returns>The first column of the first row in the resultset.</returns>
		public object ExecuteScalar()
		{
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				if (_prepared)
					InitParameters(CommandAction.Select);

				OnBeforeOperation(OperationType.ExecuteScalar);
				object result = SelectCommand.ExecuteScalar();
				OnAfterOperation (OperationType.ExecuteScalar);

				return result;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
		}

#if VER2
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T ExecuteScalar<T>()
		{
			return (T)ExecuteScalar();
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
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				if (_prepared)
					InitParameters(CommandAction.Select);

				OnBeforeOperation(OperationType.ExecuteReader);
				IDataReader result = SelectCommand.ExecuteReader();
				OnAfterOperation (OperationType.ExecuteReader);

				return result;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
		}

		/// <summary>
		/// Executes the command and builds an <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="commandBehavior">One of the <see cref="CommandBehavior"/> values.</param>
		/// <returns>An instance of the <see cref="IDataReader"/> class.</returns>
		public IDataReader ExecuteReader(CommandBehavior commandBehavior)
		{
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				if (_prepared)
					InitParameters(CommandAction.Select);

				OnBeforeOperation(OperationType.ExecuteReader);
				IDataReader result = SelectCommand.ExecuteReader(commandBehavior);
				OnAfterOperation (OperationType.ExecuteReader);

				return result;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
		}

		#endregion

		#region ExecuteDataSet

		/// <summary>
		/// Executes a SQL statement using the provided parameters.
		/// </summary>
		/// <remarks>
		/// See the <see cref="ExecuteDataSet(string)"/> method to find an example.
		/// </remarks>
		/// <returns>The <see cref="DataSet"/>.</returns>
		public DataSet ExecuteDataSet()
		{
			return ExecuteDataSet(null, 0, 0, null);
		}

		/// <summary>
		/// Executes a SQL statement using the provided parameters.
		/// </summary>
		/// <remarks>
		/// See the <see cref="ExecuteDataSet(string)"/> method to find an example.
		/// </remarks>
		/// <param name="dataSet">The input DataSet object.</param>
		/// <returns>The <see cref="DataSet"/>.</returns>
		public DataSet ExecuteDataSet(
			DataSet dataSet)
		{
			return ExecuteDataSet(dataSet, 0, 0, null);
		}

		/// <summary>
		/// Executes a SQL statement using the provided parameters.
		/// </summary>
		/// <remarks>
		/// See the <see cref="ExecuteDataSet(string)"/> method to find an example.
		/// </remarks>
		/// <param name="tableName">The name of the populating table.</param>
		/// <returns>The <see cref="DataSet"/>.</returns>
		public DataSet ExecuteDataSet(
			string tableName)
		{
			return ExecuteDataSet(null, 0, 0, tableName);
		}

		/// <summary>
		/// Executes a SQL statement using the provided parameters.
		/// </summary>
		/// <param name="dataSet">The DataSet object to populate.</param>
		/// <param name="tableName">The name of the populating table.</param>
		/// <returns>The <see cref="DataSet"/>.</returns>
		public DataSet ExecuteDataSet(
			DataSet dataSet,
			string  tableName)
		{
			return ExecuteDataSet(dataSet, 0, 0, tableName);
		}

		/// <summary>
		/// Executes a SQL statement using the provided parameters.
		/// </summary>
		/// <param name="dataSet">The DataSet object to populate.</param>
		/// <param name="tableName">The name of the populating table.</param>
		/// <param name="startRecord">The zero-based record number to start with.</param>
		/// <param name="maxRecords">The maximum number of records to retrieve.</param>
		/// <returns>The <see cref="DataSet"/>.</returns>
		public DataSet ExecuteDataSet(
			DataSet dataSet,
			int     startRecord,
			int     maxRecords,
			string  tableName)
		{
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				if (_prepared)
					InitParameters(CommandAction.Select);

				if (dataSet == null)
					dataSet = new DataSet();

				DbDataAdapter da = _dataProvider.CreateDataAdapterObject();

				((IDbDataAdapter)da).SelectCommand = SelectCommand;

				OnBeforeOperation(OperationType.Fill);

				if (tableName == null)
				{
					da.Fill(dataSet);
				}
				else if (maxRecords != 0)
				{
					da.Fill(dataSet, startRecord, maxRecords, tableName);
				}
				else
				{
					da.Fill(dataSet, tableName);
				}

				OnAfterOperation(OperationType.Fill);

				return dataSet;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
		}

		#endregion

		#region ExecuteDataTable

		/// <summary>
		/// Executes a SQL statement using the provided parameters.
		/// </summary>
		/// <returns>The <see cref="DataTable"/>.</returns>
		public DataTable ExecuteDataTable()
		{
			return ExecuteDataTable((DataTable)null);
		}

		/// <summary>
		/// Executes a SQL statement using the provided parameters.
		/// </summary>
		/// <param name="dataTable">The DataTable object to populate.</param>
		/// <returns>The <see cref="DataTable"/>.</returns>
		public DataTable ExecuteDataTable(DataTable dataTable)
		{
#if HANDLE_EXCEPTIONS
			try 
			{
#endif
				if (_prepared)
					InitParameters(CommandAction.Select);

				if (dataTable == null)
					dataTable = new DataTable();

				DbDataAdapter da = _dataProvider.CreateDataAdapterObject();
				((IDbDataAdapter)da).SelectCommand = SelectCommand;

				OnBeforeOperation(OperationType.Fill);
				da.Fill(dataTable);
				OnAfterOperation (OperationType.Fill);

				return dataTable;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tableList"></param>
		public void ExecuteDataTables(params DataTable[] tableList)
		{
			if (tableList == null || tableList.Length == 0)
				return;

			OnBeforeOperation(OperationType.ExecuteReader);

			using (IDataReader dr = ExecuteReader())
			{
				OnAfterOperation(OperationType.ExecuteReader);

				int i = 0;

				do 
				{
					Mapping.Map.ToDataTable(dr, tableList[i]);

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
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				if (_prepared)
					InitParameters(CommandAction.Select);

				OnBeforeOperation(OperationType.ExecuteReader);

				using (IDataReader dr = SelectCommand.ExecuteReader(CommandBehavior.SingleRow))
				{
					OnAfterOperation(OperationType.ExecuteReader);

					if (dr.Read()) 
					{
						Mapping.DataReaderSource drs = new Mapping.DataReaderSource(dr);
						Mapping.MapDescriptor    td  = Mapping.MapDescriptor.GetDescriptor(type);

						if (entity == null)
						{
							Mapping.MapInitializingData data = 
								new Mapping.MapInitializingData(drs, dr, td, null);

							entity = td.CreateInstanceEx(data);

							if (data.StopMapping)
								return entity;

							td = data.MapDescriptor;
						}

						Mapping.Map.MapInternal(drs, dr, td, entity);

						return entity;
					}
					else
					{
						return null;
					}
				}
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
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

#if VER2
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
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				if (_prepared)
					InitParameters(CommandAction.Select);

				OnBeforeOperation(OperationType.ExecuteReader);

				using (IDataReader dr = SelectCommand.ExecuteReader())
				{
					OnAfterOperation(OperationType.ExecuteReader);

					return Mapping.Map.ToList(dr, list, type, parameters);
				}
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
		}

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

#if VER2
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

#if VER2
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

#if VER2
		/// <summary>
		/// Executes the query, and returns an array of business entities.
		/// </summary>
		/// <typeparam name="T">Type of an object.</typeparam>
		/// <param name="list">The list of mapped business objects to populate.</param>
		/// <returns>Populated list of mapped business objects.</returns>
		public List<T> ExecuteList<T>(List<T> list) 
		{
			ExecuteListInternal(list, typeof(T), (object[])null);

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

#if VER2
		/// <summary>
		/// Executes the query, and returns an array of business entities.
		/// </summary>
		/// <typeparam name="T">Type of an object.</typeparam>
		/// <param name="list">The list of mapped business objects to populate.</param>
		/// <param name="parameters"></param>
		/// <returns>Populated list of mapped business objects.</returns>
		public List<T> ExecuteList<T>(List<T> list, params object[] parameters)
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
		/// <param name="keyFieldName">The field name that is used as a key to populate <see cref="Hashtable"/>.</param>
		/// <param name="type">Business object type.</param>
		/// <returns>An instance of the <see cref="Hashtable"/> class.</returns>
		public Hashtable ExecuteDictionary(
			string keyFieldName,
			Type   type)
		{
			Hashtable hash = new Hashtable();

			ExecuteDictionary(hash, keyFieldName, type);

			return hash;
		}

		/// <summary>
		/// Executes the query, and returns the <see cref="Hashtable"/> of business entities.
		/// </summary>
		/// <include file="Examples.xml" path='examples/db[@name="ExecuteDictionary(Hashtable,string,Type)"]/*' />
		/// <param name="dictionary">A dictionary of mapped business objects to populate.</param>
		/// <param name="keyFieldName">The field name that is used as a key to populate <see cref="IDictionary"/>.</param>
		/// <param name="type">Business object type.</param>
		/// <returns>An instance of the <see cref="IDictionary"/>.</returns>
		public IDictionary ExecuteDictionary(
			IDictionary dictionary,
			string      keyFieldName,
			Type        type)
		{
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				if (_prepared)
					InitParameters(CommandAction.Select);

				OnBeforeOperation(OperationType.ExecuteReader);

				using (IDataReader dr = SelectCommand.ExecuteReader())
				{
					OnAfterOperation(OperationType.ExecuteReader);

					return Mapping.Map.ToDictionary(dr, dictionary, keyFieldName, type);
				}
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
		}

#if VER2
		/// <summary>
		/// Executes the query, and returns a dictionary of business entities.
		/// </summary>
		/// <typeparam name="TKey">Key's type.</typeparam>
		/// <typeparam name="TValue">Value's type.</typeparam>
		/// <param name="keyFieldName">The field name that is used as a key to populate the dictionary.</param>
		/// <returns>An instance of the dictionary.</returns>
		public Dictionary<TKey, TValue> ExecuteDictionary<TKey, TValue>(string keyFieldName)
		{
			Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();

			ExecuteDictionary(dic, keyFieldName, typeof(TValue));

			return dic;
		}
#endif

		#endregion

		#region ExecuteResultSet

		/// <summary>
		/// 
		/// </summary>
		/// <param name="resultSets"></param>
		/// <returns></returns>
		public Mapping.MapResultSet[] ExecuteResultSet(Mapping.MapResultSet[] resultSets)
		{
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				if (_prepared)
					InitParameters(CommandAction.Select);

				OnBeforeOperation(OperationType.ExecuteReader);

				using (IDataReader dr = SelectCommand.ExecuteReader())
				{
					OnAfterOperation(OperationType.ExecuteReader);

					Mapping.Map.ToResultSet(dr, resultSets);
				}

				return resultSets;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
#endif
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="masterType"></param>
		/// <param name="nextResults"></param>
		/// <returns></returns>
		public Mapping.MapResultSet[] ExecuteResultSet(
			Type masterType, params Mapping.MapNextResult[] nextResults)
		{
			return ExecuteResultSet(Mapping.Map.ConvertToResultSet(masterType, nextResults));
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
			return Update(dataSet, null);
		}

		/// <summary>
		/// Calls the corresponding INSERT, UPDATE, or DELETE statements for each inserted, updated, or 
		/// deleted row in the <see cref="DataSet"/> with the specified <see cref="DataTable"/> name.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> used to update the data source.</param>
		/// <param name="tableName">The name of the source table to use for table mapping.</param>
		/// <returns>The number of rows successfully updated from the <see cref="DataSet"/>.</returns>
		public int Update(DataSet dataSet, string tableName)
		{
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				if (dataSet == null)
					throw new ArgumentNullException("dataSet",
						"DataSet must be initialized before calling Update routine. Cannot update database from a null dataset.");

				DbDataAdapter da = CreateDataAdapter();
				int           result;

				OnBeforeOperation(OperationType.Update);

				if (tableName == null)
				{
					result = da.Update(dataSet);
				}
				else
				{
					result = da.Update(dataSet, tableName);
				}

				OnAfterOperation(OperationType.Update);

				return result;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return 0;
			}
#endif
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataTable"></param>
		/// <returns></returns>
		public int Update(DataTable dataTable)
		{
#if HANDLE_EXCEPTIONS
			try
			{
#endif
				if (dataTable == null)
					throw new ArgumentNullException(
						"dataTable",
						"DataTable must be initialized before calling Update routine. Cannot update database from a null data table.");

				DbDataAdapter da = CreateDataAdapter();

				OnBeforeOperation(OperationType.Update);
				int result = da.Update(dataTable);
				OnAfterOperation (OperationType.Update);

				return result;
#if HANDLE_EXCEPTIONS
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return 0;
			}
#endif
		}

		#endregion

		#region Obsolete

		/// <summary>
		/// Executes a SQL statement and maps resultset to an object.
		/// </summary>
		/// <param name="entity">An object to populate.</param>
		/// <returns>A business object.</returns>
		[Obsolete("Use method ExecuteObject instead.")]
		public object ExecuteBizEntity(object entity)
		{
			return ExecuteObject(entity);
		}

		/// <summary>
		/// Executes a SQL statement and maps resultset to an object.
		/// </summary>
		/// <param name="type">Type of an object.</param>
		/// <returns>A business object.</returns>
		[Obsolete("Use method ExecuteObject instead.")]
		public object ExecuteBizEntity(Type type)
		{
			return ExecuteObject(type);
		}

#if VER2
		/// <summary>
		/// Executes a SQL statement and maps resultset to an object.
		/// </summary>
		/// <typeparam name="T">Type of an object.</typeparam>
		/// <returns>A business object.</returns>
		[Obsolete("Use method ExecuteObject instead.")]
		public T ExecuteBizEntity<T>()
		{
			return ExecuteObject<T>();
		}
#endif

		#endregion
	}
}
