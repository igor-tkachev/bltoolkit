using System;
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
        public void TestQuery()
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
            using (var db = _connectionFactory.CreateDbManager())
            {
                var query = from dt in db.GetTable<DataDeclarativeTrack>()
                            join dd in db.GetTable<DataDeclarativeData>() on dt.IdDeclarativeTrack equals dd.IdDeclarativeTrack
                            join ms in db.GetTable<MediaSetting>() on dd.IdMedia equals ms.IdMedia
                            where dt.Status == (short) DeclarativeTitleStatus.Default ||
                                  dt.Status == (short) DeclarativeTitleStatus.Locked
                            select new {dd.DateMedia, dt.IdDeclarativeTrack, ms.Activation};

                if (false)
                    query = query.Where(e => e.Activation == ActivationMedia.Priority);
                else
                    query = query.Where(e => e.Activation == ActivationMedia.Default);

                var res = query.Distinct().ToList();
                Assert.IsNotEmpty(res);
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
            string sql = "select                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               " +
            "FORM.ID_FORM,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        " +
            "FORM.ID_STRIKE, " +
            "FORM.DATE_CREATION, " + 

            "TACCROCHE.LBACC,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     " +
            "DATA_RADIO.SPECIFICITY,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              " +
            "SCRIPT.SCRIPT,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       " +
            "SIGNATURE.SIGNATURE,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 " +
            "FORM_FOLLOW_UP.ID_FOLLOW_UP,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         " +
            "FORM_CAMPAIGN.ID_CAMPAIGN,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           " +
            "FORM_SUBJECT.ID_SUBJECT,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             " +
            "FORM_VISUAL_KEYWORD.ID_VISUAL_KEYWORD                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                " +
            "from                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 " +
            "ORQUAFR01.FORM                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       " +
            "INNER JOIN DATA_RADIO ON DATA_RADIO.ID_FORM = FORM.ID_FORM                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           " +
            "INNER JOIN NPS.TACCROCHE ON NPS.TACCROCHE.NOACC = FORM.ID_STRIKE                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     " +
            "LEFT OUTER JOIN ORQUAFR01.SCRIPT                                                 ON SCRIPT.ID_FORM                                                   = FORM.ID_FORM                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  " +
            "LEFT OUTER JOIN ORQUAFR01.SIGNATURE                                       ON SIGNATURE.ID_FORM                                         = FORM.ID_FORM                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                " +
            "LEFT OUTER JOIN ORQUAFR01.FORM_FOLLOW_UP                      ON FORM_FOLLOW_UP.ID_FORM                        = FORM.ID_FORM                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        " +
            "LEFT OUTER JOIN ORQUAFR01.FORM_CAMPAIGN                        ON FORM_CAMPAIGN.ID_FORM                          = FORM.ID_FORM                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      " +
            "LEFT OUTER JOIN ORQUAFR01.FORM_SUBJECT                                              ON FORM_SUBJECT.ID_FORM                                 = FORM.ID_FORM                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           " +
            "LEFT OUTER JOIN ORQUAFR01.FORM_VISUAL_KEYWORD         ON FORM_VISUAL_KEYWORD.ID_FORM            = FORM.ID_FORM                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       " +
            "where 0 = 0                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          " +
            "and FORM.ACTIVATION = 0                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              " +
            "and TACCROCHE.ACTIVATION = 0                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         " +
            "and DATA_RADIO.ACTIVATION = 0                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        " +
            "and FORM.ID_VEHICLE = 2                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              " +
            "and FORM.ID_LANGUAGE_I = 33                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          " +
            "and FORM.ID_STRIKE in  (226216276, 226216517, 226216735, 226216747, 226216836, 226217152, 226217585, 226217716, 226218437, 226218504, 226218961, 226219984, 226220449, 226221179, 226221313, 226222276, 226222367, 226222528, 226222585, 226223188, 226223776, 226224846, 226224945, 226225497, 226225590, 226225608, 226226396, 226226429, 226226469, 226226479, 226228186, 226228197, 226228948, 226230072, 226230436, 226233628, 226233642, 226235385, 226235386, 226238022, 226240097, 226249835, 226278153, 226278227, 226279224, 226279249, 226280067, 226280159, 226281243, 226281317, 226283199, 226287680, 226288654, 226292566, 226293711, 226294644, 226295667, 226295725, 226297720, 226302808, 226303327, 226304060, 226307160, 226317465, 226318569, 226322524, 226325394, 226326238, 226330062, 226333837, 226335688, 226338817, 226338843, 230236149, 230246133, 230312807, 230327959, 230334979, 230375109, 230381589, 230385900, 230396831, 230402908, 230407674, 230417274, 230453610, 230459565, 230462246, 230465877, 230466228, 230475641, 230478434, 230478468, 230481478, 230484600, 230512157, 230514318, 230536716, 230540704, 230552786, 230560532, 230564343, 230574266, 230590735, 230593825, 230603532, 230615801, 230634403, 230636430, 230652441, 230781461, 230782137, 230784220, 230788580, 230790697, 230795637, 230797945, 230799610, 230803846, 230803882, 230811870, 230813666, 230819992, 230821099, 230823263, 230823264, 230840694, 230843419, 230861607, 230874110, 230885209, 230889790, 230903047, 230914238, 230929076, 230929078, 230932020, 230943061, 230968532, 230970944, 230971514, 230974270, 230975817, 230976273, 230981906, 230996489, 231011211, 231012622, 231018687, 231034495, 231034527, 231034550, 231034595, 231055572, 231062477, 231067614, 231067668, 231078644, 231078891, 231081001, 231081002, 231085682, 231086558, 231134852, 231135224, 231135307, 231135731, 231135823, 231135826, 231136794)" +
            "and TACCROCHE.NOMED = 2" +
            "ORDER BY FORM.ID_FORM";

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


            //Console.WriteLine("------------------------------------------------------------------------------");

            //GetMediaLinq();
            //GetMediaLinq();
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