using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Text;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.Mapping.Fluent;
using BLToolkit.Mapping.MemberMappers;
using NUnit.Framework;
using UnitTests.CS.JointureTests.Factories;
using UnitTests.CS.JointureTests.Mappings;
using UnitTests.CS.JointureTests.Tools;

namespace UnitTests.CS.JointureTests
{
    [TestFixture]
    public abstract class JointureTests : TestsBaseClass
    {
        public class Category
        {
            public int CategoryID;
            public string CategoryName;
            public string Description;
            public Binary Picture;
            //public AdditionalInfo AdditionalInfo;
            public List<Product> Products;
            public TimeSpan RefreshTime;
        }

        public class CategoryMap : FluentMap<Category>
        {
            public CategoryMap()
            {
                TableName("Categories");
                PrimaryKey(_ => _.CategoryID).Identity();
                Nullable(_ => _.Description);
                Nullable(_ => _.Picture);
                MapField(_ => _.RefreshTime).MemberMapper(typeof(TimeSpanBigIntMapper)).DbType(System.Data.DbType.Int64);
                //MapField(_ => _.AdditionalInfo).MapIgnore(false).MemberMapper(typeof(BinarySerialisationMapper)).DbType(System.Data.DbType.Byte);
                //Association(_ => _.Products, _ => _.CategoryID).ToMany((Product _) => _.CategoryID);
            }
        }

        public static void Main()
        {
            FluentConfig.Configure(Map.DefaultSchema).MapingFromAssemblyOf<Category>();
        }

        [Test]
        public void SelectAllArtistsIgnoreLazyLoading()
        {
            using (var db = ConnectionFactory.CreateDbManager())
            {
                var queryI = from a in db.GetTable<Artist>()
                             where a.Id == 2471
                             select a;

                var aaa = queryI.First();
                aaa.Name = "JE NE SUIS PAS UN HÃƒÂ‰ROS";


                var data = new byte[] {0x4A, 0x45, 0x20, 0x4E, 0x45, 0x20, 0x53, 0x55, 0x49, 0x53, 0x20, 0x50, 0x41, 0x53, 0x20, 0x55, 0x4E, 0x20, 0x48, 0xC3, 0x83, 0xC2, 0x89, 0x52, 0x4F, 0x53};
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
            using (var db = ConnectionFactory.CreateDbManager())
            {
                //var query = from a in db.GetTable<Artist3>()
                //            select a;
                //var res1 = query.Take(100);
                //var res2 = res1.ToList();
                //var res3 = res2.Take(50);

                DbManager dbCmd = db.SetCommand("SELECT ID_ARTIST FROM PITAFR01.Artist where date_creation > sysdate - 200");
                dbCmd.MappingSchema = new FullMappingSchema(db);

                // ExecuteList works only with LazyLoadingTrue
                List<Artist2> art = dbCmd.ExecuteList<Artist2>();

                foreach (Artist2 artist in art.Take(1))
                {
                    List<Title> titles = artist.Titles;
                    Console.WriteLine(titles.Count);
                    break;
                }
            }
        }

        [Test]
        public void SelectAllTitlesFullQueryAssociation()
        {
            using (var db = ConnectionFactory.CreateDbManager())
            {
                var query = new FullSqlQueryT<Title>(db);
                List<Title> titles2 = query.SelectAll();
                Assert.IsTrue(titles2.Count > 0);
            }
        }

        [Test]
        public void SelectArtistWithTitlesLazyLoadingOnQueryAssociation()
        {
            using (var db = ConnectionFactory.CreateDbManager())
            {
                var query2 = new FullSqlQueryT<Artist2>(db);
                Artist2 artist = query2.SelectByKey(2643);
                List<Title> titles = artist.Titles;
                Console.WriteLine(titles.Count);
            }
        }

        [Test]
        public void TestLinqAssociation()
        {
            using (var db = ConnectionFactory.CreateDbManager())
            {
                var query = from m in db.GetTable<MULTIMEDIA_DATA_VERSION>()
                            where m.DataVersion.DataRadio != null
                            select new
                                {
                                    //m.MultimediaFiles,
                                    m.DataVersion.DataRadio,
                                    m.DataVersion,
                                    m
                                };

                var res = query.Take(100).ToList();
                Console.WriteLine(res.Count);
            }
        }

        [Test(Description = "BUG : Need to be fixed")]
        public void TestLinqAssociation2()
        {
            using (var db = ConnectionFactory.CreateDbManager())
            {
                var query = from m in db.GetTable<Artist2>()
                            where m.Id == 1833
                            select new
                                {
                                    m.Name,
                                    m.Titles
                                };

                var res = query.ToList();
                Assert.IsNotEmpty(res);
            }
        }

        [Test]
        public void TestQueryAssociation()
        {
            using (var db = ConnectionFactory.CreateDbManager())
            {
                db.MappingSchema = new FullMappingSchema(db, db.MappingSchema, mappingOrder: MappingOrder.ByColumnName, ignoreMissingColumns: true);

                string requete = File.ReadAllText(@"c:\requete3.txt");
                db.SetCommand(requete);
                var res = db.ExecuteList<MULTIMEDIA_DATA_VERSION>();
                Console.WriteLine(res.Count);
            }
        }

        [Test]
        public void SelectTitleWithArtistQueryAssociation()
        {
            //Music factory

            using (var db = ConnectionFactory.CreateDbManager())
            {
                var query = new FullSqlQuery(db);
                var title = (Title)query.SelectByKey(typeof(Title), 137653);
                Console.WriteLine(title.Name);

                var query2 = new FullSqlQueryT<Title>(db);
                var title2 = query2.SelectByKey(137653);
                Console.WriteLine(title2.Name);
            }
        }
    }
}