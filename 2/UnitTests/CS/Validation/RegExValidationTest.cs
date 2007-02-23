using System;
using System.Collections.Generic;
using System.Text;
using BLToolkit.EditableObjects;
using BLToolkit.Validation;
using NUnit.Framework;

namespace UnitTests.CS.Validation
{
	[TestFixture]
	public class RegExValidationTest
	{
		public abstract class Entity : EditableObject<Entity>
		{
			[RegEx("[a-zA-Z0-9]*")]
			public string AlphaNumeric;
		}

		[Test]
		public void Test()
		{
			Entity entity = Entity.CreateInstance();

			entity.AlphaNumeric = null; Assert.IsTrue(entity.IsValid("AlphaNumeric"));
			entity.AlphaNumeric = ""; Assert.IsTrue(entity.IsValid("AlphaNumeric"));
			entity.AlphaNumeric = "abAB01"; Assert.IsTrue(entity.IsValid("AlphaNumeric"));
			entity.AlphaNumeric = "01ABab"; Assert.IsTrue(entity.IsValid("AlphaNumeric"));
			entity.AlphaNumeric = "ab_AB.01"; Assert.IsFalse(entity.IsValid("AlphaNumeric"));
			entity.AlphaNumeric = "33###"; Assert.IsFalse(entity.IsValid("AlphaNumeric"));
		}
	}
}
