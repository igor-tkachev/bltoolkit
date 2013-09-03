#region

using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

#endregion

namespace BeeMusic.WebServices.DataAccess.DbModel
{
    [TableName(Name = "PRODUCT", Owner = "BEEMUSIC01")]
    internal class DataProduct2 : Copyright
    {
        [MapField("ID_PRODUCT"), PrimaryKey, SequenceName("BEEMUSIC01" + ".SEQ_PRODUCT"), Identity]
        public int IdProduct { get; set; }

        [MapField("ID_DISTRIBUTOR")]
        public string IdDistributor { get; set; }

        [MapField("ID_CATALOG")]
        [MapValue(Catalogue.Physique, 1)]
        [MapValue(Catalogue.Digital, 2)]
        public Catalogue CatalogId { get; set; }

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

        [MapField("ALBUM_SHORT")]
        public string ShortTitle { get; set; }

        [MapField("ALBUM_RECEIPT")]
        public string ReceipTitle { get; set; }

        [MapField("SUBTITLE")]
        public string SubTitle { get; set; }

        [MapField("VERSION")]
        public string Version { get; set; }

        [MapField("DISPLAY_ARTIST")]
        public string ArtistDisplay { get; set; }

        [MapField("ID_DISPLAY_ARTIST")]
        public int? IdArtistDisplay { get; set; }

        [MapField("BRAND")]
        public string Brand { get; set; }

        [MapField("CONTAINER_TYPE")]
        public string ContainerType { get; set; }

        [MapField("ARGUMENT")]
        public string Argument { get; set; }

        [MapField("DATE_SELL")]
        public DateTime Sell { get; set; }

        [MapField("DATE_WITHDRAW")]
        public DateTime? Withdraw { get; set; }

        [MapField("AVAILABLE")]
        [MapValue(Available.Undefined, 0)]
        [MapValue(Available.Commandable, 1)]
        [MapValue(Available.PasEncoreSorti, 2)]
        [MapValue(Available.NonCommandableTemporairement, 3)]
        [MapValue(Available.PlusAuCatalogue, 7)]
        public Available Available { get; set; }

        [MapField("DURATION")]
        public string Duration { get; set; }

        [MapField("NB_DISK")]
        public int NbDisk { get; set; }

        [MapField("NB_TRACK")]
        public int? NbTrack { get; set; }

        [MapField("TRACKBUNDLECOUNT")]
        public int? NbBundle { get; set; }

        [MapField("NB_BONUS_TRACK")]
        public int? NbBonusTrack { get; set; }

        [MapField("YEAR")]
        public int? Year { get; set; }

        [MapField("P_LINE")]
        public string Pline { get; set; }

        [MapField("C_LINE")]
        public string Cline { get; set; }

        [MapField("ID_GENRE_MAPPING")]
        public int IdGenreMapping { get; set; }

        [MapField("ID_MEDIA_MAPPING")]
        public int IdMediaMapping { get; set; }

        [MapField("ID_EDITOR")]
        public int? IdEditor { get; set; }

        [MapField("ID_LABEL")]
        public int IdLabel { get; set; }

        [MapField("ID_COUNTRY")]
        public int IdCountry { get; set; }

        [MapField("ID_PRICE")]
        public string IdPrice { get; set; }

        [MapField("PRICE")]
        public decimal? PriceValue { get; set; }

        [MapField("IMPORT")]
        [MapValue(true, 1)]
        [MapValue(false, 0)]
        public bool? IsImport { get; set; }

        //[MapField("IMPORT"), DbType(System.Data.DbType.Int16)]
        //[MapValue(true, 1)]
        //[MapValue(false, 0)]
        //public bool? IsImport { get; set; }

        [MapField("LIMITED_EDITION"), DbType(System.Data.DbType.Int16)]
        [MapValue(true, 1)]
        [MapValue(false, 0)]
        public bool? IsLimitedEdition { get; set; }

        [MapField("COMPILATION"), DbType(System.Data.DbType.Int16)]
        [MapValue(true, 1)]
        [MapValue(false, 0)]
        public bool? IsCompilation { get; set; }

        [MapField("EXCLUSIVE_"), DbType(System.Data.DbType.Int16)]
        [MapValue(true, 1)]
        [MapValue(false, 0)]
        public bool IsExclusive { get; set; }

        [MapField("CRC")]
        public uint? CRC { get; set; }

        [MapField("CRC_PRICE")]
        public uint? CRCPrice { get; set; }

        [MapField("DATE_MODIFICATION_PRICE")]
        public DateTime DateModificationPrice { get; set; }

        [MapField("DATE_MODIFICATION_PRICE_VALUE")]
        public DateTime DateModificationPriceValue { get; set; }

        [MapField("CRC_SELL")]
        public uint? CRCSell { get; set; }

        [MapField("DATE_MODIFICATION_SELL")]
        public DateTime DateModificationSell { get; set; }

        [MapField("CRC_EXCLUSIVE")]
        public uint? CRCExclusive { get; set; }

        [MapField("DATE_MODIFICATION_EXCLUSIVE")]
        public DateTime DateModificationExclusive { get; set; }

        [MapField("CRC_GFK")]
        public uint? CRCGfk { get; set; }

        public DataProduct2()
        {
            Sell =  new DateTime(1900, 1, 1);
            NbDisk = 1;
            IdCountry = 0;

            DateModificationPrice = DateTime.Now;
            DateModificationPriceValue = DateModificationPrice;
            DateModificationSell = DateModificationPrice;
            DateModificationExclusive = DateModificationPrice;
        }

    }

    public enum Source
    {
        Physique = 1,
        Digital = 2,
        Rip = 3,
        Regroupement = 4
    }

    public enum Catalogue
    {
        Physique = 1,
        Digital = 2
    }

    public enum CoverType
    {
        Recto = 0,
        Verso = 1
    }

    public enum Univers
    {
        Audio = 1,
        Video = 2,
        Undefined = 0
    }

    /// <summary>
    /// 
    /// </summary>
    public enum Available
    {
        /// <summary>
        /// Pas défini
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Disponible
        /// </summary>
        Commandable = 1,
        /// <summary>
        /// A paraître 
        /// </summary>
        PasEncoreSorti = 2,
        /// <summary>
        /// En rupture de stock 
        /// </summary>
        NonCommandableTemporairement = 3,
        /// <summary>
        /// Déréférencé
        /// </summary>
        PlusAuCatalogue = 7
    }


      internal class Copyright
    {
        [MapField("DATE_CREATION"), NonUpdatable(OnUpdate = true, OnInsert = false)]
        public DateTime DateCreation { get; set; }

        [MapField("DATE_MODIFICATION")]
        public DateTime DateModification { get; set; }

        [MapField("COMMENTARY")]
        public String Commentary { get; set; }

        public Copyright()
        {
            DateCreation = DateTime.Now;
            DateModification = DateTime.Now;
        }

    }

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