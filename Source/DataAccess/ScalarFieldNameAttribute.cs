using System;

using BLToolkit.Common;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Method)]
	public class ScalarFieldNameAttribute : Attribute
	{
		public ScalarFieldNameAttribute(string name)
		{
			_nameOrIndex = name;
		}

		public ScalarFieldNameAttribute(int index)
		{
			_nameOrIndex = index;
		}

		private NameOrIndexParameter _nameOrIndex;
		public  NameOrIndexParameter  NameOrIndex
		{
			get { return _nameOrIndex; }
			set { _nameOrIndex = value; }
		}
	}
}
