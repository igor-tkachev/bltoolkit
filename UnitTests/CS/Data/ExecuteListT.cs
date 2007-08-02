using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Data;

namespace Data
{
	[TestFixture]
	public class ExecuteListT
	{
		public class SimpleObject
		{
			private int _key;
			public  int  Key
			{
				get { return _key;  }
				set { _key = value; }
			}

			private string _value;
			public  string  Value
			{
				get { return _value; }
				set { _value = value; }
			}
		}

		[Test]
		public void Test()
		{
			using (DbManager db = new DbManager())
			{
				List<SimpleObject> list = new List<SimpleObject>();

				db
#if MSSQL
					.SetCommand(@"
						SELECT 0 as [Key], 'value0' as Value UNION
						SELECT 1 as [Key], 'value1' as Value UNION
						SELECT 2 as [Key], 'value2' as Value")

#else // ORACLE || FIREBIRD || ACCESS

					.SetCommand(@"
						SELECT 0 as ""Key"", 'value0' as ""Value"" FROM Dual UNION
						SELECT 1 as ""Key"", 'value1' as ""Value"" FROM Dual UNION
						SELECT 2 as ""Key"", 'value2' as ""Value"" FROM Dual")
#endif
					.ExecuteList<SimpleObject>(list);

				Assert.IsTrue(list.Count > 0);
			}
		}
	}
}
