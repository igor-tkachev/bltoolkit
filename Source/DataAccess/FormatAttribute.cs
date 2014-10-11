using System;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public class FormatAttribute : NoMapAttribute
	{
		public FormatAttribute()
		{
			_index = int.MaxValue;
		}

		public FormatAttribute(int index)
		{
			_index = index;
		}

		private readonly int _index;
		public           int  Index
		{
			get { return _index; }
		}
	}
}
