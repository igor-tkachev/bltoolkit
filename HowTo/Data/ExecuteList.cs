using System;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace HowTo.Data
{
	[TestFixture]
	public class ExecuteList
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
					./*[a]*/ExecuteList<Person>()/*[/a]*/;
			}
		}

		[Test]
		public void SqlText()
		{
			IList<Person> list = GetPersonListSqlText();

			foreach (Person p in list)
				TypeAccessor.WriteDebug(p);
		}

		IList<Person> GetPersonListSproc()
		{
			using (DbManager db = new DbManager())
			{
				return db
					.SetSpCommand("Person_SelectAll")
					./*[a]*/ExecuteList<Person>()/*[/a]*/;
			}
		}

		[Test]
		public void Sproc()
		{
			IList<Person> list = GetPersonListSproc();

			foreach (Person p in list)
				TypeAccessor.WriteDebug(p);
		}

		void GetCustomPersonList(IList list)
		{
			using (DbManager db = new DbManager())
			{
				db
					.SetSpCommand("Person_SelectAll")
					./*[a]*/ExecuteList(list, typeof(Person))/*[/a]*/;
			}
		}

		[Test]
		public void CustomList()
		{
			ArrayList list = new ArrayList(10);

			GetCustomPersonList(list);

			foreach (Person p in list)
				TypeAccessor.WriteDebug(p);
		}
	}
}
