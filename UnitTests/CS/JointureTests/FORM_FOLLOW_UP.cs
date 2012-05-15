using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace UnitTests.CS.JointureTests
{
    [TableName(Name = "FORM_FOLLOW_UP", Owner = "ORQUAFR01")]
    public class FORM_FOLLOW_UP
    {
        [MapField("ID_FORM"), PrimaryKey, NonUpdatable]
        public long ID_FORM { get; set; }
        public long ID_FOLLOW_UP { get; set; }
        public int ACTIVATION { get; set; }
    }
}