using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace BLToolkit.Data
{
	using Common;
	using DataProvider;
	using Mapping;
	using Properties;
	using Reflection;

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
	[DesignerCategory("Code")]
	public partial class DbManager : Component
	{
		#region Constructors

		public DbManager(DataProviderBase dataProvider, string connectionString)
		{
			if (dataProvider     == null) throw new ArgumentNullException("dataProvider");
			if (connectionString == null) throw new ArgumentNullException("connectionString");

			_dataProvider = dataProvider;
			_connection   = dataProvider.CreateConnectionObject();

			_connection.ConnectionString = connectionString;

			_dataProvider.InitDbManager(this);
		}

		public DbManager(DataProviderBase dataProvider, IDbConnection connection)
		{
			if (dataProvider == null) throw new ArgumentNullException("dataProvider");
			if (connection   == null) throw new ArgumentNullException("connection");

			_dataProvider = dataProvider;
			_connection   = connection;

			_dataProvider.InitDbManager(this);
		}

		public DbManager(DataProviderBase dataProvider, IDbTransaction transaction)
		{
			if (dataProvider == null) throw new ArgumentNullException("dataProvider");
			if (transaction  == null) throw new ArgumentNullException("transaction");

			_dataProvider     = dataProvider;
			_connection       = transaction.Connection;
			_transaction      = transaction;
			_closeTransaction = false;

			_dataProvider.InitDbManager(this);
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
			[DebuggerStepThrough]
			get { return _mappingSchema; }
			set { _mappingSchema = value ?? Map.DefaultSchema; }
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
			[DebuggerStepThrough]
			get           { return _dataProvider;  }
			protected set { _dataProvider = value; }
		}

		#endregion

		#region Connection

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
			[DebuggerStepThrough]
			get
			{
				if (_connection.State == ConnectionState.Closed)
					OpenConnection();
				return _connection;
			}

			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				if (value.GetType() != _dataProvider.ConnectionType)
					InitDataProvider(value);

				_connection      = value;
				_closeConnection = false;
			}
		}

		[Obsolete]
		protected virtual string GetConnectionString(IDbConnection connection)
		{
			return connection.ConnectionString;
		}

		private void OpenConnection()
		{
			ExecuteOperation(OperationType.OpenConnection, _connection.Open);
			_closeConnection = true;
		}

		/// <summary>
		/// Closes the connection to the database.
		/// </summary>
		/// <remarks>
		/// The <b>Close</b> method rolls back any pending transactions
		/// and then closes the connection.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="Close()"]/*' />
		/// <seealso cref="Dispose(bool)"/>
		public void Close()
		{
			if (_selectCommand != null) { _selectCommand.Dispose(); _selectCommand = null; }
			if (_insertCommand != null) { _insertCommand.Dispose(); _insertCommand = null; }
			if (_updateCommand != null) { _updateCommand.Dispose(); _updateCommand = null; }
			if (_deleteCommand != null) { _deleteCommand.Dispose(); _deleteCommand = null; }

			if (_transaction != null && _closeTransaction)
			{
				ExecuteOperation(OperationType.DisposeTransaction, _transaction.Dispose);
				_transaction = null;
			}

			if (_connection != null && _closeConnection)
			{
				ExecuteOperation(OperationType.CloseConnection, _connection.Dispose);
				_connection = null;
			}
		}

		#endregion

		#region Transactions

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
		/// <seealso cref="BeginTransaction()"/>
		public IDbTransaction Transaction
		{
			[DebuggerStepThrough]
			get { return _transaction; }
		}

		/// <summary>
		/// Begins a database transaction.
		/// </summary>
		/// <remarks>
		/// Once the transaction has completed, you must explicitly commit or roll back the transaction
		/// by using the <see cref="System.Data.IDbTransaction.Commit"/>> or 
		/// <see cref="System.Data.IDbTransaction.Rollback"/> methods.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="BeginTransaction()"]/*' />
		/// <returns>This instance of the <see cref="DbManager"/>.</returns>
		/// <seealso cref="Transaction"/>
		public DbManager BeginTransaction()
		{
			return BeginTransaction(IsolationLevel.ReadCommitted);
		}

		/// <summary>
		/// Begins a database transaction with the specified <see cref="IsolationLevel"/> value.
		/// </summary>
		/// <remarks>
		/// Once the transaction has completed, you must explicitly commit or roll back the transaction
		/// by using the <see cref="System.Data.IDbTransaction.Commit"/> or 
		/// <see cref="System.Data.IDbTransaction.Rollback"/> methods.
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
				ExecuteOperation(OperationType.DisposeTransaction, _transaction.Dispose);
			}

			// Create new transaction object.
			//
			_transaction = ExecuteOperation<IDbTransaction>(OperationType.BeginTransaction,
				delegate { return Connection.BeginTransaction(il); });

			_closeTransaction = true;

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
				ExecuteOperation(OperationType.CommitTransaction, _transaction.Commit);

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
				ExecuteOperation(OperationType.RollbackTransaction, _transaction.Rollback);

				if (_closeTransaction)
					_transaction = null;
			}

			return this;
		}

		#endregion

		#region Commands

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
			[DebuggerStepThrough]
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
			[DebuggerStepThrough]
			get { return _selectCommand = OnInitCommand(_selectCommand); }
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
			[DebuggerStepThrough]
			get { return _insertCommand = OnInitCommand(_insertCommand); }
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
			[DebuggerStepThrough]
			get { return _updateCommand = OnInitCommand(_updateCommand); }
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
			[DebuggerStepThrough]
			get { return _deleteCommand = OnInitCommand(_deleteCommand); }
		}

		/// <summary>
		/// Initializes a command and raises the <see cref="InitCommand"/> event.
		/// </summary>
		protected virtual IDbCommand OnInitCommand(IDbCommand command)
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

			if (CanRaiseEvents)
			{
				InitCommandEventHandler handler = (InitCommandEventHandler)Events[EventInitCommand];

				if (handler != null)
					handler(this, new InitCommandEventArgs(command));
			}

			return command;
		}

		/// <summary>
		/// Helper function. Creates the command object and sets command type and command text.
		/// </summary>
		/// <param name="commandAction">Command action.</param>
		/// <param name="commandType">The <see cref="System.Data.CommandType"/>
		/// (stored procedure, text, etc.)</param>
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

		#endregion

		#region Events

		private static readonly object EventBeforeOperation = new object();
		/// <summary>
		/// Occurs when a server-side operation is about to start.
		/// </summary>
		public event OperationTypeEventHandler BeforeOperation
		{
			add    { Events.AddHandler   (EventBeforeOperation, value); }
			remove { Events.RemoveHandler(EventBeforeOperation, value); }
		}

		private static readonly object EventAfterOperation = new object();
		/// <summary>
		/// Occurs when a server-side operation is complete.
		/// </summary>
		public event OperationTypeEventHandler AfterOperation
		{
			add    { Events.AddHandler   (EventAfterOperation, value); }
			remove { Events.RemoveHandler(EventAfterOperation, value); }
		}

		private static readonly object EventOperationException = new object();
		/// <summary>
		/// Occurs when a server-side operation is failed to execute.
		/// </summary>
		public event OperationExceptionEventHandler OperationException
		{
			add    { Events.AddHandler   (EventOperationException, value); }
			remove { Events.RemoveHandler(EventOperationException, value); }
		}

		private static readonly object EventInitCommand = new object();
		/// <summary>
		/// Occurs when the <see cref="Command"/> is initializing.
		/// </summary>
		public event InitCommandEventHandler InitCommand
		{
			add    { Events.AddHandler   (EventInitCommand, value); }
			remove { Events.RemoveHandler(EventInitCommand, value); }
		}

		/// <summary>
		/// Raises the <see cref="BeforeOperation"/> event.
		/// </summary>
		/// <param name="op">The <see cref="OperationType"/>.</param>
		protected virtual void OnBeforeOperation(OperationType op)
		{
			if (CanRaiseEvents)
			{
				OperationTypeEventHandler handler = (OperationTypeEventHandler)Events[EventBeforeOperation];
				if (handler != null)
					handler(this, new OperationTypeEventArgs(op));
			}
		}

		/// <summary>
		/// Raises the <see cref="AfterOperation"/> event.
		/// </summary>
		/// <param name="op">The <see cref="OperationType"/>.</param>
		protected virtual void OnAfterOperation(OperationType op)
		{
			if (CanRaiseEvents)
			{
				OperationTypeEventHandler handler = (OperationTypeEventHandler)Events[EventAfterOperation];
				if (handler != null)
					handler(this, new OperationTypeEventArgs(op));
			}
		}

		/// <summary>
		/// Raises the <see cref="OperationException"/> event.
		/// </summary>
		/// <param name="op">The <see cref="OperationType"/>.</param>
		/// <param name="ex">The <see cref="Exception"/> occurred.</param>
		protected virtual void OnOperationException(OperationType op, DataException ex)
		{
			if (CanRaiseEvents)
			{
				OperationExceptionEventHandler handler = (OperationExceptionEventHandler)Events[EventOperationException];
				if (handler != null)
					handler(this, new OperationExceptionEventArgs(op, ex));
			}

			throw ex;
		}

		#endregion

		#region Protected Methods

		private  static TraceSwitch _ts;
		internal static TraceSwitch  TS
		{
			get { return _ts ?? (_ts = new TraceSwitch("DbManager", "DbManager trace switch")); }
		}

		private IDataReader ExecuteReaderInternal()
		{
			return ExecuteReader(CommandBehavior.Default);
		}

		private IDataReader ExecuteReaderInternal(CommandBehavior commandBehavior)
		{
			return ExecuteOperation<IDataReader>(OperationType.ExecuteReader, delegate { return SelectCommand.ExecuteReader(commandBehavior); });
		}

		private int ExecuteNonQueryInternal()
		{
			return ExecuteOperation<int>(OperationType.ExecuteNonQuery, SelectCommand.ExecuteNonQuery);
		}

		#endregion

		#region Parameters

		private IDbDataParameter[] CreateSpParameters(string spName, object[] parameterValues)
		{
			// Pull the parameters for this stored procedure from 
			// the parameter cache (or discover them & populate the cache)
			//
			IDbDataParameter[] commandParameters = GetSpParameters(spName, true);

			// DbParameters are bound by name, plain parameters by order
			//
			bool dbParameters = false;

			if (parameterValues == null || parameterValues.Length == 0 ||
				parameterValues[0] is IDbDataParameter || parameterValues[0] is IDbDataParameter[])
			{
				// The PrepareParameters method may add some additional parameters.
				//
				parameterValues = PrepareParameters(parameterValues);

				if (parameterValues == null || parameterValues.Length == 0)
					return commandParameters;

				dbParameters = true;
			}

			if (commandParameters == null/* || commandParameters.Length == 0*/)
			{
				commandParameters = new IDbDataParameter[parameterValues.Length];

				if (dbParameters)
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

			if (dbParameters)
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
								Debug.WriteLineIf(TS.TraceWarning, string.Format(
									"Stored Procedure '{0}'. Parameter '{1}' has different direction '{2}'. Should be '{3}'.",
										spName, name, param.Direction, p.Direction),
									TS.DisplayName);

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
						Debug.WriteLineIf(TS.TraceWarning, string.Format(
							"Stored Procedure '{0}'. Parameter '{1}' not assigned.", spName, name),
							TS.DisplayName);

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

		///<summary>
		/// Creates an one-dimension array of <see cref="IDbDataParameter"/>
		/// from any combination on IDbDataParameter, IDbDataParameter[] or null references.
		/// Null references are stripped, arrays and single parameters are combined
		/// into a new array.
		///</summary>
		/// <remarks>When two or more parameters has the same name,
		/// the first parameter is used, all the rest are ignored.</remarks>
		///<param name="parameters">Array of IDbDataParameter, IDbDataParameter[] or null references.</param>
		///<returns>An normalized array of <see cref="IDbDataParameter"/> without null references.</returns>
		///<exception cref="ArgumentException">The parameter <paramref name="parameters"/>
		/// contains anything except IDbDataParameter, IDbDataParameter[] or null reference.</exception>
		public virtual IDbDataParameter[] PrepareParameters(object[] parameters)
		{
			if (parameters == null || parameters.Length == 0)
				return null;

			// Little optimization.
			// Check if we have only one single ref parameter.
			//
			object refParam = null;

			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i] != null)
				{
					if (refParam != null)
					{
						refParam = null;
						break;
					}

					refParam = parameters[i];
				}
			}

			if (refParam is IDbDataParameter[])
			{
				return (IDbDataParameter[])refParam;
			}

			if (refParam is IDbDataParameter)
			{
				IDbDataParameter[] oneParameterArray = new IDbDataParameter[1];
				oneParameterArray[0] = (IDbDataParameter)refParam;
				return oneParameterArray;
			}

			List<IDbDataParameter>               list = new List<IDbDataParameter>(parameters.Length);
			Dictionary<string, IDbDataParameter> hash = new Dictionary<string, IDbDataParameter>(parameters.Length);

			foreach (object o in parameters)
			{
				if (o is IDbDataParameter)
				{
					IDbDataParameter p = (IDbDataParameter)o;

					if (!hash.ContainsKey(p.ParameterName))
					{
						list.Add(p);
						hash.Add(p.ParameterName, p);
					}
				}
				else if (o is IDbDataParameter[])
				{
					foreach (IDbDataParameter p in (IDbDataParameter[])o)
					{
						if (!hash.ContainsKey(p.ParameterName))
						{
							list.Add(p);
							hash.Add(p.ParameterName, p);
						}
					}
				}
				else if (o != null && o != DBNull.Value)
				{
					throw new ArgumentException(
						Resources.DbManager_NotDbDataParameter, "parameters");
				}
			}

			return list.ToArray();
		}

		/// <summary>
		/// This method is used to attach array of <see cref="IDbDataParameter"/> to a <see cref="IDbCommand"/>.
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

		private static readonly Dictionary<string, IDbDataParameter[]> _paramCache =
			new Dictionary<string, IDbDataParameter[]>();
		private static readonly object _paramCacheLock = new object();

		/// <summary>
		/// Resolve at run time the appropriate set of parameters for a stored procedure.
		/// </summary>
		/// <param name="spName">The name of the stored procedure.</param>
		/// <param name="includeReturnValueParameter">Whether or not to include their return value parameter.</param>
		/// <returns></returns>
		protected virtual IDbDataParameter[] DiscoverSpParameters(string spName, bool includeReturnValueParameter)
		{
			using (IDbConnection con = CloneConnection())
			{
				ExecuteOperation(OperationType.OpenConnection, con.Open);

				using (IDbCommand cmd = con.CreateCommand())
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.CommandText = spName;

					bool res = ExecuteOperation<bool>(OperationType.DeriveParameters, delegate { return _dataProvider.DeriveParameters(cmd); });
					ExecuteOperation(OperationType.CloseConnection, con.Close);

					if (res == false)
						return null;

					if (includeReturnValueParameter == false)
					{
						// All known data providers always treat
						// the return value as first parameter.
						//
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
			if (originalParameters == null)
				return null;

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
		/// <param name="includeReturnValueParameter">A boolean value indicating
		/// whether the return value parameter should be included in the results.</param>
		/// <returns>An array of the <see cref="IDbDataParameter"/>.</returns>
		public IDbDataParameter[] GetSpParameters(string spName, bool includeReturnValueParameter)
		{
			string key = string.Format("{0}:{1}:{2}", GetConnectionHash(), spName, includeReturnValueParameter);

			IDbDataParameter[] cachedParameters;

			// It is thread safe enought to check for a key and get its value without a lock.
			//
			if (!_paramCache.TryGetValue(key, out cachedParameters))
			{
				lock (_paramCacheLock)
				{
					// There is a possible race condition since the operation may took a time.
					//
					if (!_paramCache.TryGetValue(key, out cachedParameters))
					{
						cachedParameters = DiscoverSpParameters(spName, includeReturnValueParameter);
						_paramCache.Add(key, cachedParameters);
					}
				}
			}
		
			return CloneParameters(cachedParameters);
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
						throw new ArgumentException(Resources.DbManager_MismatchParameterCount);

					object value = parameterValues[nValues++];

					parameter.Value = value ?? DBNull.Value;
					//_dataProvider.SetParameterType(parameter, value);
				}
			}

			// We must have the same number of values as we pave parameters to put them in.
			//
			if (nValues != parameterValues.Length)
				throw new ArgumentException(Resources.DbManager_MismatchParameterCount);
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
			if (dataRow == null)
				throw new ArgumentNullException("dataRow");

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

			if (_prepared)
				InitParameters(CommandAction.Select);

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
			if (obj == null)
				throw new ArgumentNullException("obj");

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

			if (_prepared)
				InitParameters(CommandAction.Select);

			return this;
		}

		private static Array SortArray(Array array, IComparer comparer)
		{
			if (array == null)
				return null;

			Array arrayClone = (Array)array.Clone();

			Array.Sort(arrayClone, comparer);

			return arrayClone;
		}

		/// <summary>
		/// Creates an array of parameters from the <see cref="DataRow"/> object.
		/// </summary>
		/// <remarks>
		/// The method can take an additional parameter list, 
		/// which can be created by using the same method.
		/// </remarks>
		/// <include file="Examples.xml" path='examples/db[@name="CreateParameters(DataRow,IDbDataParameter[])"]/*' />
		/// <param name="dataRow">The <see cref="DataRow"/> to create parameters.</param>
		/// <param name="commandParameters">An array of parameters to be added to the result array.</param>
		/// <returns>An array of parameters.</returns>
		public IDbDataParameter[] CreateParameters(
			DataRow dataRow, params IDbDataParameter[] commandParameters)
		{
			return CreateParameters(dataRow, null, null, null, commandParameters);
		}

		/// <summary>
		/// Creates an array of parameters from the <see cref="DataRow"/> object.
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
		/// <param name="commandParameters">An array of parameters to be added to the result array.</param>
		/// <returns>An array of parameters.</returns>
		public IDbDataParameter[] CreateParameters(
			DataRow                   dataRow,
			string[]                  outputParameters,
			string[]                  inputOutputParameters,
			string[]                  ignoreParameters,
			params IDbDataParameter[] commandParameters)
		{
			if (dataRow == null)
				throw new ArgumentNullException("dataRow");

			List<IDbDataParameter> paramList = new List<IDbDataParameter>();
			IComparer              comparer  = CaseInsensitiveComparer.Default;

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

			return paramList.ToArray();
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
		/// <param name="commandParameters">An array of parameters to be added to the result array.</param>
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
		/// <param name="commandParameters">An array of parameters to be added to the result array.</param>
		/// <returns>An array of parameters.</returns>
		public IDbDataParameter[] CreateParameters(
			object                    obj,
			string[]                  outputParameters,
			string[]                  inputOutputParameters,
			string[]                  ignoreParameters,
			params IDbDataParameter[] commandParameters)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");

			bool                   isType    = obj is Type;
			Type                   type      = isType? (Type)obj: obj.GetType();
			ObjectMapper           om        = _mappingSchema.GetObjectMapper(type);
			List<IDbDataParameter> paramList = new List<IDbDataParameter>();
			IComparer              comparer  = CaseInsensitiveComparer.Default;

			outputParameters       = (string[])SortArray(outputParameters,      comparer);
			inputOutputParameters  = (string[])SortArray(inputOutputParameters, comparer);
			ignoreParameters       = (string[])SortArray(ignoreParameters,      comparer);

			foreach (MemberMapper mm in om)
			{
				if (ignoreParameters != null && Array.BinarySearch(ignoreParameters, mm.Name, comparer) >= 0)
					continue;
				
				object value = isType? null: mm.GetValue(obj);
				string name  = _dataProvider.Convert(mm.Name, ConvertType.NameToParameter).ToString();

				IDbDataParameter parameter   = mm.MapMemberInfo.Nullable || value == null?
					NullParameter(name, value, mm.MapMemberInfo.NullValue): (mm.DbType != DbType.Object)?
					Parameter    (name, value, mm.DbType ):
					Parameter    (name, value);

				if (outputParameters != null && Array.BinarySearch(outputParameters, mm.Name, comparer) >= 0)
					parameter.Direction = ParameterDirection.Output;
				else if (inputOutputParameters != null && Array.BinarySearch(inputOutputParameters, mm.Name, comparer) >= 0)
					parameter.Direction = ParameterDirection.InputOutput;

				paramList.Add(parameter);
			}

			if (commandParameters != null)
				paramList.AddRange(commandParameters);

			return paramList.ToArray();
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
									ma.SetValue(obj, _mappingSchema.ConvertChangeType(parameter.Value, ma.Type));
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
					dest.SetValue(obj, ordinal, _mappingSchema.ConvertChangeType(parameter.Value, dest.GetFieldType(ordinal)));
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
		public void MapOutputParameters(string returnValueMember, params object[] objects)
		{
			if (objects == null)
				return;

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
		/// <param name="value">The <see cref="System.Object"/>
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
		/// If the parameter is null, it's converted to <see cref="DBNull"/>.<see cref="DBNull.Value"/>.
		/// </remarks>
		/// <include file="Examples1.xml" path='examples/db[@name="NullParameter(string,object)"]/*' />
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">The <see cref="System.Object"/>
		/// that is the value of the parameter.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter NullParameter(string parameterName, object value)
		{
			if (_mappingSchema.IsNull(value))
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
		/// <param name="value">The <see cref="System.Object"/>
		/// that is the value of the parameter.</param>
		/// <param name="nullValue">The null equivalent to compare with the value.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
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
		/// <param name="value">The <see cref="System.Object"/>
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
		/// <param name="value">The <see cref="System.Object"/>
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
		/// Adds an output parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the
		/// <see cref="System.Data.ParameterDirection">ParameterDirection.Output</see> type.
		/// </remarks>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="dbType">One of the <see cref="DbType"/> values.</param>
		/// <param name="size">Size of the parameter.</param>
		/// that is the value of the parameter.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter OutputParameter(string parameterName, DbType dbType, int size)
		{
			return Parameter(ParameterDirection.Output, parameterName, dbType, size);
		}

		/// <summary>
		/// Adds an input-output parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the
		/// <see cref="System.Data.ParameterDirection">ParameterDirection.InputOutput</see> type.
		/// </remarks>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">The <see cref="System.Object"/>
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
		/// <see cref="System.Data.ParameterDirection"/> type.
		/// </remarks>
		/// <param name="parameterDirection">One of the <see cref="System.Data.ParameterDirection"/> values.</param>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">The <see cref="System.Object"/>
		/// that is the value of the parameter.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter Parameter(
			ParameterDirection parameterDirection,
			string             parameterName,
			object             value)
		{
			IDbDataParameter parameter = _dataProvider.CreateParameterObject(Command);

			parameter.ParameterName = parameterName;
			parameter.Direction     = parameterDirection;

			//_dataProvider.SetParameterType(parameter, value);

			parameter.Value = value ?? DBNull.Value;

			return parameter;
		}

		/// <summary>
		/// Adds a parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the specified
		/// <see cref="System.Data.ParameterDirection"/> type.
		/// </remarks>
		/// <param name="parameterDirection">One of the <see cref="System.Data.ParameterDirection"/> values.</param>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">The <see cref="System.Object"/>
		/// that is the value of the parameter.</param>
		/// <param name="dbType">One of the <seealso cref="DbType"/> values.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter Parameter(
			ParameterDirection parameterDirection,
			string             parameterName,
			object             value,
			DbType             dbType)
		{
			IDbDataParameter parameter = _dataProvider.CreateParameterObject(Command);

			parameter.ParameterName = parameterName;
			parameter.Direction     = parameterDirection;
			parameter.DbType        = dbType;
			parameter.Value         = value;

			return parameter;
		}

		/// <summary>
		/// Adds an input parameter to the <see cref="Command"/>.
		/// </summary>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">The <see cref="System.Object"/>
		/// that is the value of the parameter.</param>
		/// <param name="dbType">One of the <seealso cref="DbType"/> values.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter Parameter(
			string parameterName,
			object value,
			DbType dbType)
		{
			return Parameter(ParameterDirection.Input, parameterName, value, dbType);
		}

		/// <summary>
		/// Adds a parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the specified
		/// <see cref="System.Data.ParameterDirection"/> type.
		/// </remarks>
		/// <param name="parameterDirection">One of the <see cref="System.Data.ParameterDirection"/> values.</param>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">The <see cref="System.Object"/>
		/// that is the value of the parameter.</param>
		/// <param name="dbType">One of the <seealso cref="DbType"/> values.</param>
		/// <param name="size">Size of the parameter.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter Parameter(
			ParameterDirection parameterDirection,
			string             parameterName,
			object             value,
			DbType             dbType,
			int                size)
		{
			IDbDataParameter parameter = _dataProvider.CreateParameterObject(Command);

			parameter.ParameterName = parameterName;
			parameter.Direction     = parameterDirection;
			parameter.DbType        = dbType;
			parameter.Size          = size;
			parameter.Value         = value;

			return parameter;
		}

		/// <summary>
		/// Adds an input parameter to the <see cref="Command"/>.
		/// </summary>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="value">The <see cref="System.Object"/>
		/// that is the value of the parameter.</param>
		/// <param name="dbType">One of the <seealso cref="DbType"/> values.</param>
		/// <param name="size">Size of the parameter.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
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
		/// <see cref="System.Data.ParameterDirection"/> type.
		/// </remarks>
		/// <param name="parameterDirection">One of the <see cref="System.Data.ParameterDirection"/> values.</param>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="dbType">One of the <seealso cref="DbType"/> values.</param>
		/// <returns>The <see cref="IDbDataParameter"/> object.</returns>
		public IDbDataParameter Parameter(
			ParameterDirection parameterDirection,
			string             parameterName,
			DbType             dbType)
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
		/// <see cref="System.Data.ParameterDirection"/> type.
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
		/// Creates an input parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the
		/// <see cref="System.Data.ParameterDirection">ParameterDirection.Input</see> type
		/// and <see cref="System.Data.DataRowVersion">DataRowVersion.Current</see> type.
		/// </remarks>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="dbType">One of the <see cref="DbType"/> values.</param>
		/// <param name="size">Size of the parameter.</param>
		/// <param name="sourceColumn">Source column for a parameter in the <see cref="DataTable"/>.</param>
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
		/// Creates an input parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the
		/// <see cref="System.Data.ParameterDirection">ParameterDirection.Input</see> type
		/// and <see cref="System.Data.DataRowVersion">DataRowVersion.Current</see> type.
		/// </remarks>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="dbType">One of the <see cref="DbType"/> values.</param>
		/// <param name="sourceColumn">Source column for a parameter in the <see cref="DataTable"/>.</param>
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
		/// Creates an input parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the
		/// <see cref="System.Data.ParameterDirection">ParameterDirection.Input</see> type
		/// and <see cref="System.Data.DataRowVersion">DataRowVersion.Current</see> type.
		/// </remarks>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="dbType">One of the <see cref="DbType"/> values.</param>
		/// <param name="size">Size of the parameter.</param>
		/// <param name="sourceColumn">Source column for a parameter in the <see cref="DataTable"/>.</param>
		/// <param name="dataRowVersion">Version of data to use for a parameter in the <see cref="DataTable"/>.</param>
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
		/// Creates an input parameter to the <see cref="Command"/>.
		/// </summary>
		/// <remarks>
		/// The method creates a parameter with the
		/// <see cref="System.Data.ParameterDirection">ParameterDirection.Input</see> type
		/// and <see cref="System.Data.DataRowVersion">DataRowVersion.Current</see> type.
		/// </remarks>
		/// <param name="parameterName">The name of the parameter.</param>
		/// <param name="dbType">One of the <see cref="DbType"/> values.</param>
		/// <param name="sourceColumn">Source column for a parameter in the <see cref="DataTable"/>.</param>
		/// <param name="dataRowVersion">Version of data to use for a parameter in the <see cref="DataTable"/>.</param>
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
			return SetCommand(commandAction, CommandType.StoredProcedure,
					spName, CreateSpParameters(spName, parameterValues));
		}

		#region Select

		/// <summary>
		/// Creates an SQL statement.
		/// </summary>
		/// <param name="commandText">The command text to execute.</param>
		/// <returns>Current instance.</returns>
		public DbManager SetCommand(
			string commandText)
		{
			return SetCommand(
				CommandAction.Select, CommandType.Text, commandText, null);
		}

		/// <summary>
		/// Creates an SQL statement.
		/// </summary>
		/// <param name="commandType">The <see cref="System.Data.CommandType"/> (stored procedure, text, etc.)</param>
		/// <param name="commandText">The command text to execute.</param>
		/// <returns>Current instance.</returns>
		public DbManager SetCommand(
			CommandType commandType,
			string      commandText)
		{
			return SetCommand(
				CommandAction.Select, commandType, commandText, null);
		}

		/// <summary>
		/// Creates an SQL statement.
		/// </summary>
		/// <remarks>
		/// The method can be used to create the <i>INSERT</i>, <i>UPDATE</i>, and <i>DELETE</i> SQL statements.
		/// </remarks>
		/// <param name="commandText">The command text to execute.</param>
		/// <param name="commandParameters">An array of parameters used to executes the command.</param>
		/// <returns>Current instance.</returns>
		public DbManager SetCommand(
			string commandText,
			params IDbDataParameter[] commandParameters)
		{
			return SetCommand(
				CommandAction.Select, CommandType.Text, commandText, commandParameters);
		}

		/// <summary>
		/// Creates an SQL statement.
		/// </summary>
		/// <param name="commandType">The <see cref="System.Data.CommandType"/> (stored procedure, text, etc.)</param>
		/// <param name="commandText">The command text to execute.</param>
		/// <param name="commandParameters">An array of parameters used to executes the command.</param>
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
		/// <param name="spName">The name of the stored procedure</param>
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
		/// <param name="commandParameters">An array of parameters used to executes the command.</param>
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
		/// <param name="commandType">The <see cref="System.Data.CommandType"/> (stored procedure, text, etc.)</param>
		/// <param name="commandText">The command text to execute.</param>
		/// <param name="commandParameters">An array of parameters used to executes the command.</param>
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
		/// <param name="spName">The name of the stored procedure</param>
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
		/// <param name="commandParameters">An array of parameters used to executes the command.</param>
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
		/// <param name="commandType">The <see cref="System.Data.CommandType"/> (stored procedure, text, etc.)</param>
		/// <param name="commandText">The command text to execute.</param>
		/// <param name="commandParameters">An array of parameters used to executes the command.</param>
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
		/// <param name="spName">The name of the stored procedure</param>
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
		/// <param name="commandParameters">An array of parameters used to executes the command.</param>
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
		/// <param name="commandType">The <see cref="System.Data.CommandType"/> (stored procedure, text, etc.)</param>
		/// <param name="commandText">The command text to execute.</param>
		/// <param name="commandParameters">An array of parameters used to executes the command.</param>
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
		/// <param name="spName">The name of the stored procedure</param>
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

		private void PrepareCommand(
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
				ExecuteOperation(OperationType.PrepareCommand, command.Prepare);
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
					// This mysterious line of code required to fix a bug in MsSql.
					// It forces parameter's filed 'MetaType' to be set.
					// Same for p.Size = p.Size below.
					//
					p.DbType = p.DbType;

					if (p.Value is string)
					{
						int len = ((string)p.Value).Length;

						if (p.Size < len)
						{
							p.Size  = len;
							prepare = true;
						}
						else
							p.Size = p.Size;
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
						else
							p.Size  = p.Size;
					}
					else if (p.Value is char[])
					{
						int len = ((char[])p.Value).Length;

						if (p.Size < len)
						{
							p.Size  = len;
							prepare = true;
						}
						else
							p.Size  = p.Size;
					}
					else if (p.Value is decimal)
					{
						SqlDecimal d = (decimal)p.Value;
						if (p.Precision < d.Precision)
						{
							p.Precision = d.Precision;
							prepare = true;
						}
						else
							p.Precision = p.Precision;

						if (p.Scale < d.Scale)
						{
							p.Scale = d.Scale;
							prepare = true;
						}
						else
							p.Scale = p.Scale;
					}
				}

				// Re-prepare command to avoid truncation.
				//
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
		/// Executes an SQL statement for a given collection of objects and 
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
			int rowsTotal = 0;

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
					int rows = ExecuteNonQueryInternal();
					if (rows > 0)
						rowsTotal += rows;
				}
			}
		
			return rowsTotal;
		}

		/// <summary>
		/// Executes an SQL statement for a given collection of objects and 
		/// returns the number of rows affected.
		/// </summary>
		/// <remarks>
		/// The method prepares the <see cref="Command"/> object 
		/// and calls the <see cref="ExecuteNonQuery()"/> method for each item of the list.
		/// </remarks>
		/// <include file="Examples1.xml" path='examples/db[@name="Execute(CommandType,string,IList)"]/*' />
		/// <param name="collection">The list of objects used to execute the command.</param>
		/// <returns>The number of rows affected by the command.</returns>
		public int ExecuteForEach<T>(ICollection<T> collection)
		{
			int rowsTotal = 0;

			if (collection != null && collection.Count != 0)
			{
				bool initParameters = true;

				foreach (T o in collection)
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
					int rows = ExecuteNonQueryInternal();
					if (rows > 0)
						rowsTotal += rows;
				}
			}

			return rowsTotal;
		}

		/// <summary>
		/// Executes an SQL statement for the <see cref="DataTable"/> and 
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
			int rowsTotal = 0;

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
					int rows = ExecuteNonQueryInternal();
					if (rows > 0)
						rowsTotal += rows;
				}
			}
		
			return rowsTotal;
		}

		/// <summary>
		/// Executes an SQL statement for the first table of the <see cref="DataSet"/> 
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
		/// Executes an SQL statement for the specified table of the <see cref="DataSet"/> 
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
		/// Executes an SQL statement and returns the number of rows affected.
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
		/// Executes an SQL statement and returns the number of rows affected.
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
		/// Executes an SQL statement and returns the number of rows affected.
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
		/// Executes an SQL statement and returns the number of rows affected.
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
		/// Executes an SQL statement and returns the number of rows affected.
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

			return ExecuteOperation<object>(OperationType.ExecuteScalar, SelectCommand.ExecuteScalar);
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

		#endregion

		#region ExecuteScalarList

		/// <summary>
		/// Executes the query, and returns the array list of values of the
		/// specified column of  the every row in the resultset returned by the
		/// query. Other columns are ignored.
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
			if (list == null)
				list = new ArrayList();

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
		/// Other columns are ignored.
		/// </summary>
		/// <param name="list">The array to fill in.</param>
		/// <param name="type">The type of the each element.</param>
		/// <returns>Array list of values of first column of the every row in
		/// the resultset.</returns>
		public IList ExecuteScalarList(IList list, Type type)
		{
			if (list == null)
				list = new ArrayList();

			return ExecuteScalarList(list, type, 0);
		}

		/// <summary>
		/// Executes the query, and returns the array list of values of the
		/// specified column of  the every row in the resultset returned by the
		/// query. Other columns are ignored.
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
		/// Other columns are ignored.
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

		/// <summary>
		/// Executes the query, and returns the array list of values of the
		/// specified column of  the every row in the resultset returned by the
		/// query. Other columns are ignored.
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
			if (list == null)
				list = new List<T>();

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
		/// Other columns are ignored.
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
		/// Executes the query, and returns a list of values of the
		/// specified column of the every row in the resultset returned by the
		/// query. Other columns are ignored.
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
		/// Executes the query, and returns the list of values of first
		/// column of the every row in the resultset returned by the query.
		/// Other columns are ignored.
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

		#endregion

		#region ExecuteScalarDictionary

		///<summary>
		/// Executes the query, and returns the dictionary.
		/// The keys are loaded from a column specified by <paramref name="keyField"/> and
		/// values are loaded from a column specified by <paramref name="valueField"/>.
		/// Other columns are ignored.
		///</summary>
		///<param name="dic">The dictionary to add values.</param>
		///<param name="keyField">The column name/index to load keys.</param>
		///<param name="keyFieldType">The key type.</param>
		///<param name="valueField">The column name/index to load values.</param>
		///<param name="valueFieldType">The value type.</param>
		///<returns>The loaded dictionary.</returns>
		public IDictionary ExecuteScalarDictionary(
			IDictionary dic,
			NameOrIndexParameter keyField,   Type keyFieldType,
			NameOrIndexParameter valueField, Type valueFieldType)
		{
			if (dic == null)
				dic = new Hashtable();

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

		///<summary>
		/// Executes the query, and returns the dictionary.
		/// The keys are loaded from a column specified by <paramref name="keyField"/> and
		/// values are loaded from a column specified by <paramref name="valueField"/>.
		/// Other columns are ignored.
		///</summary>
		///<param name="keyField">The column name/index to load keys.</param>
		///<param name="keyFieldType">The key type.</param>
		///<param name="valueField">The column name/index to load values.</param>
		///<param name="valueFieldType">The value type.</param>
		///<returns>The loaded dictionary.</returns>
		public Hashtable ExecuteScalarDictionary(
			NameOrIndexParameter keyField,   Type keyFieldType,
			NameOrIndexParameter valueField, Type valueFieldType)
		{
			Hashtable table = new Hashtable();

			ExecuteScalarDictionary(table, keyField, keyFieldType, valueField, valueFieldType);

			return table;
		}

		///<summary>
		/// Executes the query, and returns the dictionary.
		/// The keys are loaded from a column specified by <paramref name="keyField"/> and
		/// values are loaded from a column specified by <paramref name="valueField"/>.
		/// Other columns are ignored.
		///</summary>
		///<typeparam name="K">The key type.</typeparam>
		///<typeparam name="T">The value type.</typeparam>
		///<param name="dic">The dictionary to add values.</param>
		///<param name="keyField">The column name/index to load keys.</param>
		///<param name="valueField">The column name/index to load values.</param>
		///<returns>The loaded dictionary.</returns>
		public IDictionary<K,T> ExecuteScalarDictionary<K,T>(
			IDictionary<K,T>     dic,
			NameOrIndexParameter keyField,
			NameOrIndexParameter valueField)
		{
			if (dic == null)
				dic = new Dictionary<K,T>();

			if (_prepared)
				InitParameters(CommandAction.Select);

			//object nullValue = _mappingSchema.GetNullValue(type);

			Type keyFieldType   = typeof(K);
			Type valueFieldType = typeof(T);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				if (dr.Read())
				{
					int keyIndex   = keyField.ByName?   dr.GetOrdinal(keyField.Name):   keyField.Index;
					int valueIndex = valueField.ByName? dr.GetOrdinal(valueField.Name): valueField.Index;

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

		///<summary>
		/// Executes the query, and returns the dictionary.
		/// The keys are loaded from a column specified by <paramref name="keyField"/> and
		/// values are loaded from a column specified by <paramref name="valueField"/>.
		/// Other columns are ignored.
		///</summary>
		///<typeparam name="K">The key type.</typeparam>
		///<typeparam name="T">The value type.</typeparam>
		///<param name="keyField">The column name/index to load keys.</param>
		///<param name="valueField">The column name/index to load values.</param>
		///<returns>The loaded dictionary.</returns>
		public Dictionary<K,T> ExecuteScalarDictionary<K,T>(
			NameOrIndexParameter keyField,
			NameOrIndexParameter valueField)
		{
			Dictionary<K,T> dic = new Dictionary<K,T>();

			ExecuteScalarDictionary<K,T>(dic, keyField, valueField);

			return dic;
		}

		#endregion

		#region ExecuteScalarDictionary (Index)

		///<summary>
		/// Executes the query, and returns the dictionary.
		/// The keys are loaded from columns specified by <paramref name="index"/> and
		/// values are loaded from a column specified by <paramref name="valueField"/>.
		/// Other columns are ignored.
		///</summary>
		///<param name="dic">The dictionary to add values.</param>
		///<param name="index">The <see cref="MapIndex"/> of the key columns.</param>
		///<param name="valueField">The column name/index to load values.</param>
		///<param name="valueFieldType">The value type.</param>
		///<returns>The loaded dictionary.</returns>
		public IDictionary ExecuteScalarDictionary(
			IDictionary          dic,
			MapIndex             index,
			NameOrIndexParameter valueField,
			Type                 valueFieldType)
		{
			if (dic == null)
				dic = new Hashtable();

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

		///<summary>
		/// Executes the query, and returns the dictionary.
		/// The keys are loaded from columns specified by <paramref name="index"/> and
		/// values are loaded from a column specified by <paramref name="valueField"/>.
		/// Other columns are ignored.
		///</summary>
		///<param name="index">The <see cref="MapIndex"/> of the key columns.</param>
		///<param name="valueField">The column name/index to load values.</param>
		///<param name="valueFieldType">The value type.</param>
		///<returns>The loaded dictionary.</returns>
		public Hashtable ExecuteScalarDictionary(
			MapIndex index, NameOrIndexParameter valueField, Type valueFieldType)
		{
			Hashtable table = new Hashtable();

			ExecuteScalarDictionary(table, index, valueField, valueFieldType);

			return table;
		}

		///<summary>
		/// Executes the query, and returns the dictionary.
		/// The keys are loaded from columns specified by <paramref name="index"/> and
		/// values are loaded from a column specified by <paramref name="valueField"/>.
		/// Other columns are ignored.
		///</summary>
		///<typeparam name="T">The value type.</typeparam>
		///<param name="dic">The dictionary to add values.</param>
		///<param name="index">The <see cref="MapIndex"/> of the key columns.</param>
		///<param name="valueField">The column name/index to load values.</param>
		///<returns>The loaded dictionary.</returns>
		public IDictionary<CompoundValue,T> ExecuteScalarDictionary<T>(
			IDictionary<CompoundValue, T> dic, MapIndex index, NameOrIndexParameter valueField)
		{
			if (dic == null)
				dic = new Dictionary<CompoundValue, T>();

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

		///<summary>
		/// Executes the query, and returns the dictionary.
		/// The keys are loaded from columns specified by <paramref name="index"/> and
		/// values are loaded from a column specified by <paramref name="valueField"/>.
		/// Other columns are ignored.
		///</summary>
		///<typeparam name="T">The value type.</typeparam>
		///<param name="index">The <see cref="MapIndex"/> of the key columns.</param>
		///<param name="valueField">The column name/index to load values.</param>
		///<returns>The loaded dictionary.</returns>
		public Dictionary<CompoundValue,T> ExecuteScalarDictionary<T>(
			MapIndex index, NameOrIndexParameter valueField)
		{
			Dictionary<CompoundValue,T> dic = new Dictionary<CompoundValue,T>();

			ExecuteScalarDictionary<T>(dic, index, valueField);

			return dic;
		}

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
		/// Executes an SQL statement using the provided parameters.
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
		/// Executes an SQL statement using the provided parameters.
		/// </summary>
		/// <remarks>
		/// See the <see cref="ExecuteDataSet(NameOrIndexParameter)"/> method
		/// to find an example.
		/// </remarks>
		/// <param name="dataSet">The input <see cref="DataSet"/> object.</param>
		/// <returns>The <see cref="DataSet"/>.</returns>
		public DataSet ExecuteDataSet(
			DataSet dataSet)
		{
			return ExecuteDataSet(dataSet, 0, 0, "Table");
		}

		/// <summary>
		/// Executes an SQL statement using the provided parameters.
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
		/// Executes an SQL statement using the provided parameters.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> object to populate.</param>
		/// <param name="table">The name or index of the populating table.</param>
		/// <returns>The <see cref="DataSet"/>.</returns>
		public DataSet ExecuteDataSet(
			DataSet              dataSet,
			NameOrIndexParameter table)
		{
			return ExecuteDataSet(dataSet, 0, 0, table);
		}

		/// <summary>
		/// Executes an SQL statement using the provided parameters.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> object to populate.</param>
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

			ExecuteOperation(OperationType.Fill, delegate
			{
				if (table.ByName)
					da.Fill(dataSet, startRecord, maxRecords, table.Name);
				else
					da.Fill(startRecord, maxRecords, dataSet.Tables[table.Index]);
			});

			return dataSet;
		}

		#endregion

		#region ExecuteDataTable

		/// <summary>
		/// Executes an SQL statement using the provided parameters.
		/// </summary>
		/// <returns>The <see cref="DataTable"/>.</returns>
		public DataTable ExecuteDataTable()
		{
			return ExecuteDataTable(null);
		}

		/// <summary>
		/// Executes an SQL statement using the provided parameters.
		/// </summary>
		/// <param name="dataTable">The <see cref="DataTable"/> object to populate.</param>
		/// <returns>The <see cref="DataTable"/>.</returns>
		public DataTable ExecuteDataTable(DataTable dataTable)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			if (dataTable == null)
				dataTable = new DataTable();

			DbDataAdapter da = _dataProvider.CreateDataAdapterObject();
			((IDbDataAdapter)da).SelectCommand = SelectCommand;

			ExecuteOperation(OperationType.Fill, delegate { da.Fill(dataTable); });
			return dataTable;
		}

		/// <summary>Adds or refreshes rows in a <see cref="System.Data.DataTable"/>
		/// to match those in the data source starting at the specified record
		/// and retrieving up to the specified maximum number of records.
		/// </summary>
		/// <param name="startRecord">The zero-based record number to start with.</param>
		/// <param name="maxRecords">The maximum number of records to retrieve.</param>
		/// <param name="tableList">The <see cref="System.Data.DataTable"/> objects
		/// to fill from the data source.</param>
		public void ExecuteDataTables(
			int                startRecord,
			int                maxRecords,
			params DataTable[] tableList)
		{
			if (tableList == null || tableList.Length == 0)
				return;

			if (_prepared)
				InitParameters(CommandAction.Select);

			DbDataAdapter da = _dataProvider.CreateDataAdapterObject();
			((IDbDataAdapter)da).SelectCommand = SelectCommand;

			ExecuteOperation(OperationType.Fill, delegate { da.Fill(startRecord, maxRecords, tableList); });
		}

		/// <summary>Adds or refreshes rows in a <see cref="System.Data.DataTable"/>
		/// to match those in the data source starting at the specified record
		/// and retrieving up to the specified maximum number of records.
		/// </summary>
		/// <param name="tableList">The <see cref="System.Data.DataTable"/> objects
		/// to fill from the data source.</param>
		public void ExecuteDataTables(params DataTable[] tableList)
		{
			ExecuteDataTables(0, 0, tableList);
		}

		#endregion

		#region ExecuteObject

		/// <summary>
		/// Executes an SQL statement and maps resultset to an object.
		/// </summary>
		/// <param name="entity">An object to populate.</param>
		/// <param name="type">The System.Type of the object.</param>
		/// <param name="parameters">Additional parameters passed to object constructor through <see cref="InitContext"/>.</param>
		/// <returns>A business object.</returns>
		private object ExecuteObjectInternal(object entity, Type type, object[] parameters)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal(CommandBehavior.SingleRow))
			{
				if (dr.Read())
				{
					return entity == null?
						_mappingSchema.MapDataReaderToObject(dr, type,   parameters):
						_mappingSchema.MapDataReaderToObject(dr, entity, parameters);
				}

				return null;
			}
		}

		/// <summary>
		/// Executes an SQL statement and maps resultset to an object.
		/// </summary>
		/// <param name="entity">An object to populate.</param>
		/// <returns>A business object.</returns>
		public object ExecuteObject(object entity)
		{
			if (null == entity)
				throw new ArgumentNullException("entity");

			return ExecuteObjectInternal(entity, entity.GetType(), null);
		}

		/// <summary>
		/// Executes an SQL statement and maps resultset to an object.
		/// </summary>
		/// <param name="entity">An object to populate.</param>
		/// <param name="parameters">Additional parameters passed to object constructor through <see cref="InitContext"/>.</param>
		/// <returns>A business object.</returns>
		public object ExecuteObject(object entity, params object[] parameters)
		{
			if (null == entity)
				throw new ArgumentNullException("entity");

			return ExecuteObjectInternal(entity, entity.GetType(), parameters);
		}

		/// <summary>
		/// Executes an SQL statement and maps resultset to an object.
		/// </summary>
		/// <param name="type">Type of an object.</param>
		/// <returns>A business object.</returns>
		public object ExecuteObject(Type type)
		{
			return ExecuteObjectInternal(null, type, null);
		}

		/// <summary>
		/// Executes an SQL statement and maps resultset to an object.
		/// </summary>
		/// <param name="type">Type of an object.</param>
		/// <param name="parameters">Additional parameters passed to object constructor through <see cref="InitContext"/>.</param>
		/// <returns>A business object.</returns>
		public object ExecuteObject(Type type, params object[] parameters)
		{
			return ExecuteObjectInternal(null, type, parameters);
		}

		/// <summary>
		/// Executes an SQL statement and maps resultset to an object.
		/// </summary>
		/// <typeparam name="T">Type of an object.</typeparam>
		/// <returns>A business object.</returns>
		public T ExecuteObject<T>()
		{
			return (T)ExecuteObjectInternal(null, typeof(T), null);
		}

		/// <summary>
		/// Executes an SQL statement and maps resultset to an object.
		/// </summary>
		/// <typeparam name="T">Type of an object.</typeparam>
		/// <param name="parameters">Additional parameters passed to object constructor through <see cref="InitContext"/>.</param>
		/// <returns>A business object.</returns>
		public T ExecuteObject<T>(params object[] parameters)
		{
			return (T)ExecuteObjectInternal(null, typeof(T), parameters);
		}

		#endregion

		#region ExecuteList

		private IList ExecuteListInternal(IList list, Type type, params object[] parameters)
		{
			if (list == null)
				list = new ArrayList();

			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				return _mappingSchema.MapDataReaderToList(dr, list, type, parameters);
			}
		}

		private void ExecuteListInternal<T>(IList<T> list, params object[] parameters)
		{
			if (list == null)
				list = new List<T>();

			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				_mappingSchema.MapDataReaderToList<T>(dr, list, parameters);
			}
		}

		/// <summary>
		/// Executes the query, and returns an array of business entities using the provided parameters.
		/// </summary>
		/// <param name="type">Type of the business object.</param>
		/// <returns>An array of business objects.</returns>
		public ArrayList ExecuteList(Type type)
		{
			ArrayList arrayList = new ArrayList();

			ExecuteListInternal(arrayList, type, null);

			return arrayList;
		}

		/// <summary>
		/// Executes the query, and returns an array of business entities.
		/// </summary>
		/// <typeparam name="T">Type of an object.</typeparam>
		/// <returns>Populated list of mapped business objects.</returns>
		public List<T> ExecuteList<T>()
		{
			List<T> list = new List<T>();

			ExecuteListInternal<T>(list, null);

			return list;
		}

		/// <summary>
		/// Executes the query, and returns an array of business entities using the provided parameters.
		/// </summary>
		/// <param name="type">Type of the business object.</param>
		/// <param name="parameters">Additional parameters passed to object constructor through <see cref="InitContext"/>.</param>
		/// <returns>An array of business objects.</returns>
		public ArrayList ExecuteList(Type type, params object[] parameters)
		{
			ArrayList arrayList = new ArrayList();

			ExecuteListInternal(arrayList, type, parameters);

			return arrayList;
		}

		/// <summary>
		/// Executes the query, and returns an array of business entities.
		/// </summary>
		/// <typeparam name="T">Type of an object.</typeparam>
		/// <param name="parameters">Additional parameters passed to object constructor through <see cref="InitContext"/>.</param>
		/// <returns>Populated list of mapped business objects.</returns>
		public List<T> ExecuteList<T>(params object[] parameters)
		{
			List<T> list = new List<T>();

			ExecuteListInternal<T>(list, parameters);

			return list;
		}

		/// <summary>
		/// Executes the query, and returns an array of business entities.
		/// </summary>
		/// <param name="list">The list of mapped business objects to populate.</param>
		/// <param name="type">Type of an object.</param>
		/// <returns>Populated list of mapped business objects.</returns>
		public IList ExecuteList(IList list, Type type)
		{
			return ExecuteListInternal(list, type, null);
		}

		/// <summary>
		/// Executes the query, and returns an array of business entities.
		/// </summary>
		/// <typeparam name="T">Type of an object.</typeparam>
		/// <param name="list">The list of mapped business objects to populate.</param>
		/// <returns>Populated list of mapped business objects.</returns>
		public IList<T> ExecuteList<T>(IList<T> list) 
		{
			ExecuteListInternal<T>(list, null);

			return list;
		}

		/// <summary>
		/// Executes the query, and returns an array of business entities.
		/// </summary>
		/// <param name="list">The list of mapped business objects to populate.</param>
		/// <param name="type">Type of an object.</param>
		/// <param name="parameters">Additional parameters passed to object constructor through <see cref="InitContext"/>.</param>
		/// <returns>Populated list of mapped business objects.</returns>
		public IList ExecuteList(IList list, Type type, params object[] parameters)
		{
			return ExecuteListInternal(list, type, parameters);
		}

		/// <summary>
		/// Executes the query, and returns an array of business entities.
		/// </summary>
		/// <typeparam name="T">Type of an object.</typeparam>
		/// <param name="list">The list of mapped business objects to populate.</param>
		/// <param name="parameters">Additional parameters passed to object constructor through <see cref="InitContext"/>.</param>
		/// <returns>Populated list of mapped business objects.</returns>
		public IList<T> ExecuteList<T>(IList<T> list, params object[] parameters)
		{
			ExecuteListInternal<T>(list, parameters);

			return list;
		}

		/// <summary>
		/// Executes the query, and returns an array of business entities.
		/// </summary>
		/// <typeparam name="L">Type of a list.</typeparam>
		/// <typeparam name="T">Type of an object.</typeparam>
		/// <param name="list">The list of mapped business objects to populate.</param>
		/// <param name="parameters">Additional parameters passed to object constructor through <see cref="InitContext"/>.</param>
		/// <returns>Populated list of mapped business objects.</returns>
		public L ExecuteList<L,T>(L list, params object[] parameters)
			where L : IList<T>
		{
			ExecuteListInternal(list, typeof(T), parameters);

			return list;
		}

		/// <summary>
		/// Executes the query, and returns an array of business entities.
		/// </summary>
		/// <typeparam name="L">Type of a list.</typeparam>
		/// <typeparam name="T">Type of an object.</typeparam>
		/// <param name="parameters">Additional parameters passed to object constructor through <see cref="InitContext"/>.</param>
		/// <returns>Populated list of mapped business objects.</returns>
		public L ExecuteList<L,T>(params object[] parameters)
			where L : IList<T>, new()
		{
			L list = new L();

			ExecuteListInternal(list, typeof(T), parameters);

			return list;
		}

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
			if (dictionary == null)
				dictionary = new Hashtable();

			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				return _mappingSchema.MapDataReaderToDictionary(dr, dictionary, keyField, type, parameters);
			}
		}

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
			Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

			ExecuteDictionary<TKey, TValue>(dictionary, keyField, typeof(TValue), parameters);

			return dictionary;
		}

		/// <summary>
		/// Executes the query, and returns the <see cref="Hashtable"/> of business entities.
		/// </summary>
		/// <param name="dictionary">A dictionary of mapped business objects to populate.</param>
		/// <param name="keyField">The field name or index that is used as a key to populate <see cref="IDictionary"/>.</param>
		/// <param name="parameters">Any additional parameters passed to the constructor with <see cref="InitContext"/> parameter.</param>
		/// <returns>An instance of the <see cref="IDictionary"/>.</returns>
		public IDictionary<TKey, TValue> ExecuteDictionary<TKey, TValue>(
			IDictionary<TKey, TValue> dictionary,
			NameOrIndexParameter      keyField,
			params object[]           parameters)
		{
			return ExecuteDictionary<TKey,TValue>(dictionary, keyField, typeof(TValue), parameters);
		}

		/// <summary>
		/// Executes the query, and returns the <see cref="Hashtable"/> of business entities.
		/// </summary>
		/// <param name="dictionary">A dictionary of mapped business objects to populate.</param>
		/// <param name="keyField">The field name or index that is used as a key to populate <see cref="IDictionary"/>.</param>
		/// <param name="destObjectType">Business object type.</param>
		/// <param name="parameters">Any additional parameters passed to the constructor with <see cref="InitContext"/> parameter.</param>
		/// <returns>An instance of the <see cref="IDictionary"/>.</returns>
		public IDictionary<TKey, TValue> ExecuteDictionary<TKey, TValue>(
			IDictionary<TKey, TValue> dictionary,
			NameOrIndexParameter      keyField,
			Type                      destObjectType,
			params object[]           parameters)
		{
			if (dictionary == null)
				dictionary = new Dictionary<TKey, TValue>();

			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				return _mappingSchema.MapDataReaderToDictionary<TKey,TValue>(
					dr, dictionary, keyField, destObjectType, parameters);
			}
		}

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
			if (dictionary == null)
				dictionary = new Hashtable();

			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				return _mappingSchema.MapDataReaderToDictionary(dr, dictionary, index, type, parameters);
			}
		}

		/// <summary>
		/// Executes the query, and returns a dictionary of business entities.
		/// </summary>
		/// <typeparam name="TValue">Value's type.</typeparam>
		/// <param name="index">Dictionary key fields.</param>
		/// <param name="parameters">Any additional parameters passed to the constructor with <see cref="InitContext"/> parameter.</param>
		/// <returns>An instance of the dictionary.</returns>
		public Dictionary<CompoundValue, TValue> ExecuteDictionary<TValue>(
			MapIndex        index,
			params object[] parameters)
		{
			Dictionary<CompoundValue, TValue> dictionary = new Dictionary<CompoundValue, TValue>();

			ExecuteDictionary<TValue>(dictionary, index, typeof(TValue), parameters);

			return dictionary;
		}

		/// <summary>
		/// Executes the query, and returns a dictionary of business entities.
		/// </summary>
		/// <typeparam name="TValue">Value's type.</typeparam>
		/// <param name="dictionary">A dictionary of mapped business objects to populate.</param>
		/// <param name="index">Dictionary key fields.</param>
		/// <param name="parameters">Any additional parameters passed to the constructor with <see cref="InitContext"/> parameter.</param>
		/// <returns>An instance of the dictionary.</returns>
		public IDictionary<CompoundValue, TValue> ExecuteDictionary<TValue>(
			IDictionary<CompoundValue, TValue> dictionary,
			MapIndex                           index,
			params object[]                    parameters)
		{
			return ExecuteDictionary<TValue>(dictionary, index, typeof(TValue), parameters);
		}

		/// <summary>
		/// Executes the query, and returns a dictionary of business entities.
		/// </summary>
		/// <typeparam name="TValue">Value's type.</typeparam>
		/// <param name="dictionary">A dictionary of mapped business objects to populate.</param>
		/// <param name="index">Dictionary key fields.</param>
		/// <param name="destObjectType">Business object type.</param>
		/// <param name="parameters">Any additional parameters passed to the constructor with <see cref="InitContext"/> parameter.</param>
		/// <returns>An instance of the dictionary.</returns>
		public IDictionary<CompoundValue, TValue> ExecuteDictionary<TValue>(
			IDictionary<CompoundValue, TValue> dictionary,
			MapIndex                           index,
			Type                               destObjectType,
			params object[]                    parameters)
		{
			if (dictionary == null)
				dictionary = new Dictionary<CompoundValue, TValue>();

			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				return _mappingSchema.MapDataReaderToDictionary<TValue>(
					dr, dictionary, index, destObjectType, parameters);
			}
		}

		#endregion

		#region ExecuteResultSet

		/// <summary>
		/// Executes the query, and returns multiple results.
		/// </summary>
		/// <param name="resultSets">Array of <see cref="MapResultSet"/> to populate.</param>
		/// <returns>The populated <see cref="MapResultSet"/>.</returns>
		public MapResultSet[] ExecuteResultSet(params MapResultSet[] resultSets)
		{
			if (_prepared)
				InitParameters(CommandAction.Select);

			using (IDataReader dr = ExecuteReaderInternal())
			{
				_mappingSchema.MapDataReaderToResultSet(dr, resultSets);
			}

			return resultSets;
		}

		/// <summary>
		/// Executes the query, and returns multiple results.
		/// </summary>
		/// <param name="masterType">The type of the master business object.</param>
		/// <param name="nextResults">Array of <see cref="MapNextResult"/> to populate.</param>
		/// <returns>The populated <see cref="MapResultSet"/>.</returns>
		public MapResultSet[] ExecuteResultSet(
			Type masterType, params MapNextResult[] nextResults)
		{
			return ExecuteResultSet(_mappingSchema.ConvertToResultSet(masterType, nextResults));
		}

		/// <summary>
		/// Executes the query, and returns multiple results.
		/// </summary>
		/// <typeparam name="T">The type of the master business object.</typeparam>
		/// <param name="nextResults">Array of <see cref="MapNextResult"/> to populate.</param>
		/// <returns>The populated <see cref="MapResultSet"/>.</returns>
		public MapResultSet[] ExecuteResultSet<T>(params MapNextResult[] nextResults)
		{
			return ExecuteResultSet(_mappingSchema.ConvertToResultSet(typeof(T), nextResults));
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
					"dataSet", Resources.DbManager_CannotUpdateNullDataset);

			DbDataAdapter da = CreateDataAdapter();

			return ExecuteOperation<int>(OperationType.Update, delegate
			{
				return (table.ByName) ?
					da.Update(dataSet, table.Name) :
					da.Update(dataSet.Tables[table.Index]);
			});
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
					"dataTable", Resources.DbManager_CannotUpdateNullDataTable);

			return ExecuteOperation<int>(OperationType.Update, delegate
			{
				return CreateDataAdapter().Update(dataTable);
			});
		}

		#endregion

		#region ExecuteOperation

#if !FW3
		/// <summary>Encapsulates a method that takes no parameters and does not return a value.</summary>
		private delegate void Action();
#endif

		private void ExecuteOperation(OperationType operationType, Action operation)
		{
			try
			{
				OnBeforeOperation(operationType);
				operation();
				OnAfterOperation (operationType);
			}
			catch (Exception ex)
			{
				HandleOperationException(operationType, ex);
				throw;
			}
		}

#if !FW3
		/// <summary>Encapsulates a method that has no parameters and returns a value of the type specified by the <paramref name="TResult" /> parameter.</summary>
		/// <returns>The return value of the method that this delegate encapsulates.</returns>
		private delegate TResult Func<TResult>();
#endif

		private T ExecuteOperation<T>(OperationType operationType, Func<T> operation)
		{
			T res = default(T);

			try
			{
				OnBeforeOperation(operationType);
				res = operation();
				OnAfterOperation (operationType);
			}
			catch (Exception ex)
			{
				if (res is IDisposable)
					((IDisposable)res).Dispose();

				HandleOperationException(operationType, ex);
				throw;
			}

			return res;
		}

		private void HandleOperationException(OperationType op, Exception ex)
		{
			Debug.WriteLineIf(TS.TraceError, string.Format(
				"Operation '{0}' throws exception '{1}'", op, ex), TS.DisplayName);

			OnOperationException(op, new DataException(this, ex));
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

			base.Dispose(disposing);
		}

		#endregion
	}
}
