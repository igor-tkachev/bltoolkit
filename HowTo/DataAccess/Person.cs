using System;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	public class Person
	{
		[MapField("PersonID"), PrimaryKey, NonUpdatable]
		public int    ID;

		public string LastName;
		public string FirstName;
		public string MiddleName;
		public Gender Gender;
	}
}
