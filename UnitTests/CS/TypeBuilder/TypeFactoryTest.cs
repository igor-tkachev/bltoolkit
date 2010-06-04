using System;
using System.IO;

using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace TypeBuilder
{
	[TestFixture]
	public class TypeFactoryTest : MarshalByRefObject
	{
		public TypeFactoryTest()
		{
			// Force call to TypeFactory .cctor
			//
			TypeFactory.SealTypes = true;

			Console.WriteLine("Domain name: {0}", AppDomain.CurrentDomain.FriendlyName);
		}

		[Serializable]
		public abstract class TestObject
		{
			public abstract int Number { get; set; }
		}

		public int GetNumber(TestObject o)
		{
			return o.Number;
		}

		[Test]
		public void AssemblyResolver()
		{
			AppDomainSetup setup = new AppDomainSetup();
			string         basePath = GetType().Assembly.GetName(false).CodeBase;

			basePath = basePath.Replace("file:///", "");
			basePath = Path.GetDirectoryName(basePath);

			setup.ApplicationBase = "file:///" + basePath;

			AppDomain appDomain = AppDomain.CreateDomain("NewDomain", null, setup);

			basePath = GetType().Assembly.GetName(false).CodeBase;
			basePath = Path.GetFileName(basePath).ToLower().Replace(".dll", "");

			TypeFactoryTest test = (TypeFactoryTest)appDomain.CreateInstanceAndUnwrap(basePath, GetType().FullName);

			TestObject o = (TestObject)TypeAccessor.CreateInstance(typeof(TestObject));

			o.Number = 23;

			int n = test.GetNumber(o);

			AppDomain.Unload(appDomain);

			Assert.AreEqual(23, n);
		}
	}
}
