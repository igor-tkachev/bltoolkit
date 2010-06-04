using System;

using BLToolkit.Mapping;

namespace Data.Linq.Model
{
	public class Patient
	{
		public int    PersonID;
		public string Diagnosis;

		[Association(ThisKey = "PersonID", OtherKey = "ID", CanBeNull = false)]
		public Person Person;
	}
}
