using System;

namespace BLToolkit.Mapping
{
	public sealed class Map
	{
		private Map()
		{
		}

		private static MappingSchema _defaultSchema = new MappingSchema();
		public  static MappingSchema  DefaultSchema
		{
			get { return _defaultSchema;  }
			set { _defaultSchema = value; }
		}

		public static IObjectMapper GetObjectMapper(Type type)
		{
			return _defaultSchema.GetObjectMapper(type);
		}

		#region ToEnum, FromEnum

		public static object ToEnum(object value, Type type)
		{
			return _defaultSchema.ToEnum(value, type);
		}

		public static object FromEnum(object value)
		{
			return _defaultSchema.FromEnum(value);
		}

#if FW2

		public static T ToEnum<T>(object value)
		{
			return (T)_defaultSchema.ToEnum(value, typeof(T));
		}

#endif

		#endregion
	}
}
