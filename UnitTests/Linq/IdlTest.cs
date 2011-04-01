using System;
using System.Linq;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;
using Data.Linq.Model;
using NUnit.Framework;

namespace Data.Linq
{
    [TestFixture]
    public class IdlTest : TestBase
    {
        public struct ObjectId
        {
            public ObjectId(int value)
            {
                m_value = value;
            }

            private int m_value;

            public int Value
            {
                get { return m_value; }
                set { m_value = value; }
            }

            public static implicit operator int(ObjectId val)
            {
                return val.m_value;
            }
        }

        [Test]
        public void TestComplexExpression()
        {
            // failed with BLToolkit.Data.Linq.LinqException : 'new StationObjectId() {Value = ConvertNullable(child.ChildID)}' cannot be converted to SQL.
            ForMySqlProvider(
                db =>
                {
                    var source = from child in db.GrandChild
                                      select
                                          new
                                          {
                                              NullableId =
                                                  child.ChildID == null
                                                      ? (ObjectId?)null
                                                      : new ObjectId { Value = child.ChildID.Value }
                                          };

                    var query = from e in source where e.NullableId == 1 select e;

                    var result = query.ToArray();
                    Assert.That(result, Is.Not.Null);
                });
        }


        [Test]
        public void TestJoin()
        {
            // failed with System.ArgumentOutOfRangeException : Index was out of range. Must be non-negative and less than the size of the collection.
            // Parameter name: index
            ForMySqlProvider(
                db =>
                {
                    var source = from p1 in db.Person
                                 join p2 in db.Person on p1.ID equals p2.ID
                                 select
                                     new { ID1 = new ObjectId { Value = p1.ID }, FirstName2 = p2.FirstName, };

                    var query = from p1 in source select p1.ID1.Value;

                    var result = query.ToArray();
                    Assert.That(result, Is.Not.Null);
                });
        }

        [Test]
        public void TestNullableExpression()
        {
            // failed with System.NullReferenceException : Object reference not set to an instance of an object.
            ForMySqlProvider(
                db =>
                {
                    var source = from obj in db.Person select new { Id = obj.ID, };

                    // fails for bool?, double?, int32?, int64?, string
                    // works for byte?, int16?, DateTime? 
                    double? @p1 = null;

                    var r = from c in source where @p1 != null select c;

                    Assert.That(r.ToArray(), Is.Not.Null);
                });
        }

        // success
        [Test]
        public void TestWhereExpression1()
        {
            ForMySqlProvider(db =>
                {
                    int id = 1;
                    var r = db.GetTable<Person>().Where(obj => obj.ID == id).SingleOrDefault();
                    Assert.That(r, Is.Not.Null);
                });
        }

        // success
        [Test]
        public void TestWhereExpression2()
        {
            ForMySqlProvider(db =>
            {
                var r = GetPersonById(db,1).SingleOrDefault();
                Assert.That(r, Is.Not.Null);
            });
        }

        // failed
        [Test]
        public void TestWhereExpression3()
        {
            ForMySqlProvider(db =>
                {
                    var r = GetById<Person>(db,1).SingleOrDefault();
                    Assert.That(r, Is.Not.Null);
                });
        }

        private IQueryable<Person> GetPersonById(ITestDataContext db, int id)
        {
            return db.GetTable<Person>().Where(obj => obj.ID == id);
        }

        private IQueryable<T> GetById<T>(ITestDataContext db, int id) where T : class, IHaseID
        {
            return db.GetTable<T>().Where(obj => obj.ID == id);
        }

        private void ForMySqlProvider(Action<ITestDataContext> func)
        {
            ForEachProvider(
                new[]
                    {
                        "Sql2008", "Sql2005", ProviderName.SqlCe, ProviderName.DB2, ProviderName.Informix,
                        ProviderName.Firebird, "Oracle", ProviderName.PostgreSQL, ProviderName.SQLite,
                        ProviderName.Sybase, ProviderName.Access
                    },
                func);
        }
    }
}
