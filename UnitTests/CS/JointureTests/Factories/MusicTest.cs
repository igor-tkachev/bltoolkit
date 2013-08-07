using System;
using BLToolkit.Data;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using NUnit.Framework;
using UnitTests.CS.JointureTests.Factories;
using UnitTests.CS.JointureTests.Mappings;

namespace UnitTests.CS.JointureTests
{
    [TestFixture]
    public class MusicTest : AllTests
    {
        #region Overrides of AssociationTests

        protected override DbConnectionFactory CreateFactory()
        {
            return new MusicFactory(false);
        }

        #endregion
    }

    [TableName(Name = "MULTIMEDIA_CONTEXT", Owner = "PITAFR01")]
    public class MultimediaContext
    {
        [MapField("ID_MEDIA")]
        public long MediaId { get; set; }
        [MapField("DATE_PARUTION")]
        public long DateParution { get; set; }
    }

    [TableName(Name = "DATA_MEDIA", Owner = "PITAFR01")]
    public class DataMedia
    {
        [MapField("ID_MEDIA")]
        public long MediaId { get; set; }
        [MapField("DATE_MEDIA")]
        public DateTime Date { get; set; }
        [MapField("ID_DATA_MEDIA")]
        public long Id { get; set; }
    }

[TableName(Name = "PITAFR01.MEDIA")]
public class DataMedia2
{
    [MapField("ID_MEDIA"), PrimaryKey]
    public Int64 IdMedia { get; set; }

    [MapField("MEDIA")]
    public String Media { get; set; }

    [MapField("ID_LANGUAGE_DATA")]
    public Int64 IdLanguageData { get; set; }

    [MapField("ACTIVATION")]
    public Int16 Activation { get; set; }
}

    [TableName(Name = "PITAFR01.MEDIA_SETTING")]
    public class DataMediaSetting
    {
        [MapField("ID_MEDIA"), PrimaryKey]
        public Int64 IdMedia { get; set; }

        [MapField("CAPTURE_CODE")]
        public String CaptureCode { get; set; }

        [MapField("ID_LANGUAGE_DATA_I")]
        public Int64 IdLanguageDataI { get; set; }

        [MapField("ID_USER_")]
        public Int64 IdUser { get; set; }

        [MapField("DATE_CREATION")]
        public DateTime DateCreation { get; set; }

        [MapField("DATE_MODIFICATION")]
        public DateTime DateModification { get; set; }

        [MapField("COMMENTARY")]
        public String Commentary { get; set; }

        [MapField("ACTIVATION")]
        public Int16 Activation { get; set; }
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

    public class TitleQuery
    {
        public Title Title { get; set; }
        public Artist Artist { get; set; }
    }

    [TableName(Name = "DATA_MUSIC", Owner = Consts.MusicOwner)]
    public abstract class DataMusic
    {
        [MapField(MapName = "ID_DATA_MUSIC"), PrimaryKey, Identity, NonUpdatable, SequenceName("SEQ_DATA_MUSIC")]
        public long Id { get; set; }

        [MapField(MapName = "ID_MEDIA")]
        public long MediaId { get; set; }

        [MapField(MapName = "ID_TRACK")]
        public long TitleId { get; set; }

        [MapField(MapName = "DATE_SPOT")]
        public DateTime DateSpot { get; set; }

        [MapField(MapName = "DATE_MEDIA")]
        public DateTime DateMedia { get; set; }
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

namespace Test_Query_Update_Duration.DbTypes
{
    [TableName(Name = "PITAFR01.DATA_MUSIC")]
    public class DataMusic
    {
        [MapField("ID_DATA_MUSIC"), PrimaryKey]
        public Int64 Id { get; set; }

        [MapField("ID_MEDIA")]
        public Int64 IdMedia { get; set; }

        [MapField("ID_LANGUAGE_DATA_I")]
        public Int64 IdLanguageDataI { get; set; }

        [MapField("ID_TRACK")]
        public Int64 IdTrack { get; set; }

        [MapField("DURATION")]
        public Int64 DurationInSeconds { get; set; }

        [MapField("ID_USER_")]
        public Int64 UserId { get; set; }

        [MapField("DATE_SPOT")]
        public DateTime DateSpot { get; set; }

        [MapField("DATE_MEDIA")]
        public DateTime DateMedia { get; set; }

        [MapField("DATE_CREATION")]
        public DateTime DateCreation { get; set; }

        [MapField("DATE_MODIFICATION")]
        public DateTime ModifiedAt { get; set; }

        [MapField("COMMENTARY")]
        public String Commentary { get; set; }

        [MapField("ACTIVATION")]
        public Int16 Activation { get; set; }

    }

}