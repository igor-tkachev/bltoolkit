using System;
using System.Collections;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Reflection;
using BLToolkit.Mapping;

namespace Data
{
	[TestFixture]
	public class DbManagerTest
	{
		public enum Gender
		{
			[MapValue("F")] Female,
			[MapValue("M")] Male,
			[MapValue("U")] Unknown,
			[MapValue("O")] Other
		}

		public class Person
		{
			[MapField("PersonID")]
			public int    ID;
			public string FirstName;
			public string MiddleName;
			public string LastName;
			public Gender Gender;
		}

		[Test]
		public void ExecuteList()
		{
			using (DbManager db = new DbManager("Sql"))
			{
				ArrayList list = db
					.SetCommand("SELECT * FROM Person")
					.ExecuteList(typeof(Person));
			}
		}

		[Test]
		public void ExecuteObject()
		{
			using (DbManager db = new DbManager())
			{
				Person p = (Person)db
					.SetCommand("SELECT * FROM Person WHERE PersonID = @id",
						db.Parameter("@id", 1))
					.ExecuteObject(typeof(Person));

				TypeAccessor.WriteConsole(p);
			}
		}
	}
}
