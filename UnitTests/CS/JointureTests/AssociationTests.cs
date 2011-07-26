using System.Collections.Generic;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using NUnit.Framework;

namespace UnitTests.CS.JointureTests
{
    [TestFixture]
    internal class AssociationTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            DataProviderBase provider = new OdpDataProvider();
            DbManager.AddDataProvider(provider);
            DbManager.AddConnectionString(provider.Name,
                                          string.Format(
                                              "data source={0};User Id={1};Password={2};Incr Pool Size=1;Max Pool Size=3; Connection Timeout=15; pooling=false",
                                              "MUSICFR01.TEST", "scurutchet", "kisscool12"));
        }

        //[Test]
        public void SelectAllArtistsIgnoreLazyLoading()
        {
            using (var db = new MusicDB())
            {
                var query2 = new FullSqlQuery(db, true); //loading is automatic
                var artist2 = (Artist2) query2.SelectByKey(typeof (Artist2), 2643);
                List<Title> titles2 = artist2.Titles;

                var query = new FullSqlQuery(db); // Dont ignore lazyloading
                var artist = (Artist2) query.SelectByKey(typeof (Artist2), 2643);
                List<Title> titles = artist.Titles;
                Assert.AreEqual(titles2.Count, titles.Count);
            }
        }

        //[Test]
        public void SelectAllArtistsLazyLoading()
        {
            using (var db = new MusicDB())
            {
                DbManager dbCmd =
                    db.SetCommand("SELECT ID_ARTIST FROM PITAFR01.Artist where date_creation > sysdate - 200");
                dbCmd.MappingSchema = new FullMappingSchema();

                List<Artist2> art = dbCmd.ExecuteList<Artist2>();
                List<Title> titles = art[0].Titles;

                var query2 = new FullSqlQueryT<Artist2>(db);
                List<Artist2> artists = query2.SelectAll();
                Artist2 artist2 = artists[0];
                List<Title> titles2 = artist2.Titles;
            }
        }

        //[Test]
        public void SelectAllTitlesFull()
        {
            using (var db = new MusicDB())
            {
                var query = new FullSqlQueryT<Title>(db);
                var titles1 = query.SelectAll<List<Title>>();
                List<Title> titles2 = query.SelectAll();
            }
        }

        //[Test]
        public void SelectAllTitlesFull2()
        {
            using (var db = new MusicDB())
            {
                var query2 = new FullSqlQuery(db);
                List<Title> titles = query2.SelectAll<Title>();
                var title = (Title) query2.SelectByKey(typeof (Title), 137653);
            }
        }

        //[Test]
        public void SelectArtistFullWithLazyLoadingTitles()
        {
            using (var db = new MusicDB())
            {
                var query2 = new FullSqlQueryT<Artist2>(db);
                Artist2 artist = query2.SelectByKey(2643);
                List<Title> titles = artist.Titles;
            }
        }

        //[Test]
        public void SelectTitleFull()
        {
            using (var db = new MusicDB())
            {
                var query2 = new FullSqlQueryT<Title>(db);
                Title title = query2.SelectByKey(137653);
                string titleName = title.Name;
            }
        }
    }
}