using System.Data.Common;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

namespace UnitTests.CS.JointureTests
{
    public class MusicFactory : DbConnectionFactory
    {
        public MusicFactory(bool prod)
        {
            var aa = DbProviderFactories.GetFactoryClasses();
            var bb = aa.Rows.Count;

            string database = prod ? "MUSIC01.PIGE" : "MUSICFR01.TEST";
            const string username = "scurutchet";
            const string password = "kisscool12";

            //Provider = new OdpDataProvider();
            Provider = new GenericDataProvider(ProviderFullName.Oracle);

            ConnectionString = string.Format(
                "data source={0};User Id={1};Password={2};Pooling=True;Connection Timeout=120;Max Pool Size=150;",
                database, username, password);
        }
    }
}