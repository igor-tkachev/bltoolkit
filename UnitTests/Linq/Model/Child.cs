using System;
using System.Collections.Generic;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace Data.Linq.Model
{
	public class Child
	{
		[PrimaryKey] public int ParentID;
		[PrimaryKey] public int ChildID;

		[Association(ThisKey = "ParentID", OtherKey = "ParentID")]
		public Parent  Parent;

		[Association(ThisKey = "ParentID", OtherKey = "ParentID", CanBeNull = false)]
		public Parent1 Parent1;

		[Association(ThisKey = "ParentID, ChildID", OtherKey = "ParentID, ChildID")]
		public List<GrandChild> GrandChildren;

		public override bool Equals(object obj)
		{
			return Equals(obj as Child);
		}

		public bool Equals(Child other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;

			return other.ParentID == ParentID && other.ChildID == ChildID;
		}

		public override int GetHashCode()
		{
			unchecked { return (ParentID * 397) ^ ChildID; }
		}
	}
}
