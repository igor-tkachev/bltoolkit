using System;
using System.Reflection;
using System.ComponentModel;

using BLToolkit.ComponentModel;

namespace BLToolkit.Reflection
{
	public abstract class MemberAccessor
	{
		protected MemberAccessor(TypeAccessor typeAccessor, MemberInfo memberInfo)
		{
			_typeAccessor = typeAccessor;
			_memberInfo   = memberInfo;
		}

		private MemberInfo _memberInfo;
		public  MemberInfo  MemberInfo
		{
			get { return _memberInfo;  }
		}

		private TypeAccessor _typeAccessor;
		public  TypeAccessor  TypeAccessor
		{
			get { return _typeAccessor; }
		}

		private PropertyDescriptor _propertyDescriptor;
		public  PropertyDescriptor  PropertyDescriptor
		{
			get
			{
				if (_propertyDescriptor == null)
					_propertyDescriptor = new CustomPropertyDescriptor(_typeAccessor.OriginalType, Name);
				return _propertyDescriptor;
			}
		}

		public virtual bool HasGetter { get { return false; } }
		public virtual bool HasSetter { get { return false; } }

		public virtual object GetValue(object o)
		{
			return null;
		}

		public virtual void SetValue(object o, object value)
		{
		}

		public Type Type
		{
			get
			{
				return _memberInfo is PropertyInfo?
					((PropertyInfo)_memberInfo).PropertyType:
					((FieldInfo)   _memberInfo).FieldType;
			}
		}

		public string Name
		{
			get { return _memberInfo.Name; }
		}

		private Type _underlyingType;
		public  Type  UnderlyingType
		{
			get
			{
				if (_underlyingType == null)
					_underlyingType = TypeHelper.GetUnderlyingType(Type);

				return _underlyingType;
			}
		}

		public Attribute GetAttribute(Type attributeType)
		{
			object[] attrs = _memberInfo.GetCustomAttributes(attributeType, true);

			return attrs.Length > 0? (Attribute)attrs[0]: null;
		}

		public object[] GetAttributes(Type attributeType)
		{
			object[] attrs = _memberInfo.GetCustomAttributes(attributeType, true);

			return attrs.Length > 0? attrs: null;
		}

		public object[] GetAttributes()
		{
			object[] attrs = _memberInfo.GetCustomAttributes(true);

			return attrs.Length > 0? attrs: null;
		}

		public object[] GetTypeAttributes(Type attributeType)
		{
			return TypeHelper.GetAttributes(_typeAccessor.OriginalType, attributeType);
		}
	}
}
