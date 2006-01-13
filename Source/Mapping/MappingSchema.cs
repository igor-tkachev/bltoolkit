using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
#if FW2
using System.Collections.Generic;
#endif

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class MappingSchema
	{
		#region ObjectMapper Support

		private Hashtable _mappers = new Hashtable();

		public ObjectMapper GetObjectMapper(Type type)
		{
			ObjectMapper om = (ObjectMapper)_mappers[type];

			if (om == null)
			{
				lock (_mappers.SyncRoot)
				{
					om = (ObjectMapper)_mappers[type];

					if (om == null)
					{
						om = CreateObjectMapper(type);

						if (om == null)
						{
							om = Map.DefaultSchema.CreateObjectMapper(type);

							if (om == null)
								throw new MappingException(
									string.Format("Cannot create object mapper for the '{0}' type.", type.FullName));
						}

						SetObjectMapper(type, om);

						om.Init(this, type);
					}
				}
			}

			return om;
		}

		public void SetObjectMapper(Type type, ObjectMapper om)
		{
			if (type == null) throw new ArgumentNullException("type");

			_mappers[type] = om;

			if (type.IsAbstract)
				_mappers[TypeAccessor.GetAccessor(type).Type] = om;
		}

		protected virtual ObjectMapper CreateObjectMapper(Type type)
		{
			Attribute attr = TypeHelper.GetFirstAttribute(type, typeof(ObjectMapperAttribute));
			return attr == null? GetDefaultObjectMapper(type): ((ObjectMapperAttribute)attr).ObjectMapper;
		}

		protected virtual ObjectMapper GetDefaultObjectMapper(Type type)
		{
			return new ObjectMapper();
		}

		#endregion

		#region GetNullValue

		public virtual object GetNullValue(Type type)
		{
			return TypeAccessor.GetNullValue(type);
		}

		public virtual bool IsNull(object value)
		{
			return TypeAccessor.IsNull(value);
		}

		#endregion

		#region GetMapValues

		private Hashtable _mapValues = new Hashtable();

		public virtual MapValue[] GetMapValues(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			MapValue[] mapValues = (MapValue[])_mapValues[type];

			if (mapValues != null || _mapValues.Contains(type))
				return mapValues;

			ArrayList list = null;

			if (type.IsEnum)
				list = GetEnumMapValues(type);

			object[] attrs = TypeHelper.GetAttributes(type, typeof(MapValueAttribute));

			if (attrs != null && attrs.Length != 0)
			{
				if (list == null)
					list = new ArrayList(attrs.Length);

				for (int i = 0; i < attrs.Length; i++)
				{
					MapValueAttribute a = (MapValueAttribute)attrs[i];
					list.Add(new MapValue(a.OrigValue, a.Values));
				}
			}

			if (list != null)
				mapValues = (MapValue[])list.ToArray(typeof(MapValue));

			_mapValues[type] = mapValues;

			return mapValues;
		}

		const FieldAttributes EnumField = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal;

		private static ArrayList GetEnumMapValues(Type type)
		{
			ArrayList mapValues = null;

			FieldInfo[] fields = type.GetFields();

			foreach (FieldInfo fi in fields)
			{
				if ((fi.Attributes & EnumField) == EnumField)
				{
					Attribute[] enumAttributes = Attribute.GetCustomAttributes(fi, typeof(MapValueAttribute));

					foreach (MapValueAttribute attr in enumAttributes)
					{
						if (mapValues == null)
							mapValues = new ArrayList(fields.Length);

						object origValue = Enum.Parse(type, fi.Name);

						mapValues.Add(new MapValue(origValue, attr.Values));
					}
				}
			}

			return mapValues;
		}

		#endregion

		#region GetDefaultValue

		private Hashtable _defaultValues = new Hashtable();

		public virtual object GetDefaultValue(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			object defaultValue = _defaultValues[type];

			if (defaultValue != null || _defaultValues.Contains(type))
				return defaultValue;

			if (type.IsEnum)
				defaultValue = GetEnumDefaultValue(type);

			if (defaultValue == null)
			{
				object[] attrs = TypeHelper.GetAttributes(type, typeof(DefaultValueAttribute));

				if (attrs != null && attrs.Length != 0)
					defaultValue = ((DefaultValueAttribute)attrs[0]).Value;
			}

			_defaultValues[type] = defaultValue;

			return defaultValue;
		}

		private static object GetEnumDefaultValue(Type type)
		{
			FieldInfo[] fields = type.GetFields();

			foreach (FieldInfo fi in fields)
			{
				if ((fi.Attributes & EnumField) == EnumField)
				{
					Attribute[] attrs = Attribute.GetCustomAttributes(fi, typeof(DefaultValueAttribute));

					if (attrs.Length > 0)
						return Enum.Parse(type, fi.Name);
				}
			}

			return null;
		}

		#endregion

		#region GetDataSource, GetDataDestination

		public virtual IMapDataSource GetDataSource(object obj)
		{
			if (obj == null) throw new ArgumentNullException("obj");

			if (obj is IMapDataSource)
				return (IMapDataSource)obj;

			if (obj is IDataReader)
				return new DataReaderMapper((IDataReader)obj);

			if (obj is DataRow)
				return new DataRowMapper((DataRow)obj);

			if (obj is DataRowView)
				return new DataRowMapper((DataRowView)obj);

			if (obj is DataTable)
				return new DataRowMapper(((DataTable)(obj)).Rows[0]);

			if (obj is IDictionary)
				return new DictionaryMapper((IDictionary)obj);

			return GetObjectMapper(obj.GetType());
		}

		public virtual IMapDataDestination GetDataDestination(object obj)
		{
			if (obj == null) throw new ArgumentNullException("obj");

			if (obj is IMapDataDestination)
				return (IMapDataDestination)obj;

			if (obj is DataRow)
				return new DataRowMapper((DataRow)obj);

			if (obj is DataRowView)
				return new DataRowMapper((DataRowView)obj);

			if (obj is DataTable)
			{
				DataTable dt = obj as DataTable;
				DataRow   dr = dt.NewRow();

				dt.Rows.Add(dr);

				return new DataRowMapper(dr);
			}

			if (obj is IDictionary)
				return new DictionaryMapper((IDictionary)obj);

			return GetObjectMapper(obj.GetType());
		}

		#endregion

		#region Base Mapping

		protected static int[] GetIndex(IMapDataSource source, IMapDataDestination dest)
		{
			int   count = source.Count;
			int[] index = new int[count];

			for (int i = 0; i < count; i++)
				index[i] = dest.GetOrdinal(source.GetName(i));

			return index;
		}

		protected static void MapInternal(
			IMapDataSource      source, object sourceObject,
			IMapDataDestination dest,   object destObject,
			int[]               index)
		{
			int count = index.Length;

			for (int i = 0; i < count; i++)
			{
				int n = index[i];

				if (n >= 0)
					dest.SetValue(destObject, n, source.GetValue(sourceObject, i));
			}
		}

		protected virtual void MapInternal(
			InitContext         initContext,
			IMapDataSource      source, object sourceObject, 
			IMapDataDestination dest,   object destObject,
			params object[]     parameters)
		{
			ISupportMapping smSource = sourceObject as ISupportMapping;
			ISupportMapping smDest   = destObject   as ISupportMapping;

			if (smSource != null)
			{
				if (initContext == null)
				{
					initContext = new InitContext();

					initContext.MappingSchema = this;
					initContext.DataSource    = source;
					initContext.SourceObject  = sourceObject;
					initContext.ObjectMapper  = dest as ObjectMapper;
					initContext.Parameters    = parameters;
				}

				initContext.IsSource = true;
				smSource.BeginMapping(initContext);
				initContext.IsSource = false;

				if (initContext.StopMapping)
					return;
			}

			if (smDest != null)
			{
				if (initContext == null)
				{
					initContext = new InitContext();

					initContext.MappingSchema = this;
					initContext.DataSource    = source;
					initContext.SourceObject  = sourceObject;
					initContext.ObjectMapper  = dest as ObjectMapper;
					initContext.Parameters    = parameters;
				}

				smDest.BeginMapping(initContext);

				if (initContext.StopMapping)
					return;

				if (dest != initContext.ObjectMapper && initContext.ObjectMapper != null)
					dest = initContext.ObjectMapper;
			}

			MapInternal(source, sourceObject, dest, destObject, GetIndex(source, dest));

			if (smDest != null)
				smDest.EndMapping(initContext);

			if (smSource != null)
			{
				initContext.IsSource = true;
				smSource.EndMapping(initContext);
				initContext.IsSource = false;
			}
		}

		protected virtual object MapInternal(InitContext initContext)
		{
			object dest = initContext.ObjectMapper.CreateInstance(initContext);

			if (initContext.StopMapping == false)
			{
				MapInternal(initContext,
					initContext.DataSource, initContext.SourceObject, 
					initContext.ObjectMapper, dest,
					initContext.Parameters);
			}

			return dest;
		}

		public void MapSourceToDestination(
			IMapDataSource      source, object sourceObject, 
			IMapDataDestination dest,   object destObject,
			params object[]     parameters)
		{
			MapInternal(null, source, sourceObject, dest, destObject, parameters);
		}

		public void MapSourceToDestination(object sourceObject, object destObject, params object[] parameters)
		{
			IMapDataSource      source = GetDataSource     (sourceObject);
			IMapDataDestination dest   = GetDataDestination(destObject);

			MapInternal(null, source, sourceObject, dest, destObject, parameters);
		}

		private static ObjectMapper _nullMapper = new ObjectMapper();

		public virtual void MapSourceListToDestinationList(
			IMapDataSourceList      dataSourceList,
			IMapDataDestinationList dataDestinationList,
			params object[]         parameters)
		{
			if (dataSourceList      == null) throw new ArgumentNullException("dataSourceList");
			if (dataDestinationList == null) throw new ArgumentNullException("dataDestinationList");

			InitContext ctx = new InitContext();

			ctx.MappingSchema = this;
			ctx.Parameters    = parameters;

			dataSourceList.     InitMapping(ctx); if (ctx.StopMapping) return;
			dataDestinationList.InitMapping(ctx); if (ctx.StopMapping) return;

			int[]               index   = null;
			ObjectMapper        current = _nullMapper;
			IMapDataDestination dest    = dataDestinationList.GetDataDestination(ctx);
			ObjectMapper        om      = dest as ObjectMapper;

			while (dataSourceList.SetNextDataSource(ctx))
			{
				ctx.ObjectMapper = om;

				object destObject = dataDestinationList.GetNextObject(ctx);

				if (ctx.StopMapping) continue;

				ISupportMapping smSource = ctx.SourceObject as ISupportMapping;
				ISupportMapping smDest   = destObject       as ISupportMapping;

				if (smSource != null)
				{
					ctx.IsSource = true;
					smSource.BeginMapping(ctx);
					ctx.IsSource = false;

					if (ctx.StopMapping)
						continue;
				}

				if (smDest != null)
				{
					smDest.BeginMapping(ctx);

					if (ctx.StopMapping)
						continue;
				}

				if (current != ctx.ObjectMapper)
				{
					current = ctx.ObjectMapper;
					index   = GetIndex(ctx.DataSource, current != null? current: dest);
				}

				MapInternal(ctx.DataSource, ctx.SourceObject, dest, destObject, index);

				if (smDest != null)
					smDest.EndMapping(ctx);

				if (smSource != null)
				{
					ctx.IsSource = true;
					smSource.EndMapping(ctx);
					ctx.IsSource = false;
				}
			}

			dataDestinationList.EndMapping(ctx);
			dataSourceList.     EndMapping(ctx);
		}

		#endregion

		#region ValueToEnum, EnumToValue

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public object MapValueToEnum(object value, Type type)
		{
			if (value == null)
				return GetNullValue(type);

			MapValue[] mapValues = GetMapValues(type);

			if (mapValues != null)
			{
				IComparable comp = (IComparable)value;

				foreach (MapValue mv in mapValues)
				foreach (object mapValue in mv.MapValues)
				{
					try
					{
						if (comp.CompareTo(mapValue) == 0)
							return mv.OrigValue;
					}
					catch
					{
					}
				}

				// Default value.
				//
				object defaultValue = GetDefaultValue(type);

				if (defaultValue != null)
					return defaultValue;
			}

			return value;
		}

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public object MapEnumToValue(object value)
		{
			if (value == null)
				return null;

			Type type = value.GetType();

			object nullValue = GetNullValue(type);

			if (nullValue != null)
			{
				IComparable comp = (IComparable)value;

				try
				{
					if (comp.CompareTo(nullValue) == 0)
						return null;
				}
				catch
				{
				}
			}

			MapValue[] mapValues = GetMapValues(type);

			if (mapValues != null)
			{
				IComparable comp = (IComparable)value;

				foreach (MapValue mv in mapValues)
				{
					try
					{
						if (comp.CompareTo(mv.OrigValue) == 0)
							return mv.MapValues[0];
					}
					catch
					{
					}
				}
			}

			return value;
		}

#if FW2
		public T MapValueToEnum<T>(object value)
		{
			return (T)MapValueToEnum(value, typeof(T));
		}
#endif

		#endregion

		#region Object

		#region MapObjectToObject

		public object MapObjectToObject(object sourceObject, object destObject, params object[] parameters)
		{
			if (sourceObject == null) throw new ArgumentNullException("sourceObject");
			if (destObject   == null) throw new ArgumentNullException("destObject");

			MapInternal(
				null,
				GetObjectMapper(sourceObject.GetType()), sourceObject,
				GetObjectMapper(destObject.  GetType()), destObject,
				parameters);

			return destObject;
		}

		public object MapObjectToObject(object sourceObject, Type destObjectType, params object[] parameters)
		{
			if (sourceObject == null) throw new ArgumentNullException("sourceObject");

			InitContext ctx = new InitContext();

			ctx.MappingSchema = this;
			ctx.DataSource    = GetObjectMapper(sourceObject.GetType());
			ctx.SourceObject  = sourceObject;
			ctx.ObjectMapper  = GetObjectMapper(destObjectType);
			ctx.Parameters    = parameters;

			return MapInternal(ctx);
		}

#if FW2
		public T MapObjectToObject<T>(object sourceObject, params object[] parameters)
		{
			return (T)MapObjectToObject(sourceObject, typeof(T), parameters);
		}
#endif

		#endregion

		#region MapObjectToDataRow

		public DataRow MapObjectToDataRow(object sourceObject, DataRow destRow)
		{
			if (sourceObject == null) throw new ArgumentNullException("sourceObject");

			MapInternal(
				null,
				GetObjectMapper(sourceObject.GetType()), sourceObject,
				new DataRowMapper(destRow),              destRow,
				null);

			return destRow;
		}

		public DataRow MapObjectToDataRow(object sourceObject, DataTable destTable)
		{
			if (destTable    == null) throw new ArgumentNullException("destTable");
			if (sourceObject == null) throw new ArgumentNullException("sourceObject");

			DataRow destRow = destTable.NewRow();

			destTable.Rows.Add(destRow);

			MapInternal(
				null,
				GetObjectMapper(sourceObject.GetType()), sourceObject,
				new DataRowMapper(destRow),              destRow,
				null);

			return destRow;
		}

		#endregion

		#region MapObjectToDictionary

		public IDictionary MapObjectToDictionary(object sourceObject, IDictionary destDictionary)
		{
			if (sourceObject == null) throw new ArgumentNullException("sourceObject");

			MapInternal(
				null,
				GetObjectMapper(sourceObject.GetType()), sourceObject,
				new DictionaryMapper(destDictionary),    destDictionary,
				null);

			return destDictionary;
		}

		public Hashtable MapObjectToDictionary(object sourceObject)
		{
			if (sourceObject == null) throw new ArgumentNullException("sourceObject");

			ObjectMapper om = GetObjectMapper(sourceObject.GetType());

			Hashtable destDictionary = new Hashtable(om.Count);

			MapInternal(
				null,
				om, sourceObject,
				new DictionaryMapper(destDictionary), destDictionary,
				null);

			return destDictionary;
		}

		#endregion

		#endregion

		#region DataRow

		#region MapDataRowToObject

		public object MapDataRowToObject(DataRow dataRow, object destObject, params object[] parameters)
		{
			if (destObject == null) throw new ArgumentNullException("destObject");

			MapInternal(
				null,
				new DataRowMapper(dataRow), dataRow,
				GetObjectMapper(destObject.  GetType()), destObject,
				parameters);

			return destObject;
		}

		public object MapDataRowToObject(
			DataRow dataRow, DataRowVersion version, object destObject, params object[] parameters)
		{
			if (destObject == null) throw new ArgumentNullException("destObject");

			MapInternal(
				null,
				new DataRowMapper(dataRow, version), dataRow,
				GetObjectMapper(destObject.  GetType()), destObject,
				parameters);

			return destObject;
		}

		public object MapDataRowToObject(DataRow dataRow, Type destObjectType, params object[] parameters)
		{
			InitContext ctx = new InitContext();

			ctx.MappingSchema = this;
			ctx.DataSource    = new DataRowMapper(dataRow);
			ctx.SourceObject  = dataRow;
			ctx.ObjectMapper  = GetObjectMapper(destObjectType);
			ctx.Parameters    = parameters;

			return MapInternal(ctx);
		}

		public object MapDataRowToObject(
			DataRow dataRow, DataRowVersion version, Type destObjectType, params object[] parameters)
		{
			InitContext ctx = new InitContext();

			ctx.MappingSchema = this;
			ctx.DataSource    = new DataRowMapper(dataRow, version);
			ctx.SourceObject  = dataRow;
			ctx.ObjectMapper  = GetObjectMapper(destObjectType);
			ctx.Parameters    = parameters;

			return MapInternal(ctx);
		}

#if FW2
		public T MapDataRowToObject<T>(DataRow dataRow, params object[] parameters)
		{
			return (T)MapDataRowToObject(dataRow, typeof(T), parameters);
		}

		public T MapDataRowToObject<T>(DataRow dataRow, DataRowVersion version, params object[] parameters)
		{
			return (T)MapDataRowToObject(dataRow, version, typeof(T), parameters);
		}
#endif

		#endregion

		#region MapDataRowToDataRow

		public DataRow MapDataRowToDataRow(DataRow sourceRow, DataRow destRow)
		{
			MapInternal(
				null,
				new DataRowMapper(sourceRow), sourceRow,
				new DataRowMapper(destRow),   destRow,
				null);

			return destRow;
		}

		public DataRow MapDataRowToDataRow(DataRow sourceRow, DataRowVersion version, DataRow destRow)
		{
			MapInternal(
				null,
				new DataRowMapper(sourceRow, version), sourceRow,
				new DataRowMapper(destRow), destRow,
				null);

			return destRow;
		}

		public DataRow MapDataRowToDataRow(DataRow sourceRow, DataTable destTable)
		{
			if (destTable == null) throw new ArgumentNullException("destTable");

			DataRow destRow = destTable.NewRow();

			destTable.Rows.Add(destRow);

			MapInternal(
				null,
				new DataRowMapper(sourceRow), sourceRow,
				new DataRowMapper(destRow),   destRow,
				null);

			return destRow;
		}

		public DataRow MapDataRowToDataRow(DataRow sourceRow, DataRowVersion version, DataTable destTable)
		{
			if (destTable == null) throw new ArgumentNullException("destTable");

			DataRow destRow = destTable.NewRow();

			destTable.Rows.Add(destRow);

			MapInternal(
				null,
				new DataRowMapper(sourceRow, version), sourceRow,
				new DataRowMapper(destRow), destRow,
				null);

			return destRow;
		}

		#endregion

		#region MapDataRowToDictionary

		public IDictionary MapDataRowToDictionary(DataRow sourceRow, IDictionary destDictionary)
		{
			MapInternal(
				null,
				new DataRowMapper   (sourceRow),      sourceRow,
				new DictionaryMapper(destDictionary), destDictionary,
				null);

			return destDictionary;
		}

		public Hashtable MapDataRowToDictionary(DataRow sourceRow)
		{
			if (sourceRow == null) throw new ArgumentNullException("sourceRow");

			Hashtable destDictionary = new Hashtable(sourceRow.Table.Columns.Count);

			MapInternal(
				null,
				new DataRowMapper   (sourceRow),      sourceRow,
				new DictionaryMapper(destDictionary), destDictionary,
				null);

			return destDictionary;
		}

		public IDictionary MapDataRowToDictionary(DataRow sourceRow, DataRowVersion version, IDictionary destDictionary)
		{
			MapInternal(
				null,
				new DataRowMapper   (sourceRow, version), sourceRow,
				new DictionaryMapper(destDictionary),     destDictionary,
				null);

			return destDictionary;
		}

		public Hashtable MapDataRowToDictionary(DataRow sourceRow, DataRowVersion version)
		{
			if (sourceRow == null) throw new ArgumentNullException("sourceRow");

			Hashtable destDictionary = new Hashtable(sourceRow.Table.Columns.Count);

			MapInternal(
				null,
				new DataRowMapper   (sourceRow, version), sourceRow,
				new DictionaryMapper(destDictionary),     destDictionary,
				null);

			return destDictionary;
		}

		#endregion

		#endregion

		#region DataReader

		#region MapDataReaderToObject

		public object MapDataReaderToObject(IDataReader dataReader, object destObject, params object[] parameters)
		{
			if (destObject == null) throw new ArgumentNullException("destObject");

			MapInternal(
				null,
				new DataReaderMapper(dataReader), dataReader,
				GetObjectMapper(destObject.  GetType()), destObject,
				parameters);

			return destObject;
		}

		public object MapDataReaderToObject(IDataReader dataReader, Type destObjectType, params object[] parameters)
		{
			InitContext ctx = new InitContext();

			ctx.MappingSchema = this;
			ctx.DataSource    = new DataReaderMapper(dataReader);
			ctx.SourceObject  = dataReader;
			ctx.ObjectMapper  = GetObjectMapper(destObjectType);
			ctx.Parameters    = parameters;

			return MapInternal(ctx);
		}

#if FW2
		public T MapDataReaderToObject<T>(IDataReader dataReader, params object[] parameters)
		{
			return (T)MapDataReaderToObject(dataReader, typeof(T), parameters);
		}
#endif

		#endregion

		#region MapDataReaderToDataRow

		public DataRow MapDataReaderToDataRow(IDataReader dataReader, DataRow destRow)
		{
			MapInternal(
				null,
				new DataReaderMapper(dataReader), dataReader,
				new DataRowMapper(destRow), destRow,
				null);

			return destRow;
		}

		public DataRow MapDataReaderToDataRow(IDataReader dataReader, DataTable destTable)
		{
			if (destTable == null) throw new ArgumentNullException("destTable");

			DataRow destRow = destTable.NewRow();

			destTable.Rows.Add(destRow);

			MapInternal(
				null,
				new DataReaderMapper(dataReader), dataReader,
				new DataRowMapper(destRow),       destRow,
				null);

			return destRow;
		}

		#endregion

		#region MapDataReaderToDictionary

		public IDictionary MapDataReaderToDictionary(IDataReader dataReader, IDictionary destDictionary)
		{
			MapInternal(
				null,
				new DataReaderMapper(dataReader),     dataReader,
				new DictionaryMapper(destDictionary), destDictionary,
				null);

			return destDictionary;
		}

		public Hashtable MapDataReaderToDictionary(IDataReader dataReader)
		{
			if (dataReader == null) throw new ArgumentNullException("dataReader");

			Hashtable destDictionary = new Hashtable(dataReader.FieldCount);

			MapInternal(
				null,
				new DataReaderMapper(dataReader),     dataReader,
				new DictionaryMapper(destDictionary), destDictionary,
				null);

			return destDictionary;
		}

		#endregion

		#endregion

		#region Dictionary

		#region MapDictionaryToObject

		public object MapDictionaryToObject(
			IDictionary sourceDictionary, object destObject, params object[] parameters)
		{
			if (destObject == null) throw new ArgumentNullException("destObject");

			MapInternal(
				null,
				new DictionaryMapper(sourceDictionary), sourceDictionary,
				GetObjectMapper(destObject.  GetType()), destObject,
				parameters);

			return destObject;
		}

		public object MapDictionaryToObject(
			IDictionary sourceDictionary, Type destObjectType, params object[] parameters)
		{
			InitContext ctx = new InitContext();

			ctx.MappingSchema = this;
			ctx.DataSource    = new DictionaryMapper(sourceDictionary);
			ctx.SourceObject  = sourceDictionary;
			ctx.ObjectMapper  = GetObjectMapper(destObjectType);
			ctx.Parameters    = parameters;

			return MapInternal(ctx);
		}

#if FW2
		public T MapDictionaryToObject<T>(IDictionary sourceDictionary, params object[] parameters)
		{
			return (T)MapDictionaryToObject(sourceDictionary, typeof(T), parameters);
		}
#endif

		#endregion

		#region MapDictionaryToDataRow

		public DataRow MapDictionaryToDataRow(IDictionary sourceDictionary, DataRow destRow)
		{
			MapInternal(
				null,
				new DictionaryMapper(sourceDictionary), sourceDictionary,
				new DataRowMapper   (destRow),          destRow,
				null);

			return destRow;
		}

		public DataRow MapDictionaryToDataRow(IDictionary sourceDictionary, DataTable destTable)
		{
			if (destTable == null) throw new ArgumentNullException("destTable");

			DataRow destRow = destTable.NewRow();

			destTable.Rows.Add(destRow);

			MapInternal(
				null,
				new DictionaryMapper(sourceDictionary), sourceDictionary,
				new DataRowMapper   (destRow),          destRow,
				null);

			return destRow;
		}

		#endregion

		#endregion

		#region List

		#region MapListToList

		public IList MapListToList(
			ICollection     sourceList,
			IList           destList,
			Type            destObjectType,
			params object[] parameters)
		{
			if (sourceList == null) throw new ArgumentNullException("sourceList");

			MapSourceListToDestinationList(
				new EnumeratorMapper(sourceList.GetEnumerator()),
				new ObjectListMapper(destList, GetObjectMapper(destObjectType)),
				parameters);

			return destList;
		}

		public ArrayList MapListToList(ICollection sourceList, Type destObjectType, params object[] parameters)
		{
			if (sourceList == null) throw new ArgumentNullException("sourceList");

			ArrayList destList = new ArrayList();

			MapSourceListToDestinationList(
				new EnumeratorMapper(sourceList.GetEnumerator()),
				new ObjectListMapper(destList, GetObjectMapper(destObjectType)),
				parameters);

			return destList;
		}

#if FW2
		public List<T> MapListToList<T>(ICollection sourceList, List<T> destList, params object[] parameters)
		{
			MapSourceListToDestinationList(
				new EnumeratorMapper(sourceList.GetEnumerator()),
				new ObjectListMapper(destList, GetObjectMapper(typeof(T))),
				parameters);

			return destList;
		}

		public List<T> MapListToList<T>(ICollection sourceList, params object[] parameters)
		{
			List<T> destList = new List<T>();

			MapSourceListToDestinationList(
				new EnumeratorMapper(sourceList.GetEnumerator()),
				new ObjectListMapper(destList, GetObjectMapper(typeof(T))),
				parameters);

			return destList;
		}
#endif

		#endregion

		#region MapListToDataTable

		public DataTable MapListToDataTable(ICollection sourceList, DataTable destTable)
		{
			if (sourceList == null) throw new ArgumentNullException("sourceList");

			MapSourceListToDestinationList(
				new EnumeratorMapper(sourceList.GetEnumerator()),
				new DataTableMapper(destTable),
				null);

			return destTable;
		}

		[SuppressMessage("Microsoft.Globalization", "CA1306:SetLocaleForDataTypes")]
		public DataTable MapListToDataTable(ICollection sourceList)
		{
			if (sourceList == null) throw new ArgumentNullException("sourceList");

			DataTable destTable = new DataTable();

			MapSourceListToDestinationList(
				new EnumeratorMapper(sourceList.GetEnumerator()),
				new DataTableMapper(destTable),
				null);

			return destTable;
		}

		#endregion

		#region MapListToDictionary

		public IDictionary MapListToDictionary(
			ICollection     sourceList,
			IDictionary     destDictionary,
			string          keyFieldName,
			Type            destObjectType,
			params object[] parameters)
		{
			if (sourceList == null) throw new ArgumentNullException("sourceList");

			MapSourceListToDestinationList(
				new EnumeratorMapper    (sourceList.GetEnumerator()),
				new DictionaryListMapper(destDictionary, keyFieldName, GetObjectMapper(destObjectType)),
				parameters);

			return destDictionary;
		}

		public Hashtable MapListToDictionary(
			ICollection     sourceList,
			string          keyFieldName,
			Type            destObjectType,
			params object[] parameters)
		{
			if (sourceList == null) throw new ArgumentNullException("sourceList");

			Hashtable dest = new Hashtable();

			MapSourceListToDestinationList(
				new EnumeratorMapper    (sourceList.GetEnumerator()),
				new DictionaryListMapper(dest, keyFieldName, GetObjectMapper(destObjectType)),
				parameters);

			return dest;
		}

#if FW2
		public Dictionary<K,T> MapListToDictionary<K,T>(
			ICollection     sourceList,
			Dictionary<K,T> destDictionary,
			string          keyFieldName,
			params object[] parameters)
		{
			MapSourceListToDestinationList(
				new EnumeratorMapper    (sourceList.GetEnumerator()),
				new DictionaryListMapper(destDictionary, keyFieldName, GetObjectMapper(typeof(T))),
				parameters);

			return destDictionary;
		}

		public Dictionary<K,T> MapListToDictionary<K,T>(
			ICollection     sourceList,
			string          keyFieldName,
			params object[] parameters)
		{
			Dictionary<K, T> dest = new Dictionary<K,T>();

			MapSourceListToDestinationList(
				new EnumeratorMapper    (sourceList.GetEnumerator()),
				new DictionaryListMapper(dest, keyFieldName, GetObjectMapper(typeof(T))),
				parameters);

			return dest;
		}
#endif

		#endregion

		#endregion

		#region Table

		#region MapDataTableToDataTable

		public DataTable MapDataTableToDataTable(DataTable sourceTable, DataTable destTable)
		{
			MapSourceListToDestinationList(
				new DataTableMapper(sourceTable),
				new DataTableMapper(destTable),
				null);

			return destTable;
		}

		public DataTable MapDataTableToDataTable(DataTable sourceTable, DataRowVersion version, DataTable destTable)
		{
			MapSourceListToDestinationList(
				new DataTableMapper(sourceTable, version),
				new DataTableMapper(destTable),
				null);

			return destTable;
		}

		public DataTable MapDataTableToDataTable(DataTable sourceTable)
		{
			if (sourceTable == null) throw new ArgumentNullException("sourceTable");

			DataTable destTable = sourceTable.Clone();

			MapSourceListToDestinationList(
				new DataTableMapper(sourceTable),
				new DataTableMapper(destTable),
				null);

			return destTable;
		}

		public DataTable MapDataTableToDataTable(DataTable sourceTable, DataRowVersion version)
		{
			if (sourceTable == null) throw new ArgumentNullException("sourceTable");

			DataTable destTable = sourceTable.Clone();

			MapSourceListToDestinationList(
				new DataTableMapper(sourceTable, version),
				new DataTableMapper(destTable),
				null);

			return destTable;
		}

		#endregion

		#region MapDataTableToList

		public IList MapDataTableToList(
			DataTable sourceTable, IList list, Type destObjectType, params object[] parameters)
		{
			MapSourceListToDestinationList(
				new DataTableMapper (sourceTable),
				new ObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

		public IList MapDataTableToList(
			DataTable       sourceTable,
			DataRowVersion  version,
			IList           list,
			Type            destObjectType,
			params object[] parameters)
		{
			MapSourceListToDestinationList(
				new DataTableMapper (sourceTable, version),
				new ObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

		public ArrayList MapDataTableToList(DataTable sourceTable, Type destObjectType, params object[] parameters)
		{
			ArrayList list = new ArrayList();

			MapSourceListToDestinationList(
				new DataTableMapper (sourceTable),
				new ObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

		public ArrayList MapDataTableToList(
			DataTable sourceTable, DataRowVersion version, Type destObjectType, params object[] parameters)
		{
			ArrayList list = new ArrayList();

			MapSourceListToDestinationList(
				new DataTableMapper (sourceTable, version),
				new ObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

#if FW2
		public List<T> MapDataTableToList<T>(DataTable sourceTable, List<T> list, params object[] parameters)
		{
			MapSourceListToDestinationList(
				new DataTableMapper (sourceTable),
				new ObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}

		public List<T> MapDataTableToList<T>(
			DataTable       sourceTable,
			DataRowVersion  version,
			List<T>         list,
			params object[] parameters)
		{
			MapSourceListToDestinationList(
				new DataTableMapper (sourceTable, version),
				new ObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}

		public List<T> MapDataTableToList<T>(DataTable sourceTable, params object[] parameters)
		{
			List<T> list = new List<T>();

			MapSourceListToDestinationList(
				new DataTableMapper (sourceTable),
				new ObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}

		public List<T> MapDataTableToList<T>(DataTable sourceTable, DataRowVersion version, params object[] parameters)
		{
			List<T> list = new List<T>();

			MapSourceListToDestinationList(
				new DataTableMapper (sourceTable, version),
				new ObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}
#endif

		#endregion

		#region MapDataTableToDictionary

		public IDictionary MapDataTableToDictionary(
			DataTable       sourceTable,
			IDictionary     destDictionary,
			string          keyFieldName,
			Type            destObjectType,
			params object[] parameters)
		{
			MapSourceListToDestinationList(
				new DataTableMapper     (sourceTable),
				new DictionaryListMapper(destDictionary, keyFieldName, GetObjectMapper(destObjectType)),
				parameters);

			return destDictionary;
		}

		public Hashtable MapDataTableToDictionary(
			DataTable       sourceTable,
			string          keyFieldName,
			Type            destObjectType,
			params object[] parameters)
		{
			Hashtable dest = new Hashtable();

			MapSourceListToDestinationList(
				new DataTableMapper     (sourceTable),
				new DictionaryListMapper(dest, keyFieldName, GetObjectMapper(destObjectType)),
				parameters);

			return dest;
		}

#if FW2
		public Dictionary<K,T> MapDataTableToDictionary<K,T>(
			DataTable       sourceTable,
			Dictionary<K,T> destDictionary,
			string          keyFieldName,
			params object[] parameters)
		{
			MapSourceListToDestinationList(
				new DataTableMapper     (sourceTable),
				new DictionaryListMapper(destDictionary, keyFieldName, GetObjectMapper(typeof(T))),
				parameters);

			return destDictionary;
		}

		public Dictionary<K,T> MapDataTableToDictionary<K,T>(
			DataTable       sourceTable,
			string          keyFieldName,
			params object[] parameters)
		{
			Dictionary<K, T> dest = new Dictionary<K,T>();

			MapSourceListToDestinationList(
				new DataTableMapper     (sourceTable),
				new DictionaryListMapper(dest, keyFieldName, GetObjectMapper(typeof(T))),
				parameters);

			return dest;
		}
#endif

		#endregion

		#endregion

		#region DataReader

		#region MapDataReaderToList

		public IList MapDataReaderToList(
			IDataReader     reader,
			IList           list,
			Type            destObjectType,
			params object[] parameters)
		{
			MapSourceListToDestinationList(
				new DataReaderListMapper(reader),
				new ObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

		public ArrayList MapDataReaderToList(IDataReader reader, Type destObjectType, params object[] parameters)
		{
			ArrayList list = new ArrayList();

			MapSourceListToDestinationList(
				new DataReaderListMapper(reader),
				new ObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

#if FW2
		public List<T> MapDataReaderToList<T>(IDataReader reader, List<T> list, params object[] parameters)
		{
			MapSourceListToDestinationList(
				new DataReaderListMapper(reader),
				new ObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}

		public List<T> MapDataReaderToList<T>(IDataReader reader, params object[] parameters)
		{
			List<T> list = new List<T>();

			MapSourceListToDestinationList(
				new DataReaderListMapper(reader),
				new ObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}
#endif

		#endregion

		#region MapDataReaderToDataTable

		public DataTable MapDataReaderToDataTable(IDataReader reader, DataTable destTable)
		{
			MapSourceListToDestinationList(
				new DataReaderListMapper(reader),
				new DataTableMapper(destTable),
				null);

			return destTable;
		}

		[SuppressMessage("Microsoft.Globalization", "CA1306:SetLocaleForDataTypes")]
		public DataTable MapDataReaderToDataTable(IDataReader reader)
		{
			DataTable destTable = new DataTable();

			MapSourceListToDestinationList(
				new DataReaderListMapper(reader),
				new DataTableMapper(destTable),
				null);

			return destTable;
		}

		#endregion

		#region MapDataReaderToDictionary

		public IDictionary MapDataReaderToDictionary(
			IDataReader     dataReader,
			IDictionary     destDictionary,
			string          keyFieldName,
			Type            destObjectType,
			params object[] parameters)
		{
			MapSourceListToDestinationList(
				new DataReaderListMapper(dataReader),
				new DictionaryListMapper(destDictionary, keyFieldName, GetObjectMapper(destObjectType)),
				parameters);

			return destDictionary;
		}

		public Hashtable MapDataReaderToDictionary(
			IDataReader     dataReader,
			string          keyFieldName,
			Type            destObjectType,
			params object[] parameters)
		{
			Hashtable dest = new Hashtable();

			MapSourceListToDestinationList(
				new DataReaderListMapper(dataReader),
				new DictionaryListMapper(dest, keyFieldName, GetObjectMapper(destObjectType)),
				parameters);

			return dest;
		}

#if FW2
		public Dictionary<K,T> MapDataReaderToDictionary<K,T>(
			IDataReader     dataReader,
			Dictionary<K,T> destDictionary,
			string          keyFieldName,
			params object[] parameters)
		{
			MapSourceListToDestinationList(
				new DataReaderListMapper(dataReader),
				new DictionaryListMapper(destDictionary, keyFieldName, GetObjectMapper(typeof(T))),
				parameters);

			return destDictionary;
		}

		public Dictionary<K,T> MapDataReaderToDictionary<K,T>(
			IDataReader     dataReader,
			string          keyFieldName,
			params object[] parameters)
		{
			Dictionary<K, T> dest = new Dictionary<K,T>();

			MapSourceListToDestinationList(
				new DataReaderListMapper(dataReader),
				new DictionaryListMapper(dest, keyFieldName, GetObjectMapper(typeof(T))),
				parameters);

			return dest;
		}
#endif

		#endregion

		#endregion

		#region Dictionary

		#region MapDictionaryToList

		public IList MapDictionaryToList(
			IDictionary     sourceDictionary,
			IList           destList,
			Type            destObjectType,
			params object[] parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			MapSourceListToDestinationList(
				new EnumeratorMapper(sourceDictionary.Values.GetEnumerator()),
				new ObjectListMapper(destList, GetObjectMapper(destObjectType)),
				parameters);

			return destList;
		}

		public ArrayList MapDictionaryToList(
			IDictionary     sourceDictionary,
			Type            destObjectType,
			params object[] parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			ArrayList destList = new ArrayList();

			MapSourceListToDestinationList(
				new EnumeratorMapper(sourceDictionary.Values.GetEnumerator()),
				new ObjectListMapper(destList, GetObjectMapper(destObjectType)),
				parameters);

			return destList;
		}

#if FW2
		public List<T> MapDictionaryToList<T>(
			IDictionary     sourceDictionary,
			List<T>         destList,
			params object[] parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			MapSourceListToDestinationList(
				new EnumeratorMapper(sourceDictionary.Values.GetEnumerator()),
				new ObjectListMapper(destList, GetObjectMapper(typeof(T))),
				parameters);

			return destList;
		}

		public List<T> MapDictionaryToList<T>(
			IDictionary     sourceDictionary,
			params object[] parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			List<T> destList = new List<T>();

			MapSourceListToDestinationList(
				new EnumeratorMapper(sourceDictionary.Values.GetEnumerator()),
				new ObjectListMapper(destList, GetObjectMapper(typeof(T))),
				parameters);

			return destList;
		}
#endif

		#endregion

		#region MapDictionaryToDataTable

		public DataTable MapDictionaryToDataTable(IDictionary sourceDictionary, DataTable destTable)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			MapSourceListToDestinationList(
				new EnumeratorMapper(sourceDictionary.Values.GetEnumerator()),
				new DataTableMapper (destTable),
				null);

			return destTable;
		}

		[SuppressMessage("Microsoft.Globalization", "CA1306:SetLocaleForDataTypes")]
		public DataTable MapDictionaryToDataTable(IDictionary sourceDictionary)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			DataTable destTable = new DataTable();

			MapSourceListToDestinationList(
				new EnumeratorMapper(sourceDictionary.Values.GetEnumerator()),
				new DataTableMapper (destTable),
				null);

			return destTable;
		}

		#endregion

		#region MapDictionaryToDictionary

		public IDictionary MapDictionaryToDictionary(
			IDictionary     sourceDictionary,
			IDictionary     destDictionary,
			string          keyFieldName,
			Type            destObjectType,
			params object[] parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			MapSourceListToDestinationList(
				new EnumeratorMapper    (sourceDictionary.Values.GetEnumerator()),
				new DictionaryListMapper(destDictionary, keyFieldName, GetObjectMapper(destObjectType)),
				parameters);

			return destDictionary;
		}

		public Hashtable MapDictionaryToDictionary(
			IDictionary     sourceDictionary,
			string          keyFieldName,
			Type            destObjectType,
			params object[] parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			Hashtable dest = new Hashtable();

			MapSourceListToDestinationList(
				new EnumeratorMapper    (sourceDictionary.Values.GetEnumerator()),
				new DictionaryListMapper(dest, keyFieldName, GetObjectMapper(destObjectType)),
				parameters);

			return dest;
		}

#if FW2
		public Dictionary<K,T> MapDictionaryToDictionary<K,T>(
			IDictionary     sourceDictionary,
			Dictionary<K,T> destDictionary,
			string          keyFieldName,
			params object[] parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			MapSourceListToDestinationList(
				new EnumeratorMapper    (sourceDictionary.Values.GetEnumerator()),
				new DictionaryListMapper(destDictionary, keyFieldName, GetObjectMapper(typeof(T))),
				parameters);

			return destDictionary;
		}

		public Dictionary<K,T> MapDictionaryToDictionary<K,T>(
			IDictionary     sourceDictionary,
			string          keyFieldName,
			params object[] parameters)
		{
			if (sourceDictionary == null) throw new ArgumentNullException("sourceDictionary");

			Dictionary<K, T> dest = new Dictionary<K,T>();

			MapSourceListToDestinationList(
				new EnumeratorMapper    (sourceDictionary.Values.GetEnumerator()),
				new DictionaryListMapper(dest, keyFieldName, GetObjectMapper(typeof(T))),
				parameters);

			return dest;
		}
#endif

		#endregion

		#endregion

		#region MapToResultSet

		public void MapResultSets(MapResultSet[] resultSets)
		{
			Hashtable   initTable     = null;
			object      lastContainer = null;
			InitContext context       = new InitContext();

			context.MappingSchema = this;

			try
			{
				// Map relations.
				//
				foreach (MapResultSet rs in resultSets)
				{
					if (rs.Relations == null)
						continue;

					ObjectMapper masterMapper = rs.ObjectMapper;

					foreach (MapRelation r in rs.Relations)
					{
						// Create hash.
						//
						if (rs.IndexID != r.MasterIndex.ID)
						{
							rs.Hashtable = new Hashtable();
							rs.IndexID   = r.MasterIndex.ID;

							foreach (object o in rs.List)
							{
								object key = r.MasterIndex.GetKey(masterMapper, o);
								rs.Hashtable[key] = o;
							}
						}

						// Map.
						//
						MapResultSet slave = r.SlaveResultSet;

						foreach (object o in slave.List)
						{
							object key    = r.SlaveIndex.GetKey(slave.ObjectMapper, o);
							object master = rs.Hashtable[key];

							MemberAccessor ma = masterMapper.TypeAccessor[r.ContainerName];

							if (ma == null)
								throw new MappingException(string.Format("Type '{0}' does not contain field '{1}'.",
									masterMapper.TypeAccessor.OriginalType.Name, r.ContainerName));

							object container = ma.GetValue(master);

							if (container is IList)
							{
								if (lastContainer != container)
								{
									lastContainer = container;

									ISupportMapping sm = container as ISupportMapping;

									if (sm != null)
									{
										if (initTable == null)
											initTable = new Hashtable();

										if (initTable.ContainsKey(container) == false)
										{
											sm.BeginMapping(context);
											initTable[container] = sm;
										}
									}
								}

								((IList)container).Add(o);
							}
							else
							{
								masterMapper.TypeAccessor[r.ContainerName].SetValue(o, master);
							}
						}
					}
				}
			}
			finally
			{
				if (initTable != null)
					foreach (ISupportMapping si in initTable.Values)
						si.EndMapping(context);
			}
		}

		public void MapDataReaderToResultSet(IDataReader reader, MapResultSet[] resultSets)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			foreach (MapResultSet rs in resultSets)
			{
				MapDataReaderToList(reader, rs.List, rs.ObjectType, rs.Parameters);

				if (reader.NextResult() == false)
					break;
			}

			MapResultSets(resultSets);
		}

		public void MapDataSetToResultSet(DataSet dataSet, MapResultSet[] resultSets)
		{
			for (int i = 0; i < resultSets.Length && i < dataSet.Tables.Count; i++)
			{
				MapResultSet rs = resultSets[i];

				MapDataTableToList(dataSet.Tables[i], rs.List, rs.ObjectType, rs.Parameters);
			}

			MapResultSets(resultSets);
		}

		public MapResultSet[] Clone(MapResultSet[] resultSets)
		{
			MapResultSet[] output = new MapResultSet[resultSets.Length];

			for (int i = 0; i < resultSets.Length; i++)
				output[i] = new MapResultSet(resultSets[i]);

			return output;
		}

		private int GetResultCount(MapNextResult[] nextResults)
		{
			int n = nextResults.Length;

			foreach (MapNextResult nr in nextResults)
				n += GetResultCount(nr.NextResults);

			return n;
		}

		private int GetResultSets(
			int current, MapResultSet[] output, MapResultSet master, MapNextResult[] nextResults)
		{
			foreach (MapNextResult nr in nextResults)
			{
				output[current] = new MapResultSet(nr.ObjectType);

				master.AddRelation(output[current], nr.SlaveIndex, nr.MasterIndex, nr.ContainerName);

				current += GetResultSets(current + 1, output, output[current], nr.NextResults);
			}

			return current;
		}

		public MapResultSet[] ConvertToResultSet(Type masterType, params MapNextResult[] nextResults)
		{
			MapResultSet[] output = new MapResultSet[1 + GetResultCount(nextResults)];

			output[0] = new MapResultSet(masterType);

			GetResultSets(1, output, output[0], nextResults);

			return output;
		}

		#endregion
	}
}
