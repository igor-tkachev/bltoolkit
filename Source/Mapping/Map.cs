using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace BLToolkit.Mapping
{
	using Common;
	using Reflection;
	using Reflection.Extension;

	public class Map
	{
		#region Public Members
		
		private static MappingSchema _defaultSchema = new DefaultMappingSchema();
		public  static MappingSchema  DefaultSchema
		{
			[System.Diagnostics.DebuggerStepThrough]
			get { return _defaultSchema;  }
			set { _defaultSchema = value; }
		}

		public static ExtensionList Extensions
		{
			[System.Diagnostics.DebuggerStepThrough]
			get { return _defaultSchema.Extensions;  }
			set { _defaultSchema.Extensions = value; }
		}

		public static ObjectMapper GetObjectMapper(Type type)
		{
			return _defaultSchema.GetObjectMapper(type);
		}

		#endregion

		#region GetNullValue

		public static object GetNullValue(Type type)
		{
			return _defaultSchema.GetNullValue(type);
		}

		public static bool IsNull(object value)
		{
			return _defaultSchema.IsNull(value);
		}

		#endregion

		#region Base Mapping

		public static void SourceToDestination(object sourceObject, object destObject, params object[] parameters)
		{
			_defaultSchema.MapSourceToDestination(sourceObject, destObject, parameters);
		}

		[CLSCompliant(false)]
		public static void MapSourceToDestination(
			IMapDataSource      source, object sourceObject, 
			IMapDataDestination dest,   object destObject,
			params object[]     parameters)
		{
			_defaultSchema.MapSourceToDestination(source, sourceObject, dest, destObject, parameters);
		}

		[CLSCompliant(false)]
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

		public static T ToEnum<T>(object value)
		{
			return (T)_defaultSchema.MapValueToEnum(value, typeof(T));
		}

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

		public static T ObjectToObject<T>(object sourceObject, params object[] parameters)
		{
			return (T)_defaultSchema.MapObjectToObject(sourceObject, typeof(T), parameters);
		}

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

		public static T DataRowToObject<T>(DataRow dataRow, params object[] parameters)
		{
			return (T)_defaultSchema.MapDataRowToObject(dataRow, typeof(T), parameters);
		}

		public static T DataRowToObject<T>(DataRow dataRow, DataRowVersion version, params object[] parameters)
		{
			return (T)_defaultSchema.MapDataRowToObject(dataRow, version, typeof(T), parameters);
		}

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

		public static T DataReaderToObject<T>(IDataReader dataReader, params object[] parameters)
		{
			return (T)_defaultSchema.MapDataReaderToObject(dataReader, typeof(T), parameters);
		}

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

		public static T DictionaryToObject<T>(IDictionary sourceDictionary, params object[] parameters)
		{
			return (T)_defaultSchema.MapDictionaryToObject(sourceDictionary, typeof(T), parameters);
		}

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

		public static List<T> ListToList<T>(ICollection sourceList, List<T> destList, params object[] parameters)
		{
			return _defaultSchema.MapListToList<T>(sourceList, destList, parameters);
		}

		public static List<T> ListToList<T>(ICollection sourceList, params object[] parameters)
		{
			return _defaultSchema.MapListToList<T>(sourceList, parameters);
		}

		#endregion

		#region ListToDataTable

		public static DataTable ListToDataTable(ICollection sourceList, DataTable destTable)
		{
			return _defaultSchema.MapListToDataTable(sourceList, destTable);
		}

		public static DataTable ListToDataTable(ICollection sourceList)
		{
			return _defaultSchema.MapListToDataTable(sourceList);
		}

		#endregion

		#region MapListToDictionary

		public static IDictionary ListToDictionary(
			ICollection          sourceList,
			IDictionary          destDictionary,
			NameOrIndexParameter keyField,
			Type                 destObjectType,
			params object[]      parameters)
		{
			return _defaultSchema.MapListToDictionary(
				sourceList, destDictionary, keyField, destObjectType, parameters);
		}

		public static Hashtable ListToDictionary(
			ICollection          sourceList,
			NameOrIndexParameter keyField,
			Type                 destObjectType,
			params object[]      parameters)
		{
			return _defaultSchema.MapListToDictionary(sourceList, keyField, destObjectType, parameters);
		}

		public static IDictionary<K,T> ListToDictionary<K,T>(
			ICollection          sourceList,
			IDictionary<K,T>     destDictionary,
			NameOrIndexParameter keyField,
			params object[]      parameters)
		{
			return _defaultSchema.MapListToDictionary<K,T>(sourceList, destDictionary, keyField, parameters);
		}

		public static Dictionary<K,T> ListToDictionary<K,T>(
			ICollection          sourceList,
			NameOrIndexParameter keyField,
			params object[]      parameters)
		{
			return _defaultSchema.MapListToDictionary<K,T>(sourceList, keyField, parameters);
		}

		#endregion

		#region MapListToDictionary (Index)

		public static IDictionary ListToDictionary(
			ICollection     sourceList,
			IDictionary     destDictionary,
			MapIndex        index,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapListToDictionary(
				sourceList, destDictionary, index, destObjectType, parameters);
		}

		public static Hashtable ListToDictionary(
			ICollection     sourceList,
			MapIndex        index,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapListToDictionary(sourceList, index, destObjectType, parameters);
		}

		public static IDictionary<CompoundValue,T> ListToDictionary<T>(
			ICollection                  sourceList,
			IDictionary<CompoundValue,T> destDictionary,
			MapIndex                     index,
			params object[]              parameters)
		{
			return _defaultSchema.MapListToDictionary<T>(sourceList, destDictionary, index, parameters);
		}

		public static Dictionary<CompoundValue,T> ListToDictionary<T>(
			ICollection     sourceList,
			MapIndex        index,
			params object[] parameters)
		{
			return _defaultSchema.MapListToDictionary<T>(sourceList, index, parameters);
		}

		#endregion

		#endregion

		#region DataTable

		#region DataTableToDataTable

		public static DataTable DataTableToDataTable(DataTable sourceTable, DataTable destTable)
		{
			return _defaultSchema.MapDataTableToDataTable(sourceTable, destTable);
		}

		public static DataTable DataTableToDataTable(DataTable sourceTable, DataRowVersion version, DataTable destTable)
		{
			return _defaultSchema.MapDataTableToDataTable(sourceTable, version, destTable);
		}

		public static DataTable DataTableToDataTable(DataTable sourceTable)
		{
			return _defaultSchema.MapDataTableToDataTable(sourceTable);
		}

		public static DataTable DataTableToDataTable(DataTable sourceTable, DataRowVersion version)
		{
			return _defaultSchema.MapDataTableToDataTable(sourceTable, version);
		}

		#endregion

		#region DataTableToList

		public static IList DataTableToList(
			DataTable sourceTable, IList list, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.MapDataTableToList(sourceTable, list, destObjectType, parameters);
		}

		public static IList DataTableToList(
			DataTable       sourceTable,
			DataRowVersion  version,
			IList           list,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapDataTableToList(sourceTable, version, list, destObjectType, parameters);
		}

		public static ArrayList DataTableToList(DataTable sourceTable, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.MapDataTableToList(sourceTable, destObjectType, parameters);
		}

		public static ArrayList DataTableToList(
			DataTable sourceTable, DataRowVersion version, Type destObjectType, params object[] parameters)
		{
			return _defaultSchema.MapDataTableToList(sourceTable, version, destObjectType, parameters);
		}

		public static List<T> DataTableToList<T>(DataTable sourceTable, List<T> list, params object[] parameters)
		{
			return _defaultSchema.MapDataTableToList<T>(sourceTable, list, parameters);
		}

		public static List<T> DataTableToList<T>(
			DataTable       sourceTable,
			DataRowVersion  version,
			List<T>         list,
			params object[] parameters)
		{
			return _defaultSchema.MapDataTableToList<T>(sourceTable, version, list, parameters);
		}

		public static List<T> DataTableToList<T>(DataTable sourceTable, params object[] parameters)
		{
			return _defaultSchema.MapDataTableToList<T>(sourceTable, parameters);
		}

		public static List<T> DataTableToList<T>(DataTable sourceTable, DataRowVersion version, params object[] parameters)
		{
			return _defaultSchema.MapDataTableToList<T>(sourceTable, version, parameters);
		}

		#endregion

		#region DataTableToDictionary

		public static IDictionary DataTableToDictionary(
			DataTable            sourceTable,
			IDictionary          destDictionary,
			NameOrIndexParameter keyField,
			Type                 destObjectType,
			params object[]      parameters)
		{
			return _defaultSchema.MapDataTableToDictionary(
				sourceTable, destDictionary, keyField, destObjectType, parameters);
		}

		public static Hashtable DataTableToDictionary(
			DataTable            sourceTable,
			NameOrIndexParameter keyField,
			Type                 destObjectType,
			params object[]      parameters)
		{
			return _defaultSchema.MapDataTableToDictionary(sourceTable, keyField, destObjectType, parameters);
		}

		public static IDictionary<K,T> DataTableToDictionary<K,T>(
			DataTable            sourceTable,
			IDictionary<K,T>     destDictionary,
			NameOrIndexParameter keyField,
			params object[]      parameters)
		{
			return _defaultSchema.MapDataTableToDictionary<K,T>(
				sourceTable, destDictionary, keyField, parameters);
		}

		public static Dictionary<K,T> DataTableToDictionary<K,T>(
			DataTable            sourceTable,
			NameOrIndexParameter keyField,
			params object[]      parameters)
		{
			return _defaultSchema.MapDataTableToDictionary<K,T>(sourceTable, keyField, parameters);
		}

		#endregion

		#region DataTableToDictionary (Index)

		public static IDictionary DataTableToDictionary(
			DataTable       sourceTable,
			IDictionary     destDictionary,
			MapIndex        index,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapDataTableToDictionary(
				sourceTable, destDictionary, index, destObjectType, parameters);
		}

		public static Hashtable DataTableToDictionary(
			DataTable       sourceTable,
			MapIndex        index,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapDataTableToDictionary(sourceTable, index, destObjectType, parameters);
		}

		public static IDictionary<CompoundValue,T> DataTableToDictionary<T>(
			DataTable                    sourceTable,
			IDictionary<CompoundValue,T> destDictionary,
			MapIndex                     index,
			params object[]              parameters)
		{
			return _defaultSchema.MapDataTableToDictionary<T>(
				sourceTable, destDictionary, index, parameters);
		}

		public static Dictionary<CompoundValue,T> DataTableToDictionary<T>(
			DataTable       sourceTable,
			MapIndex        index,
			params object[] parameters)
		{
			return _defaultSchema.MapDataTableToDictionary<T>(sourceTable, index, parameters);
		}

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

		public static IList<T> DataReaderToList<T>(IDataReader reader, IList<T> list, params object[] parameters)
		{
			return _defaultSchema.MapDataReaderToList<T>(reader, list, parameters);
		}

		public static List<T> DataReaderToList<T>(IDataReader reader, params object[] parameters)
		{
			return _defaultSchema.MapDataReaderToList<T>(reader, parameters);
		}

		#endregion

		#region DataReaderToDataTable

		public static DataTable DataReaderToDataTable(IDataReader reader, DataTable destTable)
		{
			return _defaultSchema.MapDataReaderToDataTable(reader, destTable);
		}

		public static DataTable DataReaderToDataTable(IDataReader reader)
		{
			return _defaultSchema.MapDataReaderToDataTable(reader);
		}

		#endregion

		#region DataReaderToDictionary

		public static IDictionary DataReaderToDictionary(
			IDataReader          dataReader,
			IDictionary          destDictionary,
			NameOrIndexParameter keyField,
			Type                 destObjectType,
			params object[]      parameters)
		{
			return _defaultSchema.MapDataReaderToDictionary(
				dataReader, destDictionary, keyField, destObjectType, parameters);
		}

		public static Hashtable DataReaderToDictionary(
			IDataReader          dataReader,
			NameOrIndexParameter keyField,
			Type                 destObjectType,
			params object[]      parameters)
		{
			return _defaultSchema.MapDataReaderToDictionary(dataReader, keyField, destObjectType, parameters);
		}

		public static IDictionary<K,T> DataReaderToDictionary<K,T>(
			IDataReader          dataReader,
			IDictionary<K,T>     destDictionary,
			NameOrIndexParameter keyField,
			params object[]      parameters)
		{
			return _defaultSchema.MapDataReaderToDictionary<K,T>(
				dataReader, destDictionary, keyField, parameters);
		}

		public static Dictionary<K,T> DataReaderToDictionary<K,T>(
			IDataReader          dataReader,
			NameOrIndexParameter keyField,
			params object[]      parameters)
		{
			return _defaultSchema.MapDataReaderToDictionary<K,T>(dataReader, keyField, parameters);
		}

		#endregion

		#region DataReaderToDictionary (Index)

		public static IDictionary DataReaderToDictionary(
			IDataReader     dataReader,
			IDictionary     destDictionary,
			MapIndex        index,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapDataReaderToDictionary(
				dataReader, destDictionary, index, destObjectType, parameters);
		}

		public static Hashtable DataReaderToDictionary(
			IDataReader     dataReader,
			MapIndex        index,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapDataReaderToDictionary(dataReader, index, destObjectType, parameters);
		}

		public static IDictionary<CompoundValue,T> DataReaderToDictionary<T>(
			IDataReader                  dataReader,
			IDictionary<CompoundValue,T> destDictionary,
			MapIndex                     index,
			params object[]              parameters)
		{
			return _defaultSchema.MapDataReaderToDictionary<T>(
				dataReader, destDictionary, index, parameters);
		}

		public static Dictionary<CompoundValue,T> DataReaderToDictionary<T>(
			IDataReader     dataReader,
			MapIndex        index,
			params object[] parameters)
		{
			return _defaultSchema.MapDataReaderToDictionary<T>(dataReader, index, parameters);
		}

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

		#endregion

		#region DictionaryToDataTable

		public static DataTable DictionaryToDataTable(IDictionary sourceDictionary, DataTable destTable)
		{
			return _defaultSchema.MapDictionaryToDataTable(sourceDictionary, destTable);
		}

		public static DataTable DictionaryToDataTable(IDictionary sourceDictionary)
		{
			return _defaultSchema.MapDictionaryToDataTable(sourceDictionary);
		}

		#endregion

		#region DictionaryToDictionary

		public static IDictionary DictionaryToDictionary(
			IDictionary          sourceDictionary,
			IDictionary          destDictionary,
			NameOrIndexParameter keyField,
			Type                 destObjectType,
			params object[]      parameters)
		{
			return _defaultSchema.MapDictionaryToDictionary(
				sourceDictionary, destDictionary, keyField, destObjectType, parameters);
		}

		public static Hashtable DictionaryToDictionary(
			IDictionary          sourceDictionary,
			NameOrIndexParameter keyField,
			Type                 destObjectType,
			params object[]      parameters)
		{
			return _defaultSchema.MapDictionaryToDictionary(
				sourceDictionary, keyField, destObjectType, parameters);
		}

		public static IDictionary<K,T> DictionaryToDictionary<K,T>(
			IDictionary          sourceDictionary,
			IDictionary<K,T>     destDictionary,
			NameOrIndexParameter keyField,
			params object[]      parameters)
		{
			return _defaultSchema.MapDictionaryToDictionary<K,T>(
				sourceDictionary, destDictionary, keyField, parameters);
		}

		public static Dictionary<K,T> DictionaryToDictionary<K,T>(
			IDictionary          sourceDictionary,
			NameOrIndexParameter keyField,
			params object[]      parameters)
		{
			return _defaultSchema.MapDictionaryToDictionary<K,T>(sourceDictionary, keyField, parameters);
		}

		#endregion

		#region DictionaryToDictionary (Index)

		public static IDictionary DictionaryToDictionary(
			IDictionary     sourceDictionary,
			IDictionary     destDictionary,
			MapIndex        index,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapDictionaryToDictionary(
				sourceDictionary, destDictionary, index, destObjectType, parameters);
		}

		public static Hashtable DictionaryToDictionary(
			IDictionary     sourceDictionary,
			MapIndex        index,
			Type            destObjectType,
			params object[] parameters)
		{
			return _defaultSchema.MapDictionaryToDictionary(
				sourceDictionary, index, destObjectType, parameters);
		}

		public static IDictionary<CompoundValue,T> DictionaryToDictionary<T>(
			IDictionary                  sourceDictionary,
			IDictionary<CompoundValue,T> destDictionary,
			MapIndex                     index,
			params object[]              parameters)
		{
			return _defaultSchema.MapDictionaryToDictionary<T>(
				sourceDictionary, destDictionary, index, parameters);
		}

		public static Dictionary<CompoundValue,T> DictionaryToDictionary<T>(
			IDictionary     sourceDictionary,
			MapIndex        index,
			params object[] parameters)
		{
			return _defaultSchema.MapDictionaryToDictionary<T>(sourceDictionary, index, parameters);
		}

		#endregion

		#endregion

		#region ToResultSet

		public static void ResultSets(MapResultSet[] resultSets)
		{
			_defaultSchema.MapResultSets(resultSets);//, true);
		}

		//public static void ResultSets(MapResultSet[] resultSets, bool throwException)
		//{
		//    _defaultSchema.MapResultSets(resultSets, throwException);
		//}

		//public static void DataReaderToResultSet(IDataReader reader, MapResultSet[] resultSets)
		//{
		//    _defaultSchema.MapDataReaderToResultSet(reader, resultSets, true);
		//}

		//public static void DataReaderToResultSet(
		//    IDataReader reader, MapResultSet[] resultSets, bool throwException)
		//{
		//    _defaultSchema.MapDataReaderToResultSet(reader, resultSets, throwException);
		//}

		//public static void DataSetToResultSet(DataSet dataSet, MapResultSet[] resultSets)
		//{
		//    _defaultSchema.MapDataSetToResultSet(dataSet, resultSets, true);
		//}

		//public static void DataSetToResultSet(
		//    DataSet dataSet, MapResultSet[] resultSets, bool throwException)
		//{
		//    _defaultSchema.MapDataSetToResultSet(dataSet, resultSets, throwException);
		//}

		//public static MapResultSet[] Clone(MapResultSet[] resultSets)
		//{
		//    return _defaultSchema.Clone(resultSets);
		//}

		//public static MapResultSet[] ConvertToResultSet(Type masterType, params MapNextResult[] nextResults)
		//{
		//    return _defaultSchema.ConvertToResultSet(masterType, nextResults);
		//}

		#endregion

		#region CreateInstance

		public static object CreateInstance(Type type)
		{
			return TypeAccessor.CreateInstanceEx(type);
		}

		public static T CreateInstance<T>()
		{
			return TypeAccessor<T>.CreateInstanceEx();
		}

		#endregion
	}
}
