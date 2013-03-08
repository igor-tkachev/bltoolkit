#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.IO;
using System.Linq;
using BLToolkit.Data;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.Mapping.Fluent;
using BLToolkit.Mapping.MemberMappers;
using Castle.DynamicProxy;
using NUnit.Framework;
using UnitTests.CS.JointureTests.Factories;
using UnitTests.CS.JointureTests.Mappings;
using UnitTests.CS.JointureTests.Tools;

#endregion

namespace UnitTests.CS.JointureTests
{
    //[TestFixture]
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
                MapField(_ => _.RefreshTime).MemberMapper(typeof (TimeSpanBigIntMapper)).DbType(System.Data.DbType.Int64);
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
                //var queryI = from a in db.GetTable<Artist>()
                //             where a.Id == 2471
                //             select a;

                //var aaa = queryI.First();
                //aaa.Name = "JE NE SUIS PAS UN HÃƒÂ‰ROS";


                //var data = new byte[] {0x4A, 0x45, 0x20, 0x4E, 0x45, 0x20, 0x53, 0x55, 0x49, 0x53, 0x20, 0x50, 0x41, 0x53, 0x20, 0x55, 0x4E, 0x20, 0x48, 0xC3, 0x83, 0xC2, 0x89, 0x52, 0x4F, 0x53};
                //aaa.Name = Encoding.UTF8.GetString(data);

                //var updateQuery = new SqlQuery(db);
                //updateQuery.Update(aaa);

                //var ccc = aaa.Id;

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
                DbManager dbCmd = db.SetCommand("SELECT ID_ARTIST FROM PITAFR01.Artist where date_creation > sysdate - 200");
                dbCmd.MappingSchema = new FullMappingSchema(db);

                // ExecuteList works only with LazyLoadingTrue
                List<Artist2> art = dbCmd.ExecuteList<Artist2>();

                foreach (Artist2 artist in art.Take(10))
                {
                    List<Title> titles = artist.Titles;
                    if (titles.Count > 5)
                        Console.WriteLine(artist.Name + " " + string.Join(",", titles.Select(e => e.Name)) + "\n");
                    //break;
                }
            }
        }

        [Test]
        public void SelectAllTitlesFullQueryAssociation()
        {
            using (var db = ConnectionFactory.CreateDbManager())
            {
                //db.MappingSchema = new FullMappingSchema(db);

                var query = new FullSqlQueryT<Title>(db);
                List<Title> titles2 = query.SelectAll();
                var artist = titles2.First().Artist;
                Console.WriteLine(artist.Name);
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
        public void FullMappingSchemaLazyLoading()
        {
            //Music factory

            using (var db = ConnectionFactory.CreateDbManager())
            {
                var mappingSchema = new FullMappingSchema(db);

                var objectMapper = mappingSchema.GetObjectMapper(typeof(Title));
                var title = (Title)objectMapper.CreateInstance();
                title.Id = 137653;

                Console.WriteLine(title.Artist);
                Console.WriteLine(title.Name);
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

                Console.WriteLine(title.Artist);
                Console.WriteLine(title.Name);
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
        public void TestLinqAssociation4()
        {
            using (var db = ConnectionFactory.CreateDbManager())
            {
                db.MappingSchema = new FullMappingSchema(db, mappingOrder: MappingOrder.ByColumnName);

                var query = from m in db.GetTable<Artist2>()
                            where m.Name.StartsWith("Metall")
                            select m;

                var res = query.ToList();
                var t = res.First().Titles;
                Console.WriteLine(t.Count);
            }
        }

        [Test]
        public void TestLinqAssociationOleron()
        {
            using (var db = ConnectionFactory.CreateDbManager())
            {
                db.MappingSchema = new FullMappingSchema(db, mappingOrder: MappingOrder.ByColumnName);

                var query = from m in db.GetTable<MULTIMEDIA_DATA_VERSION>()
                            where m.DataVersion.DataRadio != null
                            select
                                new
                                    {
                                        //m.MultimediaFiles,
                                        m.DataVersion.DataRadio,
                                        m.DataVersion,
                                        m
                                    };

                var res = query.Take(100).ToList();
                var t = res.First().m.DataVersion;
                Console.WriteLine(res.Count);
            }
        }

        [Test]
        public void TestQueryAssociation()
        {
            using (var db = ConnectionFactory.CreateDbManager())
            {
                db.MappingSchema = new FullMappingSchema(db, mappingOrder: MappingOrder.ByColumnName);

                string requete = File.ReadAllText(@"c:\requete3.txt");
                db.SetCommand(requete);
                var res = db.ExecuteList<MULTIMEDIA_DATA_VERSION>();
                var dataVersion = res.First().DataVersion;
                Console.WriteLine(dataVersion);
            }
        }
    }
}