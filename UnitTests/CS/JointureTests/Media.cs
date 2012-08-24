using BLToolkit.DataAccess;

namespace UnitTests.CS.JointureTests
{
    [TableName(Name = "MEDIA", Owner = Consts.Owner)]
    public class Media
    {
        public long ID_MEDIA { get; set; }
        public long ID_BASIC_MEDIA { get; set; }
        public string MEDIA { get; set; }
        public int ACTIVATION { get; set; }

        public string CaptureCode { get; set; }

        public bool IsActivate { get; set; }
    }
}