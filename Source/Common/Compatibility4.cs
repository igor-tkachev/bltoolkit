namespace System
{
	using Collections;
	using Collections.Generic;
	using Text;

	public delegate void Action<T1,T2,T3,T4,T5>                                                 (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
	public delegate void Action<T1,T2,T3,T4,T5,T6>                                              (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
	public delegate void Action<T1,T2,T3,T4,T5,T6,T7>                                           (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
	public delegate void Action<T1,T2,T3,T4,T5,T6,T7,T8>                                        (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
	public delegate void Action<T1,T2,T3,T4,T5,T6,T7,T8,T9>                                     (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);
	public delegate void Action<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>                                 (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);
	public delegate void Action<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>                             (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);
	public delegate void Action<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>                         (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);
	public delegate void Action<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>                     (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);
	public delegate void Action<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>                 (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);
	public delegate void Action<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>             (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);
	public delegate void Action<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>         (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);

	public delegate TResult Func<T1,T2,T3,T4,T5,TResult>                                        (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
	public delegate TResult Func<T1,T2,T3,T4,T5,T6,TResult>                                     (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
	public delegate TResult Func<T1,T2,T3,T4,T5,T6,T7,TResult>                                  (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
	public delegate TResult Func<T1,T2,T3,T4,T5,T6,T7,T8,TResult>                               (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
	public delegate TResult Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,TResult>                            (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);
	public delegate TResult Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,TResult>                        (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);
	public delegate TResult Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,TResult>                    (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);
	public delegate TResult Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,TResult>                (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);
	public delegate TResult Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,TResult>            (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);
	public delegate TResult Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,TResult>        (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);
	public delegate TResult Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,TResult>    (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);
	public delegate TResult Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);

	#region Tuple

	interface ITuple
	{
		int    GetHashCode(IEqualityComparer comparer);
		string ToString   (StringBuilder sb);

		int Size { get; }
	}

	public static class Tuple
	{
		internal static int CombineHashCodes(int h1, int h2)                                                 { return (((h1 << 5) + h1) ^ h2);                                                              }
		internal static int CombineHashCodes(int h1, int h2, int h3)                                         { return CombineHashCodes(CombineHashCodes(h1, h2), h3);                                       }
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4)                                 { return CombineHashCodes(CombineHashCodes(h1, h2), CombineHashCodes(h3, h4));                 }
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5)                         { return CombineHashCodes(CombineHashCodes(h1, h2, h3, h4), h5);                               }
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6)                 { return CombineHashCodes(CombineHashCodes(h1, h2, h3, h4), CombineHashCodes(h5, h6));         }
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7)         { return CombineHashCodes(CombineHashCodes(h1, h2, h3, h4), CombineHashCodes(h5, h6, h7));     }
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8) { return CombineHashCodes(CombineHashCodes(h1, h2, h3, h4), CombineHashCodes(h5, h6, h7, h8)); }

		public static Tuple<T1>       Create<T1>      (T1 item1)                     { return new Tuple<T1>      (item1);               }
		public static Tuple<T1,T2>    Create<T1,T2>   (T1 item1, T2 item2)           { return new Tuple<T1,T2>   (item1, item2);        }
		public static Tuple<T1,T2,T3> Create<T1,T2,T3>(T1 item1, T2 item2, T3 item3) { return new Tuple<T1,T2,T3>(item1, item2, item3); }

		public static Tuple<T1,T2,T3,T4> Create<T1,T2,T3,T4>(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			return new Tuple<T1,T2,T3,T4>(item1, item2, item3, item4);
		}

		public static Tuple<T1,T2,T3,T4,T5> Create<T1,T2,T3,T4,T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			return new Tuple<T1,T2,T3,T4,T5>(item1, item2, item3, item4, item5);
		}

		public static Tuple<T1,T2,T3,T4,T5,T6> Create<T1,T2,T3,T4,T5,T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			return new Tuple<T1,T2,T3,T4,T5,T6>(item1, item2, item3, item4, item5, item6);
		}

		public static Tuple<T1,T2,T3,T4,T5,T6,T7> Create<T1,T2,T3,T4,T5,T6,T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			return new Tuple<T1,T2,T3,T4,T5,T6,T7>(item1, item2, item3, item4, item5, item6, item7);
		}

		public static Tuple<T1,T2,T3,T4,T5,T6,T7,Tuple<T8>> Create<T1,T2,T3,T4,T5,T6,T7,T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
		{
			return new Tuple<T1,T2,T3,T4,T5,T6,T7,Tuple<T8>>(item1, item2, item3, item4, item5, item6, item7, new Tuple<T8>(item8));
		}
	}

	[Serializable]
	public class Tuple<T1> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
	{
		readonly T1 _item1;

		public Tuple(T1 item1)
		{
			_item1 = item1;
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
				return 1;

			var tuple = other as Tuple<T1>;

			if (tuple == null)
				throw new ArgumentException(string.Format("Type '{0}' is not a tuple", other.GetType()), "other");

			return comparer.Compare(_item1, tuple._item1);
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
				return false;

			var tuple = other as Tuple<T1>;

			if (tuple == null)
				return false;

			return comparer.Equals(_item1, tuple._item1);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return comparer.GetHashCode(_item1);
		}

		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		int ITuple.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		string ITuple.ToString(StringBuilder sb)
		{
			sb.Append(_item1).Append(")");
			return sb.ToString();
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("(");
			return ((ITuple) this).ToString(sb);
		}

		public T1 Item1 { get { return _item1; } }

		int ITuple.Size { get { return 2; } }
	}

	[Serializable]
	public class Tuple<T1,T2> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
	{
		readonly T1 _item1;
		readonly T2 _item2;

		public Tuple(T1 item1, T2 item2)
		{
			_item1 = item1;
			_item2 = item2;
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
				return 1;

			var tuple = other as Tuple<T1,T2>;

			if (tuple == null)
				throw new ArgumentException(string.Format("Type '{0}' is not a tuple", other.GetType()), "other");

			var num = comparer.Compare(_item1, tuple._item1);
			if (num == 0)
				return num;
			return comparer.Compare(_item2, tuple._item2);
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
				return false;

			var tuple = other as Tuple<T1,T2>;

			if (tuple == null)
				return false;

			return
				comparer.Equals(_item1, tuple._item1) &&
				comparer.Equals(_item2, tuple._item2);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return Tuple.CombineHashCodes(comparer.GetHashCode(_item1), comparer.GetHashCode(_item2));
		}

		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		int ITuple.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		string ITuple.ToString(StringBuilder sb)
		{
			sb
				.Append(_item1).Append(", ")
				.Append(_item2).Append(")");

			return sb.ToString();
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("(");
			return ((ITuple) this).ToString(sb);
		}

		public T1 Item1 { get { return _item1; } }
		public T2 Item2 { get { return _item2; } }

		int ITuple.Size { get { return 2; } }
	}

	[Serializable]
	public class Tuple<T1,T2,T3> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
	{
		readonly T1 _item1;
		readonly T2 _item2;
		readonly T3 _item3;

		public Tuple(T1 item1, T2 item2, T3 item3)
		{
			_item1 = item1;
			_item2 = item2;
			_item3 = item3;
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
				return 1;

			var tuple = other as Tuple<T1,T2,T3>;

			if (tuple == null)
				throw new ArgumentException(string.Format("Type '{0}' is not a tuple", other.GetType()), "other");

			int num;

			num = comparer.Compare(_item1, tuple._item1); if (num == 0) return num;
			num = comparer.Compare(_item2, tuple._item2); if (num != 0) return num;

			return comparer.Compare(_item3, tuple._item3);
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
				return false;

			var tuple = other as Tuple<T1,T2,T3>;

			if (tuple == null)
				return false;

			return
				comparer.Equals(_item1, tuple._item1) &&
				comparer.Equals(_item2, tuple._item2) &&
				comparer.Equals(_item3, tuple._item3);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return
				Tuple.CombineHashCodes(
					comparer.GetHashCode(_item1),
					comparer.GetHashCode(_item2),
					comparer.GetHashCode(_item3));
		}

		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		int ITuple.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		string ITuple.ToString(StringBuilder sb)
		{
			sb
				.Append(_item1).Append(", ")
				.Append(_item2).Append(", ")
				.Append(_item3).Append(")");

			return sb.ToString();
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("(");
			return ((ITuple) this).ToString(sb);
		}

		public T1 Item1 { get { return _item1; } }
		public T2 Item2 { get { return _item2; } }
		public T3 Item3 { get { return _item3; } }

		int ITuple.Size { get { return 3; } }
	}

	[Serializable]
	public class Tuple<T1,T2,T3,T4> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
	{
		readonly T1 _item1;
		readonly T2 _item2;
		readonly T3 _item3;
		readonly T4 _item4;

		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			_item1 = item1;
			_item2 = item2;
			_item3 = item3;
			_item4 = item4;
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
				return 1;

			var tuple = other as Tuple<T1,T2,T3,T4>;

			if (tuple == null)
				throw new ArgumentException(string.Format("Type '{0}' is not a tuple", other.GetType()), "other");

			int num;

			num = comparer.Compare(_item1, tuple._item1); if (num == 0) return num;
			num = comparer.Compare(_item2, tuple._item2); if (num != 0) return num;
			num = comparer.Compare(_item3, tuple._item3); if (num != 0) return num;

			return comparer.Compare(_item4, tuple._item4);
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
				return false;

			var tuple = other as Tuple<T1,T2,T3,T4>;

			if (tuple == null)
				return false;

			return
				comparer.Equals(_item1, tuple._item1) &&
				comparer.Equals(_item2, tuple._item2) &&
				comparer.Equals(_item3, tuple._item3) &&
				comparer.Equals(_item4, tuple._item4);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return
				Tuple.CombineHashCodes(
					comparer.GetHashCode(_item1),
					comparer.GetHashCode(_item2),
					comparer.GetHashCode(_item3),
					comparer.GetHashCode(_item4));
		}

		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		int ITuple.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		string ITuple.ToString(StringBuilder sb)
		{
			sb
				.Append(_item1).Append(", ")
				.Append(_item2).Append(", ")
				.Append(_item3).Append(", ")
				.Append(_item4).Append(")");

			return sb.ToString();
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("(");
			return ((ITuple) this).ToString(sb);
		}

		public T1 Item1 { get { return _item1; } }
		public T2 Item2 { get { return _item2; } }
		public T3 Item3 { get { return _item3; } }
		public T4 Item4 { get { return _item4; } }

		int ITuple.Size { get { return 4; } }
	}

	[Serializable]
	public class Tuple<T1,T2,T3,T4,T5> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
	{
		readonly T1 _item1;
		readonly T2 _item2;
		readonly T3 _item3;
		readonly T4 _item4;
		readonly T5 _item5;

		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			_item1 = item1;
			_item2 = item2;
			_item3 = item3;
			_item4 = item4;
			_item5 = item5;
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
				return 1;

			var tuple = other as Tuple<T1,T2,T3,T4,T5>;

			if (tuple == null)
				throw new ArgumentException(string.Format("Type '{0}' is not a tuple", other.GetType()), "other");

			int num;

			num = comparer.Compare(_item1, tuple._item1); if (num == 0) return num;
			num = comparer.Compare(_item2, tuple._item2); if (num != 0) return num;
			num = comparer.Compare(_item3, tuple._item3); if (num != 0) return num;
			num = comparer.Compare(_item4, tuple._item4); if (num != 0) return num;

			return comparer.Compare(_item5, tuple._item5);
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
				return false;

			var tuple = other as Tuple<T1,T2,T3,T4,T5>;

			if (tuple == null)
				return false;

			return
				comparer.Equals(_item1, tuple._item1) &&
				comparer.Equals(_item2, tuple._item2) &&
				comparer.Equals(_item3, tuple._item3) &&
				comparer.Equals(_item4, tuple._item4) &&
				comparer.Equals(_item5, tuple._item5);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return
				Tuple.CombineHashCodes(
					comparer.GetHashCode(_item1),
					comparer.GetHashCode(_item2),
					comparer.GetHashCode(_item3),
					comparer.GetHashCode(_item4),
					comparer.GetHashCode(_item5));
		}

		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		int ITuple.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		string ITuple.ToString(StringBuilder sb)
		{
			sb
				.Append(_item1).Append(", ")
				.Append(_item2).Append(", ")
				.Append(_item3).Append(", ")
				.Append(_item4).Append(", ")
				.Append(_item5).Append(")");

			return sb.ToString();
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("(");
			return ((ITuple) this).ToString(sb);
		}

		public T1 Item1 { get { return _item1; } }
		public T2 Item2 { get { return _item2; } }
		public T3 Item3 { get { return _item3; } }
		public T4 Item4 { get { return _item4; } }
		public T5 Item5 { get { return _item5; } }

		int ITuple.Size { get { return 5; } }
	}

	[Serializable]
	public class Tuple<T1,T2,T3,T4,T5,T6> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
	{
		readonly T1 _item1;
		readonly T2 _item2;
		readonly T3 _item3;
		readonly T4 _item4;
		readonly T5 _item5;
		readonly T6 _item6;

		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			_item1 = item1;
			_item2 = item2;
			_item3 = item3;
			_item4 = item4;
			_item5 = item5;
			_item6 = item6;
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
				return 1;

			var tuple = other as Tuple<T1,T2,T3,T4,T5,T6>;

			if (tuple == null)
				throw new ArgumentException(string.Format("Type '{0}' is not a tuple", other.GetType()), "other");

			int num;

			num = comparer.Compare(_item1, tuple._item1); if (num == 0) return num;
			num = comparer.Compare(_item2, tuple._item2); if (num != 0) return num;
			num = comparer.Compare(_item3, tuple._item3); if (num != 0) return num;
			num = comparer.Compare(_item4, tuple._item4); if (num != 0) return num;
			num = comparer.Compare(_item5, tuple._item5); if (num != 0) return num;

			return comparer.Compare(_item6, tuple._item6);
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
				return false;

			var tuple = other as Tuple<T1,T2,T3,T4,T5,T6>;

			if (tuple == null)
				return false;

			return
				comparer.Equals(_item1, tuple._item1) &&
				comparer.Equals(_item2, tuple._item2) &&
				comparer.Equals(_item3, tuple._item3) &&
				comparer.Equals(_item4, tuple._item4) &&
				comparer.Equals(_item5, tuple._item5) &&
				comparer.Equals(_item6, tuple._item6);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return
				Tuple.CombineHashCodes(
					comparer.GetHashCode(_item1),
					comparer.GetHashCode(_item2),
					comparer.GetHashCode(_item3),
					comparer.GetHashCode(_item4),
					comparer.GetHashCode(_item5),
					comparer.GetHashCode(_item6));
		}

		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		int ITuple.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		string ITuple.ToString(StringBuilder sb)
		{
			sb
				.Append(_item1).Append(", ")
				.Append(_item2).Append(", ")
				.Append(_item3).Append(", ")
				.Append(_item4).Append(", ")
				.Append(_item5).Append(", ")
				.Append(_item6).Append(")");

			return sb.ToString();
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("(");
			return ((ITuple) this).ToString(sb);
		}

		public T1 Item1 { get { return _item1; } }
		public T2 Item2 { get { return _item2; } }
		public T3 Item3 { get { return _item3; } }
		public T4 Item4 { get { return _item4; } }
		public T5 Item5 { get { return _item5; } }
		public T6 Item6 { get { return _item6; } }

		int ITuple.Size { get { return 6; } }
	}

	[Serializable]
	public class Tuple<T1,T2,T3,T4,T5,T6,T7> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
	{
		readonly T1 _item1;
		readonly T2 _item2;
		readonly T3 _item3;
		readonly T4 _item4;
		readonly T5 _item5;
		readonly T6 _item6;
		readonly T7 _item7;

		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			_item1 = item1;
			_item2 = item2;
			_item3 = item3;
			_item4 = item4;
			_item5 = item5;
			_item6 = item6;
			_item7 = item7;
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
				return 1;

			var tuple = other as Tuple<T1,T2,T3,T4,T5,T6,T7>;

			if (tuple == null)
				throw new ArgumentException(string.Format("Type '{0}' is not a tuple", other.GetType()), "other");

			int num;

			num = comparer.Compare(_item1, tuple._item1); if (num == 0) return num;
			num = comparer.Compare(_item2, tuple._item2); if (num != 0) return num;
			num = comparer.Compare(_item3, tuple._item3); if (num != 0) return num;
			num = comparer.Compare(_item4, tuple._item4); if (num != 0) return num;
			num = comparer.Compare(_item5, tuple._item5); if (num != 0) return num;
			num = comparer.Compare(_item6, tuple._item6); if (num != 0) return num;

			return comparer.Compare(_item7, tuple._item7);
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
				return false;

			var tuple = other as Tuple<T1,T2,T3,T4,T5,T6,T7>;

			if (tuple == null)
				return false;

			return
				comparer.Equals(_item1, tuple._item1) &&
				comparer.Equals(_item2, tuple._item2) &&
				comparer.Equals(_item3, tuple._item3) &&
				comparer.Equals(_item4, tuple._item4) &&
				comparer.Equals(_item5, tuple._item5) &&
				comparer.Equals(_item6, tuple._item6) &&
				comparer.Equals(_item7, tuple._item7);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return
				Tuple.CombineHashCodes(
					comparer.GetHashCode(_item1),
					comparer.GetHashCode(_item2),
					comparer.GetHashCode(_item3),
					comparer.GetHashCode(_item4),
					comparer.GetHashCode(_item5),
					comparer.GetHashCode(_item6),
					comparer.GetHashCode(_item7));
		}

		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		int ITuple.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		string ITuple.ToString(StringBuilder sb)
		{
			sb
				.Append(_item1).Append(", ")
				.Append(_item2).Append(", ")
				.Append(_item3).Append(", ")
				.Append(_item4).Append(", ")
				.Append(_item5).Append(", ")
				.Append(_item6).Append(", ")
				.Append(_item7).Append(")");

			return sb.ToString();
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("(");
			return ((ITuple) this).ToString(sb);
		}

		public T1 Item1 { get { return _item1; } }
		public T2 Item2 { get { return _item2; } }
		public T3 Item3 { get { return _item3; } }
		public T4 Item4 { get { return _item4; } }
		public T5 Item5 { get { return _item5; } }
		public T6 Item6 { get { return _item6; } }
		public T7 Item7 { get { return _item7; } }

		int ITuple.Size { get { return 7; } }
	}

	[Serializable]
	public class Tuple<T1,T2,T3,T4,T5,T6,T7,TRest> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
	{
		readonly T1    _item1;
		readonly T2    _item2;
		readonly T3    _item3;
		readonly T4    _item4;
		readonly T5    _item5;
		readonly T6    _item6;
		readonly T7    _item7;
		readonly TRest _rest;

		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest)
		{
			if (!(rest is ITuple))
				throw new ArgumentException("Argument 'rest' is not a tuple", "rest");

			_item1 = item1;
			_item2 = item2;
			_item3 = item3;
			_item4 = item4;
			_item5 = item5;
			_item6 = item6;
			_item7 = item7;
			_rest  = rest;
		}

		public override bool Equals(object obj)
		{
			return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
		}

		public override int GetHashCode()
		{
			return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
				return 1;

			var tuple = other as Tuple<T1,T2,T3,T4,T5,T6,T7,TRest>;

			if (tuple == null)
				throw new ArgumentException(string.Format("Type '{0}' is not a tuple", other.GetType()), "other");

			int num;

			num = comparer.Compare(_item1, tuple._item1); if (num == 0) return num;
			num = comparer.Compare(_item2, tuple._item2); if (num != 0) return num;
			num = comparer.Compare(_item3, tuple._item3); if (num != 0) return num;
			num = comparer.Compare(_item4, tuple._item4); if (num != 0) return num;
			num = comparer.Compare(_item5, tuple._item5); if (num != 0) return num;
			num = comparer.Compare(_item6, tuple._item6); if (num != 0) return num;
			num = comparer.Compare(_item7, tuple._item7); if (num != 0) return num;

			return comparer.Compare(_rest, tuple._rest);
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null)
				return false;

			var tuple = other as Tuple<T1,T2,T3,T4,T5,T6,T7,TRest>;

			if (tuple == null)
				return false;

			return
				comparer.Equals(_item1, tuple._item1) &&
				comparer.Equals(_item2, tuple._item2) &&
				comparer.Equals(_item3, tuple._item3) &&
				comparer.Equals(_item4, tuple._item4) &&
				comparer.Equals(_item5, tuple._item5) &&
				comparer.Equals(_item6, tuple._item6) &&
				comparer.Equals(_item7, tuple._item7) &&
				comparer.Equals(_rest,  tuple._rest);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			var rest = (ITuple)_rest;

			if (rest.Size >= 8)
				return rest.GetHashCode(comparer);

			switch (8 - rest.Size)
			{
				case 1: return Tuple.CombineHashCodes(comparer.GetHashCode(_item7), rest.GetHashCode(comparer));
				case 2: return Tuple.CombineHashCodes(comparer.GetHashCode(_item6), comparer.GetHashCode(_item7), rest.GetHashCode(comparer));
				case 3: return Tuple.CombineHashCodes(comparer.GetHashCode(_item5), comparer.GetHashCode(_item6), comparer.GetHashCode(_item7), rest.GetHashCode(comparer));
				case 4: return Tuple.CombineHashCodes(comparer.GetHashCode(_item4), comparer.GetHashCode(_item5), comparer.GetHashCode(_item6), comparer.GetHashCode(_item7), rest.GetHashCode(comparer));
				case 5: return Tuple.CombineHashCodes(comparer.GetHashCode(_item3), comparer.GetHashCode(_item4), comparer.GetHashCode(_item5), comparer.GetHashCode(_item6), comparer.GetHashCode(_item7), rest.GetHashCode(comparer));
				case 6: return Tuple.CombineHashCodes(comparer.GetHashCode(_item2), comparer.GetHashCode(_item3), comparer.GetHashCode(_item4), comparer.GetHashCode(_item5), comparer.GetHashCode(_item6), comparer.GetHashCode(_item7), rest.GetHashCode(comparer));
				case 7: return Tuple.CombineHashCodes(comparer.GetHashCode(_item1), comparer.GetHashCode(_item2), comparer.GetHashCode(_item3), comparer.GetHashCode(_item4), comparer.GetHashCode(_item5), comparer.GetHashCode(_item6), comparer.GetHashCode(_item7), rest.GetHashCode(comparer));
			}

			return -1;
		}

		int IComparable.CompareTo(object obj)
		{
			return ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);
		}

		int ITuple.GetHashCode(IEqualityComparer comparer)
		{
			return ((IStructuralEquatable)this).GetHashCode(comparer);
		}

		string ITuple.ToString(StringBuilder sb)
		{
			sb
				.Append(_item1).Append(", ")
				.Append(_item2).Append(", ")
				.Append(_item3).Append(", ")
				.Append(_item4).Append(", ")
				.Append(_item5).Append(", ")
				.Append(_item6).Append(", ")
				.Append(_item7).Append(", ");

			return ((ITuple)_rest).ToString(sb);
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("(");
			return ((ITuple)this).ToString(sb);
		}

		public T1    Item1 { get { return _item1; } }
		public T2    Item2 { get { return _item2; } }
		public T3    Item3 { get { return _item3; } }
		public T4    Item4 { get { return _item4; } }
		public T5    Item5 { get { return _item5; } }
		public T6    Item6 { get { return _item6; } }
		public T7    Item7 { get { return _item7; } }
		public TRest Rest  { get { return _rest;  } }

		int ITuple.Size { get { return 7 + ((ITuple)_rest).Size; } }
	}

	#endregion

	namespace Collections
	{
		public interface IStructuralEquatable
		{
			bool Equals     (object other, IEqualityComparer comparer);
			int  GetHashCode(IEqualityComparer comparer);
		}

		public interface IStructuralComparable
		{
			int CompareTo(object other, IComparer comparer);
		}
	}
}
