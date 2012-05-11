using System;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace UnitTests.CS.JointureTests
{
    [TableName(Name = "ARTIST", Owner = Consts.Owner)]
    public class Artist
    {
        [MapField("ID_ARTIST"), PrimaryKey, KeyGenerator(PrimaryKeyGeneratorType.Sequence, true), Sequence("SEQ_ARTIST")]
        public long Id { get; set; }

        [MapField("ARTIST")]
        public string Name { get; set; }
    }


    [TableName(Name = "LABEL", Owner = Consts.Owner)]
    public class Label
    {
        [MapField("ID_LABEL"), PrimaryKey, KeyGenerator(PrimaryKeyGeneratorType.Sequence, true), Sequence("SEQ_LABEL")]
        public long Id { get; set; }

        [MapField("LABEL")]
        public string Name { get; set; }

        public int ID_USER_ { get; set; }
        public DateTime DATE_CREATION { get; set; }
        public DateTime DATE_MODIFICATION { get; set; }
        public int ACTIVATION { get; set; }
    }
}