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

		public int    PersonID;
		public string FirstName;
		public string LastName;
		public string MiddleName;
		public char   Gender;
	}
}
