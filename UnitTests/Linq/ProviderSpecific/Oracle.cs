using System;
using System.Linq;

using BLToolkit.Data.Linq;

using NUnit.Framework;

using UnitTests.Linq.Interface.Model;

namespace Data.Linq.ProviderSpecific
{
	[TestFixture]
	public class Oracle : TestBase
	{
		[Test]
		public void SequenceInsert()
		{
			using (var db = new TestDbManager("Oracle"))
			{
				db.GetTable<OracleSpecific.SequenceTest>().Where(_ => _.Value == "SeqValue").Delete();
				db.Insert(new OracleSpecific.SequenceTest { Value = "SeqValue" });

				var id = db.GetTable<OracleSpecific.SequenceTest>().Single(_ => _.Value == "SeqValue").ID;

				db.GetTable<OracleSpecific.SequenceTest>().Where(_ => _.ID == id).Delete();

				Assert.AreEqual(0, db.GetTable<OracleSpecific.SequenceTest>().Count(_ => _.Value == "SeqValue"));
			}
		}

		[Test]
		public void SequenceInsertWithIdentity()
		{
			using (var db = new TestDbManager("Oracle"))
			{
				db.GetTable<OracleSpecific.SequenceTest>().Where(_ => _.Value == "SeqValue").Delete();

				var id1 = Convert.ToInt32(db.InsertWithIdentity(new OracleSpecific.SequenceTest { Value = "SeqValue" }));
				var id2 = db.GetTable<OracleSpecific.SequenceTest>().Single(_ => _.Value == "SeqValue").ID;

				Assert.AreEqual(id1, id2);

				db.GetTable<OracleSpecific.SequenceTest>().Where(_ => _.ID == id1).Delete();

				Assert.AreEqual(0, db.GetTable<OracleSpecific.SequenceTest>().Count(_ => _.Value == "SeqValue"));
			}
		}
	}
}
