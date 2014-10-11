using System;
using System.Collections.Generic;
using System.Data;

using NUnit.Framework;

using BLToolkit.Data;

namespace Data
{
	[TestFixture]
	public class ExecuteForEach
	{
		public class TypeWrapper<T>
		{
			public TypeWrapper(T value)
			{
				_value = value;
			}

			private T _value;
			public  T  Value
			{
				get { return _value;  }
				set { _value = value; }
			}
		}

		[Test]
		public void TestFixedTypes()
		{
			RunTest(new int[] { 1, 2, 3, 4 });
			RunTest(new bool[] { true, false });
			RunTest(new DateTime[] { DateTime.Now, DateTime.Today });
			RunTest(new double[] { 1, 2, 3, 4 });
		}

		[Test]
		public void TestVarTypes()
		{
			RunTest(new byte[][] { new byte[] { 1, 2 }, new byte[] { 3, 4 } });
			RunTest(new char[][] { new char[] { '1', '2' }, new char[] { '3', '4' } });
		}

		[Test]
		public void TestDecimal()
		{
			RunTest(new decimal[] { 1, 2, 3, 4 });
		}

		[Test]
		public void TestString()
		{
			RunTest(new string[] { "1", "2", "3", "4" });
		}

		public class Item
		{
			public int    Length;
			public string Name;

			public Item(string s)
			{
				Length = s.Length;
				Name = s;
			}

			public Item()
			{
				Name = string.Empty;
			}
		}

		[Test]
		public void TestStringWithDiffLength()
		{
			List<Item> test = new List<Item>();
			test.Add(new Item("aaaa"));
			test.Add(new Item("bb"));
			test.Add(new Item("ccccccc"));

			using (DbManager db = new DbManager())
			{
				db
					.BeginTransaction()
					.SetCommand(
					@"if exists (select 1 from  sysobjects where  id = object_id('_tmp'))
							drop table _tmp
					create table _tmp ( Length int, name varchar(50) )")
					.ExecuteNonQuery();


				db
					.SetCommand(CommandType.Text,
						"insert into _tmp ( [Length], [name] ) VALUES ( @Length, @name )")
					.ExecuteForEach<Item>(test);

				List<Item> actial = db
					.SetCommand("select [Length], [name] from _tmp order by [Name]")
					.ExecuteList<Item>();

				Assert.AreEqual(test.Count, actial.Count);

				for (int i = 0; i < test.Count; ++i)
				{
					Assert.AreEqual(test[i].Length, actial[i].Length);
					Assert.AreEqual(test[i].Name,   actial[i].Name);
				}
			}
		}

		private static void RunTest<T>(T[] args)
		{
			List<TypeWrapper<T>> col = new List<TypeWrapper<T>>();

			col.AddRange(Array.ConvertAll<T, TypeWrapper<T>>(args, delegate(T val) { return new TypeWrapper<T>(val); }));

			using (DbManager db = new DbManager())
			{
				db
					.SetCommand(@"SELECT @Value as 'value'")
					.ExecuteForEach<TypeWrapper<T>>(col);
			}
		}
	}
}