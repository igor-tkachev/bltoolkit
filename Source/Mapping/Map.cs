using System;

namespace BLToolkit.Mapping
{
	public sealed class Map
	{
		private Map()
		{
		}

		private static Mapper _defaultMapper = new DefaultMapper();
		public  static Mapper  DefaulMapper
		{
			get { return _defaultMapper;  }
			set { _defaultMapper = value; }
		}

		public static IObjectMapper Mapper(Type type)
		{
			return _defaultMapper.GetObjectMapper(type);
		}
	}
}
