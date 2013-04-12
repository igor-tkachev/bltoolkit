#region

using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using UnitTests.CS.JointureTests;

#endregion

namespace PitagorDataAccess.Mappings.DataEntry
{
    [TableName(Name = "MULTIMEDIA_COBRANDING", Owner = Consts.Owner)]
    public class MultimediaCobranding
    {
        [MapField("ID_MULTIMEDIA")]
        public long MultimediaId { get; set; }

        [MapField("ID_PRODUCT")]
        public long ProductId { get; set; }

        [MapField("ID_CATEGORY_MULTIMEDIA")]
        public long CategoryMultimediaId { get; set; }
    }
}