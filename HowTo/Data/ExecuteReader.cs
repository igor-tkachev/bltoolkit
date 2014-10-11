using System;
using System.Data;

using NUnit.Framework;

using BLToolkit.Data;

namespace HowTo.Data
{
	[TestFixture]
	public class ExecuteReader
	{
		[Test]
		public void Test()
		{
			using (DbManager db = new DbManager())
			{
				db.SetCommand("SELECT * FROM Person");

				using (IDataReader rd = db./*[a]*/ExecuteReader/*[/a]*/())
				{
					while (rd.Read())
					{
						// ...
					}
				}
			}
		}
	}
}
