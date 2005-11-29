using System;
using System.Collections;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class MappingSchema
	{
		#region ObjectMapper Support

		private Hashtable _mappers = new Hashtable();

		public IObjectMapper GetObjectMapper(Type type)
		{
			IObjectMapper om = (IObjectMapper)_mappers[type];

			if (om == null)
			{
				lock (_mappers.SyncRoot)
				{
					om = (IObjectMapper)_mappers[type];

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
					}
				}
			}

			return om;
		}

		public void SetObjectMapper(Type type, IObjectMapper om)
		{
			if (type == null) throw new ArgumentNullException("type");

			_mappers[type] = om;

			if (type.IsAbstract)
				_mappers[TypeAccessor.GetAccessor(type).Type] = om;
		}

		protected virtual IObjectMapper CreateObjectMapper(Type type)
		{
			Attribute attr = TypeHelper.GetFirstAttribute(type, typeof(ObjectMapperAttribute));

			IObjectMapper om = attr == null? GetDefaultObjectMapper(type): ((ObjectMapperAttribute)attr).ObjectMapper;

			om.Init(this, TypeAccessor.GetAccessor(type));

			return om;
		}

		protected virtual IObjectMapper GetDefaultObjectMapper(Type type)
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
					if (type == typeof(Int32))   return (Int32)0;
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
					if (type == typeof(Decimal))  return (decimal)0m;
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

		#region Mapping

		protected int[] GetIndex(IMapDataSource source, IMapDataDestination dest)
		{
			int   count = source.Count;
			int[] index = new int[count];

			for (int i = 0; i < count; i++)
				index[i] = dest.GetOrdinal(source.GetName(i));

			return index;
		}

		protected virtual void MapInternal(
			IMapDataSource      source, object sourceObject,
			IMapDataDestination dest,   object destObject,
			int[]               index)
		{
			ISupportMapping sm = destObject as ISupportMapping;

			if (sm != null) sm.BeginMapping();

			int count = index.Length;

			for (int i = 0; i < count; i++)
			{
				int n = index[i];

				if (n >= 0)
					dest.SetValue(destObject, n, source.GetValue(sourceObject, i));
			}

			if (sm != null) sm.EndMapping();
		}


		#endregion
	}
}
