using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Reflection.Extension;

namespace Mapping
{
	[TestFixture]
	public class PersonMappingTest
	{
		[TypeExtension(FileName="Person.mapping.xml")]
		public abstract class Person : BLToolkit.EditableObjects.EditableObject
		{
			public abstract string FirstName { get; set; }
			public abstract string LastName  { get; set; }
		}

		[Test]
		public void Test()
		{
			using (DbManager db = new DbManager())
			{
				Person p = (Person)db
					.SetCommand("SELECT '1' as FIRST_NAME, '2' as 'LAST_NAME'")
					.ExecuteObject(typeof(Person));

				Assert.AreEqual("1", p.FirstName);
				Assert.AreEqual("2", p.LastName);
			}
		}
	}
}
