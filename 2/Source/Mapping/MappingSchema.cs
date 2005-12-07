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
								throw new InvalidOperationException(
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
			if (type == null) throw new ArgumentNullException("type");

			if (type.IsValueType)
			{
				if (type.IsEnum)
					return GetEnumNullValue(type);

				if (type.IsPrimitive)
				{
					if (type == typeof(Int32))   return 0;
					if (type == typeof(Double))  return (Double)0;
					if (type == typeof(Int16))   return (Int16)0;
					if (type == typeof(SByte))   return (SByte)0;
					if (type == typeof(Int64))   return (Int64)0;
					if (type == typeof(Byte))    return (Byte)0;
					if (type == typeof(UInt16))  return (UInt16)0;
					if (type == typeof(UInt32))  return (UInt32)0;
					if (type == typeof(UInt64))  return (UInt64)0;
					if (type == typeof(UInt64))  return (UInt64)0;
					if (type == typeof(Single))  return (Single)0;
					if (type == typeof(Boolean)) return false;
				}
				else
				{
					if (type == typeof(DateTime)) return DateTime.MinValue;
					if (type == typeof(Decimal))  return 0m;
					if (type == typeof(Guid))     return Guid.Empty;
				}
			}
			else
			{
				if (type == typeof(String)) return string.Empty;
			}

			return null;
		}

		private Hashtable _nullValues = new Hashtable();

		private object GetEnumNullValue(Type type)
		{
			object nullValue = _nullValues[type];

			if (nullValue != null || _nullValues.Contains(type))
				return nullValue;

			FieldInfo[] fields = type.GetFields();

			foreach (FieldInfo fi in fields)
			{
				if ((fi.Attributes & EnumField) == EnumField)
				{
					Attribute[] attrs = Attribute.GetCustomAttributes(fi, typeof(NullValueAttribute));

					if (attrs.Length > 0)
					{
						nullValue = Enum.Parse(type, fi.Name);
						break;
					}
				}
			}

			_nullValues[type] = nullValue;

			return nullValue;
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

		[SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
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

			//if (obj is IDictionary)
			//	return new DictionaryReader((IDictionary)obj);

			return GetObjectMapper(obj.GetType());
		}

		[SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
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

			//if (obj is IDictionary)
			//	return new DictionaryReader((IDictionary)obj);

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
				smDest.BeginMapping(initContext);
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
		[SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
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

		#endregion

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

		public IDictionary MapDataRowToDictionary(DataRow sourceRow, DataRowVersion version, IDictionary destDictionary)
		{
			MapInternal(
				null,
				new DataRowMapper   (sourceRow, version), sourceRow,
				new DictionaryMapper(destDictionary),     destDictionary,
				null);

			return destDictionary;
		}

		#endregion

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

		#endregion


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

		#region MapListToTable

		public DataTable MapListToTable(ICollection sourceList, DataTable destTable)
		{
			if (sourceList == null) throw new ArgumentNullException("sourceList");

			MapSourceListToDestinationList(
				new EnumeratorMapper(sourceList.GetEnumerator()),
				new DataTabletMapper(destTable),
				null);

			return destTable;
		}

		[SuppressMessage("Microsoft.Globalization", "CA1306:SetLocaleForDataTypes")]
		public DataTable MapListToTable(ICollection sourceList)
		{
			if (sourceList == null) throw new ArgumentNullException("sourceList");

			DataTable destTable = new DataTable();

			MapSourceListToDestinationList(
				new EnumeratorMapper(sourceList.GetEnumerator()),
				new DataTabletMapper(destTable),
				null);

			return destTable;
		}

		#endregion

		#region MapTableToTable

		public DataTable MapTableToTable(DataTable sourceTable, DataTable destTable)
		{
			MapSourceListToDestinationList(
				new DataTabletMapper(sourceTable),
				new DataTabletMapper(destTable),
				null);

			return destTable;
		}

		public DataTable MapTableToTable(DataTable sourceTable, DataRowVersion version, DataTable destTable)
		{
			MapSourceListToDestinationList(
				new DataTabletMapper(sourceTable, version),
				new DataTabletMapper(destTable),
				null);

			return destTable;
		}

		public DataTable MapTableToTable(DataTable sourceTable)
		{
			if (sourceTable == null) throw new ArgumentNullException("sourceTable");

			DataTable destTable = sourceTable.Clone();

			MapSourceListToDestinationList(
				new DataTabletMapper(sourceTable),
				new DataTabletMapper(destTable),
				null);

			return destTable;
		}

		public DataTable MapTableToTable(DataTable sourceTable, DataRowVersion version)
		{
			if (sourceTable == null) throw new ArgumentNullException("sourceTable");

			DataTable destTable = sourceTable.Clone();

			MapSourceListToDestinationList(
				new DataTabletMapper(sourceTable, version),
				new DataTabletMapper(destTable),
				null);

			return destTable;
		}

		#endregion

		#region MapTableToList

		public IList MapTableToList(
			DataTable sourceTable, IList list, Type destObjectType, params object[] parameters)
		{
			MapSourceListToDestinationList(
				new DataTabletMapper(sourceTable),
				new ObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

		public IList MapTableToList(
			DataTable       sourceTable,
			DataRowVersion  version,
			IList           list,
			Type            destObjectType,
			params object[] parameters)
		{
			MapSourceListToDestinationList(
				new DataTabletMapper(sourceTable, version),
				new ObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

		public ArrayList MapTableToList(DataTable sourceTable, Type destObjectType, params object[] parameters)
		{
			ArrayList list = new ArrayList();

			MapSourceListToDestinationList(
				new DataTabletMapper(sourceTable),
				new ObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

		public ArrayList MapTableToList(
			DataTable sourceTable, DataRowVersion version, Type destObjectType, params object[] parameters)
		{
			ArrayList list = new ArrayList();

			MapSourceListToDestinationList(
				new DataTabletMapper(sourceTable, version),
				new ObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

#if FW2
		public List<T> MapTableToList<T>(DataTable sourceTable, List<T> list, params object[] parameters)
		{
			MapSourceListToDestinationList(
				new DataTabletMapper(sourceTable),
				new ObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}

		public List<T> MapTableToList<T>(
			DataTable       sourceTable,
			DataRowVersion  version,
			List<T>         list,
			params object[] parameters)
		{
			MapSourceListToDestinationList(
				new DataTabletMapper(sourceTable, version),
				new ObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}

		public List<T> MapTableToList<T>(DataTable sourceTable, params object[] parameters)
		{
			List<T> list = new List<T>();

			MapSourceListToDestinationList(
				new DataTabletMapper(sourceTable),
				new ObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}

		public List<T> MapTableToList<T>(DataTable sourceTable, DataRowVersion version, params object[] parameters)
		{
			List<T> list = new List<T>();

			MapSourceListToDestinationList(
				new DataTabletMapper(sourceTable, version),
				new ObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}
#endif

		#endregion

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

		#region MapDataReaderToTable

		public DataTable MapDataReaderToTable(IDataReader reader, DataTable destTable)
		{
			MapSourceListToDestinationList(
				new DataReaderListMapper(reader),
				new DataTabletMapper(destTable),
				null);

			return destTable;
		}

		[SuppressMessage("Microsoft.Globalization", "CA1306:SetLocaleForDataTypes")]
		public DataTable MapDataReaderToTable(IDataReader reader)
		{
			DataTable destTable = new DataTable();

			MapSourceListToDestinationList(
				new DataReaderListMapper(reader),
				new DataTabletMapper(destTable),
				null);

			return destTable;
		}

		#endregion
	}
}
