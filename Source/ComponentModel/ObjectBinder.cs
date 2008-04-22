using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing;
using System.Windows.Forms;

using BLToolkit.EditableObjects;
using BLToolkit.Reflection;

namespace BLToolkit.ComponentModel
{
	/// <summary>
	/// http://www.bltoolkit.net/Doc/ComponentModel/ObjectBinder.htm
	/// </summary>
	//[ComplexBindingProperties("DataSource", "DataMember")]
	[ComplexBindingProperties("DataSource")]
	[DefaultProperty("ItemType")]
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(ObjectBinder))]
	public class ObjectBinder : Component, ITypedList, IBindingListView, ICancelAddNew
	{
		#region Constructors

		static readonly EditableArrayList _empty = new EditableArrayList(typeof(object));

		public ObjectBinder()
		{
		}

		public ObjectBinder(IContainer container)
			: this()
		{
			if (container != null)
				container.Add(this);
		}

		#endregion

		#region Public members

		private object _dataSource;

		[AttributeProvider(typeof(IListSource))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(null)]
		[Category("Data")]
		public  object  DataSource
		{
			get { return _dataSource; }
			set
			{
				_dataSource = value;

				if      (value is Type)          ItemType   = (Type)value;
				else if (value is BindingSource) DataSource = ((BindingSource)value).DataSource;
				else if (value is IList)         List       = (IList)value;
				else if (value is IListSource)   List       = ((IListSource)value).GetList();
				else                             Object     = value;
			}
		}

		private Type _itemType;
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(null)]
		[Category("Data")]
		[TypeConverter(typeof(TypeTypeConverter))]
		[Editor(typeof(Design.TypeEditor), typeof(UITypeEditor))]
		public  Type  ItemType
		{
			get { return _itemType; }
			set
			{
				_itemType = value;

				OnListChanged(ListChangedType.PropertyDescriptorChanged, -1);

				List = null;
			}
		}

		private Type _objectViewType;
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(null)]
		[Category("Data")]
		[TypeConverter(typeof(TypeTypeConverter))]
		[Editor(typeof(Design.ObjectViewTypeEditor), typeof(UITypeEditor))]
		public  Type  ObjectViewType
		{
			get { return _objectViewType; }
			set
			{
				_objectViewType = value;

				OnListChanged(ListChangedType.PropertyDescriptorChanged, -1);
			}
		}

		private object _object;
		[Browsable(false)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object Object
		{
			get { return _object; }
			set
			{
				if (value == null)
				{
					List = null;
				}
				else
				{
					EditableArrayList list = new EditableArrayList(value.GetType(), 1);

					list.Add(value, false);

					List = list;
					_object = value;
				}
			}
		}

		private bool              _isListCreatedInternally = false;
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
					if (_list != _empty)
						_list.ListChanged -= ListChangedHandler;

					_list = _itemType == null? _empty: new EditableArrayList(_itemType);
					_isListCreatedInternally = true;
				}
				else
				{
					EditableArrayList list;

					if (value is EditableArrayList)
					{
						list = (EditableArrayList)value;

						_isListCreatedInternally = false;
					}
					else
					{
						if (value.Count != 0 && _itemType == null)
							list = EditableArrayList.Adapter(value);
						else
							list = EditableArrayList.Adapter(_itemType, value);

						_isListCreatedInternally = true;
					}

					if (_itemType == null)
					{
						_itemType = list.ItemType;
					}
					else
					{
						if (list.ItemType != _itemType && !list.ItemType.IsSubclassOf(_itemType))
							throw new ArgumentException(string.Format(
								"Item type {0} of the new list must be a subclass of {1}.",
								list.ItemType,
								_itemType));
					}

					if (_list != _empty)
					{
						_list.ListChanged -= ListChangedHandler;
						
						if (_disposeList || (_isListCreatedInternally && _disposeCreatedList))
							_list.Dispose();
					}

					_list = list;
				}

				if (_list != _empty)
					_list.ListChanged += ListChangedHandler;
				OnListChanged(ListChangedType.Reset, -1);
			}
		}

		private bool _disposeList = false;
		[DefaultValue(false)]
		[Category("Behavior")]
		[Description("Determines whether ObjectBinder will invoke underlying List's dispose when being itself disposed.")]
		public bool DisposeList
		{
			get { return _disposeList;  }
			set { _disposeList = value; }
		}

		private bool _disposeCreatedList = true;
		[DefaultValue(true)]
		[Category("Behavior")]
		[Description("Determines whether ObjectBinder will invoke underlying internally created List's dispose when being itself disposed")]
		public bool DisposeCreatedList
		{
			get { return _disposeCreatedList;  }
			set { _disposeCreatedList = value; }
		}

		private bool _allowNew = true;
		[DefaultValue(true)]
		[Category("Behavior")]
		[Description("Determines whether new items can be added to the list.")]
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

		private IsNullHandler _isNull;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public  IsNullHandler  IsNull
		{
			get { return _isNull;  }
			set { _isNull = value; }
		}

		bool IBindingList.AllowNew    { get { return AllowNew;    } }
		bool IBindingList.AllowEdit   { get { return AllowEdit;   } }
		bool IBindingList.AllowRemove { get { return AllowRemove; } }

		#endregion

		#region Protected Members

		protected virtual void OnListChanged(ListChangedEventArgs e)
		{
			if (ListChanged != null)
				ListChanged(this, e);
		}

		protected void OnListChanged(ListChangedType listChangedType, int newIndex)
		{
			OnListChanged(new ListChangedEventArgs(listChangedType, newIndex));
		}

		private void ListChangedHandler(object sender, ListChangedEventArgs e)
		{
			OnListChanged(e);
		}

		protected override void Dispose(bool disposing)
		{
			if (_list != _empty)
			{
				_list.ListChanged -= ListChangedHandler;

				if (_disposeList || (_isListCreatedInternally && _disposeCreatedList))
					_list.Dispose();
			}

			_list = _empty;
			
			base.Dispose(disposing);
		}

		#endregion

		#region ITypedList Members

		private static readonly Hashtable _descriptors = new Hashtable();

		PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			if (_itemType == null)
				return new PropertyDescriptorCollection(new PropertyDescriptor[0]);

			string key =
				_itemType + "." +
				(_objectViewType == null? string.Empty: _objectViewType.ToString()) + "." +
				(_isNull == null? "0": "1");

			if (listAccessors != null)
				foreach (PropertyDescriptor pd in listAccessors)
					key += "." + pd.Name;

			PropertyDescriptorCollection pdc = (PropertyDescriptorCollection)_descriptors[key];

			if (pdc == null)
			{
				pdc = _list.GetItemProperties(listAccessors, _objectViewType, _isNull, !DesignMode);

				if (!DesignMode)
					_descriptors[key] = pdc;
			}

			return pdc;
		}

		string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
		{
			return _list.GetListName(listAccessors);
		}

		#endregion

		#region IBindingListView Members

		bool IBindingListView.SupportsAdvancedSorting
		{
			get { return _list.SupportsAdvancedSorting; }
		}

		ListSortDescriptionCollection IBindingListView.SortDescriptions
		{
			get { return _list.SortDescriptions; }
		}

		void IBindingListView.ApplySort(ListSortDescriptionCollection sorts)
		{
			_list.ApplySort(sorts);
		}

		bool IBindingListView.SupportsFiltering
		{
			get { return _list.SupportsFiltering; }
		}

		string IBindingListView.Filter
		{
			get { return _list.Filter;  }
			set { _list.Filter = value; }
		}

		void IBindingListView.RemoveFilter()
		{
			_list.RemoveFilter();
		}

		#endregion

		#region ICancelAddNew Members

		void ICancelAddNew.CancelNew(int itemIndex)
		{
			_list.CancelNew(itemIndex);
		}

		void ICancelAddNew.EndNew(int itemIndex)
		{
			_list.EndNew(itemIndex);
		}

		#endregion

		#region IBindingList Members

		void IBindingList.AddIndex(PropertyDescriptor property)
		{
			_list.AddIndex(property);
		}

		object IBindingList.AddNew()
		{
			return _list.AddNew();
		}

		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			_list.ApplySort(property, direction);
		}

		int IBindingList.Find(PropertyDescriptor property, object key)
		{
			return _list.Find(property, key);
		}

		bool IBindingList.IsSorted
		{
			get { return _list.IsSorted; }
		}

		public event ListChangedEventHandler ListChanged;

		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{
			_list.RemoveIndex(property);
		}

		void IBindingList.RemoveSort()
		{
			_list.RemoveSort();
		}

		ListSortDirection IBindingList.SortDirection
		{
			get { return _list.SortDirection; }
		}

		PropertyDescriptor IBindingList.SortProperty
		{
			get { return _list.SortProperty; }
		}

		bool IBindingList.SupportsChangeNotification
		{
			get { return _list.SupportsChangeNotification; }
		}

		bool IBindingList.SupportsSearching
		{
			get { return _list.SupportsSearching; }
		}

		bool IBindingList.SupportsSorting
		{
			get { return _list.SupportsSorting; }
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
	}
}
