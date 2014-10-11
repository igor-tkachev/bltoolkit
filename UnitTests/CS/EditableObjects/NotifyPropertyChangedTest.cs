using System;
using System.ComponentModel;
using System.Reflection;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;
using NUnit.Framework;

namespace EditableObjects
{
	[TestFixture]
	public class NotifyPropertyChangedTest
	{
		[PropertyChanged]
		public abstract class ObservableObject : INotifyPropertyChanged, IPropertyChanged
		{
			#region Implementation of IPropertyChanged

			public event PropertyChangedEventHandler PropertyChanged;

			#endregion

			public abstract int ID { get; set; }
			public abstract string Name { get; set; }
			public abstract int Seconds { get; set; }

			public static ObservableObject CreateInstance()
			{
				return TypeAccessor<ObservableObject>.CreateInstance();
			}

			#region Implementation of IPropertyChanged

			void IPropertyChanged.OnPropertyChanged(PropertyInfo propertyInfo)
			{
				OnPropertyChanged(propertyInfo.Name);
			}

			#endregion

			protected internal virtual void OnPropertyChanged(string propertyName)
			{
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
