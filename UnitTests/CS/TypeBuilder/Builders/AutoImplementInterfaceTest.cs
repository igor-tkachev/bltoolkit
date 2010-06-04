using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Mapping;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace TypeBuilder.Builders
{
	[TestFixture]
	public class AutoImplementInterfaceTest
	{
		#region Test

		[AutoImplementInterface]
		public interface Test1
		{
			string Name { get; }
		}

		[Test]
		public void Test()
		{
			TypeAccessor ta = TypeAccessor.GetAccessor(typeof(Test1));
			Test1 t = (Test1)ta.CreateInstance();

			Dictionary<string, object> dic = new Dictionary<string,object>();

			dic.Add("Name", "John");

			Map.MapSourceToDestination(
				Map.DefaultSchema.CreateDictionaryMapper(dic), dic,
				Map.GetObjectMapper(t.GetType()), t);

			Assert.AreEqual("John", t.Name);
		}

		#endregion

		#region TestException

		public interface Test2
		{
			string Name { get; }
		}

		[Test, ExpectedException(typeof(TypeBuilderException))]
		public void TestException()
		{
			TypeAccessor ta = TypeAccessor.GetAccessor(typeof(Test2));
			Test2 t = (Test2)ta.CreateInstance();
		}

		#endregion

		#region TestMemberImpl

		[AutoImplementInterface]
		public interface Test3
		{
			string Name { get; set; }
		}

		[AutoImplementInterface]
		public interface Test4
		{
			Test3 Test { get; }
		}

		[Test]
		public void TestMemberImpl()
		{
			Test4 t = TypeAccessor<Test4>.CreateInstance();

			t.Test.Name = "John";

			Assert.AreEqual("John", t.Test.Name);
		}

		#endregion

		#region Inheritance

		public interface InterfaceBase
		{
			string Name { get; set; }
		}

		public interface Interface1 : InterfaceBase
		{
			void Foo();
		}

		[AutoImplementInterface]
		public interface Interface2 : Interface1, InterfaceBase
		{
			void Bar();
		}

		[Test]
		public void TestInheritance()
		{
			//TypeFactory.SaveTypes = true;

			Interface2 i2 = TypeAccessor<Interface2>.CreateInstance();
			Interface1 i1 = i2;

			i1.Foo();
			i2.Foo();
			i2.Bar();

			i1.Name = "John";

			Assert.AreEqual("John", i1.Name);
			Assert.AreEqual("John", i2.Name);
		}

		#endregion

		#region AssociateTypeTest

		public class MyClass : Interface1
		{
			public void Foo() {}

			private string _name;
			public  string  Name
			{
				get { return _name;  }
				set { _name = value; }
			}

			public string Address;
		}

		[Test]
		public void AssociateTypeTest()
		{
			TypeAccessor.AssociateType(typeof(Interface1), typeof(MyClass));

			Interface1 i1 = TypeAccessor<Interface1>.CreateInstance();

			i1.Name = "John";

			Assert.AreEqual("John", i1.Name);
			Assert.AreEqual("John", TypeAccessor<Interface1>.Instance["Name"].GetValue(i1).ToString());
		}

		#endregion
		
		#region AssociateTypeHandlerTest

		public interface IMy
		{
			string Name { get; set; }
		}

		public class MyImpl : IMy
		{
			private string _name;
			public  string  Name
			{
				get { return _name;  }
				set { _name = value; }
			}
		}


		[Test]
		public void AssociateTypeHandlerTest()
		{
			TypeAccessor.AssociatedTypeHandler += delegate(Type parent)
			{
				if (parent == typeof(IMy)) return typeof(MyImpl);
				return null;
			};

			IMy i = TypeAccessor<IMy>.CreateInstance();

			i.Name = "John";

			Assert.AreEqual("John", i.Name);
			Assert.AreEqual("John", TypeAccessor<IMy>.Instance["Name"].GetValue(i).ToString());
		}

		#endregion
	}
}
