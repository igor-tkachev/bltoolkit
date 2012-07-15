﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using BLToolkit.Data;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using NUnit.Framework;

namespace UnitTests.CS.JointureTests
{
    [TestFixture]
    public abstract class AssociationTests
    {
        private DbConnectionFactory _connectionFactory;

        public abstract DbConnectionFactory CreateFactory();

        [TestFixtureSetUp]
        public void Setup()
        {
            _connectionFactory = CreateFactory();

            DbManager.AddDataProvider(_connectionFactory.Provider);
            DbManager.AddConnectionString(_connectionFactory.Provider.Name, _connectionFactory.ConnectionString);

            DbManager.TurnTraceSwitchOn();
        }

        private void WriteTraceLine(string s, string s1)
        {
            Console.WriteLine(s + " : " + s1);
        }

        [Test]
        public void TestQueryAssociation()
        {
            using (var db = _connectionFactory.CreateDbManager())
            {
                db.MappingSchema = new FullMappingSchema(inheritedMappingSchema: db.MappingSchema, mappingOrder: MappingOrder.ByColumnName,
                                                                ignoreMissingColumns: true);

                db.SetCommand(File.ReadAllText(@"c:\requete.txt"));
                var res = db.ExecuteList<MULTIMEDIA_DB>();
                Assert.IsNotEmpty(res);
            }
        }

        [Test]
        public void TestGetDataRadioWithTimeSpan()
        {
            using (var db = _connectionFactory.CreateDbManager())
            {
                var query = from m in db.GetTable<DATA_VERSION>()
                            where m.DATE_CREATION.TimeOfDay > TimeSpan.FromHours(11)
                            select m;
                query = query.Take(5);
                var res = query.ToList();
                Assert.IsNotEmpty(res);
            }
        }

        [Test]
        public void TestLinqAssociation()
        {
            using (var db = _connectionFactory.CreateDbManager())
            {
                var query = from m in db.GetTable<MULTIMEDIA_DB>()
                            where m.ID_MULTIMEDIA == 31265081
                            select new 
                            {
                                //m.MultimediaFiles,
                                m.DataVersion.DataRadio,
                                m.DataVersion,
                                m
                            };

                var res = query.ToList();
                Assert.IsNotEmpty(res);
            }
        }

        [Test]
        public void TestLinqAssociation2()
        {
            using (var db = _connectionFactory.CreateDbManager())
            {
                var query = from m in db.GetTable<Artist2>()
                            where m.Id == 1833
                            select new
                            {
                                m.Name, m.Titles
                            };

                var res = query.ToList();
                Assert.IsNotEmpty(res);
            }
        }

        [Test]
        public void SelectTooLong2()
        {
            using (var db = _connectionFactory.CreateDbManager())
            {
                db.SetCommand(File.ReadAllText(@"c:\requete2.txt"));
                var res = db.ExecuteList<Monitoring>();

                Assert.IsNotEmpty(res);
            }
        }

        [Test]
        public void SelectTooLong()
        {
            using (new ExecTimeInfo())
            {
                using (var db = _connectionFactory.CreateDbManager())
                {
                    var query = from dt in db.GetTable<DataDeclarativeTrack>()
                                join dd in db.GetTable<DataDeclarativeData>() on dt.IdDeclarativeTrack equals dd.IdDeclarativeTrack
                                join ms in db.GetTable<MediaSetting>() on dd.IdMedia equals ms.IdMedia
                                where dt.Status == (short)DeclarativeTitleStatus.Default ||
                                      dt.Status == (short)DeclarativeTitleStatus.Locked
                                select new { dd.DateMedia, dt.IdDeclarativeTrack, ms.Activation };

                    query = false
                                ? query.Where(e => e.Activation == ActivationMedia.Priority)
                                : query.Where(e => e.Activation == ActivationMedia.Default);

                    var res = query.Distinct().ToList();
                    Assert.IsNotEmpty(res);
                }
            }
        }

        [Test]
        public void SelectInPeriod()
        {
            using (new ExecTimeInfo())
            {
                using (var db = _connectionFactory.CreateDbManager())
                {

                    var beginMinute = 0;
                    var beginHour = 0;

                    var endMinute = 50;
                    var endHour = 23;

                    IQueryable<TitleQuery> queryTitle = from data in db.GetTable<DataMusic>()
                                                        join title in db.GetTable<Title>() on data.TitleId equals
                                                            title.Id
                                                        join artist in db.GetTable<Artist>() on title.ArtistID
                                                            equals artist.Id
                                                        where data.MediaId == 2002
                                                              &&
                                                              data.DateMedia >=
                                                              DateTime.Today
                                                              &&
                                                              data.DateMedia <=
                                                              DateTime.Today
                                                              &&
                                                              data.DateSpot.TimeOfDay >=
                                                              new TimeSpan(beginHour, beginMinute, 0)
                                                              &&
                                                              data.DateSpot.TimeOfDay <=
                                                              new TimeSpan(endHour, endMinute, 0)
                                                        select new TitleQuery
                                                                   {
                                                                       Title = title,
                                                                       Artist = artist,
                                                                   };
                    var res = queryTitle.ToList();
                }
            }
        }


        [Test]
        public void InsertWithIdentity()
        {
            using (var db = _connectionFactory.CreateDbManager())
            {
                var t = db.GetTable<RECO_RADIO>();
                var now = DateTime.Now.AddDays(30);
                t.InsertWithIdentity(
                    () =>
                    new RECO_RADIO
                    {
                        ACTIVATION = 0,
                        COMMENTARY = string.Empty,
                        DATE_CREATION = DateTime.Now,
                        DATE_LAST_IMPORT = DateTime.Today,
                        DATE_MEDIA = DateTime.Today,
                        DATE_MODIFICATION = DateTime.Now,
                        ID_LANGUAGE_DATA_I = 33,
                        ID_MEDIA = 2001,
                        ID_MULTIMEDIA_FILE = 463413,
                        ID_MULTIMEDIA_VALIDATED = 0,
                        INPUT_STATUS = 0,
                        RATE = 0,
                        TAG_DURATION = 20,
                        TAG_MATCH_BEGINNING = 0,
                        TAG_MATCH_DURATION = 20,
                        TIME_MEDIA = now
                    });
            }
        }

        [Test]
        public void InsertWithIdentityQuery()
        {
            using (var db = _connectionFactory.CreateDbManager())
            {
                var dataProduct = new DataProductPending()
                    {
                        Activation = 0,
                        Commentary = "Valeriu",
                        DateCreation = DateTime.Now,
                        DateModification = DateTime.Now,
                        ProductPending = "Valeriu",
                        IdUserCreate = 0,
                        UserProgram = "Valeriu"
                    };

                var query = new SqlQuery(db);
                var res = query.InsertWithIdentity(dataProduct);
                Console.WriteLine(res);
                Console.WriteLine(db.LastQuery);                
            }
        }

        [Test]
        public void InsertWithIdentityDbManager()
        {
            using (var db = _connectionFactory.CreateDbManager())
            {
                var dataProduct = new DataProductPending()
                {
                    Activation = 0,
                    Commentary = "Valeriu",
                    DateCreation = DateTime.Now,
                    DateModification = DateTime.Now,
                    ProductPending = "Valeriu",
                    IdUserCreate = 0,
                    UserProgram = "Valeriu"
                };

                var res = db.InsertWithIdentity(dataProduct);
                Console.WriteLine(res);
                Console.WriteLine(db.LastQuery);
            }
        }

        [Test]
        public void InsertWithIdentityProductPending()
        {
            using (var db = _connectionFactory.CreateDbManager())
            {                
                var t = db.GetTable<DataProductPending>();
                var now = DateTime.Now.AddDays(30);
                var res =  t.InsertWithIdentity(
                    () =>
                    new DataProductPending()
                    {
                        Activation = 0,
                        Commentary = "Valeriu",
                        DateCreation = DateTime.Now,
                        DateModification = DateTime.Now,
                        ProductPending = "Valeriu",
                        IdUserCreate = 0,
                        UserProgram = "Valeriu"
                    });
                Console.WriteLine(res);
                Console.WriteLine(db.LastQuery);
            }
        }

        [Test]
        public void TestUpdate()
        {
            using (var db = _connectionFactory.CreateDbManager())
            {
                var query = from d in db.GetTable<DataProductPending>()
                            where d.IdProductPending == 1
                            select d;

                DataProductPending productPending = query.First();
                productPending.DateModification = DateTime.Now;

                var queryUpdate = new SqlQuery(db);
                queryUpdate.Update(productPending);

                Console.WriteLine(db.LastQuery);
            }
        }

        [Test]
        public void InsertBatch()
         {            
            using (var db = _connectionFactory.CreateDbManager())
            {
                db.InsertBatch(new[]
                                   {
                                       new DataImport()
                                           {
                                               Commentary = "aaaa",
                                               DeclaredProduct = "cc",
                                               IdMedia = 2024,
                                               DeclaredId = 1,
                                           },
                                       new DataImport()
                                           {
                                               Commentary = "bbb",
                                               DeclaredProduct = "ddd",
                                               IdMedia = 2024,
                                               DeclaredId = 1,
                                           }
                                   });
            }
         }

        [Test]
        public void InsertArtistWithAutoSequence()
        {
            using (var db = _connectionFactory.CreateDbManager())
            {
                var query = new SqlQuery(db);
                var artist = new Label() { Name = "TEST", DATE_CREATION = DateTime.Now, DATE_MODIFICATION = DateTime.Now, ACTIVATION = 10, ID_USER_ = 200 };

                query.Insert(new DataImport()
                                 {
                                     Commentary = "aaaa",
                                     DeclaredProduct = "ssfsfsfsfsfsf",
                                     IdMedia = 2024,
                                     DeclaredId = 1
                                 });
            }
        }

        [Test]
        public void SelectTest()
        {
            using (var db = _connectionFactory.CreateDbManager())
            {
                IQueryable<Mapping> resultG = from d in db.GetTable<DataMapping>()
                                              where d.IdLocker == null && d.IdLanguage == 33
                                              select new Mapping
                                                         {
                                                             IdMapping = d.IdMapping,
                                                             DeclaredId = d.DeclaredId,
                                                             DeclaredProduct = d.DeclaredProduct,
                                                             MappingState = (MappingState) d.Activation,
                                                             Product = new Product
                                                                           {
                                                                               IdProduct =
                                                                                   d.IdProduct == null
                                                                                       ? 0
                                                                                       : d.IdProduct.Value
                                                                           },
                                                             ProductPending = new ProductPending
                                                                                  {
                                                                                      IdProductPending =
                                                                                          d.IdProductPending == null
                                                                                              ? 0
                                                                                              : d.IdProductPending.Value
                                                                                  }
                                                         };

                var res = resultG.Distinct().OrderBy(p => p.IdMapping).ToList();
            }
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
                aaa.Name = "JE NE SUIS PAS UN HÃƒÂ‰ROS";
                

                var data = new byte[]{0x4A,0x45,0x20,0x4E,0x45,0x20,0x53,0x55,0x49,0x53,0x20,0x50,0x41,0x53,0x20,0x55,0x4E,0x20,0x48,0xC3,0x83,0xC2,0x89,0x52,0x4F,0x53};
                aaa.Name = Encoding.UTF8.GetString(data);

                var updateQuery = new SqlQuery(db);
                updateQuery.Update(aaa);

                var ccc = aaa.Id;

                var query2 = new FullSqlQuery(db, true); //loading is automatic
                var artist2 = (Artist2) query2.SelectByKey(typeof (Artist2), 2643);
                List<Title> titles2 = artist2.Titles;

                var query = new FullSqlQuery(db);
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

                foreach (Artist2 artist in art)
                {
                    List<Title> titles = artist.Titles;   
                    break;
                }                

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
                Assert.IsTrue(titles2.Count > 0);
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
        public void SelectTest1()
        {
            string sql = File.ReadAllText(@"c:\requeteOrqua.txt");

            var forms = new List<FILE_FORM>();
            using (var db = new MusicDB())
            {
                //var query = from f in 


                using (var a = new ExecTimeInfo())
                {
                    //db.SetCommand(sql);
                    //var reader = db.ExecuteReader();
                    //var schemaTable = reader.GetSchemaTable();
                    //while (reader.Read())
                    //{
                    //    //var values = new object[schemaTable.Columns.Count];
                    //    //reader.GetValues(values);
                    //    reader.GetValue(0);
                    //}

                    DbManager dbCmd = db.SetCommand(sql);
                    dbCmd.MappingSchema = new FullMappingSchema(inheritedMappingSchema: dbCmd.MappingSchema, mappingOrder: MappingOrder.ByColumnName,
                                                                ignoreMissingColumns: true, ignoreLazyLoad: false);
                    var allMedia = dbCmd.ExecuteList<FILE_FORM>();
                    forms = allMedia;
                    foreach (FILE_FORM fileForm in allMedia)
                    {
                        if (fileForm.SCRIPT_FIELD != null && !fileForm.SCRIPT_FIELD.IsNull)
                        {
                            //string clobValue = fileForm.SCRIPT_FIELD.Value;
                        }
                    }
                }
            }
        }

        [Test]
        public void SelectTitleFull()
        {
            string req = " select m.ID_MEDIA, m.ID_BASIC_MEDIA, m.ACTIVATION from PITAFR01.MEDIA m " +
                         " inner join PITAFR01.BASIC_MEDIA bm on m.ID_BASIC_MEDIA  = bm.ID_BASIC_MEDIA " +
                         " where m.ACTIVATION = 0 and bm.ACTIVATION = 0 and bm.ID_CATEGORY IN (21, 24, 25, 27, 38, 221) ";
        
            GetMediaReq(req);
            GetMediaReq(req);


            Console.WriteLine("------------------------------------------------------------------------------");

            GetMediaLinq();
            GetMediaLinq();
        }

        private static void GetMediaLinq()
        {
            var all = new List<long> { 21, 24, 25, 27, 38, 221 };

            using (var db = new MusicDB())
            {
                using (var a = new ExecTimeInfo())
                {
                    var query = from m in db.GetTable<Media>()
                                join basicMedia in db.GetTable<BasicMedia>() on m.ID_BASIC_MEDIA equals
                                    basicMedia.ID_BASIC_MEDIA
                                where m.ACTIVATION == 0 && basicMedia.ACTIVATION == 0 && all.Contains(basicMedia.ID_CATEGORY)
                                select m;
                    var allMedia = query.ToList();
                }
                using (var a = new ExecTimeInfo())
                {
                    var query = from m in db.GetTable<Media>()
                                join basicMedia in db.GetTable<BasicMedia>() on m.ID_BASIC_MEDIA equals
                                    basicMedia.ID_BASIC_MEDIA
                                where m.ACTIVATION == 0 && basicMedia.ACTIVATION == 0 && all.Contains(basicMedia.ID_CATEGORY)
                                select m;
                    var allMedia = query.ToList();
                }

                using (var a = new ExecTimeInfo())
                {
                    var query = from basicMedia in db.GetTable<BasicMedia>()
                                where basicMedia.ACTIVATION == 0
                                select basicMedia;
                    var allMedia = query.ToList();
                }
                using (var a = new ExecTimeInfo())
                {
                    var query = from basicMedia in db.GetTable<BasicMedia>()
                                where basicMedia.ACTIVATION == 0
                                select basicMedia;
                    var allMedia = query.ToList();
                }
            }
        }

        private static void GetMediaReq(string req)
        {
            using (var db = new MusicDB())
            {
                using (var a = new ExecTimeInfo())
                {
                    db.SetCommand(req);
                    var allMedia = db.ExecuteList<Media>();
                }
                using (var a = new ExecTimeInfo())
                {
                    db.SetCommand(req);
                    var allMedia = db.ExecuteList<Media>();
                }
            }
        }
    }
}