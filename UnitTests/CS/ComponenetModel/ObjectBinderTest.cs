using System.ComponentModel;
using BLToolkit.ComponentModel;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace UnitTests.CS.ComponenetModel
{
	[TestFixture]
	public class ObjectBinderTest
	{
		[Test]
		public void GetItemPropertiesTest()
		{
			ObjectBinder binder = new ObjectBinder();

			binder.ListChanged += delegate(object sender, ListChangedEventArgs e)
			{
				PropertyDescriptorCollection properties = ((ITypedList)sender).GetItemProperties(null);

				Assert.That(properties, Is.Not.Null);
				Assert.That(properties, Is.Not.Empty);
			};

			binder.ItemType = typeof(string);
		}
	}
}
