using System.Linq;
using Data.Linq.Model;
using NUnit.Framework;

namespace Data.Linq
{
    [TestFixture]
    public class IdlTest : TestBase
    {
        public class ObjectID
        {
            public int Value { get; set; }
        }

        // fail
        [Test]
        public void Join1()
        {
            ForEachProvider(db =>
                {
                    var query1 = from p1 in db.Person
                    join p2 in db.Person on p1.ID equals p2.ID
                    select new
                        {
                            ID1 = new ObjectID {Value = p1.ID},
                            ID2 = new ObjectID {Value = p2.ID},
                            FirstName1 = p1.FirstName, 
                            FirstName2 = p2.FirstName, 
                            Gender1 = p1.Gender,
                            Gender2 = p2.Gender
                        };

                    var query2 = from p1 in query1
                                 select p1.ID1.Value;

                    var result = query2.ToArray();
                });
        }

        // fail
        [Test]
        public void Join2()
        {
            ForEachProvider(db =>
            {
                var query1 = from p1 in db.Person
                             join p2 in db.Person on p1.ID equals p2.ID
                             select new
                             {
                                 ID1 = new ObjectID { Value = p1.ID },
                                 ID2 = new ObjectID { Value = p2.ID },
                                 FirstName1 = p1.FirstName,
                                 FirstName2 = p2.FirstName,
                                 Gender1 = p1.Gender,
                                 Gender2 = p2.Gender
                             };

                var query2 = from p1 in query1
                             select p1.ID1;

                var result = query2.ToArray();
            });
        }

        // success
        [Test]
        public void Join3()
        {
            ForEachProvider(db =>
            {
                var query1 = from p1 in db.Person
                             join p2 in db.Person on p1.ID equals p2.ID
                             select new
                             {
                                 ID1 = new ObjectID { Value = p1.ID },
                                 ID2 = new ObjectID { Value = p2.ID },
                                 FirstName1 = p1.FirstName,
                                 FirstName2 = p2.FirstName,
                                 Gender1 = p1.Gender,
                                 Gender2 = p2.Gender
                             };

                var query2 = from p1 in query1
                             select p1;

                var result = query2.ToArray();
            });
        }
    }
}
