using System;
using NUnit.Framework;
using BLToolkit.Reflection;
using BLToolkit.Mapping;
using BLToolkit.Data;
using System.Collections.Generic;

namespace UnitTests.CS.Data
{
	[TestFixture]
	public class ExecuteObjectTest
	{
		const string _query = @"  SELECT 1 AS PersonId, '1' AS FirstName, '1' AS MiddleName, '1' AS LastName
							UNION SELECT 2 AS PersonId, '2' AS FirstName, '2' AS MiddleName, '2' AS LastName
							UNION SELECT 3 AS PersonId, '3' AS FirstName, '3' AS MiddleName, '3' AS LastName
							UNION SELECT 4 AS PersonId, '4' AS FirstName, '0' AS MiddleName, '4' AS LastName
							UNION SELECT 5 AS PersonId, '5' AS FirstName, '1' AS MiddleName, '5' AS LastName
							UNION SELECT 6 AS PersonId, '6' AS FirstName, '2' AS MiddleName, '6' AS LastName
							UNION SELECT 7 AS PersonId, '7' AS FirstName, '3' AS MiddleName, '7' AS LastName
							UNION SELECT 8 AS PersonId, '8' AS FirstName, '0' AS MiddleName, '8' AS LastName";

		[ObjectFactory(typeof(Person.Factory))]
		public class Person
		{
			public class Factory : IObjectFactory
			{
				public static Type GetType(int id)
				{
					int r = id % 4;

					switch (r)
					{
						case 0:
							return typeof(Person);
						case 1:
							return typeof(Person1);
						case 2:
							return typeof(Person2);
						default:
							return typeof(Person3);
					}
				}

				#region IObjectFactory Members

				public object CreateInstance(TypeAccessor typeAccessor, InitContext context)
				{
					int id = context.DataSource.GetInt32(context.SourceObject,
						context.DataSource.GetOrdinal("PersonId"));
					
					context.ObjectMapper = context.MappingSchema.GetObjectMapper(Factory.GetType(id));

					return context.ObjectMapper.TypeAccessor.CreateInstance(context);
				}

				#endregion
			}

			public int    PersonId;
			public string FirstName;
		}

		public class Person1 : Person 
		{ 
			public string MiddleName;
		}

		public class Person2 : Person
		{
			public string LastName;
		}

		public class Person3 : Person 
		{ 
			public string MiddleName;
			public string LastName;
		}

		[Test]
		public void PerfomanceTest()
		{
			using (DbManager db = new DbManager())
			{

				List<Person> list = db.SetCommand(_query)
					.ExecuteList<Person>();

				foreach (Person a in list)
				{
					Assert.AreEqual(Person.Factory.GetType(a.PersonId), a.GetType());

					Assert.AreEqual(a.PersonId.ToString(), a.FirstName);

					if (a is Person2)
						Assert.AreEqual(a.PersonId.ToString(), (a as Person2).LastName);

					if (a is Person1)
						Assert.AreEqual((a.PersonId % 4).ToString(), (a as Person1).MiddleName);

					if (a is Person3)
					{
						Assert.AreEqual(a.PersonId.ToString(),       (a as Person3).LastName);
						Assert.AreEqual((a.PersonId % 4).ToString(), (a as Person3).MiddleName);
					}
				}
			}
		}
	}
}
