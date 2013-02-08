using System;
using System.Reflection;

namespace BLToolkit.TypeBuilder
{
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public class RefCursorAttribute : Attribute
	{
	}

	public static class RefCursorAttributeHelpers
	{
		public static bool IsRefCursor(this ParameterInfo pi)
		{
			return pi.GetCustomAttributes(typeof(RefCursorAttribute), false).Length > 0;
		}
	}
}
