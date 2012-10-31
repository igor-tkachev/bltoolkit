using System;

namespace BLToolkit.Aspects
{
	[Flags]
	public enum InterceptType
	{
		BeforeCall = 0x01,
		AfterCall  = 0x02,
		OnCatch    = 0x04,
		OnFinally  = 0x08
	}
}
