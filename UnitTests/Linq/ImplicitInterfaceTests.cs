using System;
using System.Linq;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class ImplicitInterfaceTests : TestBase
	{
		interface IDate
		{
			DateTime? Date { get; }
		}

		[TableName("LinqDataTypes")]
		class TestTable : IDate
		{
			[MapField("GuidValue")]
			Guid? GuidValue { get; set; }

			[MapField("BoolValue")]
			public bool? Bit { get; set; }

			private DateTime? _date;

			[MapField("DateTimeValue", Storage = "_date")]
			DateTime? IDate.Date
			{
				get
				{
					return _date;
				}
			}
		}

		static IQueryable<T> SelectNoDate<T>(IQueryable<T> items) where T : IDate
		{
			return items.Where(i => i.Date == null);
		}

		[Test]
		public void TestInterface()
		{
			using (var db = new TestDbManager())
			{
				var result = SelectNoDate(db.GetTable<TestTable>()).ToList();
			}
		}
	}
}
