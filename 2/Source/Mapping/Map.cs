using System;

namespace BLToolkit.Mapping
{
	public sealed class Map
	{
		private Map()
		{
		}

		private static Mapper _defaultMapper = new DefaultMapper();
		public  static Mapper  DefaultMapper
		{
			get { return _defaultMapper; }
		}

		public static IObjectMapper Mapper(Type type)
		{
			return _defaultMapper.GetObjectMapper(type);
		}
	}
}
