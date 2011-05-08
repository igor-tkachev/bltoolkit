using System;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Data.Linq.Model;
using NUnit.Framework;

namespace Data.Linq
{
    [TestFixture]
    public class IdlTest : TestBase
    {
        #region PersonWithId

        public interface IHasID
        {
            int ID { get; set; }
        }

        [TableName(Name = "Person")]
        public class PersonWithId : IHasID
        {
            public PersonWithId()
            {
            }

            public PersonWithId(int id)
            {
                ID = id;
            }

            public PersonWithId(int id, string firstName)
            {
                ID = id;
                FirstName = firstName;
            }

            [Identity, PrimaryKey]
            [SequenceName("Firebird", "PersonID")]
            [MapField("PersonID")]
            public int ID { get; set; }
            public string FirstName { get; set; }
            public string LastName;
            [Nullable]
            public string MiddleName;
            public Gender Gender;

            [MapIgnore]
            public string Name { get { return FirstName + " " + LastName; } }

            public override bool Equals(object obj)
            {
                return Equals(obj as PersonWithId);
            }

            public bool Equals(PersonWithId other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return
                    other.ID == ID &&
                    Equals(other.LastName, LastName) &&
                    Equals(other.MiddleName, MiddleName) &&
                    other.Gender == Gender &&
                    Equals(other.FirstName, FirstName);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var result = ID;
                    result = (result * 397) ^ (LastName != null ? LastName.GetHashCode() : 0);
                    result = (result * 397) ^ (MiddleName != null ? MiddleName.GetHashCode() : 0);
                    result = (result * 397) ^ Gender.GetHashCode();
                    result = (result * 397) ^ (FirstName != null ? FirstName.GetHashCode() : 0);
                    return result;
                }
            }
        }
        #endregion

        #region ObjectId

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

        public struct NullableObjectId
        {
            public NullableObjectId(int? value)
            {
                m_value = value;
            }

            private int? m_value;

            public int? Value
            {
                get { return m_value; }
                set { m_value = value; }
            }

            public static implicit operator int?(NullableObjectId val)
            {
                return val.m_value;
            }
        }
        #endregion

        [Test]
        public void TestComplexExpression()
        {
            // failed with BLToolkit.Data.Linq.LinqException : 'new StationObjectId() {Value = ConvertNullable(child.ChildID)}' 
            //   cannot be converted to SQL.
            ForMySqlProvider(
                db =>
                {
                    var source = from child in db.GrandChild
                                 select
                                     new
                                     {
                                              NullableId = new NullableObjectId { Value = child.ChildID }
                                     };

                    var query = from e in source where e.NullableId == 1 select e;

                    var result = query.ToArray();
                    Assert.That(result, Is.Not.Null);
                });
        }


        [Test]
        public void TestJoin()
        {
            // failed with System.ArgumentOutOfRangeException : Index was out of range. Must be non-negative and less than 
            //   the size of the collection.
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

        [Test]
        public void TestLookupWithInterfaceProperty()
        {
            ForMySqlProvider(
                db =>
                    {
                        var r = GetById<PersonWithId>(db, 1).SingleOrDefault();
                        Assert.That(r, Is.Not.Null);
                    });
        }

        #region ObjectExt

        public abstract class ObjectWithId
        {
            public ObjectId Id;
        }

        public class ParentEx : ObjectWithId
        {
            public int? Value1;
        }

        #endregion

        [Test]
        public void TestForObjectExt()
        {
            ForMySqlProvider(db =>
                {
                    var r = from p in db.Parent
                                select new ParentEx
                                {
                                    Id = new ObjectId { Value = p.ParentID },
                                    Value1 = p.Value1,
                                };
                    Assert.That(r.ToArray(), Is.Not.Null);
                });
        }

        private void getData(ITestDataContext db, IEnumerable<int?> d, IEnumerable<int?> compareWith)
        {
            var r1 = db.GrandChild
                .Where(x => d.Contains(x.ParentID))
                .GroupBy(x => x.ChildID, x => x.GrandChildID)
                .ToList();
            foreach (var group in r1)
            {
                Assert.That(compareWith.Any(x => group.Contains(x)), Is.True);
            }
        }

        [Test]
        public void TestForGroupBy()
        {
            ForMySqlProvider(db =>
                {
                    /* no error in first call */
                    getData(db, new List<int?> { 2 }, new List<int?> { 211, 212, 221, 222 });

                    /* error in second and more calls */
                    /*
                     * GROUP BY select clause is correct
                        SELECT x.ChildID FROM GrandChild x WHERE x.ParentID IN (3) GROUP BY x.ChildID

                     * But next SELECT clause contains "x.ParentID IN (2)" instead "x.ParentID IN (3)"
                        -- DECLARE ?p1 Int32
                        -- SET ?p1 = 31
                        SELECT x.GrandChildID FROM GrandChild x WHERE x.ParentID IN (2) AND x.ChildID = ?p1
                     */
                    getData(db, new List<int?> { 3 }, new List<int?> { 311, 312, 313, 321 });

                });
        }

        private static IQueryable<T> GetById<T>(ITestDataContext db, int id) where T : class, IHasID
        {
            return db.GetTable<T>().Where(obj => obj.ID == id);
        }

        private void ForMySqlProvider(Action<ITestDataContext> func)
        {
            ForEachProvider(Providers.Select(p => p.Name).Except(new [] {ProviderName.MySql}).ToArray(),func);
        }
    }
}
