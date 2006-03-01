using System;

using NUnit.Framework;

using BLToolkit.Data;

namespace Data
{
	[TestFixture]
	public class BinaryTest
	{
		public class BinaryData
		{
			public int    BinaryDataID;
			public byte[] Stamp;
			public byte[] Data;
		}

		[Test]
		public void Test()
		{
			using (DbManager db = new DbManager())
			{
				object id = db
					.SetCommand(@"
						INSERT INTO BinaryData (Data) VALUES (@Data)
						SELECT Cast(SCOPE_IDENTITY() as int)",
						db.Parameter("@Data", new byte[] { 1, 2, 3, 4, 5}))
					.ExecuteScalar();

				BinaryData bd = (BinaryData)db
					.SetCommand(
						"SELECT * FROM BinaryData WHERE BinaryDataID = @id",
						db.Parameter("@id", id))
					.ExecuteObject(typeof(BinaryData));

				Assert.AreEqual(5, bd.Data. Length);
				Assert.AreEqual(8, bd.Stamp.Length);

				db
					.SetCommand("TRUNCATE TABLE BinaryData")
					.ExecuteNonQuery();
			}
		}
	}
}
