#region

using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

#endregion

namespace BeeMusic.WebServices.DataAccess.DbModel
{
    [TableName(Name = "PRODUCT", Owner = "BEEMUSIC01")]
    internal class DataProduct
    {
        [MapField("ID_PRODUCT"), PrimaryKey, SequenceName("BEEMUSIC01.SEQ_PRODUCT"), Identity]
        public int IdProduct { get; set; }

        [MapField("ID_DISTRIBUTOR")]
        public string IdDistributor { get; set; }

        [MapField("REF_PROVIDER")]
        public string RefProvider { get; set; }

        [MapField("BARCODE")]
        public string Barcode { get; set; }

        [MapField("GRID")]
        public string Grid { get; set; }

        [MapField("CATALOG_NUMBER")]
        public string CatalogNumber { get; set; }

        [MapField("TITLE")]
        public string Title { get; set; }

        [MapField("SUBTITLE")]
        public string SubTitle { get; set; }

        [MapField("VERSION")]
        public string Version { get; set; }

        [MapField("DISPLAY_ARTIST")]
        public string ArtistDisplay { get; set; }

        [MapField("ARGUMENT")]
        public string Argument { get; set; }

        [MapField("DATE_SELL")]
        public DateTime Sell { get; set; }

        [MapField("DATE_WITHDRAW")]
        public DateTime? Withdraw { get; set; }

        [MapField("DURATION")]
        public string Duration { get; set; }

        [MapField("NB_DISK")]
        public int NbDisk { get; set; }

        [MapField("NB_TRACK")]
        public int? NbTrack { get; set; }

        [MapField("P_LINE")]
        public string Pline { get; set; }

        [MapField("C_LINE")]
        public string Cline { get; set; }

        [MapField("YEAR")]
        public int? Year { get; set; }

        [MapField("ID_GENRE_MAPPING")]
        public int IdGenreMapping { get; set; }

        [MapField("ID_MEDIA_MAPPING")]
        public int IdMediaMapping { get; set; }

        [MapField("ID_EDITOR")]
        public int? IdEditor { get; set; }

        [MapField("ID_LABEL")]
        public int IdLabel { get; set; }
    }
}