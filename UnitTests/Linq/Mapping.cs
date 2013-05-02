using System;
using System.Linq;

using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.Reflection;
using NUnit.Framework;

using Convert = System.Convert;

#pragma warning disable 0649

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class Mapping : TestBase
	{
		[Test]
		public void Enum1()
		{
			var expected = from p in Person where new[] { Gender.Male }.Contains(p.Gender) select p;
			ForEachProvider(db => AreEqual(expected, from p in db.Person where new[] { Gender.Male }.Contains(p.Gender) select p));
		}

		[Test]
		public void Enum2()
		{
			ForEachProvider(db => AreEqual(
				from p in    Person where p.Gender == Gender.Male select p,
				from p in db.Person where p.Gender == Gender.Male select p));
		}

		[Test]
		public void Enum21()
		{
			var gender = Gender.Male;

			ForEachProvider(db => AreEqual(
				from p in    Person where p.Gender == gender select p,
				from p in db.Person where p.Gender == gender select p));
		}

		[Test]
		public void Enum3()
		{
			var fm = Gender.Female;

			var expected = from p in Person where p.Gender != fm select p;
			ForEachProvider(db => AreEqual(expected, from p in db.Person where p.Gender != fm select p));
		}

		[Test]
		public void Enum4()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent4 where p.Value1 == TypeValue.Value1 select p,
				from p in db.Parent4 where p.Value1 == TypeValue.Value1 select p));
		}

		[Test]
		public void Enum5()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent4 where p.Value1 == TypeValue.Value3 select p,
				from p in db.Parent4 where p.Value1 == TypeValue.Value3 select p));
		}

		[Test]
		public void Enum6()
		{
			ForEachProvider(db => AreEqual(
				from p in Parent4
				join c in Child on p.ParentID equals c.ParentID
				where p.Value1 == TypeValue.Value1 select p,
				from p in db.Parent4
				join c in db.Child on p.ParentID equals c.ParentID
				where p.Value1 == TypeValue.Value1 select p));
		}

		[Test]
		public void Enum7()
		{
			var v1 = TypeValue.Value1;

			ForEachProvider(db => db.Parent4.Update(p => p.Value1 == v1, p => new Parent4 { Value1 = v1 }));
		}

		enum TestValue
		{
			Value1 = 1,
		}

		[TableName("Parent")]
		class TestParent
		{
			public int       ParentID;
			public TestValue Value1;
		}

		[Test]
		public void Enum81()
		{
			ForEachProvider(db => db.GetTable<TestParent>().Where(p => p.Value1 == TestValue.Value1).ToList());
		}

		[Test]
		public void Enum82()
		{
			var testValue = TestValue.Value1;
			ForEachProvider(db => db.GetTable<TestParent>().Where(p => p.Value1 == testValue).ToList());
		}

		public enum Gender9
		{
			[MapValue('M')] Male,
			[MapValue('F')] Female,
			[MapValue('U')] Unknown,
			[MapValue('O')] Other,
		}

		[TableName("Person")]
		public class Person9
		{
			public int     PersonID;
			public string  FirstName;
			public string  LastName;
			public string  MiddleName;
			public Gender9 Gender;
		}

		[Test]
		public void Enum9()
		{
			ForEachProvider(db =>
				db.GetTable<Person9>().Where(p => p.PersonID == 1 && p.Gender == Gender9.Male).ToList());
		}

		[Test]
		public void EditableObject()
		{
			ForEachProvider(db =>
			{
				var e = (from p in db.GetTable<EditableParent>() where p.ParentID == 1 select p).First();
				Assert.AreEqual(1, e.ParentID);
				Assert.AreEqual(1, e.Value1);
			});
		}

		[TableName("Parent")]
		[MapField("Value1", "Value.Value1")]
		public class ParentObject
		{
			public int   ParentID;
			public Inner Value = new Inner();

			public class Inner
			{
				public int? Value1;
			}
		}

		[Test]
		public void Inner1()
		{
			ForEachProvider(db =>
			{
				var e = db.GetTable<ParentObject>().First(p => p.ParentID == 1);
				Assert.AreEqual(1, e.ParentID);
				Assert.AreEqual(1, e.Value.Value1);
			});
		}

		[Test]
		public void Inner2()
		{
			ForEachProvider(db =>
			{
				var e = db.GetTable<ParentObject>().First(p => p.ParentID == 1 && p.Value.Value1 == 1);
				Assert.AreEqual(1, e.ParentID);
				Assert.AreEqual(1, e.Value.Value1);
			});
		}

		[TableName("Child")]
		public class ChildObject
		{
			public int ParentID;
			public int ChildID;

			[Association(ThisKey="ParentID", OtherKey="ParentID")]
			public ParentObject Parent;
		}

		[Test]
		public void Inner3()
		{
			ForEachProvider(db =>
			{
				var e = db.GetTable<ChildObject>().First(c => c.Parent.Value.Value1 == 1);
				Assert.AreEqual(1, e.ParentID);
			});
		}

		[TableName("Parent")]
		public class ParentObject2
		{
			class IntToDateMemberMapper : MemberMapper
			{
				public override void SetValue(object o, object value)
				{
					((ParentObject2)o).Value1 = new DateTime(2010, 1, Convert.ToInt32(value));
				}
			}

			public int      ParentID;
			[MemberMapper(typeof(IntToDateMemberMapper))]
			public DateTime Value1;
		}

		[Test]
		public void MemberMapperTest1()
		{
			ForEachProvider(db =>
			{
				var q =
					from p in db.GetTable<ParentObject2>()
					where p.ParentID == 1
					select p;

				Assert.AreEqual(new DateTime(2010, 1, 1), q.First().Value1);
			});
		}

		//[Test]
		public void MemberMapperTest2()
		{
			ForEachProvider(db =>
			{
				var q =
					from p in db.GetTable<ParentObject2>()
					where p.ParentID == 1
					select p.Value1;

				Assert.AreEqual(new DateTime(2010, 1, 1), q.First());
			});
		}

		struct MyInt
		{
			public int MyValue;
		}

		[TableName("Parent")]
		class MyParent
		{
			public MyInt ParentID;
			public int?  Value1;
		}

		class MyMappingSchema : MappingSchema
		{
			public override object ConvertChangeType(object value, Type conversionType, bool isNullable)
			{
				if (conversionType == typeof(MyInt))
					return new MyInt { MyValue = Convert.ToInt32(value) };

				if (value is MyInt)
					value = ((MyInt)value).MyValue;

				return base.ConvertChangeType(value, conversionType, isNullable);
			}

			public override object ConvertParameterValue(object value, Type systemType)
			{
				return value is MyInt ? ((MyInt)value).MyValue : value;
			}
		}

		static readonly MyMappingSchema _myMappingSchema = new MyMappingSchema();

		[Test]
		public void MyType1()
		{
			using (var db = new TestDbManager { MappingSchema = _myMappingSchema })
			{
				var list = db.GetTable<MyParent>().ToList();
			}
		}

		[Test]
		public void MyType2()
		{
			using (var db = new TestDbManager { MappingSchema = _myMappingSchema })
			{
				var list = db.GetTable<MyParent>()
					.Select(t => new MyParent { ParentID = t.ParentID, Value1 = t.Value1 })
					.ToList();
			}
		}

		[Test]
		public void MyType3()
		{
			using (var db = new TestDbManager { MappingSchema = _myMappingSchema })
			{
				db.BeginTransaction();
				db.Insert(new MyParent { ParentID = new MyInt { MyValue = 1001 }, Value1 = 1001 });
				db.Parent.Delete(p => p.ParentID >= 1000);
			}
		}

		[TableName("Parent")]
		class MyParent1
		{
			public int  ParentID;
			public int? Value1;

			[MapIgnore]
			public string Value2 { get { return "1"; } }

			public int GetValue() { return 2;}
		}

		[Test]
		public void MapIgnore1()
		{
			ForEachProvider(db => AreEqual(
				              Parent    .Select(p => new { p.ParentID, Value2 = "1" }),
				db.GetTable<MyParent1>().Select(p => new { p.ParentID, p.Value2 })));
		}

		[Test]
		public void MapIgnore2()
		{
			ForEachProvider(db => AreEqual(
				              Parent    .Select(p => new { p.ParentID, Length = 1      }),
				db.GetTable<MyParent1>().Select(p => new { p.ParentID, p.Value2.Length })));
		}

		[Test]
		public void MapIgnore3()
		{
			ForEachProvider(db => AreEqual(
				              Parent    .Select(p => new { p.ParentID, Value = 2            }),
				db.GetTable<MyParent1>().Select(p => new { p.ParentID, Value = p.GetValue() })));
		}

		[TableName("Parent")]
		public abstract class AbsParent : EditableObject
		{
			public abstract int  ParentID { get; set; }
			public abstract int? Value1   { get; set; }
		}

		[TableName("Child")]
		public abstract class AbsChild : EditableObject
		{
			public abstract int ParentID { get; set; }
			public abstract int ChildID  { get; set; }

			[Association(ThisKey = "ParentID", OtherKey = "ParentID", CanBeNull = false)]
			public AbsParent Parent;
		}

		[Test]
		public void MapAbstract()
		{
			using (var db = new TestDbManager())
			{
				var q = from a in db.GetTable<AbsChild>()
				select new { a.ChildID, a.Parent.Value1 };

				var ql = q.ToList();
			}
		}

		public class     Entity    { public int Id { get; set; } }
		public interface IDocument { int Id { get; set; } }
		public class     Document : Entity, IDocument { }

		[Test]
		public void TestMethod()
		{
			using (var db = new TestDbManager())
			{
				IQueryable<IDocument> query = db.GetTable<Document>();
				var idsQuery = query.Select(s => s.Id);
				var str = idsQuery.ToString(); // Exception
				Assert.IsNotNull(str);
			}
		}

		[TableName("Parent")]
		public abstract class ParentX
		{
			[MapField("ParentID")]
			public abstract int  ParentID { get; set; }
			[MapField("Value1")]
			public abstract int? Value1 { get; set; }
		}

		[TableName("Child")]
		[MapField("ParentID", "Parent.ParentID")]
		public abstract class ChildX
		{
			[MapField("ChildID")]
			public abstract int     ChildID { get; set; }
			public abstract ParentX Parent  { get; set; }
		}

		[Test]
		public void Test4([DataContexts] string contexts)
		{
			using (var db = GetDataContext(contexts))
			{
				db.Child. Delete(p => p.ParentID == 1001);
				db.Parent.Delete(p => p.ParentID == 1001);

				try
				{
					var child  = TypeAccessor.CreateInstance<ChildX>();
					var parent = TypeAccessor.CreateInstance<ParentX>();

					parent.ParentID = 1001;
					parent.Value1   = 1;

					db.Insert(parent);

					child.ChildID = 1001;
					child.Parent  = parent;

					db.Insert(child);
				}
				finally
				{
					db.Child. Delete(p => p.ParentID == 1001);
					db.Parent.Delete(p => p.ParentID == 1001);
				}
			}
		}
	}
}
