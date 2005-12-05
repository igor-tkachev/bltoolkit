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

		#region ToEnum, FromEnum

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public object ToEnum(object value, Type type)
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
		public object FromEnum(object value)
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
		public T ToEnum<T>(object value)
		{
			return (T)ToEnum(value, typeof(T));
		}
#endif

		#endregion

		#region GetDataSource, GetDataDestination

		[SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
		public virtual IMapDataSource GetDataSource(object obj)
		{
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

		#region Mapping

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
			IMapDataDestination dest,   object destObject)
		{
			ISupportMapping sm = destObject as ISupportMapping;

			if (sm != null)
			{
				if (initContext == null)
					initContext = new InitContext();

				initContext.MappingSchema = this;
				initContext.DataSource    = source;
				initContext.SourceObject  = sourceObject;
				initContext.ObjectMapper  = dest as ObjectMapper;

				sm.BeginMapping(initContext);

				if (initContext.StopMapping)
					return;

				if (dest != initContext.ObjectMapper && initContext.ObjectMapper != null)
					dest = initContext.ObjectMapper;
			}

			MapInternal(source, sourceObject, dest, destObject, GetIndex(source, dest));

			if (sm != null)
				sm.EndMapping(initContext);
		}

		protected virtual object MapInternal(InitContext initContext)
		{
			object dest = initContext.ObjectMapper.CreateInstance(initContext);

			if (initContext.StopMapping == false)
			{
				MapInternal(initContext,
					initContext.DataSource, initContext.SourceObject, 
					initContext.ObjectMapper, dest);
			}

			return dest;
		}

		private static ObjectMapper _nullMapper = new ObjectMapper();

		public virtual void SourceListToDestinationList(
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

				ISupportMapping sm = destObject as ISupportMapping;

				if (sm != null)
				{
					sm.BeginMapping(ctx);

					if (ctx.StopMapping)
						continue;
				}

				if (current != ctx.ObjectMapper)
				{
					current = ctx.ObjectMapper;
					index   = GetIndex(ctx.DataSource, current != null? current: dest);
				}

				MapInternal(ctx.DataSource, ctx.SourceObject, dest, destObject, index);

				if (sm != null)
					sm.EndMapping(ctx);
			}

			dataDestinationList.EndMapping(ctx);
			dataSourceList.     EndMapping(ctx);
		}

		#endregion

		#region ToObject

			#region From Object

		public object ToObject(object sourceObject, object destObject)
		{
			MapInternal(
				null,
				GetDataSource     (sourceObject), sourceObject,
				GetDataDestination(destObject),   destObject);

			return destObject;
		}

		public object ToObject(object sourceObject, object destObject, params object[] parameters)
		{
			IMapDataSource      source = GetDataSource     (sourceObject);
			IMapDataDestination dest   = GetDataDestination(destObject);

			if (dest is ObjectMapper)
			{
				InitContext ctx = new InitContext();

				ctx.DataSource   = source;
				ctx.SourceObject = sourceObject;
				ctx.ObjectMapper = (ObjectMapper)dest;
				ctx.Parameters   = parameters;

				MapInternal(ctx, ctx.DataSource, ctx.SourceObject, ctx.ObjectMapper, destObject);
			}
			else
			{
				MapInternal(null, source, sourceObject, dest, destObject);
			}

			return destObject;
		}

		public object ToObject(object sourceObject, Type destObjectType)
		{
			return ToObject(sourceObject, destObjectType, null);
		}

		public object ToObject(object sourceObject, Type destObjectType, params object[] parameters)
		{
			InitContext ctx = new InitContext();

			ctx.MappingSchema = this;
			ctx.DataSource    = GetDataSource(sourceObject);
			ctx.SourceObject  = sourceObject;
			ctx.ObjectMapper  = GetObjectMapper(destObjectType);
			ctx.Parameters    = parameters;

			return MapInternal(ctx);
		}

#if FW2
		public T ToObject<T>(object sourceObject)
		{
			return (T)ToObject(sourceObject, typeof(T), (object[])null);
		}
		
		public T ToObject<T>(object sourceObject, params object[] parameters)
		{
			return (T)ToObject(sourceObject, typeof(T), parameters);
		}
#endif

			#endregion

			#region From DataRow

		public object ToObject(DataRow dataRow, DataRowVersion version, object destObject)
		{
			MapInternal(
				null,
				new DataRowMapper(dataRow, version), dataRow,
				GetDataDestination(destObject),      destObject);

			return destObject;
		}

		public object ToObject(
			DataRow dataRow, DataRowVersion version, object destObject, params object[] parameters)
		{
			IMapDataSource      source = new DataRowMapper(dataRow, version);
			IMapDataDestination dest   = GetDataDestination(destObject);

			if (dest is ObjectMapper)
			{
				InitContext ctx = new InitContext();

				ctx.DataSource = source;
				ctx.SourceObject  = dataRow;
				ctx.ObjectMapper  = (ObjectMapper)dest;
				ctx.Parameters    = parameters;

				MapInternal(ctx, ctx.DataSource, ctx.SourceObject, ctx.ObjectMapper, destObject);
			}
			else
			{
				MapInternal(null, source, dataRow, dest, destObject);
			}

			return destObject;
		}

		public object ToObject(DataRow dataRow, DataRowVersion version, Type destObjectType)
		{
			return ToObject(dataRow, version, destObjectType, null);
		}

		public object ToObject(
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
		public T ToObject<T>(DataRow dataRow, DataRowVersion version)
		{
			return (T)ToObject(dataRow, version, typeof(T), (object[])null);
		}

		public T ToObject<T>(DataRow dataRow, DataRowVersion version, params object[] parameters)
		{
			return (T)ToObject(dataRow, version, typeof(T), parameters);
		}
#endif

			#endregion

		#endregion

		#region ListToList

		public IList ListToList(
			ICollection     sourceList,
			IList           destList,
			Type            destObjectType,
			params object[] parameters)
		{
			SourceListToDestinationList(
				new EnumeratorMapper(sourceList.GetEnumerator()),
				new ObjectListMapper(destList, GetObjectMapper(destObjectType)),
				parameters);

			return destList;
		}

		public ArrayList ListToList(ICollection sourceList, Type destObjectType, params object[] parameters)
		{
			ArrayList destList = new ArrayList();

			SourceListToDestinationList(
				new EnumeratorMapper(sourceList.GetEnumerator()),
				new ObjectListMapper(destList, GetObjectMapper(destObjectType)),
				parameters);

			return destList;
		}

#if FW2
		public List<T> ListToList<T>(ICollection sourceList, List<T> destList, params object[] parameters)
		{
			SourceListToDestinationList(
				new EnumeratorMapper(sourceList.GetEnumerator()),
				new ObjectListMapper(destList, GetObjectMapper(typeof(T))),
				parameters);

			return destList;
		}

		public List<T> ListToList<T>(ICollection sourceList, params object[] parameters)
		{
			List<T> destList = new List<T>();

			SourceListToDestinationList(
				new EnumeratorMapper(sourceList.GetEnumerator()),
				new ObjectListMapper(destList, GetObjectMapper(typeof(T))),
				parameters);

			return destList;
		}
#endif

		#endregion

		#region ListToTable

		public DataTable ListToTable(ICollection sourceList, DataTable destTable)
		{
			SourceListToDestinationList(
				new EnumeratorMapper(sourceList.GetEnumerator()),
				new DataTabletMapper(destTable),
				null);

			return destTable;
		}

		public DataTable ListToTable(ICollection sourceList)
		{
			DataTable destTable = new DataTable();

			SourceListToDestinationList(
				new EnumeratorMapper(sourceList.GetEnumerator()),
				new DataTabletMapper(destTable),
				null);

			return destTable;
		}

		#endregion

		#region TableToTable

		public DataTable TableToTable(DataTable sourceTable, DataTable destTable)
		{
			SourceListToDestinationList(
				new DataTabletMapper(sourceTable),
				new DataTabletMapper(destTable),
				null);

			return destTable;
		}

		public DataTable TableToTable(DataTable sourceTable, DataRowVersion version, DataTable destTable)
		{
			SourceListToDestinationList(
				new DataTabletMapper(sourceTable, version),
				new DataTabletMapper(destTable),
				null);

			return destTable;
		}

		public DataTable TableToTable(DataTable sourceTable)
		{
			DataTable destTable = sourceTable.Clone();

			SourceListToDestinationList(
				new DataTabletMapper(sourceTable),
				new DataTabletMapper(destTable),
				null);

			return destTable;
		}

		public DataTable TableToTable(DataTable sourceTable, DataRowVersion version)
		{
			DataTable destTable = sourceTable.Clone();

			SourceListToDestinationList(
				new DataTabletMapper(sourceTable, version),
				new DataTabletMapper(destTable),
				null);

			return destTable;
		}

		#endregion

		#region TableToList

		public IList TableToList(
			DataTable sourceTable, IList list, Type destObjectType, params object[] parameters)
		{
			SourceListToDestinationList(
				new DataTabletMapper(sourceTable),
				new ObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

		public IList TableToList(
			DataTable       sourceTable,
			DataRowVersion  version,
			IList           list,
			Type            destObjectType,
			params object[] parameters)
		{
			SourceListToDestinationList(
				new DataTabletMapper(sourceTable, version),
				new ObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

		public ArrayList TableToList(DataTable sourceTable, Type destObjectType, params object[] parameters)
		{
			ArrayList list = new ArrayList();

			SourceListToDestinationList(
				new DataTabletMapper(sourceTable),
				new ObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

		public ArrayList TableToList(
			DataTable sourceTable, DataRowVersion version, Type destObjectType, params object[] parameters)
		{
			ArrayList list = new ArrayList();

			SourceListToDestinationList(
				new DataTabletMapper(sourceTable, version),
				new ObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

#if FW2
		public List<T> TableToList<T>(DataTable sourceTable, List<T> list, params object[] parameters)
		{
			SourceListToDestinationList(
				new DataTabletMapper(sourceTable),
				new ObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}

		public List<T> TableToList<T>(
			DataTable       sourceTable,
			DataRowVersion  version,
			List<T>         list,
			params object[] parameters)
		{
			SourceListToDestinationList(
				new DataTabletMapper(sourceTable, version),
				new ObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}

		public List<T> TableToList<T>(DataTable sourceTable, params object[] parameters)
		{
			List<T> list = new List<T>();

			SourceListToDestinationList(
				new DataTabletMapper(sourceTable),
				new ObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}

		public List<T> TableToList<T>(DataTable sourceTable, DataRowVersion version, params object[] parameters)
		{
			List<T> list = new List<T>();

			SourceListToDestinationList(
				new DataTabletMapper(sourceTable, version),
				new ObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}
#endif

		#endregion

		#region DataReaderToList

		public IList DataReaderToList(
			IDataReader     reader,
			IList           list,
			Type            destObjectType,
			params object[] parameters)
		{
			SourceListToDestinationList(
				new DataReaderListMapper(reader),
				new ObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

		public ArrayList DataReaderToList(IDataReader reader, Type destObjectType, params object[] parameters)
		{
			ArrayList list = new ArrayList();

			SourceListToDestinationList(
				new DataReaderListMapper(reader),
				new ObjectListMapper(list, GetObjectMapper(destObjectType)),
				parameters);

			return list;
		}

#if FW2
		public List<T> DataReaderToList<T>(IDataReader reader, List<T> list, params object[] parameters)
		{
			SourceListToDestinationList(
				new DataReaderListMapper(reader),
				new ObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}

		public List<T> DataReaderToList<T>(IDataReader reader, params object[] parameters)
		{
			List<T> list = new List<T>();

			SourceListToDestinationList(
				new DataReaderListMapper(reader),
				new ObjectListMapper(list, GetObjectMapper(typeof(T))),
				parameters);

			return list;
		}
#endif

		#endregion
	}
}
