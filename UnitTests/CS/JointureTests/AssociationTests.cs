using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using NUnit.Framework;

namespace UnitTests.CS.JointureTests
{
    [TestFixture]
    public class AssociationTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            var aa = DbProviderFactories.GetFactoryClasses();
            var bb = aa.Rows.Count;

            DataProviderBase provider = new OdpDataProvider();
            //DataProviderBase provider = new GenericDataProvider(ProviderFullName.Oracle);
            DbManager.AddDataProvider(provider);
            DbManager.AddConnectionString(provider.Name,
                                                      string.Format(
                                                          "data source={0};User Id={1};Password={2};",
                                                          "MUSICFR01.TEST", "scurutchet", "kisscool12"));
            //DbManager.AddConnectionString(provider.Name,
            //                              string.Format(
            //                                  "data source={0};User Id={1};Password={2};Incr Pool Size=1;Max Pool Size=3; Connection Timeout=15; pooling=false",
            //                                  "MUSICFR01.TEST", "scurutchet", "kisscool12"));
        }

        [Test]
        public void SelectAllArtistsIgnoreLazyLoading()
        {
            using (var db = new MusicDB())
            {

                var queryI = from a in db.GetTable<Artist>()
                             where a.Id == 2471
                             select a;
                
                var aaa = queryI.First();
                var ccc = aaa.Id;

                var query2 = new FullSqlQuery(db, true); //loading is automatic
                var artist2 = (Artist2) query2.SelectByKey(typeof (Artist2), 2643);
                List<Title> titles2 = artist2.Titles;

                var query = new FullSqlQuery(db, false); // Dont ignore lazyloading
                var artist = (Artist2) query.SelectByKey(typeof (Artist2), 2643);
                List<Title> titles = artist.Titles;
                Assert.AreEqual(titles2.Count, titles.Count);
            }
        }

        [Test]
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

        [Test]
        public void SelectAllTitlesFull()
        {
            using (var db = new MusicDB())
            {
                var query = new FullSqlQueryT<Title>(db);
                var titles1 = query.SelectAll<List<Title>>();
                List<Title> titles2 = query.SelectAll();
            }
        }

        [Test]
        public void SelectAllTitlesFull2()
        {
            using (var db = new MusicDB())
            {
                var query2 = new FullSqlQuery(db);
                List<Title> titles = query2.SelectAll<Title>();
                var title = (Title) query2.SelectByKey(typeof (Title), 137653);
            }
        }

        [Test]
        public void SelectArtistFullWithLazyLoadingTitles()
        {
            using (var db = new MusicDB())
            {
                //db.Prepare();
                var query2 = new FullSqlQueryT<Artist2>(db);
                var sw = new Stopwatch();
                sw.Start();
                for (int i = 0; i < 10000; i++)
                {
                    Artist2 artist = query2.SelectByKey(2643);
                    //List<Title> titles = artist.Titles;
                }
                sw.Stop();                
            }
        }

        [Test]
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