using System;
using System.Collections.Generic;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace Data.Linq.Model
{
	[TableName("Parent")]
	public class Parent1 : IEquatable<Parent1>, IComparable
	{
		[PrimaryKey]
		public int  ParentID;
		public int? Value1;

		[Association(ThisKey = "ParentID", OtherKey = "ParentID")]
		public List<Child> Children;

		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof (Parent1)) return false;
			return Equals((Parent1)obj);
		}

		public bool Equals(Parent1 other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other.ParentID == ParentID;
		}

		public override int GetHashCode()
		{
			unchecked { return ParentID * 397; }
		}

		public int CompareTo(object obj)
		{
			return ParentID - ((Parent1)obj).ParentID;
		}
	}
}
