using System.Collections.Generic;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

namespace UnitTests.CS.JointureTests.Mappings
{
    [TableName(Name = "ARTIST", Owner = Consts.Owner)]
    public class Artist2
    {
        [MapField("ID_ARTIST"), PrimaryKey, NonUpdatable]
        public long Id { get; set; }

        [MapField("ARTIST")]
        public virtual string Name { get; set; }

        [LazyInstance(true)]
        [Association(ThisKey = "Id", OtherKey = "ArtistID")]
        public virtual List<Title> Titles { get; set; }
    }

    [TableName(Name = "ARTIST", Owner = Consts.Owner)]
    public class Artist4
    {
        [PrimaryKey, NonUpdatable]
        public long ID_ARTIST { get; set; }
        public string ARTIST { get; set; }

        [LazyInstance(true)]
        [Association(ThisKey = "ID_ARTIST", OtherKey = "ID_ARTIST")]
        public virtual List<Title2> Titles { get; set; }
    }
}