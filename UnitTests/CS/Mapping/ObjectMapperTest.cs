using System;
using System.Data;
using BLToolkit.EditableObjects;
using NUnit.Framework;

using BLToolkit.Mapping;

namespace Mapping
{
	[TestFixture]
	public class ObjectMapperTest
	{
		public class Object1
		{
			[MapField("col2")]
			public int Col1 { get; set; }
		}


		[Test]
		public void NameTest1()
		{
			var dt = new DataTable();

			dt.Columns.Add("col1", typeof(int));
			dt.Columns.Add("col2", typeof(int));
			dt.Rows.Add(1, 2);

			var o = Map.DataRowToObject<Object1>(dt.Rows[0]);

			Assert.AreEqual(2, o.Col1);
		}

		[Test]
		public void NameTest2()
		{
			var dt = new DataTable();

			dt.Columns.Add("col2", typeof(int));
			dt.Columns.Add("col1", typeof(int));
			dt.Rows.Add(2, 1);

			var o = Map.DataRowToObject<Object1>(dt.Rows[0]);

			Assert.AreEqual(2, o.Col1);
		}

		[Test]
		public void GetMemberMapperByNameAliasTest()
		{
			var om = Map.GetObjectMapper(typeof(Object1));

			Assert.IsNotNull(om["col2"]);
			Assert.IsNull   (om["Col1"]);
			Assert.IsNotNull(om["Col1", true]);
			Assert.IsNull   (om["col1", true]);
		}

		public interface ICheckOut
		{
			DateTime DateCheckOut { get; }
		}

		public class Test : ICheckOut
		{
			public int    ID    { get; set; }
			public string Value { get; set; }

			DateTime ICheckOut.DateCheckOut
			{
				get { return DateTime.Now; }
			}
		}

		[Test]
		public void MapObjectToObject()
		{
			var s1 = new Test { ID = 1, Value = "123" };
			var s2 = new Test();

			Map.ObjectToObject(s1, s2, null);
		}

		public abstract class BizEntityTest : EditableObject<BizEntityTest>
		{
			public abstract int                ID                 { get; set; }
			public abstract string             Name               { get; set; }
			public abstract BizEntityTestInner BizEntityTestInner { get; set; }
		}

		public abstract class BizEntityTestInner : EditableObject<BizEntityTestInner>
		{
			public abstract int    ID   { get; set; }
			public abstract string Name { get; set; }
		}

		[Test]
		public void DestabilizeTest()
		{
			var bet = BizEntityTest.CreateInstance();

			bet.ID                      = 1;
			bet.Name                    = "BizEntityTest";
			bet.BizEntityTestInner      = BizEntityTestInner.CreateInstance();
			bet.BizEntityTestInner.ID   = 111;
			bet.BizEntityTestInner.Name = "BizEntityTestInner";

			var mapper = Map.GetObjectMapper<BizEntityTest, BizEntityTest>(true);

			var bet2 = mapper(bet);
		}
	}
}
