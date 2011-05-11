using BLToolkit.Mapping;

namespace Mono.Model
{
	public class Patient
	{
		public int    PersonID;
		public string Diagnosis;

		[Association(ThisKey = "PersonID", OtherKey = "ID", CanBeNull = false)]
		public Person Person;
	}
}
