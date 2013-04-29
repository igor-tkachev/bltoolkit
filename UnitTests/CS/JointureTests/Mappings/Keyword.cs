using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace UnitTests.CS.JointureTests.Mappings
{
    [Serializable]
    [TableName("Visual_keyword", Owner = "ORQUAFR01")]
    public class Keyword
    {
        [MapField("ID_VISUAL_KEYWORD"), PrimaryKey, SequenceName("ORQUAFR01.SEQ_VISUAL_KEYWORD"), Identity]
        public int ID { get; set; }
        [MapField("VISUAL_KEYWORD")]
        public string NAME { get; set; }

        public DateTime DATE_CREATION { get; set; }
    }
}