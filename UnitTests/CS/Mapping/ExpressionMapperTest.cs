using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using NUnit.Framework;

using BLToolkit.Common;
using BLToolkit.Mapping;

namespace Mapping
{
	[TestFixture]
	public class ExpressionMapperTest
	{
		[Test]
		public void MapIntToString()
		{
			var mapper = Map.GetObjectMapper<int,string>();
			var dest   = mapper(42);

			Assert.AreEqual("42", dest);
		}

		[Test]
		public void MapStringToInt()
		{
			var mapper = Map.GetObjectMapper<string,int>();
			var dest   = mapper("42");

			Assert.AreEqual(42, dest);
		}

		[Test]
		public void MapGenderToString()
		{
			var mapper = Map.GetObjectMapper<Gender,string>();
			var dest   = mapper(Gender.Male);

			Assert.AreEqual("M", dest);
		}

		[Test]
		public void MapStringToGender()
		{
			var mapper = Map.GetObjectMapper<string,Gender>();
			var dest   = mapper("M");

			Assert.AreEqual(Gender.Male, dest);
		}

		public enum Gender
		{
			[MapValue("F")] Female,
			[MapValue("M")] Male,
			[MapValue("U")] Unknown,
			[MapValue("O")] Other
		}

		public enum Enum1
		{
			Value1 = 10,
			Value2,
			Value3,
			Value4,
		}

		public enum Enum2
		{
			Value1,
			Value2 = 10,
			Value3,
			Value4,
		}

		public class Dest
		{
			                     public int    Field1;
			[MapField("Field2")] public float  Field3;
			                     public int    Field4;
			                     public int?   Field6;
			                     public int    Field7;
			                     public int    Field8;
			                     public int    Field9;
			                     public string Field10;
			                     public int    Field11;
			[NullValue(12)]      public int    Field12;
			[NullValue(13)]      public int    Field13;
			[NullValue(14)]      public int    Field14;
			                     public Gender Field15;
			                     public string Field16;
			                     public Enum2  Field17;
		}

		public class Source
		{
			                     public int      Field1  = 1;
			                     public int      Field2  = 2;
			[MapField("Field4")] public float    Field5  = 5;
			                     public int      Field6  = 6;
			                     public int?     Field7  = 7;
			                     public int?     Field8;
			                     public decimal? Field9  = 9m;
			                     public int      Field10 = 10;
			                     public string   Field11 = "11";
			                     public string   Field12 { get; set; }
			                     public int?     Field13;
			                     public decimal? Field14;
			                     public string   Field15 = "F";
			                     public Gender   Field16 = Gender.Male;
			                     public Enum1    Field17 = Enum1.Value1;
		}

		void Clone(Expression<Func<Source,Dest>> mapper)
		{
		/*
		 Expression.Lambda<Func<Source, Dest>>(
			Expression.MemberInit(
				Expression.New((ConstructorInfo) methodof(Dest..ctor), new Expression[0]),
				new MemberBinding[]
				{
					Expression.Bind(
						fieldof(Dest.Field1),
						Expression.Field(CS$0$0000 = Expression.Parameter(typeof(Source), "s"), fieldof(Source.Field1))),
					Expression.Bind(
						fieldof(Dest.Field2),
						Expression.Convert(Expression.Field(CS$0$0000, fieldof(Source.Field2)), typeof(float)))
					}), new ParameterExpression[] { CS$0$0000 });
		*/
		}

		[Test]
		public void MapObjects()
		{
			var c = Convert<float,int>.From;

			//Clone(s => new Dest { Field1 = s.Field1, Field10 = s.Field10.ToString() });

			//Map.ObjectToObject(new Source(), typeof(Dest));

			var mapper = Map.GetObjectMapper<Source,Dest>();
			var src    = new Source();
			var dest   = mapper(src);

			Assert.AreEqual(1,                      dest.Field1);
			Assert.AreEqual(2,                      dest.Field3);
			Assert.AreEqual(src.Field5,             dest.Field4);
			Assert.AreEqual(src.Field6,             dest.Field6.Value);
			Assert.AreEqual(src.Field7.Value,       dest.Field7);
			Assert.AreEqual(src.Field8 ?? 0,        dest.Field8);
			Assert.AreEqual(src.Field9 ?? 0,        dest.Field9);
			Assert.AreEqual(src.Field10.ToString(), dest.Field10);
			Assert.AreEqual(src.Field11,            dest.Field11.ToString());
			Assert.AreEqual(12,                     dest.Field12);
			Assert.AreEqual(13,                     dest.Field13);
			Assert.AreEqual(14,                     dest.Field14);
			Assert.AreEqual(Gender.Female,          dest.Field15);
			Assert.AreEqual("M",                    dest.Field16);
			Assert.AreEqual(Enum2.Value2,           dest.Field17);
		}

		[Test]
		public void MapObject()
		{
			var mapper = Map.GetObjectMapper<Source,Source>();
			var src    = new Source();
			var dest   = mapper(src);

			Assert.AreNotSame(src,         dest);
			Assert.AreEqual  (src.Field1,  dest.Field1);
			Assert.AreEqual  (src.Field2,  dest.Field2);
			Assert.AreEqual  (src.Field5,  dest.Field5);
			Assert.AreEqual  (src.Field6,  dest.Field6);
			Assert.AreEqual  (src.Field7,  dest.Field7);
			Assert.AreEqual  (src.Field8,  dest.Field8);
			Assert.AreEqual  (src.Field9,  dest.Field9);
			Assert.AreEqual  (src.Field10, dest.Field10);
			Assert.AreEqual  (src.Field11, dest.Field11);
			Assert.AreEqual  (src.Field12, dest.Field12);
			Assert.AreEqual  (src.Field13, dest.Field13);
			Assert.AreEqual  (src.Field14, dest.Field14);
			Assert.AreEqual  (src.Field15, dest.Field15);
			Assert.AreEqual  (src.Field16, dest.Field16);
			Assert.AreEqual  (src.Field17, dest.Field17);
		}

		class Class1 { public int Field = 1; }
		class Class2 { public int Field = 2; }
		class Class3 { public Class1 Class = new Class1(); }
		class Class4 { public Class2 Class = new Class2(); }

		[Test]
		public void MapInnerObject1()
		{
			var mapper = Map.GetObjectMapper<Class3,Class4>();
			var src    = new Class3();
			var dest   = mapper(src);

			Assert.AreEqual(src.Class.Field, dest.Class.Field);
		}

		class Class5 { public Class1 Class1 = new Class1(); public Class1 Class2; }
		class Class6 { public Class2 Class1 = new Class2(); public Class2 Class2 = null; }

		[Test]
		public void MapInnerObject2()
		{
			var mapper = Map.GetObjectMapper<Class5,Class6>();
			var src    = new Class5();

			src.Class2 = src.Class1;

			var dest = mapper(src);

			Assert.IsNotNull(dest.Class1);
			Assert.AreSame(dest.Class1, dest.Class2);
		}

		class Class7  { public Class9  Class; }
		class Class8  { public Class10 Class = null; }
		class Class9  { public Class7  Class = new Class7(); }
		class Class10 { public Class8  Class = new Class8(); }

		[Test]
		public void SelfReference1()
		{
			var mapper = Map.GetObjectMapper<Class9,Class10>();
			var src    = new Class9();

			src.Class.Class = src;

			var dest = mapper(src);

			Assert.AreSame(dest, dest.Class.Class);
		}

		class Class11 { public Class9  Class = new Class9();  }
		class Class12 { public Class10 Class = new Class10(); }

		[Test]
		public void SelfReference2()
		{
			var mapper = Map.GetObjectMapper<Class11,Class12>();
			var src    = new Class11();

			src.Class.Class.Class = src.Class;

			var dest = mapper(src);

			Assert.AreSame(dest.Class, dest.Class.Class.Class);
		}

		class Class13 { public Class1 Class = new Class1();  }
		class Class14 { public Class1 Class = new Class1();  }

		[Test]
		public void DeepCopy1()
		{
			var mapper = Map.GetObjectMapper<Class13,Class14>();
			var src    = new Class13();

			var dest = mapper(src);

			Assert.AreNotSame(src.Class, dest.Class);
		}

		[Test]
		public void DeepCopy2()
		{
			var mapper = Map.GetObjectMapper<Class13,Class14>(false);
			var src    = new Class13();

			var dest = mapper(src);

			Assert.AreSame(src.Class, dest.Class);
		}

		class Class15 { public List<Class1> List = new List<Class1> { new Class1(), new Class1() }; }
		class Class16 { public List<Class2> List = null; }

		[Test]
		public void ObjectList()
		{
			var mapper = Map.GetObjectMapper<Class15,Class16>();
			var src    = new Class15();

			src.List.Add(src.List[0]);

			var dest = mapper(src);

			Assert.AreEqual  (3, dest.List.Count);
			Assert.IsNotNull (dest.List[0]);
			Assert.IsNotNull (dest.List[1]);
			Assert.IsNotNull (dest.List[2]);
			Assert.AreNotSame(dest.List[0], dest.List[1]);
			Assert.AreSame   (dest.List[0], dest.List[2]);
		}

		[Test]
		public void ScalarList()
		{
			var mapper = Map.GetObjectMapper<List<int>,IList<string>>();
			var dest = mapper(new List<int> { 1, 2, 3});

			Assert.AreEqual("1", dest[0]);
			Assert.AreEqual("2", dest[1]);
			Assert.AreEqual("3", dest[2]);
		}

		[Test]
		public void ScalarArray()
		{
			var mapper = Map.GetObjectMapper<int[],string[]>();
			var dest = mapper(new[] { 1, 2, 3});

			Assert.AreEqual("1", dest[0]);
			Assert.AreEqual("2", dest[1]);
			Assert.AreEqual("3", dest[2]);
		}

		class Class17
		{
			public IEnumerable<Class9> Arr { get { return GetEnumerable(); }}

			static IEnumerable<Class9> GetEnumerable()
			{
				var c = new Class9();

				yield return c;
				yield return new Class9();
				yield return c;
			}
		}

		class Class18 { public Class9[] Arr = null; }

		[Test]
		public void ObjectArray1()
		{
			var mapper = Map.GetObjectMapper<Class17,Class18>();
			var dest = mapper(new Class17());

			Assert.AreEqual  (3, dest.Arr.Length);
			Assert.IsNotNull (dest.Arr[0]);
			Assert.IsNotNull (dest.Arr[1]);
			Assert.IsNotNull (dest.Arr[2]);
			Assert.AreNotSame(dest.Arr[0], dest.Arr[1]);
			Assert.AreSame   (dest.Arr[0], dest.Arr[2]);
		}

		class Class19
		{
			public Class9[] Arr { get { return new Class17().Arr.ToArray();  }}
		}

		[Test]
		public void ObjectArray2()
		{
			var mapper = Map.GetObjectMapper<Class19,Class18>();
			var dest = mapper(new Class19());

			Assert.AreEqual  (3, dest.Arr.Length);
			Assert.IsNotNull (dest.Arr[0]);
			Assert.IsNotNull (dest.Arr[1]);
			Assert.IsNotNull (dest.Arr[2]);
			Assert.AreNotSame(dest.Arr[0], dest.Arr[1]);
			Assert.AreSame   (dest.Arr[0], dest.Arr[2]);
		}

		class Class20 { public Source Class1 = new Source(); public Source Class2; }
		class Class21 { public Dest   Class1 = null;         public Dest   Class2 = null; }

		[Test]
		public void NoCrossRef()
		{
			var mapper = new ExpressionMapper<Class20,Class21> { HandleBackReferences = false }.GetMapper();
			var source = new Class20();

			source.Class2 = source.Class1;

			var dest = mapper(source);

			Assert.IsNotNull (dest.Class1);
			Assert.IsNotNull (dest.Class2);
			Assert.AreNotSame(dest.Class1, dest.Class2);
		}

		class Object1
		{
			public int Field1;
			[ExpressionMapIgnore]
			public int Field2;
		}

		class Object2
		{
			public int Field1;
			public int Field2;
		}

		[Test]
		public void ExpressionMapIgnoreTest()
		{
			var mapper1  = Map.GetObjectMapper<Object1,Object2>();
			var object2 = mapper1(new Object1 { Field1 = 1, Field2 = 2 });

			Assert.That(object2.Field2, Is.Not.EqualTo(2));

			var mapper2 = Map.GetObjectMapper<Object2,Object1>();
			var object1 = mapper2(new Object2 { Field1 = 1, Field2 = 2 });

			Assert.That(object1.Field2, Is.Not.EqualTo(2));
		}

		[MapField("SomethingColumnInDB", "MyInnerClass.Something")]
		class MyClass
		{
			public int          ID;
			public string       Name;
			public MyInnerClass MyInnerClass;
		}

		class MyInnerClass
		{
			public string Something;
		}

		[Test]
		public void MapFieldTest()
		{
			var entity = new MyClass 
			{ 
				ID           = 1,
				Name         = "Test",
				MyInnerClass = new MyInnerClass { Something = "Something" } 
			};

			var mapper = Map.GetObjectMapper<MyClass,MyClass>(true, true);
			var clone = mapper(entity);

			Assert.That(clone.MyInnerClass, Is.Not.Null);
		}
	}
}
