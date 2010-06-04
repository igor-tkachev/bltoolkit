using System;

namespace BLToolkit.Mapping
{
	public class NullGuidAttribute : NullValueAttribute
	{
		public NullGuidAttribute()
			: base(Guid.Empty)
		{
		}

		public NullGuidAttribute(byte[] b)
			: base(new Guid(b))
		{
		}

		public NullGuidAttribute(string g)
			: base(new Guid(g))
		{
		}

		public NullGuidAttribute(int a, short b, short c, byte[] d)
			: base(new Guid(a, b, c, d))
		{
		}

		public NullGuidAttribute(int a, short b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
			: base(new Guid(a, b, c, d, e, f, g, h, i, j, k))
		{
		}

		[CLSCompliant(false)]
		public NullGuidAttribute(uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
			: base(new Guid(a, b, c, d, e, f, g, h, i, j, k))
		{
		}
	}
}
