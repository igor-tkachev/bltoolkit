using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace BLToolkit.EditableObjects
{
	[Serializable]
	[DebuggerDisplay("Count = {Count}")]
	public class EditableList<T> : EditableArrayList, IList<T>
	{
		#region Constructors

		public EditableList()
			: base(typeof(T))
		{
		}

		public EditableList(int capacity)
			: base(typeof(T), capacity)
		{
		}

		public EditableList(ICollection c)
			: base(typeof(T), c)
		{
		}

		public EditableList(bool trackChanges)
			: base(typeof(T), trackChanges)
		{
		}

		public EditableList(int capacity, bool trackChanges)
			: base(typeof(T), capacity, trackChanges)
		{
		}

		public EditableList(ICollection c, bool trackChanges)
			: base(typeof(T), c, trackChanges)
		{
		}

		public EditableList(EditableList<T> list) 
			: base(list, true)
		{
		}

		public EditableList(EditableList<T> list, bool trackChanges)
			: base(list, trackChanges)
		{
		}

		#endregion

		#region Typed Methods

		public override object Clone()
		{
			return Clone(new EditableList<T>((ArrayList)List.Clone()));
		}

		public new T this[int index]
		{
			get { return (T)base[index]; }
			set { base[index] = value; }
		}

		public new T[] ToArray()
		{
			return (T[])base.ToArray(typeof(T));
		}

		public new T AddNew()
		{
			return (T)base.AddNew();
		}

		public int RemoveAll(Predicate<T> match)
		{
			var n = 0;

			for (var i = 0; i < Count; i++)
			{
				var item = this[i];

				if (match(item))
				{
					Remove((object)item);
					i--;
					n++;
				}
			}

			return n;
		}

		#endregion

		#region Like List<T> Methods

		public T Find(Predicate<T> match)
		{
			if (match == null) throw new ArgumentNullException("match");

			foreach (T t in List)
				if (match(t))
					return t;

			return default(T);
		}

		public EditableList<T> FindAll(Predicate<T> match)
		{
			if (match == null) throw new ArgumentNullException("match");

			var list = new EditableList<T>();

			foreach (T t in List)
				if (match(t))
					list.Add(t);

			return list;
		}

		public int FindIndex(Predicate<T> match)
		{
			return FindIndex(0, List.Count, match);
		}

		public int FindIndex(int startIndex, Predicate<T> match)
		{
			return FindIndex(startIndex, List.Count - startIndex, match);
		}

		public int FindIndex(int startIndex, int count, Predicate<T> match)
		{
			if (startIndex > List.Count)
				throw new ArgumentOutOfRangeException("startIndex");

			if (count < 0 || startIndex > List.Count - count)
				throw new ArgumentOutOfRangeException("count");

			if (match == null)
				throw new ArgumentNullException("match");

			for (var i = startIndex; i < startIndex + count; i++)
				if (match((T)List[i]))
					return i;

			return -1;
		}

		public T FindLast(Predicate<T> match)
		{
			if (match == null) throw new ArgumentNullException("match");

			for (var i = List.Count - 1; i >= 0; i--)
			{
				var t = (T)List[i];

				if (match(t))
					return t;
			}

			return default(T);
		}

		public int FindLastIndex(Predicate<T> match)
		{
			return FindLastIndex(List.Count - 1, List.Count, match);
		}

		public int FindLastIndex(int startIndex, Predicate<T> match)
		{
			return FindLastIndex(startIndex, startIndex + 1, match);
		}

		public int FindLastIndex(int startIndex, int count, Predicate<T> match)
		{
			if (startIndex >= List.Count)
				throw new ArgumentOutOfRangeException("startIndex");

			if (count < 0 || startIndex - count + 1 < 0)
				throw new ArgumentOutOfRangeException("count");

			if (match == null)
				throw new ArgumentNullException("match");

			for (var i = startIndex; i > startIndex - count; i--)
			{
				var t = (T)List[i];

				if (match(t))
					return i;
			}

			return -1;
		}

		public void ForEach(Action<T> action)
		{
			if (action == null) throw new ArgumentNullException("action");

			foreach (T t in List)
				action(t);
		}

		public void Sort(IComparer<T> comparer)
		{
			Sort(0, List.Count, comparer);
		}

		public void Sort(int index, int count, IComparer<T> comparer)
		{
			if (List.Count > 1 && count > 1)
			{
				var items = new T[count];

				List.CopyTo(index, items, 0, count);
				Array.Sort(items, index, count, comparer);

				for (var i = 0; i < count; i++)
					List[i + index] = items[i];

				OnListChanged(ListChangedType.Reset, 0);
			}
		}

		public void Sort(Comparison<T> comparison)
		{
			if (List.Count > 1)
			{
				var items = new T[List.Count];

				List.CopyTo(items);
				Array.Sort(items, comparison);

				for (var i = 0; i < List.Count; i++)
					List[i] = items[i];

				OnListChanged(ListChangedType.Reset, 0);
			}
		}

		#endregion

		#region Static Methods

		internal EditableList(ArrayList list)
			: base(typeof(T), list)
		{
		}

		public static EditableList<T> Adapter(List<T> list)
		{
			return new EditableList<T>(ArrayList.Adapter(list));
		}

		public static new EditableList<T> Adapter(IList list)
		{
			return list is ArrayList? 
				new EditableList<T>((ArrayList)list):
				new EditableList<T>(ArrayList.Adapter(list));
		}

		#endregion

		#region IList<T> Members

		public int IndexOf(T item)
		{
			return IndexOf((object)item);
		}

		public void Insert(int index, T item)
		{
			Insert(index, (object)item);
		}

		#endregion

		#region ICollection<T> Members

		public void Add(T item)
		{
			Add((object)item);
		}

		public bool Contains(T item)
		{
			return Contains((object)item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			CopyTo((Array)array, arrayIndex);
		}

		public bool Remove(T item)
		{
			if (Contains(item) == false)
				return false;

			Remove((object)item);

			return true;
		}

		#endregion

		#region IEnumerable<T> Members

		public new IEnumerator<T> GetEnumerator()
		{
			return new Enumerator(List.GetEnumerator());
		}

		class Enumerator : IEnumerator<T>
		{
			public Enumerator(IEnumerator enumerator)
			{
				_enumerator = enumerator;
			}

			private readonly IEnumerator _enumerator;

			#region IEnumerator<T> Members

			T IEnumerator<T>.Current
			{
				get { return (T)_enumerator.Current; }
			}

			#endregion

			#region IEnumerator Members

			object IEnumerator.Current
			{
				get { return _enumerator.Current; }
			}

			bool IEnumerator.MoveNext()
			{
				return _enumerator.MoveNext();
			}

			void IEnumerator.Reset()
			{
				_enumerator.Reset();
			}

			#endregion

			#region IDisposable Members

			void IDisposable.Dispose()
			{
			}

			#endregion
		}

		#endregion
	}
}

