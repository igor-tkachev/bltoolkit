using System.Reflection;
using BLToolkit.EditableObjects;
using NUnit.Framework;

using BLToolkit.TypeBuilder;
using BLToolkit.Reflection;

namespace TypeBuilder.Builders
{
	[TestFixture]
	public class PropertyChangeBuilderTest
	{
		public abstract class TestObject1 : IPropertyChanged
		{
			public string NotifiedName;

			public abstract int    ID   { get; set; }
			public abstract string Name { get; set; }

			public void OnPropertyChanged(PropertyInfo pi)
			{
				NotifiedName = pi.Name;
			}
		}

		[Test]
		public void TestPublic()
		{
			//Configuration.NotifyOnEqualSet = true;

			TestObject1 o = (TestObject1)TypeAccessor.CreateInstance(typeof(TestObject1));

			o.ID = 1;

			Assert.AreEqual("ID", o.NotifiedName);

			//Configuration.NotifyOnChangesOnly = false;
		}

		public abstract class TestObject2 : IPropertyChanged
		{
			public string NotifiedName;

			public abstract int    ID   { get; set; }
			public abstract string Name { get; set; }

			void IPropertyChanged.OnPropertyChanged(PropertyInfo pi)
			{
				NotifiedName = pi.Name;
			}
		}

		[Test]
		public void TestPrivate()
		{
			TestObject2 o = (TestObject2)TypeAccessor.CreateInstance(typeof(TestObject2));

			o.ID = 1;

			Assert.AreEqual("ID", o.NotifiedName);
		}

		[PropertyChanged()]
		public abstract class TestObject_Notification : IPropertyChanged
		{
			public abstract int ID { get; set; }
			public abstract string Name { get; set; }
			public abstract object Info { get; set; }

			void IPropertyChanged.OnPropertyChanged(PropertyInfo propertyInfo)
			{
			}
		}

		[PropertyChanged(false)]
		public abstract class TestObject_NoNotification : IPropertyChanged
		{
			public abstract int ID { get; set; }
			public abstract string Name { get; set; }
			public abstract object Info { get; set; }

			void IPropertyChanged.OnPropertyChanged(PropertyInfo propertyInfo)
			{
			}
		}

		[PropertyChanged(false, false)]
		public abstract class TestObject_NoNotificationEquals : IPropertyChanged
		{
			public abstract int ID { get; set; }
			public abstract string Name { get; set; }
			public abstract object Info { get; set; }

			void IPropertyChanged.OnPropertyChanged(PropertyInfo propertyInfo)
			{
			}
		}

		[PropertyChanged(false, false, false)]
		public abstract class TestObject_NoNotificationEqualsNoSkip : IPropertyChanged
		{
			public abstract int ID { get; set; }
			public abstract string Name { get; set; }
			public abstract object Info { get; set; }

			protected abstract decimal Dec { get; set; }

			void IPropertyChanged.OnPropertyChanged(PropertyInfo propertyInfo)
			{
			}
		}

		public abstract class Derived_TONNENS : TestObject_NoNotificationEqualsNoSkip
		{
			public abstract decimal NewVal { get; set; }
		}

		[Test]
		public void TestGeneration()
		{
			TestObject_Notification ton = (TestObject_Notification) TypeAccessor.CreateInstance(typeof(TestObject_Notification));
			TestObject_NoNotification tonn = (TestObject_NoNotification) TypeAccessor.CreateInstance(typeof(TestObject_NoNotification));
			TestObject_NoNotificationEquals tonne = (TestObject_NoNotificationEquals) TypeAccessor.CreateInstance(typeof(TestObject_NoNotificationEquals));
			TestObject_NoNotificationEqualsNoSkip tonnes = (TestObject_NoNotificationEqualsNoSkip) TypeAccessor.CreateInstance(typeof(TestObject_NoNotificationEqualsNoSkip));

			Derived_TONNENS derived_TONNENS = (Derived_TONNENS) TypeAccessor.CreateInstance(typeof(Derived_TONNENS));
		}

		[PropertyChanged(false)]
		public abstract class NullableFieldNoEqualsSetTest : IPropertyChanged
		{
			public string NotifiedName = "";

			public abstract bool? NullableBool { get; set; }
			public abstract int? NullableInt { get; set; }
			public abstract decimal? NullableDecimal { get; set; }
			public abstract float? NullableFloat { get; set; }

			void IPropertyChanged.OnPropertyChanged(PropertyInfo propertyInfo)
			{
				NotifiedName = propertyInfo.Name;
			}
		}

		[Test]
		public void TestNullableGeneration()
		{
			TypeFactory.SaveTypes = true;

			var testObject = TypeFactory.CreateInstance<NullableFieldNoEqualsSetTest>();

			testObject.NotifiedName = "";
			testObject.NullableInt = null;
			Assert.AreEqual("", testObject.NotifiedName);
			testObject.NullableInt = 10;
			Assert.AreEqual("NullableInt", testObject.NotifiedName);
			testObject.NotifiedName = "";
			testObject.NullableInt = 10;
			Assert.AreEqual("", testObject.NotifiedName);
			testObject.NullableInt = null;
			Assert.AreEqual("NullableInt", testObject.NotifiedName);

			testObject.NotifiedName = "";
			testObject.NullableBool = null;
			Assert.AreEqual("", testObject.NotifiedName);
			testObject.NullableBool = true;
			Assert.AreEqual("NullableBool", testObject.NotifiedName);
			testObject.NotifiedName = "";
			testObject.NullableBool = true;
			Assert.AreEqual("", testObject.NotifiedName);
			testObject.NullableBool = null;
			Assert.AreEqual("NullableBool", testObject.NotifiedName);

			testObject.NotifiedName = "";
			testObject.NullableDecimal = null;
			Assert.AreEqual("", testObject.NotifiedName);
			testObject.NullableDecimal = 10m;
			Assert.AreEqual("NullableDecimal", testObject.NotifiedName);
			testObject.NotifiedName = "";
			testObject.NullableDecimal = 10m;
			Assert.AreEqual("", testObject.NotifiedName);
			testObject.NullableDecimal = null;
			Assert.AreEqual("NullableDecimal", testObject.NotifiedName);

			testObject.NotifiedName = "";
			testObject.NullableFloat = null;
			Assert.AreEqual("", testObject.NotifiedName);
			testObject.NullableFloat = 0.1f;
			Assert.AreEqual("NullableFloat", testObject.NotifiedName);
			testObject.NotifiedName = "";
			testObject.NullableFloat = 0.1f;
			Assert.AreEqual("", testObject.NotifiedName);
			testObject.NullableFloat = null;
			Assert.AreEqual("NullableFloat", testObject.NotifiedName);

		}

		public struct UserClass { }

		[PropertyChanged(false)]
		public abstract class UserConfig : EditableObject
		{
			public abstract UserClass Range { get; set; }
		}

		[Test]
		public void TestStructPropertyChangedGeneration()
		{
			TypeAccessor.CreateInstance<UserConfig>().Range = new UserClass();
		}
	}
}
