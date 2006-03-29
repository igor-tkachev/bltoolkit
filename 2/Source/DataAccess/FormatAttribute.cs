using System;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public class FormatAttribute : Attribute
	{
		public FormatAttribute()
		{
			_index = int.MaxValue;
		}

		public FormatAttribute(int index)
		{
			_index = index;
		}

		private int _index;
		public  int  Index
		{
			get { return _index; }
		}
	}
}
