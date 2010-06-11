namespace UnitTests.CS.ProviderSpecific.MySql
{
	internal class Person
	{
		public int PersonID;
		public string FirstName;
		public string LastName;
		public string MiddleName;
		public char Gender;

		public override bool Equals(object obj)
		{
			return Equals(obj as Person);
		}
		public bool Equals(Person other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other.PersonID == PersonID && Equals(other.FirstName, FirstName) && Equals(other.LastName, LastName) && Equals(other.MiddleName, MiddleName) && other.Gender == Gender;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = PersonID;
				result = (result * 397) ^ (FirstName != null ? FirstName.GetHashCode() : 0);
				result = (result * 397) ^ (LastName != null ? LastName.GetHashCode() : 0);
				result = (result * 397) ^ (MiddleName != null ? MiddleName.GetHashCode() : 0);
				result = (result * 397) ^ Gender.GetHashCode();
				return result;
			}
		}
	}
}
