using System;
using System.Linq;

using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class ExplicitInterfaceTests : TestBase
	{
		interface IDate
		{
			DateTime? Date { get; }
		}

		interface IDate2
		{
			DateTime? Date { get; set; }
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

		[TableName("LinqDataTypes")]
		public class TestTable2 : IDate
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

		[TableName("LinqDataTypes")]
		public class TestTable3 : IDate2
		{
			[MapField("GuidValue")]
			Guid? GuidValue { get; set; }

			[MapField("BoolValue")]
			public bool? Bit { get; set; }

			[MapField("DateTimeValue")]
			DateTime? IDate2.Date
			{
				get;
				set;
			}
		}

		static IQueryable<T> SelectNoDate<T>(IQueryable<T> items) where T : IDate
		{
			return items.Where(i => i.Date == null);
		}

		static IQueryable<T> SelectNoDate2<T>(IQueryable<T> items) where T : IDate2
		{
			return items.Where(i => i.Date == null);
		}

		[Test]
		public void ExplicitInterface1()
		{
			using (var db = new TestDbManager())
			{
				var result = SelectNoDate(db.GetTable<TestTable>()).ToList();
			}
		}

		[Test]
		public void ExplicitInterface2()
		{
			using (var db = new TestDbManager())
			{
				var result = SelectNoDate(db.GetTable<TestTable2>()).ToList();
			}
		}

		[Test]
		public void ExplicitInterface3()
		{
			using (var db = new TestDbManager())
			{
				var result = SelectNoDate2(db.GetTable<TestTable3>()).ToList();
			}
		}
	}
}
