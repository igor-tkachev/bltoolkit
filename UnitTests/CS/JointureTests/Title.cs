using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace UnitTests.CS.JointureTests
{
    [TableName(Name = "TRACK", Owner = Consts.Owner)]
    public class Title
    {
        [MapField("ID_TRACK"), PrimaryKey, NonUpdatable]
        public long Id { get; set; }

        [MapField("TRACK")]
        public string Name { get; set; }

        [MapField("ID_ARTIST")]
        public long? ArtistID { get; set; }

        [Association(ThisKey = "ArtistID", OtherKey = "Id", CanBeNull = false)]
        public Artist Artist { get; set; }
    }

    [TableName(Name = "MEDIA", Owner = Consts.Owner)]
    public class Media
    {
        public long ID_MEDIA { get; set; }
        public long ID_BASIC_MEDIA { get; set; }
        public string MEDIA { get; set; }
        public int ACTIVATION { get; set; }
    }

    [TableName(Name = "BASIC_MEDIA", Owner = Consts.Owner)]
    public class BasicMedia
    {
        public long ID_BASIC_MEDIA { get; set; }
        public string BASIC_MEDIA { get; set; }
        public int ACTIVATION { get; set; }
        public int ID_CATEGORY { get; set; }
    }
}