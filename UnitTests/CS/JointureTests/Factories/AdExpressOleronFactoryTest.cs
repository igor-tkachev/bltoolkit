using System;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using NUnit.Framework;

namespace UnitTests.CS.JointureTests.Factories
{
    [TestFixture]
    public class AdExpressOleronFactoryTest : JointureTests
    {
        #region Overrides of AssociationTests

        protected override DbConnectionFactory CreateFactory()
        {
            return new AdExpressOleronFactory();
        }

        #endregion
    }

    [TableName(Name = "pitafr01.VALO_NB_PAGES_EVALIANT")]
    public class ValoNbPagesEvaliant
    {
        [PrimaryKey, MapField("ID_MEDIA")]
        public long IdMedia { get; set; }

        [PrimaryKey, MapField("DATE_MEDIA")]
        public DateTime DateMedia { get; set; }

        [MapField("NB_PAGES")]
        public int NbPages { get; set; }

        [MapField("ID_LANGUAGE_DATA_I")]
        public int IdLanguageDataI { get; set; }

        [MapField("DATE_CREATION")]
        public DateTime DateCreation { get; set; }

        [MapField("DATE_MODIFICATION")]
        public DateTime DateModification { get; set; }

        [MapField("ACTIVATION")]
        public int Activation { get; set; }

        [MapField("ID_SESSION_DETAIL")]
        public long IdSessionDetail { get; set; }
    }
}