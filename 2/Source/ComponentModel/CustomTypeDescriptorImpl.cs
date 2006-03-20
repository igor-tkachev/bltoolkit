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
			_typeAccessor = TypeAccessor.GetAccessor(GetType());
		}

		public CustomTypeDescriptorImpl(Type type)
		{
			_typeAccessor = TypeAccessor.GetAccessor(type);
		}

		private TypeAccessor _typeAccessor;

		#endregion

		#region ICustomTypeDescriptor Members

		private         AttributeCollection   _attributes;
		public virtual AttributeCollection GetAttributes()
		{
			if (_attributes == null)
				_attributes = new AttributeCollection(
					(Attribute[])new TypeHelper(_typeAccessor.OriginalType).GetAttributes());

			return _attributes;
		}

		public virtual string GetClassName()
		{
			return _typeAccessor.OriginalType.Name;
		}

		public virtual string GetComponentName()
		{
			return _typeAccessor.OriginalType.Name;
		}

		private        TypeConverter   _converter;
		public virtual TypeConverter GetConverter()
		{
			if (_converter == null)
			{
				TypeConverterAttribute attr =
					GetAttributes()[typeof(TypeConverterAttribute)] as TypeConverterAttribute;

				if (attr != null && attr.ConverterTypeName != null && attr.ConverterTypeName.Length > 0)
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

		private        bool          _readDefaultEvent;
		private        EventDescriptor   _defaultEvent;
		public virtual EventDescriptor GetDefaultEvent()
		{
			if (_readDefaultEvent == false)
			{
				_readDefaultEvent = true;

				DefaultEventAttribute attr =
					GetAttributes()[typeof(DefaultEventAttribute)] as DefaultEventAttribute;

				if (attr != null && attr.Name != null && attr.Name.Length > 0)
					_defaultEvent = new CustomEventDescriptor(_typeAccessor.OriginalType.GetEvent(attr.Name));
			}

			return _defaultEvent;
		}

		private        bool             _readDefaultProperty;
		private        PropertyDescriptor   _defaultProperty;
		public virtual PropertyDescriptor GetDefaultProperty()
		{
			if (_readDefaultProperty == false)
			{
				_readDefaultProperty = true;

				DefaultPropertyAttribute attr =
					GetAttributes()[typeof(DefaultPropertyAttribute)] as DefaultPropertyAttribute;

				if (attr != null && attr.Name != null && attr.Name.Length > 0)
				{
					MemberAccessor ma = _typeAccessor[attr.Name];

					if (ma != null)
						_defaultProperty = ma.PropertyDescriptor;
				}
			}

			return _defaultProperty;
		}

		private Hashtable _editors;
		public virtual object GetEditor(Type editorBaseType)
		{
			if (_editors == null)
				_editors = new Hashtable();

			object editor = _editors[editorBaseType];

			if (editor == null)
			{
				if (_editors.Contains(editorBaseType))
					return null;

				foreach (Attribute attr in GetAttributes())
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

		private        EventDescriptorCollection   _events;
		public virtual EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			if (_events == null)
			{
				EventInfo[]       ei = _typeAccessor.OriginalType.GetEvents();
				EventDescriptor[] ed = new EventDescriptor[ei.Length];

				for (int i = 0; i < ei.Length; i++)
					ed[i] = new CustomEventDescriptor(ei[i]);

				_events = new EventDescriptorCollection(ed);
			}

			return _events;
		}

		public virtual EventDescriptorCollection GetEvents()
		{
			return GetEvents(null);
		}

		private        PropertyDescriptorCollection   _properties;
		public virtual PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			if (_properties == null)
				_properties = _typeAccessor.CreatePropertyDescriptors();

			return _properties;
		}

		public virtual PropertyDescriptorCollection GetProperties()
		{
			return GetProperties(null);
		}

		public virtual object GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}

		#endregion

		#region Protected Members

		private Type GetTypeByName(string typeName)
		{
			if (typeName == null || typeName.Length == 0)
				return null;

			Type type = Type.GetType(typeName);

			if (type != null)
				return type;

			int idx = typeName.IndexOf(',');

			if (idx != -1)
				typeName = typeName.Substring(0, idx);

			return _typeAccessor.OriginalType.Assembly.GetType(typeName);
		}

		private object CreateInstance(Type type)
		{
			ConstructorInfo ci = type.GetConstructor(new Type[]{ typeof(Type) });

			return ci != null?
				Activator.CreateInstance(type, new object[] { _typeAccessor.OriginalType }):
				Activator.CreateInstance(type);
		}

		#endregion

		#region CustomEventDescriptor

		class CustomEventDescriptor : EventDescriptor
		{
			public CustomEventDescriptor(EventInfo eventInfo)
				: base(eventInfo.Name, null)
			{
				_eventInfo = eventInfo;
			}

			private EventInfo _eventInfo;

			public override void AddEventHandler(object component, Delegate value)
			{
				_eventInfo.AddEventHandler(component, value);
			}

			public override void RemoveEventHandler(object component, Delegate value)
			{
				_eventInfo.RemoveEventHandler(component, value);
			}

			public override Type ComponentType { get { return _eventInfo.DeclaringType;    } }
			public override Type EventType     { get { return _eventInfo.EventHandlerType; } }
			public override bool IsMulticast   { get { return _eventInfo.IsMulticast;      } }
		}

		#endregion
	}
}
