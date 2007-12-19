using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace HowTo.Reflection
{
	[TestFixture]
	public class ObjectFactoryTest
	{
		/*[a]*/[ObjectFactory(typeof(Person.ObjectFactory))]/*[/a]*/
		public class Person
		{
			[MapField("PersonID")]
			public int    ID;

			public string LastName;
			public string FirstName;
			public string MiddleName;

			/*[a]*/class ObjectFactory : IObjectFactory/*[/a]*/
			{
				public object /*[a]*/CreateInstance(TypeAccessor typeAccessor, InitContext context)/*[/a]*/
				{
					// Get the object type indicator field.
					//
					object objectType = context.DataSource.GetValue(context.SourceObject, "PersonType");

					// Target ObjectMapper must be changed in order to provide correct mapping.
					//
					switch ((string)objectType)
					{
						case "D": /*[a]*/context.ObjectMapper = ObjectMapper<Doctor>. Instance;/*[/a]*/ break;
						case "P": /*[a]*/context.ObjectMapper = ObjectMapper<Patient>.Instance;/*[/a]*/ break;
					}

					// Create an object instance.
					// Do not call ObjectMapper.CreateInstance as it will lead to infinite recursion.
					//
					return context.ObjectMapper./*[a]*/TypeAccessor/*[/a]*/.CreateInstance(context);
				}
			}
		}

		public class /*[a]*/Doctor : Person/*[/a]*/
		{
			public string Taxonomy;
		}

		public class /*[a]*/Patient : Person/*[/a]*/
		{
			public string Diagnosis;
		}

		[Test]
		public void Test()
		{
			using (DbManager db = new DbManager())
			{
				List<Person> list = db
					.SetCommand(@"
						SELECT
							ps.*,
							d.Taxonomy,
							p.Diagnosis,
							CASE
								WHEN d.PersonID IS NOT NULL THEN 'D'
								WHEN p.PersonID IS NOT NULL THEN 'P'
							END as PersonType
						FROM
							Person ps
								LEFT JOIN Doctor  d ON d.PersonID = ps.PersonID
								LEFT JOIN Patient p ON p.PersonID = ps.PersonID
						ORDER BY
							ps.PersonID")
					.ExecuteList<Person>();

				Assert.AreEqual(list[0].GetType(), /*[a]*/typeof(Doctor)/*[/a]*/);
				Assert.AreEqual(list[1].GetType(), /*[a]*/typeof(Patient)/*[/a]*/);

				if (list.Count > 2)
					Assert.AreEqual(list[2].GetType(), typeof(Person));
			}
		}
	}
}
