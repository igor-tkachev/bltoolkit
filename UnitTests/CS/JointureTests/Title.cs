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

        [Association(CanBeNull = false, ThisKey = "ArtistID", OtherKey = "Id")]
        public Artist Artist { get; set; }
    }
}