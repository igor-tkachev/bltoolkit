using System;

namespace BLToolkit.Data.Sql
{
	public class Parameter
	{
		public Parameter(string name)
		{
			_name = name;
		}

		private string _name;
		public  string  Name { get { return _name; } set { _name = value; } }
	}
}
