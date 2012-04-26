using System;
using System.Collections.Generic;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

namespace UnitTests.CS.JointureTests
{
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

        //public int ACTIVATION { get; set; }
        public string COMMENTARY { get; set; }
        public long ID_LANGUAGE_I { get; set; }
        
        public DateTime DATE_MODIFICATION { get; set; }

        public string CLIP { get; set; }
        public string AWARD { get; set; }

        // Dates en integer ..
        //public int DATE_MODIFICATION_DATA { get; set; }
        //public int DATE_MEDIA { get; set; }
        //public int DATE_DATA { get; set; }

        //[Association(ThisKey = "ID_FORM", OtherKey = "ID_FORM")]
        //public FORM_SCRIPT FORM_SCRIPT { get; set; }

        //[LazyInstance(false)]
        //[Association(ThisKey = "ID_FORM", OtherKey = "ID_FOLLOW_UP")]
        //public List<FORM_FOLLOW_UP> FORM_FOLLOW_UPs { get; set; }
    }
}