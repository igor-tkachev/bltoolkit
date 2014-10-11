using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

using BLToolkit.Reflection;

namespace BLToolkit.ComponentModel
{
	public abstract class TypeDescriptorExtender : ICustomTypeDescriptor, ITypeDescriptionProvider
	{
		#region Constructors

		protected TypeDescriptorExtender(Type baseType)
		{
			if (baseType == null)
				throw new ArgumentNullException("baseType");

			_baseObject = TypeAccessor.CreateInstanceEx(baseType);
		}

		protected TypeDescriptorExtender(object baseObject)
		{
			if (baseObject == null)
				throw new ArgumentNullException("baseObject");

			_baseObject = baseObject;
		}

		#endregion

		#region Public Members

		private readonly object _baseObject;
		public           object  BaseObject
		{
			get { return _baseObject; }
		}

		#endregion

		#region Protected Members

		private static readonly Hashtable _hashDescriptors = new Hashtable();

		[NonSerialized]
		private ICustomTypeDescriptor _typeDescriptor;
		private ICustomTypeDescriptor  TypeDescriptor
		{
			get
			{
				if (_typeDescriptor == null)
				{
					Type key1 = GetType();
					Type key2 = _baseObject.GetType();

					Hashtable tbl = (Hashtable)_hashDescriptors[key1];

					if (tbl == null)
						_hashDescriptors[key1] = tbl = new Hashtable();

					_typeDescriptor = (ICustomTypeDescriptor)tbl[key2];

					if (_typeDescriptor == null)
						tbl[key2] = _typeDescriptor = CreateTypeDescriptor();
				}

				return _typeDescriptor;
			}
		}

		private ICustomTypeDescriptor CreateTypeDescriptor()
		{
			return new CustomTypeDescriptorImpl(this);
		}

		#endregion

		#region ICustomTypeDescriptor Members

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
			return TypeDescriptor.GetPropertyOwner(pd);
		}

		#endregion

		#region ITypeDescriptionProvider Members

		[NonSerialized]
		private ICustomTypeDescriptor _baseTypeDescriptor;
		private ICustomTypeDescriptor  BaseTypeDescriptor
		{
			get
			{
				if (_baseTypeDescriptor == null)
				{
					_baseTypeDescriptor = _baseObject as ICustomTypeDescriptor ??
						new CustomTypeDescriptorImpl(_baseObject.GetType());
				}

				return _baseTypeDescriptor;
			}
		}

		[NonSerialized]
		private ITypeDescriptionProvider _provider;
		private ITypeDescriptionProvider  Provider
		{
			get
			{
				if (_provider == null)
					_provider = TypeAccessor.GetAccessor(GetType());

				return _provider;
			}
		}

		Type   ITypeDescriptionProvider.OriginalType  { get { return GetType();      } }
		string ITypeDescriptionProvider.ClassName     { get { return GetType().Name; } }
		string ITypeDescriptionProvider.ComponentName { get { return GetType().Name; } }

		EventDescriptor ITypeDescriptionProvider.GetEvent(string name)
		{
			EventInfo ei = GetType().GetEvent(name);

			return ei != null ? Provider.GetEvent(name) : BaseTypeDescriptor.GetEvents()[name];
		}

		PropertyDescriptor ITypeDescriptionProvider.GetProperty(string name)
		{
			PropertyDescriptor pd = Provider.GetProperty(name);

			return pd ?? BaseTypeDescriptor.GetProperties()[name];
		}

		AttributeCollection ITypeDescriptionProvider.GetAttributes()
		{
			AttributeCollection col1 = Provider.GetAttributes();
			AttributeCollection col2 = BaseTypeDescriptor.GetAttributes();

			Attribute[] attrs = new Attribute[col1.Count + col2.Count];

			for (int i = 0; i < col1.Count; i++)
				attrs[i] = col1[i];

			for (int i = 0; i < col2.Count; i++)
				attrs[col1.Count + i] = col2[i];

			return new AttributeCollection(attrs);
		}

		EventDescriptorCollection ITypeDescriptionProvider.GetEvents()
		{
			EventDescriptorCollection col1 = Provider.GetEvents();
			EventDescriptorCollection col2 = BaseTypeDescriptor.GetEvents();

			EventDescriptorCollection col  = new EventDescriptorCollection(new EventDescriptor[0]);

			foreach (EventDescriptor ed in col1)
				col.Add(ed);

			foreach (EventDescriptor ed in col2)
				if (col.Find(ed.Name, false) == null)
					col.Add(ed);

			return col;
		}

		PropertyDescriptorCollection ITypeDescriptionProvider.GetProperties()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}
