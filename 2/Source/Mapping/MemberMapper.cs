using System;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class MemberMapper
	{
		private MemberAccessor _memberAccessor;
		public  MemberAccessor  MemberAccessor
		{
			get { return _memberAccessor; }
		}

		internal void SetMemberAccessor(MemberAccessor memberAccessor)
		{
			_memberAccessor = memberAccessor;
		}

		private string _name;
		public  string  Name
		{
			get { return _name;  }
		}

		internal void SetName(string name)
		{
			_name = name;
		}

		private int _ordinal;
		public  int  Ordinal
		{
			get { return _ordinal;  }
		}

		internal void SetOrdinal(int ordinal)
		{
			_ordinal = ordinal;
		}

		public virtual object GetValue(object o)
		{
			return _memberAccessor.GetValue(o);
		}

		public virtual void SetValue(object o, object value)
		{
			_memberAccessor.SetValue(o, value);
		}
	}
}
