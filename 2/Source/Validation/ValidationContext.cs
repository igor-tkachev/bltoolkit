using System;
using System.ComponentModel;
using System.Reflection;
using BLToolkit.Mapping;
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
			set
			{
				_value     = value;
				_nullValue = null;
			}
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

		private object _nullValue;
		public  object  NullValue
		{
			get
			{
				if (_nullValue == null)
				{
					if (_value == null)
						throw new InvalidOperationException("NullValue is undefined when Value == null");

					ObjectMapper om = Map.GetObjectMapper(Object.GetType());
					MemberMapper mm = om[MemberName, true];

					_nullValue =
						mm != null && mm.MapMemberInfo.Nullable && mm.MapMemberInfo.NullValue != null?
							mm.MapMemberInfo.NullValue:
							TypeAccessor.GetNullValue(Value.GetType());

					if (_nullValue == null)
						_nullValue = DBNull.Value;
				}

				return _nullValue;

			}
		}

		public string MemberName
		{
			get
			{
				if (_memberInfo != null)
					return _memberInfo.Name;

				if (_propertyDescriptor != null)
					return _propertyDescriptor.Name;

				return null;
			}
		}
	}
}
