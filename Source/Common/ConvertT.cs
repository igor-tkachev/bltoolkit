using System;
using System.Reflection;

using BLToolkit.Reflection;

namespace BLToolkit.Common
{
	public static class Convert<T,P>
	{
		public delegate T ConvertMethod(P p);

		public static T[] FromArray(P[] src)
		{
			return Array.ConvertAll<P,T>(src, (Converter<P,T>)((object)From));
		}

		public static ConvertMethod From = GetConverter();
		public static ConvertMethod GetConverter()
		{
			string methodName;

			Type from = typeof(P);
			Type to   = typeof(T);

			// Convert to the same type.
			//
			if (TypeHelper.IsSameOrParent(to, from))
				return (ConvertMethod)(object)(Convert<P,P>.ConvertMethod)SameType;

			if (TypeHelper.IsNullable(to))
				methodName = "ToNullable" + to.GetGenericArguments()[0].Name;
			else if (to.IsArray)
				methodName = "To" + to.GetElementType().Name + "Array";
			else
				methodName = "To" + to.Name;

			MethodInfo     mi = typeof(Convert).GetMethod(methodName,
				BindingFlags.Public | BindingFlags.Static,
				null, new Type[] {from}, null);

			if (null != mi)
				return (ConvertMethod)(object)Delegate.CreateDelegate(typeof(ConvertMethod), mi);

			return Default;
		}

		private static P SameType(P p) { return p; }
		private static T Default(P p)  { return (T)System.Convert.ChangeType(p, typeof(T)); }
	}

	public static class ConvertTo<T>
	{
		public static T From<P>(P p)
		{
			return Convert<T,P>.From(p);
		}
	}
}
