using System;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public interface IObjectMapper : IMapDataSource, IMapDataDestination
	{
		object CreateInstance();
		object CreateInstance(InitContext context);

		void Init(MappingSchema mappingSchema, TypeAccessor typeAccessor);
	}
}
