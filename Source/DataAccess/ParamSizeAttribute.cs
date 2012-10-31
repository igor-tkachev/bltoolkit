using System;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public class ParamSizeAttribute : Attribute
	{
		public ParamSizeAttribute(int size)
		{
			_size = size;
		}

		private readonly int _size;
		public           int  Size
		{
			get { return _size; }
		}
	}
}
