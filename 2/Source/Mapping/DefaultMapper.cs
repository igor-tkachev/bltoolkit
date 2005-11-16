using System;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	class DefaultMapper : Mapper
	{
		public override IObjectMapper GetObjectMapper(Type type)
		{
			IObjectMapper om = (IObjectMapper)Mappers[type];

			if (om == null)
				Mappers[type] = om = CreateMapper(type);

			return om;
		}

		private static IObjectMapper CreateMapper(Type type)
		{
			//TypeHelper   typeHelper = new TypeHelper(type);
			TypeAccessor accessor   = TypeAccessor.GetAccessor(type);

			ObjectMapper om = new ObjectMapper();

			om.SetTypeAccessor(accessor);

			return om;
		}
	}
}
