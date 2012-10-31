using System;

namespace BLToolkit.Data.Linq
{
	[AttributeUsageAttribute(AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
	public sealed class SqlEnumAttribute : Attribute
	{
	}
}
