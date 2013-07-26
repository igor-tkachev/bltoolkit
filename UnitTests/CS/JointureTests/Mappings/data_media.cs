using System;
using BLToolkit.DataAccess;

namespace UnitTests.CS.JointureTests.Mappings
{
    [TableName(Name = "data_media")]
    public class data_media
    {
        [PrimaryKey]
        public long ID_MEDIA { get; set; }
        [PrimaryKey]
        public long ID_DATA_MEDIA { get; set; }
        public long ID_LANGUAGE_DATA_I { get; set; }
        public long ID_RATE_CARD { get; set; }
        public long ID_LANGUAGE { get; set; }
        public long ID_PERIODICITY { get; set; }
        public long ID_REGION_DATA { get; set; }
        public long ID_USER_ { get; set; }
        public long TOTAL_PAGE { get; set; }
        public long? TOTAL_SUPPLEMENT { get; set; }
        public string COPY_NUMBER { get; set; }
        public long? WEEK_NUMBER { get; set; }
        public long? REEL { get; set; }
        public long? DATA_PRESENCE { get; set; }
        public DateTime? DATE_PUBLICATION { get; set; }
        public DateTime? DATE_MEDIA { get; set; }
        public DateTime DATE_BEGINNING_DATA { get; set; }
        public DateTime DATE_END_DATA { get; set; }
        public long DURATION_DATA { get; set; }
        public DateTime DATE_MODIFICATION { get; set; }
        public DateTime DATE_CREATION { get; set; }
        public string COMMENTARY { get; set; }
        public long ACTIVATION { get; set; }
        public DateTime? DATE_VALORISATION { get; set; }
        public DateTime? DATE_EXPORT { get; set; }
        public string NO_PRINT_TOMORROW { get; set; }
        public long? ID_DATA_SOURCE { get; set; }
        public long ID_SESSION_DETAIL { get; set; }
        public string APPLICATION_VERSION { get; set; }

        public override string ToString()
        {
            return string.Format("" + ID_MEDIA);
        }
    }
}