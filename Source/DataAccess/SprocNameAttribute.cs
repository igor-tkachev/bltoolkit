using System;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Method)]
	public class SprocNameAttribute : Attribute
	{
		public SprocNameAttribute(string name)
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
