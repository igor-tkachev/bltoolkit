using System;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TableNameAttribute : Attribute
	{
		public TableNameAttribute(string name)
		{
			_name = name;
		}

		private readonly string _name;
		public           string  Name
		{
			get { return _name; }
		}
	}
}
