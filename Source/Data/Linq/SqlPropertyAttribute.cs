using System;

namespace BLToolkit.Data.Linq
{
	[SerializableAttribute]
	[AttributeUsageAttribute(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class SqlPropertyAttribute : SqlFunctionAttribute
	{
		public SqlPropertyAttribute()
		{
		}

		public SqlPropertyAttribute(string name)
			: base(name)
		{
		}

		public SqlPropertyAttribute(string sqlProvider, string name)
			: base(sqlProvider, name)
		{
		}
	}
}
