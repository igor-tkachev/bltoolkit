using System;
using System.Data;
using System.Data.Common;
using BLToolkit.Data.Sql.SqlProvider;

namespace BLToolkit.Data.DataProvider
{
    /// <summary>
    /// Creates an instance of a db provider for a specified provider name.
    /// </summary>
    public sealed class GenericDataProvider : DataProviderBase
	{
        private readonly string _providerName;
        private readonly DbProviderFactory _factory;

        public GenericDataProvider(string providerName)
        {
            _providerName = providerName;
            _factory = DbProviderFactories.GetFactory(providerName);
        }

        #region Overrides of DataProviderBase

        public override Type ConnectionType
        {
            get
            {
                var dbConnection = _factory.CreateConnection();
                if (dbConnection != null) 
                    return dbConnection.GetType();
                return null;
            }
        }

        public override string Name
        {
            get { return _providerName; }
        }

        public override IDbConnection CreateConnectionObject()
        {
            return _factory.CreateConnection();
        }

        public override DbDataAdapter CreateDataAdapterObject()
        {
            return _factory.CreateDataAdapter();
        }

        /// <summary>
        /// Populates the specified IDbCommand object's Parameters collection with 
        /// parameter information for the stored procedure specified in the IDbCommand.
        /// </summary>
        public override bool DeriveParameters(IDbCommand command)
        {
            return false;
        }

        public override ISqlProvider CreateSqlProvider()
        {
            switch (Name)
            {
                case ProviderFullName.OracleNet:
                    return new OracleSqlProvider();
                case ProviderFullName.SQLite:
                    return new SQLiteSqlProvider();
                case ProviderFullName.Oracle:
                    return new OracleSqlProvider();
                default:
                    throw new Exception(string.Format("The sql provider {0} isnt supported in the GenericDataProvider", Name));
            }
        }

        public override IDataReader GetDataReader(IDbCommand command, CommandBehavior commandBehavior)
        {
            if (Name == ProviderFullName.Oracle)
            {
                command.CommandText = OracleHelper.Interpret(command);
                command.Parameters.Clear();
            }
    
            return base.GetDataReader(command, commandBehavior);
        }

        #endregion
	}
}
