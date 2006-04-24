using System;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Method)]
	public class ScalarFieldNameAttribute : Attribute
	{
		public ScalarFieldNameAttribute(string name)
		{
			_name = name;
		}

		private string _name;
		public  string  Name
		{
			get { return _name; }
		}
	}
}
