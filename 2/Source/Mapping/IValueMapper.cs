using System;

namespace BLToolkit.Mapping
{
	[CLSCompliant(false)]
	public interface IValueMapper
	{
		[CLSCompliant(false)]
		void Map(
			IMapDataSource      source, object sourceObject, int sourceIndex,
			IMapDataDestination dest,   object destObject,   int destIndex);
	}
}
