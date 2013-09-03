﻿using System;
using System.Data.Linq.SqlClient;
using System.Linq;

using NUnit.Framework;

using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;

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
		public void ContainsConstant3([DataContexts] string context)
		{
			using (var db = GetDataContext(context))
			{
				var arr = new[] { "oh", "oh'", "oh\\" };

				var q = from p in db.Person where  arr.Contains(p.FirstName) select p;
				Assert.AreEqual(0, q.Count());
			}
		}

		[Test]
		public void ContainsParameter1()
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
		public void ContainsParameter3()
		{
			var str = "o";

			using (var db = new TestDbManager())
			{
				var q =
					from d in db.Doctor
					join p in db.Person.Where(p => p.FirstName.Contains(str))
					on d.PersonID equals p.ID
					select p;

				Assert.AreEqual(1, q.ToList().First().ID);
			}
		}

		[Test]
		public void ContainsParameter4()
		{
			ForEachProvider(db =>
				AreEqual(
					from p in Person
					select new
					{
						p,
						Field1 = p.FirstName.Contains("Jo")
					} into p
					where p.Field1
					orderby p.Field1
					select p,
					from p in db.Person
					select new
					{
						p,
						Field1 = p.FirstName.Contains("Jo")
					} into p
					where p.Field1
					orderby p.Field1
					select p));
		}

		[Test]
		public void StartsWith1()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where p.FirstName.StartsWith("Jo") && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void StartsWith2()
		{
			ForEachProvider(
				new[] { ProviderName.DB2, ProviderName.Access },
				db => AreEqual(
					from p in    Person where "John123".StartsWith(p.FirstName) select p,
					from p in db.Person where "John123".StartsWith(p.FirstName) select p));
		}

		[Test]
		public void StartsWith3()
		{
			var str = "John123";

			ForEachProvider(
				new[] { ProviderName.DB2, ProviderName.Access },
				db => AreEqual(
					from p in    Person where str.StartsWith(p.FirstName) select p,
					from p in db.Person where str.StartsWith(p.FirstName) select p));
		}

		[Test]
		public void StartsWith4()
		{
			ForEachProvider(
				new[] { ProviderName.DB2, ProviderName.Access },
				db => AreEqual(
					from p1 in    Person
					from p2 in    Person
					where p1.ID == p2.ID && p1.FirstName.StartsWith(p2.FirstName)
					select p1,
					from p1 in db.Person
					from p2 in db.Person
					where p1.ID == p2.ID && 
						Sql.Like(p1.FirstName, p2.FirstName.Replace("%", "~%"), '~')
					select p1));
		}

		[Test]
		public void StartsWith5()
		{
			ForEachProvider(
				new[] { ProviderName.DB2, ProviderName.Access },
				db => AreEqual(
					from p1 in    Person
					from p2 in    Person
					where p1.ID == p2.ID && p1.FirstName.Replace("J", "%").StartsWith(p2.FirstName.Replace("J", "%"))
					select p1,
					from p1 in db.Person
					from p2 in db.Person
					where p1.ID == p2.ID && p1.FirstName.Replace("J", "%").StartsWith(p2.FirstName.Replace("J", "%"))
					select p1));
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
				var q = from p in db.Person where Sql.Like(p.FirstName, "%hn%") && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Like22()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where !Sql.Like(p.FirstName, @"%h~%n%", '~') && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void IndexOf11()
		{
			ForEachProvider(new[] { ProviderName.Firebird, ProviderName.Informix }, db => 
			{
				var q = from p in db.Person where p.FirstName.IndexOf("oh") == 1 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void IndexOf12()
		{
			ForEachProvider(new[] { ProviderName.Firebird, ProviderName.Informix }, db => 
			{
				var q = from p in db.Person where p.FirstName.IndexOf("") == 0 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void IndexOf2()
		{
			ForEachProvider(new[] { ProviderName.Firebird, ProviderName.Informix }, db => 
			{
				var q = from p in db.Person where p.LastName.IndexOf("e", 2) == 4 && p.ID == 2 select p;
				Assert.AreEqual(2, q.ToList().First().ID);
			});
		}

		[Test]
		public void IndexOf3([DataContexts(
			ProviderName.DB2, ProviderName.Firebird, ProviderName.Informix, ProviderName.SqlCe, ProviderName.Sybase, ProviderName.Access)] string context)
		{
			var s = "e";
			var n1 = 2;
			var n2 = 5;

			using (var db = GetDataContext(context))
			{
				var q = from p in db.Person where p.LastName.IndexOf(s, n1, n2) == 1 && p.ID == 2 select p;
				Assert.AreEqual(2, q.ToList().First().ID);
			}
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
				q = q.Where(p => p.FirstName.LastIndexOf("123", 5) == 8);
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void LastIndexOf3()
		{
			ForEachProvider(_lastIndexExcludeList, db => 
			{
				var q = from p in db.Person where p.ID == 1 select new { p.ID, FirstName = "123" + p.FirstName + "0123451234" };
				q = q.Where(p => p.FirstName.LastIndexOf("123", 5, 6) == 8);
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void CharIndex1()
		{
			ForEachProvider(new[] { ProviderName.Firebird, ProviderName.Informix }, db => 
			{
				var q = from p in db.Person where Sql.CharIndex("oh", p.FirstName) == 2 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void CharIndex2()
		{
			ForEachProvider(new[] { ProviderName.Firebird, ProviderName.Informix }, db => 
			{
				var q = from p in db.Person where Sql.CharIndex("p", p.LastName, 2) == 3 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Left()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where Sql.Left(p.FirstName, 2) == "Jo" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Right()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where Sql.Right(p.FirstName, 3) == "ohn" && p.ID == 1 select p;
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
				var q = from p in db.Person where Sql.Reverse(p.FirstName) == "nhoJ" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Stuff()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where Sql.Stuff(p.FirstName, 3, 1, "123") == "Jo123n" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Insert()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where p.FirstName.Insert(2, "123") == "Jo123hn" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Remove1()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where p.FirstName.Remove(2) == "Jo" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Remove2()
		{
			ForEachProvider(db => 
			{
				var q = from p in db.Person where p.FirstName.Remove(1, 2) == "Jn" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Space()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where p.FirstName + Sql.Space(p.ID + 1) + "123" == "John  123" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void PadRight()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where Sql.PadRight(p.FirstName, 6, ' ') + "123" == "John  123" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void PadRight1()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where p.FirstName.PadRight(6) + "123" == "John  123" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void PadRight2()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where p.FirstName.PadRight(6, '*') + "123" == "John**123" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void PadLeft()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where "123" + Sql.PadLeft(p.FirstName, 6, ' ') == "123  John" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void PadLeft1()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where "123" + p.FirstName.PadLeft(6) == "123  John" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void PadLeft2()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where "123" + p.FirstName.PadLeft(6, '*') == "123**John" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

        [Test]
		public void Replace()
		{
			ForEachProvider(new[] { ProviderName.Access }, db =>
			{
				var q = from p in db.Person where p.FirstName.Replace("hn", "lie") == "Jolie" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Trim()
		{
			ForEachProvider(db =>
			{
				var q = 
					from p in db.Person where p.ID == 1 select new { p.ID, Name = "  " + p.FirstName + " " } into pp
					where pp.Name.Trim() == "John" select pp;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void TrimLeft()
		{
			ForEachProvider(db =>
			{
				var q = 
					from p in db.Person where p.ID == 1 select new { p.ID, Name = "  " + p.FirstName + " " } into pp
					where pp.Name.TrimStart() == "John " select pp;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void TrimRight()
		{
			ForEachProvider(db =>
			{
				var q = 
					from p in db.Person where p.ID == 1 select new { p.ID, Name = "  " + p.FirstName + " " } into pp
					where pp.Name.TrimEnd() == "  John" select pp;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void ToLower()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where p.FirstName.ToLower() == "john" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void ToUpper()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where p.FirstName.ToUpper() == "JOHN" && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void CompareTo()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where p.FirstName.CompareTo("John") == 0 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void CompareToNotEqual1()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where p.FirstName.CompareTo("Jo") != 0 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void CompareToNotEqual2()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where 0 != p.FirstName.CompareTo("Jo") && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void CompareTo1()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where p.FirstName.CompareTo("Joh") > 0 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void CompareTo2()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where p.FirstName.CompareTo("Johnn") < 0 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void CompareTo21()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where p.FirstName.CompareTo("Johnn") <= 0 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void CompareTo22()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where 0 >= p.FirstName.CompareTo("Johnn") && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void CompareTo3()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where p.FirstName.CompareTo(55) > 0 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void CompareTo31()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where p.FirstName.CompareTo(55) >= 0 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void CompareTo32()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where 0 <= p.FirstName.CompareTo(55) && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void CompareOrdinal1()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where string.CompareOrdinal(p.FirstName, "Joh") > 0 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void CompareOrdinal2()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where string.CompareOrdinal(p.FirstName, 1, "Joh", 1, 2) == 0 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Compare1()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where string.Compare(p.FirstName, "Joh") > 0 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Compare2()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where string.Compare(p.FirstName, "joh", true) > 0 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Compare3()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where string.Compare(p.FirstName, 1, "Joh", 1, 2) == 0 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void Compare4()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where string.Compare(p.FirstName, 1, "Joh", 1, 2, true) == 0 && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void IsNullOrEmpty1()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where !string.IsNullOrEmpty(p.FirstName) && p.ID == 1 select p;
				Assert.AreEqual(1, q.ToList().First().ID);
			});
		}

		[Test]
		public void IsNullOrEmpty2()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where p.ID == 1 select string.IsNullOrEmpty(p.FirstName);
				Assert.AreEqual(false, q.ToList().First());
			});
		}

		//[Test]
		public void Test()
		{
			using (var db = new TestDbManager(ProviderName.Firebird))
			{
				var p = db
					.SetCommand(@"
						SELECT
							t1.ParentID,
							t1.Value1
						FROM
							Parent t1
								LEFT JOIN (
									SELECT
										t3.ParentID as ParentID1,
										Coalesce(t3.ParentID, 1) as c1
									FROM
										Child t3
								) t2 ON t1.ParentID = t2.ParentID1
						WHERE
							t2.c1 IS NULL")
					.ExecuteList<Parent>();

				var p1 = p.First();
				Assert.AreEqual(1, p1.ParentID);


				var da = new SqlQuery();
				var pr  = (Person)da.SelectByKey(typeof(Person), 1);

				Assert.AreEqual("Pupkin", pr.LastName);


				//Assert.AreEqual(1, p.ID);
			}
		}
	}
}
