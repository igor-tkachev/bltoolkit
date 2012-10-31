using System;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	[CLSCompliant(false)]
	public interface IMapDataDestinationList
	{
		void                InitMapping       (InitContext initContext);
		[CLSCompliant(false)]
		IMapDataDestination GetDataDestination(InitContext initContext);
		object              GetNextObject     (InitContext initContext);
		void                EndMapping        (InitContext initContext);
	}
}
