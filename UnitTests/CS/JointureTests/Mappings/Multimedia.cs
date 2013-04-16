using System;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using UnitTests.CS.JointureTests;

namespace PitagorDataAccess.Mappings.DataEntry
{

    [TableName(Name = "PRODUCT", Owner = Consts.Owner)]
    public class CProduct 
    {
        private long _activation;

        [MapField("ID_PRODUCT")]
        public  long Id { get; set; }
        [MapField("PRODUCT")]
        public  string Label { get; set; }
        [MapField("ID_LANGUAGE_DATA")]
        public long LanguageId { get; set; }
        [MapField("ACTIVATION")]
        public long Activation { get; set; }
        [MapField("ID_ADVERTISER")]
        public long AdvertiserId { get; set; }

        [MapField("ID_SEGMENT")]
        public long SegmentId { get; set; }
    }

    [TableName(Name = "MULTIMEDIA", Owner = Consts.Owner)]
    public class Multimedia
    {
        private const int multimediaIndexSplit = 50;

        [MapField("ID_MULTIMEDIA")]
        public long Id { get; set; }
        [MapField("MULTIMEDIA")]
        public string MultimediaName
        {
            get; set; //get
            //{
            //    if (string.IsNullOrWhiteSpace(AdditionalInformation))
            //        return Label.Trim();

            //    return Label.Trim().PadRight(multimediaIndexSplit, ' ') + AdditionalInformation.Trim();
            //}
            //set
            //{
            //    if (value == null)
            //    {
            //        Label = "";
            //        AdditionalInformation = "";
            //    }
            //    else if (value.Length > multimediaIndexSplit)
            //    {
            //        Label = value.Substring(0, multimediaIndexSplit).Trim();
            //        AdditionalInformation = value.Substring(multimediaIndexSplit).Trim();
            //    }
            //    else
            //    {
            //        Label = value.Trim();
            //        AdditionalInformation = "";
            //    }
            //}
        }
        [MapField("DURATION")]
        public long DurationInSeconds
        {
            get; set;
            //get { return (long)Duration.TotalSeconds; }
            //set { Duration = TimeSpan.FromSeconds(value); }
        }

        [MapField("ID_CATEGORY_MULTIMEDIA")]
        public long CategoryMultimediaId { get; set; }
        [MapField("ACTIVATION")]
        public long Activation { get; set; }

        [MapField("ID_PRODUCT")]
        public long ProductId { get; set; }

        //[MapIgnore]
        //public override TimeSpan Duration { get { return base.Duration; } set { base.Duration = value; } }
        //[MapIgnore]
        //public override string Label { get { return base.Label; } set { base.Label = value; } }
        //[MapIgnore]
        //public override string AdditionalInformation { get { return base.AdditionalInformation; } set { base.AdditionalInformation = value; } }
        //[MapIgnore]
        //public override bool IsProductValid { get { return base.IsProductValid; } set { base.IsProductValid = value; } }
        //[MapIgnore]
        //public override string ClipPath { get { return base.ClipPath; } set { base.ClipPath = value; } }
        //[MapIgnore]
        //public override string ClipNameWithoutExtension { get { return base.ClipNameWithoutExtension; } set { base.ClipNameWithoutExtension = value; } }
        //[MapIgnore]
        //public override DateTime? DateBroadcastMax { get { return base.DateBroadcastMax; } set { base.DateBroadcastMax = value; } }
        //[MapIgnore]
        //public override long? MultimediaFileId{get { return base.MultimediaFileId; }set { base.MultimediaFileId = value; }}

        public override bool Equals(object obj)
        {
            var comp = obj as Multimedia;

            if (comp == null)
                return false;

            return Equals(comp);
        }

        protected bool Equals(Multimedia other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}