using System;
using System.Collections;
using System.ComponentModel;

using BLToolkit.Reflection;
using BLToolkit.EditableObjects;
using System.Diagnostics;

namespace BLToolkit.ComponentModel
{
	public class BindingListImpl :
#if FW2
		IBindingListView, ICancelAddNew
#else
		IBindingList
#endif
	{
		#region Init

		public BindingListImpl(IList list, Type itemType)
		{
			if (list     == null) throw new ArgumentNullException("list");
			if (itemType == null) throw new ArgumentNullException("itemType");

			_list     = list;
			_itemType = itemType;
		}

		#endregion

		#region Protected Members

		private IList _list;
		private Type  _itemType;

		private void ApplySort(IComparer comparer)
		{
			if (_list is ISortable)
				((ISortable)_list).Sort(0, _list.Count, comparer);
			else if (_list is ArrayList)
				((ArrayList)_list).Sort(0, _list.Count, comparer);
			else if (_list is Array)
				Array.Sort((Array)_list, comparer);
			else
			{
				object[] items = new object[_list.Count];

				_list.CopyTo(items, 0);
				Array.Sort(items, comparer);

				for (int i = 0; i < _list.Count; i++)
					_list[i] = items[i];
			}

			_isSorted = true;
		}

		#endregion

		#region IBindingList Members

			#region Command

		private int               _newItemIndex = -1;
		private INotifyObjectEdit _newObject;

		public object AddNew()
		{
			if (AllowNew == false)
				throw new NotSupportedException();

			EndNew();

			object o = TypeAccessor.CreateInstanceEx(_itemType);

			_newItemIndex = _list.Add(o);
			_newObject    = o as INotifyObjectEdit;

			if (_newObject != null)
				_newObject.ObjectEdit += new ObjectEditEventHandler(NewObject_ObjectEdit);

			Debug.WriteLine(string.Format("AddNew - ({0})", o.GetType().Name));

			return o;
		}

		void NewObject_ObjectEdit(object sender, ObjectEditEventArgs args)
		{
			if (sender == _newObject)
			{
				switch (args.EditType)
				{
					case ObjectEditType.End:    EndNew();                 break;
					case ObjectEditType.Cancel: CancelNew(_newItemIndex); break;
					default:                    return;
				}
			}
		}

		public bool AllowNew
		{
			get { return !_list.IsFixedSize; }
		}

		public bool AllowEdit
		{
			get { return !_list.IsReadOnly; }
		}

		public bool AllowRemove
		{
			get { return !_list.IsFixedSize; }
		}

			#endregion

			#region Change Notification

		private bool _notifyChanges = true;
		public  bool  NotifyChanges
		{
			get { return _notifyChanges; }
			set { _notifyChanges = value; }
		}

		public bool SupportsChangeNotification
		{
			get { return true; }
		}

		public event ListChangedEventHandler ListChanged;

		private void FireListChangedEvent(object sender, EditableListChangedEventArgs e)
		{
			if (_notifyChanges && ListChanged != null)
				ListChanged(sender, e);
		}
		
		public virtual void OnListChanged(EditableListChangedEventArgs e)
		{
			FireListChangedEvent(this, e);
		}

		protected void OnListChanged(ListChangedType listChangedType, int index)
		{
			OnListChanged(new EditableListChangedEventArgs(listChangedType, index));
		}

		private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (_notifyChanges && sender != null)
			{
				int indexOfSender = _list.IndexOf(sender);

				if (indexOfSender >= 0)
				{
					MemberAccessor ma = TypeAccessor.GetAccessor(sender.GetType())[e.PropertyName];

					if (ma != null)
						OnListChanged(new EditableListChangedEventArgs(indexOfSender, ma.PropertyDescriptor));

					if (_isSorted && _list.Count > 1)
					{
						int newIndex = GetItemSortedPosition(indexOfSender, sender);

						if (newIndex != indexOfSender)
						{
							_list.RemoveAt(indexOfSender);
							_list.Insert(newIndex, sender);

							OnListChanged(new EditableListChangedEventArgs(newIndex, indexOfSender));
						}
					}
				}
			}
		}
			#endregion

			#region Sorting

		public bool SupportsSorting
		{
			get { return true; }
		}

		[NonSerialized]
		private bool _isSorted;
		public  bool  IsSorted
		{
			get { return _isSorted; }
		}

		[NonSerialized]
		private PropertyDescriptor _sortProperty;
		public  PropertyDescriptor  SortProperty
		{
			get { return _sortProperty; }
		}

		[NonSerialized]
		private ListSortDirection _sortDirection;
		public  ListSortDirection  SortDirection
		{
			get { return _sortDirection; }
		}

		public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			Debug.WriteLine(string.Format("Begin ApplySort(\"{0}\", {1})", property.Name, direction));

			_sortProperty  = property;
			_sortDirection = direction;
			
#if FW2
			_sortDescriptions = null;
#endif

			ApplySort(new SortPropertyComparer(property, direction));
			
			if (_list.Count > 0)
				OnListChanged(new EditableListChangedEventArgs(ListChangedType.Reset));

			Debug.WriteLine(string.Format("End   ApplySort(\"{0}\", {1})", property.Name, direction));
		}

		public void RemoveSort()
		{
			_isSorted     = false;
			_sortProperty = null;
			
#if FW2
			_sortDescriptions = null;
#endif
		}

			#endregion

			#region Searching

		public bool SupportsSearching
		{
			get { return true; }
		}

		public int Find(PropertyDescriptor property, object key)
		{
			if (property == null) throw new ArgumentException("property");

			if (key != null)
				for (int i = 0; i < _list.Count; i++)
					if (key.Equals(property.GetValue(_list[i])))
						return i;

			return -1;
		}

			#endregion

			#region Indexes

		public void AddIndex(PropertyDescriptor property)
		{
		}

		public void RemoveIndex(PropertyDescriptor property)
		{
		}

			#endregion

		#endregion

		#region ICancelAddNew Members

		public void CancelNew(int itemIndex)
		{
			if (itemIndex >= 0 && itemIndex == _newItemIndex)
			{
				_list.RemoveAt(itemIndex);
				EndNew();
			}
		}

		public void EndNew(int itemIndex)
		{
			if (itemIndex == _newItemIndex)
				EndNew();
		}

		public void EndNew()
		{
			_newItemIndex = -1;

			if (_newObject != null)
				_newObject.ObjectEdit -= new ObjectEditEventHandler(NewObject_ObjectEdit);

			_newObject = null;
		}

		#endregion

		#region IList Members

		public int Add(object value)
		{
			int index;
			
			if (!_isSorted)
				index = _list.Add(value);
			else
			{
				index = GetSortedInsertIndex(value);
				_list.Insert(index, value);
			}

			AddInternal(value);
			OnListChanged(new EditableListChangedEventArgs(ListChangedType.ItemAdded, index));
			
			return index;
		}

		public void Clear()
		{
			if (_list.Count > 0)
			{
				RemoveInternal(_list);
				
				_list.Clear();
				
				OnListChanged(new EditableListChangedEventArgs(ListChangedType.Reset));
			}
		}

		public bool Contains(object value)
		{
			return _list.Contains(value);
		}

		public int IndexOf(object value)
		{
			return _list.IndexOf(value);
		}

		public void Insert(int index, object value)
		{
			if (_isSorted)
				index = GetSortedInsertIndex(value);
			
			_list.Insert(index, value);
			AddInternal(value);
			
			OnListChanged(new EditableListChangedEventArgs(ListChangedType.ItemAdded, index));
		}

		public bool IsFixedSize
		{
			get { return _list.IsFixedSize; }
		}

		public bool IsReadOnly
		{
			get { return _list.IsReadOnly; }
		}

		public void Remove(object value)
		{
			int removalIndex = IndexOf(value);
			
			if (removalIndex >= 0)
				RemoveInternal(value);

			_list.Remove(value);
			
			if (removalIndex >= 0)
			OnListChanged(new EditableListChangedEventArgs(ListChangedType.ItemDeleted, removalIndex));
		}

		public void RemoveAt(int index)
		{
			object o = this[index];

			RemoveInternal(o);
			
			_list.RemoveAt(index);
			
			OnListChanged(new EditableListChangedEventArgs(ListChangedType.ItemDeleted, index));
		}

		public object this[int index]
		{
			get { return _list[index]; }
			set 
			{
				object o = _list[index];

				if (o != value)
				{
					RemoveInternal(o);
					
					_list[index] = value;

					AddInternal(o);

					OnListChanged(new EditableListChangedEventArgs(ListChangedType.ItemChanged, index));
					
					if (_isSorted)
					{
						int newIndex = GetItemSortedPosition(index, value);
						
						if (newIndex != index)
						{
							_list.RemoveAt(index);
							_list.Insert(newIndex, value);
						}
						
						OnListChanged(new EditableListChangedEventArgs(newIndex, index));
					}
				}
			}
		}

		#endregion

		#region ICollection Members

		public void CopyTo(Array array, int index)
		{
			_list.CopyTo(array, index);
		}

		public int Count
		{
			get { return _list.Count; }
		}

		public bool IsSynchronized
		{
			get { return _list.IsSynchronized; }
		}

		public object SyncRoot
		{
			get { return _list.SyncRoot; }
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _list.GetEnumerator();
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

		#region IComparer Accessor
		
		public IComparer GetSortComparer()
		{
			if (_isSorted)
			{
#if FW2
				if (_sortDescriptions != null)
					return new SortListPropertyComparer(_sortDescriptions);
#endif
				return new SortPropertyComparer(_sortProperty, _sortDirection);
			}

			return null;
		}
		
		#endregion

#if FW2

		#region IBindingListView Members

		public bool SupportsAdvancedSorting
		{
			get { return true; }
		}

		public void ApplySort(ListSortDescriptionCollection sorts)
		{
			_sortDescriptions = sorts;

			_isSorted = true;
			_sortProperty = null;
			
			ApplySort(new SortListPropertyComparer(sorts));
			
			if (_list.Count > 0)
				OnListChanged(new EditableListChangedEventArgs(ListChangedType.Reset));
		}

		[NonSerialized]
		private ListSortDescriptionCollection _sortDescriptions;
		public  ListSortDescriptionCollection  SortDescriptions
		{
			get { return _sortDescriptions; }
		}

		public bool SupportsFiltering
		{
			get { return false; }
		}

		public string Filter
		{
			get { throw new NotImplementedException("The method 'BindingListImpl.get_Filter' is not implemented."); }
			set { throw new NotImplementedException("The method 'BindingListImpl.set_Filter' is not implemented."); }
		}

		public void RemoveFilter()
		{
			throw new NotImplementedException("The method 'BindingListImpl.RemoveFilter()' is not implemented.");
		}

		#endregion

		#region SortListPropertyComparer

		class SortListPropertyComparer : IComparer
		{
			ListSortDescriptionCollection _sorts;

			public SortListPropertyComparer(ListSortDescriptionCollection sorts)
			{
				_sorts = sorts;
			}

			public int Compare(object x, object y)
			{
				for (int i = 0; i < _sorts.Count; i++)
				{
					PropertyDescriptor property = _sorts[i].PropertyDescriptor;

					object a = property.GetValue(x);
					object b = property.GetValue(y);

					int n = Comparer.Default.Compare(a, b);

					if (n != 0)
						return _sorts[i].SortDirection == ListSortDirection.Ascending? n: -n;
				}

				return 0;
			}
		}

		#endregion

#endif


		#region Sort enforcement

		public int GetItemSortedPosition(int index, object sender)
		{
			IComparer comparer = GetSortComparer();

			if (comparer == null)
				return index;

			int result     = index > 0 ? comparer.Compare(_list[index - 1], sender) : -1;
			int nextResult = index < _list.Count - 1 ? comparer.Compare(_list[index + 1], sender) : 1;

			if (result > 0 || nextResult < 0)
			{
				bool placed = false;

				for (int i = 0; i < _list.Count; i++)
				{
					if (i == index)
						continue;

					result = comparer.Compare(_list[i], sender);

					if (result > 0)
					{
						if (index == i - 1)
							return index;
								
						return i;
					}
				}

				if (!placed)
					return _list.Count - 1;
			}

			return index;
		}

		public int GetSortedInsertIndex(object value)
		{
			IComparer comparer = GetSortComparer();
			int result;

			if (comparer == null)
				return -1;

			for (int i = 0; i < _list.Count; i++)
			{
				result = comparer.Compare(_list[i], value);

				if (result > 0)
					return i;
			}

			return _list.Count;
		}

		#endregion

		#region Misc/Range Operations
		
		public void Move(int newIndex, int oldIndex)
		{
			if (oldIndex != newIndex)
			{
				EndNew();

				object o = _list[oldIndex];

				_list.RemoveAt(oldIndex);
				if (!_isSorted)
					_list.Insert(newIndex, o);
				else
					_list.Insert(newIndex = GetSortedInsertIndex(o), o);

				OnListChanged(new EditableListChangedEventArgs(newIndex, oldIndex));
			}
		}
		
		public void AddRange(ICollection c)
		{
			foreach (object o in c)
			{
				if (!_isSorted)
					_list.Add(o);
				else
					_list.Insert(GetSortedInsertIndex(o), o);
			}

			AddInternal(c);

			OnListChanged(new EditableListChangedEventArgs(ListChangedType.Reset, -1));
		}
		
		public void InsertRange(int index, ICollection c)
		{
			if (c.Count == 0)
				return;
			
			foreach (object o in c)
			{
				if (!_isSorted)
					_list.Insert(index++, o);
				else
					_list.Insert(GetSortedInsertIndex(o), o);
			}

			AddInternal(c);

			OnListChanged(new EditableListChangedEventArgs(ListChangedType.Reset, -1));
		}
		
		public void RemoveRange(int index, int count)
		{
			object[] toRemove = new object[count];

			for (int i = index; i < _list.Count && i < index + count; i++)
				toRemove[i - index] = _list[i];
			
			RemoveInternal(toRemove);

			foreach (object o in toRemove)
				_list.Remove(o);

			OnListChanged(new EditableListChangedEventArgs(ListChangedType.Reset, -1));
		}
		
		public void SetRange(int index, ICollection c)
		{
			int cCount = c.Count;
			
			if (index < 0 || index >= _list.Count - cCount)
				throw new ArgumentOutOfRangeException("index");

			bool oldNotifyChanges = _notifyChanges;
			_notifyChanges = false;

			int i = index;
			foreach (object newObject in c)
			{
				RemoveInternal(_list[i + index]);
				_list[i + index] = newObject;
			}
			
			AddInternal(c);
			
			if (_isSorted)
				ApplySort(GetSortComparer());

			_notifyChanges = oldNotifyChanges;
			OnListChanged(new EditableListChangedEventArgs(ListChangedType.Reset));
		}
		
		#endregion

		#region Add/Remove Internal
		private void AddInternal(object value)
		{
			EndNew();

			if (value is INotifyPropertyChanged)
				((INotifyPropertyChanged)value).PropertyChanged += 
					new PropertyChangedEventHandler(ItemPropertyChanged);
		}

		private void RemoveInternal(object value)
		{
			EndNew();

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
	}
}
