using System;

namespace Data.Linq.Model
{
	using BLToolkit.Mapping;

	public class Person
	{
		public Person()
		{
		}

		public Person(int id)
		{
			ID = id;
		}

		public Person(int id, string firstName)
		{
			ID        = id;
			FirstName = firstName;
		}

		[MapField("PersonID")] public int    ID;
		                       public string FirstName { get; set; }
		                       public string LastName;
		[Nullable]             public string MiddleName;
		                       public char   Gender;
	}
}
