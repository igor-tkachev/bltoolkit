using System;
using System.Data;
using System.Collections;
#if FW2
using System.Collections.Generic;
#endif

namespace BLToolkit.Mapping
{
	public 
#if FW2
	static
#endif
	class Map
	{
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

		#region Base Mapping

		public static void SourceToDestination(object sourceObject, object destObject, params object[] parameters)
		{
			_defaultSchema.MapSourceToDestination(sourceObject, destObject, parameters);
		}

		public static void SourceListToDestinationList(
			IMapDataSourceList      dataSourceList,
			IMapDataDestinationList dataDestinationList,
			params object[]         parameters)
		{
			_defaultSchema.MapSourceListToDestinationList(dataSourceList, dataDestinationList, parameters);
		}

		#endregion

		#region ValueToEnum, EnumToValue

		public static object ValueToEnum(object value, Type type)
		{
			return _defaultSchema.MapValueToEnum(value, type);
		}

		public static object EnumToValue(object value)
		{
			return _defaultSchema.MapEnumToValue(value);
		}
#if FW2

		public static T ToEnum<T>(object value)
		{
			return (T)_defaultSchema.MapValueToEnum(value, typeof(T));
		}

#endif

		#endregion

		#region Object

		#region ObjectToObject

		public static object ObjectToObject(object sourceObject, object destObject, params object[] parameters)
		{
			return _defaultSchema.MapObjectToObject(sourceObject, destObject, parameters);
		}

		public static object ObjectToObject(object sourceObject, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.MapObjectToObject(sourceObject, destObjectType, parameters);
		}

#if FW2
		public static T ObjectToObject<T>(object sourceObject, params object[] parameters)
		{
			return (T)_defaultSchema.MapObjectToObject(sourceObject, typeof(T), parameters);
		}
#endif

		#endregion

		#region ObjectToDataRow

		public static DataRow ObjectToDataRow(object sourceObject, DataRow destRow)
		{
			return _defaultSchema.MapObjectToDataRow(sourceObject, destRow);
		}

		public static DataRow ObjectToDataRow(object sourceObject, DataTable destTable)
		{
			return _defaultSchema.MapObjectToDataRow(sourceObject, destTable);
		}

		#endregion

		#region ObjectToDictionary

		public static IDictionary ObjectToDictionary(object sourceObject, IDictionary destDictionary)
		{
			return _defaultSchema.MapObjectToDictionary(sourceObject, destDictionary);
		}

		public static Hashtable ObjectToDictionary(object sourceObject)
		{
			return _defaultSchema.MapObjectToDictionary(sourceObject);
		}

		#endregion

		#endregion

		#region DataRow

		#region DataRowToObject

		public static object DataRowToObject(DataRow dataRow, object destObject, params object[] parameters)
		{
			return _defaultSchema.MapDataRowToObject(dataRow, destObject, parameters);
		}

		public static object DataRowToObject(
			DataRow dataRow, DataRowVersion version, object destObject, params object[] parameters)
		{
			return _defaultSchema.MapDataRowToObject(dataRow, version, destObject, parameters);
		}

		public static object DataRowToObject(DataRow dataRow, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.MapDataRowToObject(dataRow, destObjectType, parameters);
		}

		public static object DataRowToObject(
			DataRow dataRow, DataRowVersion version, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.MapDataRowToObject(dataRow, version, destObjectType, parameters);
		}

#if FW2
		public static T DataRowToObject<T>(DataRow dataRow, params object[] parameters)
		{
			return (T)_defaultSchema.MapDataRowToObject(dataRow, typeof(T), parameters);
		}

		public static T DataRowToObject<T>(DataRow dataRow, DataRowVersion version, params object[] parameters)
		{
			return (T)_defaultSchema.MapDataRowToObject(dataRow, version, typeof(T), parameters);
		}
#endif

		#endregion

		#region DataRowToDataRow

		public static DataRow DataRowToDataRow(DataRow sourceRow, DataRow destRow)
		{
			return _defaultSchema.MapDataRowToDataRow(sourceRow, destRow);
		}

		public static DataRow DataRowToDataRow(DataRow sourceRow, DataRowVersion version, DataRow destRow)
		{
			return _defaultSchema.MapDataRowToDataRow(sourceRow, version, destRow);
		}

		public static DataRow DataRowToDataRow(DataRow sourceRow, DataTable destTable)
		{
			return _defaultSchema.MapDataRowToDataRow(sourceRow, destTable);
		}

		public static DataRow DataRowToDataRow(DataRow sourceRow, DataRowVersion version, DataTable destTable)
		{
			return _defaultSchema.MapDataRowToDataRow(sourceRow, version, destTable);
		}

		#endregion

		#region DataRowToDictionary

		public static IDictionary DataRowToDictionary(DataRow sourceRow, IDictionary destDictionary)
		{
			return _defaultSchema.MapDataRowToDictionary(sourceRow, destDictionary);
		}

		public static Hashtable DataRowToDictionary(DataRow sourceRow)
		{
			return _defaultSchema.MapDataRowToDictionary(sourceRow);
		}

		public static IDictionary DataRowToDictionary(
			DataRow sourceRow, DataRowVersion version, IDictionary destDictionary)
		{
			return _defaultSchema.MapDataRowToDictionary(sourceRow, version, destDictionary);
		}

		public static Hashtable DataRowToDictionary(DataRow sourceRow, DataRowVersion version)
		{
			return _defaultSchema.MapDataRowToDictionary(sourceRow, version);
		}

		#endregion

		#endregion

		#region DataReader

		#region DataReaderToObject

		public static object DataReaderToObject(IDataReader dataReader, object destObject, params object[] parameters)
		{
			return _defaultSchema.MapDataReaderToObject(dataReader, destObject, parameters);
		}

		public static object DataReaderToObject(IDataReader dataReader, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.MapDataReaderToObject(dataReader, destObjectType, parameters);
		}

#if FW2
		public static T DataReaderToObject<T>(IDataReader dataReader, params object[] parameters)
		{
			return (T)_defaultSchema.MapDataReaderToObject(dataReader, typeof(T), parameters);
		}
#endif

		#endregion

		#region DataReaderToDataRow

		public static DataRow DataReaderToDataRow(IDataReader dataReader, DataRow destRow)
		{
			return _defaultSchema.MapDataReaderToDataRow(dataReader, destRow);
		}

		public static DataRow DataReaderToDataRow(IDataReader dataReader, DataTable destTable)
		{
			return _defaultSchema.MapDataReaderToDataRow(dataReader, destTable);
		}

		#endregion

		#region DataReaderToDictionary

		public static IDictionary DataReaderToDictionary(IDataReader dataReader, IDictionary destDictionary)
		{
			return _defaultSchema.MapDataReaderToDictionary(dataReader, destDictionary);
		}

		public static Hashtable DataReaderToDictionary(IDataReader dataReader)
		{
			return _defaultSchema.MapDataReaderToDictionary(dataReader);
		}

		#endregion

		#endregion

		#region Dictionary

		#region DictionaryToObject

		public static object DictionaryToObject(
			IDictionary sourceDictionary, object destObject, params object[] parameters)
		{
			return _defaultSchema.MapDictionaryToObject(sourceDictionary, destObject, parameters);
		}

		public static object DictionaryToObject(
			IDictionary sourceDictionary, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.MapDictionaryToObject(sourceDictionary, destObjectType, parameters);
		}

#if FW2
		public static T DictionaryToObject<T>(IDictionary sourceDictionary, params object[] parameters)
		{
			return (T)_defaultSchema.MapDictionaryToObject(sourceDictionary, typeof(T), parameters);
		}
#endif

		#endregion

		#region DictionaryToDataRow

		public static DataRow DictionaryToDataRow(IDictionary sourceDictionary, DataRow destRow)
		{
			return _defaultSchema.MapDictionaryToDataRow(sourceDictionary, destRow);
		}

		public static DataRow DictionaryToDataRow(IDictionary sourceDictionary, DataTable destTable)
		{
			return _defaultSchema.MapDictionaryToDataRow(sourceDictionary, destTable);
		}

		#endregion

		#endregion

		#region List

		#region ListToList

		public static IList ListToList(
			ICollection     sourceList,
			IList           destList,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapListToList(sourceList, destList, destObjectType, parameters);
		}

		public static ArrayList ListToList(ICollection sourceList, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.MapListToList(sourceList, destObjectType, parameters);
		}

#if FW2
		public static List<T> ListToList<T>(ICollection sourceList, List<T> destList, params object[] parameters)
		{
			return _defaultSchema.MapListToList<T>(sourceList, destList, parameters);
		}

		public static List<T> ListToList<T>(ICollection sourceList, params object[] parameters)
		{
			return _defaultSchema.MapListToList<T>(sourceList, parameters);
		}
#endif

		#endregion

		#region ListToTable

		public static DataTable ListToTable(ICollection sourceList, DataTable destTable)
		{
			return _defaultSchema.MapListToTable(sourceList, destTable);
		}

		public static DataTable ListToTable(ICollection sourceList)
		{
			return _defaultSchema.MapListToTable(sourceList);
		}

		#endregion

		#region MapListToDictionary

		public static IDictionary ListToDictionary(
			ICollection     sourceList,
			IDictionary     destDictionary,
			string          keyFieldName,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapListToDictionary(
				sourceList, destDictionary, keyFieldName, destObjectType, parameters);
		}

		public static Hashtable ListToDictionary(
			ICollection     sourceList,
			string          keyFieldName,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapListToDictionary(sourceList, keyFieldName, destObjectType, parameters);
		}

#if FW2
		public static Dictionary<K,T> ListToDictionary<K,T>(
			ICollection     sourceList,
			Dictionary<K,T> destDictionary,
			string          keyFieldName,
			params object[] parameters)
		{
			return _defaultSchema.MapListToDictionary<K,T>(sourceList, destDictionary, keyFieldName, parameters);
		}

		public static Dictionary<K,T> ListToDictionary<K,T>(
			ICollection     sourceList,
			string          keyFieldName,
			params object[] parameters)
		{
			return _defaultSchema.MapListToDictionary<K,T>(sourceList, keyFieldName, parameters);
		}
#endif

		#endregion

		#endregion

		#region Table

		#region TableToTable

		public static DataTable TableToTable(DataTable sourceTable, DataTable destTable)
		{
			return _defaultSchema.MapTableToTable(sourceTable, destTable);
		}

		public static DataTable TableToTable(DataTable sourceTable, DataRowVersion version, DataTable destTable)
		{
			return _defaultSchema.MapTableToTable(sourceTable, version, destTable);
		}

		public static DataTable TableToTable(DataTable sourceTable)
		{
			return _defaultSchema.MapTableToTable(sourceTable);
		}

		public static DataTable TableToTable(DataTable sourceTable, DataRowVersion version)
		{
			return _defaultSchema.MapTableToTable(sourceTable, version);
		}

		#endregion

		#region TableToList

		public static IList TableToList(
			DataTable sourceTable, IList list, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.MapTableToList(sourceTable, list, destObjectType, parameters);
		}

		public static IList TableToList(
			DataTable       sourceTable,
			DataRowVersion  version,
			IList           list,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapTableToList(sourceTable, version, list, destObjectType, parameters);
		}

		public static ArrayList TableToList(DataTable sourceTable, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.MapTableToList(sourceTable, destObjectType, parameters);
		}

		public static ArrayList TableToList(
			DataTable sourceTable, DataRowVersion version, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.MapTableToList(sourceTable, version, destObjectType, parameters);
		}

#if FW2
		public static List<T> TableToList<T>(DataTable sourceTable, List<T> list, params object[] parameters)
		{
			return _defaultSchema.MapTableToList<T>(sourceTable, list, parameters);
		}

		public static List<T> TableToList<T>(
			DataTable       sourceTable,
			DataRowVersion  version,
			List<T>         list,
			params object[] parameters)
		{
			return _defaultSchema.MapTableToList<T>(sourceTable, version, list, parameters);
		}

		public static List<T> TableToList<T>(DataTable sourceTable, params object[] parameters)
		{
			return _defaultSchema.MapTableToList<T>(sourceTable, parameters);
		}

		public static List<T> TableToList<T>(DataTable sourceTable, DataRowVersion version, params object[] parameters)
		{
			return _defaultSchema.MapTableToList<T>(sourceTable, version, parameters);
		}
#endif

		#endregion

		#region TableToDictionary

		public static IDictionary TableToDictionary(
			DataTable       sourceTable,
			IDictionary     destDictionary,
			string          keyFieldName,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapTableToDictionary(
				sourceTable, destDictionary, keyFieldName, destObjectType, parameters);
		}

		public static Hashtable TableToDictionary(
			DataTable       sourceTable,
			string          keyFieldName,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapTableToDictionary(sourceTable, keyFieldName, destObjectType, parameters);
		}

#if FW2
		public static Dictionary<K,T> TableToDictionary<K,T>(
			DataTable       sourceTable,
			Dictionary<K,T> destDictionary,
			string          keyFieldName,
			params object[] parameters)
		{
			return _defaultSchema.MapTableToDictionary<K,T>(
				sourceTable, destDictionary, keyFieldName, parameters);
		}

		public static Dictionary<K,T> TableToDictionary<K,T>(
			DataTable       sourceTable,
			string          keyFieldName,
			params object[] parameters)
		{
			return _defaultSchema.MapTableToDictionary<K,T>(sourceTable, keyFieldName, parameters);
		}
#endif

		#endregion

		#endregion

		#region DataReader

		#region DataReaderToList

		public static IList DataReaderToList(
			IDataReader     reader,
			IList           list,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapDataReaderToList(reader, list, destObjectType, parameters);
		}

		public static ArrayList DataReaderToList(IDataReader reader, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.MapDataReaderToList(reader, destObjectType, parameters);
		}

#if FW2
		public static List<T> DataReaderToList<T>(IDataReader reader, List<T> list, params object[] parameters)
		{
			return _defaultSchema.MapDataReaderToList<T>(reader, list, parameters);
		}

		public static List<T> DataReaderToList<T>(IDataReader reader, params object[] parameters)
		{
			return _defaultSchema.MapDataReaderToList<T>(reader, parameters);
		}
#endif

		#endregion

		#region DataReaderToTable

		public static DataTable DataReaderToTable(IDataReader reader, DataTable destTable)
		{
			return _defaultSchema.MapDataReaderToTable(reader, destTable);
		}

		public static DataTable DataReaderToTable(IDataReader reader)
		{
			return _defaultSchema.MapDataReaderToTable(reader);
		}

		#endregion

		#region DataReaderToDictionary

		public static IDictionary DataReaderToDictionary(
			IDataReader     dataReader,
			IDictionary     destDictionary,
			string          keyFieldName,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapDataReaderToDictionary(
				dataReader, destDictionary, keyFieldName, destObjectType, parameters);
		}

		public static Hashtable DataReaderToDictionary(
			IDataReader     dataReader,
			string          keyFieldName,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapDataReaderToDictionary(dataReader, keyFieldName, destObjectType, parameters);
		}

#if FW2
		public static Dictionary<K,T> DataReaderToDictionary<K,T>(
			IDataReader     dataReader,
			Dictionary<K,T> destDictionary,
			string          keyFieldName,
			params object[] parameters)
		{
			return _defaultSchema.MapDataReaderToDictionary<K,T>(
				dataReader, destDictionary, keyFieldName, parameters);
		}

		public static Dictionary<K,T> DataReaderToDictionary<K,T>(
			IDataReader     dataReader,
			string          keyFieldName,
			params object[] parameters)
		{
			return _defaultSchema.MapDataReaderToDictionary<K,T>(dataReader, keyFieldName, parameters);
		}
#endif

		#endregion

		#endregion

		#region Dictionary

		#region DictionaryToList

		public static IList DictionaryToList(
			IDictionary     sourceDictionary,
			IList           destList,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapDictionaryToList(sourceDictionary, destList, destObjectType, parameters);
		}

		public static ArrayList DictionaryToList(
			IDictionary     sourceDictionary,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapDictionaryToList(sourceDictionary, destObjectType, parameters);
		}

#if FW2
		public static List<T> DictionaryToList<T>(
			IDictionary     sourceDictionary,
			List<T>         destList,
			params object[] parameters)
		{
			return _defaultSchema.MapDictionaryToList<T>(sourceDictionary, destList, parameters);
		}

		public static List<T> DictionaryToList<T>(IDictionary sourceDictionary, params object[] parameters)
		{
			return _defaultSchema.MapDictionaryToList<T>(sourceDictionary, parameters);
		}
#endif

		#endregion

		#region DictionaryToTable

		public static DataTable DictionaryToTable(IDictionary sourceDictionary, DataTable destTable)
		{
			return _defaultSchema.MapDictionaryToTable(sourceDictionary, destTable);
		}

		public static DataTable DictionaryToTable(IDictionary sourceDictionary)
		{
			return _defaultSchema.MapDictionaryToTable(sourceDictionary);
		}

		#endregion

		#region DictionaryToDictionary

		public static IDictionary DictionaryToDictionary(
			IDictionary     sourceDictionary,
			IDictionary     destDictionary,
			string          keyFieldName,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapDictionaryToDictionary(
				sourceDictionary, destDictionary, keyFieldName, destObjectType, parameters);
		}

		public static Hashtable DictionaryToDictionary(
			IDictionary     sourceDictionary,
			string          keyFieldName,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapDictionaryToDictionary(
				sourceDictionary, keyFieldName, destObjectType, parameters);
		}

#if FW2
		public static Dictionary<K,T> DictionaryToDictionary<K,T>(
			IDictionary     sourceDictionary,
			Dictionary<K,T> destDictionary,
			string          keyFieldName,
			params object[] parameters)
		{
			return _defaultSchema.MapDictionaryToDictionary<K,T>(
				sourceDictionary, destDictionary, keyFieldName, parameters);
		}

		public static Dictionary<K,T> DictionaryToDictionary<K,T>(
			IDictionary     sourceDictionary,
			string          keyFieldName,
			params object[] parameters)
		{
			return _defaultSchema.MapDictionaryToDictionary<K,T>(sourceDictionary, keyFieldName, parameters);
		}
#endif

		#endregion

		#endregion
	}
}
