using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace UnitTests.CS.JointureTests
{
    [TableName(Name = "SCRIPT", Owner = "ORQUAFR01")]
    public class FORM_SCRIPT
    {
        [MapField("ID_FORM"), PrimaryKey, NonUpdatable]
        public long ID_FORM { get; set; }

        //[MapField("SCRIPT")]
        //public string SCRIPT_FIELD { get; set; }

        public int ACTIVATION { get; set; }
    }
}