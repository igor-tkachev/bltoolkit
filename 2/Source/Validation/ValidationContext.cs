using System;
using System.Reflection;

using BLToolkit.Reflection;

namespace BLToolkit.Validation
{
	public class ValidationContext
	{
		public delegate bool IsNullHandler(ValidationContext context);

		private object _object;
		public  object  Object
		{
			get { return _object; }
			set 
			{
				_object       = value;
				_typeAccessor = TypeAccessor.GetAccessor(value.GetType());
			}
		}

		private TypeAccessor _typeAccessor;
		public  TypeAccessor  TypeAccessor
		{
			get { return _typeAccessor;  }
		}

		private IsNullHandler _isNull;
		public  IsNullHandler  IsNull
		{
			get { return _isNull;  }
			set { _isNull = value; }
		}

		public  bool IsValueNull
		{
			get { return _isNull(this);  }
		}

		private object _value;
		public  object  Value
		{
			get { return _value;  }
			set { _value = value; }
		}

		private MemberAccessor _memberAccessor;
		public  MemberAccessor  MemberAccessor
		{
			get { return _memberAccessor; }
			set 
			{
				_memberAccessor = value;
				_memberInfo     = value.MemberInfo;
			}
		}

		private MemberInfo _memberInfo;
		public  MemberInfo  MemberInfo
		{
			get { return _memberInfo;  }
		}
	}
}
