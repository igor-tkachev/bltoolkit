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

			_supportsChangeNotification = _list is EditableArrayList || _list is IBindingList;
		}

		#endregion

		#region Protected Members

		private IList _list;
		private Type  _itemType;
		private bool  _supportsChangeNotification;

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
			if (((IBindingList)this).AllowNew == false)
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

		public bool SupportsChangeNotification
		{
			get { return _supportsChangeNotification; }
		}

		public event ListChangedEventHandler ListChanged;

		public void FireListChangedEvent(object sender, EditableListChangedEventArgs e)
		{
			if (ListChanged != null)
				ListChanged(sender, e);
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

			ApplySort(new SortPropertyComparer(property, direction));

			Debug.WriteLine(string.Format("End   ApplySort(\"{0}\", {1})", property.Name, direction));
		}

		public void RemoveSort()
		{
			_isSorted     = false;
			_sortProperty = null;
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
			return _list.Add(value);
		}

		public void Clear()
		{
			_list.Clear();
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
			_list.Insert(index, value);
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
			_list.Remove(value);
		}

		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
		}

		public object this[int index]
		{
			get { return _list[index];  }
			set { _list[index] = value; }
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

#if FW2

		#region IBindingListView Members

		public bool SupportsAdvancedSorting
		{
			get { return true; }
		}

		public void ApplySort(ListSortDescriptionCollection sorts)
		{
			ApplySort(new SortListPropertyComparer(sorts));

			_sortDescriptions = sorts;
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
			get { throw new Exception("The method or operation is not implemented."); }
			set { throw new Exception("The method or operation is not implemented."); }
		}

		public void RemoveFilter()
		{
			throw new Exception("The method or operation is not implemented.");
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
	}
}
