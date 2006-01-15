using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace HowTo.Data
{
	[TestFixture]
	public class ExecuteObjectList
	{
		[MapValue(Gender.Female,  "F")]
		[MapValue(Gender.Male,    "M")]
		[MapValue(Gender.Unknown, "U")]
		[MapValue(Gender.Other,   "O")]
		public enum Gender
		{
			Female,
			Male,
			Unknown,
			Other
		}

		[MapField("PersonID", "ID")]
		public class Person
		{
			public int    ID;

			public string LastName;
			public string FirstName;
			public string MiddleName;
			public Gender Gender;
		}

		IList<Person> GetPersonListSqlText()
		{
			using (DbManager db = new DbManager())
			{
				return db
					.SetCommand("SELECT * FROM Person")
					.ExecuteList<Person>();
			}
		}

		IList<Person> GetPersonListSproc()
		{
			using (DbManager db = new DbManager())
			{
				return db
					.SetSpCommand("Person_SelectAll")
					.ExecuteList<Person>();
			}
		}

		[Test]
		public void SqlText()
		{
			IList<Person> list = GetPersonListSqlText();

			foreach (Person p in list)
				TypeAccessor.WriteDebug(p);
		}

		[Test]
		public void Sproc()
		{
			IList<Person> list = GetPersonListSproc();

			foreach (Person p in list)
				TypeAccessor.WriteDebug(p);
		}
	}
}

