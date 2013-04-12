using System;
using BLToolkit.Data;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using NUnit.Framework;

namespace UnitTests.CS.JointureTests.Factories
{
    [TestFixture]
    public class AdExpressTest : AllTests
    {
        #region Overrides of AssociationTests

        protected override DbConnectionFactory CreateFactory()
        {
            return new AdExpressFactory();
        }

        #endregion
    }

    [TableName(Name = "webnav01.static_nav_session")]
    public class static_nav_session
    {
        [MapField("ID_STATIC_NAV_SESSION"), PrimaryKey, Identity, SequenceName("WEBNAV01.SEQ_STATIC_NAV_SESSION")]
        public Int64 ID_STATIC_NAV_SESSION { get; set; }

        [MapField("PDF_NAME")]
        public String PDF_NAME { get; set; }

        [MapField("ID_PDF_RESULT_TYPE")]
        public Int64 ID_PDF_RESULT_TYPE { get; set; }

        [MapField("STATUS")]
        public Int16 STATUS { get; set; }

        [MapField("DATE_CREATION")]
        public DateTime DateCreation { get; set; }

        [MapField("DATE_MODIFICATION")]
        public DateTime DateModification { get; set; }

        [MapField("DATE_EXEC")]
        public DateTime DATE_EXEC { get; set; }

        [MapField("PDF_USER_FILENAME")]
        public String PDF_USER_FILENAME { get; set; }

        [MapField("ID_LOGIN")]
        public long ID_LOGIN { get; set; }

        [MapField("STATIC_NAV_SESSION")]
        //public Oracle.DataAccess.Types.OracleBlob STATIC_NAV_SESSION { get; set; }
        public byte[] STATIC_NAV_SESSION { get; set; }
    }
}