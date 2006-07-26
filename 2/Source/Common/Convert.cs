using System;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;

using BLToolkit.Reflection;

namespace BLToolkit.Common
{
	public interface IConvertible<T,P>
	{
		T From(P p);
	}

	public static class Convert<T,P>
	{
		public static T From(P p)
		{
			return Instance.From(p);
		}

		public static T[] FromArray(P[] src)
		{
			T[] dst = new T[src.Length];

			for (int i = 0; i < src.Length; ++i)
			{
				dst[i] = Instance.From(src[i]);
			}

			return dst;
		}

		public static IConvertible<T,P> Instance = GetConverter();

		private static IConvertible<T,P> GetConverter()
		{
			if (TypeHelper.IsSameOrParent(typeof(T), typeof(P)))
			{
				return (IConvertible<T,P>)(object)(new ConvertAssignable<P,P>());
			}

			return new ConvertExplicit<T,P>();
		}
	}

	public static class ConvertTo<T>
	{
		public static T From<P>(P p)
		{
			return Convert<T,P>.From(p);
		}
	}

	internal class ConvertAssignable<T,P>: IConvertible<T,P> where P: T
	{
		T IConvertible<T,P>.From(P p) { return p; }
	}

	internal class ConvertDefault<T,P>: IConvertible<T,P>
	{
		T IConvertible<T,P>.From(P p) { return (T)Convert.ChangeType(p, typeof(T)); }
	}

	internal partial class ConvertPartial<T,P>: ConvertDefault<T,P>
	{
	}

	internal partial class ConvertExplicit<T,P>: ConvertPartial<T,P>
	{
	}
}
