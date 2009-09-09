using System;

namespace BLToolkit.Data.Linq
{
	[SerializableAttribute]
	[AttributeUsageAttribute(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class SqlFunctionAttribute : Attribute
	{
		public SqlFunctionAttribute()
		{
		}

		public SqlFunctionAttribute(string name)
		{
			Name = name;
		}

		public SqlFunctionAttribute(string sqlProvider, string name)
		{
			SqlProvider = sqlProvider;
			Name        = name;
		}

		public string SqlProvider    { get; set; }
		public string Name           { get; set; }
		public bool   ServerSideOnly { get; set; }
	}
}
