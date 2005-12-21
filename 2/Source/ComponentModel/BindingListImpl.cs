using System;
using System.Collections;
using System.ComponentModel;

using BLToolkit.Reflection;
using BLToolkit.EditableObjects;

namespace BLToolkit.ComponentModel
{
	[Serializable]
	public class BindingListImpl :
#if FW2
		IBindingListView,
#else
		IBindingList,
#endif
		IDisposable
	{
		public BindingListImpl(IList list, Type itemType)
		{
			if (list == null) throw new ArgumentNullException("list");

			_list     = list;
			_itemType = itemType;

			WireList();
		}

		#region Protected Members

		private IList _list;
		private Type  _itemType;
		private bool  _supportsChangeNotification;

		private void _list_ListChanged(object sender, ListChangedEventArgs e)
		{
			if (ListChanged != null)
				ListChanged(sender, e);
		}

		private void WireList()
		{
			if (_list is EditableArrayList)
			{
				((EditableArrayList)_list).ListChanged += new ListChangedEventHandler(_list_ListChanged);
				_supportsChangeNotification = true;
			}
			else if (_list is IBindingList)
			{
				((IBindingList)_list).ListChanged += new ListChangedEventHandler(_list_ListChanged);
				_supportsChangeNotification = true;
			}
		}

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

		object IBindingList.AddNew()
		{
			if (((IBindingList)this).AllowNew == false)
				throw new InvalidOperationException();

			object o = TypeAccessor.CreateInstanceEx(_itemType);

			_list.Add(o);

			return o;
		}

		bool IBindingList.AllowNew
		{
			get { return !_list.IsFixedSize; }
		}

		bool IBindingList.AllowEdit
		{
			get { return !_list.IsReadOnly; }
		}

		bool IBindingList.AllowRemove
		{
			get { return !_list.IsFixedSize; }
		}

			#endregion

			#region Change Notification

		bool IBindingList.SupportsChangeNotification
		{
			get { return _supportsChangeNotification; }
		}

		public event ListChangedEventHandler ListChanged;

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
			ApplySort(new SortPropertyComparer(property, direction));

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
				for (int i = 0; i < _list.Count; i++)
					if (key.Equals(property.GetValue(_list[i])))
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
			get { return _list[index];  }
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

		#region IDisposable Members

		public void Dispose()
		{
			if (_supportsChangeNotification)
			{
				if (_list is EditableArrayList)
					((EditableArrayList)_list).ListChanged -= new ListChangedEventHandler(_list_ListChanged);
				else if (_list is IBindingList)
					((IBindingList)_list).ListChanged -= new ListChangedEventHandler(_list_ListChanged);
			}

			_list = null;
		}

		#endregion

#if FW2

		#region IBindingListView Members

		bool IBindingListView.SupportsAdvancedSorting
		{
			get { return true; }
		}

		void IBindingListView.ApplySort(ListSortDescriptionCollection sorts)
		{
			ApplySort(new SortListPropertyComparer(sorts));

			_sortDescriptions = sorts;
		}

		[NonSerialized]
		ListSortDescriptionCollection                 _sortDescriptions;
		ListSortDescriptionCollection IBindingListView.SortDescriptions
		{
			get { return _sortDescriptions; }
		}

		bool IBindingListView.SupportsFiltering
		{
			get { return false; }
		}

		string IBindingListView.Filter
		{
			get { throw new Exception("The method or operation is not implemented."); }
			set { throw new Exception("The method or operation is not implemented."); }
		}

		void IBindingListView.RemoveFilter()
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
