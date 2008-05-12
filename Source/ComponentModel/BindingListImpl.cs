using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

using BLToolkit.Reflection;
using BLToolkit.EditableObjects;

namespace BLToolkit.ComponentModel
{
	public class BindingListImpl: IBindingListView, ICancelAddNew
	{
		#region Init

		public BindingListImpl(IList list, Type itemType)
		{
			if (list     == null) throw new ArgumentNullException("list");
			if (itemType == null) throw new ArgumentNullException("itemType");

			_list     = list;
			_itemType = itemType;

			AddInternal(_list);
		}

		#endregion

		#region Protected Members

		private readonly IList _list;
		private readonly Type  _itemType;

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
				_newObject.ObjectEdit += NewObject_ObjectEdit;

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
					else
						OnListChanged(new EditableListChangedEventArgs(ListChangedType.ItemChanged, indexOfSender));

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
			_sortDescriptions = null;

			ApplySort(GetSortComparer(_sortProperty, _sortDirection));
			
			if (_list.Count > 0)
				OnListChanged(new EditableListChangedEventArgs(ListChangedType.Reset));

			Debug.WriteLine(string.Format("End   ApplySort(\"{0}\", {1})", property.Name, direction));
		}

		public void RemoveSort()
		{
			_isSorted     = false;
			_sortProperty = null;
			_sortDescriptions = null;

			OnListChanged(new EditableListChangedEventArgs(ListChangedType.Reset));
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
				_newObject.ObjectEdit -= NewObject_ObjectEdit;

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
			readonly PropertyDescriptor _property;
			readonly ListSortDirection  _direction;

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
				if (_sortDescriptions != null)
					return GetSortComparer(_sortDescriptions);

				return GetSortComparer(_sortProperty, _sortDirection);
			}

			return null;
		}
		
		private IComparer GetSortComparer(PropertyDescriptor sortProperty, ListSortDirection sortDirection)
		{
			if (_sortSubstitutions.ContainsKey(sortProperty.Name))
				sortProperty = ((SortSubstitutionPair)_sortSubstitutions[sortProperty.Name]).Substitute;

			return new SortPropertyComparer(sortProperty, sortDirection);
		}

		private IComparer GetSortComparer(ListSortDescriptionCollection sortDescriptions)
		{
			bool needSubstitution = false;

			if (_sortSubstitutions.Count > 0)
			{
				foreach (ListSortDescription sortDescription in sortDescriptions)
				{
					if (_sortSubstitutions.ContainsKey(sortDescription.PropertyDescriptor.Name))
					{
						needSubstitution = true;
						break;
					}
				}

				if (needSubstitution)
				{
					ListSortDescription[] sorts = new ListSortDescription[sortDescriptions.Count];
					sortDescriptions.CopyTo(sorts, 0);

					for (int i = 0; i < sorts.Length; i++)
						if (_sortSubstitutions.ContainsKey(sorts[i].PropertyDescriptor.Name))
							sorts[i] = new ListSortDescription(((SortSubstitutionPair)_sortSubstitutions[sorts[i].PropertyDescriptor.Name]).Substitute, 
								                               sorts[i].SortDirection);

					sortDescriptions = new ListSortDescriptionCollection(sorts);
				}
			}

			return new SortListPropertyComparer(sortDescriptions);
		}
		
		#endregion

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
			
			ApplySort(GetSortComparer(sorts));
			
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
			readonly ListSortDescriptionCollection _sorts;

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

		#region Sorting enhancement

		private readonly Hashtable _sortSubstitutions = new Hashtable();

		private class SortSubstitutionPair
		{
			public SortSubstitutionPair(PropertyDescriptor original, PropertyDescriptor substitute)
			{
				Original = original;
				Substitute = substitute;
			}

			public readonly PropertyDescriptor Original;
			public readonly PropertyDescriptor Substitute;
		}

		public void CreateSortSubstitution(string originalProperty, string substituteProperty)
		{
			TypeAccessor typeAccessor = TypeAccessor.GetAccessor(_itemType);

			PropertyDescriptor originalDescriptor = typeAccessor.PropertyDescriptors[originalProperty];
			PropertyDescriptor substituteDescriptor = typeAccessor.PropertyDescriptors[substituteProperty];

			if (originalDescriptor == null)   throw new InvalidOperationException("Can not retrieve PropertyDescriptor for original property: " + originalProperty);
			if (substituteDescriptor == null) throw new InvalidOperationException("Can not retrieve PropertyDescriptor for substitute property: " + substituteProperty);

			_sortSubstitutions[originalProperty] = new SortSubstitutionPair(originalDescriptor, substituteDescriptor);
		}

		public void RemoveSortSubstitution(string originalProperty)
		{
			_sortSubstitutions.Remove(originalProperty);
		}

		#endregion

		#region Sort enforcement

		public int GetItemSortedPosition(int index, object sender)
		{
			IComparer comparer = GetSortComparer();

			if (comparer == null)
				return index;

			if ((index > 0 && comparer.Compare(_list[index - 1], sender) > 0) ||
				(index < _list.Count - 1 && comparer.Compare(_list[index + 1], sender) < 0))
			{
				for (int i = 0; i < _list.Count; i++)
				{
					if (i != index && comparer.Compare(_list[i], sender) > 0)
					{
						if (i > index)
							return i - 1;

						return i;
					}
				}

				return _list.Count - 1;
			}

			return index;
		}

		public int GetSortedInsertIndex(object value)
		{
			IComparer comparer = GetSortComparer();

			if (comparer == null)
				return -1;

			for (int i = 0; i < _list.Count; i++)
				if (comparer.Compare(_list[i], value) > 0)
					return i;

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
					ItemPropertyChanged;
		}

		private void RemoveInternal(object value)
		{
			EndNew();

			if (value is INotifyPropertyChanged)
				((INotifyPropertyChanged)value).PropertyChanged -=
					ItemPropertyChanged;
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
