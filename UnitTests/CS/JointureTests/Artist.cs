using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace UnitTests.CS.JointureTests
{
    [TableName(Name = "ARTIST", Owner = Consts.Owner)]
    public class Artist
    {
        [MapField("ID_ARTIST"), PrimaryKey, KeyGenerator(PrimaryKeyGeneratorType.Sequence, true)]
        public long Id { get; set; }

        [MapField("ARTIST")]
        public string Name { get; set; }
    }


    [TableName(Name = "LABEL", Owner = Consts.Owner)]
    public class Label
    {
        [MapField("ID_LABEL"), PrimaryKey, SequenceName("SEQ_LABEL"), KeyGenerator(PrimaryKeyGeneratorType.Sequence, false)]
        public long Id { get; set; }

        [MapField("LABEL")]
        public string Name { get; set; }

        public int ID_USER_ { get; set; }
        public DateTime DATE_CREATION { get; set; }
        public DateTime DATE_MODIFICATION { get; set; }
        public int ACTIVATION { get; set; }
    }

    public class FredCopyright
    {
        [MapField("ID_LANGUAGE_DATA_I"), NonUpdatable]
        public int IdLanguage { get; set; }

        [MapField("DATE_CREATION"), NonUpdatable]
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

    [TableName(Name = "IMP_SEARCH", Owner = "PITAFR01")]
    public class DataImport : FredCopyright
    {
        [MapField("ID_IMP_SEARCH"), PrimaryKey, SequenceName("SEQ_IMP_SEARCH"), KeyGenerator(PrimaryKeyGeneratorType.Sequence, true)]
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