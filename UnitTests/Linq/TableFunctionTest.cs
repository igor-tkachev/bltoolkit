using System;
using System.Linq;

using NUnit.Framework;

using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class TableFunctionTest : TestBase
	{
		[Test]
		public void Test1()
		{
			using (var db = new TestDbManager())
			{
				var q =
					from p in new Model.Functions(db).GetParentByID(1)
					select p;

				q.ToList();
			}
		}
	}
}
