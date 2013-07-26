using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

namespace UnitTests.CS.JointureTests.Factories
{
    public class TvPigeFactory : DbConnectionFactory
    {
        public TvPigeFactory()
        {
            Provider = new  OdpDataProvider();
            //Provider = new GenericDataProvider(ProviderFullName.Oracle);

            string database = "PITATVFR01.PIGE";
            const string username = "pitafr01_proc_11";
            const string password = "smsmms8";

            ConnectionString = string.Format(
                "data source={0};User Id={1};Password={2};Pooling=True;Connection Timeout=120;Max Pool Size=150;",
                database, username, password);
        }
    }
}