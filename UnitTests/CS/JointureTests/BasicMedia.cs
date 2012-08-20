using BLToolkit.DataAccess;

namespace UnitTests.CS.JointureTests
{
    [TableName(Name = "BASIC_MEDIA", Owner = Consts.Owner)]
    public class BasicMedia
    {
        public long ID_BASIC_MEDIA { get; set; }
        public string BASIC_MEDIA { get; set; }
        public int ACTIVATION { get; set; }
        public int ID_CATEGORY { get; set; }
    }
}