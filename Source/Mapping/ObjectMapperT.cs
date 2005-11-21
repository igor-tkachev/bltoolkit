using System;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public static class ObjectMapper<T>
	{
		public static object CreateInstance()
		{
			return _mapper.CreateInstance();
		}

		public static object CreateInstance(InitContext context)
		{
			return _mapper.CreateInstance(context);
		}

		private static IObjectMapper _mapper = Map.GetObjectMapper(typeof(T));
		public  static IObjectMapper  Mapper
		{
			get { return _mapper; }
		}
	}
}
