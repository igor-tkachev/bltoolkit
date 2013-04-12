using System;
using BLToolkit.Data;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using NUnit.Framework;

namespace UnitTests.CS.JointureTests.Factories
{
    [TestFixture]
    public class OleronTest : AllTests
    {
        #region Overrides of AssociationTests

        protected override DbConnectionFactory CreateFactory()
        {
            return new OleronFactory();
        }

        #endregion
    }

    [TableName(Owner = "MEDIADISC01")]
    public class ALBUM_FOLLOWED
    {
        public string BARCODE { get; set; }
        public string ID_DISTRIBUTOR { get; set; }
        public DateTime? DATE_REQUEST { get; set; }
        public DateTime? DATE_RECEPTION { get; set; }
        public DateTime? DATE_NUMERISATION { get; set; }
        public DateTime? DATE_RETURN { get; set; }
        public decimal? REASON { get; set; }
        public decimal? PACKAGE_NUMBER { get; set; }
        public decimal? NB_DISK { get; set; }
        public Decimal? NB_DISK_DONE { get; set; }
        public DateTime DATE_CREATION { get; set; }
        public DateTime DATE_MODIFICATION { get; set; }
        public String COMMENTARY { get; set; }
    }

    [TableName(Owner = "PITAFR01")]
    public class DATA_RADIO
    {
        [PrimaryKey, Identity, SequenceName("PITAFR01.SEQ_DATA_RADIO")]
        public long ID_DATA_RADIO { get; set; }

        //[PrimaryKey]
        [MapField("ID_COBRANDING_ADVERTISER")]
        public long IdCobAdvert { get; set; }

        public long ID_DATA_VERSION { get; set; }
        public long ID_MEDIA { get; set; }
        public DateTime DATE_MEDIA { get; set; }
        // SAISIE :
        public DateTime DATE_SPOT_BEGINNING { get; set; }
        public DateTime DATE_SPOT_END { get; set; }
        public int DURATION { get; set; }					// durée du spot
        public int DURATION_COBRANDING { get; set; }		// divisée par le nbre de participants
        public long ID_PRODUCT { get; set; }
        //ECRAN (recalcul)
        public string COMMERCIAL_BREAK { get; set; }
        public Int16? RANK { get; set; }
        public int? NUMBER_SPOT { get; set; }
        public int? DURATION_COMMERCIAL_BREAK { get; set; }
        // Without Auto Promo (ref 180000)
        public Int16? RANK_WAP { get; set; }
        public int? NUMBER_SPOT_WAP { get; set; }
        [MapField("DURATION_COMMERCIAL_BREAK_WAP")]
        public int? DurComBreakWap { get; set; }
        // Valorisation (recalcul)        
        public decimal? EXPENDITURE { get; set; }			// divisé par le nbre de participants
        public decimal? EXPENDITURE_EURO { get; set; }
        public long? ID_PRICING_DURATION_RADIO { get; set; }
        public long? ID_PRICING_RADIO { get; set; }
        // TODO 
        public string LINK { get; set; }
        //
        public DateTime DATE_CREATION { get; set; }
        public DateTime DATE_MODIFICATION { get; set; }	// cf revalo et export
        // STUFF
        public Int64 ID_LANGUAGE_DATA_I { get; set; }
        public Int64 ID_REGION_DATA { get; set; }
        public string COMMENTARY { get; set; }
        public Int16 ACTIVATION { get; set; }
    }

    public class DataEntryBroadcast : LightBroadcast
    {
        public long? VersionId { get; set; }
    }

    public class LightBroadcast
    {
        public long Id { get; set; } // correspond à ID_DATA_VERSION
        public long RecognitionId { get; set; } // correspond à ID_RECO_RADIO (passage à valider ou invalider)

        public DateTime SpotBegin { get; set; }
        public DateTime SpotEnd { get; set; }

        public long MediaId { get; set; }
        public DateTime DateMedia { get; set; }

        public BroadcastType BroadcastType { get; set; }

        public override string ToString()
        {
            string id;
            switch (BroadcastType)
            {
                case BroadcastType.DataEntry:
                    id = Convert.ToString(Id);
                    break;
                default:
                    id = Convert.ToString(RecognitionId);
                    break;
            }

            return string.Format("Id: {0} Type: {1} Begin: {2} End: {3}", id, BroadcastType, SpotBegin, SpotEnd);
        }
    }

    [Flags]
    public enum BroadcastType
    {
        Recognition = 1,
        DataEntry = 2,

        Block = 4,
        Nat = 8,
        Box = 16,
        WithoutPub = 32,
        OutOfScreen = 64,
        WrongVersion = 128,
        Isolated = 256,

        DataEntryNat = DataEntry | Nat,
        DataEntryBox = DataEntry | Box,
        DataEntryWithoutPub = DataEntry | WithoutPub,

        IsolatedAutoPromotions = Recognition | OutOfScreen | Isolated,
        RecognitionBlock = Recognition | Block,
        //RecognitionOutOfScreen = Recognition | OutOfScreen,
        RecognitionWrongVersion = Recognition | WrongVersion,
    }

    [TableName(Owner = "PITAFR01")]
    public class DATA_VERSION
    {
        [PrimaryKey]
        public long ID_DATA_VERSION { get; set; }

        public long ID_DATA_MEDIA { get; set; }
        public long ID_MEDIA { get; set; }
        public DateTime DATE_MEDIA { get; set; }
        // saisie
        public Int16 NUMBER_COBRANDING_ADVERTISER { get; set; }
        public long? ID_MULTIMEDIA { get; set; }
        public Int16? ID_TYPE { get; set; }
        public long ID_CURRENCY { get; set; }
        public Int32 INSERTION { get; set; }
        public Int16 PRICING_MANUAL { get; set; }

        // revalo
        public long? ID_PRICING { get; set; }
        public decimal REFERENCE1_EXPENDITURE { get; set; }
        public decimal REFERENCE2_EXPENDITURE { get; set; }

        public DateTime DATE_VALORISATION { get; set; }
        public DateTime DATE_CREATION { get; set; }
        public DateTime DATE_MODIFICATION { get; set; }

        public long ID_LANGUAGE_DATA_I { get; set; }
        public long ID_REGION_DATA { get; set; }
        public long ID_USER_ { get; set; }
        public long ID_CUMUL_PERIODICITY { get; set; }
        public string COMMENTARY { get; set; }
        public Int16 ACTIVATION { get; set; }

        public long? ID_DATA_SOURCE { get; set; }
        public long ID_SESSION_DETAIL { get; set; }
        public long? ID_MEDIA_PRICING { get; set; }
    }

    [TableName(Owner = "PITAFR01", Name = "DATA_VERSION")]
    public class DATA_VERSION_DATA_RADIO : DATA_VERSION
    {
        [Association(ThisKey = "ID_DATA_VERSION", OtherKey = "ID_DATA_VERSION")]
        public DATA_RADIO DataRadio { get; set; }
    }

    [TableName(Name = "MULTIMEDIA", Owner = "PITAFR01")]
    public class MULTIMEDIA_DB
    {
        [PrimaryKey]
        public long ID_MULTIMEDIA { get; set; }

        public string MULTIMEDIA { get; set; }
        public int DURATION { get; set; }
        public long ID_CATEGORY_MULTIMEDIA { get; set; }

        public long ID_USER_ { get; set; }
        public DateTime DATE_CREATION { get; set; }
        public DateTime DATE_MODIFICATION { get; set; }

        public long ID_DURATION { get; set; }
        public long ID_PRODUCT { get; set; }		// inutilisé apres la migration 2011/2012 - remplacé par MultimediaCobranding.ID_PRODUCT

        public Int16 STATUS_QUALI { get; set; }

        // Stuff
        public Int64 ID_MULTIMEDIA_CHARACTERISTIC { get; set; }
        public Int64 ID_REGION_DATA { get; set; }
        public Int64 ID_LANGUAGE_DATA_I { get; set; }
        public Int64 ID_VEHICLE_I { get; set; }
        public Int16 ACTIVATION { get; set; }
        public string COMMENTARY { get; set; }
    }

    [TableName(Name = "MULTIMEDIA", Owner = "PITAFR01")]
    public class MULTIMEDIA_DATA_VERSION : MULTIMEDIA_DB
    {
        [LazyInstance(true)]
        [Association(ThisKey = "ID_MULTIMEDIA", OtherKey = "ID_MULTIMEDIA")]
        public virtual DATA_VERSION_DATA_RADIO DataVersion { get; set; }

        //[Association(ThisKey = "ID_MULTIMEDIA", OtherKey = "ID_MULTIMEDIA")]
        //public List<MULTIMEDIA_FILE> MultimediaFiles { get; set; }

        //[Association]
        //public List<MULTIMEDIA_COBRANDING> MultimediaCobrandings { get; set; }

        //[Association]
        //public List<MULTIMEDIA_DATA_ITEM> MultimediaDataItems { get; set; }
    }

    [TableName(Name = "MULTIMEDIA_FILE", Owner = "PITAFR01")]
    public class MULTIMEDIA_FILE
    {
        // NB : sert de lien	 avec les données de reco :
        [MapField("ID_MULTIMEDIA_FILE"), PrimaryKey]
        public long ID_MULTIMEDIA_FILE { get; set; }
        public long ID_CATEGORY_MULTIMEDIA { get; set; }
        public long ID_MULTIMEDIA { get; set; }

        public string FILE_NAME { get; set; }
        public string FILE_PATH { get; set; }
        public Int64 DURATION { get; set; }
        public Int64 ID_FILE_NUMBER { get; set; }

        public Int64 ID_USER_ { get; set; }

        public long ID_TYPE_MULTIMEDIA { get; set; }
        public long ID_TYPE_FILE { get; set; }
        //public long ID_MEDIA_I { get; set; }				        pas de sens

        // Stuff		
        public long ID_REGION_DATA { get; set; }
        public long ID_LANGUAGE_DATA_I { get; set; }
        public DateTime DATE_CREATION { get; set; }
        public string COMMENTARY { get; set; }
        public Int16 ACTIVATION { get; set; }

        public const int CST_ID_TYPE_MULTIMEDIA_AUDIO = 3;
        //public const int CST_ID_TYPE_FILE_RADIO = 3; 

    }

    [TableName(Name = "MULTIMEDIA_COBRANDING", Owner = "PITAFR01")]
    public class MULTIMEDIA_COBRANDING
    {
        [MapField("ID_MULTIMEDIA"), PrimaryKey]
        public long ID_MULTIMEDIA { get; set; }

        [MapField("ID_COBRANDING_ADVERTISER"), PrimaryKey]
        public Int64 ID_COBRANDING_ADVERTISER { get; set; }


        public long ID_CATEGORY_MULTIMEDIA { get; set; }
        public long ID_PRODUCT { get; set; }

        public string NEW_PRODUCT { get; set; }		// cf trombones
        // Stuff
        public DateTime DATE_CREATION { get; set; }
        public string COMMENTARY { get; set; }
        public Int16 ACTIVATION { get; set; }
    }

    [TableName(Name = "DATA_ITEM_MULTIMEDIA", Owner = "PITAFR01")]
    public class MULTIMEDIA_DATA_ITEM
    {
        [MapField("ID_DATA_ITEM_MULTIMEDIA"), PrimaryKey]
        public long ID_DATA_ITEM_MULTIMEDIA { get; set; }

        public long ID_CATEGORY_MULTIMEDIA { get; set; }

        public long ID_MULTIMEDIA { get; set; }
        public long ID_ADDITIONAL_DATA_ITEM { get; set; }
        public long ID_VALUE { get; set; }

        // Stuff
        public string TABLE_LIST { get; set; }
        public string IDENT_TABLE_LIST { get; set; }
        public long ID_REGION_DATA { get; set; }
        public long ID_LANGUAGE_DATA_I { get; set; }
        public DateTime DATE_CREATION { get; set; }
        public string COMMENTARY { get; set; }
        public Int16 ACTIVATION { get; set; }
    }
}