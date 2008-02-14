using System;
using BLToolkit.Data.DataProvider;
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
				object id;
#if ORACLE
				id = db
					.SetCommand("INSERT INTO BinaryData (Data) VALUES (:pData) RETURNING BinaryDataID INTO :pID",
						db.Parameter("pData", new byte[] { 1, 2, 3, 4, 5}),
						db.OutputParameter("pID", System.Data.DbType.Int32)
						)
					.ExecuteScalar(ScalarSourceType.OutputParameter, "ID");
#elif MSSQL
				id = db
					.SetCommand("INSERT INTO BinaryData (Data) VALUES (@Data)\nSELECT Cast(SCOPE_IDENTITY() as int)",
						db.Parameter("@Data", new byte[] { 1, 2, 3, 4, 5}))
					.ExecuteScalar();
#elif FIREBIRD
				db
					.SetCommand("INSERT INTO BinaryData (Data) VALUES (@Data)",
						db.Parameter("@Data", new byte[] { 1, 2, 3, 4, 5}))
					.ExecuteNonQuery();

				id = db
					.SetCommand("SELECT GEN_ID(PersonID, 0) FROM dual")
					.ExecuteScalar();
#elif ACCESS
				db
					.SetCommand("INSERT INTO BinaryData (Data) VALUES (@Data)",
						db.Parameter("@Data", new byte[] { 1, 2, 3, 4, 5}))
					.ExecuteNonQuery();

				id = db
					.SetCommand("SELECT @@IDENTITY")
					.ExecuteScalar();
#else
				Assert.Fail("Unknown DB type.");
#endif

				BinaryData bd = (BinaryData)db
					.SetCommand(
						"SELECT * FROM BinaryData WHERE BinaryDataID = " + db.DataProvider.Convert("id", ConvertType.NameToQueryParameter),
					db.Parameter("id", id))
					.ExecuteObject(typeof(BinaryData));

				Assert.IsNotNull(bd);
				Assert.AreEqual(5, bd.Data. Length);
#if FIREBIRD
				// Stamps are integers in Firebird.
				//
				Assert.AreEqual(4, bd.Stamp.Length);
#elif ACCESS
				// Not supported in MS Access
				//
				Assert.IsNull(bd.Stamp);
#else
				Assert.AreEqual(8, bd.Stamp.Length);
#endif

				db
#if FIREBIRD || ACCESS
					.SetCommand("DELETE FROM BinaryData")
#else
					.SetCommand("TRUNCATE TABLE BinaryData")
#endif
					.ExecuteNonQuery();
			}
		}
	}
}
