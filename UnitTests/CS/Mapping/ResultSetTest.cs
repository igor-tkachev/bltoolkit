using System;
using System.Collections;

using BLToolkit.Data;
using BLToolkit.Mapping;

using NUnit.Framework;

namespace Mapping
{
	[TestFixture]
	public class ResultSetTest
	{
		public class Master
		{
			[MapField("MasterID")]
			public int ID;

			public ArrayList Slaves = new ArrayList();
		}

		public class Slave
		{
			[MapField("SlaveID")]
			public int ID;

			public int MasterID;
		}

		const string SqlResultSet = @"
			SELECT       1 as MasterID
			UNION SELECT 2 as MasterID

			SELECT       4 SlaveID, 1 as MasterID
			UNION SELECT 5 SlaveID, 2 as MasterID
			UNION SELECT 6 SlaveID, 2 as MasterID
			UNION SELECT 7 SlaveID, 1 as MasterID";

		[Test]
		public void TestResultSet()
		{
			MapResultSet[] sets = new MapResultSet[2];

			sets[0] = new MapResultSet(typeof(Master));
			sets[1] = new MapResultSet(typeof(Slave));

			sets[0].AddRelation(sets[1], "MasterID", "MasterID", "Slaves");

			using (DbManager db = new DbManager())
			{
				db
#if ORACLE
					.SetSpCommand("ResultSetTest")
#else
					.SetCommand(SqlResultSet)
#endif
					.ExecuteResultSet(sets);
			}

			Assert.AreEqual(7, ((Slave)(((Master)sets[0].List[0]).Slaves[1])).ID);
		}

		[Test]
		[ExpectedException(typeof(MappingException))]
		public void TestFailResultSet1()
		{
			MapResultSet[] sets = new MapResultSet[2];

			sets[0] = new MapResultSet(typeof(Master));
			sets[1] = new MapResultSet(typeof(Slave));

			sets[0].AddRelation(sets[1], "MasterID", "ID", "Slaves");

			using (DbManager db = new DbManager())
			{
				db
#if ORACLE
					.SetSpCommand("ResultSetTest")
#else
					.SetCommand(SqlResultSet)
#endif
					.ExecuteResultSet(sets);
			}
		}

		[Test]
		[ExpectedException(typeof(MappingException))]
		public void TestFailResultSet2()
		{
			MapResultSet[] sets = new MapResultSet[2];

			sets[0] = new MapResultSet(typeof(Master));
			sets[1] = new MapResultSet(typeof(Slave));

			sets[0].AddRelation(sets[1], "Master", "MasterID", "Slaves");

			using (DbManager db = new DbManager())
			{
				db
#if ORACLE
					.SetSpCommand("ResultSetTest")
#else
					.SetCommand(SqlResultSet)
#endif
					.ExecuteResultSet(sets);
			}
		}

		[Test]
		[ExpectedException(typeof(MappingException))]
		public void TestFailResultSet3()
		{
			MapResultSet[] sets = new MapResultSet[2];

			sets[0] = new MapResultSet(typeof(Master));
			sets[1] = new MapResultSet(typeof(Slave));

			sets[0].AddRelation(sets[1], "MasterID", "MasterID", "Slave");

			using (DbManager db = new DbManager())
			{
				db
#if ORACLE
					.SetSpCommand("ResultSetTest")
#else
					.SetCommand(SqlResultSet)
#endif
					.ExecuteResultSet(sets);
			}
		}

		[Test]
		public void TestNextResult()
		{
			using (DbManager db = new DbManager())
			{
				MapResultSet[] sets = db
#if ORACLE
					.SetSpCommand("ResultSetTest")
#else
					.SetCommand(SqlResultSet)
#endif
					.ExecuteResultSet(typeof(Master),
						new MapNextResult(typeof(Slave), "MasterID", "MasterID", "Slaves"));

				Assert.AreEqual(7, ((Slave)(((Master)sets[0].List[0]).Slaves[1])).ID);
			}
		}
	}
}
