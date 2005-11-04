using System;
using System.Reflection;

namespace BLToolkit.Reflection
{
	public abstract class MemberAccessor
	{
		protected MemberAccessor(MemberInfo memberInfo)
		{
			_memberInfo = memberInfo;
		}

		private MemberInfo _memberInfo;
		public  MemberInfo  MemberInfo
		{
			get { return _memberInfo;  }
			set { _memberInfo = value; }
		}

//		public abstract object GetValue(object o);
//		public abstract void   SetValue(object o, object value);
	}
}
