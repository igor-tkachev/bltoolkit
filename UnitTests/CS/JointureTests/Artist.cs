using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace UnitTests.CS.JointureTests
{
    [TableName(Name = "ARTIST", Owner = Consts.Owner)]
    public class Artist
    {
        [MapField("ID_ARTIST"), PrimaryKey, Identity]
        public long Id { get; set; }

        [MapField("ARTIST")]
        public string Name { get; set; }
    }

    [TableName(Name = "RECO_RADIO", Owner = Consts.Owner)]
    // ReSharper disable InconsistentNaming
    public class RECO_RADIO
    {
        [MapField("ID_RECO_RADIO"), PrimaryKey, Identity, SequenceName("SEQ_RECO_RADIO")]
        public long ID_RECO_RADIO { get; set; }

        public long ID_MULTIMEDIA_VALIDATED { get; set; }
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



    [TableName(Name = "LABEL", Owner = Consts.Owner)]
    public class Label
    {
        //[MapField("ID_LABEL"), PrimaryKey, SequenceName("SEQ_LABEL"), KeyGenerator(PrimaryKeyGeneratorType.Sequence, false)]
        [MapField("ID_LABEL"), PrimaryKey, SequenceName("SEQ_LABEL"), Identity]
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

        [MapField("ID_PRODUCT"), NonUpdatable(OnInsert = false, OnUpdate = true)]
        public int? IdProduct { get; set; }

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
        [MapField("ID_IMP_SEARCH"), PrimaryKey, Identity, SequenceName("SEQ_IMP_SEARCH"), Identity]
        public int IdImport { get; set; }

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