using System;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Method)]
	public class IndexAttribute : Attribute
	{
		public IndexAttribute(params string[] fields)
		{
			_fields = fields;
		}

		public IndexAttribute(string field1)
			: this(new string[] { field1 })
		{
		}

		public IndexAttribute(string field1, string field2)
			: this(new string[] { field1, field2 })
		{
		}

		public IndexAttribute(string field1, string field2, string field3)
			: this(new string[] { field1, field2, field3 })
		{
		}

		public IndexAttribute(string field1, string field2, string field3, string field4)
			: this(new string[] { field1, field2, field3, field4 })
		{
		}

		public IndexAttribute(string field1, string field2, string field3, string field4, string field5)
			: this(new string[] { field1, field2, field3, field4, field5 })
		{
		}

		private string[] _fields;
		public  string[]  Fields
		{
			get { return _fields; }
		}
	}
}
