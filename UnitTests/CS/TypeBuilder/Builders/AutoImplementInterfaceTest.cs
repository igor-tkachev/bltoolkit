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
		public void TestMemberImp()
		{
			TypeFactory.SaveTypes = true;

			Test4 t = TypeAccessor<Test4>.CreateInstance();

			t.Test.Name = "John";

			Assert.AreEqual("John", t.Test.Name);
		}
	}
}
