using System;

namespace BLToolkit.Mapping
{
	public class NullDateTimeAttribute : NullValueAttribute
	{
		public NullDateTimeAttribute()
			: base(DateTime.MinValue)
		{
		}

		public NullDateTimeAttribute(int year, int month, int day)
			: base(new DateTime(year, month, day))
		{
		}

		public NullDateTimeAttribute(int year, int month, int day, int hour, int minute, int second)
			: base(new DateTime(year, month, day, hour, minute, second))
		{
		}

		public NullDateTimeAttribute(int year, int month, int day, int hour, int minute, int second, int millisecond)
			: base(new DateTime(year, month, day, hour, minute, second, millisecond))
		{
		}
	}
}
