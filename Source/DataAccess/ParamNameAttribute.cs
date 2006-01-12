using System;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public class ParamNameAttribute : Attribute
	{
		public ParamNameAttribute(string name)
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
