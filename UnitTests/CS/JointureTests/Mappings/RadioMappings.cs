#region

using System;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

#endregion

namespace UnitTests.CS.JointureTests.Mappings
{
    [TableName(Name = "PRODUCT", Owner = "PITAFR01")]
    public class PRODUCT
    {
        [MapField("ID_PRODUCT"), PrimaryKey, NonUpdatable]
        public long Id { get; set; }

        [MapField("PRODUCT")]
        public string Name { get; set; }

        [MapField("ID_SEGMENT")]
        public long SegmentId { get; set; }

        public int ID_LANGUAGE_DATA { get; set; }
    }

    [TableName(Name = "SEGMENT", Owner = "PITAFR01")]
    public class SEGMENT
    {
        [MapField("ID_SEGMENT"), PrimaryKey, NonUpdatable]
        public long Id { get; set; }

        [MapField("ID_GROUP_")]
        public long GroupId { get; set; }

        public int ID_LANGUAGE_DATA { get; set; }
    }

    [TableName(Name = "MULTIMEDIA", Owner = "PITAFR01")]
    public class MULTIMEDIA_TABLE
    {
        [PrimaryKey]
        public long ID_MULTIMEDIA { get; set; }

        public long ID_PRODUCT { get; set; }

        public DateTime DATE_CREATION { get; set; }

        public int STATUS_QUALI { get; set; }
        public int ID_CATEGORY_MULTIMEDIA { get; set; }
        public int ID_VEHICLE_I { get; set; }
        public int ACTIVATION { get; set; }
    }
}