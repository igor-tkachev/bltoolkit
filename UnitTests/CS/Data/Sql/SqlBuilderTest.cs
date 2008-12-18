using System;

using NUnit.Framework;

using BLToolkit.Data.Sql;

namespace Data.Sql
{
	[TestFixture]
	public class SqlBuilderTest
	{
		[Test]
		public void TableTest()
		{
			new Table()
			{
				Name   = "Table",
				Fields =
				{
					new Field { Name = "Field1" },
					new Field { Name = "Field1" }
				}
			};
		}
	}
}
