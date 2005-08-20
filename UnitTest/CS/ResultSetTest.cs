using System;
using System.Data;
using System.Collections;

using NUnit.Framework;

using Rsdn.Framework.Data;
using Rsdn.Framework.Data.Mapping;

namespace CS
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
			using (DbManager   db = new DbManager())
			using (IDataReader rd = db
				.SetCommand(SqlResultSet)
				.ExecuteReader())
			{
				MapResultSet[] sets = new MapResultSet[2];

				sets[0] = new MapResultSet(typeof(Master));
				sets[1] = new MapResultSet(typeof(Slave));

				sets[0].AddRelation("MasterID", sets[1], "MasterID", "Slaves");

				IList[] lists = Map.ToListInternal(rd, sets);

				Assert.AreEqual(2, lists.Length);
				Assert.AreEqual(7, ((Slave)(((Master)lists[0][0]).Slaves[1])).ID);
			}
		}

		[Test]
		[ExpectedException(typeof(RsdnMapException))]
		public void TestFailResultSet1()
		{
			using (DbManager   db = new DbManager())
			using (IDataReader rd = db
					   .SetCommand(SqlResultSet)
					   .ExecuteReader())
			{
				MapResultSet[] sets = new MapResultSet[2];

				sets[0] = new MapResultSet(typeof(Master));
				sets[1] = new MapResultSet(typeof(Slave));

				sets[0].AddRelation("ID", sets[1], "MasterID", "Slaves");

				IList[] lists = Map.ToListInternal(rd, sets);
			}
		}

		[Test]
		[ExpectedException(typeof(RsdnMapException))]
		public void TestFailResultSet2()
		{
			using (DbManager   db = new DbManager())
			using (IDataReader rd = db
					   .SetCommand(SqlResultSet)
					   .ExecuteReader())
			{
				MapResultSet[] sets = new MapResultSet[2];

				sets[0] = new MapResultSet(typeof(Master));
				sets[1] = new MapResultSet(typeof(Slave));

				sets[0].AddRelation("MasterID", sets[1], "Master", "Slaves");

				IList[] lists = Map.ToListInternal(rd, sets);
			}
		}

		[Test]
		[ExpectedException(typeof(RsdnMapException))]
		public void TestFailResultSet3()
		{
			using (DbManager   db = new DbManager())
			using (IDataReader rd = db
					   .SetCommand(SqlResultSet)
					   .ExecuteReader())
			{
				MapResultSet[] sets = new MapResultSet[2];

				sets[0] = new MapResultSet(typeof(Master));
				sets[1] = new MapResultSet(typeof(Slave));

				sets[0].AddRelation("MasterID", sets[1], "MasterID", "Slave");

				IList[] lists = Map.ToListInternal(rd, sets);
			}
		}
	}
}
