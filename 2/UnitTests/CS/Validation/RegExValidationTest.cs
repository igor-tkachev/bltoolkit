using BLToolkit.EditableObjects;
using BLToolkit.Reflection;
using BLToolkit.Validation;
using NUnit.Framework;

namespace UnitTests.CS.Validation
{
	[TestFixture]
	public class RegExValidationTest
	{
		public abstract class Entity : EditableObject
		{
			[RegEx("[a-zA-Z0-9]*")]
			public string AlphaNumeric;
		}

		[Test]
		public void Test()
		{
			Entity entity = (Entity)TypeAccessor.CreateInstance(typeof(Entity));

			entity.AlphaNumeric = null; Assert.IsTrue(entity.IsValid("AlphaNumeric"));
			entity.AlphaNumeric = ""; Assert.IsTrue(entity.IsValid("AlphaNumeric"));
			entity.AlphaNumeric = "abAB01"; Assert.IsTrue(entity.IsValid("AlphaNumeric"));
			entity.AlphaNumeric = "01ABab"; Assert.IsTrue(entity.IsValid("AlphaNumeric"));
			entity.AlphaNumeric = "ab_AB.01"; Assert.IsFalse(entity.IsValid("AlphaNumeric"));
			entity.AlphaNumeric = "33###"; Assert.IsFalse(entity.IsValid("AlphaNumeric"));
		}
	}
}
