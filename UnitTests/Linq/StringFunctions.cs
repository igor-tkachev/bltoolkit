using System;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Linq;

using NUnit.Framework;

using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class StringFunctions : TestBase
	{
		[Test]
		public void Length()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where p.FirstName.Length == "John".Length && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void ContainsConstant()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where p.FirstName.Contains("oh") && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void ContainsConstant2()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where !p.FirstName.Contains("o%h") && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void ContainsParameter()
		{
			var str = "oh";

			ForEachProvider(db => 
			{
				var q = from p in db.Person where p.FirstName.Contains(str) && p.ID == 1 select new { p, str };
				var r = q.ToList().First();
				Assert.AreEqual(1,   r.p.ID);
				Assert.AreEqual(str, r.str);
			});
		}

		[Test]
		public void ContainsParameter2()
		{
			var str = "o%h";

			ForEachProvider(db => 
			{
				var q = from p in db.Person where !p.FirstName.Contains(str) && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void StartsWith()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where p.FirstName.StartsWith("Jo") && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void EndsWith()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where p.FirstName.EndsWith("hn") && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Like11()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where SqlMethods.Like(p.FirstName, "%hn%") && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Like12()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where !SqlMethods.Like(p.FirstName, @"%h~%n%", '~') && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Like21()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where p.FirstName.Like("%hn%") && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Like22()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where !p.FirstName.Like(@"%h~%n%", '~') && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void IndexOf1()
		{
			ForEachProvider(new[] { ProviderName.Firebird, ProviderName.Informix }, db => 
			{
				var q = from p in db.Person where p.FirstName.IndexOf("oh") == 1 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void IndexOf2()
		{
			ForEachProvider(new[] { ProviderName.Firebird, ProviderName.Informix }, db => 
			{
				var q = from p in db.Person where p.LastName.IndexOf("p", 1) == 2 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void IndexOf3()
		{
			var n = 3;
			ForEachProvider(new[] { ProviderName.Firebird, ProviderName.Informix, ProviderName.SqlCe }, db => 
			{
				var q = from p in db.Person where p.LastName.IndexOf("p", 1, n) == 2 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		static readonly string[] _lastIndexExcludeList = new[]
		{
			ProviderName.DB2, ProviderName.Firebird, ProviderName.Informix, ProviderName.SqlCe, ProviderName.Access
		};

		[Test]
		public void LastIndexOf1()
		{
			ForEachProvider(_lastIndexExcludeList, db => 
			{
				var q = from p in db.Person where p.LastName.LastIndexOf("p") == 2 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void LastIndexOf2()
		{
			ForEachProvider(_lastIndexExcludeList, db => 
			{
				var q = from p in db.Person where p.ID == 1 select new { p.ID, FirstName = "123" + p.FirstName + "012345" };
				q = q.Where(p => p.FirstName.LastIndexOf("123", 5) == 7);
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void CharIndex1()
		{
			ForEachProvider(new[] { ProviderName.Firebird, ProviderName.Informix }, db => 
			{
				var q = from p in db.Person where p.FirstName.CharIndex("oh") == 2 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void CharIndex2()
		{
			ForEachProvider(new[] { ProviderName.Firebird, ProviderName.Informix }, db => 
			{
				var q = from p in db.Person where p.LastName.CharIndex("p", 2) == 3 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Left()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where p.FirstName.Left(2) == "Jo" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Substring1()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where p.FirstName.Substring(1) == "ohn" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Substring2()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where p.FirstName.Substring(1, 2) == "oh" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Reverse()
		{
			ForEachProvider(new[] { ProviderName.DB2, ProviderName.Informix, ProviderName.SqlCe, ProviderName.Access }, db => 
			{
				var q = from p in db.Person where p.FirstName.Reverse() == "nhoJ" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Test()
		{
			using (var db = new TestDbManager(ProviderName.Informix))
			{
				var p = db
					.SetCommand(@"
						SELECT
							p.PersonID,
							p.LastName,
							p.MiddleName,
							p.Gender,
							p.FirstName
						FROM
							Person p
						WHERE
							NOT p.FirstName LIKE ? ESCAPE '~' AND p.PersonID = 1",
						db.Parameter("?", "%o~%h%", DbType.String))
					.ExecuteObject<Person>();

				Assert.AreEqual(1, p.ID);
			}
		}
	}
}
