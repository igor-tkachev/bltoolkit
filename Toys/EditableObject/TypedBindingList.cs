using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.ComponentModel.Design.Serialization;
using System.Reflection;

namespace Rsdn.Framework.EditableObject
{
	public class TypedBindingList : Component, ITypedList, IBindingList
	{
		#region Constructors

		static EditableArrayList _empty = new EditableArrayList(typeof(object));

		public TypedBindingList()
		{
		}

		#endregion

		#region Public members

		private TypeWrapper _type;
		[DefaultValue(null)]
		public TypeWrapper ItemType
		{
			get { return _type; }
			set
			{
				_type = value;
				List  = null;
			}
		}

		private EditableArrayList _list = _empty;
		[Browsable(false)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IList List
		{
			get { return _list; }
			set
			{
				if (value == null)
				{
					_list = _type == null || _type.Type == null?
						_empty: new EditableArrayList(_type.Type);
				}
				else
				{
					EditableArrayList list;

					if (value is EditableArrayList)
					{
						list = (EditableArrayList)value;
					}
					else if (value is ArrayList)
					{
						if (value.Count != 0 || _type == null || _type.Type == null)
							list = EditableArrayList.Adapter((ArrayList)value);
						else
							list = EditableArrayList.Adapter(_type.Type, (ArrayList)value);
					}
					else
					{
						if (value.Count != 0 && _type == null || _type.Type == null)
							list = EditableArrayList.Adapter(value);
						else
							list = EditableArrayList.Adapter(_type.Type, value);
					}

					if (_type == null || _type.Type == null)
					{
						_type = new TypeWrapper(list.ItemType);
					}
					else
					{
						if (list.ItemType.IsSubclassOf(_type.Type) == false)
							throw new ArgumentException(string.Format(
								"Item type (0) of the new list must be a subclass of {1}.",
								list.ItemType,
								_type.Type));
					}

					_list = list;
				}
			}
		}

		private bool _allowNew = true;
		[DefaultValue(true)]
		[Category("Behavior")]
		[Description("Determines whether the TypedBindingList allows new items to be added to the list.")]
		public  bool  AllowNew
		{
			get { return _allowNew && _list.AllowNew;  }
			set { _allowNew = value; }
		}

		private bool _allowEdit = true;
		[DefaultValue(true)]
		[Category("Behavior")]
		[Description("Determines whether items in the list can be edited.")]
		public  bool  AllowEdit
		{
			get { return _allowEdit && _list.AllowEdit; }
			set { _allowEdit = value; }
		}

		private bool _allowRemove = true;
		[DefaultValue(true)]
		[Category("Behavior")]
		[Description("Determines whether items can be removed from the list.")]
		public bool AllowRemove
		{
			get { return _allowRemove && _list.AllowRemove;  }
			set { _allowRemove = value; }
		}

		#endregion Public members

		#region ITypedList Members

		PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			return ((ITypedList)_list).GetItemProperties(listAccessors);
		}

		string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
		{
			return ((ITypedList)_list).GetListName(listAccessors);
		}

		#endregion

		#region IBindingList Members

		void IBindingList.AddIndex(PropertyDescriptor property)
		{
			((IBindingList)_list).AddIndex(property);
		}

		object IBindingList.AddNew()
		{
			return _list.AddNew();
		}

		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			((IBindingList)_list).ApplySort(property, direction);
		}

		int IBindingList.Find(PropertyDescriptor property, object key)
		{
			return ((IBindingList)_list).Find(property, key);
		}

		bool IBindingList.IsSorted
		{
			get { return ((IBindingList)_list).IsSorted; }
		}

		event ListChangedEventHandler IBindingList.ListChanged
		{
			add    { ((IBindingList)_list).ListChanged += value; }
			remove { ((IBindingList)_list).ListChanged -= value; }
		}

		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{
			((IBindingList)_list).RemoveIndex(property);
		}

		void IBindingList.RemoveSort()
		{
			((IBindingList)_list).RemoveSort();
		}

		ListSortDirection IBindingList.SortDirection
		{
			get { return ((IBindingList)_list).SortDirection; }
		}

		PropertyDescriptor IBindingList.SortProperty
		{
			get { return ((IBindingList)_list).SortProperty; }
		}

		bool IBindingList.SupportsChangeNotification
		{
			get { return ((IBindingList)_list).SupportsChangeNotification; }
		}

		bool IBindingList.SupportsSearching
		{
			get { return ((IBindingList)_list).SupportsSearching; }
		}

		bool IBindingList.SupportsSorting
		{
			get { return ((IBindingList)_list).SupportsSorting; }
		}

		#endregion

		#region IList Members

		int IList.Add(object value)
		{
			return _list.Add(value);
		}

		void IList.Clear()
		{
			_list.Clear();
		}

		bool IList.Contains(object value)
		{
			return _list.Contains(value);
		}

		int IList.IndexOf(object value)
		{
			return _list.IndexOf(value);
		}

		void IList.Insert(int index, object value)
		{
			_list.Insert(index, value);
		}

		bool IList.IsFixedSize
		{
			get { return _list.IsFixedSize; }
		}

		bool IList.IsReadOnly
		{
			get { return _list.IsReadOnly; }
		}

		void IList.Remove(object value)
		{
			_list.Remove(value);
		}

		void IList.RemoveAt(int index)
		{
			_list.RemoveAt(index);
		}

		object IList.this[int index]
		{
			get { return index == -1? null: _list[index];  }
			set { _list[index] = value; }
		}

		#endregion

		#region ICollection Members

		void ICollection.CopyTo(Array array, int index)
		{
			_list.CopyTo(array, index);
		}

		int ICollection.Count
		{
			get { return _list.Count; }
		}

		bool ICollection.IsSynchronized
		{
			get { return _list.IsSynchronized; }
		}

		object ICollection.SyncRoot
		{
			get { return _list.SyncRoot; }
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		#endregion

		#region TypeWrapper

		/// <summary>
		/// Following in the footsteps of Garry.
		/// </summary>
		[TypeConverter(typeof(TypeWrapperConverter))]
		public class TypeWrapper
		{
			private Type _type;

			public TypeWrapper(Type type)
			{
				if (type == null)
					throw new NullReferenceException("type");

				_type = type;
			}

			internal TypeWrapper(string typeName)
			{
				_type = Type.GetType(typeName, true);
			}

			public Type Type
			{
				get { return _type; }
			}
		}

		#endregion ExTypeWrapper

		#region TypeWrapperConverter

		/// <summary>
		/// Following in the footsteps of Garry.
		/// </summary>
		public class TypeWrapperConverter : TypeConverter
		{
			public TypeWrapperConverter()
			{
			}

			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				if (sourceType == typeof(string))
					return true;

				return base.CanConvertFrom(context, sourceType);
			}

			public override object ConvertFrom(
				ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				if (value is string)
				{
					try
					{
						return new TypeWrapper(value.ToString());
					}
					catch (Exception)
					{
						return null;
					}
				}

				return base.ConvertFrom(context, culture, value);
			}

			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(InstanceDescriptor))
					return true;

				if (destinationType == typeof(string))
					return true;

				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(
				ITypeDescriptorContext context, 
				CultureInfo            culture,
				object                 value,
				Type                   destinationType)
			{
				if (destinationType == typeof(InstanceDescriptor))
				{
					ConstructorInfo ci = 
						typeof(TypeWrapper).GetConstructor(new Type[] { typeof(Type) });

					return new InstanceDescriptor(ci, new Type[] { ((TypeWrapper)value).Type });
				}

				if (destinationType == typeof(string))
				{
					if (value == null)
						return "(none)";

					return ((TypeWrapper)value).Type.ToString();
				}

				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		#endregion ExTypeWrapperConverter
	}
}
