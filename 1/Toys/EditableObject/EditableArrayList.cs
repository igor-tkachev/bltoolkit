using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;

using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.EditableObject
{
	public delegate bool IsNullHandler(object value);

	[Serializable, ComVisible(true)]
	public class EditableArrayList :
		ArrayList, IBindingList, ITypedList, ISupportInitialize, IDisposable,
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

		private IsNullHandler _isNull;
		public  IsNullHandler  IsNull
		{
			get { return _isNull;  }
			set { _isNull = value; }
		}

		public void Sort(string memberName)
		{
			Sort(memberName, ListSortDirection.Ascending);
		}

		public void Sort(string memberName, ListSortDirection direction)
		{
			Sort(new SortMemberComparer(Map.Descriptor(ItemType)[memberName], direction));
		}

		public void Move(int newIndex, int oldIndex)
		{
			if (oldIndex != newIndex)
			{
				object o = _list[oldIndex];

				_list.RemoveAt(oldIndex);
				_list.Insert  (newIndex, o);

				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemMoved, newIndex, oldIndex));
			}
		}

		public void Move(int newIndex, object item)
		{
			int index = IndexOf(item);

			if (index >= 0)
				Move(newIndex, index);
		}

		#endregion

		#region Protected Members

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

		void AddInternal(object value)
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

		public bool AcceptChanges(string memberName, MapPropertyInfo propertyInfo)
		{
			return false;
		}

		public bool RejectChanges(string memberName, MapPropertyInfo propertyInfo)
		{
			return false;
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

		bool IEditable.IsDirtyMember(string memberName, MapPropertyInfo propertyInfo, ref bool isDirty)
		{
			return false;
		}

		public void GetDirtyMembers([MapPropertyInfo] MapPropertyInfo propertyInfo, ArrayList list)
		{
		}

		public void PrintDebugState([MapPropertyInfo] MapPropertyInfo propertyInfo, ref string str)
		{
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

		private static Hashtable _descriptors = new Hashtable();
		private static Hashtable _fieldCount  = new Hashtable();

		public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			return GetItemProperties(ItemType, "", new Type[] {}, new PropertyDescriptor[] {}, listAccessors);
		}

		private PropertyDescriptorCollection GetItemProperties(Type itemType, string propertyPrefix, Type[] parentTypes, PropertyDescriptor[] parentAccessors, PropertyDescriptor[] listAccessors)
		{
			string key = itemType.ToString() + "." + (_isNull == null? "0": "1");

			PropertyDescriptorCollection pdc =
				propertyPrefix.Length == 0? (PropertyDescriptorCollection)_descriptors[key]: null;

			if (pdc == null)
			{
				pdc = TypeDescriptor.GetProperties(itemType);

				ArrayList     list      = new ArrayList(pdc.Count);
				bool          isDataRow = itemType.IsSubclassOf(typeof(DataRow));
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
						pd = new IListPropertyDescriptor(p, propertyPrefix, parentAccessors, this, IsNull);
					}
					else
					{
						if (isDataRow == false)
						{
							if (md == null)
								md = Map.Descriptor(itemType);

							foreach (IMemberMapper mm in md)
							{
								if (mm.OriginalName == p.Name)
								{
									if (mm.MemberType.IsEnum == false && mm.MapValueAttributeList.Length == 0)
									{
										pd = new MapPropertyDescriptor(p, propertyPrefix, parentAccessors, mm, IsNull);
									}

									break;
								}
							}
						}
					}

					if (pd == null)
						pd = new StandardPropertyDescriptor(p, propertyPrefix, parentAccessors, IsNull);

					if (pd != null && !p.PropertyType.IsValueType && p.PropertyType != typeof(string) && 
						Array.IndexOf(parentTypes, p.GetType()) == -1)
					{
						Type[] childParentTypes = new Type[parentTypes.Length+1];
						
						parentTypes.CopyTo(childParentTypes, 0);
						childParentTypes[parentTypes.Length] = itemType;

						PropertyDescriptor[] childParentAccessors = new PropertyDescriptor[parentAccessors.Length + 1];
						
						parentAccessors.CopyTo(childParentAccessors, 0);
						childParentAccessors[parentAccessors.Length] = pd;

						PropertyDescriptorCollection pdch = 
							GetItemProperties(p.PropertyType, p.Name + "+", childParentTypes, childParentAccessors, listAccessors);

						list.AddRange(pdch);
					}
					else
						list.Add(pd);
				}

				list.Sort(new PropertyDescriptorComparer());

				pdc = new PropertyDescriptorCollection(
					(PropertyDescriptor[])list.ToArray(typeof(PropertyDescriptor)));

				if (md != null && propertyPrefix.Length == 0)
				{
					_descriptors[key] = pdc;
					_fieldCount [key] = md.AllMembersCount;
				}
			}
			else
			{
				if ((int)_fieldCount[key] != Map.Descriptor(itemType).AllMembersCount)
				{
					_descriptors.Remove(key);
					return GetItemProperties(itemType, propertyPrefix, parentTypes, parentAccessors, listAccessors);
				}
			}

			return pdc;
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

		internal EditableArrayList(Type itemType, ArrayList list)
		{
			if (itemType == null) throw new ArgumentNullException("itemType");
			if (list     == null) throw new ArgumentNullException("list");

			_itemType = itemType;
			_list     = list;

			//if (_itemType.IsAbstract)
			//	_itemType = Map.Descriptor(_itemType).MappedType;

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

		public static EditableArrayList Adapter(ArrayList list)
		{
			return new EditableArrayList(GetItemType(list), list);
		}

		public static new EditableArrayList Adapter(IList list)
		{
			return new EditableArrayList(GetItemType(list), ArrayList.Adapter(list));
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
			protected IsNullHandler      _isNull;

			protected string               _prefixedName;
			protected string               _namePrefix;
			protected PropertyDescriptor[] _chainAccessors;

			public StandardPropertyDescriptor(PropertyDescriptor pd, string namePrefix, PropertyDescriptor[] chainAccessors, IsNullHandler isNull)
				: base(pd)
			{
				_descriptor     = pd;
				_isNull         = isNull;
				_prefixedName   = namePrefix + pd.Name;
				_namePrefix     = namePrefix;
				_chainAccessors = chainAccessors;
			}

			protected object GetNestedComponent(object component)
			{
				if (_chainAccessors.Length > 0)
					for (int i = 0; i < _chainAccessors.Length; i++)
						component = _chainAccessors[i].GetValue(component);

				return component;
			}

			public override void SetValue(object component, object value)
			{
				_descriptor.SetValue(GetNestedComponent(component), value);
			}

			public override object GetValue(object component)
			{
				return CheckNull(_descriptor.GetValue(GetNestedComponent(component)));
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

			public override string Name
			{
				get { return _prefixedName; }
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

			protected object CheckNull(object value)
			{
				return _isNull != null && _isNull(value)? DBNull.Value: value;
			}
		}

		#endregion

		#region MapPropertyDescriptor

		class MapPropertyDescriptor : StandardPropertyDescriptor
		{
			private IMemberMapper _memberMapper;

			public MapPropertyDescriptor(PropertyDescriptor pd, string namePrefix, PropertyDescriptor[] chainAccessors, IMemberMapper mm, IsNullHandler isNull)
				: base(pd, namePrefix, chainAccessors, isNull)
			{
				_memberMapper = mm;
			}

			public override void SetValue(object component, object value)
			{
				_memberMapper.SetValue(GetNestedComponent(component), value);
			}

			public override object GetValue(object component)
			{
				return CheckNull(_memberMapper.GetValue(GetNestedComponent(component)));
			}
		}

		#endregion

		#region IListPropertyDescriptor

		class IListPropertyDescriptor : StandardPropertyDescriptor
		{
			private EditableArrayList _parent;

			public IListPropertyDescriptor(
				PropertyDescriptor descriptor, string namePrefix, PropertyDescriptor[] chainAccessors, EditableArrayList parent, IsNullHandler isNull)
				: base(descriptor, namePrefix, chainAccessors, isNull)
			{
				_parent = parent;
			}

			public override object GetValue(object component)
			{
				object value = base.GetValue(GetNestedComponent(component));

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

		#region SortMemberComparer

		class SortMemberComparer : IComparer
		{
			IMemberMapper      _member;
			ListSortDirection  _direction;

			public SortMemberComparer(IMemberMapper member, ListSortDirection direction)
			{
				if (member == null) throw new ArgumentNullException("member");

				_member    = member;
				_direction = direction;
			}

			public int Compare(object x, object y)
			{
				object a = _member.GetValue(x);
				object b = _member.GetValue(y);

				int n = Comparer.Default.Compare(a, b);

				return _direction == ListSortDirection.Ascending? n: -n;
			}
		}

		#endregion

		#region IDisposable

		public void Dispose()
		{
			Clear();
		}

		#endregion
	}
}
