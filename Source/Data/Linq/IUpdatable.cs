using System;

namespace BLToolkit.Data.Linq
{
	public interface IUpdatable<
#if !FW3
out
#endif
T>
	{
	}
}
