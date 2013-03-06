using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace UnitTests.CS.JointureTests.Mappings
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

        [Association(CanBeNull = false, ThisKey = "ArtistID", OtherKey = "Id")]
        public Artist Artist { get; set; }
    }


    [TableName(Name = "TRACK", Owner = Consts.Owner)]
    public class Title2
    {
        [PrimaryKey, NonUpdatable]
        public long ID_TRACK { get; set; }
        public string TRACK { get; set; }
        public long? ID_ARTIST { get; set; }

        [Association(CanBeNull = false, ThisKey = "ID_ARTIST", OtherKey = "ID_ARTIST")]
        public SimpleArtist Artist { get; set; }
    }
}