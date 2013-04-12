using System;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;
using NUnit.Framework;
using UnitTests.CS.JointureTests.Factories;
using UnitTests.CS.JointureTests.Mappings;
using UnitTests.CS.JointureTests.Tools;

namespace UnitTests.CS.JointureTests
{
    [TestFixture]
    public abstract partial class AllTests
    {
        [Test]
        public void InsertBatchWithGenericDataProvider()
        {
            using (var db = ConnectionFactory.CreateDbManager())
            {
                var day = new DateTime(1950, 1, 1);

                var pageViews = new List<ValoNbPagesEvaliant> { new ValoNbPagesEvaliant { DateMedia = day, IdMedia = 10633, NbPages = 200 }, new ValoNbPagesEvaliant { DateMedia = day, IdMedia = 10634, NbPages = 300 }, };

                pageViews.ForEach(e =>
                    {
                        e.Activation = 0;
                        e.DateCreation = DateTime.Now;
                        e.DateModification = DateTime.Now;
                        e.IdLanguageDataI = 33;

                        e.IdSessionDetail = 0;
                    });

                db.UseQueryText = true;
                db.InsertBatch(pageViews);
                db.UseQueryText = false;
            }
        }

        [Test]
        public void InsertBatchWithIdentityWithTransaction()
        {
            var list = new List<DataImport>();
            for (int i = 0; i < 1000; i++)
            {
                list.Add(new DataImport
                    {
                        DeclaredProduct = "zzz" + i,
                        IdMedia = 2024,
                        DeclaredId = i,
                    });
            }

            using (new ExecTimeInfo())
            {
                using (var db = ConnectionFactory.CreateDbManager())
                {
                    if (ConnectionFactory.Provider is GenericDataProvider)
                        db.UseQueryText = true;

                    db.BeginTransaction();
                    db.InsertBatchWithIdentity(list.Take(10));
                    db.RollbackTransaction();
                }
            }
        }

        /// <summary>
        ///     Fastest method
        /// </summary>
        [Test]
        public void InsertBatchWithIdentityWithoutTransaction()
        {
            var list = new List<DataImport>();
            for (int i = 0; i < 1000; i++)
            {
                list.Add(new DataImport
                    {
                        DeclaredProduct = "zzz" + i,
                        IdMedia = 2024,
                        DeclaredId = i,
                    });
            }

            using (var db = ConnectionFactory.CreateDbManager())
            {
                if (ConnectionFactory.Provider is GenericDataProvider)
                    db.UseQueryText = true;

                db.InsertBatchWithIdentity(list.Take(2));
            }
        }

        [Test(Description = "Fast selection when using the UseQueryText property. Useful when dealing with oracle table partitions")]
        public void FastSelectionUsingQueryTextProperty()
        {
            var beginSpotPeriod = new DateTime(2012, 09, 03);

            using (var pitagorDb = ConnectionFactory.CreateDbManager())
            {
                pitagorDb.UseQueryText = true;

                var dbQuery = from dr in pitagorDb.GetTable<DATA_RADIO>()
                              join dv in pitagorDb.GetTable<DATA_VERSION>() on dr.ID_DATA_VERSION equals dv.ID_DATA_VERSION
                              where dr.DATE_MEDIA == beginSpotPeriod
                              select
                                  new DataEntryBroadcast
                                      {
                                          Id = dr.ID_DATA_VERSION,
                                          VersionId = dv.ID_MULTIMEDIA,
                                          DateMedia = dr.DATE_MEDIA,
                                          MediaId = dr.ID_MEDIA,
                                          SpotBegin = dr.DATE_SPOT_BEGINNING,
                                          SpotEnd = dr.DATE_SPOT_END,
                                      };


                dbQuery = dbQuery.Where(e => e.MediaId == 2015);

                var res = dbQuery.ToList();
                Assert.IsNotEmpty(res);
            }
        }
    }
}