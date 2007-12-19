using System;
using System.ComponentModel;

using BLToolkit.Reflection;

namespace BLToolkit.ComponentModel
{
	public class MemberPropertyDescriptor : PropertyDescriptor
	{
		public MemberPropertyDescriptor(Type componentType, string memberName)
			: base(memberName, null)
		{
			_componentType  = componentType;
			_memberAccessor = TypeAccessor.GetAccessor(componentType)[memberName];
		}

		private readonly Type _componentType;
		public  override Type  ComponentType
		{
			get { return _componentType; }
		}

		public override Type PropertyType
		{
			get { return _memberAccessor.Type; }
		}

		private readonly MemberAccessor _memberAccessor;
		public           MemberAccessor  MemberAccessor
		{
			get { return _memberAccessor; }
		}

		public override bool CanResetValue(object component)
		{
			if (PropertyType.IsValueType)
				return TypeAccessor.GetNullValue(PropertyType) != null;
			else
				return PropertyType == typeof(string);
		}

		public override void ResetValue(object component)
		{
			SetValue(component, TypeAccessor.GetNullValue(PropertyType));
		}

		public override object GetValue(object component)
		{
			return component != null? _memberAccessor.GetValue(component): null;
		}

		public override void SetValue(object component, object value)
		{
			if (component != null)
				_memberAccessor.SetValue(component, value);
		}

		public override bool IsReadOnly
		{
			get { return !_memberAccessor.HasSetter; }
		}

		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		private         AttributeCollection _attributes;
		public override AttributeCollection  Attributes
		{
			get
			{
				if (_attributes == null)
				{
					object[]    memberAttrs = _memberAccessor.GetAttributes();
					Attribute[] attrs       = new Attribute[memberAttrs == null? 0: memberAttrs.Length];

					if (memberAttrs != null)
						memberAttrs.CopyTo(attrs, 0);

					_attributes = new AttributeCollection(attrs);
				}

				return _attributes;
			}
		}
	}
}
