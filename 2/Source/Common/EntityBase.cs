using System;
using System.Collections;
using System.ComponentModel;

using BLToolkit.ComponentModel;
using BLToolkit.Mapping;
using BLToolkit.Validation;


namespace BLToolkit.Common
{
	[Serializable, Trimmable]
	public abstract class EntityBase : ICustomTypeDescriptor
	{
		#region ICustomTypeDescriptor Members

		[NonSerialized]
		private CustomTypeDescriptorImpl _typedescriptor;
		private CustomTypeDescriptorImpl  TypeDescriptor
		{
			get
			{
				if (_typedescriptor == null)
					_typedescriptor =  new CustomTypeDescriptorImpl(GetType());

				return _typedescriptor;
			}
		}

		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return TypeDescriptor.GetAttributes();
		}

		string ICustomTypeDescriptor.GetClassName()
		{
			return TypeDescriptor.GetClassName();
		}

		string ICustomTypeDescriptor.GetComponentName()
		{
			return TypeDescriptor.GetComponentName();
		}

		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return TypeDescriptor.GetConverter();
		}

		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent();
		}

		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty();
		}

		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return TypeDescriptor.GetEditor(editorBaseType);
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(attributes);
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return TypeDescriptor.GetEvents();
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(attributes);
		}

		private static Hashtable _hashDescriptors = new Hashtable();

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			PropertyDescriptorCollection col = (PropertyDescriptorCollection)_hashDescriptors[GetType()];

			if (col == null)
			{
				col = TypeDescriptor.GetProperties();

				_hashDescriptors.Add(GetType(), col);
			}

			return col;
		}

		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return TypeDescriptor.GetPropertyOwner(pd);
		}

		#endregion

		#region Validation

		public virtual void Validate()
		{
			Validator.Validate(this);
		}

		public virtual bool IsValid(string fieldName)
		{
			return Validator.IsValid(this, fieldName);
		}

		public virtual string[] GetErrorMessages(string fieldName)
		{
			return Validator.GetErrorMessages(this, fieldName);
		}

		#endregion
	}
}
