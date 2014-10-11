using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

using BLToolkit.Reflection;

namespace BLToolkit.ComponentModel
{
	public class CustomTypeDescriptorImpl : ICustomTypeDescriptor
	{
		#region Constructors

		public CustomTypeDescriptorImpl()
		{
			_typeDescriptionProvider = CreateTypeDescriptionProvider(GetType());
		}

		public CustomTypeDescriptorImpl(Type type)
		{
			_typeDescriptionProvider = CreateTypeDescriptionProvider(type);
		}

		public CustomTypeDescriptorImpl(ITypeDescriptionProvider typeDescriptionProvider)
		{
			_typeDescriptionProvider = typeDescriptionProvider;
		}

		private readonly ITypeDescriptionProvider _typeDescriptionProvider;

		protected virtual ITypeDescriptionProvider CreateTypeDescriptionProvider(Type type)
		{
			return TypeAccessor.GetAccessor(type);
		}

		#endregion

		#region ICustomTypeDescriptor Members

		AttributeCollection                      _attributes;
		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			if (_attributes == null)
				_attributes = _typeDescriptionProvider.GetAttributes();

			return _attributes;
		}

		string ICustomTypeDescriptor.GetClassName()
		{
			return _typeDescriptionProvider.ClassName;
		}

		string ICustomTypeDescriptor.GetComponentName()
		{
			return _typeDescriptionProvider.ComponentName;
		}

		TypeConverter                         _converter;
		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			if (_converter == null)
			{
				TypeConverterAttribute attr =
					_td.GetAttributes()[typeof(TypeConverterAttribute)] as TypeConverterAttribute;

				if (attr != null && !string.IsNullOrEmpty(attr.ConverterTypeName))
				{
					Type type = GetTypeByName(attr.ConverterTypeName);

					if (type != null && typeof(TypeConverter).IsAssignableFrom(type))
						_converter = (TypeConverter)CreateInstance(type);
				}

				if (_converter == null)
					_converter = new TypeConverter();
			}

			return _converter;
		}

		bool                                _readDefaultEvent;
		EventDescriptor                         _defaultEvent;
		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			if (_readDefaultEvent == false)
			{
				_readDefaultEvent = true;

				DefaultEventAttribute attr =
					_td.GetAttributes()[typeof(DefaultEventAttribute)] as DefaultEventAttribute;

				if (attr != null && !string.IsNullOrEmpty(attr.Name))
					_defaultEvent = _typeDescriptionProvider.GetEvent(attr.Name);
			}

			return _defaultEvent;
		}

		bool                                   _readDefaultProperty;
		PropertyDescriptor                         _defaultProperty;
		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			if (_readDefaultProperty == false)
			{
				_readDefaultProperty = true;

				DefaultPropertyAttribute attr =
					_td.GetAttributes()[typeof(DefaultPropertyAttribute)] as DefaultPropertyAttribute;

				if (attr != null && !string.IsNullOrEmpty(attr.Name))
					_defaultProperty = _typeDescriptionProvider.GetProperty(attr.Name);
			}

			return _defaultProperty;
		}

		Hashtable _editors;
		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			if (_editors == null)
				_editors = new Hashtable();

			object editor = _editors[editorBaseType];

			if (editor == null)
			{
				if (_editors.Contains(editorBaseType))
					return null;

				foreach (Attribute attr in _td.GetAttributes())
				{
					if (attr is EditorAttribute)
					{
						EditorAttribute ea = (EditorAttribute)attr;

						if (ea.EditorBaseTypeName != null &&
							ea.EditorTypeName     != null &&
							editorBaseType == GetTypeByName(ea.EditorBaseTypeName))
						{
							Type type = GetTypeByName(ea.EditorTypeName);

							if (type != null)
							{
								editor = CreateInstance(type);
								break;
							}
						}
					}
				}

				_editors[editorBaseType] = editor;
			}

			return editor;
		}

		EventDescriptorCollection                         _events;
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			if (_events == null)
				_events = _typeDescriptionProvider.GetEvents();

			return _events;
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return _td.GetEvents(null);
		}

		PropertyDescriptorCollection                         _properties;
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			if (_properties == null)
				_properties = _typeDescriptionProvider.GetProperties();

			return _properties;
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return _td.GetProperties(null);
		}

		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}

		#endregion

		#region Protected Members

		private Type GetTypeByName(string typeName)
		{
			if (string.IsNullOrEmpty(typeName))
				return null;

			Type type = Type.GetType(typeName);

			if (type != null)
				return type;

			int idx = typeName.IndexOf(',');

			if (idx != -1)
				typeName = typeName.Substring(0, idx);

			return _typeDescriptionProvider.OriginalType.Assembly.GetType(typeName);
		}

		private object CreateInstance(Type type)
		{
			ConstructorInfo ci = type.GetConstructor(new Type[]{ typeof(Type) });

			return ci != null?
				Activator.CreateInstance(type, new object[] { _typeDescriptionProvider.OriginalType }):
				Activator.CreateInstance(type);
		}

		private ICustomTypeDescriptor _td
		{
			get { return this; }
		}

		#endregion
	}
}
