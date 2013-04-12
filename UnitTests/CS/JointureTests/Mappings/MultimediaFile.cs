#region

using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using UnitTests.CS.JointureTests;

#endregion

namespace PitagorDataAccess.Mappings.DataEntry
{
    [TableName(Name = "MULTIMEDIA_FILE", Owner = Consts.Owner)]
    public class MultimediaFile
    {
        [MapField("ID_MULTIMEDIA_FILE")]
        public long Id { get; set; }

        [MapField("FILE_PATH")]
        public string FilePath { get; set; }

        [MapField("FILE_NAME")]
        public string FileName { get; set; }

        [MapField("ID_FILE_NUMBER")]
        public long FileNumberId { get; set; }

        [MapField("ID_MULTIMEDIA")]
        public long MultimediaId { get; set; }
    }
}