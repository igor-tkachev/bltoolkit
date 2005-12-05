using System;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public interface IMapDataDestinationList
	{
		void                InitMapping       (InitContext initContext);
		IMapDataDestination GetDataDestination(InitContext initContext);
		object              GetNextObject     (InitContext initContext);
		void                EndMapping        (InitContext initContext);
	}
}
