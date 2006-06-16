using System;

namespace BLToolkit.Common
{
	public class Convert<T,P>
	{
		public delegate T CV(P p);

		private static CV Conv<T1,P1>(Convert<T1,P1>.CV op)
		{
			return (CV)(object)(Convert<T1,P1>.CV)op;
		}

		private static CV Conv<T1>(Convert<T1,P>.CV op)
		{
			return (CV)(object)(Convert<T1,P>.CV)op;
		}

		public  static CV From = GetConverter();
		private static CV GetConverter()
		{
			if (typeof(T) == typeof(SByte))
			{
				return Conv<SByte>(delegate(P p) { return System.Convert.ToSByte(p); });
			}

			if (typeof(T) == typeof(Byte))
			{
				return Conv<Byte>(delegate(P p) { return System.Convert.ToByte(p); });
			}

			if (typeof(T) == typeof(Int16))
			{
				return Conv<Int16>(delegate(P p) { return System.Convert.ToInt16(p); });
			}

			if (typeof(T) == typeof(Int32))
			{
				if (typeof(P) == typeof(Int32)) return Conv<Int32,Int32>(delegate(Int32 p) { return p; });

				return Conv<Int32>(delegate(P p) { return System.Convert.ToInt32(p); });
			}

			return delegate(P o) { return (T)System.Convert.ChangeType(o, typeof(T)); };
		}
	}

	public static class Convert<T>
	{
		public static T From<P>(P p)
		{
			return Convert<T, P>.From(p);
		}
	}
}
