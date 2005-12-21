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
	[DebuggerDisplay("Count = {Count}")]
#endif
	[Serializable]
	public class EditableArrayList : ArrayList, IEditable, ISortable, ISupportMapping, IDisposable
	{
		#region Constructors

		public EditableArrayList(Type itemType, ArrayList list)
		{
			if (itemType == null) throw new ArgumentNullException("itemType");
			if (list     == null) throw new ArgumentNullException("list");

			_itemType = itemType;
			_list     = list;

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

		public event ListChangedEventHandler ListChanged;

		private bool _notifyChanges = true;
		public  bool  NotifyChanges
		{
			get { return _notifyChanges;  }
			set { _notifyChanges = value; }
		}

		protected virtual void OnListChanged(EditableListChangedEventArgs e)
		{
			if (_notifyChanges && ListChanged != null)
				ListChanged(this, e);
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

		bool IEditable.AcceptMemberChanges(PropertyInfo propertyInfo, string memberName)
		{
			return false;
		}

		bool IEditable.RejectMemberChanges(PropertyInfo propertyInfo, string memberName)
		{
			return false;
		}

		bool IEditable.IsDirtyMember(PropertyInfo propertyInfo, string memberName, ref bool isDirty)
		{
			return false;
		}

		void IEditable.GetDirtyMembers(PropertyInfo propertyInfo, ArrayList list)
		{
		}

		void IEditable.PrintDebugState(PropertyInfo propertyInfo, ref string str)
		{
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
			_list.Reverse();

			if (_list.Count > 1)
				OnListChanged(ListChangedType.Reset, 0);
		}

		public override void Reverse(int index, int count)
		{
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
			_list.Sort();

			if (_list.Count > 1)
				OnListChanged(ListChangedType.Reset, 0);
		}

		public override void Sort(int index, int count, IComparer comparer)
		{
			_list.Sort(index, count, comparer);

			if (count > 1)
				OnListChanged(ListChangedType.Reset, 0);
		}

		public override void Sort(IComparer comparer)
		{
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

		private static Type GetItemType(IList list)
		{
			if (list == null) throw new ArgumentNullException("list");

			PropertyInfo pi = list.GetType().GetProperty("Item", new Type[] { typeof(int) });
			Type         it = pi == null? typeof(object): pi.PropertyType;

			if (it == typeof(object) && list.Count > 0)
			{
				object o = list[0];

				if (o != null)
					it = o.GetType();
			}

			return it;
		}

		public static new EditableArrayList Adapter(IList list)
		{
			return Adapter(GetItemType(list), list);
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
	}
}
