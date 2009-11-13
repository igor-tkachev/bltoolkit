using System;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace Data.Linq.Model
{
	[TableName("GrandChild")]
	public class GrandChild1 : IEquatable<GrandChild1>
	{
		public int  ParentID;
		public int? ChildID;
		public int? GrandChildID;

		[Association(ThisKey = "ParentID, ChildID", OtherKey = "ParentID, ChildID")]
		public Child Child;

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (GrandChild1)) return false;

			return Equals((GrandChild1)obj);
		}

		public bool Equals(GrandChild1 other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;

			return other.ParentID.Equals(ParentID) && other.ChildID.Equals(ChildID) && other.GrandChildID.Equals(GrandChildID);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var result = ParentID;

				result = (result * 397) ^ (ChildID.     HasValue ? ChildID.     Value : 0);
				result = (result * 397) ^ (GrandChildID.HasValue ? GrandChildID.Value : 0);

				return result;
			}
		}
	}
}
