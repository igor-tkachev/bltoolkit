using System;

namespace BLToolkit.Common
{
	public class TypeHelper
	{
		public static bool IsSameOrParent(Type type,Type type1)
		{
			return true;
		}
	}

	public interface IConvertible<T,P>
	{
		T From(P p);
	}

	public static partial class Convert<T,P>
	{
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