using System;
using System.Data;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Mapping;

namespace Data
{
	[TestFixture]
	public class NullableParameter
	{
		public class NullableInt
		{
			[NullValue(-1)]
			public int I;
			public int II;
		}

		[Test]
		public void Test()
		{
			const int testValue = 0;

			NullableInt ni = new NullableInt();

			ni.I  = testValue;
			ni.II = testValue;
			
			using (DbManager db = new DbManager())
			{
				IDbDataParameter[] ps = db.CreateParameters(ni);

				foreach (IDbDataParameter p in ps)
				{
					switch (p.ParameterName)
					{
						case "@I":
							Assert.AreEqual(testValue, p.Value);
							break;
						case "@II":
							Assert.AreEqual(testValue, p.Value);
							break;
					}
				}
			}
		}
	}
}
