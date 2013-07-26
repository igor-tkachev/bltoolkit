using BLToolkit.DataAccess;

namespace UnitTests.CS.JointureTests.Mappings
{
    [TableName(Name = "all_media")]
    public class all_media
    {
        public long ID_MEDIA { get; set; }
        public string MEDIA { get; set; }
        public long ID_BASIC_MEDIA { get; set; }
        public string BASIC_MEDIA { get; set; }
        public long ID_CATEGORY { get; set; }
        public string CATEGORY { get; set; }
        public long ID_VEHICLE { get; set; }
        public string VEHICLE { get; set; }
        public long ID_COUNTRY { get; set; }
        public string COUNTRY { get; set; }
    }
}