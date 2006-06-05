using System;

using BLToolkit.Common;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Method)]
	public class IndexAttribute : Attribute
	{
		public IndexAttribute(string[] names)
		{
			if (null == names)
				throw new ArgumentNullException("names");

			if (names.Length == 0)
				throw new ArgumentException("At least one field name must be specified", "names");

			_fields = NameOrIndexParameter.FromStringArray(names);
		}

		public IndexAttribute(int[] indices)
		{
			if (null == indices)
				throw new ArgumentNullException("indices");

			if (indices.Length == 0)
				throw new ArgumentException("At least one field ndex must be specified", "indices");

			_fields = NameOrIndexParameter.FromIndexArray(indices);
		}

		public IndexAttribute(params NameOrIndexParameter[] fields)
		{
			if (null == fields)
				throw new ArgumentNullException("fields");

			if (fields.Length == 0)
				throw new ArgumentException("At least one field name or index must be specified", "fields");
			
			_fields = fields;
		}

		public IndexAttribute(string field1)
			: this(new NameOrIndexParameter[] { field1 })
		{
		}

		public IndexAttribute(string field1, string field2)
			: this(new NameOrIndexParameter[] { field1, field2 })
		{
		}

		public IndexAttribute(string field1, string field2, string field3)
			: this(new NameOrIndexParameter[] { field1, field2, field3 })
		{
		}

		public IndexAttribute(string field1, string field2, string field3, string field4)
			: this(new NameOrIndexParameter[] { field1, field2, field3, field4 })
		{
		}

		public IndexAttribute(string field1, string field2, string field3, string field4, string field5)
			: this(new NameOrIndexParameter[] { field1, field2, field3, field4, field5 })
		{
		}

		public IndexAttribute(int field1)
			: this(new NameOrIndexParameter[] { field1 })
		{
		}

		public IndexAttribute(int field1, int field2)
			: this(new NameOrIndexParameter[] { field1, field2 })
		{
		}

		public IndexAttribute(int field1, int field2, int field3)
			: this(new NameOrIndexParameter[] { field1, field2, field3 })
		{
		}

		public IndexAttribute(int field1, int field2, int field3, int field4)
			: this(new NameOrIndexParameter[] { field1, field2, field3, field4 })
		{
		}

		public IndexAttribute(int field1, int field2, int field3, int field4, int field5)
			: this(new NameOrIndexParameter[] { field1, field2, field3, field4, field5 })
		{
		}

		private NameOrIndexParameter[] _fields;
		public  NameOrIndexParameter[]  Fields
		{
			get { return _fields; }
		}
	}
}
