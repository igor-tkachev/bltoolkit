using System;

using NUnit.Framework;

using Rsdn.Framework.Data.Mapping;

namespace CS
{
	[TestFixture]
	public class PropertyChangeNotification
	{
		public abstract class TestObject : IMapNotifyPropertyChanged
		{
			public string NotifiedName;

			public abstract int    ID   { get; set; }
			public abstract string Name { get; set; }

			public void PropertyChanged(MapPropertyInfo pi)
			{
				NotifiedName = pi.PropertyName;
			}
		}

		[Test]
		public void TestNotification()
		{
			TestObject o = (TestObject)Map.Descriptor(typeof(TestObject)).CreateInstance();

			o.ID = 1;

			Assert.AreEqual("ID", o.NotifiedName);
		}
	}
}
