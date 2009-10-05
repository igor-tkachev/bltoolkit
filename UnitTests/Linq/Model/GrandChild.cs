using System;

namespace Data.Linq.Model
{
	public class GrandChild : IEquatable<GrandChild>
	{
		public int? ParentID;
		public int? ChildID;
		public int? GrandChildID;

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (GrandChild)) return false;

			return Equals((GrandChild)obj);
		}

		public bool Equals(GrandChild other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;

			return other.ParentID.Equals(ParentID) && other.ChildID.Equals(ChildID) && other.GrandChildID.Equals(GrandChildID);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var result = ParentID.HasValue ? ParentID.Value : 0;

				result = (result * 397) ^ (ChildID.     HasValue ? ChildID.     Value : 0);
				result = (result * 397) ^ (GrandChildID.HasValue ? GrandChildID.Value : 0);

				return result;
			}
		}
	}
}
