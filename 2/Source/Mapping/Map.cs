using System;
using System.Data;
using System.Collections;
#if FW2
using System.Collections.Generic;
#endif

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

		public static object ToObject(object sourceObject, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.ToObject(sourceObject, destObjectType, parameters);
		}

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

		public static object ToObject(
			DataRow dataRow, DataRowVersion version, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.ToObject(dataRow, version, destObjectType, parameters);
		}

#if FW2
		public static T ToObject<T>(object sourceObject)
		{
			return _defaultSchema.ToObject<T>(sourceObject);
		}

		public static T ToObject<T>(object sourceObject, params object[] parameters)
		{
			return _defaultSchema.ToObject<T>(sourceObject, parameters);
		}

		public static T ToObject<T>(DataRow dataRow, DataRowVersion version)
		{
			return _defaultSchema.ToObject<T>(dataRow, version);
		}

		public static T ToObject<T>(DataRow dataRow, DataRowVersion version, params object[] parameters)
		{
			return _defaultSchema.ToObject<T>(dataRow, version, parameters);
		}
#endif

		#endregion

		#region ListToList

		public static void SourceListToDestinationList(
			IMapDataSourceList      dataSourceList,
			IMapDataDestinationList dataDestinationList,
			params object[]         parameters)
		{
			_defaultSchema.SourceListToDestinationList(dataSourceList, dataDestinationList, parameters);
		}

		public static IList ListToList(
			ICollection     sourceList,
			IList           destList,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.ListToList(sourceList, destList, destObjectType, parameters);
		}

		public static ArrayList ListToList(ICollection sourceList, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.ListToList(sourceList, destObjectType, parameters);
		}

#if FW2
		public static List<T> ListToList<T>(ICollection sourceList, List<T> destList, params object[] parameters)
		{
			return _defaultSchema.ListToList<T>(sourceList, destList, parameters);
		}

		public static List<T> ListToList<T>(ICollection sourceList, params object[] parameters)
		{
			return _defaultSchema.ListToList<T>(sourceList, parameters);
		}
#endif

		#endregion

		#region ListToTable

		public static DataTable ListToTable(ICollection sourceList, DataTable destTable)
		{
			return _defaultSchema.ListToTable(sourceList, destTable);
		}

		public static DataTable ListToTable(ICollection sourceList)
		{
			return _defaultSchema.ListToTable(sourceList);
		}

		#endregion

		#region TableToTable

		public static DataTable TableToTable(DataTable sourceTable, DataTable destTable)
		{
			return _defaultSchema.TableToTable(sourceTable, destTable);
		}

		public static DataTable TableToTable(DataTable sourceTable, DataRowVersion version, DataTable destTable)
		{
			return _defaultSchema.TableToTable(sourceTable, version, destTable);
		}

		public static DataTable TableToTable(DataTable sourceTable)
		{
			return _defaultSchema.TableToTable(sourceTable);
		}

		public static DataTable TableToTable(DataTable sourceTable, DataRowVersion version)
		{
			return _defaultSchema.TableToTable(sourceTable, version);
		}

		#endregion

		#region TableToList

		public static IList TableToList(
			DataTable sourceTable, IList list, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.TableToList(sourceTable, list, destObjectType, parameters);
		}

		public static IList TableToList(
			DataTable       sourceTable,
			DataRowVersion  version,
			IList           list,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.TableToList(sourceTable, version, list, destObjectType, parameters);
		}

		public static ArrayList TableToList(DataTable sourceTable, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.TableToList(sourceTable, destObjectType, parameters);
		}

		public static ArrayList TableToList(
			DataTable sourceTable, DataRowVersion version, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.TableToList(sourceTable, version, destObjectType, parameters);
		}

#if FW2
		public static List<T> TableToList<T>(DataTable sourceTable, List<T> list, params object[] parameters)
		{
			return _defaultSchema.TableToList<T>(sourceTable, list, parameters);
		}

		public static List<T> TableToList<T>(
			DataTable       sourceTable,
			DataRowVersion  version,
			List<T>         list,
			params object[] parameters)
		{
			return _defaultSchema.TableToList<T>(sourceTable, version, list, parameters);
		}

		public static List<T> TableToList<T>(DataTable sourceTable, params object[] parameters)
		{
			return _defaultSchema.TableToList<T>(sourceTable, parameters);
		}

		public static List<T> TableToList<T>(DataTable sourceTable, DataRowVersion version, params object[] parameters)
		{
			return _defaultSchema.TableToList<T>(sourceTable, version, parameters);
		}
#endif

		#endregion

		#region DataReaderToList

		public static IList DataReaderToList(
			IDataReader     reader,
			IList           list,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.DataReaderToList(reader, list, destObjectType, parameters);
		}

		public static ArrayList DataReaderToList(IDataReader reader, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.DataReaderToList(reader, destObjectType, parameters);
		}

#if FW2
		public static List<T> DataReaderToList<T>(IDataReader reader, List<T> list, params object[] parameters)
		{
			return _defaultSchema.DataReaderToList<T>(reader, list, parameters);
		}

		public static List<T> DataReaderToList<T>(IDataReader reader, params object[] parameters)
		{
			return _defaultSchema.DataReaderToList<T>(reader, parameters);
		}
#endif

		#endregion
	}
}
