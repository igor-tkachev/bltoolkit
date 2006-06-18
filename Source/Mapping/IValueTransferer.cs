using System;

namespace BLToolkit.Mapping
{
	[CLSCompliant(false)]
	public interface IValueTransferer
	{
		[CLSCompliant(false)]
		void Transfer(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex);
	}
}
