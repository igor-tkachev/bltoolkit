using System;

namespace BLToolkit.Data.Linq
{
	[SerializableAttribute]
	[AttributeUsageAttribute(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class SqlPropertyAttribute : SqlFunctionAttribute
	{
		public SqlPropertyAttribute()
		{
		}

		public SqlPropertyAttribute(string name)
			: base(name)
		{
		}
	}
}
