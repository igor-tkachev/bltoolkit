using System;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class DefaultMapper : Mapper
	{
		protected override IObjectMapper CreateMapper(Type type)
		{
			IObjectMapper om = new ObjectMapper();

			om.Init(TypeAccessor.GetAccessor(type));

			return om;
		}
	}
}
