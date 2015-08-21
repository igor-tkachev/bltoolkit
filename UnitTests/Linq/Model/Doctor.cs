using System;

namespace Data.Linq.Model
{
	public class Doctor
	{
		public int    PersonID;
		public string Taxonomy;

		public override bool Equals(object obj)
		{
			var doc = obj as Doctor;
			if (doc == null)
				return false;

			return PersonID.Equals(doc.PersonID) && Taxonomy.Equals(doc.Taxonomy);
		}

		public override int GetHashCode()
		{
			return (PersonID * 387) ^ (Taxonomy ?? "").GetHashCode();
		}
	}
}
