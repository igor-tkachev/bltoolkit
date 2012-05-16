using System;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using NUnit.Framework;

namespace UnitTests.CS.JointureTests
{
    [TestFixture]
    public class MusicTest : AssociationTests
    {
        #region Overrides of AssociationTests

        public override DbConnectionFactory CreateFactory()
        {
            return new MusicFactory(true);
        }

        #endregion
    }
    

    [TableName(Name = "DECLARATIVE_TRACK", Owner = Consts.MusicOwner)]
    public class DataDeclarativeTrack
    {
        [MapField("ID_DECLARATIVE_TRACK"), PrimaryKey]
        public Int64 IdDeclarativeTrack { get; set; }

        [MapField("DECLARATIVE_TRACK")]
        public String DeclarativeTrack { get; set; }

        [MapField("ID_USER_")]
        public Int64 IdUser { get; set; }

        [MapField("ID_TRACK")]
        public Int64? IdTrack { get; set; }

        [MapField("DATE_CREATION")]
        public DateTime DateCreation { get; set; }

        [MapField("DATE_MODIFICATION")]
        public DateTime DateModification { get; set; }

        [MapField("COMMENTARY")]
        public String Commentary { get; set; }

        [MapField("ACTIVATION")]
        public Int16 Activation { get; set; }

        [MapField("STATUS")]
        public Int16 Status { get; set; }

        [MapField("DECLARATIVE_ARTIST")]
        public String DeclarativeArtist { get; set; }
    }

    [TableName(Name = "DECLARATIVE_DATA", Owner = Consts.MusicOwner)]
    public class DataDeclarativeData
    {
        [MapField("ID_DECLARATIVE_DATA"), PrimaryKey]
        public Int64 IdDeclarativeData { get; set; }

        [MapField("ID_MEDIA")]
        public Int64 IdMedia { get; set; }

        [MapField("ID_DECLARATIVE_TRACK")]
        public Int64 IdDeclarativeTrack { get; set; }

        [MapField("DURATION")]
        public Int64 Duration { get; set; }

        [MapField("DATE_MEDIA")]
        public DateTime DateMedia { get; set; }

        [MapField("DATE_SPOT")]
        public DateTime DateSpot { get; set; }

        [MapField("ID_LANGUAGE_DATA_I")]
        public Int64 IdLanguageDataI { get; set; }

        [MapField("ID_USER_")]
        public Int64 IdUser { get; set; }

        [MapField("DATE_CREATION")]
        public DateTime DateCreation { get; set; }

        [MapField("DATE_MODIFICATION")]
        public DateTime DateModification { get; set; }

        [MapField("ACTIVATION")]
        public Int16 Activation { get; set; }
    }

    [TableName(Name = "MEDIA_SETTING", Owner = Consts.MusicOwner)]
    public class MediaSetting
    {
        [MapField("ID_MEDIA"), PrimaryKey]
        public Int64 IdMedia { get; set; }

        [MapField("ID_LANGUAGE_DATA_I")]
        public Int64 IdLanguageDataI { get; set; }

        [MapField("USER_")]
        public String User { get; set; }

        [MapField("ACTIVATION")]
        public ActivationMedia Activation { get; set; }
    }

    public enum ActivationMedia
    {        
        //[MapValue(5)]
        Priority = 5,
        //[MapValue(30)]
        Inactive = 30,
        [MapValue(0)]
        Default = 0,
    }


    public enum DeclarativeTitleStatus : short
    {
        Default = 0,
        Rejected = 1,
        Associated = 2,
        Locked = 3,
        SeeLater = 4,
    }

    public class Monitoring
    {
        //public DateTime DATEMEDIA { get; set; }
        public long IDDECLARATIVETRACK { get; set; }
        public Int16 ACTIVATION { get; set; }
    }

}