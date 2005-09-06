using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.EditableObject
{
	[Serializable, ComVisible(true)]
	public class EditableArrayList :
		ArrayList, IBindingList, ITypedList, ISupportInitialize, 
#if VER2
		//IBindingListView,
#endif
		IEditable
	{
		#region Public Members

		public EditableArrayList(Type itemType)
			: this(itemType, new ArrayList())
		{
		}

		public EditableArrayList(Type itemType, int capacity)
			: this(itemType, new ArrayList(capacity))
		{
		}

		public EditableArrayList(Type itemType, ICollection c)
			: this(itemType, new ArrayList(c))
		{
		}

		private  ArrayList _list;
		internal ArrayList  List
		{
			get { return _list; }
		}

		private Type _itemType;
		public  Type  ItemType
		{
			get { return _itemType; }
		}

		#endregion

		#region Protected Members

		private ArrayList _newItems;
		private ArrayList  NewItems
		{
			get
			{
				if (_newItems == null)
					_newItems = new ArrayList();
				return _newItems;
			}
		}

		private ArrayList _delItems;
		private ArrayList  DelItems
		{
			get
			{
				if (_delItems == null)
					_delItems = new ArrayList();
				return _delItems;
			}
		}

		public virtual object Clone(EditableArrayList el)
		{
			if (_newItems != null) el._newItems = (ArrayList)_newItems.Clone();
			if (_delItems != null) el._delItems = (ArrayList)_delItems.Clone();

			el._trackingChanges = _trackingChanges;

			el.AddInternal(el);

			return el;
		}

		#endregion

		#region Change Notification

		private bool _isChanged;

		protected virtual void OnListChanged(ListChangedEventArgs e)
		{
			_isSorted = false;

			if (IsTrackingChanges)
			{
				_isChanged = true;

				if (_listChanged != null)
					_listChanged(this, e);
			}
		}

		protected void OnListChanged(ListChangedType listChangedType, int newIndex)
		{
			_isSorted = false;

			if (IsTrackingChanges)
				OnListChanged(new ListChangedEventArgs(listChangedType, newIndex));
		}

		private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, _list.IndexOf(sender)));
		}

		#endregion

		#region Add/Remove Internal

		private void AddInternal(object value)
		{
			if (IsTrackingChanges)
			{
				if (DelItems.Contains(value))
					DelItems.Remove(value);
				else
					NewItems.Add(value);
			}

			if (value is INotifyPropertyChanged)
				((INotifyPropertyChanged)value).PropertyChanged += 
					new PropertyChangedEventHandler(ItemPropertyChanged);
		}

		private void RemoveInternal(object value)
		{
			if (IsTrackingChanges)
			{
				if (NewItems.Contains(value))
					NewItems.Remove(value);
				else
					DelItems.Add(value);
			}

			if (value is INotifyPropertyChanged)
				((INotifyPropertyChanged)value).PropertyChanged -=
					new PropertyChangedEventHandler(ItemPropertyChanged);
		}

		private void AddInternal(IEnumerable e)
		{
			foreach (object o in e)
				AddInternal(o);
		}

		private void RemoveInternal(IEnumerable e)
		{
			foreach (object o in e)
				RemoveInternal(o);
		}

		#endregion

		#region ISupportInitialize Members

		private int   _trackingChanges;
		private bool IsTrackingChanges
		{
			get { return _trackingChanges == 0; }
		}

		public void BeginInit()
		{
			_trackingChanges++;
		}

		public void EndInit()
		{
			if (_trackingChanges == 1)
				AcceptChanges();

			_trackingChanges--;
		}

		#endregion

		#region IEditable Members

		public virtual void AcceptChanges()
		{
			if (_list != null)
				foreach (object o in _list)
					if (o is IEditable)
						((IEditable)o).AcceptChanges();

			_isChanged = false;
			_newItems  = null;
			_delItems  = null;
		}

		public virtual void RejectChanges()
		{
			BeginInit();

			if (_delItems != null)
				foreach (object o in _delItems)
					Add(o);

			if (_newItems != null)
				foreach (object o in _newItems)
					Remove(o);

			if (_list != null)
				foreach (object o in _list)
					if (o is IEditable)
						((IEditable)o).RejectChanges();

			EndInit();

			_isSorted  = false;
			_isChanged = false;
			_newItems  = null;
			_delItems  = null;
		}

		public virtual bool IsDirty
		{
			get
			{
				if (_isChanged ||
					_newItems != null && _newItems.Count > 0 ||
					_delItems != null && _delItems.Count > 0)
					return true;

				if (_list != null)
					foreach (object o in _list)
						if (o is IEditable)
							if (((IEditable)o).IsDirty)
								return true;

				return false;
			}
		}

		#endregion

		#region IBindingList Members

			#region Command

		public virtual object AddNew()
		{
			if (AllowNew == false)
				throw new InvalidOperationException();

			object o = Map.CreateInstance(_itemType);

			Add(o);

			return o;
		}

		public virtual bool AllowNew
		{
			get { return !_list.IsFixedSize; }
		}

		public virtual bool AllowEdit
		{
			get { return !_list.IsReadOnly; }
		}

		public virtual bool AllowRemove
		{
			get { return !_list.IsFixedSize; }
		}

			#endregion

			#region Change Notification

		bool IBindingList.SupportsChangeNotification
		{
			get { return true; }
		}

		private ListChangedEventHandler _listChanged;
		event   ListChangedEventHandler IBindingList.ListChanged
		{
			add    { _listChanged += value; }
			remove { _listChanged -= value; }
		}

			#endregion

			#region Sorting

		bool IBindingList.SupportsSorting
		{
			get { return true; }
		}

		[NonSerialized]
		bool             _isSorted;
		bool IBindingList.IsSorted
		{
			get { return _isSorted; }
		}

		[NonSerialized]
		PropertyDescriptor             _sortProperty;
		PropertyDescriptor IBindingList.SortProperty
		{
			get { return _sortProperty; }
		}

		[NonSerialized]
		ListSortDirection             _sortDirection;
		ListSortDirection IBindingList.SortDirection
		{
			get { return _sortDirection; }
		}

		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			Sort(new SortPropertyComparer(property, direction));

			_isSorted      = true;
			_sortProperty  = property;
			_sortDirection = direction;
		}

		void IBindingList.RemoveSort()
		{
			_isSorted     = false;
			_sortProperty = null;
		}

			#endregion

			#region Searching

		bool IBindingList.SupportsSearching
		{
			get { return true; }
		}

		int IBindingList.Find(PropertyDescriptor property, object key)
		{
			if (key != null)
				for (int i = 0; i < List.Count; i++)
					if (key.Equals(property.GetValue(List[i])))
						return i;

			return -1;
		}

			#endregion

			#region Indexes

		void IBindingList.AddIndex(PropertyDescriptor property)
		{
		}

		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{
		}

			#endregion

		#endregion

		#region ITypedList Members

		PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(ItemType);

			ArrayList     list      = new ArrayList(pdc.Count);
			bool          isDataRow = ItemType.IsSubclassOf(typeof(DataRow));
			MapDescriptor md        = null;

			foreach (PropertyDescriptor p in pdc)
			{
				if (p.Attributes.Matches(BindableAttribute.No))
					continue;

				if (isDataRow && p.Name == "ItemArray")
					continue;

				PropertyDescriptor pd = null;

				if (p.PropertyType.GetInterface("IList") != null)
				{
					pd = new IListPropertyDescriptor(p, this);
				}
				else
				{
					if (isDataRow == false)
					{
						if (md == null)
							md = Map.Descriptor(ItemType);

						foreach (IMemberMapper mm in md)
						{
							if (mm.OriginalName == p.Name)
							{
								pd = new MapPropertyDescriptor(p, mm);
								break;
							}
						}
					}
				}

				if (pd == null)
					pd = new StandardPropertyDescriptor(pd);

				list.Add(pd);
			}

			list.Sort(new PropertyDescriptorComparer());

			return new PropertyDescriptorCollection(
				(PropertyDescriptor[])list.ToArray(typeof(PropertyDescriptor)));
		}

		string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
		{
			string name = ItemType.Name;

			if (listAccessors != null)
				foreach (PropertyDescriptor pd in listAccessors)
					name += "_" + pd.Name;

			return name;
		}

		#endregion

		#region Overridden Methods

		public override int Add(object value)
		{
			int idx = _list.Add(value);

			AddInternal(value);
			OnListChanged(ListChangedType.ItemAdded, idx);

			return idx;
		}

		public override void AddRange(ICollection c)
		{
			int idx = Count;

			_list.AddRange(c);

			AddInternal(c);
			OnListChanged(ListChangedType.ItemAdded, Count);
		}

		public override int BinarySearch(int index, int count, object value, IComparer comparer)
		{
			return _list.BinarySearch(index, count, value, comparer);
		}

		public override int BinarySearch(object value)
		{
			return _list.BinarySearch(value);
		}

		public override int BinarySearch(object value, IComparer comparer)
		{
			return _list.BinarySearch(value, comparer);
		}

		public override int Capacity
		{
			get { return _list.Capacity;  }
			set { _list.Capacity = value; }
		}

		public override void Clear()
		{
			_list.Clear();
		}

		public override object Clone()
		{
			return Clone(new EditableArrayList(ItemType, (ArrayList)_list.Clone()));
		}

		public override bool Contains(object item)
		{
			return _list.Contains(item);
		}

		public override void CopyTo(int index, Array array, int arrayIndex, int count)
		{
			_list.CopyTo(index, array, arrayIndex, count);
		}

		public override void CopyTo(Array array)
		{
			_list.Add(array);
		}

		public override void CopyTo(Array array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		public override int Count
		{
			get { return _list.Count; }
		}

		public override bool Equals(object obj)
		{
			return _list.Equals(obj);
		}

		public override IEnumerator GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		public override IEnumerator GetEnumerator(int index, int count)
		{
			return _list.GetEnumerator(index, count);
		}

		public override int GetHashCode()
		{
			return _list.GetHashCode();
		}

		public override ArrayList GetRange(int index, int count)
		{
			return _list.GetRange(index, count);
		}

		public override int IndexOf(object value)
		{
			return _list.IndexOf(value);
		}

		public override int IndexOf(object value, int startIndex)
		{
			return _list.IndexOf(value, startIndex);
		}

		public override int IndexOf(object value, int startIndex, int count)
		{
			return _list.IndexOf(value, startIndex, count);
		}

		public override void Insert(int index, object value)
		{
			_list.Insert(index, value);

			AddInternal(value);
			OnListChanged(ListChangedType.ItemAdded, index);
		}

		public override void InsertRange(int index, ICollection c)
		{
			_list.InsertRange(index, c);

			if (c.Count > 0)
			{
				AddInternal(c);
				OnListChanged(ListChangedType.ItemAdded, index);
			}
		}

		public override bool IsFixedSize
		{
			get { return _list.IsFixedSize; }
		}

		public override bool IsReadOnly
		{
			get { return _list.IsReadOnly; }
		}

		public override bool IsSynchronized
		{
			get { return _list.IsSynchronized; }
		}

		public override int LastIndexOf(object value)
		{
			return _list.LastIndexOf(value);
		}

		public override int LastIndexOf(object value, int startIndex)
		{
			return _list.LastIndexOf(value, startIndex);
		}

		public override int LastIndexOf(object value, int startIndex, int count)
		{
			return _list.LastIndexOf(value, startIndex, count);
		}

		public override void Remove(object obj)
		{
			int n = IndexOf(obj);

			if (n >= 0)
				RemoveInternal(obj);

			_list.Remove(obj);

			if (n >= 0)
				OnListChanged(ListChangedType.ItemDeleted, n);
		}

		public override void RemoveAt(int index)
		{
			object o = this[index];

			RemoveInternal(o);

			_list.RemoveAt(index);
			
			OnListChanged(ListChangedType.ItemDeleted, index);
		}

		public override void RemoveRange(int index, int count)
		{
			for (int i = index; i < _list.Count && i < index + count; i++)
				RemoveInternal(_list[i]);

			_list.RemoveRange(index, count);

			OnListChanged(ListChangedType.ItemDeleted, index);
		}

		public override void Reverse()
		{
			_list.Reverse();

			if (_list.Count > 1)
				OnListChanged(ListChangedType.ItemMoved, 0);
		}

		public override void Reverse(int index, int count)
		{
			_list.Reverse(index, count);

			if (count > 1)
				OnListChanged(ListChangedType.ItemMoved, index);
		}

		public override void SetRange(int index, ICollection c)
		{
			_list.SetRange(index, c);

			if (_list.Count > 1)
			{
				AddInternal(c);
				OnListChanged(ListChangedType.ItemAdded, index);
			}
		}

		public override void Sort()
		{
			_list.Sort();

			if (_list.Count > 1)
				OnListChanged(ListChangedType.ItemMoved, 0);
		}

		public override void Sort(int index, int count, IComparer comparer)
		{
			_list.Sort(index, count, comparer);

			if (count > 1)
				OnListChanged(ListChangedType.ItemMoved, index);
		}

		public override void Sort(IComparer comparer)
		{
			_list.Sort(comparer);

			if (_list.Count > 1)
				OnListChanged(ListChangedType.ItemMoved, 0);
		}

		public override object SyncRoot
		{
			get { return _list.SyncRoot; }
		}

		public override object this[int index]
		{
			get { return _list[index];  }
			set
			{
				object o = _list[index];

				if (o != value)
				{
					RemoveInternal(o);

					_list[index] = value;

					AddInternal(value);
					
					OnListChanged(ListChangedType.ItemChanged, index);
				}
			}
		}

		public override object[] ToArray()
		{
			return _list.ToArray();
		}

		public override Array ToArray(Type type)
		{
			return _list.ToArray(type);
		}

		public override string ToString()
		{
			return _list.ToString();
		}

		public override void TrimToSize()
		{
			_list.TrimToSize();
		}

		#endregion

		#region Static Methods

		internal EditableArrayList(Type itemType, ArrayList list)
		{
			if (itemType == null) throw new ArgumentNullException("itemType");
			if (list     == null) throw new ArgumentNullException("list");

			_itemType = itemType;
			_list     = list;

			AddInternal(_list);
		}

		public static EditableArrayList Adapter(Type itemType, ArrayList list)
		{
			return new EditableArrayList(itemType, list);
		}

		public static EditableArrayList Adapter(Type itemType, IList list)
		{
			return new EditableArrayList(itemType, ArrayList.Adapter(list));
		}

		public static EditableArrayList Adapter(ArrayList list)
		{
			if (list == null) throw new ArgumentNullException("list");

			PropertyInfo pi = list.GetType().GetProperty("Item", new Type[] { typeof(int) });
			Type         it = pi == null? typeof(object): pi.PropertyType;

			return new EditableArrayList(it, list);
		}

		public static new EditableArrayList Adapter(IList list)
		{
			if (list == null) throw new ArgumentNullException("list");

			PropertyInfo pi = list.GetType().GetProperty("Item", new Type[] { typeof(int) });
			Type         it = pi == null? typeof(object): pi.PropertyType;

			return new EditableArrayList(it, ArrayList.Adapter(list));
		}

		#endregion

		#region PropertyDescriptorComparer

		class PropertyDescriptorComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				return String.Compare(((PropertyDescriptor)x).Name, ((PropertyDescriptor)y).Name);
			}
		}

		#endregion PropertyDescriptorComparer

		#region StandardPropertyDescriptor

		class StandardPropertyDescriptor : PropertyDescriptor
		{
			protected PropertyDescriptor _descriptor = null;

			public StandardPropertyDescriptor(PropertyDescriptor pd)
				: base(pd)
			{
				_descriptor = pd;
			}

			public override void SetValue(object component, object value)
			{
				_descriptor.SetValue(component, value);
			}

			public override object GetValue(object component)
			{
				return _descriptor.GetValue(component);
			}

			public override Type ComponentType
			{
				get { return _descriptor.ComponentType; }
			}

			public override bool IsReadOnly
			{
				get { return _descriptor.IsReadOnly; }
			}

			public override Type PropertyType
			{
				get { return _descriptor.PropertyType; }
			}

			public override bool CanResetValue(object component)
			{
				return _descriptor.CanResetValue(component);
			}

			public override bool ShouldSerializeValue(object component)
			{
				return _descriptor.ShouldSerializeValue(component);
			}

			public override void ResetValue(object component)
			{
				_descriptor.ResetValue(component);
			}

		}

		#endregion

		#region StandardPropertyDescriptor

		class MapPropertyDescriptor : StandardPropertyDescriptor
		{
			private IMemberMapper _memberMapper;

			public MapPropertyDescriptor(PropertyDescriptor pd, IMemberMapper mm)
				: base(pd)
			{
				_memberMapper = mm;
			}

			public override void SetValue(object component, object value)
			{
				_memberMapper.SetValue(component, value);
			}

			public override object GetValue(object component)
			{
				return _memberMapper.GetValue(component);
			}
		}

		#endregion

		#region IListPropertyDescriptor

		class IListPropertyDescriptor : StandardPropertyDescriptor
		{
			private EditableArrayList _parent;

			public IListPropertyDescriptor(PropertyDescriptor descriptor, EditableArrayList parent)
				: base(descriptor)
			{
				_parent = parent;
			}

			public override object GetValue(object component)
			{
				object value = base.GetValue(component);

				if (value == null)
					return value;

				if (value is IBindingList && value is ITypedList)
					return value;

				PropertyInfo pi = value.GetType().GetProperty("Item", new Type[] { typeof(int) });
				
				Type  itemType = pi == null? typeof(object): pi.PropertyType;
				IList list = (IList)value;

				if (value is ArrayList)
					return EditableArrayList.Adapter(itemType, (ArrayList)list);

				return EditableArrayList.Adapter(itemType, list);
			}
		}

		#endregion

		#region SortPropertyComparer

		class SortPropertyComparer : IComparer
		{
			PropertyDescriptor _property;
			ListSortDirection  _direction;

			public SortPropertyComparer(PropertyDescriptor property, ListSortDirection direction)
			{
				_property  = property;
				_direction = direction;
			}

			public int Compare(object x, object y)
			{
				object a = _property.GetValue(x);
				object b = _property.GetValue(y);

				int n = Comparer.Default.Compare(a, b);

				return _direction == ListSortDirection.Ascending? n: -n;
			}
		}

		#endregion
	}
}
