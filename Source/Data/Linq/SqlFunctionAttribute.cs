using System;

namespace BLToolkit.Data.Linq
{
	[SerializableAttribute]
	[AttributeUsageAttribute(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class SqlFunctionAttribute : Attribute
	{
		public SqlFunctionAttribute()
		{
		}

		public SqlFunctionAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; set; }
	}
}
