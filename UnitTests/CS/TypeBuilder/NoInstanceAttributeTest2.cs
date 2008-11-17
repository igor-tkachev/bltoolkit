using NUnit.Framework;

using BLToolkit.EditableObjects;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace TypeBuilder
{
	[TestFixture]
	public class NoInstanceAttributeTest2
	{
		public abstract class PersonCitizenship : EditableObject
		{
		}

		public abstract class Person : EditableObject
		{
			[NoInstance]
			public abstract PersonCitizenship Citizenship { get; set; }
		}

		[Test]
		public void Text()
		{
			Person person = (Person)TypeAccessor.CreateInstance(typeof(Person));

			Assert.IsNull(person.Citizenship);

			person.Citizenship = (PersonCitizenship)TypeAccessor.CreateInstance(typeof(PersonCitizenship));

			Assert.IsNotNull(person.Citizenship);
		}
	}
}
