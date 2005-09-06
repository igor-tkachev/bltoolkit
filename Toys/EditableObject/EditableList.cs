using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Rsdn.Framework.EditableObject
{
	[Serializable, ComVisible(true)]
	public class EditableList<T> : EditableArrayList
	{
		#region Public Members

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

		#endregion

		#region Like List<T> Methods

		public T Find(Predicate<T> match)
		{
			if (match == null)
				new ArgumentNullException("match");

			foreach (T t in List)
				if (match(t))
					return t;

			return default(T);
		}

		public EditableList<T> FindAll(Predicate<T> match)
		{
			if (match == null)
				new ArgumentNullException("match");

			EditableList<T> list = new EditableList<T>();

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
				new ArgumentNullException("match");

			for (int i = startIndex; i < startIndex + count; i++)
				if (match((T)List[i]))
					return i;

			return -1;
		}

		public T FindLast(Predicate<T> match)
		{
			if (match == null)
				new ArgumentNullException("match");

			for (int i = List.Count - 1; i >= 0; i--)
			{
				T t = (T)List[i];

				if (match(t))
					return t;
			}

			return default(T);
		}

		public int FindLastIndex(Predicate<T> match)
		{
			return this.FindLastIndex(List.Count - 1, List.Count, match);
		}

		public int FindLastIndex(int startIndex, Predicate<T> match)
		{
			return this.FindLastIndex(startIndex, startIndex + 1, match);
		}

		public int FindLastIndex(int startIndex, int count, Predicate<T> match)
		{
			if (startIndex >= List.Count)
				throw new ArgumentOutOfRangeException("startIndex");

			if (count < 0 || startIndex - count + 1 < 0)
				throw new ArgumentOutOfRangeException("count");

			if (match == null)
				throw new ArgumentNullException("match");

			for (int i = startIndex; i > startIndex - count; i--)
			{
				T t = (T)List[i];

				if (match(t))
					return i;
			}

			return -1;
		}

		public void ForEach(Action<T> action)
		{
			if (action == null)
				throw new ArgumentNullException("match");

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
				T[] items = new T[count];

				List.CopyTo(index, items, 0, count);
				Array.Sort<T>(items, index, count, comparer);

				for (int i = 0; i < count; i++)
					List[i + index] = items[i];

				OnListChanged(ListChangedType.ItemChanged, 0);
			}
		}

		public void Sort(Comparison<T> comparison)
		{
			if (List.Count > 1)
			{
				T[] items = new T[List.Count];

				List.CopyTo(items);
				Array.Sort<T>(items, comparison);

				for (int i = 0; i < List.Count; i++)
					List[i] = items[i];

				OnListChanged(ListChangedType.ItemChanged, 0);
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

		public static new EditableList<T> Adapter(ArrayList list)
		{
			return new EditableList<T>(list);
		}

		public static new EditableList<T> Adapter(IList list)
		{
			return new EditableList<T>(ArrayList.Adapter(list));
		}

		#endregion
	}
}
