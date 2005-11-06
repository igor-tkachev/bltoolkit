using System;
using System.Reflection;

using NUnit.Framework;

using BLToolkit.TypeBuilder;
using BLToolkit.Reflection;

namespace TypeBuilder.Builders
{
	[TestFixture]
	public class PropertyChangeBuilder
	{
		public PropertyChangeBuilder()
		{
			TypeFactory.SaveTypes = true;
		}

		public abstract class TestObject1 : IPropertyChanged
		{
			public string NotifiedName;

			public abstract int    ID   { get; set; }
			public abstract string Name { get; set; }

			public void PropertyChanged(PropertyInfo pi)
			{
				NotifiedName = pi.Name;
			}
		}

		[Test]
		public void TestPublic()
		{
			TestObject1 o = (TestObject1)TypeAccessor.GetAccessor(typeof(TestObject1)).CreateInstance();

			o.ID = 1;

			Assert.AreEqual("ID", o.NotifiedName);
		}

		public abstract class TestObject2 : IPropertyChanged
		{
			public string NotifiedName;

			public abstract int    ID   { get; set; }
			public abstract string Name { get; set; }

			void IPropertyChanged.PropertyChanged(PropertyInfo pi)
			{
				NotifiedName = pi.Name;
			}
		}

		[Test]
		public void TestPrivate()
		{
			TestObject2 o = (TestObject2)TypeAccessor.GetAccessor(typeof(TestObject2)).CreateInstance();

			o.ID = 1;

			Assert.AreEqual("ID", o.NotifiedName);
		}
	}
}
