using System;
using System.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Oracle.DataAccess.Types;

namespace UnitTests.CS.JointureTests
{
    [TableName(Name = "SCRIPT", Owner = "ORQUAFR01")]
    public class SCRIPT_TABLE
    {
        [PrimaryKey, NonUpdatable]
        public long ID_FORM { get; set; }

        public string SCRIPT { get; set; }

        public DateTime DATE_CREATION { get; set; }
    }

    [TableName(Name = "FORM", Owner = "ORQUAFR01")]
    public class FILE_FORM
    {
        [MapField("ID_FORM"), PrimaryKey, NonUpdatable]
        public long ID_FORM { get; set; }
        public long ID_STRIKE { get; set; }

        public DateTime DATE_CREATION { get; set; }

        public long ID_MEDIA { get; set; }
        public long ID_DOC_IMAGE { get; set; }

        public long ID_VEHICLE { get; set; }

        public long ID_OPERATOR_MODIF { get; set; }
        public long ID_OPERATOR_DATA { get; set; }

        public int ID_PRODUCT { get; set; }
        public long ID_CLASSIFICATION_INTER { get; set; }
        public long? ID_OPERATOR { get; set; }

        public long FORM_INTER { get; set; }
        public string BRAND { get; set; }

        public string SCRIPT_STATE { get; set; }
        public long ID_COUNTRY { get; set; }

        public DateTime DATE_ALERTE_FR { get; set; }
        public DateTime DATE_ALERTE_INTER { get; set; }

        public int ACTIVATION { get; set; }
        public string COMMENTARY { get; set; }
        public long ID_LANGUAGE_I { get; set; }
        
        public DateTime DATE_MODIFICATION { get; set; }

        public string CLIP { get; set; }
        public string AWARD { get; set; }

        // Dates en integer ..
        public int DATE_MODIFICATION_DATA { get; set; }
        public int DATE_MEDIA { get; set; }
        public int DATE_DATA { get; set; }

        [MapField("SCRIPT")]
        public OracleClob SCRIPT_FIELD { get; set; }

        //[Association(ThisKey = "ID_FORM", OtherKey = "ID_FORM")]
        //public FORM_SCRIPT FORM_SCRIPT { get; set; }
        //[Association(ThisKey = "ID_FORM", OtherKey = "ID_FORM")]
        //public FORM_DATA_RADIO FORM_DATA_RADIO { get; set; }
        //[Association(ThisKey = "ID_FORM", OtherKey = "ID_FORM")]
        //public TACCROCHE TACCROCHE { get; set; }
        //[Association(ThisKey = "ID_FORM", OtherKey = "ID_FORM")]
        //public FORM_SIGNATURE FORM_SIGNATURE { get; set; }

        //[LazyInstance(false)]
        //[Association(ThisKey = "ID_FORM", OtherKey = "ID_FOLLOW_UP")]
        //public List<FORM_FOLLOW_UP> FORM_FOLLOW_UPs { get; set; }
    }

    [TableName(Name = "SIGNATURE", Owner = "ORQUAFR01")]
    public class FORM_SIGNATURE
    {
        [MapField("ID_FORM"), PrimaryKey, NonUpdatable]
        public long ID_FORM { get; set; }

        [MapField("SIGNATURE")]
        [DbType(DbType.Binary)]
        public object SIGNATURE_FIELD { get; set; }

        public int ACTIVATION { get; set; }

        public DateTime DATE_CREATION { get; set; }
        public DateTime DATE_MODIFICATION { get; set; }
    }

    [TableName(Name = "DATA_RADIO", Owner = "ORQUAFR01")]
    public class FORM_DATA_RADIO
    {
        [MapField("ID_FORM"), PrimaryKey, NonUpdatable]
        public long ID_FORM { get; set; }

        public int DURATION { get; set; }
        public string STATION { get; set; }
        public string TOP_SCHEDULE { get; set; }

        public string MUSIC_COMPOSER_RADIO { get; set; }
        public string MUSIC_PERFORMER_RADIO { get; set; }
        public string MUSIC_TITLE_RADIO { get; set; }
        public string MUSIC_VERSION_RADIO { get; set; }

        public string SPECIFICITY { get; set; }

        public long ACTIVATION { get; set; }
        public string COMMENTARY { get; set; }

    }

    [TableName(Name = "TACCROCHE", Owner = "NPS")]
    public class TACCROCHE
    {
        public long ID_FORM { get; set; }

        public long NOACC { get; set; }
        public string LBACC { get; set; }
        public int ACTIVATION { get; set; }

        public DateTime DATE_MODIFICATION { get; set; }
        public DateTime DATE_CREATION { get; set; }

        //NOFAM		NUMBER(9)
        //NOCLA		NUMBER(9)
        //NOGRO		NUMBER(9)
        //NOVAR		NUMBER(9)
        //NOREF	NOT NULL	NUMBER(9)
        //NOANO		NUMBER(9)
        //NOSUP		NUMBER(9)
        //NOPRO		NUMBER(9)
        //NOMED	NOT NULL	NUMBER(9)
        //NOAGE		NUMBER(9)
        //NOCAT		NUMBER(9)
        //NOFCI		NUMBER(9)
        //NOCCI		NUMBER(9)
        //NOMAR		NUMBER(9)
        //CODEMAJ		VARCHAR2(1)
        //FLAGGESCO		NUMBER(1)
        //MAJ		VARCHAR2(50)


    }
}