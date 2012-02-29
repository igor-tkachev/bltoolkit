using BLToolkit.Data.DataProvider;

namespace BLToolkit.Data
{
    public abstract class DbConnectionFactory : IDbConnectionFactory
    {
        internal DataProviderBase Provider;
        internal string ConnectionString { get; set; }

        #region IDbConnectionFactory Members

        public DbManager CreateDbManager()
        {
            return new DbManager(Provider, ConnectionString);
        }

        #endregion
    }
}