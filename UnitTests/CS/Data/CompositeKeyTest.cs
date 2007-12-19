using System;
using System.Data;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Mapping;

namespace Data
{
	[TestFixture]
	public class CompositeKeyTest
	{
		public class CompositeKey
		{
			public int Id;
			public int Revision;
		}

		[MapField("Id",       "Key.Id")]
		[MapField("Revision", "Key.Revision")]
		public class Entity
		{
			public CompositeKey Key = new CompositeKey();
			public string       Name;
		}

		[Test]
		public void CompositeObjectTest()
		{
			using (DbManager db = new DbManager())
			{
				IDbDataParameter[] prms = db.CreateParameters(new Entity());
				Assert.AreEqual(3, prms.Length);
			}
		}
	}
}
