using System;
using System.Collections.Generic;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using NUnit.Framework;

namespace UnitTests.CS.JointureTests
{
    [TestFixture]
    public class OleronTest : AssociationTests
    {
        #region Overrides of AssociationTests

        public override DbConnectionFactory CreateFactory()
        {
            return new OleronFactory();
        }

        #endregion
    }

    [TableName(Owner = "PITAFR01")]
    public class DATA_RADIO
    {
        [PrimaryKey]
        public long ID_DATA_RADIO { get; set; }

        [PrimaryKey]
        public long ID_COBRANDING_ADVERTISER { get; set; }

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
        public int? RANK { get; set; }
        public int? NUMBER_SPOT { get; set; }
        public int? DURATION_COMMERCIAL_BREAK { get; set; }
        // Without Auto Promo (ref 180000)
        public int? RANK_WAP { get; set; }
        public int? NUMBER_SPOT_WAP { get; set; }
        public int? DURATION_COMMERCIAL_BREAK_WAP { get; set; }
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

    [TableName(Owner = "PITAFR01")]
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
        [Association(ThisKey = "ID_MULTIMEDIA", OtherKey = "ID_MULTIMEDIA")]
        public DATA_VERSION_DATA_RADIO DataVersion { get; set; }

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