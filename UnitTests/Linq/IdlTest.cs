﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public partial class IdlTest : TestBase
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

        public interface IHasObjectId2
        {
            ObjectId Id { get; set; }
        }

        public interface IHasObjectId1
        {
            ObjectId Id { get; set; }
        }

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

        public class WithObjectIdBase : IHasObjectId1
        {
            public ObjectId Id { get; set; }
        }
        
        public class PersonWithObjectId : WithObjectIdBase, IHasObjectId2
        {
            public string FistName { get; set; }
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
                    getData(db, new List<int?> { 3 }, new List<int?> { 311, 312, 313, 321, 333 });

                });
        }

        [Test]
        public void TestLinqMax()
        {
            ForMySqlProvider(
                db =>
                    {
                        Assert.That(db.Patient.Where(x => x.PersonID < 0).Select(x => (int?)x.PersonID).Max(), Is.Null);
                        Assert.That(db.Patient.Where(x => x.PersonID < 0).Max(x => (int?)x.PersonID), Is.Null);
                        Assert.Catch<InvalidOperationException>(
                            () => db.Patient.Where(x => x.PersonID < 0).Select(x => x.PersonID).Max());
                        Assert.Catch<InvalidOperationException>(
                            () => db.Patient.Where(x => x.PersonID < 0).Max(x => x.PersonID));
                    });
        }

        [Test]
        public void TestConvertFunction()
        {
            ForMySqlProvider(
                db =>
                {
                    var ds = new IdlPatientSource(db);
                    var r1 = ds.Patients().ToList();
                    var r2 = ds.Persons().ToList();

                    Assert.That(r1, Is.Not.Empty);
                    Assert.That(r2, Is.Not.Empty);

                    var r3 = ds.Patients().ToIdlPatientEx(ds);
                    var r4 = r3.ToList();
                    Assert.That(r4, Is.Not.Empty);
                });
        }

        [Test]
        public void TestJoinOrder()
        {
            ForMySqlProvider(
                db =>
                    {
                        var source = new IdlPatientSource(db);

                        // Success when use result from second JOIN
                        var query1 = from p1 in source.GrandChilds()
                                     join p2 in source.Persons() on p1.ParentID equals p2.Id
                                     join p3 in source.Persons() on p1.ChildID equals p3.Id
                                     select
                                         new
                                             {
                                                 p1.ChildID, 
                                                 p1.ParentID,
                                                 //Parent = p2,
                                                 Child = p3,
                                             };
                        var data1 = query1.ToList();

                        // Fail when use result from first JOIN
                        var query2 = from p1 in source.GrandChilds()
                                    join p2 in source.Persons() on p1.ParentID equals p2.Id
                                    join p3 in source.Persons() on p1.ChildID equals p3.Id
                                    select
                                        new
                                        {
                                            p1.ChildID,
                                            p1.ParentID,
                                            Parent = p2,
                                            //Child = p3,
                                        };
                        var data2 = query2.ToList();
                    });
        }

        private static IQueryable<T> GetById<T>(ITestDataContext db, int id) where T : class, IHasID
        {
            return db.GetTable<T>().Where(obj => obj.ID == id);
        }

        private void ForMySqlProvider(Action<ITestDataContext> func)
        {
            ForEachProvider(Providers.Select(p => p.Name).Except(new[] { ProviderName.MySql }).ToArray(), func);
        }

        [Test]
        public void ImplicitCastTest()
        {
            ForMySqlProvider(db =>
            {
                var people =
                    from p in db.Person
                    select new IdlPerson
                    {
                        Id   = new ObjectId { Value = p.ID },
                        Name = p.FirstName
                    };

                var sql1 = (from p in people where p.Id       == 1 select p).ToString();
                var sql2 = (from p in people where p.Id.Value == 1 select p).ToString();

                Assert.That(sql1, Is.EqualTo(sql2));
            });
        }

        [Test]
        public void ListvsArrayTest()
        {
            ForMySqlProvider(
                db =>
                {
                    var st = "John";

                    //SQL - x.FirstName IN ('John')
                    var queryList = from x in db.Person
                                    where new List<string> { st }.Contains(x.FirstName)
                                    select x.ID;

                    //SQL - x.FirstName IN ('J', 'o', 'h', 'n')
                    var queryArray = from x in db.Person
                                     where new[] { st }.Contains(x.FirstName)
                                     select x.ID;

                    Assert.That(queryList.ToList(), Is.EqualTo(queryArray.ToList()));
                });
        }

        [Test]
        public void ConcatJoinOrderByTest()
        {
            ForMySqlProvider(
                db =>
                {

                    var query = from y in
                                    ((from pat in db.Patient
                                      where pat.Diagnosis == "a"
                                      select pat)
                                    .Concat
                                    (from pat in db.Patient
                                     where pat.Diagnosis == "b"
                                     select pat))
                                join person in db.Person on y.PersonID equals person.ID
                                orderby person.ID
                                select new { Id = person.ID, Id2 = y.PersonID };

                    Assert.That(query.ToList(), Is.Not.Null);

                });
        }

        [Test]
        public void TestIsContainedInArrayOfEnumValues()
        {
            var types2 = new[] { TypeValue.Value2, TypeValue.Value3, TypeValue.Value4 };

            ForMySqlProvider(
                db =>
                    {
                        var result = (from x in db.Parent4 where types2.Contains(x.Value1) select x)
                            .ToList();

                        Assert.That(result, Is.Not.Null);
                    });
        }

        [Test]
        public void TestQueryWithInterface()
        {
            ForMySqlProvider(
                db =>
                    {
                        var persons =
                            from x in db.Person
                            select new PersonWithObjectId
                                {
                                    Id = new ObjectId { Value = x.ID },
                                    FistName = x.FirstName,
                                };

                        // this works
                        var r1 = FilterSourceByIdDefinedInBaseClass(persons, 5).ToArray();
                        Assert.That(r1, Is.Not.Null);

                        // and this works
                        var r2 = FilterSourceByIdDefinedInInterface1(persons, 5).ToArray();
                        Assert.That(r2, Is.Not.Null);

                        // but this fails
                        var r3 = FilterSourceByIdDefinedInInterface2(persons, 5).ToArray();
                        Assert.That(r3, Is.Not.Null);
                    });
        }

        private IQueryable<T> FilterSourceByIdDefinedInBaseClass<T>(IQueryable<T> source, int id) where T : WithObjectIdBase
        {
            return from x in source where x.Id == id select x;
        }

        private IQueryable<T> FilterSourceByIdDefinedInInterface1<T>(IQueryable<T> source, int id) where T : IHasObjectId1
        {
            return from x in source where x.Id == id select x;
        }

        private IQueryable<T> FilterSourceByIdDefinedInInterface2<T>(IQueryable<T> source, int id) where T : IHasObjectId2
        {
            return from x in source where x.Id == id select x;
        }

        [Test]
        public void TestComparePropertyOfEnumTypeToVaribleInSubquery()
        {
            ForMySqlProvider(
                db =>
                    {
                        var gender = Gender.Other;
                        var q = from x in db.Patient
                                join y in db.Person.Where(x => x.Gender == gender) on x.PersonID equals y.ID
                                select x;

                        var r = q.ToList();
                        Assert.That(r, Is.Not.Null);
                    });
        }

        #region GenericQuery classes

        public abstract partial class GenericQueryBase
        {
            private readonly IdlPatientSource m_ds;

            protected GenericQueryBase(ITestDataContext ds)
            {
                m_ds = new IdlPatientSource(ds);
            }

            #region Object sources

            protected IQueryable<IdlPerson> AllPersons
            {
                get { return m_ds.Persons(); }
            }

            protected IQueryable<IdlPatient> AllPatients
            {
                get { return m_ds.Patients(); }
            }

            protected IQueryable<IdlGrandChild> AllGrandChilds
            {
                get { return m_ds.GrandChilds(); }
            }

            #endregion

            public abstract IEnumerable<object> Query();
        }

        public class GenericConcatQuery : GenericQueryBase
        {
            private System.String @p1;
            private System.Int32 @p2;

            public GenericConcatQuery(ITestDataContext ds, object[] args)
                : base(ds)
            {
                @p1 = (System.String)args[0];
                @p2 = (System.Int32)args[1];
            }

            public override IEnumerable<object> Query()
            {
                return (from y in AllPersons
                        select y.Name)
                            .Concat(
                                from x in AllPersons
                                from z in AllPatients
                                where (x.Name == @p1 || z.Id == new ObjectId { Value = @p2 })
                                select x.Name
                            );
            }
        }

        public class GenericConcatJoinOrderQuery : GenericQueryBase
        {
            private String @p1;
            private Int32 @p2;

            public GenericConcatJoinOrderQuery(ITestDataContext ds, object[] args)
                : base(ds)
            {
                @p1 = (System.String)args[0];
                @p2 = (System.Int32)args[1];
            }

            public override IEnumerable<object> Query()
            {
                return (from j in
                            (from y in AllPersons
                             select new { FirstName = y.Name })
                                .Concat(
                                    from x in AllPersons
                                    from z in AllPatients
                                    where (x.Name == @p1 || z.Id == new ObjectId { Value = @p2 })
                                    select new { FirstName = x.Name }
                                )
                        join g in AllGrandChilds on j.FirstName equals @p1
                        orderby g.ParentID.Value
                        select new { FirstName = g.ParentID.Value.ToString() });
            }
        }

        #endregion

        [Test]
        public void TestMono01()
        {
            ForMySqlProvider(
                 db =>
                 {
                     var ds = new IdlPatientSource(db);
                     var t = "A";
                     var query =
                         (from y in ds.Persons()
                          select y.Name)
                             .Concat(
                                 from x in ds.Persons()
                                 where x.Name == t
                                 select x.Name
                             );

                     Assert.That(query.ToList(), Is.Not.Null);
                 });
        }

        [Test]
        public void TestMono03()
        {
            ForMySqlProvider(
                db => Assert.That(new GenericConcatQuery(db, new object[] { "A", 1 }).Query().ToList(), Is.Not.Null));
        }

        [Test]
        public void TestMono04()
        {
            ForMySqlProvider(
                db => Assert.That(new GenericConcatJoinOrderQuery(db, new object[] { "A", 1 }).Query().ToList(), Is.Not.Null));
        }

        public static IQueryable<TSource> Concat2<TSource>(IQueryable<TSource> source1, IEnumerable<TSource> source2)
        {
            return source1.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    typeof(Queryable).GetMethod("Concat").MakeGenericMethod(typeof(TSource)),
                    new[] { source1.Expression, Expression.Constant(source2, typeof (IEnumerable<TSource>)) }));
        }

        [Test]
        public void TestMonoConcat()
        {
            ForMySqlProvider(db =>
            {
                var ds = new IdlPatientSource(db);
                var t  = "A";
                var query = Concat2(
                    from y in ds.Persons() select y.Name,
                    from x in ds.Persons() where x.Name == t select x.Name);

                Assert.That(query.ToList(), Is.Not.Null);
            });
        }

        [Test]
        public void TestMonoConcat2()
        {
            ForMySqlProvider(
            db =>
            {
                var ds = new IdlPatientSource(db);
                var t = "A";
                var query1 = Concat2(
                from y in ds.Persons() select y.Name,
                from x in ds.Persons() where x.Name == t select x.Name);

                Assert.That(query1.ToList(), Is.Not.Null);
            });

            ForMySqlProvider(
            db =>
            {
                var ds = new IdlPatientSource(db);
                var t = "A";
                var query2 = Concat2(
                from y in ds.Persons() select y.Name,
                from x in ds.Persons() where x.Name == t select x.Name);

                Assert.That(query2.ToList(), Is.Not.Null);
            });
        }
    }

    #region TestConvertFunction classes

    public class IdlPatient
    {
        public IdlTest.ObjectId Id { get; set; }
    }

    public class IdlPerson
    {
        public IdlTest.ObjectId Id { get; set; }
        public string Name { get; set; }
    }

    public class IdlGrandChild
    {
        public IdlTest.ObjectId ParentID { get; set; }
        public IdlTest.ObjectId ChildID { get; set; }
        public IdlTest.ObjectId GrandChildID { get; set; }
    }

    public class IdlPatientEx : IdlPatient
    {
        public IdlPerson Person { get; set; }
    }

    public class IdlPatientSource
    {
        private readonly ITestDataContext m_dc;

        public IdlPatientSource(ITestDataContext dc)
        {
            m_dc = dc;
        }

        public IQueryable<IdlGrandChild> GrandChilds()
        {
                return m_dc.GrandChild.Select(x => new IdlGrandChild
                    {
                        ChildID = new IdlTest.ObjectId {Value = x.ChildID.Value},
                        GrandChildID = new IdlTest.ObjectId { Value = x.GrandChildID.Value },
                        ParentID = new IdlTest.ObjectId { Value = x.ParentID.Value }
                    });
        }

        public IQueryable<IdlPatient> Patients()
        {
            return m_dc.Patient.Select(x => new IdlPatient { Id = new IdlTest.ObjectId { Value = x.PersonID }, });
        }

        public IQueryable<IdlPerson> Persons()
        {
            return
                m_dc.Person.Select(
                    x => new IdlPerson { Id = new IdlTest.ObjectId { Value = x.ID }, Name = x.FirstName, });
        }
    }

    public static class IdlPersonConverterExtensions
    {
        public static IEnumerable<IdlPatientEx> ToIdlPatientEx(this IQueryable<IdlPatient> list, IdlPatientSource source)
        {
            return from x in list
                   join person in source.Persons() on x.Id.Value equals person.Id.Value
                   select new IdlPatientEx
                       {
                           Id = x.Id,
                           Person = new IdlPerson { Id = new IdlTest.ObjectId { Value = person.Id }, Name = person.Name,},
                       };
        }
    }

    #endregion
}