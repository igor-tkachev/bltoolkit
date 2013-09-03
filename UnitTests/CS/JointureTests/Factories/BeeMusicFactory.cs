using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

namespace UnitTests.CS.JointureTests.Factories
{
    public class BeeMusicFactory : DbConnectionFactory
    {
        public BeeMusicFactory()
        {
            Provider = new GenericDataProvider(ProviderFullName.Oracle);
            //Provider = new OdpDataProvider();

            const string oraDbUserName = "BEEMUSIC01_PROC_1";
            const string oraDbPassword = "zb88zb";
            const string oraDbDataSource = "(DESCRIPTION =(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST = frmitch-or004)(PORT = 1521)))(CONNECT_DATA =(SID = MEDIADISC01)))";
            const string connexionString = @"User Id={0}; Password={1}; Data Source={2}; Pooling=True;Connection Timeout=120;Max Pool Size=150;Decr Pool Size=20;";

            ConnectionString = string.Format(connexionString, oraDbUserName, oraDbPassword, oraDbDataSource);
        }
    }
}