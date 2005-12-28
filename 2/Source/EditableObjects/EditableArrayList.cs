using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

using BLToolkit.Reflection;
using BLToolkit.Mapping;
using BLToolkit.ComponentModel;

namespace BLToolkit.EditableObjects
{
#if FW2
	[DebuggerDisplay("Count = {Count}, ItemType = {ItemType}")]
#endif
	[Serializable]
	public class EditableArrayList :
		ArrayList, IEditable, ISortable, ISupportMapping, IDisposable, IPrintDebugState, ITypedList,
#if FW2
		IBindingListView, ICancelAddNew
#else
		IBindingList
#endif
	{
		#region Constructors

		public EditableArrayList(Type itemType, ArrayList list)
		{
			if (itemType == null) throw new ArgumentNullException("itemType");
			if (list     == null) throw new ArgumentNullException("list");

			_itemType        = itemType;
			_list            = list;
			_typedListImpl   = new TypedListImpl(itemType);
			_bindingListImpl = new BindingListImpl(this, itemType);

			AddInternal(_list);
		}

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

		#endregion

		#region Public Members

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

		private ArrayList _newItems;
		public  ArrayList  NewItems
		{
			get
			{
				if (_newItems == null)
					_newItems = new ArrayList();
				return _newItems;
			}
		}

		private ArrayList _delItems;
		public  ArrayList  DelItems
		{
			get
			{
				if (_delItems == null)
					_delItems = new ArrayList();
				return _delItems;
			}
		}

		public void Sort(params string[] memberNames)
		{
			Sort(ListSortDirection.Ascending, memberNames);
		}

		public void Sort(ListSortDirection direction, params string[] memberNames)
		{
			if (memberNames        == null) throw new ArgumentNullException      ("memberNames");
			if (memberNames.Length == 0)    throw new ArgumentOutOfRangeException("memberNames");

			Sort(new SortMemberComparer(TypeAccessor.GetAccessor(ItemType), direction, memberNames));
		}

		public void Move(int newIndex, int oldIndex)
		{
			if (oldIndex != newIndex)
			{
				_bindingListImpl.EndNew();

				object o = _list[oldIndex];

				_list.RemoveAt(oldIndex);
				_list.Insert  (newIndex, o);

				if (_notifyChanges)
					OnListChanged(new EditableListChangedEventArgs(newIndex, oldIndex));
			}
		}

		public void Move(int newIndex, object item)
		{
			int index = IndexOf(item);

			if (index >= 0)
				Move(newIndex, index);
		}

		#endregion

		#region Change Notification

		private bool _notifyChanges = true;
		public  bool  NotifyChanges
		{
			get { return _notifyChanges;  }
			set { _notifyChanges = value; }
		}

		protected virtual void OnListChanged(EditableListChangedEventArgs e)
		{
			if (_notifyChanges)
				_bindingListImpl.FireListChangedEvent(this, e);
		}

		protected void OnListChanged(ListChangedType listChangedType, int index)
		{
			if (_notifyChanges)
				OnListChanged(new EditableListChangedEventArgs(listChangedType, index));
		}

		private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (_notifyChanges && sender != null)
			{
				MemberAccessor ma = TypeAccessor.GetAccessor(sender.GetType())[e.PropertyName];

				if (ma != null)
					OnListChanged(new EditableListChangedEventArgs(_list.IndexOf(sender), ma.PropertyDescriptor));
			}
		}

		#endregion

		#region Add/Remove Internal

		void AddInternal(object value)
		{
			_bindingListImpl.EndNew();

			if (_isTrackingChanges)
			{
				if (DelItems.Contains(value))
					DelItems.Remove(value);
				else
					NewItems.Add(value);
			}

			if (value is INotifyPropertyChanged)
				((INotifyPropertyChanged)value).PropertyChanged += 
					new PropertyChangedEventHandler(ItemPropertyChanged);

			OnAdd(value);
		}

		private void RemoveInternal(object value)
		{
			_bindingListImpl.EndNew();

			if (_isTrackingChanges)
			{
				if (NewItems.Contains(value))
					NewItems.Remove(value);
				else
					DelItems.Add(value);
			}

			if (value is INotifyPropertyChanged)
				((INotifyPropertyChanged)value).PropertyChanged -=
					new PropertyChangedEventHandler(ItemPropertyChanged);

			OnRemove(value);
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

		protected virtual void OnAdd(object value)
		{
		}

		protected virtual void OnRemove(object value)
		{
		}

		#endregion

		#region ISupportMapping Members

		private bool _isTrackingChanges = true;

		public void BeginMapping(InitContext initContext)
		{
			_isTrackingChanges = false;
		}

		public void EndMapping(InitContext initContext)
		{
			AcceptChanges();
			_isTrackingChanges = true;
		}

		#endregion

		#region IEditable Members

		public virtual void AcceptChanges()
		{
			foreach (object o in _list)
				if (o is IEditable)
					((IEditable)o).AcceptChanges();

			_newItems = null;
			_delItems = null;
		}

		public virtual void RejectChanges()
		{
			_isTrackingChanges = false;

			if (_delItems != null)
				foreach (object o in _delItems)
					Add(o);

			if (_newItems != null)
				foreach (object o in _newItems)
					Remove(o);

			foreach (object o in _list)
				if (o is IEditable)
					((IEditable)o).RejectChanges();

			_isTrackingChanges = true;

			_newItems  = null;
			_delItems  = null;
		}

		public virtual bool IsDirty
		{
			get
			{
				if (_newItems != null && _newItems.Count > 0 ||
					_delItems != null && _delItems.Count > 0)
					return true;

				foreach (object o in _list)
					if (o is IEditable)
						if (((IEditable)o).IsDirty)
							return true;

				return false;
			}
		}

		void IPrintDebugState.PrintDebugState(PropertyInfo propertyInfo, ref string str)
		{
			int original = _list.Count
				- (_newItems == null? 0: _newItems.Count)
				+ (_delItems == null? 0: _delItems.Count);

			str += string.Format("{0,-20} {1} {2,-40} {3,-40} \r\n",
				propertyInfo.Name, IsDirty? "*": " ", original, _list.Count);
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
			OnListChanged(ListChangedType.Reset, idx);
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
			if (_list.Count > 0)
			{
				RemoveInternal(_list);
				_list.Clear();
				OnListChanged(ListChangedType.Reset, -1);
			}
		}

		protected EditableArrayList Clone(EditableArrayList el)
		{
			if (_newItems != null) el._newItems = (ArrayList)_newItems.Clone();
			if (_delItems != null) el._delItems = (ArrayList)_delItems.Clone();

			el._notifyChanges     = _notifyChanges;
			el._isTrackingChanges = _isTrackingChanges;

			return el;
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
			_list.CopyTo(array);
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
			if (c.Count > 0)
			{
				_list.InsertRange(index, c);

				AddInternal(c);
				OnListChanged(ListChangedType.Reset, index);
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

			OnListChanged(ListChangedType.Reset, index);
		}

		public override void Reverse()
		{
			_bindingListImpl.EndNew();

			_list.Reverse();

			if (_list.Count > 1)
				OnListChanged(ListChangedType.Reset, 0);
		}

		public override void Reverse(int index, int count)
		{
			_bindingListImpl.EndNew();

			_list.Reverse(index, count);

			if (count > 1)
				OnListChanged(ListChangedType.Reset, 0);
		}

		public override void SetRange(int index, ICollection c)
		{
			_list.SetRange(index, c);

			if (_list.Count > 1)
			{
				AddInternal(c);
				OnListChanged(ListChangedType.Reset, index);
			}
		}

		public override void Sort()
		{
			_bindingListImpl.EndNew();

			_list.Sort();

			if (_list.Count > 1)
				OnListChanged(ListChangedType.Reset, 0);
		}

		public override void Sort(int index, int count, IComparer comparer)
		{
			_bindingListImpl.EndNew();

			_list.Sort(index, count, comparer);

			if (count > 1)
				OnListChanged(ListChangedType.Reset, 0);
		}

		public override void Sort(IComparer comparer)
		{
			_bindingListImpl.EndNew();

			_list.Sort(comparer);

			if (_list.Count > 1)
				OnListChanged(ListChangedType.Reset, 0);
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

		public static EditableArrayList Adapter(Type itemType, IList list)
		{
			if (list == null) throw new ArgumentNullException("list");

			return list is ArrayList?
				new EditableArrayList(itemType, (ArrayList)list):
				new EditableArrayList(itemType, ArrayList.Adapter(list));
		}

		public static new EditableArrayList Adapter(IList list)
		{
			return Adapter(TypeHelper.GetListItemType(list), list);
		}

		#endregion

		#region IDisposable

		public void Dispose()
		{
			Clear();
		}

		#endregion

		#region SortMemberComparer

		class SortMemberComparer : IComparer
		{
			ListSortDirection  _direction;
			string[]           _memberNames;
			TypeAccessor       _typeAccessor;
			MemberAccessor[]   _members;
			MemberAccessor     _member;

			public SortMemberComparer(TypeAccessor typeAccessor, ListSortDirection direction, string[] memberNames)
			{
				_typeAccessor = typeAccessor;
				_direction    = direction;
				_memberNames  = memberNames;
				_members      = new MemberAccessor[memberNames.Length];

				_member = _members[0] = _typeAccessor[memberNames[0]];
			}

			public int Compare(object x, object y)
			{
				object a = _member.GetValue(x);
				object b = _member.GetValue(y);
				int    n = Comparer.Default.Compare(a, b);

				if (n == 0) for (int i = 1; n == 0 && i < _members.Length; i++)
				{
					MemberAccessor member = _members[i];

					if (member == null)
						member = _members[i] = _typeAccessor[_memberNames[i]];

					a = member.GetValue(x);
					b = member.GetValue(y);
					n = Comparer.Default.Compare(a, b);
				}

				return _direction == ListSortDirection.Ascending? n: -n;
			}
		}

		#endregion

		#region ITypedList Members

		private TypedListImpl _typedListImpl;

		public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			return _typedListImpl.GetItemProperties(listAccessors);
		}

		public string GetListName(PropertyDescriptor[] listAccessors)
		{
			return _typedListImpl.GetListName(listAccessors);
		}

		#endregion

		#region IBindingList Members

		private BindingListImpl _bindingListImpl;

		public void AddIndex(PropertyDescriptor property)
		{
			_bindingListImpl.AddIndex(property);
		}

		public object AddNew()
		{
			return _bindingListImpl.AddNew();
		}

		public bool AllowEdit
		{
			get { return _bindingListImpl.AllowEdit; }
		}

		public bool AllowNew
		{
			get { return _bindingListImpl.AllowNew; }
		}

		public bool AllowRemove
		{
			get { return _bindingListImpl.AllowRemove; }
		}

		public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			_bindingListImpl.ApplySort(property, direction);
		}

		public int Find(PropertyDescriptor property, object key)
		{
			return _bindingListImpl.Find(property, key);
		}

		public bool IsSorted
		{
			get { return _bindingListImpl.IsSorted; }
		}

		public void RemoveIndex(PropertyDescriptor property)
		{
			_bindingListImpl.RemoveIndex(property);
		}

		public void RemoveSort()
		{
			_bindingListImpl.RemoveSort();
		}

		public ListSortDirection SortDirection
		{
			get { return _bindingListImpl.SortDirection; }
		}

		public PropertyDescriptor SortProperty
		{
			get { return _bindingListImpl.SortProperty; }
		}

		public bool SupportsChangeNotification
		{
			get { return _bindingListImpl.SupportsChangeNotification; }
		}

		public event ListChangedEventHandler ListChanged
		{
			add    { _bindingListImpl.ListChanged += value; }
			remove { _bindingListImpl.ListChanged -= value; }
		}

		public bool SupportsSearching
		{
			get { return _bindingListImpl.SupportsSearching; }
		}

		public bool SupportsSorting
		{
			get { return _bindingListImpl.SupportsSorting; }
		}

		#endregion

#if FW2

		#region IBindingListView Members

		public void ApplySort(ListSortDescriptionCollection sorts)
		{
			_bindingListImpl.ApplySort(sorts);
		}

		public string Filter
		{
			get { return _bindingListImpl.Filter;  }
			set { _bindingListImpl.Filter = value; }
		}

		public void RemoveFilter()
		{
			_bindingListImpl.RemoveFilter();
		}

		public ListSortDescriptionCollection SortDescriptions
		{
			get { return _bindingListImpl.SortDescriptions; }
		}

		public bool SupportsAdvancedSorting
		{
			get { return _bindingListImpl.SupportsAdvancedSorting; }
		}

		public bool SupportsFiltering
		{
			get { return _bindingListImpl.SupportsFiltering; }
		}

		#endregion

		#region ICancelAddNew Members

		public void CancelNew(int itemIndex)
		{
			_bindingListImpl.CancelNew(itemIndex);
		}

		public void EndNew(int itemIndex)
		{
			_bindingListImpl.EndNew(itemIndex);
		}

		#endregion

#endif
	}
}
