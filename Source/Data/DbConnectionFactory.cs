using BLToolkit.Data.DataProvider;

namespace BLToolkit.Data
{
    public abstract class DbConnectionFactory : IDbConnectionFactory
    {
        public DataProviderBase Provider;
        public string ConnectionString { get; set; }

        #region IDbConnectionFactory Members

        public DbManager CreateDbManager()
        {
            return CreateDbManager(Provider, ConnectionString);
        }

        public virtual DbManager CreateDbManager(DataProviderBase provider, string connectionString)
        {
            return new DbManager(provider, connectionString);
        }

        #endregion
    }
}