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
	[DebuggerDisplay("Count = {Count}, ItemType = {ItemType}")]
	[Serializable]
	public class EditableArrayList : ArrayList, IEditable, ISortable, ISupportMapping,
		IDisposable, IPrintDebugState, ITypedList, IBindingListView, ICancelAddNew
	{
		#region Constructors

		public EditableArrayList(Type itemType, ArrayList list, bool trackChanges)
		{
			if (itemType == null) throw new ArgumentNullException("itemType");
			if (list     == null) throw new ArgumentNullException("list");

			if (!trackChanges)
			{
				SetTrackingChanges(trackChanges);
				_minTrackingChangesCount = 1;
			}

			_itemType        = itemType;
			_list            = list;

			AddInternal(_list);
		}

		public EditableArrayList(Type itemType)
			: this(itemType, new ArrayList(), true)
		{
		}

		public EditableArrayList(Type itemType, bool trackChanges)
			: this(itemType, new ArrayList(), trackChanges)
		{
		}

		public EditableArrayList(Type itemType, int capacity)
			: this(itemType, new ArrayList(capacity), true)
		{
		}

		public EditableArrayList(Type itemType, int capacity, bool trackChanges)
			: this(itemType, new ArrayList(capacity), trackChanges)
		{
		}

		public EditableArrayList(Type itemType, ICollection c)
			: this(itemType, new ArrayList(c), true)
		{
		}

		public EditableArrayList(Type itemType, ICollection c, bool trackChanges)
			: this(itemType, new ArrayList(c), trackChanges)
		{
		}

		public EditableArrayList(Type itemType, ArrayList list)
			: this(itemType, list, true)
		{

		}

		public EditableArrayList(EditableArrayList list)
			: this(list.ItemType, new ArrayList(list), true)
		{

		}

		public EditableArrayList(EditableArrayList list, bool trackChanges)
			: this(list.ItemType, new ArrayList(list), trackChanges)
		{

		}

		public EditableArrayList(Type itemType, EditableArrayList list)
			: this(itemType, new ArrayList(list), true)
		{
			
		}

		public EditableArrayList(Type itemType, EditableArrayList list, bool trackChanges)
			: this(itemType, new ArrayList(list), trackChanges)
		{

		}

		#endregion

		#region Public Members

		private readonly ArrayList _list;
		internal         ArrayList  List
		{
			get { return _list; }
		}

		private readonly Type _itemType;
		public           Type  ItemType
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
			BindingListImpl.Move(newIndex, oldIndex);
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
			get { return BindingListImpl.NotifyChanges;  }
			set { BindingListImpl.NotifyChanges = value; }
		}

		protected virtual void OnListChanged(EditableListChangedEventArgs e)
		{
			BindingListImpl.OnListChanged(e);
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

		private          int _noTrackingChangesCount;
		private readonly int _minTrackingChangesCount = 0;

		public  bool IsTrackingChanges
		{
			get { return _noTrackingChangesCount == 0; }
		}

		protected void SetTrackingChanges(bool trackChanges)
		{
			if (trackChanges)
			{
				_noTrackingChangesCount--;

				if (_noTrackingChangesCount < _minTrackingChangesCount)
				{
					_noTrackingChangesCount = _minTrackingChangesCount;
					throw new InvalidOperationException("Tracking Changes Counter can not be negative.");
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
			get { return BindingListImpl.IsFixedSize; }
		}

		public override bool IsReadOnly
		{
			get { return BindingListImpl.IsReadOnly; }
		}

		public override object this[int index]
		{
			get { return BindingListImpl[index];  }
			set
			{
				object o = BindingListImpl[index];
				
				BindingListImpl[index] = value;

				if (o != value)
				{
					RemoveInternal(o);
					AddInternal(o);
				}
			}
		} 

		public override int Add(object value)
		{
			int index = BindingListImpl.Add(value);

			AddInternal(value);

			return index;
		}

		public override void Clear()
		{
			if (_list.Count > 0)
				RemoveInternal(_list);
			
			BindingListImpl.Clear();
		}

		public override bool Contains(object item)
		{
			return BindingListImpl.Contains(item);
		}

		public override int IndexOf(object value)
		{
			return BindingListImpl.IndexOf(value);
		}

		public override void Insert(int index, object value)
		{
			BindingListImpl.Insert(index, value);

			AddInternal(value);
		}

		public override void Remove(object value)
		{
			RemoveInternal(value);

			BindingListImpl.Remove(value);
		}

		public override void RemoveAt(int index)
		{
			RemoveInternal(BindingListImpl[index]);
			
			BindingListImpl.RemoveAt(index);
		}
		
		#endregion

		#region ICollection Members

		public override int Count
		{
			get { return BindingListImpl.Count; }
		}

		public override bool IsSynchronized
		{
			get { return BindingListImpl.IsSynchronized; }
		}

		public override object SyncRoot
		{
			get { return BindingListImpl.SyncRoot; }
		}

		public override void CopyTo(Array array, int arrayIndex)
		{
			BindingListImpl.CopyTo(array, arrayIndex);
		}
		
		#endregion

		#region IEnumerable Members

		public override IEnumerator GetEnumerator()
		{
			return BindingListImpl.GetEnumerator();
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
			if (c.Count == 0)
				return;

			BindingListImpl.AddRange(c);
			
			AddInternal(c);
		}

		public void AddRange(ICollection c, bool trackChanges)
		{
			if (c.Count == 0)
				return;

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
			BindingListImpl.InsertRange(index, c);
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
			BindingListImpl.RemoveRange(index, count);
		}

		public void RemoveRange(int index, int count, bool trackChanges)
		{
			if (!trackChanges) _noTrackingChangesCount++;
			RemoveRange(index, count);
			if (!trackChanges) _noTrackingChangesCount--;
		}

		public override void Reverse()
		{
			BindingListImpl.EndNew();

			if (!BindingListImpl.IsSorted)
				_list.Reverse();
			else
				throw new InvalidOperationException("Reverse is not supported for already sorted arrays. Invoke IBindingList.RemoveSort() first or provide revese sort direction.");

			if (_list.Count > 1)
				OnListChanged(ListChangedType.Reset, -1);
		}

		public override void Reverse(int index, int count)
		{
			BindingListImpl.EndNew();

			if (!BindingListImpl.IsSorted)
				_list.Reverse(index, count);
			else
				throw new InvalidOperationException("Range Reverse is not supported for already sorted arrays. Invoke IBindingList.RemoveSort() first.");

			if (count > 1)
				OnListChanged(ListChangedType.Reset, -1);
		}

		public override void SetRange(int index, ICollection c)
		{
			if (c.Count == 0)
				return;

			BindingListImpl.SetRange(index, c);

			AddInternal(c);
		}

		public override void Sort()
		{
			BindingListImpl.EndNew();

			if (!BindingListImpl.IsSorted)
			{
				_list.Sort();

				if (_list.Count > 1)
					OnListChanged(ListChangedType.Reset, -1);
			}
			else
			{
				if (BindingListImpl.SortProperty != null)
					BindingListImpl.ApplySort(BindingListImpl.SortProperty, BindingListImpl.SortDirection);
				else if (BindingListImpl.SortDescriptions != null)
					BindingListImpl.ApplySort(BindingListImpl.SortDescriptions);
				else
					throw new InvalidOperationException("Currntly applied sort method is not recognised/supported by EditableArrayList.");
			}
		}

		public override void Sort(int index, int count, IComparer comparer)
		{
			BindingListImpl.EndNew();

			if (!BindingListImpl.IsSorted)
				_list.Sort(index, count, comparer);
			else
				throw new InvalidOperationException("Custom sorting is not supported on already sorted arrays. Invoke IBindingList.RemoveSort first.");

			if (count > 1)
				OnListChanged(ListChangedType.Reset, -1);
		}

		public override void Sort(IComparer comparer)
		{
			BindingListImpl.EndNew();

			if (!BindingListImpl.IsSorted)
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

			if (list.IsFixedSize)
				return new EditableArrayList(itemType, new ArrayList(list));

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
			readonly ListSortDirection  _direction;
			readonly string[]           _memberNames;
			readonly TypeAccessor       _typeAccessor;
			readonly MemberAccessor[]   _members;
			readonly MemberAccessor     _member;

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

		[NonSerialized]
		private TypedListImpl _typedListImpl;
		private TypedListImpl  TypedListImpl
		{
			get
			{
				if (_typedListImpl == null)
					_typedListImpl = new TypedListImpl(_itemType);
				return _typedListImpl;
			}
		}

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
			return TypedListImpl.GetItemProperties(listAccessors, objectViewType, isNull, cache);
		}

		public string GetListName(PropertyDescriptor[] listAccessors)
		{
			return TypedListImpl.GetListName(listAccessors);
		}

		#endregion

		#region IBindingList Members

		[NonSerialized]
		private BindingListImpl _bindingListImpl;
		private BindingListImpl  BindingListImpl
		{
			get
			{
				if (_bindingListImpl == null)
					_bindingListImpl = new BindingListImpl(_list, _itemType);
				return _bindingListImpl;
			}
		}

		public void AddIndex(PropertyDescriptor property)
		{
			BindingListImpl.AddIndex(property);
		}

		public object AddNew()
		{
			object newObject = BindingListImpl.AddNew();

			AddInternal(newObject);

			return newObject;
		}

		public bool AllowEdit
		{
			get { return BindingListImpl.AllowEdit; }
		}

		public bool AllowNew
		{
			get { return BindingListImpl.AllowNew; }
		}

		public bool AllowRemove
		{
			get { return BindingListImpl.AllowRemove; }
		}

		public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			BindingListImpl.ApplySort(property, direction);
		}

		public int Find(PropertyDescriptor property, object key)
		{
			return BindingListImpl.Find(property, key);
		}

		public bool IsSorted
		{
			get { return BindingListImpl.IsSorted; }
		}

		public void RemoveIndex(PropertyDescriptor property)
		{
			BindingListImpl.RemoveIndex(property);
		}

		public void RemoveSort()
		{
			BindingListImpl.RemoveSort();
		}

		public ListSortDirection SortDirection
		{
			get { return BindingListImpl.SortDirection; }
		}

		public PropertyDescriptor SortProperty
		{
			get { return BindingListImpl.SortProperty; }
		}

		public bool SupportsChangeNotification
		{
			get { return BindingListImpl.SupportsChangeNotification; }
		}

		public event ListChangedEventHandler ListChanged
		{
			add    { BindingListImpl.ListChanged += value; }
			remove { BindingListImpl.ListChanged -= value; }
		}

		public bool SupportsSearching
		{
			get { return BindingListImpl.SupportsSearching; }
		}

		public bool SupportsSorting
		{
			get { return BindingListImpl.SupportsSorting; }
		}

		#endregion

		#region Sorting Enhancement

		public void CreateSortSubstitution(string originalProperty, string substituteProperty)
		{
			BindingListImpl.CreateSortSubstitution(originalProperty, substituteProperty);
		}

		public void RemoveSortSubstitution(string originalProperty)
		{
			BindingListImpl.RemoveSortSubstitution(originalProperty);
		}

		#endregion

		#region IBindingListView Members

		public void ApplySort(ListSortDescriptionCollection sorts)
		{
			BindingListImpl.ApplySort(sorts);
		}

		public string Filter
		{
			get { return BindingListImpl.Filter;  }
			set { BindingListImpl.Filter = value; }
		}

		public void RemoveFilter()
		{
			BindingListImpl.RemoveFilter();
		}

		public ListSortDescriptionCollection SortDescriptions
		{
			get { return BindingListImpl.SortDescriptions; }
		}

		public bool SupportsAdvancedSorting
		{
			get { return BindingListImpl.SupportsAdvancedSorting; }
		}

		public bool SupportsFiltering
		{
			get { return BindingListImpl.SupportsFiltering; }
		}

		#endregion

		#region ICancelAddNew Members

		public void CancelNew(int itemIndex)
		{
			BindingListImpl.CancelNew(itemIndex);
		}

		public void EndNew(int itemIndex)
		{
			BindingListImpl.EndNew(itemIndex);
		}

		#endregion
	}
}
