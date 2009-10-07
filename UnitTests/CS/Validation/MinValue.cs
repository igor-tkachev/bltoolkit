using System;

using NUnit.Framework;

using BLToolkit.Validation;

namespace Validation
{
	[TestFixture]
	public class MinValue
	{
		class Entity
		{
			[MinValue(1), Required]
			public float Test { get; set; }
		}

		[Test]
		public void Test()
		{
			/*
			var nullValue = TypeAccessor.GetNullValue;
			TypeAccessor.GetNullValue = type =>
			{
				if (type == typeof(float)) 
					return float.MinValue;
				else return nullValue(type);
			};
			*/

			var foo = new Entity();
			Assert.AreEqual(false, Validator.IsValid(foo, "Test"));
		}
	}
}
