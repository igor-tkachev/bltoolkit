using System.Data.Common;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

namespace UnitTests.CS.JointureTests.Factories
{
    public class MediaDiscFactory : DbConnectionFactory
    {
        public MediaDiscFactory()
        {
            var aa = DbProviderFactories.GetFactoryClasses();
            var bb = aa.Rows.Count;

            string username = "mediadisc01_proc_1";
            string password = "proki35";
            string database = "mediadiscfr01.test";

            Provider = new OdpDataProvider();

            ConnectionString = string.Format(
                "data source={0};User Id={1};Password={2};Pooling=True;Connection Timeout=120;Max Pool Size=150;",
                database, username, password);
        }
    }
}