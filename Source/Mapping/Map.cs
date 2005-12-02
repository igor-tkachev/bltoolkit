using System;
using System.Data;

namespace BLToolkit.Mapping
{
	public sealed class Map
	{
		private Map()
		{
		}

		#region Public Members
		
		private static MappingSchema _defaultSchema = new MappingSchema();
		public  static MappingSchema  DefaultSchema
		{
			get { return _defaultSchema;  }
			set { _defaultSchema = value; }
		}

		public static ObjectMapper GetObjectMapper(Type type)
		{
			return _defaultSchema.GetObjectMapper(type);
		}

		#endregion

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

		public static object ToObject(DataRow dataRow, DataRowVersion version, object destObject)
		{
			return _defaultSchema.ToObject(dataRow, version, destObject);
		}

		public static object ToObject(
			DataRow dataRow, DataRowVersion version, object destObject, params object[] parameters)
		{
			return _defaultSchema.ToObject(dataRow, version, destObject, parameters);
		}

		public static object ToObject(DataRow dataRow, DataRowVersion version, Type destObjectType)
		{
			return _defaultSchema.ToObject(dataRow, version, destObjectType);
		}

#if FW2
		public static T ToObject<T>(DataRow dataRow, DataRowVersion version)
		{
			return _defaultSchema.ToObject<T>(dataRow, version);
		}
#endif

		public static object ToObject(
			DataRow dataRow, DataRowVersion version, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.ToObject(dataRow, version, destObjectType, parameters);
		}

#if FW2
		public static T ToObject<T>(DataRow dataRow, DataRowVersion version, params object[] parameters)
		{
			return _defaultSchema.ToObject<T>(dataRow, version, parameters);
		}
#endif

		#endregion
	}
}
