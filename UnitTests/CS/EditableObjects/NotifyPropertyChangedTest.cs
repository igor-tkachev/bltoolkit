using System.ComponentModel;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;
using NUnit.Framework;

namespace EditableObjects
{
	[TestFixture]
	public class NotifyPropertyChangedTest
	{
		[PropertyChanged]
		public abstract class ObservableObject : INotifyPropertyChanged
		{
			public event PropertyChangedEventHandler PropertyChanged;

			public abstract int ID { get; set; }
			public abstract string Name { get; set; }
			public abstract int Seconds { get; set; }

			public static ObservableObject CreateInstance()
			{
				return TypeAccessor<ObservableObject>.CreateInstance();
			}
		}

		[Test]
		public void TestPropertyChangedFired()
		{
			ObservableObject obj = ObservableObject.CreateInstance();
			bool propertyChangedFired = false;
			obj.PropertyChanged += delegate { propertyChangedFired = true; };

			obj.Name = "this should fire PropertyChanged event";
			Assert.That(propertyChangedFired);
		}
	}
}
