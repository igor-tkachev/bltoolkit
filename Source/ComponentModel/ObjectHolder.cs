using System;
using System.ComponentModel;

using BLToolkit.Reflection;

namespace BLToolkit.ComponentModel
{
	public class ObjectHolder : ICustomTypeDescriptor
	{
		public ObjectHolder(object obj, ObjectBinder objectBinder)
		{
			_object             = obj;
			_originalProperties = ((ITypedList)objectBinder).GetItemProperties(null);
		}

		private readonly PropertyDescriptorCollection _originalProperties;
		private          PropertyDescriptorCollection _customProperties;

		private readonly object _object;
		public           object  Object
		{
			get { return _object; }
		}

		private ICustomTypeDescriptor _customTypeDescriptor;
		private ICustomTypeDescriptor  CustomTypeDescriptor
		{
			get
			{
				if (_customTypeDescriptor == null)
				{
					_customTypeDescriptor = _object is ICustomTypeDescriptor?
						(ICustomTypeDescriptor)_object:
						TypeAccessor.GetCustomTypeDescriptor(_object.GetType());
				}

				return _customTypeDescriptor;
			}
		}

		#region ICustomTypeDescriptor Members

		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return CustomTypeDescriptor.GetAttributes();
		}

		string ICustomTypeDescriptor.GetClassName()
		{
			return CustomTypeDescriptor.GetClassName();
		}

		string ICustomTypeDescriptor.GetComponentName()
		{
			return CustomTypeDescriptor.GetComponentName();
		}

		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return CustomTypeDescriptor.GetConverter();
		}

		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return CustomTypeDescriptor.GetDefaultEvent();
		}

		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return CustomTypeDescriptor.GetDefaultProperty();
		}

		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return CustomTypeDescriptor.GetEditor(editorBaseType);
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return CustomTypeDescriptor.GetEvents(attributes);
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return CustomTypeDescriptor.GetEvents();
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			if (_customProperties == null)
			{
				PropertyDescriptor[] properties = new PropertyDescriptor[_originalProperties.Count];

				for (int i = 0; i < properties.Length; i++)
				{
					properties[i] = new ObjectPropertyDescriptor(_originalProperties[i]);
				}

				_customProperties = new PropertyDescriptorCollection(properties);
			}

			return _customProperties;
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return ((ICustomTypeDescriptor)this).GetProperties(null);
		}

		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return CustomTypeDescriptor.GetPropertyOwner(pd);
		}

		#endregion

		#region ObjectPropertyDescriptor

		class ObjectPropertyDescriptor : PropertyDescriptorWrapper
		{
			public ObjectPropertyDescriptor(PropertyDescriptor pd)
				: base(pd)
			{
			}

			public override object GetValue(object component)
			{
				return base.GetValue(((ObjectHolder)component).Object);
			}

			public override void SetValue(object component, object value)
			{
				base.SetValue(((ObjectHolder)component).Object, value);
			}
		}

		#endregion
	}
}
