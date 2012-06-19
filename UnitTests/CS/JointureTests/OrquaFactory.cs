using System.Data.Common;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

namespace UnitTests.CS.JointureTests
{
    public class OrquaFactory : DbConnectionFactory
    {
        public OrquaFactory()
        {
            var aa = DbProviderFactories.GetFactoryClasses();
            var bb = aa.Rows.Count;

            string username = "FSIMON";
            string password = "MAN1AGER";
            string database = "ORQUAF01.PIGE";

            //database = "MUSICFR01.TEST";
            //username = "scurutchet";
            //password = "kisscool12";

            //database = "pitaoleronfr01.pige";
            //database = "(DESCRIPTION=(ADDRESS_LIST =(ADDRESS = (COMMUNITY = tcp.pige) (PROTOCOL = TCP) (HOST = OLERON) (PORT = 1521))) (CONNECT_DATA = (SID = PITAFR01)))";
            //username = "bmasson";
            //password = "sandie5";

            Provider = new OdpDataProvider();
            //Provider = new GenericDataProvider(ProviderFullName.Oracle);

            ConnectionString = string.Format(
                "data source={0};User Id={1};Password={2};Pooling=True;Connection Timeout=120;Max Pool Size=150;",
                database, username, password);
        }
    }
}