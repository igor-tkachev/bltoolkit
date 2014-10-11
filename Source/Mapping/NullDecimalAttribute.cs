using System;

namespace BLToolkit.Mapping
{
	public class NullDecimalAttribute : NullValueAttribute
	{
		public NullDecimalAttribute()
			: base(0m)
		{
		}

		public NullDecimalAttribute(decimal nullValue)
			: base(nullValue)
		{
		}

		public NullDecimalAttribute(double value)
			: base(new Decimal(value))
		{
		}

		public NullDecimalAttribute(int[] bits)
			: base(new Decimal(bits))
		{
		}

		public NullDecimalAttribute(long value)
			: base(new Decimal(value))
		{
		}

		[CLSCompliant(false)]
		public NullDecimalAttribute(ulong value)
			: base(new Decimal(value))
		{
		}

		public NullDecimalAttribute(int lo, int mid, int hi, bool isNegative, byte scale)
			: base(new Decimal(lo, mid, hi, isNegative, scale))
		{
		}
	}
}
