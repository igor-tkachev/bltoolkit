using System;

using NUnit.Framework;

using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace Reflection
{
	[TestFixture]
	public class TypeAccessorTest3
	{
		public class TestObject
		{
			private int _field;
			[MapField("_field")]
			public  int  Field
			{
				get { return _field; }
				set { _field = 10;   }
			}
		}

		[Test]
		public void Test()
		{
			var o = TypeAccessor<TestObject>.CreateInstance();

			TypeAccessor<TestObject>.Instance["Field"].SetValue(o, 5);

			Assert.AreEqual(5, o.Field);
		}

		class AAA
		{
			private delegate object GetValueAsObject(object o);

			private GetValueAsObject _getValueAsObject;

			public virtual object GetValue(object o) { return _getValueAsObject(o); }
			
		}

	}
}
