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
			_bindingListImpl = new BindingListImpl(list, itemType);

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

		public void SortEx(string sortExpression)
		{
			string[] sorts = sortExpression.Split(',');

			for (int i = 0; i < sorts.Length; i++)
				sorts[i] = sorts[i].Trim();

			string last  = sorts[sorts.Length - 1];

			bool desc = last.ToLower().EndsWith(" desc");

			if (desc)
				sorts[sorts.Length - 1] = last.Substring(0, last.Length - " desc".Length);

			Sort(desc? ListSortDirection.Descending: ListSortDirection.Ascending, sorts);
		}

		public void Move(int newIndex, int oldIndex)
		{
			_bindingListImpl.Move(newIndex, oldIndex);
		}

		public void Move(int newIndex, object item)
		{
			lock (SyncRoot)
			{
				int index = IndexOf(item);

				if (index >= 0)
					Move(newIndex, index);
			}
		}

		#endregion

		#region Change Notification

		public bool NotifyChanges
		{
			get { return _bindingListImpl.NotifyChanges;  }
			set { _bindingListImpl.NotifyChanges = value; }
		}

		protected virtual void OnListChanged(EditableListChangedEventArgs e)
		{
			_bindingListImpl.OnListChanged(e);
		}

		protected void OnListChanged(ListChangedType listChangedType, int index)
		{
			OnListChanged(new EditableListChangedEventArgs(listChangedType, index));
		}

		#endregion

		#region Add/Remove Internal

		void AddInternal(object value)
		{
			if (IsTrackingChanges)
			{
				if (DelItems.Contains(value))
					DelItems.Remove(value);
				else
					NewItems.Add(value);
			}

			OnAdd(value);
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

		#region Track Changes

		private int _noTrackingChangesCount;

		public  bool IsTrackingChanges
		{
			get { return _noTrackingChangesCount == 0; }
		}

		protected void SetTrackingChanges(bool trackChanges)
		{
			if (trackChanges)
			{
				_noTrackingChangesCount--;

				if (_noTrackingChangesCount < 0)
				{
					_noTrackingChangesCount = 0;
					throw new InvalidOperationException("Tracking Changes Counter con not be negative.");
				}
			}
			else
			{
				_noTrackingChangesCount++;
			}
		}

		#endregion

		#region ISupportMapping Members

		public virtual void BeginMapping(InitContext initContext)
		{
			if (initContext.IsDestination)
				_noTrackingChangesCount++;
		}

		public virtual void EndMapping(InitContext initContext)
		{
			if (initContext.IsDestination)
			{
				AcceptChanges();
				_noTrackingChangesCount--;
			}
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
			_noTrackingChangesCount++;

			if (_delItems != null)
				foreach (object o in _delItems)
					Add(o);

			if (_newItems != null)
				foreach (object o in _newItems)
					Remove(o);

			foreach (object o in _list)
				if (o is IEditable)
					((IEditable)o).RejectChanges();

			_noTrackingChangesCount--;

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

		#region IList Members

		public override bool IsFixedSize
		{
			get { return _bindingListImpl.IsFixedSize; }
		}

		public override bool IsReadOnly
		{
			get { return _bindingListImpl.IsReadOnly; }
		}

		public override object this[int index]
		{
			get { return _bindingListImpl[index];  }
			set
			{
				object o = _bindingListImpl[index];
				
				_bindingListImpl[index] = value;

				if (o != value)
				{
					RemoveInternal(o);
					AddInternal(o);
				}
			}
		} 

		public override int Add(object value)
		{
			int index = _bindingListImpl.Add(value);

			AddInternal(value);

			return index;
		}

		public override void Clear()
		{
			if (_list.Count > 0)
				RemoveInternal(_list);
			
			_bindingListImpl.Clear();
		}

		public override bool Contains(object item)
		{
			return _bindingListImpl.Contains(item);
		}

		public override int IndexOf(object value)
		{
			return _bindingListImpl.IndexOf(value);
		}

		public override void Insert(int index, object value)
		{
			_bindingListImpl.Insert(index, value);

			AddInternal(value);
		}

		public override void Remove(object value)
		{
			RemoveInternal(value);

			_bindingListImpl.Remove(value);
		}

		public override void RemoveAt(int index)
		{
			RemoveInternal(_bindingListImpl[index]);
			
			_bindingListImpl.RemoveAt(index);
		}
		
		#endregion

		#region ICollection Members

		public override int Count
		{
			get { return _bindingListImpl.Count; }
		}

		public override bool IsSynchronized
		{
			get { return _bindingListImpl.IsSynchronized; }
		}

		public override object SyncRoot
		{
			get { return _bindingListImpl.SyncRoot; }
		}

		public override void CopyTo(Array array, int arrayIndex)
		{
			_bindingListImpl.CopyTo(array, arrayIndex);
		}
		
		#endregion

		#region IEnumerable Members

		public override IEnumerator GetEnumerator()
		{
			return _bindingListImpl.GetEnumerator();
		}
		
		#endregion

		#region Overridden Methods

		public int Add(object value, bool trackChanges)
		{
			if (!trackChanges) _noTrackingChangesCount++;
			int idx = Add(value);
			if (!trackChanges) _noTrackingChangesCount--;

			return idx;
		}

		public override void AddRange(ICollection c)
		{
			_bindingListImpl.AddRange(c);
			
			AddInternal(c);
		}

		public void AddRange(ICollection c, bool trackChanges)
		{
			if (!trackChanges) _noTrackingChangesCount++;
			AddRange(c);
			if (!trackChanges) _noTrackingChangesCount--;
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

		public void Clear(bool trackChanges)
		{
			if (!trackChanges) _noTrackingChangesCount++;
			Clear();
			if (!trackChanges) _noTrackingChangesCount--;
		}

		protected EditableArrayList Clone(EditableArrayList el)
		{
			if (_newItems != null) el._newItems = (ArrayList)_newItems.Clone();
			if (_delItems != null) el._delItems = (ArrayList)_delItems.Clone();

			el.NotifyChanges           = NotifyChanges;
			el._noTrackingChangesCount = _noTrackingChangesCount;

			return el;
		}

		public override object Clone()
		{
			return Clone(new EditableArrayList(ItemType, (ArrayList)_list.Clone()));
		}

		public override void CopyTo(int index, Array array, int arrayIndex, int count)
		{
			_list.CopyTo(index, array, arrayIndex, count);
		}

		public override void CopyTo(Array array)
		{
			_list.CopyTo(array);
		}

		public override bool Equals(object obj)
		{
			return _list.Equals(obj);
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

		public override int IndexOf(object value, int startIndex)
		{
			return _list.IndexOf(value, startIndex);
		}

		public override int IndexOf(object value, int startIndex, int count)
		{
			return _list.IndexOf(value, startIndex, count);
		}

		public void Insert(int index, object value, bool trackChanges)
		{
			if (!trackChanges) _noTrackingChangesCount++;
			Insert(index, value);
			if (!trackChanges) _noTrackingChangesCount--;
		}

		public override void InsertRange(int index, ICollection c)
		{
			_bindingListImpl.InsertRange(index, c);
		}

		public void InsertRange(int index, ICollection c, bool trackChanges)
		{
			if (!trackChanges) _noTrackingChangesCount++;
			InsertRange(index, c);
			if (!trackChanges) _noTrackingChangesCount--;
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

		public void Remove(object obj, bool trackChanges)
		{
			if (!trackChanges) _noTrackingChangesCount++;
			Remove(obj);
			if (!trackChanges) _noTrackingChangesCount--;
		}

		public void RemoveAt(int index, bool trackChanges)
		{
			if (!trackChanges) _noTrackingChangesCount++;
			RemoveAt(index);
			if (!trackChanges) _noTrackingChangesCount--;
		}

		public override void RemoveRange(int index, int count)
		{
			_bindingListImpl.RemoveRange(index, count);
		}

		public void RemoveRange(int index, int count, bool trackChanges)
		{
			if (!trackChanges) _noTrackingChangesCount++;
			RemoveRange(index, count);
			if (!trackChanges) _noTrackingChangesCount--;
		}

		public override void Reverse()
		{
			_bindingListImpl.EndNew();

			if (!_bindingListImpl.IsSorted)
				_list.Reverse();
			else
				throw new InvalidOperationException("Reverse is not supported for already sorted arrays. Invoke IBindingList.RemoveSort() first or provide revese sort direction.");

			if (_list.Count > 1)
				OnListChanged(ListChangedType.Reset, -1);
		}

		public override void Reverse(int index, int count)
		{
			_bindingListImpl.EndNew();

			if (!_bindingListImpl.IsSorted)
				_list.Reverse(index, count);
			else
				throw new InvalidOperationException("Range Reverse is not supported for already sorted arrays. Invoke IBindingList.RemoveSort() first.");

			if (count > 1)
				OnListChanged(ListChangedType.Reset, -1);
		}

		public override void SetRange(int index, ICollection c)
		{
			_bindingListImpl.SetRange(index, c);

			AddInternal(c);
		}

		public override void Sort()
		{
			_bindingListImpl.EndNew();

			if (!_bindingListImpl.IsSorted)
			{
				_list.Sort();

				if (_list.Count > 1)
					OnListChanged(ListChangedType.Reset, -1);
			}
			else
			{
				if (_bindingListImpl.SortProperty != null)
					_bindingListImpl.ApplySort(_bindingListImpl.SortProperty, _bindingListImpl.SortDirection);
#if FW2
				else if (_bindingListImpl.SortDescriptions != null)
					_bindingListImpl.ApplySort(_bindingListImpl.SortDescriptions);
#endif
				else
					throw new InvalidOperationException("Currntly applied sort method is not recognised/supported by EditableArrayList.");
			}
		}

		public override void Sort(int index, int count, IComparer comparer)
		{
			_bindingListImpl.EndNew();

			if (!_bindingListImpl.IsSorted)
				_list.Sort(index, count, comparer);
			else
				throw new InvalidOperationException("Custom sorting is not supported on already sorted arrays. Invoke IBindingList.RemoveSort first.");

			if (count > 1)
				OnListChanged(ListChangedType.Reset, -1);
		}

		public override void Sort(IComparer comparer)
		{
			_bindingListImpl.EndNew();

			if (!_bindingListImpl.IsSorted)
				_list.Sort(comparer);
			else
				throw new InvalidOperationException("Custom sorting is not supported on already sorted arrays. Invoke IBindingList.RemoveSort first.");

			if (_list.Count > 1)
				OnListChanged(ListChangedType.Reset, -1);
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

				if (_member == null)
					throw new InvalidOperationException(
						string.Format("Field '{0}.{1}' not found.",
							_typeAccessor.OriginalType.Name, _memberNames[0]));
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
					{
						member = _members[i] = _typeAccessor[_memberNames[i]];

						if (member == null)
							throw new InvalidOperationException(
								string.Format("Field '{0}.{1}' not found.",
									_typeAccessor.OriginalType.Name, _memberNames[i]));
					}

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
			return GetItemProperties(listAccessors, null, null, true);
		}

		public PropertyDescriptorCollection GetItemProperties(
			PropertyDescriptor[] listAccessors,
			Type                 objectViewType,
			IsNullHandler        isNull,
			bool                 cache)
		{
			return _typedListImpl.GetItemProperties(listAccessors, objectViewType, isNull, cache);
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
