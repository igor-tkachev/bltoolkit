using System;

namespace BLToolkit.Data.Linq
{
	[AttributeUsageAttribute(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class IgnoreIEnumerableAttribute : Attribute
	{
	}
}
