using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;

using BLToolkit.ComponentModel;
using BLToolkit.Mapping;
using BLToolkit.Validation;

namespace BLToolkit.Common
{
	[Serializable, Trimmable, ComVisible(true), JetBrains.Annotations.UsedImplicitly]
	public abstract class EntityBase : ICustomTypeDescriptor
	{
		#region Protected members

		protected virtual ICustomTypeDescriptor CreateTypeDescriptor()
		{
			return new CustomTypeDescriptorImpl(GetType());
		}

		#endregion

		#region ICustomTypeDescriptor Members

		private static readonly Hashtable _hashDescriptors = new Hashtable();

		[NonSerialized]
		private ICustomTypeDescriptor _typeDescriptor;
		private ICustomTypeDescriptor  TypeDescriptor
		{
			get
			{
				if (_typeDescriptor == null)
				{
					Type key = GetType();

					_typeDescriptor = (ICustomTypeDescriptor)_hashDescriptors[key];

					if (_typeDescriptor == null)
						_hashDescriptors[key] = _typeDescriptor = CreateTypeDescriptor();
				}

				return _typeDescriptor;
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

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return TypeDescriptor.GetProperties();
		}

		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			// Do not relay this call to TypeDescriptor. We are the owner.
			//
			return this;
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
