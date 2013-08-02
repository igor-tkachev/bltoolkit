using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace UnitTests.CS.JointureTests.Mappings
{
    [TableName(Name = "ARTIST", Owner = Consts.MusicOwner)]
    public class Artist
    {
        [MapField("ID_ARTIST"), PrimaryKey, Identity, SequenceName(Consts.MusicOwner + ".SEQ_ARTIST")]
        public long Id { get; set; }

        [MapField("ARTIST")]
        public string Name { get; set; }
    }

    [TableName(Name = "ARTIST", Owner = Consts.MusicOwner)]
    public class ExtArtist : Artist
    {
        public decimal EXT_ID_ARTIST { get; set; }
    }

    [TableName(Name = "track", Owner = "SPRE01")]
    public class track
    {
        [PrimaryKey, Identity, SequenceName("SPRE01.SEQ_TRACK")]
        public long ID_TRACK { get; set; }

        public string TRACK { get; set; }
        public short DURATION { get; set; }
        public string ID_COUNTRY_CODE { get; set; }
        public long ID_ARTIST { get; set; }
        public long ID_USER_ { get; set; }
        public short VALIDITY_STATUS { get; set; }
        public string PAPERCLIP_FILENAME { get; set; }
        public string TRACK_LIKE { get; set; }
        //public DateTime? DATE_CONFIRMED { get; set; }
        public DateTime DATE_CREATION { get; set; }
        public DateTime DATE_MODIFICATION { get; set; }
        public string COMMENTARY { get; set; }
        public short ACTIVATION { get; set; }
        //public DateTime? DATE_NPF { get; set; }
        //public short? NEW_PROD_STATUS { get; set; }
        public long? ID_LABEL { get; set; }
        [Nullable]
        public DateTime? DATE_RELEASE { get; set; }
        public long? ID_GENRE { get; set; }
        public long? ID_TRACK_VERSION { get; set; }
        public string ISRC { get; set; }
    }

    [TableName(Name = "artist", Owner = "SPRE01")]
    public class artist
    {
        [PrimaryKey, Identity, SequenceName("SPRE01.SEQ_ARTIST")]
        public long ID_ARTIST { get; set; }

        public string ARTIST { get; set; }
        public long ID_USER_ { get; set; }
        public string ARTIST_LIKE { get; set; }
        public DateTime DATE_CONFIRMED { get; set; }
        public long EXT_ARTIST_ID { get; set; }
        public DateTime DATE_CREATION { get; set; }
        public DateTime DATE_MODIFICATION { get; set; }
        public string COMMENTARY { get; set; }
        public short ACTIVATION { get; set; }
    }

    [TableName(Name = "ARTIST", Owner = Consts.Owner)]
    public class SimpleArtist
    {
        [PrimaryKey, Identity, SequenceName("SEQ_ARTIST")]
        public long ID_ARTIST { get; set; }
        public string ARTIST { get; set; }
    }

    [TableName(Name = "ARTIST", Owner = Consts.Owner)]
    public class Artist3
    {
        public long ID_ARTIST;
        public string Artist;
    }

    [TableName(Name = "RECO_RADIO", Owner = Consts.Owner)]
    // ReSharper disable InconsistentNaming
    public class RECO_RADIO
    {
        [MapField("ID_RECO_RADIO"), PrimaryKey, Identity, SequenceName("SEQ_RECO_RADIO")]
        public long ID_RECO_RADIO { get; set; }

        [MapField("ID_MULTIMEDIA_VALIDATED")]
        public long IdMultVal { get; set; }
        public int INPUT_STATUS { get; set; }
        public long ID_MULTIMEDIA_FILE { get; set; }
        public long ID_MEDIA { get; set; }
        public DateTime DATE_MEDIA { get; set; }

        /// <summary>
        /// Offset sur le parallèle-antenne
        /// </summary>
        public DateTime TIME_MEDIA { get; set; }

        public float TAG_MATCH_BEGINNING { get; set; }
        public float TAG_MATCH_DURATION { get; set; }
        public float TAG_DURATION { get; set; }

        public DateTime DATE_LAST_IMPORT { get; set; }

        public DateTime DATE_CREATION { get; set; }
        public DateTime DATE_MODIFICATION { get; set; }
        public int ACTIVATION { get; set; }
        public string COMMENTARY { get; set; }
        public int ID_LANGUAGE_DATA_I { get; set; }

        /// <summary>
        /// Inutilisé pour l'instant
        /// </summary>
        public int RATE { get; set; }
    }



    [TableName(Name = "LABEL", Owner = Consts.MusicOwner)]
    public class Label
    {
        [MapField("ID_LABEL"), PrimaryKey, SequenceName(Consts.MusicOwner + ".SEQ_LABEL"), Identity]
        public long Id { get; set; }

        [MapField("LABEL")]
        public string Name { get; set; }

        public int ID_USER_ { get; set; }
        public DateTime DATE_CREATION { get; set; }
        public DateTime DATE_MODIFICATION { get; set; }
        public int ACTIVATION { get; set; }
    }

    [TableName(Name = "PRODUCT_PENDING", Owner = "PITAFR01")]
    public class DataProductPending : FredCopyright
    {
        [MapField("ID_PRODUCT_PENDING"), PrimaryKey, SequenceName("PITAFR01.SEQ_PRODUCT_PENDING"), Identity]
        public int IdProductPending { get; set; }

        //[MapField("ID_PRODUCT"), NonUpdatable(OnInsert = false, OnUpdate = true)]
        //public int? IdProduct { get; set; }

        [MapField("ID_USER_"), NonUpdatable(OnInsert = false, OnUpdate = true)]
        public int IdUser { get; set; }

        [MapField("ID_USER_CREATE"), NonUpdatable(OnInsert = false, OnUpdate = true)]
        public int IdUserCreate { get; set; }

        [MapField("PRODUCT_PENDING"), NonUpdatable(OnInsert = false, OnUpdate = true)]
        public string ProductPending { get; set; }

        [MapField("VISUAL_PATH"), NonUpdatable(OnInsert = false, OnUpdate = true)]
        public string VisualPath { get; set; }

        [MapField("USER_PROGRAM"), NonUpdatable(OnInsert = false, OnUpdate = true)]
        public string UserProgram { get; set; }
    }


    public class FredCopyright
    {
        [MapField("ID_LANGUAGE_DATA_I"), NonUpdatable(OnUpdate = true, OnInsert = false)]
        public int IdLanguage { get; set; }

        [MapField("DATE_CREATION"), NonUpdatable(OnUpdate = true, OnInsert = false)]
        public DateTime DateCreation { get; set; }

        [MapField("DATE_MODIFICATION")]
        public DateTime DateModification { get; set; }

        [MapField("COMMENTARY")]
        public String Commentary { get; set; }

        [MapField("ACTIVATION")]
        public int Activation { get; set; }


        public FredCopyright()
        {
            DateCreation = DateTime.Now;
            DateModification = DateTime.Now;
            IdLanguage = 33;
        }
    }


    public class Mapping
    {
        public int IdMapping { get; set; }

        public String DeclaredProduct { get; set; }

        public int DeclaredId { get; set; }

        public Product Product { get; set; }

        public ProductPending ProductPending { get; set; }

        public MappingState MappingState { get; set; }
    }

    public class Product
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; }

        public int IdAdvertiser { get; set; }
        public string AdvertiserName { get; set; }
    }

    public class ProductPending
    {
        public int IdProductPending { get; set; }
        public string ProductName { get; set; }
    }

    public enum MappingState { Valid = 0, Todo = 1, Declared = 2, Rejected = 10, PaperClip = 6, Dead = 50, PendingSetProduct = 10, PendingValid = 0 };

    [TableName(Name = "USER_", Owner = "PITAFRLOCK1")]
    public class PitagorUSER_
    {
        [MapField("ID_USER_"), PrimaryKey, NonUpdatable]
        public int UserId { get; set; }

        [MapField("USER_")]
        public string UserName { get; set; }

        public long ACTIVATION { set; get; }
    }

    [TableName(Name = "MAPPING_SEARCH", Owner = "PITAFR01")]
    public class DataMapping : FredCopyright
    {
        [MapField("ID_MAPPING_SEARCH"), PrimaryKey, SequenceName("SEQ_IMP_SEARCH"), Identity]
        public int IdMapping { get; set; }

        [MapField("DECLARED_PRODUCT"), NonUpdatable(OnInsert = false, OnUpdate = true)]
        public string DeclaredProduct { get; set; }

        [MapField("DECLARED_ID_PRODUCT"), NonUpdatable(OnInsert = false, OnUpdate = true)]
        public int DeclaredId { get; set; }

        [MapField("ID_PRODUCT")]
        public int? IdProduct { get; set; }

        [MapField("ID_PRODUCT_PENDING")]
        public int? IdProductPending { get; set; }

        [MapField("ID_USER_")]
        public int IdUser { get; set; }

        [MapField("ID_USER_LOCK")]
        public int? IdLocker { get; set; }

    }

    [TableName(Name = "IMP_SEARCH", Owner = "PITAFR01")]
    public class DataImport : FredCopyright
    {
        [MapField("ID_IMP_SEARCH"), PrimaryKey, Identity, SequenceName("PITAFR01.SEQ_IMP_SEARCH")]
        public Int64 IdImport { get; set; }

        [MapField("ID_MEDIA")]
        public int IdMedia { get; set; }

        [MapField("ID_DATA_SEARCH")]
        public int? IdData { get; set; }

        [MapField("DATE_MEDIA")]
        public DateTime DateMedia { get; set; }

        [MapField("DECLARED_PRODUCT")]
        public String DeclaredProduct { get; set; }

        [MapField("DECLARED_ID_PRODUCT")]
        public int DeclaredId { get; set; }

        [MapField("DECLARED_EXPENDITURE")]
        public double DeclaredExpenditure { get; set; }

        [MapField("DECLARED_URL")]
        public String DeclaredUrl { get; set; }

        [MapField("FILE_NAME")]
        public String FileName { get; set; }

        [MapField("FILE_PATH")]
        public String FilePath { get; set; }

    }

}