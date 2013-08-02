#region

using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

#endregion

namespace BeeMusic.WebServices.DataAccess.DbModel
{
    [TableName(Name = "GENRE", Owner = "BEEMUSIC01")]
    public class DataGenre
    {
        [MapField("ID_GENRE"), PrimaryKey, SequenceName("BEEMUSIC01.SEQ_GENRE"), Identity]
        public int IdGenre { get; set; }

        [MapField("GENRE")]
        public string Genre { get; set; }

        [MapField("ID_SUB_FAMILY")]
        public string IdSubFamily { get; set; }
    }
}