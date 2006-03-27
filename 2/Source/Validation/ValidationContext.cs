using System;
using System.ComponentModel;
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
			get { return _object;  }
			set { _object = value; }
		}

		private TypeAccessor _typeAccessor;
		public  TypeAccessor  TypeAccessor
		{
			get
			{
				if (_typeAccessor == null)
					_typeAccessor = TypeAccessor.GetAccessor(_object.GetType());

				return _typeAccessor;
			}
		}

		private PropertyDescriptor _propertyDescriptor;
		public  PropertyDescriptor  PropertyDescriptor
		{
			get { return _propertyDescriptor;  }
			set { _propertyDescriptor = value; }
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
				_memberInfo     = value != null? value.MemberInfo: null;
			}
		}

		private MemberInfo _memberInfo;
		public  MemberInfo  MemberInfo
		{
			get { return _memberInfo;  }
		}
	}
}
