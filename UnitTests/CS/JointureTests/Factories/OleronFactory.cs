using System.Data.Common;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

namespace UnitTests.CS.JointureTests.Factories
{
    public  class OleronFactory : DbConnectionFactory
    {
        public  OleronFactory()
        {
            var aa = DbProviderFactories.GetFactoryClasses();
            var bb = aa.Rows.Count;           

            string username = "FSIMON";
            string password = "MAN1AGER";
            string database = "pitaoleronfr01.pige";
            //database = "PITARADIOFR01.PIGE";

            //username = "pitafr01_proc_11";
            //password = "smsmms8";

            Provider = new OdpDataProvider();
            //Provider = new GenericDataProvider(ProviderFullName.Oracle);

            ConnectionString = string.Format(
                "data source={0};User Id={1};Password={2};Pooling=True;Connection Timeout=120;Max Pool Size=150;",
                database, username, password);
        }
    }
}