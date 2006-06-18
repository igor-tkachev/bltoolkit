using System;

namespace BLToolkit.Common
{
	public static class Convert<T,P>
	{
		public delegate T ConvertMethod(P p);

		private static ConvertMethod Conv<T1,P1>(Convert<T1,P1>.ConvertMethod op)
		{
			return (ConvertMethod)(object)op;
		}

		private static ConvertMethod Conv<T1>(Convert<T1,P>.ConvertMethod op)
		{
			return (ConvertMethod)(object)op;
		}

		public static ConvertMethod From = GetConverter();
		public static ConvertMethod GetConverter()
		{
			Type t = typeof(T);

			// Convert to the same type.
			//
			if (t.IsAssignableFrom(typeof(P)))
				return Conv<P>(delegate(P p) { return p; });

			if (t == typeof(SByte)) return GetSByteConverter();
			if (t == typeof(Int16)) return GetInt16Converter();
			if (t == typeof(Int32)) return GetInt32Converter();
			if (t == typeof(Int64)) return GetInt64Converter();

			if (t == typeof(Byte))  return GetByteConverter();

			return delegate(P o) { return (T)Convert.ChangeType(o, typeof(T)); };
		}

		private static ConvertMethod GetSByteConverter()
		{
			return Conv<SByte>(delegate(P p) { return Convert.ToSByte(p); });
		}

		private static ConvertMethod GetInt16Converter()
		{
			return Conv<Int16>(delegate(P p) { return Convert.ToInt16(p); });
		}

		private static ConvertMethod GetInt32Converter()
		{
			Type t = typeof(P);

			if (t == typeof(SByte))   return Conv<Int32,SByte>  (delegate(SByte   p) { return        p; });
			if (t == typeof(Int16))   return Conv<Int32,Int16>  (delegate(Int16   p) { return        p; });
			if (t == typeof(Int32))   return Conv<Int32,Int32>  (delegate(Int32   p) { return        p; });
			if (t == typeof(Int64))   return Conv<Int32,Int64>  (delegate(Int64   p) { return (Int32)p; });

			if (t == typeof(Byte))    return Conv<Int32,Byte>   (delegate(Byte    p) { return        p; });
			if (t == typeof(UInt16))  return Conv<Int32,UInt16> (delegate(UInt16  p) { return        p; });
			if (t == typeof(UInt32))  return Conv<Int32,UInt32> (delegate(UInt32  p) { return (Int32)p; });
			if (t == typeof(UInt64))  return Conv<Int32,UInt64> (delegate(UInt64  p) { return (Int32)p; });

			if (t == typeof(Char))    return Conv<Int32,Char>   (delegate(Char    p) { return        p; });
			if (t == typeof(Single))  return Conv<Int32,Single> (delegate(Single  p) { return (Int32)p; });
			if (t == typeof(Double))  return Conv<Int32,Double> (delegate(Double  p) { return (Int32)p; });
			if (t == typeof(Decimal)) return Conv<Int32,Decimal>(delegate(Decimal p) { return (Int32)p; });

			return Conv<Int32>(delegate(P p) { return Convert.ToInt32(p); });
		}

		private static ConvertMethod GetInt64Converter()
		{
			return Conv<Int64>(delegate(P p) { return Convert.ToInt64(p); });
		}

		private static ConvertMethod GetByteConverter()
		{
			return Conv<Byte>(delegate(P p) { return Convert.ToByte(p); });
		}
	}

	public static class ConvertTo<T>
	{
		public static T From<P>(P p)
		{
			return Convert<T, P>.From(p);
		}
	}
}
