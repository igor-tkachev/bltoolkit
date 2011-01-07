using System;

namespace BLToolkit.Data.Linq
{
	[SerializableAttribute]
	[AttributeUsageAttribute(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class TableFunctionAttribute : Attribute
	{
		public TableFunctionAttribute()
		{
		}

		public TableFunctionAttribute(string name)
		{
			Name = name;
		}

		public TableFunctionAttribute(string sqlProvider, string name)
		{
			SqlProvider = sqlProvider;
			Name        = name;
		}

		public string SqlProvider { get; set; }
		public string Name        { get; set; }
	}
}
