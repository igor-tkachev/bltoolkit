using System;
using System.Collections;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class MappingSchema
	{
		private Hashtable _mappers = new Hashtable();

		public IObjectMapper GetObjectMapper(Type type)
		{
			IObjectMapper om = (IObjectMapper)_mappers[type];

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

		public virtual object GetNullValue(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

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

			if (type.IsValueType)
			{
				if (type == typeof(DateTime)) return DateTime.MinValue;
				if (type == typeof(Decimal))  return (decimal)0m;
				if (type == typeof(Guid))     return Guid.Empty;
			}
			else
			{
				if (type == typeof(String)) return string.Empty;
			}

			return null;
		}
	}
}
