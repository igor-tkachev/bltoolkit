using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

namespace UnitTests.CS.JointureTests.Factories
{
    public class RadioPigeFactory : DbConnectionFactory
    {
        public RadioPigeFactory()
        {
            Provider = new OdpDataProvider();

            string database = "PITARADIOFR01.PIGE";
            const string username = "pitafr01_proc_11";
            const string password = "smsmms8";

            ConnectionString = string.Format(
                "data source={0};User Id={1};Password={2};Pooling=True;Connection Timeout=120;Max Pool Size=150;",
                database, username, password);
        }
    }
}