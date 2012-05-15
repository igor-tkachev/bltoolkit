using System.Collections.Generic;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

namespace UnitTests.CS.JointureTests
{
    [TableName(Name = "ARTIST", Owner = Consts.Owner)]
    public class Artist2
    {
        [MapField("ID_ARTIST"), PrimaryKey, NonUpdatable]
        public long Id { get; set; }

        [MapField("ARTIST")]
        public string Name { get; set; }

        [LazyInstance(true)]
        [Association(ThisKey = "Id", OtherKey = "ArtistID", CanBeNull = false)]
        public virtual List<Title> Titles { get; set; }
    }
}