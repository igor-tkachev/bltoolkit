using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace BLToolkit.EditableObjects
{
	using Reflection;
	using Mapping;
	using ComponentModel;

	[DebuggerDisplay("Count = {Count}, ItemType = {ItemType}")]
	[Serializable]
	public class EditableArrayList : ArrayList, IEditable, ISortable, ISupportMapping,
		IDisposable, IPrintDebugState, ITypedList, IBindingListView, ICancelAddNew, INotifyCollectionChanged
	{
		#region Constructors

		public EditableArrayList() : this(typeof(object), new ArrayList(), true)
		{
		}

		public EditableArrayList([JetBrains.Annotations.NotNull] Type itemType, [JetBrains.Annotations.NotNull] ArrayList list, bool trackChanges)
		{
			if (itemType == null) throw new ArgumentNullException("itemType");
			if (list     == null) throw new ArgumentNullException("list");

			ItemType = itemType;
			List     = list;

			_noTrackingChangesCount++;
			AddInternal(List);
			_noTrackingChangesCount--;

			if (!trackChanges)
			{
				SetTrackingChanges(false);
				_minTrackingChangesCount = 1;
			}
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

		internal ArrayList List     { get; private set; }
		public   Type      ItemType { get; private set; }

		private ArrayList _newItems;
		public  ArrayList  NewItems
		{
			get { return _newItems ?? (_newItems = new ArrayList()); }
		}

		private ArrayList _delItems;
		public  ArrayList  DelItems
		{
			get { return _delItems ?? (_delItems = new ArrayList()); }
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
			var sorts = sortExpression.Split(',');

			for (var i = 0; i < sorts.Length; i++)
				sorts[i] = sorts[i].Trim();

			var last = sorts[sorts.Length - 1];
			var desc = last.ToLower().EndsWith(" desc");

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
				var index = IndexOf(item);

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

		protected virtual void OnListChanged(ListChangedEventArgs e)
		{
			if (!_supressEvent && NotifyChanges && ListChanged != null)
				ListChanged(this, e);
		}

		protected void OnListChanged(ListChangedType listChangedType, int index)
		{
			OnListChanged(new EditableListChangedEventArgs(listChangedType, index));
		}

		private void OnResetList()
		{
			OnListChanged(ListChangedType.Reset, -1);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		private void OnAddItem(object newObject, int index)
		{
			OnListChanged(ListChangedType.ItemAdded, index);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newObject, index));
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
			foreach (var o in e)
				AddInternal(o);
		}

		private void RemoveInternal(IEnumerable e)
		{
			foreach (var o in e)
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
		private readonly int _minTrackingChangesCount;

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
			foreach (var o in List)
			{
				if (o is EditableObject)
					((EditableObject)o).AcceptChanges();
				else if (o is IEditable)
					((IEditable)o).AcceptChanges();
			}

			_newItems = null;
			_delItems = null;
		}

		public virtual void RejectChanges()
		{
			_noTrackingChangesCount++;

			if (_delItems != null)
				foreach (var o in _delItems)
					Add(o);

			if (_newItems != null)
				foreach (var o in _newItems)
					Remove(o);

			foreach (var o in List)
			{
				if (o is EditableObject)
					((EditableObject)o).RejectChanges();
				else if (o is IEditable)
					((IEditable)o).RejectChanges();
			}

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

				foreach (var o in List)
				{
					if (o is EditableObject)
						if (((EditableObject)o).IsDirty)
							return true;
					else if (o is IEditable)
						if (((IEditable)o).IsDirty)
							return true;
				}

				return false;
			}
		}

		void IPrintDebugState.PrintDebugState(PropertyInfo propertyInfo, ref string str)
		{
			var original = List.Count
				- (_newItems == null? 0: _newItems.Count)
				+ (_delItems == null? 0: _delItems.Count);

			str += string.Format("{0,-20} {1} {2,-40} {3,-40} \r\n",
				propertyInfo.Name, IsDirty? "*": " ", original, List.Count);
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
				var o = BindingListImpl[index];

				if (o != value)
				{
					RemoveInternal(o);
					AddInternal(value);
				}
				
				BindingListImpl[index] = value;
			}
		} 

		public override int Add(object value)
		{
			AddInternal(value);

			return BindingListImpl.Add(value);
		}

		public override void Clear()
		{
			if (List.Count > 0)
				RemoveInternal(List);
			
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
			AddInternal(value);

			BindingListImpl.Insert(index, value);
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
			var idx = Add(value);
			if (!trackChanges) _noTrackingChangesCount--;

			return idx;
		}

		public override void AddRange(ICollection c)
		{
			if (c.Count == 0)
				return;
			
			AddInternal(c);

			BindingListImpl.AddRange(c);
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
			return List.BinarySearch(index, count, value, comparer);
		}

		public override int BinarySearch(object value)
		{
			return List.BinarySearch(value);
		}

		public override int BinarySearch(object value, IComparer comparer)
		{
			return List.BinarySearch(value, comparer);
		}

		public override int Capacity
		{
			get { return List.Capacity;  }
			set { List.Capacity = value; }
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
			return Clone(new EditableArrayList(ItemType, (ArrayList)List.Clone()));
		}

		public override void CopyTo(int index, Array array, int arrayIndex, int count)
		{
			List.CopyTo(index, array, arrayIndex, count);
		}

		public override void CopyTo(Array array)
		{
			List.CopyTo(array);
		}

		public override bool Equals(object obj)
		{
			return List.Equals(obj);
		}

		public override IEnumerator GetEnumerator(int index, int count)
		{
			return List.GetEnumerator(index, count);
		}

		public override int GetHashCode()
		{
			return List.GetHashCode();
		}

		public override ArrayList GetRange(int index, int count)
		{
			return List.GetRange(index, count);
		}

		public override int IndexOf(object value, int startIndex)
		{
			return List.IndexOf(value, startIndex);
		}

		public override int IndexOf(object value, int startIndex, int count)
		{
			return List.IndexOf(value, startIndex, count);
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
			return List.LastIndexOf(value);
		}

		public override int LastIndexOf(object value, int startIndex)
		{
			return List.LastIndexOf(value, startIndex);
		}

		public override int LastIndexOf(object value, int startIndex, int count)
		{
			return List.LastIndexOf(value, startIndex, count);
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
			RemoveInternal(GetRange(index, count));

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
				List.Reverse();
			else
				throw new InvalidOperationException("Reverse is not supported for already sorted arrays. Invoke IBindingList.RemoveSort() first or provide reverse sort direction.");

			if (List.Count > 1)
				OnResetList();
		}

		public override void Reverse(int index, int count)
		{
			BindingListImpl.EndNew();

			if (!BindingListImpl.IsSorted)
				List.Reverse(index, count);
			else
				throw new InvalidOperationException("Range Reverse is not supported for already sorted arrays. Invoke IBindingList.RemoveSort() first.");

			if (count > 1)
				OnResetList();
		}

		public override void SetRange(int index, ICollection c)
		{
			if (c.Count == 0)
				return;

			AddInternal(c);

			BindingListImpl.SetRange(index, c);
		}

		public override void Sort()
		{
			BindingListImpl.EndNew();

			if (!BindingListImpl.IsSorted)
			{
				List.Sort();

				if (List.Count > 1)
					OnResetList();
			}
			else
			{
				if (BindingListImpl.SortProperty != null)
					BindingListImpl.ApplySort(BindingListImpl.SortProperty, BindingListImpl.SortDirection);
				else if (BindingListImpl.SortDescriptions != null)
					BindingListImpl.ApplySort(BindingListImpl.SortDescriptions);
				else
					throw new InvalidOperationException("Currently applied sort method is not recognized/supported by EditableArrayList.");
			}
		}

		public override void Sort(int index, int count, IComparer comparer)
		{
			BindingListImpl.EndNew();

			if (!BindingListImpl.IsSorted)
				List.Sort(index, count, comparer);
			else
				throw new InvalidOperationException("Custom sorting is not supported on already sorted arrays. Invoke IBindingList.RemoveSort first.");

			if (count > 1)
				OnResetList();
		}

		public override void Sort(IComparer comparer)
		{
			BindingListImpl.EndNew();

			if (!BindingListImpl.IsSorted)
				List.Sort(comparer);
			else
				throw new InvalidOperationException("Custom sorting is not supported on already sorted arrays. Invoke IBindingList.RemoveSort first.");

			if (List.Count > 1)
				OnResetList();
		}

		public override object[] ToArray()
		{
			return List.ToArray();
		}

		public override Array ToArray(Type type)
		{
			return List.ToArray(type);
		}

		public override string ToString()
		{
			return List.ToString();
		}

		public override void TrimToSize()
		{
			List.TrimToSize();
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
				var a = _member.GetValue(x);
				var b = _member.GetValue(y);
				var n = Comparer.Default.Compare(a, b);

				if (n == 0) for (var i = 1; n == 0 && i < _members.Length; i++)
				{
					var member = _members[i];

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
			get { return _typedListImpl ?? (_typedListImpl = new TypedListImpl(ItemType)); }
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

		sealed class BindingListImplInternal : BindingListImpl
		{
			readonly EditableArrayList _owner;

			public BindingListImplInternal(IList list, Type itemType, EditableArrayList owner)
				: base(list, itemType)
			{
				_owner = owner;
			}

			protected override void OnListChanged(EditableListChangedEventArgs e)
			{
				_owner.OnListChanged(e);
			}

			protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs ea)
			{
				_owner.OnCollectionChanged(ea);
			}
		}

		[NonSerialized]
		private BindingListImpl _bindingListImpl;
		private BindingListImpl  BindingListImpl
		{
			get { return _bindingListImpl ?? (_bindingListImpl = new BindingListImplInternal(List, ItemType, this)); }
		}

		public void AddIndex(PropertyDescriptor property)
		{
			BindingListImpl.AddIndex(property);
		}

		public object AddNew()
		{
			object newObject;

			try
			{
				BeginSuppressEvent();
				newObject = BindingListImpl.AddNew();
			}
			finally
			{
				EndSuppressEvent();
			}

			AddInternal(newObject);

			var index = IndexOf(newObject);

			EndSuppressEvent();
			OnAddItem(newObject, index);

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

		public event ListChangedEventHandler ListChanged;

		private bool _supressEvent;

		private void BeginSuppressEvent()
		{
			_supressEvent = true;
		}

		private void EndSuppressEvent()
		{
			_supressEvent = false;
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
			if (itemIndex >= 0 && itemIndex < List.Count)
				NewItems.Remove(List[itemIndex]);

			BindingListImpl.CancelNew(itemIndex);
		}

		public void EndNew(int itemIndex)
		{
			BindingListImpl.EndNew(itemIndex);
		}

		#endregion

		#region INotifyCollectionChanged Members

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (!_supressEvent && NotifyChanges && CollectionChanged != null)
				CollectionChanged(this, e);
		}

		#endregion
	}
}
