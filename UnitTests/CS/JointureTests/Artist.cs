using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace UnitTests.CS.JointureTests
{
    [TableName(Name = "ARTIST", Owner = Consts.Owner)]
    public class Artist
    {
        [MapField("ID_ARTIST"), PrimaryKey, NonUpdatable]
        public long Id { get; set; }

        [MapField("ARTIST")]
        public string Name { get; set; }
    }
}