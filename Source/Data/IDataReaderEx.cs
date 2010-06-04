using System;

namespace BLToolkit.Data
{
	public interface IDataReaderEx
	{
#if FW3
		DateTimeOffset GetDateTimeOffset(int i);
#endif
	}
}
