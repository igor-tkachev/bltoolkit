using System;

namespace Data.Linq.Model
{
	public class Person
	{
		public Person()
		{
		}

		public Person(int id)
		{
			PersonID = id;
		}

		public Person(int id, string firstName)
		{
			PersonID  = id;
			FirstName = firstName;
		}

		public int    PersonID;
		public string FirstName;
		public string LastName;
		public string MiddleName;
		public char   Gender;
	}
}
