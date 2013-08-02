#region

using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

#endregion

namespace BeeMusic.WebServices.DataAccess.DbModel
{
    [TableName(Name = "GENRE_MAPPING", Owner = "BEEMUSIC01")]
    public class DataGenreMapping
    {
        [MapField("ID_GENRE_MAPPING"), PrimaryKey, SequenceName("BEEMUSIC01.SEQ_GENRE_MAPPING"), Identity]
        public int IdGenreMapping { get; set; }

        [MapField("GENRE_MAPPING")]
        public string GenreMapping { get; set; }

        [MapField("ID_DISTRIBUTOR")]
        public string IdDistributor { get; set; }

        [MapField("ID_GENRE")]
        public int? IdGenre { get; set; }
    }

     [TableName(Name = "GENRE_MAPPING", Owner = "BEEMUSIC01")]
    public class FullDataGenreMapping : DataGenreMapping
    {
         [Association(CanBeNull = true, ThisKey = "IdGenre", OtherKey = "IdGenre")]
         public DataGenre Genre { get; set; }
    }
}