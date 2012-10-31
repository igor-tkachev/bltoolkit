using System;

using BLToolkit.Common;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
	public class DataSetTableAttribute : Attribute
	{
		public DataSetTableAttribute()
		{
		}

		public DataSetTableAttribute(string name)
		{
			_nameOrIndex = name;
		}

		public DataSetTableAttribute(int index)
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
