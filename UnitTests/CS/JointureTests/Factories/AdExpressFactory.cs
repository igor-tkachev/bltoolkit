using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

namespace UnitTests.CS.JointureTests
{
    public class AdExpressFactory : DbConnectionFactory
    {
        public AdExpressFactory()
        {
            Provider = new  GenericDataProvider(ProviderFullName.Oracle);

            string database = "ADEXPRESS_WEB_TEST.PIGE";
            const string username = "dmussuma";
            const string password = "sandie5";

            ConnectionString = string.Format(
                "data source={0};User Id={1};Password={2};Pooling=True;Connection Timeout=120;Max Pool Size=150;",
                database, username, password);
        }
    }

    public class AdExpressOleronFactory : DbConnectionFactory
    {
        public AdExpressOleronFactory()
        {
            Provider = new  GenericDataProvider(ProviderFullName.Oracle);

            string database = "ADEXPRESS_WEB_TEST.PIGE";
            const string username = "dmussuma";
            const string password = "sandie5";

            const string provider = "Oracle.DataAccess.Client";
            const string oraDbUserName = "bmasson";
            const string oraDbPassword = "sandie5";
            const string oraDbDataSource = "(DESCRIPTION =(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST = OLERON)(PORT = 1521)))(CONNECT_DATA =(SID = PITAFR01)))";
            const string connexionString = @"User Id={0}; Password={1}; Data Source={2}; Pooling=True;Connection Timeout=120;Max Pool Size=150;Decr Pool Size=20;";

            ConnectionString = string.Format(connexionString, oraDbUserName, oraDbPassword, oraDbDataSource);
        }
    }
}