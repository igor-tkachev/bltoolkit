using System;

using BLToolkit.Common;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Method), CLSCompliant(false)]
	public class IndexAttribute : Attribute
	{
		public IndexAttribute(params string[] names)
		{
			if (null == names)
				throw new ArgumentNullException("names");

			if (names.Length == 0)
				throw new ArgumentException("At least one field name must be specified", "names");

			_fields = NameOrIndexParameter.FromStringArray(names);
		}

		public IndexAttribute(params int[] indices)
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

		private readonly NameOrIndexParameter[] _fields;
		public           NameOrIndexParameter[]  Fields
		{
			get { return _fields; }
		}
	}
}
