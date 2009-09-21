using System;

namespace Data.Linq.Model
{
	public class Parent : IEquatable<Parent>
	{
		public int  ParentID;
		public int? Value1;

		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof (Parent)) return false;
			return Equals((Parent)obj);
		}

		public bool Equals(Parent other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other.ParentID == ParentID && other.Value1.Equals(Value1);
		}

		public override int GetHashCode()
		{
			unchecked { return (ParentID * 397) ^ (Value1 ?? 0); }
		}
	}
}
