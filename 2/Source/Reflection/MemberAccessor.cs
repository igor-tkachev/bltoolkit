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
			get { return _memberInfo; }
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
					_propertyDescriptor = new MemberPropertyDescriptor(_typeAccessor.OriginalType, Name);

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

		[CLSCompliant(false)]
		public virtual SByte   GetSByte  (object o) { return (SByte)  GetValue(o); }
		public virtual Int16   GetInt16  (object o) { return (Int16)  GetValue(o); }
		public virtual Int32   GetInt32  (object o) { return (Int32)  GetValue(o); }
		public virtual Int64   GetInt64  (object o) { return (Int64)  GetValue(o); }

		public virtual Byte    GetByte   (object o) { return (Byte)   GetValue(o); }
		[CLSCompliant(false)]
		public virtual UInt16  GetUInt16 (object o) { return (UInt16) GetValue(o); }
		[CLSCompliant(false)]
		public virtual UInt32  GetUInt32 (object o) { return (UInt32) GetValue(o); }
		[CLSCompliant(false)]
		public virtual UInt64  GetUInt64 (object o) { return (UInt64) GetValue(o); }

		public virtual Boolean GetBoolean(object o) { return (Boolean)GetValue(o); }
		public virtual Char    GetChar   (object o) { return (Char)   GetValue(o); }
		public virtual Single  GetSingle (object o) { return (Single) GetValue(o); }
		public virtual Double  GetDouble (object o) { return (Double) GetValue(o); }
		public virtual Decimal GetDecimal(object o) { return (Decimal)GetValue(o); }

		[CLSCompliant(false)]
		public virtual void    SetSByte  (object o, SByte   value) { SetValue(o, value); }
		public virtual void    SetInt16  (object o, Int16   value) { SetValue(o, value); }
		public virtual void    SetInt32  (object o, Int32   value) { SetValue(o, value); }
		public virtual void    SetInt64  (object o, Int64   value) { SetValue(o, value); }

		public virtual void    SetByte   (object o, Byte    value) { SetValue(o, value); }
		[CLSCompliant(false)]
		public virtual void    SetUInt16 (object o, UInt16  value) { SetValue(o, value); }
		[CLSCompliant(false)]
		public virtual void    SetUInt32 (object o, UInt32  value) { SetValue(o, value); }
		[CLSCompliant(false)]
		public virtual void    SetUInt64 (object o, UInt64  value) { SetValue(o, value); }

		public virtual void    SetBoolean(object o, Boolean value) { SetValue(o, value); }
		public virtual void    SetChar   (object o, Char    value) { SetValue(o, value); }
		public virtual void    SetSingle (object o, Single  value) { SetValue(o, value); }
		public virtual void    SetDouble (object o, Double  value) { SetValue(o, value); }
		public virtual void    SetDecimal(object o, Decimal value) { SetValue(o, value); }

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
