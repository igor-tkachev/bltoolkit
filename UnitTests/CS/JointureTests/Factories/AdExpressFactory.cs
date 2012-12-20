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
}