using System;
using System.Linq;
using BLToolkit.Data.DataProvider;
using NUnit.Framework;

namespace Data.Linq
{
    [TestFixture]
    public class IdlTest : TestBase
    {
        public struct StationObjectId
        {
            public StationObjectId(int value)
            {
                m_value = value;
            }

            private int m_value;

            public int Value
            {
                get { return m_value; }
                set { m_value = value; }
            }

            public static implicit operator int(StationObjectId val)
            {
                return val.m_value;
            }
        }

        private void TestJoin(Action<ITestDataContext> func)
        {
            ForEachProvider(
                new[]
                    {
                        "Sql2008", "Sql2005", ProviderName.SqlCe, ProviderName.DB2, ProviderName.Informix,
                        ProviderName.Firebird, "Oracle", ProviderName.PostgreSQL, ProviderName.SQLite, ProviderName.Sybase
                        , ProviderName.Access
                    }, func);
        }

        // failed with BLToolkit.Data.Linq.LinqException : 'new StationObjectId() {Value = ConvertNullable(child.ChildID)}' cannot be converted to SQL.
        [Test]
        public void Join1()
        {
            TestJoin(
                db =>
                    {
                        var dbm = db;

                        var allEquities = from child in dbm.GrandChild
                                          select
                                              new
                                              {
                                                  NullableId =
                                                      child.ChildID == null
                                                          ? (StationObjectId?)null
                                                          : new StationObjectId { Value = child.ChildID.Value }
                                              };

                        var query = from e in allEquities where e.NullableId == 1 select e;

                        var result = query.ToArray();
                        Assert.That(result, Is.Not.Null);
                    });
        }

        // failed with System.NullReferenceException : Object reference not set to an instance of an object.
        [Test]
        public void Join2()
        {
            TestJoin(
                db =>
                    {
                        var objects = from obj in db.Person select new { Id = obj.ID, };

                        double? @p1 = null;

                        var r = from c in objects where @p1 != null select c;

                        Assert.That(r.ToArray(), Is.Not.Null);
                    });
        }

        // failed with System.ArgumentOutOfRangeException : Index was out of range. Must be non-negative and less than the size of the collection.
        // Parameter name: index
        [Test]
        public void Join3()
        {
            TestJoin(
                db =>
                    {
                        var source = from p1 in db.Person
                                     join p2 in db.Person on p1.ID equals p2.ID
                                     select
                                         new { ID1 = new StationObjectId { Value = p1.ID }, FirstName2 = p2.FirstName, };

                        var query = from p1 in source
                                    select p1.ID1.Value;

                        var result = query.ToArray();
                        Assert.That(result, Is.Not.Null);
                    });
        }
    }
}
