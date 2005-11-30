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

		#region ToObject

		public static object ToObject(object sourceObject, object destObject)
		{
			return _defaultSchema.ToObject(sourceObject, destObject);
		}

		public static object ToObject(object sourceObject, object destObject, params object[] parameters)
		{
			return _defaultSchema.ToObject(sourceObject, destObject, parameters);
		}

		public static object ToObject(object sourceObject, Type destObjectType)
		{
			return _defaultSchema.ToObject(sourceObject, destObjectType);
		}

#if FW2
		public static T ToObject<T>(object sourceObject)
		{
			return _defaultSchema.ToObject<T>(sourceObject);
		}
#endif

		public static object ToObject(object sourceObject, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.ToObject(sourceObject, destObjectType, parameters);
		}

#if FW2
		public static T ToObject<T>(object sourceObject, params object[] parameters)
		{
			return _defaultSchema.ToObject<T>(sourceObject, parameters);
		}
#endif

		#endregion
	}
}
