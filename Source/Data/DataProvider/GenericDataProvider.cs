using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Transactions;
using BLToolkit.Data.DataProvider.Interpreters;
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
        private readonly DataProviderInterpreterBase _dataProviderInterpreter;

        public GenericDataProvider(string providerName)
        {
            _providerName = providerName;
            using (new TransactionScope())
            {
                _factory = DbProviderFactories.GetFactory(providerName);
            }
            
            switch (Name)
            {
                case ProviderFullName.Oracle:
                case ProviderFullName.OracleNet:
                    _dataProviderInterpreter = new OracleDataProviderInterpreter();
                    break;
                case ProviderFullName.SQLite:
                    _dataProviderInterpreter = new SqliteDataProviderInterpreter();
                    break;                    
                default:
                    throw new Exception(string.Format("The sql provider {0} isnt supported in the DataProviderInterpreterBase", Name));
            }
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

        public override void SetParameterValue(IDbDataParameter parameter, object value)
        {
            _dataProviderInterpreter.SetParameterValue(parameter, value);
        }

        public override string GetSequenceQuery(string sequenceName)
        {
            return _dataProviderInterpreter.GetSequenceQuery(sequenceName);
        }

        public override string NextSequenceQuery(string sequenceName)
        {
            return _dataProviderInterpreter.NextSequenceQuery(sequenceName);
        }

        public override string GetReturningInto(string columnName)
        {
            return _dataProviderInterpreter.GetReturningInto(columnName);
        }

        public override object Convert(object value, ConvertType convertType)
        {
            if (Name == ProviderFullName.Oracle)
            {
                switch (convertType)
                {
                    case ConvertType.NameToQueryParameter:
                        var qname = (string) value;

                        //
                        // Avoid "ORA-00972: identifier is too long" error
                        // Cause error : You tried to reference a table, cluster, view, index, synonym, tablespace, or username with a value that was longer than 30 characters.
                        // Resolution : Names for tables, clusters, views, indexes, synonyms, tablespaces, and usernames must be 30 characters or less. 
                        // You must shorten the name to no more than 30 characters for these objects.
                        //
                        if (qname.Length > 30)
                        {
                            qname = qname.Substring(0, 30);
                            return SqlProvider.Convert(qname, convertType);
                        }
                        return SqlProvider.Convert(value, convertType);
                }
            }

            return base.Convert(value, convertType);
        }

        public override int InsertBatchWithIdentity<T>(DbManager db, string insertText, IEnumerable<T> collection, Mapping.MemberMapper[] members, int maxBatchSize, DbManager.ParameterProvider<T> getParameters)
        {
            if (db.UseQueryText && Name == ProviderFullName.Oracle)
            {
                List<string> sqlList = _dataProviderInterpreter.GetInsertBatchSqlList(insertText, collection, members, maxBatchSize);
                return ExecuteSqlList(db, sqlList);
            }
            return base.InsertBatchWithIdentity(db, insertText, collection, members, maxBatchSize, getParameters);
        }

        public override int InsertBatch<T>(DbManager db, string insertText, IEnumerable<T> collection, Mapping.MemberMapper[] members, int maxBatchSize, DbManager.ParameterProvider<T> getParameters)
        {
            if (Name == ProviderFullName.Oracle)
            {
                if (db.UseQueryText)
                {
                    List<string> sqlList = _dataProviderInterpreter.GetInsertBatchSqlList(insertText, collection, members, maxBatchSize);
                    return ExecuteSqlList(db, sqlList);
                }
                throw new NotSupportedException("Set UseQueryText = true on the current generic data provider!");                
            }
            return base.InsertBatch(db, insertText, collection, members, maxBatchSize, getParameters);
        }

        #endregion
	}
}
