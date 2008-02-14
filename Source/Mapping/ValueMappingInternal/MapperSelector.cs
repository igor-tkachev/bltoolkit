using System;
using System.Data.SqlTypes;

namespace BLToolkit.Mapping.ValueMappingInternal
{
	internal static class MapperSelector
	{
		public static IValueMapper GetMapper(Type from, Type to)
		{
			if (to == typeof(SByte))       return GetI8Mapper(from);
			if (to == typeof(Int16))       return GetI16Mapper(from);
			if (to == typeof(Int32))       return GetI32Mapper(from);
			if (to == typeof(Int64))       return GetI64Mapper(from);

			if (to == typeof(Byte))        return GetU8Mapper(from);
			if (to == typeof(UInt16))      return GetU16Mapper(from);
			if (to == typeof(UInt32))      return GetU32Mapper(from);
			if (to == typeof(UInt64))      return GetU64Mapper(from);

			if (to == typeof(Boolean))     return GetBMapper(from);
			if (to == typeof(Char))        return GetCMapper(from);
			if (to == typeof(Single))      return GetR4Mapper(from);
			if (to == typeof(Double))      return GetR8Mapper(from);
			if (to == typeof(Decimal))     return GetDMapper(from);

#if FW2
			if (to == typeof(SByte?))      return GetNI8Mapper(from);
			if (to == typeof(Int16?))      return GetNI16Mapper(from);
			if (to == typeof(Int32?))      return GetNI32Mapper(from);
			if (to == typeof(Int64?))      return GetNI64Mapper(from);

			if (to == typeof(Byte?))       return GetNU8Mapper(from);
			if (to == typeof(UInt16?))     return GetNU16Mapper(from);
			if (to == typeof(UInt32?))     return GetNU32Mapper(from);
			if (to == typeof(UInt64?))     return GetNU64Mapper(from);

			if (to == typeof(Boolean?))    return GetNBMapper(from);
			if (to == typeof(Char?))       return GetNCMapper(from);
			if (to == typeof(Single?))     return GetNR4Mapper(from);
			if (to == typeof(Double?))     return GetNR8Mapper(from);
			if (to == typeof(Decimal?))    return GetNDMapper(from);

#endif
			if (to == typeof(SqlString))   return GetdbSMapper(from);

			if (to == typeof(SqlByte))     return GetdbU8Mapper(from);
			if (to == typeof(SqlInt16))    return GetdbI16Mapper(from);
			if (to == typeof(SqlInt32))    return GetdbI32Mapper(from);
			if (to == typeof(SqlInt64))    return GetdbI64Mapper(from);

			if (to == typeof(SqlSingle))   return GetdbR4Mapper(from);
			if (to == typeof(SqlDouble))   return GetdbR8Mapper(from);
			if (to == typeof(SqlDecimal))  return GetdbDMapper(from);
			if (to == typeof(SqlMoney))    return GetdbMMapper(from);

			if (to == typeof(SqlBoolean))  return GetdbBMapper(from);
			if (to == typeof(SqlGuid))     return GetdbGMapper(from);
			if (to == typeof(SqlDateTime)) return GetdbDTMapper(from);

			return null;
		}

		#region Scalar Types


		private static IValueMapper GetI8Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToI8();
			if (t == typeof(Int16))       return new I16ToI8();
			if (t == typeof(Int32))       return new I32ToI8();
			if (t == typeof(Int64))       return new I64ToI8();

			if (t == typeof(Byte))        return new U8ToI8();
			if (t == typeof(UInt16))      return new U16ToI8();
			if (t == typeof(UInt32))      return new U32ToI8();
			if (t == typeof(UInt64))      return new U64ToI8();

			if (t == typeof(Boolean))     return new BToI8();
			if (t == typeof(Char))        return new CToI8();
			if (t == typeof(Single))      return new R4ToI8();
			if (t == typeof(Double))      return new R8ToI8();
			if (t == typeof(Decimal))     return new DToI8();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToI8();
			if (t == typeof(Int16?))      return new NI16ToI8();
			if (t == typeof(Int32?))      return new NI32ToI8();
			if (t == typeof(Int64?))      return new NI64ToI8();

			if (t == typeof(Byte?))       return new NU8ToI8();
			if (t == typeof(UInt16?))     return new NU16ToI8();
			if (t == typeof(UInt32?))     return new NU32ToI8();
			if (t == typeof(UInt64?))     return new NU64ToI8();

			if (t == typeof(Boolean?))    return new NBToI8();
			if (t == typeof(Char?))       return new NCToI8();
			if (t == typeof(Single?))     return new NR4ToI8();
			if (t == typeof(Double?))     return new NR8ToI8();
			if (t == typeof(Decimal?))    return new NDToI8();

#endif
			if (t == typeof(SqlString))   return new dbSToI8();

			if (t == typeof(SqlByte))     return new dbU8ToI8();
			if (t == typeof(SqlInt16))    return new dbI16ToI8();
			if (t == typeof(SqlInt32))    return new dbI32ToI8();
			if (t == typeof(SqlInt64))    return new dbI64ToI8();

			if (t == typeof(SqlSingle))   return new dbR4ToI8();
			if (t == typeof(SqlDouble))   return new dbR8ToI8();
			if (t == typeof(SqlDecimal))  return new dbDToI8();
			if (t == typeof(SqlMoney))    return new dbMToI8();

			if (t == typeof(SqlBoolean))  return new dbBToI8();

			return null;
		}

		private static IValueMapper GetI16Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToI16();
			if (t == typeof(Int16))       return new I16ToI16();
			if (t == typeof(Int32))       return new I32ToI16();
			if (t == typeof(Int64))       return new I64ToI16();

			if (t == typeof(Byte))        return new U8ToI16();
			if (t == typeof(UInt16))      return new U16ToI16();
			if (t == typeof(UInt32))      return new U32ToI16();
			if (t == typeof(UInt64))      return new U64ToI16();

			if (t == typeof(Boolean))     return new BToI16();
			if (t == typeof(Char))        return new CToI16();
			if (t == typeof(Single))      return new R4ToI16();
			if (t == typeof(Double))      return new R8ToI16();
			if (t == typeof(Decimal))     return new DToI16();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToI16();
			if (t == typeof(Int16?))      return new NI16ToI16();
			if (t == typeof(Int32?))      return new NI32ToI16();
			if (t == typeof(Int64?))      return new NI64ToI16();

			if (t == typeof(Byte?))       return new NU8ToI16();
			if (t == typeof(UInt16?))     return new NU16ToI16();
			if (t == typeof(UInt32?))     return new NU32ToI16();
			if (t == typeof(UInt64?))     return new NU64ToI16();

			if (t == typeof(Boolean?))    return new NBToI16();
			if (t == typeof(Char?))       return new NCToI16();
			if (t == typeof(Single?))     return new NR4ToI16();
			if (t == typeof(Double?))     return new NR8ToI16();
			if (t == typeof(Decimal?))    return new NDToI16();

#endif
			if (t == typeof(SqlString))   return new dbSToI16();

			if (t == typeof(SqlByte))     return new dbU8ToI16();
			if (t == typeof(SqlInt16))    return new dbI16ToI16();
			if (t == typeof(SqlInt32))    return new dbI32ToI16();
			if (t == typeof(SqlInt64))    return new dbI64ToI16();

			if (t == typeof(SqlSingle))   return new dbR4ToI16();
			if (t == typeof(SqlDouble))   return new dbR8ToI16();
			if (t == typeof(SqlDecimal))  return new dbDToI16();
			if (t == typeof(SqlMoney))    return new dbMToI16();

			if (t == typeof(SqlBoolean))  return new dbBToI16();

			return null;
		}

		private static IValueMapper GetI32Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToI32();
			if (t == typeof(Int16))       return new I16ToI32();
			if (t == typeof(Int32))       return new I32ToI32();
			if (t == typeof(Int64))       return new I64ToI32();

			if (t == typeof(Byte))        return new U8ToI32();
			if (t == typeof(UInt16))      return new U16ToI32();
			if (t == typeof(UInt32))      return new U32ToI32();
			if (t == typeof(UInt64))      return new U64ToI32();

			if (t == typeof(Boolean))     return new BToI32();
			if (t == typeof(Char))        return new CToI32();
			if (t == typeof(Single))      return new R4ToI32();
			if (t == typeof(Double))      return new R8ToI32();
			if (t == typeof(Decimal))     return new DToI32();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToI32();
			if (t == typeof(Int16?))      return new NI16ToI32();
			if (t == typeof(Int32?))      return new NI32ToI32();
			if (t == typeof(Int64?))      return new NI64ToI32();

			if (t == typeof(Byte?))       return new NU8ToI32();
			if (t == typeof(UInt16?))     return new NU16ToI32();
			if (t == typeof(UInt32?))     return new NU32ToI32();
			if (t == typeof(UInt64?))     return new NU64ToI32();

			if (t == typeof(Boolean?))    return new NBToI32();
			if (t == typeof(Char?))       return new NCToI32();
			if (t == typeof(Single?))     return new NR4ToI32();
			if (t == typeof(Double?))     return new NR8ToI32();
			if (t == typeof(Decimal?))    return new NDToI32();

#endif
			if (t == typeof(SqlString))   return new dbSToI32();

			if (t == typeof(SqlByte))     return new dbU8ToI32();
			if (t == typeof(SqlInt16))    return new dbI16ToI32();
			if (t == typeof(SqlInt32))    return new dbI32ToI32();
			if (t == typeof(SqlInt64))    return new dbI64ToI32();

			if (t == typeof(SqlSingle))   return new dbR4ToI32();
			if (t == typeof(SqlDouble))   return new dbR8ToI32();
			if (t == typeof(SqlDecimal))  return new dbDToI32();
			if (t == typeof(SqlMoney))    return new dbMToI32();

			if (t == typeof(SqlBoolean))  return new dbBToI32();

			return null;
		}

		private static IValueMapper GetI64Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToI64();
			if (t == typeof(Int16))       return new I16ToI64();
			if (t == typeof(Int32))       return new I32ToI64();
			if (t == typeof(Int64))       return new I64ToI64();

			if (t == typeof(Byte))        return new U8ToI64();
			if (t == typeof(UInt16))      return new U16ToI64();
			if (t == typeof(UInt32))      return new U32ToI64();
			if (t == typeof(UInt64))      return new U64ToI64();

			if (t == typeof(Boolean))     return new BToI64();
			if (t == typeof(Char))        return new CToI64();
			if (t == typeof(Single))      return new R4ToI64();
			if (t == typeof(Double))      return new R8ToI64();
			if (t == typeof(Decimal))     return new DToI64();
			if (t == typeof(DateTime))    return new DTToI64();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToI64();
			if (t == typeof(Int16?))      return new NI16ToI64();
			if (t == typeof(Int32?))      return new NI32ToI64();
			if (t == typeof(Int64?))      return new NI64ToI64();

			if (t == typeof(Byte?))       return new NU8ToI64();
			if (t == typeof(UInt16?))     return new NU16ToI64();
			if (t == typeof(UInt32?))     return new NU32ToI64();
			if (t == typeof(UInt64?))     return new NU64ToI64();

			if (t == typeof(Boolean?))    return new NBToI64();
			if (t == typeof(Char?))       return new NCToI64();
			if (t == typeof(Single?))     return new NR4ToI64();
			if (t == typeof(Double?))     return new NR8ToI64();
			if (t == typeof(Decimal?))    return new NDToI64();
			if (t == typeof(DateTime?))   return new NDTToI64();
#endif
			if (t == typeof(SqlString))   return new dbSToI64();

			if (t == typeof(SqlByte))     return new dbU8ToI64();
			if (t == typeof(SqlInt16))    return new dbI16ToI64();
			if (t == typeof(SqlInt32))    return new dbI32ToI64();
			if (t == typeof(SqlInt64))    return new dbI64ToI64();

			if (t == typeof(SqlSingle))   return new dbR4ToI64();
			if (t == typeof(SqlDouble))   return new dbR8ToI64();
			if (t == typeof(SqlDecimal))  return new dbDToI64();
			if (t == typeof(SqlMoney))    return new dbMToI64();

			if (t == typeof(SqlBoolean))  return new dbBToI64();
			if (t == typeof(SqlDateTime)) return new dbDTToI64();

			return null;
		}


		private static IValueMapper GetU8Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToU8();
			if (t == typeof(Int16))       return new I16ToU8();
			if (t == typeof(Int32))       return new I32ToU8();
			if (t == typeof(Int64))       return new I64ToU8();

			if (t == typeof(Byte))        return new U8ToU8();
			if (t == typeof(UInt16))      return new U16ToU8();
			if (t == typeof(UInt32))      return new U32ToU8();
			if (t == typeof(UInt64))      return new U64ToU8();

			if (t == typeof(Boolean))     return new BToU8();
			if (t == typeof(Char))        return new CToU8();
			if (t == typeof(Single))      return new R4ToU8();
			if (t == typeof(Double))      return new R8ToU8();
			if (t == typeof(Decimal))     return new DToU8();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToU8();
			if (t == typeof(Int16?))      return new NI16ToU8();
			if (t == typeof(Int32?))      return new NI32ToU8();
			if (t == typeof(Int64?))      return new NI64ToU8();

			if (t == typeof(Byte?))       return new NU8ToU8();
			if (t == typeof(UInt16?))     return new NU16ToU8();
			if (t == typeof(UInt32?))     return new NU32ToU8();
			if (t == typeof(UInt64?))     return new NU64ToU8();

			if (t == typeof(Boolean?))    return new NBToU8();
			if (t == typeof(Char?))       return new NCToU8();
			if (t == typeof(Single?))     return new NR4ToU8();
			if (t == typeof(Double?))     return new NR8ToU8();
			if (t == typeof(Decimal?))    return new NDToU8();

#endif
			if (t == typeof(SqlString))   return new dbSToU8();

			if (t == typeof(SqlByte))     return new dbU8ToU8();
			if (t == typeof(SqlInt16))    return new dbI16ToU8();
			if (t == typeof(SqlInt32))    return new dbI32ToU8();
			if (t == typeof(SqlInt64))    return new dbI64ToU8();

			if (t == typeof(SqlSingle))   return new dbR4ToU8();
			if (t == typeof(SqlDouble))   return new dbR8ToU8();
			if (t == typeof(SqlDecimal))  return new dbDToU8();
			if (t == typeof(SqlMoney))    return new dbMToU8();

			if (t == typeof(SqlBoolean))  return new dbBToU8();

			return null;
		}

		private static IValueMapper GetU16Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToU16();
			if (t == typeof(Int16))       return new I16ToU16();
			if (t == typeof(Int32))       return new I32ToU16();
			if (t == typeof(Int64))       return new I64ToU16();

			if (t == typeof(Byte))        return new U8ToU16();
			if (t == typeof(UInt16))      return new U16ToU16();
			if (t == typeof(UInt32))      return new U32ToU16();
			if (t == typeof(UInt64))      return new U64ToU16();

			if (t == typeof(Boolean))     return new BToU16();
			if (t == typeof(Char))        return new CToU16();
			if (t == typeof(Single))      return new R4ToU16();
			if (t == typeof(Double))      return new R8ToU16();
			if (t == typeof(Decimal))     return new DToU16();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToU16();
			if (t == typeof(Int16?))      return new NI16ToU16();
			if (t == typeof(Int32?))      return new NI32ToU16();
			if (t == typeof(Int64?))      return new NI64ToU16();

			if (t == typeof(Byte?))       return new NU8ToU16();
			if (t == typeof(UInt16?))     return new NU16ToU16();
			if (t == typeof(UInt32?))     return new NU32ToU16();
			if (t == typeof(UInt64?))     return new NU64ToU16();

			if (t == typeof(Boolean?))    return new NBToU16();
			if (t == typeof(Char?))       return new NCToU16();
			if (t == typeof(Single?))     return new NR4ToU16();
			if (t == typeof(Double?))     return new NR8ToU16();
			if (t == typeof(Decimal?))    return new NDToU16();

#endif
			if (t == typeof(SqlString))   return new dbSToU16();

			if (t == typeof(SqlByte))     return new dbU8ToU16();
			if (t == typeof(SqlInt16))    return new dbI16ToU16();
			if (t == typeof(SqlInt32))    return new dbI32ToU16();
			if (t == typeof(SqlInt64))    return new dbI64ToU16();

			if (t == typeof(SqlSingle))   return new dbR4ToU16();
			if (t == typeof(SqlDouble))   return new dbR8ToU16();
			if (t == typeof(SqlDecimal))  return new dbDToU16();
			if (t == typeof(SqlMoney))    return new dbMToU16();

			if (t == typeof(SqlBoolean))  return new dbBToU16();

			return null;
		}

		private static IValueMapper GetU32Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToU32();
			if (t == typeof(Int16))       return new I16ToU32();
			if (t == typeof(Int32))       return new I32ToU32();
			if (t == typeof(Int64))       return new I64ToU32();

			if (t == typeof(Byte))        return new U8ToU32();
			if (t == typeof(UInt16))      return new U16ToU32();
			if (t == typeof(UInt32))      return new U32ToU32();
			if (t == typeof(UInt64))      return new U64ToU32();

			if (t == typeof(Boolean))     return new BToU32();
			if (t == typeof(Char))        return new CToU32();
			if (t == typeof(Single))      return new R4ToU32();
			if (t == typeof(Double))      return new R8ToU32();
			if (t == typeof(Decimal))     return new DToU32();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToU32();
			if (t == typeof(Int16?))      return new NI16ToU32();
			if (t == typeof(Int32?))      return new NI32ToU32();
			if (t == typeof(Int64?))      return new NI64ToU32();

			if (t == typeof(Byte?))       return new NU8ToU32();
			if (t == typeof(UInt16?))     return new NU16ToU32();
			if (t == typeof(UInt32?))     return new NU32ToU32();
			if (t == typeof(UInt64?))     return new NU64ToU32();

			if (t == typeof(Boolean?))    return new NBToU32();
			if (t == typeof(Char?))       return new NCToU32();
			if (t == typeof(Single?))     return new NR4ToU32();
			if (t == typeof(Double?))     return new NR8ToU32();
			if (t == typeof(Decimal?))    return new NDToU32();

#endif
			if (t == typeof(SqlString))   return new dbSToU32();

			if (t == typeof(SqlByte))     return new dbU8ToU32();
			if (t == typeof(SqlInt16))    return new dbI16ToU32();
			if (t == typeof(SqlInt32))    return new dbI32ToU32();
			if (t == typeof(SqlInt64))    return new dbI64ToU32();

			if (t == typeof(SqlSingle))   return new dbR4ToU32();
			if (t == typeof(SqlDouble))   return new dbR8ToU32();
			if (t == typeof(SqlDecimal))  return new dbDToU32();
			if (t == typeof(SqlMoney))    return new dbMToU32();

			if (t == typeof(SqlBoolean))  return new dbBToU32();

			return null;
		}

		private static IValueMapper GetU64Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToU64();
			if (t == typeof(Int16))       return new I16ToU64();
			if (t == typeof(Int32))       return new I32ToU64();
			if (t == typeof(Int64))       return new I64ToU64();

			if (t == typeof(Byte))        return new U8ToU64();
			if (t == typeof(UInt16))      return new U16ToU64();
			if (t == typeof(UInt32))      return new U32ToU64();
			if (t == typeof(UInt64))      return new U64ToU64();

			if (t == typeof(Boolean))     return new BToU64();
			if (t == typeof(Char))        return new CToU64();
			if (t == typeof(Single))      return new R4ToU64();
			if (t == typeof(Double))      return new R8ToU64();
			if (t == typeof(Decimal))     return new DToU64();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToU64();
			if (t == typeof(Int16?))      return new NI16ToU64();
			if (t == typeof(Int32?))      return new NI32ToU64();
			if (t == typeof(Int64?))      return new NI64ToU64();

			if (t == typeof(Byte?))       return new NU8ToU64();
			if (t == typeof(UInt16?))     return new NU16ToU64();
			if (t == typeof(UInt32?))     return new NU32ToU64();
			if (t == typeof(UInt64?))     return new NU64ToU64();

			if (t == typeof(Boolean?))    return new NBToU64();
			if (t == typeof(Char?))       return new NCToU64();
			if (t == typeof(Single?))     return new NR4ToU64();
			if (t == typeof(Double?))     return new NR8ToU64();
			if (t == typeof(Decimal?))    return new NDToU64();

#endif
			if (t == typeof(SqlString))   return new dbSToU64();

			if (t == typeof(SqlByte))     return new dbU8ToU64();
			if (t == typeof(SqlInt16))    return new dbI16ToU64();
			if (t == typeof(SqlInt32))    return new dbI32ToU64();
			if (t == typeof(SqlInt64))    return new dbI64ToU64();

			if (t == typeof(SqlSingle))   return new dbR4ToU64();
			if (t == typeof(SqlDouble))   return new dbR8ToU64();
			if (t == typeof(SqlDecimal))  return new dbDToU64();
			if (t == typeof(SqlMoney))    return new dbMToU64();

			if (t == typeof(SqlBoolean))  return new dbBToU64();

			return null;
		}


		private static IValueMapper GetBMapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToB();
			if (t == typeof(Int16))       return new I16ToB();
			if (t == typeof(Int32))       return new I32ToB();
			if (t == typeof(Int64))       return new I64ToB();

			if (t == typeof(Byte))        return new U8ToB();
			if (t == typeof(UInt16))      return new U16ToB();
			if (t == typeof(UInt32))      return new U32ToB();
			if (t == typeof(UInt64))      return new U64ToB();

			if (t == typeof(Boolean))     return new BToB();
			if (t == typeof(Char))        return new CToB();
			if (t == typeof(Single))      return new R4ToB();
			if (t == typeof(Double))      return new R8ToB();
			if (t == typeof(Decimal))     return new DToB();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToB();
			if (t == typeof(Int16?))      return new NI16ToB();
			if (t == typeof(Int32?))      return new NI32ToB();
			if (t == typeof(Int64?))      return new NI64ToB();

			if (t == typeof(Byte?))       return new NU8ToB();
			if (t == typeof(UInt16?))     return new NU16ToB();
			if (t == typeof(UInt32?))     return new NU32ToB();
			if (t == typeof(UInt64?))     return new NU64ToB();

			if (t == typeof(Boolean?))    return new NBToB();
			if (t == typeof(Char?))       return new NCToB();
			if (t == typeof(Single?))     return new NR4ToB();
			if (t == typeof(Double?))     return new NR8ToB();
			if (t == typeof(Decimal?))    return new NDToB();

#endif
			if (t == typeof(SqlString))   return new dbSToB();

			if (t == typeof(SqlByte))     return new dbU8ToB();
			if (t == typeof(SqlInt16))    return new dbI16ToB();
			if (t == typeof(SqlInt32))    return new dbI32ToB();
			if (t == typeof(SqlInt64))    return new dbI64ToB();

			if (t == typeof(SqlSingle))   return new dbR4ToB();
			if (t == typeof(SqlDouble))   return new dbR8ToB();
			if (t == typeof(SqlDecimal))  return new dbDToB();
			if (t == typeof(SqlMoney))    return new dbMToB();

			if (t == typeof(SqlBoolean))  return new dbBToB();

			return null;
		}

		private static IValueMapper GetCMapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToC();
			if (t == typeof(Int16))       return new I16ToC();
			if (t == typeof(Int32))       return new I32ToC();
			if (t == typeof(Int64))       return new I64ToC();

			if (t == typeof(Byte))        return new U8ToC();
			if (t == typeof(UInt16))      return new U16ToC();
			if (t == typeof(UInt32))      return new U32ToC();
			if (t == typeof(UInt64))      return new U64ToC();

			if (t == typeof(Boolean))     return new BToC();
			if (t == typeof(Char))        return new CToC();
			if (t == typeof(Single))      return new R4ToC();
			if (t == typeof(Double))      return new R8ToC();
			if (t == typeof(Decimal))     return new DToC();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToC();
			if (t == typeof(Int16?))      return new NI16ToC();
			if (t == typeof(Int32?))      return new NI32ToC();
			if (t == typeof(Int64?))      return new NI64ToC();

			if (t == typeof(Byte?))       return new NU8ToC();
			if (t == typeof(UInt16?))     return new NU16ToC();
			if (t == typeof(UInt32?))     return new NU32ToC();
			if (t == typeof(UInt64?))     return new NU64ToC();

			if (t == typeof(Boolean?))    return new NBToC();
			if (t == typeof(Char?))       return new NCToC();
			if (t == typeof(Single?))     return new NR4ToC();
			if (t == typeof(Double?))     return new NR8ToC();
			if (t == typeof(Decimal?))    return new NDToC();

#endif
			if (t == typeof(SqlString))   return new dbSToC();

			if (t == typeof(SqlByte))     return new dbU8ToC();
			if (t == typeof(SqlInt16))    return new dbI16ToC();
			if (t == typeof(SqlInt32))    return new dbI32ToC();
			if (t == typeof(SqlInt64))    return new dbI64ToC();

			if (t == typeof(SqlSingle))   return new dbR4ToC();
			if (t == typeof(SqlDouble))   return new dbR8ToC();
			if (t == typeof(SqlDecimal))  return new dbDToC();
			if (t == typeof(SqlMoney))    return new dbMToC();

			if (t == typeof(SqlBoolean))  return new dbBToC();

			return null;
		}

		private static IValueMapper GetR4Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToR4();
			if (t == typeof(Int16))       return new I16ToR4();
			if (t == typeof(Int32))       return new I32ToR4();
			if (t == typeof(Int64))       return new I64ToR4();

			if (t == typeof(Byte))        return new U8ToR4();
			if (t == typeof(UInt16))      return new U16ToR4();
			if (t == typeof(UInt32))      return new U32ToR4();
			if (t == typeof(UInt64))      return new U64ToR4();

			if (t == typeof(Boolean))     return new BToR4();
			if (t == typeof(Char))        return new CToR4();
			if (t == typeof(Single))      return new R4ToR4();
			if (t == typeof(Double))      return new R8ToR4();
			if (t == typeof(Decimal))     return new DToR4();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToR4();
			if (t == typeof(Int16?))      return new NI16ToR4();
			if (t == typeof(Int32?))      return new NI32ToR4();
			if (t == typeof(Int64?))      return new NI64ToR4();

			if (t == typeof(Byte?))       return new NU8ToR4();
			if (t == typeof(UInt16?))     return new NU16ToR4();
			if (t == typeof(UInt32?))     return new NU32ToR4();
			if (t == typeof(UInt64?))     return new NU64ToR4();

			if (t == typeof(Boolean?))    return new NBToR4();
			if (t == typeof(Char?))       return new NCToR4();
			if (t == typeof(Single?))     return new NR4ToR4();
			if (t == typeof(Double?))     return new NR8ToR4();
			if (t == typeof(Decimal?))    return new NDToR4();

#endif
			if (t == typeof(SqlString))   return new dbSToR4();

			if (t == typeof(SqlByte))     return new dbU8ToR4();
			if (t == typeof(SqlInt16))    return new dbI16ToR4();
			if (t == typeof(SqlInt32))    return new dbI32ToR4();
			if (t == typeof(SqlInt64))    return new dbI64ToR4();

			if (t == typeof(SqlSingle))   return new dbR4ToR4();
			if (t == typeof(SqlDouble))   return new dbR8ToR4();
			if (t == typeof(SqlDecimal))  return new dbDToR4();
			if (t == typeof(SqlMoney))    return new dbMToR4();

			if (t == typeof(SqlBoolean))  return new dbBToR4();

			return null;
		}

		private static IValueMapper GetR8Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToR8();
			if (t == typeof(Int16))       return new I16ToR8();
			if (t == typeof(Int32))       return new I32ToR8();
			if (t == typeof(Int64))       return new I64ToR8();

			if (t == typeof(Byte))        return new U8ToR8();
			if (t == typeof(UInt16))      return new U16ToR8();
			if (t == typeof(UInt32))      return new U32ToR8();
			if (t == typeof(UInt64))      return new U64ToR8();

			if (t == typeof(Boolean))     return new BToR8();
			if (t == typeof(Char))        return new CToR8();
			if (t == typeof(Single))      return new R4ToR8();
			if (t == typeof(Double))      return new R8ToR8();
			if (t == typeof(Decimal))     return new DToR8();
			if (t == typeof(DateTime))    return new DTToR8();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToR8();
			if (t == typeof(Int16?))      return new NI16ToR8();
			if (t == typeof(Int32?))      return new NI32ToR8();
			if (t == typeof(Int64?))      return new NI64ToR8();

			if (t == typeof(Byte?))       return new NU8ToR8();
			if (t == typeof(UInt16?))     return new NU16ToR8();
			if (t == typeof(UInt32?))     return new NU32ToR8();
			if (t == typeof(UInt64?))     return new NU64ToR8();

			if (t == typeof(Boolean?))    return new NBToR8();
			if (t == typeof(Char?))       return new NCToR8();
			if (t == typeof(Single?))     return new NR4ToR8();
			if (t == typeof(Double?))     return new NR8ToR8();
			if (t == typeof(Decimal?))    return new NDToR8();
			if (t == typeof(DateTime?))   return new NDTToR8();

#endif
			if (t == typeof(SqlString))   return new dbSToR8();

			if (t == typeof(SqlByte))     return new dbU8ToR8();
			if (t == typeof(SqlInt16))    return new dbI16ToR8();
			if (t == typeof(SqlInt32))    return new dbI32ToR8();
			if (t == typeof(SqlInt64))    return new dbI64ToR8();

			if (t == typeof(SqlSingle))   return new dbR4ToR8();
			if (t == typeof(SqlDouble))   return new dbR8ToR8();
			if (t == typeof(SqlDecimal))  return new dbDToR8();
			if (t == typeof(SqlMoney))    return new dbMToR8();

			if (t == typeof(SqlBoolean))  return new dbBToR8();
			if (t == typeof(SqlDateTime)) return new dbDTToR8();

			return null;
		}

		private static IValueMapper GetDMapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToD();
			if (t == typeof(Int16))       return new I16ToD();
			if (t == typeof(Int32))       return new I32ToD();
			if (t == typeof(Int64))       return new I64ToD();

			if (t == typeof(Byte))        return new U8ToD();
			if (t == typeof(UInt16))      return new U16ToD();
			if (t == typeof(UInt32))      return new U32ToD();
			if (t == typeof(UInt64))      return new U64ToD();

			if (t == typeof(Boolean))     return new BToD();
			if (t == typeof(Char))        return new CToD();
			if (t == typeof(Single))      return new R4ToD();
			if (t == typeof(Double))      return new R8ToD();
			if (t == typeof(Decimal))     return new DToD();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToD();
			if (t == typeof(Int16?))      return new NI16ToD();
			if (t == typeof(Int32?))      return new NI32ToD();
			if (t == typeof(Int64?))      return new NI64ToD();

			if (t == typeof(Byte?))       return new NU8ToD();
			if (t == typeof(UInt16?))     return new NU16ToD();
			if (t == typeof(UInt32?))     return new NU32ToD();
			if (t == typeof(UInt64?))     return new NU64ToD();

			if (t == typeof(Boolean?))    return new NBToD();
			if (t == typeof(Char?))       return new NCToD();
			if (t == typeof(Single?))     return new NR4ToD();
			if (t == typeof(Double?))     return new NR8ToD();
			if (t == typeof(Decimal?))    return new NDToD();

#endif
			if (t == typeof(SqlString))   return new dbSToD();

			if (t == typeof(SqlByte))     return new dbU8ToD();
			if (t == typeof(SqlInt16))    return new dbI16ToD();
			if (t == typeof(SqlInt32))    return new dbI32ToD();
			if (t == typeof(SqlInt64))    return new dbI64ToD();

			if (t == typeof(SqlSingle))   return new dbR4ToD();
			if (t == typeof(SqlDouble))   return new dbR8ToD();
			if (t == typeof(SqlDecimal))  return new dbDToD();
			if (t == typeof(SqlMoney))    return new dbMToD();

			if (t == typeof(SqlBoolean))  return new dbBToD();

			return null;
		}


		#endregion 
#if FW2
		#region Nullable Types

		private static IValueMapper GetNI8Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToNI8();
			if (t == typeof(Int16))       return new I16ToNI8();
			if (t == typeof(Int32))       return new I32ToNI8();
			if (t == typeof(Int64))       return new I64ToNI8();

			if (t == typeof(Byte))        return new U8ToNI8();
			if (t == typeof(UInt16))      return new U16ToNI8();
			if (t == typeof(UInt32))      return new U32ToNI8();
			if (t == typeof(UInt64))      return new U64ToNI8();

			if (t == typeof(Boolean))     return new BToNI8();
			if (t == typeof(Char))        return new CToNI8();
			if (t == typeof(Single))      return new R4ToNI8();
			if (t == typeof(Double))      return new R8ToNI8();
			if (t == typeof(Decimal))     return new DToNI8();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToNI8();
			if (t == typeof(Int16?))      return new NI16ToNI8();
			if (t == typeof(Int32?))      return new NI32ToNI8();
			if (t == typeof(Int64?))      return new NI64ToNI8();

			if (t == typeof(Byte?))       return new NU8ToNI8();
			if (t == typeof(UInt16?))     return new NU16ToNI8();
			if (t == typeof(UInt32?))     return new NU32ToNI8();
			if (t == typeof(UInt64?))     return new NU64ToNI8();

			if (t == typeof(Boolean?))    return new NBToNI8();
			if (t == typeof(Char?))       return new NCToNI8();
			if (t == typeof(Single?))     return new NR4ToNI8();
			if (t == typeof(Double?))     return new NR8ToNI8();
			if (t == typeof(Decimal?))    return new NDToNI8();

#endif
			if (t == typeof(SqlString))   return new dbSToNI8();

			if (t == typeof(SqlByte))     return new dbU8ToNI8();
			if (t == typeof(SqlInt16))    return new dbI16ToNI8();
			if (t == typeof(SqlInt32))    return new dbI32ToNI8();
			if (t == typeof(SqlInt64))    return new dbI64ToNI8();

			if (t == typeof(SqlSingle))   return new dbR4ToNI8();
			if (t == typeof(SqlDouble))   return new dbR8ToNI8();
			if (t == typeof(SqlDecimal))  return new dbDToNI8();
			if (t == typeof(SqlMoney))    return new dbMToNI8();

			if (t == typeof(SqlBoolean))  return new dbBToNI8();

			return null;
		}

		private static IValueMapper GetNI16Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToNI16();
			if (t == typeof(Int16))       return new I16ToNI16();
			if (t == typeof(Int32))       return new I32ToNI16();
			if (t == typeof(Int64))       return new I64ToNI16();

			if (t == typeof(Byte))        return new U8ToNI16();
			if (t == typeof(UInt16))      return new U16ToNI16();
			if (t == typeof(UInt32))      return new U32ToNI16();
			if (t == typeof(UInt64))      return new U64ToNI16();

			if (t == typeof(Boolean))     return new BToNI16();
			if (t == typeof(Char))        return new CToNI16();
			if (t == typeof(Single))      return new R4ToNI16();
			if (t == typeof(Double))      return new R8ToNI16();
			if (t == typeof(Decimal))     return new DToNI16();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToNI16();
			if (t == typeof(Int16?))      return new NI16ToNI16();
			if (t == typeof(Int32?))      return new NI32ToNI16();
			if (t == typeof(Int64?))      return new NI64ToNI16();

			if (t == typeof(Byte?))       return new NU8ToNI16();
			if (t == typeof(UInt16?))     return new NU16ToNI16();
			if (t == typeof(UInt32?))     return new NU32ToNI16();
			if (t == typeof(UInt64?))     return new NU64ToNI16();

			if (t == typeof(Boolean?))    return new NBToNI16();
			if (t == typeof(Char?))       return new NCToNI16();
			if (t == typeof(Single?))     return new NR4ToNI16();
			if (t == typeof(Double?))     return new NR8ToNI16();
			if (t == typeof(Decimal?))    return new NDToNI16();

#endif
			if (t == typeof(SqlString))   return new dbSToNI16();

			if (t == typeof(SqlByte))     return new dbU8ToNI16();
			if (t == typeof(SqlInt16))    return new dbI16ToNI16();
			if (t == typeof(SqlInt32))    return new dbI32ToNI16();
			if (t == typeof(SqlInt64))    return new dbI64ToNI16();

			if (t == typeof(SqlSingle))   return new dbR4ToNI16();
			if (t == typeof(SqlDouble))   return new dbR8ToNI16();
			if (t == typeof(SqlDecimal))  return new dbDToNI16();
			if (t == typeof(SqlMoney))    return new dbMToNI16();

			if (t == typeof(SqlBoolean))  return new dbBToNI16();

			return null;
		}

		private static IValueMapper GetNI32Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToNI32();
			if (t == typeof(Int16))       return new I16ToNI32();
			if (t == typeof(Int32))       return new I32ToNI32();
			if (t == typeof(Int64))       return new I64ToNI32();

			if (t == typeof(Byte))        return new U8ToNI32();
			if (t == typeof(UInt16))      return new U16ToNI32();
			if (t == typeof(UInt32))      return new U32ToNI32();
			if (t == typeof(UInt64))      return new U64ToNI32();

			if (t == typeof(Boolean))     return new BToNI32();
			if (t == typeof(Char))        return new CToNI32();
			if (t == typeof(Single))      return new R4ToNI32();
			if (t == typeof(Double))      return new R8ToNI32();
			if (t == typeof(Decimal))     return new DToNI32();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToNI32();
			if (t == typeof(Int16?))      return new NI16ToNI32();
			if (t == typeof(Int32?))      return new NI32ToNI32();
			if (t == typeof(Int64?))      return new NI64ToNI32();

			if (t == typeof(Byte?))       return new NU8ToNI32();
			if (t == typeof(UInt16?))     return new NU16ToNI32();
			if (t == typeof(UInt32?))     return new NU32ToNI32();
			if (t == typeof(UInt64?))     return new NU64ToNI32();

			if (t == typeof(Boolean?))    return new NBToNI32();
			if (t == typeof(Char?))       return new NCToNI32();
			if (t == typeof(Single?))     return new NR4ToNI32();
			if (t == typeof(Double?))     return new NR8ToNI32();
			if (t == typeof(Decimal?))    return new NDToNI32();

#endif
			if (t == typeof(SqlString))   return new dbSToNI32();

			if (t == typeof(SqlByte))     return new dbU8ToNI32();
			if (t == typeof(SqlInt16))    return new dbI16ToNI32();
			if (t == typeof(SqlInt32))    return new dbI32ToNI32();
			if (t == typeof(SqlInt64))    return new dbI64ToNI32();

			if (t == typeof(SqlSingle))   return new dbR4ToNI32();
			if (t == typeof(SqlDouble))   return new dbR8ToNI32();
			if (t == typeof(SqlDecimal))  return new dbDToNI32();
			if (t == typeof(SqlMoney))    return new dbMToNI32();

			if (t == typeof(SqlBoolean))  return new dbBToNI32();

			return null;
		}

		private static IValueMapper GetNI64Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToNI64();
			if (t == typeof(Int16))       return new I16ToNI64();
			if (t == typeof(Int32))       return new I32ToNI64();
			if (t == typeof(Int64))       return new I64ToNI64();

			if (t == typeof(Byte))        return new U8ToNI64();
			if (t == typeof(UInt16))      return new U16ToNI64();
			if (t == typeof(UInt32))      return new U32ToNI64();
			if (t == typeof(UInt64))      return new U64ToNI64();

			if (t == typeof(Boolean))     return new BToNI64();
			if (t == typeof(Char))        return new CToNI64();
			if (t == typeof(Single))      return new R4ToNI64();
			if (t == typeof(Double))      return new R8ToNI64();
			if (t == typeof(Decimal))     return new DToNI64();
			if (t == typeof(DateTime))    return new DTToNI64();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToNI64();
			if (t == typeof(Int16?))      return new NI16ToNI64();
			if (t == typeof(Int32?))      return new NI32ToNI64();
			if (t == typeof(Int64?))      return new NI64ToNI64();

			if (t == typeof(Byte?))       return new NU8ToNI64();
			if (t == typeof(UInt16?))     return new NU16ToNI64();
			if (t == typeof(UInt32?))     return new NU32ToNI64();
			if (t == typeof(UInt64?))     return new NU64ToNI64();

			if (t == typeof(Boolean?))    return new NBToNI64();
			if (t == typeof(Char?))       return new NCToNI64();
			if (t == typeof(Single?))     return new NR4ToNI64();
			if (t == typeof(Double?))     return new NR8ToNI64();
			if (t == typeof(Decimal?))    return new NDToNI64();
			if (t == typeof(DateTime?))   return new NDTToNI64();
#endif
			if (t == typeof(SqlString))   return new dbSToNI64();

			if (t == typeof(SqlByte))     return new dbU8ToNI64();
			if (t == typeof(SqlInt16))    return new dbI16ToNI64();
			if (t == typeof(SqlInt32))    return new dbI32ToNI64();
			if (t == typeof(SqlInt64))    return new dbI64ToNI64();

			if (t == typeof(SqlSingle))   return new dbR4ToNI64();
			if (t == typeof(SqlDouble))   return new dbR8ToNI64();
			if (t == typeof(SqlDecimal))  return new dbDToNI64();
			if (t == typeof(SqlMoney))    return new dbMToNI64();

			if (t == typeof(SqlBoolean))  return new dbBToNI64();
			if (t == typeof(SqlDateTime)) return new dbDTToNI64();

			return null;
		}


		private static IValueMapper GetNU8Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToNU8();
			if (t == typeof(Int16))       return new I16ToNU8();
			if (t == typeof(Int32))       return new I32ToNU8();
			if (t == typeof(Int64))       return new I64ToNU8();

			if (t == typeof(Byte))        return new U8ToNU8();
			if (t == typeof(UInt16))      return new U16ToNU8();
			if (t == typeof(UInt32))      return new U32ToNU8();
			if (t == typeof(UInt64))      return new U64ToNU8();

			if (t == typeof(Boolean))     return new BToNU8();
			if (t == typeof(Char))        return new CToNU8();
			if (t == typeof(Single))      return new R4ToNU8();
			if (t == typeof(Double))      return new R8ToNU8();
			if (t == typeof(Decimal))     return new DToNU8();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToNU8();
			if (t == typeof(Int16?))      return new NI16ToNU8();
			if (t == typeof(Int32?))      return new NI32ToNU8();
			if (t == typeof(Int64?))      return new NI64ToNU8();

			if (t == typeof(Byte?))       return new NU8ToNU8();
			if (t == typeof(UInt16?))     return new NU16ToNU8();
			if (t == typeof(UInt32?))     return new NU32ToNU8();
			if (t == typeof(UInt64?))     return new NU64ToNU8();

			if (t == typeof(Boolean?))    return new NBToNU8();
			if (t == typeof(Char?))       return new NCToNU8();
			if (t == typeof(Single?))     return new NR4ToNU8();
			if (t == typeof(Double?))     return new NR8ToNU8();
			if (t == typeof(Decimal?))    return new NDToNU8();

#endif
			if (t == typeof(SqlString))   return new dbSToNU8();

			if (t == typeof(SqlByte))     return new dbU8ToNU8();
			if (t == typeof(SqlInt16))    return new dbI16ToNU8();
			if (t == typeof(SqlInt32))    return new dbI32ToNU8();
			if (t == typeof(SqlInt64))    return new dbI64ToNU8();

			if (t == typeof(SqlSingle))   return new dbR4ToNU8();
			if (t == typeof(SqlDouble))   return new dbR8ToNU8();
			if (t == typeof(SqlDecimal))  return new dbDToNU8();
			if (t == typeof(SqlMoney))    return new dbMToNU8();

			if (t == typeof(SqlBoolean))  return new dbBToNU8();

			return null;
		}

		private static IValueMapper GetNU16Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToNU16();
			if (t == typeof(Int16))       return new I16ToNU16();
			if (t == typeof(Int32))       return new I32ToNU16();
			if (t == typeof(Int64))       return new I64ToNU16();

			if (t == typeof(Byte))        return new U8ToNU16();
			if (t == typeof(UInt16))      return new U16ToNU16();
			if (t == typeof(UInt32))      return new U32ToNU16();
			if (t == typeof(UInt64))      return new U64ToNU16();

			if (t == typeof(Boolean))     return new BToNU16();
			if (t == typeof(Char))        return new CToNU16();
			if (t == typeof(Single))      return new R4ToNU16();
			if (t == typeof(Double))      return new R8ToNU16();
			if (t == typeof(Decimal))     return new DToNU16();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToNU16();
			if (t == typeof(Int16?))      return new NI16ToNU16();
			if (t == typeof(Int32?))      return new NI32ToNU16();
			if (t == typeof(Int64?))      return new NI64ToNU16();

			if (t == typeof(Byte?))       return new NU8ToNU16();
			if (t == typeof(UInt16?))     return new NU16ToNU16();
			if (t == typeof(UInt32?))     return new NU32ToNU16();
			if (t == typeof(UInt64?))     return new NU64ToNU16();

			if (t == typeof(Boolean?))    return new NBToNU16();
			if (t == typeof(Char?))       return new NCToNU16();
			if (t == typeof(Single?))     return new NR4ToNU16();
			if (t == typeof(Double?))     return new NR8ToNU16();
			if (t == typeof(Decimal?))    return new NDToNU16();

#endif
			if (t == typeof(SqlString))   return new dbSToNU16();

			if (t == typeof(SqlByte))     return new dbU8ToNU16();
			if (t == typeof(SqlInt16))    return new dbI16ToNU16();
			if (t == typeof(SqlInt32))    return new dbI32ToNU16();
			if (t == typeof(SqlInt64))    return new dbI64ToNU16();

			if (t == typeof(SqlSingle))   return new dbR4ToNU16();
			if (t == typeof(SqlDouble))   return new dbR8ToNU16();
			if (t == typeof(SqlDecimal))  return new dbDToNU16();
			if (t == typeof(SqlMoney))    return new dbMToNU16();

			if (t == typeof(SqlBoolean))  return new dbBToNU16();

			return null;
		}

		private static IValueMapper GetNU32Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToNU32();
			if (t == typeof(Int16))       return new I16ToNU32();
			if (t == typeof(Int32))       return new I32ToNU32();
			if (t == typeof(Int64))       return new I64ToNU32();

			if (t == typeof(Byte))        return new U8ToNU32();
			if (t == typeof(UInt16))      return new U16ToNU32();
			if (t == typeof(UInt32))      return new U32ToNU32();
			if (t == typeof(UInt64))      return new U64ToNU32();

			if (t == typeof(Boolean))     return new BToNU32();
			if (t == typeof(Char))        return new CToNU32();
			if (t == typeof(Single))      return new R4ToNU32();
			if (t == typeof(Double))      return new R8ToNU32();
			if (t == typeof(Decimal))     return new DToNU32();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToNU32();
			if (t == typeof(Int16?))      return new NI16ToNU32();
			if (t == typeof(Int32?))      return new NI32ToNU32();
			if (t == typeof(Int64?))      return new NI64ToNU32();

			if (t == typeof(Byte?))       return new NU8ToNU32();
			if (t == typeof(UInt16?))     return new NU16ToNU32();
			if (t == typeof(UInt32?))     return new NU32ToNU32();
			if (t == typeof(UInt64?))     return new NU64ToNU32();

			if (t == typeof(Boolean?))    return new NBToNU32();
			if (t == typeof(Char?))       return new NCToNU32();
			if (t == typeof(Single?))     return new NR4ToNU32();
			if (t == typeof(Double?))     return new NR8ToNU32();
			if (t == typeof(Decimal?))    return new NDToNU32();

#endif
			if (t == typeof(SqlString))   return new dbSToNU32();

			if (t == typeof(SqlByte))     return new dbU8ToNU32();
			if (t == typeof(SqlInt16))    return new dbI16ToNU32();
			if (t == typeof(SqlInt32))    return new dbI32ToNU32();
			if (t == typeof(SqlInt64))    return new dbI64ToNU32();

			if (t == typeof(SqlSingle))   return new dbR4ToNU32();
			if (t == typeof(SqlDouble))   return new dbR8ToNU32();
			if (t == typeof(SqlDecimal))  return new dbDToNU32();
			if (t == typeof(SqlMoney))    return new dbMToNU32();

			if (t == typeof(SqlBoolean))  return new dbBToNU32();

			return null;
		}

		private static IValueMapper GetNU64Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToNU64();
			if (t == typeof(Int16))       return new I16ToNU64();
			if (t == typeof(Int32))       return new I32ToNU64();
			if (t == typeof(Int64))       return new I64ToNU64();

			if (t == typeof(Byte))        return new U8ToNU64();
			if (t == typeof(UInt16))      return new U16ToNU64();
			if (t == typeof(UInt32))      return new U32ToNU64();
			if (t == typeof(UInt64))      return new U64ToNU64();

			if (t == typeof(Boolean))     return new BToNU64();
			if (t == typeof(Char))        return new CToNU64();
			if (t == typeof(Single))      return new R4ToNU64();
			if (t == typeof(Double))      return new R8ToNU64();
			if (t == typeof(Decimal))     return new DToNU64();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToNU64();
			if (t == typeof(Int16?))      return new NI16ToNU64();
			if (t == typeof(Int32?))      return new NI32ToNU64();
			if (t == typeof(Int64?))      return new NI64ToNU64();

			if (t == typeof(Byte?))       return new NU8ToNU64();
			if (t == typeof(UInt16?))     return new NU16ToNU64();
			if (t == typeof(UInt32?))     return new NU32ToNU64();
			if (t == typeof(UInt64?))     return new NU64ToNU64();

			if (t == typeof(Boolean?))    return new NBToNU64();
			if (t == typeof(Char?))       return new NCToNU64();
			if (t == typeof(Single?))     return new NR4ToNU64();
			if (t == typeof(Double?))     return new NR8ToNU64();
			if (t == typeof(Decimal?))    return new NDToNU64();

#endif
			if (t == typeof(SqlString))   return new dbSToNU64();

			if (t == typeof(SqlByte))     return new dbU8ToNU64();
			if (t == typeof(SqlInt16))    return new dbI16ToNU64();
			if (t == typeof(SqlInt32))    return new dbI32ToNU64();
			if (t == typeof(SqlInt64))    return new dbI64ToNU64();

			if (t == typeof(SqlSingle))   return new dbR4ToNU64();
			if (t == typeof(SqlDouble))   return new dbR8ToNU64();
			if (t == typeof(SqlDecimal))  return new dbDToNU64();
			if (t == typeof(SqlMoney))    return new dbMToNU64();

			if (t == typeof(SqlBoolean))  return new dbBToNU64();

			return null;
		}


		private static IValueMapper GetNBMapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToNB();
			if (t == typeof(Int16))       return new I16ToNB();
			if (t == typeof(Int32))       return new I32ToNB();
			if (t == typeof(Int64))       return new I64ToNB();

			if (t == typeof(Byte))        return new U8ToNB();
			if (t == typeof(UInt16))      return new U16ToNB();
			if (t == typeof(UInt32))      return new U32ToNB();
			if (t == typeof(UInt64))      return new U64ToNB();

			if (t == typeof(Boolean))     return new BToNB();
			if (t == typeof(Char))        return new CToNB();
			if (t == typeof(Single))      return new R4ToNB();
			if (t == typeof(Double))      return new R8ToNB();
			if (t == typeof(Decimal))     return new DToNB();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToNB();
			if (t == typeof(Int16?))      return new NI16ToNB();
			if (t == typeof(Int32?))      return new NI32ToNB();
			if (t == typeof(Int64?))      return new NI64ToNB();

			if (t == typeof(Byte?))       return new NU8ToNB();
			if (t == typeof(UInt16?))     return new NU16ToNB();
			if (t == typeof(UInt32?))     return new NU32ToNB();
			if (t == typeof(UInt64?))     return new NU64ToNB();

			if (t == typeof(Boolean?))    return new NBToNB();
			if (t == typeof(Char?))       return new NCToNB();
			if (t == typeof(Single?))     return new NR4ToNB();
			if (t == typeof(Double?))     return new NR8ToNB();
			if (t == typeof(Decimal?))    return new NDToNB();

#endif
			if (t == typeof(SqlString))   return new dbSToNB();

			if (t == typeof(SqlByte))     return new dbU8ToNB();
			if (t == typeof(SqlInt16))    return new dbI16ToNB();
			if (t == typeof(SqlInt32))    return new dbI32ToNB();
			if (t == typeof(SqlInt64))    return new dbI64ToNB();

			if (t == typeof(SqlSingle))   return new dbR4ToNB();
			if (t == typeof(SqlDouble))   return new dbR8ToNB();
			if (t == typeof(SqlDecimal))  return new dbDToNB();
			if (t == typeof(SqlMoney))    return new dbMToNB();

			if (t == typeof(SqlBoolean))  return new dbBToNB();

			return null;
		}

		private static IValueMapper GetNCMapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToNC();
			if (t == typeof(Int16))       return new I16ToNC();
			if (t == typeof(Int32))       return new I32ToNC();
			if (t == typeof(Int64))       return new I64ToNC();

			if (t == typeof(Byte))        return new U8ToNC();
			if (t == typeof(UInt16))      return new U16ToNC();
			if (t == typeof(UInt32))      return new U32ToNC();
			if (t == typeof(UInt64))      return new U64ToNC();

			if (t == typeof(Boolean))     return new BToNC();
			if (t == typeof(Char))        return new CToNC();
			if (t == typeof(Decimal))     return new DToNC();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToNC();
			if (t == typeof(Int16?))      return new NI16ToNC();
			if (t == typeof(Int32?))      return new NI32ToNC();
			if (t == typeof(Int64?))      return new NI64ToNC();

			if (t == typeof(Byte?))       return new NU8ToNC();
			if (t == typeof(UInt16?))     return new NU16ToNC();
			if (t == typeof(UInt32?))     return new NU32ToNC();
			if (t == typeof(UInt64?))     return new NU64ToNC();

			if (t == typeof(Boolean?))    return new NBToNC();
			if (t == typeof(Char?))       return new NCToNC();
			if (t == typeof(Single?))     return new NR4ToNC();
			if (t == typeof(Double?))     return new NR8ToNC();
			if (t == typeof(Decimal?))    return new NDToNC();

#endif
			if (t == typeof(SqlString))   return new dbSToNC();

			if (t == typeof(SqlByte))     return new dbU8ToNC();
			if (t == typeof(SqlInt16))    return new dbI16ToNC();
			if (t == typeof(SqlInt32))    return new dbI32ToNC();
			if (t == typeof(SqlInt64))    return new dbI64ToNC();

			if (t == typeof(SqlSingle))   return new dbR4ToNC();
			if (t == typeof(SqlDouble))   return new dbR8ToNC();
			if (t == typeof(SqlDecimal))  return new dbDToNC();
			if (t == typeof(SqlMoney))    return new dbMToNC();

			if (t == typeof(SqlBoolean))  return new dbBToNC();

			return null;
		}

		private static IValueMapper GetNR4Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToNR4();
			if (t == typeof(Int16))       return new I16ToNR4();
			if (t == typeof(Int32))       return new I32ToNR4();
			if (t == typeof(Int64))       return new I64ToNR4();

			if (t == typeof(Byte))        return new U8ToNR4();
			if (t == typeof(UInt16))      return new U16ToNR4();
			if (t == typeof(UInt32))      return new U32ToNR4();
			if (t == typeof(UInt64))      return new U64ToNR4();

			if (t == typeof(Boolean))     return new BToNR4();
			if (t == typeof(Char))        return new CToNR4();
			if (t == typeof(Single))      return new R4ToNR4();
			if (t == typeof(Double))      return new R8ToNR4();
			if (t == typeof(Decimal))     return new DToNR4();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToNR4();
			if (t == typeof(Int16?))      return new NI16ToNR4();
			if (t == typeof(Int32?))      return new NI32ToNR4();
			if (t == typeof(Int64?))      return new NI64ToNR4();

			if (t == typeof(Byte?))       return new NU8ToNR4();
			if (t == typeof(UInt16?))     return new NU16ToNR4();
			if (t == typeof(UInt32?))     return new NU32ToNR4();
			if (t == typeof(UInt64?))     return new NU64ToNR4();

			if (t == typeof(Boolean?))    return new NBToNR4();
			if (t == typeof(Char?))       return new NCToNR4();
			if (t == typeof(Single?))     return new NR4ToNR4();
			if (t == typeof(Double?))     return new NR8ToNR4();
			if (t == typeof(Decimal?))    return new NDToNR4();

#endif
			if (t == typeof(SqlString))   return new dbSToNR4();

			if (t == typeof(SqlByte))     return new dbU8ToNR4();
			if (t == typeof(SqlInt16))    return new dbI16ToNR4();
			if (t == typeof(SqlInt32))    return new dbI32ToNR4();
			if (t == typeof(SqlInt64))    return new dbI64ToNR4();

			if (t == typeof(SqlSingle))   return new dbR4ToNR4();
			if (t == typeof(SqlDouble))   return new dbR8ToNR4();
			if (t == typeof(SqlDecimal))  return new dbDToNR4();
			if (t == typeof(SqlMoney))    return new dbMToNR4();

			if (t == typeof(SqlBoolean))  return new dbBToNR4();

			return null;
		}

		private static IValueMapper GetNR8Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToNR8();
			if (t == typeof(Int16))       return new I16ToNR8();
			if (t == typeof(Int32))       return new I32ToNR8();
			if (t == typeof(Int64))       return new I64ToNR8();

			if (t == typeof(Byte))        return new U8ToNR8();
			if (t == typeof(UInt16))      return new U16ToNR8();
			if (t == typeof(UInt32))      return new U32ToNR8();
			if (t == typeof(UInt64))      return new U64ToNR8();

			if (t == typeof(Boolean))     return new BToNR8();
			if (t == typeof(Char))        return new CToNR8();
			if (t == typeof(Single))      return new R4ToNR8();
			if (t == typeof(Double))      return new R8ToNR8();
			if (t == typeof(Decimal))     return new DToNR8();
			if (t == typeof(DateTime))    return new DTToNR8();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToNR8();
			if (t == typeof(Int16?))      return new NI16ToNR8();
			if (t == typeof(Int32?))      return new NI32ToNR8();
			if (t == typeof(Int64?))      return new NI64ToNR8();

			if (t == typeof(Byte?))       return new NU8ToNR8();
			if (t == typeof(UInt16?))     return new NU16ToNR8();
			if (t == typeof(UInt32?))     return new NU32ToNR8();
			if (t == typeof(UInt64?))     return new NU64ToNR8();

			if (t == typeof(Boolean?))    return new NBToNR8();
			if (t == typeof(Char?))       return new NCToNR8();
			if (t == typeof(Single?))     return new NR4ToNR8();
			if (t == typeof(Double?))     return new NR8ToNR8();
			if (t == typeof(Decimal?))    return new NDToNR8();
			if (t == typeof(DateTime?))   return new NDTToNR8();
#endif
			if (t == typeof(SqlString))   return new dbSToNR8();

			if (t == typeof(SqlByte))     return new dbU8ToNR8();
			if (t == typeof(SqlInt16))    return new dbI16ToNR8();
			if (t == typeof(SqlInt32))    return new dbI32ToNR8();
			if (t == typeof(SqlInt64))    return new dbI64ToNR8();

			if (t == typeof(SqlSingle))   return new dbR4ToNR8();
			if (t == typeof(SqlDouble))   return new dbR8ToNR8();
			if (t == typeof(SqlDecimal))  return new dbDToNR8();
			if (t == typeof(SqlMoney))    return new dbMToNR8();

			if (t == typeof(SqlBoolean))  return new dbBToNR8();
			if (t == typeof(SqlDateTime)) return new dbDTToNR8();

			return null;
		}

		private static IValueMapper GetNDMapper(Type t)
		{
			if (t == typeof(SByte))       return new I8ToND();
			if (t == typeof(Int16))       return new I16ToND();
			if (t == typeof(Int32))       return new I32ToND();
			if (t == typeof(Int64))       return new I64ToND();

			if (t == typeof(Byte))        return new U8ToND();
			if (t == typeof(UInt16))      return new U16ToND();
			if (t == typeof(UInt32))      return new U32ToND();
			if (t == typeof(UInt64))      return new U64ToND();

			if (t == typeof(Boolean))     return new BToND();
			if (t == typeof(Char))        return new CToND();
			if (t == typeof(Single))      return new R4ToND();
			if (t == typeof(Double))      return new R8ToND();
			if (t == typeof(Decimal))     return new DToND();

#if FW2
			if (t == typeof(SByte?))      return new NI8ToND();
			if (t == typeof(Int16?))      return new NI16ToND();
			if (t == typeof(Int32?))      return new NI32ToND();
			if (t == typeof(Int64?))      return new NI64ToND();

			if (t == typeof(Byte?))       return new NU8ToND();
			if (t == typeof(UInt16?))     return new NU16ToND();
			if (t == typeof(UInt32?))     return new NU32ToND();
			if (t == typeof(UInt64?))     return new NU64ToND();

			if (t == typeof(Boolean?))    return new NBToND();
			if (t == typeof(Char?))       return new NCToND();
			if (t == typeof(Single?))     return new NR4ToND();
			if (t == typeof(Double?))     return new NR8ToND();
			if (t == typeof(Decimal?))    return new NDToND();

#endif
			if (t == typeof(SqlString))   return new dbSToND();

			if (t == typeof(SqlByte))     return new dbU8ToND();
			if (t == typeof(SqlInt16))    return new dbI16ToND();
			if (t == typeof(SqlInt32))    return new dbI32ToND();
			if (t == typeof(SqlInt64))    return new dbI64ToND();

			if (t == typeof(SqlSingle))   return new dbR4ToND();
			if (t == typeof(SqlDouble))   return new dbR8ToND();
			if (t == typeof(SqlDecimal))  return new dbDToND();
			if (t == typeof(SqlMoney))    return new dbMToND();

			if (t == typeof(SqlBoolean))  return new dbBToND();

			return null;
		}

		#endregion 
#endif

		#region SqlTypes

		private static IValueMapper GetdbSMapper(Type t)
		{
			if (t == typeof(SByte))       return new I8TodbS();
			if (t == typeof(Int16))       return new I16TodbS();
			if (t == typeof(Int32))       return new I32TodbS();
			if (t == typeof(Int64))       return new I64TodbS();

			if (t == typeof(Byte))        return new U8TodbS();
			if (t == typeof(UInt16))      return new U16TodbS();
			if (t == typeof(UInt32))      return new U32TodbS();
			if (t == typeof(UInt64))      return new U64TodbS();

			if (t == typeof(Boolean))     return new BTodbS();
			if (t == typeof(Char))        return new CTodbS();
			if (t == typeof(Single))      return new R4TodbS();
			if (t == typeof(Double))      return new R8TodbS();
			if (t == typeof(Decimal))     return new DTodbS();

#if FW2
			if (t == typeof(SByte?))      return new NI8TodbS();
			if (t == typeof(Int16?))      return new NI16TodbS();
			if (t == typeof(Int32?))      return new NI32TodbS();
			if (t == typeof(Int64?))      return new NI64TodbS();

			if (t == typeof(Byte?))       return new NU8TodbS();
			if (t == typeof(UInt16?))     return new NU16TodbS();
			if (t == typeof(UInt32?))     return new NU32TodbS();
			if (t == typeof(UInt64?))     return new NU64TodbS();

			if (t == typeof(Boolean?))    return new NBTodbS();
			if (t == typeof(Char?))       return new NCTodbS();
			if (t == typeof(Single?))     return new NR4TodbS();
			if (t == typeof(Double?))     return new NR8TodbS();
			if (t == typeof(Decimal?))    return new NDTodbS();

#endif
			if (t == typeof(SqlString))   return new dbSTodbS();

			if (t == typeof(SqlByte))     return new dbU8TodbS();
			if (t == typeof(SqlInt16))    return new dbI16TodbS();
			if (t == typeof(SqlInt32))    return new dbI32TodbS();
			if (t == typeof(SqlInt64))    return new dbI64TodbS();

			if (t == typeof(SqlSingle))   return new dbR4TodbS();
			if (t == typeof(SqlDouble))   return new dbR8TodbS();
			if (t == typeof(SqlDecimal))  return new dbDTodbS();
			if (t == typeof(SqlMoney))    return new dbMTodbS();

			if (t == typeof(SqlBoolean))  return new dbBTodbS();

			return null;
		}


		private static IValueMapper GetdbU8Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8TodbU8();
			if (t == typeof(Int16))       return new I16TodbU8();
			if (t == typeof(Int32))       return new I32TodbU8();
			if (t == typeof(Int64))       return new I64TodbU8();

			if (t == typeof(Byte))        return new U8TodbU8();
			if (t == typeof(UInt16))      return new U16TodbU8();
			if (t == typeof(UInt32))      return new U32TodbU8();
			if (t == typeof(UInt64))      return new U64TodbU8();

			if (t == typeof(Boolean))     return new BTodbU8();
			if (t == typeof(Char))        return new CTodbU8();
			if (t == typeof(Single))      return new R4TodbU8();
			if (t == typeof(Double))      return new R8TodbU8();
			if (t == typeof(Decimal))     return new DTodbU8();

#if FW2
			if (t == typeof(SByte?))      return new NI8TodbU8();
			if (t == typeof(Int16?))      return new NI16TodbU8();
			if (t == typeof(Int32?))      return new NI32TodbU8();
			if (t == typeof(Int64?))      return new NI64TodbU8();

			if (t == typeof(Byte?))       return new NU8TodbU8();
			if (t == typeof(UInt16?))     return new NU16TodbU8();
			if (t == typeof(UInt32?))     return new NU32TodbU8();
			if (t == typeof(UInt64?))     return new NU64TodbU8();

			if (t == typeof(Boolean?))    return new NBTodbU8();
			if (t == typeof(Char?))       return new NCTodbU8();
			if (t == typeof(Single?))     return new NR4TodbU8();
			if (t == typeof(Double?))     return new NR8TodbU8();
			if (t == typeof(Decimal?))    return new NDTodbU8();

#endif
			if (t == typeof(SqlString))   return new dbSTodbU8();

			if (t == typeof(SqlByte))     return new dbU8TodbU8();
			if (t == typeof(SqlInt16))    return new dbI16TodbU8();
			if (t == typeof(SqlInt32))    return new dbI32TodbU8();
			if (t == typeof(SqlInt64))    return new dbI64TodbU8();

			if (t == typeof(SqlSingle))   return new dbR4TodbU8();
			if (t == typeof(SqlDouble))   return new dbR8TodbU8();
			if (t == typeof(SqlDecimal))  return new dbDTodbU8();
			if (t == typeof(SqlMoney))    return new dbMTodbU8();

			if (t == typeof(SqlBoolean))  return new dbBTodbU8();

			return null;
		}

		private static IValueMapper GetdbI16Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8TodbI16();
			if (t == typeof(Int16))       return new I16TodbI16();
			if (t == typeof(Int32))       return new I32TodbI16();
			if (t == typeof(Int64))       return new I64TodbI16();

			if (t == typeof(Byte))        return new U8TodbI16();
			if (t == typeof(UInt16))      return new U16TodbI16();
			if (t == typeof(UInt32))      return new U32TodbI16();
			if (t == typeof(UInt64))      return new U64TodbI16();

			if (t == typeof(Boolean))     return new BTodbI16();
			if (t == typeof(Char))        return new CTodbI16();
			if (t == typeof(Single))      return new R4TodbI16();
			if (t == typeof(Double))      return new R8TodbI16();
			if (t == typeof(Decimal))     return new DTodbI16();

#if FW2
			if (t == typeof(SByte?))      return new NI8TodbI16();
			if (t == typeof(Int16?))      return new NI16TodbI16();
			if (t == typeof(Int32?))      return new NI32TodbI16();
			if (t == typeof(Int64?))      return new NI64TodbI16();

			if (t == typeof(Byte?))       return new NU8TodbI16();
			if (t == typeof(UInt16?))     return new NU16TodbI16();
			if (t == typeof(UInt32?))     return new NU32TodbI16();
			if (t == typeof(UInt64?))     return new NU64TodbI16();

			if (t == typeof(Boolean?))    return new NBTodbI16();
			if (t == typeof(Char?))       return new NCTodbI16();
			if (t == typeof(Single?))     return new NR4TodbI16();
			if (t == typeof(Double?))     return new NR8TodbI16();
			if (t == typeof(Decimal?))    return new NDTodbI16();

#endif
			if (t == typeof(SqlString))   return new dbSTodbI16();

			if (t == typeof(SqlByte))     return new dbU8TodbI16();
			if (t == typeof(SqlInt16))    return new dbI16TodbI16();
			if (t == typeof(SqlInt32))    return new dbI32TodbI16();
			if (t == typeof(SqlInt64))    return new dbI64TodbI16();

			if (t == typeof(SqlSingle))   return new dbR4TodbI16();
			if (t == typeof(SqlDouble))   return new dbR8TodbI16();
			if (t == typeof(SqlDecimal))  return new dbDTodbI16();
			if (t == typeof(SqlMoney))    return new dbMTodbI16();

			if (t == typeof(SqlBoolean))  return new dbBTodbI16();

			return null;
		}

		private static IValueMapper GetdbI32Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8TodbI32();
			if (t == typeof(Int16))       return new I16TodbI32();
			if (t == typeof(Int32))       return new I32TodbI32();
			if (t == typeof(Int64))       return new I64TodbI32();

			if (t == typeof(Byte))        return new U8TodbI32();
			if (t == typeof(UInt16))      return new U16TodbI32();
			if (t == typeof(UInt32))      return new U32TodbI32();
			if (t == typeof(UInt64))      return new U64TodbI32();

			if (t == typeof(Boolean))     return new BTodbI32();
			if (t == typeof(Char))        return new CTodbI32();
			if (t == typeof(Single))      return new R4TodbI32();
			if (t == typeof(Double))      return new R8TodbI32();
			if (t == typeof(Decimal))     return new DTodbI32();

#if FW2
			if (t == typeof(SByte?))      return new NI8TodbI32();
			if (t == typeof(Int16?))      return new NI16TodbI32();
			if (t == typeof(Int32?))      return new NI32TodbI32();
			if (t == typeof(Int64?))      return new NI64TodbI32();

			if (t == typeof(Byte?))       return new NU8TodbI32();
			if (t == typeof(UInt16?))     return new NU16TodbI32();
			if (t == typeof(UInt32?))     return new NU32TodbI32();
			if (t == typeof(UInt64?))     return new NU64TodbI32();

			if (t == typeof(Boolean?))    return new NBTodbI32();
			if (t == typeof(Char?))       return new NCTodbI32();
			if (t == typeof(Single?))     return new NR4TodbI32();
			if (t == typeof(Double?))     return new NR8TodbI32();
			if (t == typeof(Decimal?))    return new NDTodbI32();

#endif
			if (t == typeof(SqlString))   return new dbSTodbI32();

			if (t == typeof(SqlByte))     return new dbU8TodbI32();
			if (t == typeof(SqlInt16))    return new dbI16TodbI32();
			if (t == typeof(SqlInt32))    return new dbI32TodbI32();
			if (t == typeof(SqlInt64))    return new dbI64TodbI32();

			if (t == typeof(SqlSingle))   return new dbR4TodbI32();
			if (t == typeof(SqlDouble))   return new dbR8TodbI32();
			if (t == typeof(SqlDecimal))  return new dbDTodbI32();
			if (t == typeof(SqlMoney))    return new dbMTodbI32();

			if (t == typeof(SqlBoolean))  return new dbBTodbI32();

			return null;
		}

		private static IValueMapper GetdbI64Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8TodbI64();
			if (t == typeof(Int16))       return new I16TodbI64();
			if (t == typeof(Int32))       return new I32TodbI64();
			if (t == typeof(Int64))       return new I64TodbI64();

			if (t == typeof(Byte))        return new U8TodbI64();
			if (t == typeof(UInt16))      return new U16TodbI64();
			if (t == typeof(UInt32))      return new U32TodbI64();
			if (t == typeof(UInt64))      return new U64TodbI64();

			if (t == typeof(Boolean))     return new BTodbI64();
			if (t == typeof(Char))        return new CTodbI64();
			if (t == typeof(Single))      return new R4TodbI64();
			if (t == typeof(Double))      return new R8TodbI64();
			if (t == typeof(Decimal))     return new DTodbI64();

#if FW2
			if (t == typeof(SByte?))      return new NI8TodbI64();
			if (t == typeof(Int16?))      return new NI16TodbI64();
			if (t == typeof(Int32?))      return new NI32TodbI64();
			if (t == typeof(Int64?))      return new NI64TodbI64();

			if (t == typeof(Byte?))       return new NU8TodbI64();
			if (t == typeof(UInt16?))     return new NU16TodbI64();
			if (t == typeof(UInt32?))     return new NU32TodbI64();
			if (t == typeof(UInt64?))     return new NU64TodbI64();

			if (t == typeof(Boolean?))    return new NBTodbI64();
			if (t == typeof(Char?))       return new NCTodbI64();
			if (t == typeof(Single?))     return new NR4TodbI64();
			if (t == typeof(Double?))     return new NR8TodbI64();
			if (t == typeof(Decimal?))    return new NDTodbI64();

#endif
			if (t == typeof(SqlString))   return new dbSTodbI64();

			if (t == typeof(SqlByte))     return new dbU8TodbI64();
			if (t == typeof(SqlInt16))    return new dbI16TodbI64();
			if (t == typeof(SqlInt32))    return new dbI32TodbI64();
			if (t == typeof(SqlInt64))    return new dbI64TodbI64();

			if (t == typeof(SqlSingle))   return new dbR4TodbI64();
			if (t == typeof(SqlDouble))   return new dbR8TodbI64();
			if (t == typeof(SqlDecimal))  return new dbDTodbI64();
			if (t == typeof(SqlMoney))    return new dbMTodbI64();

			if (t == typeof(SqlBoolean))  return new dbBTodbI64();
			if (t == typeof(SqlDateTime)) return new dbDTTodbI64();

			return null;
		}


		private static IValueMapper GetdbR4Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8TodbR4();
			if (t == typeof(Int16))       return new I16TodbR4();
			if (t == typeof(Int32))       return new I32TodbR4();
			if (t == typeof(Int64))       return new I64TodbR4();

			if (t == typeof(Byte))        return new U8TodbR4();
			if (t == typeof(UInt16))      return new U16TodbR4();
			if (t == typeof(UInt32))      return new U32TodbR4();
			if (t == typeof(UInt64))      return new U64TodbR4();

			if (t == typeof(Boolean))     return new BTodbR4();
			if (t == typeof(Char))        return new CTodbR4();
			if (t == typeof(Single))      return new R4TodbR4();
			if (t == typeof(Double))      return new R8TodbR4();
			if (t == typeof(Decimal))     return new DTodbR4();

#if FW2
			if (t == typeof(SByte?))      return new NI8TodbR4();
			if (t == typeof(Int16?))      return new NI16TodbR4();
			if (t == typeof(Int32?))      return new NI32TodbR4();
			if (t == typeof(Int64?))      return new NI64TodbR4();

			if (t == typeof(Byte?))       return new NU8TodbR4();
			if (t == typeof(UInt16?))     return new NU16TodbR4();
			if (t == typeof(UInt32?))     return new NU32TodbR4();
			if (t == typeof(UInt64?))     return new NU64TodbR4();

			if (t == typeof(Boolean?))    return new NBTodbR4();
			if (t == typeof(Char?))       return new NCTodbR4();
			if (t == typeof(Single?))     return new NR4TodbR4();
			if (t == typeof(Double?))     return new NR8TodbR4();
			if (t == typeof(Decimal?))    return new NDTodbR4();

#endif
			if (t == typeof(SqlString))   return new dbSTodbR4();

			if (t == typeof(SqlByte))     return new dbU8TodbR4();
			if (t == typeof(SqlInt16))    return new dbI16TodbR4();
			if (t == typeof(SqlInt32))    return new dbI32TodbR4();
			if (t == typeof(SqlInt64))    return new dbI64TodbR4();

			if (t == typeof(SqlSingle))   return new dbR4TodbR4();
			if (t == typeof(SqlDouble))   return new dbR8TodbR4();
			if (t == typeof(SqlDecimal))  return new dbDTodbR4();
			if (t == typeof(SqlMoney))    return new dbMTodbR4();

			if (t == typeof(SqlBoolean))  return new dbBTodbR4();

			return null;
		}

		private static IValueMapper GetdbR8Mapper(Type t)
		{
			if (t == typeof(SByte))       return new I8TodbR8();
			if (t == typeof(Int16))       return new I16TodbR8();
			if (t == typeof(Int32))       return new I32TodbR8();
			if (t == typeof(Int64))       return new I64TodbR8();

			if (t == typeof(Byte))        return new U8TodbR8();
			if (t == typeof(UInt16))      return new U16TodbR8();
			if (t == typeof(UInt32))      return new U32TodbR8();
			if (t == typeof(UInt64))      return new U64TodbR8();

			if (t == typeof(Boolean))     return new BTodbR8();
			if (t == typeof(Char))        return new CTodbR8();
			if (t == typeof(Single))      return new R4TodbR8();
			if (t == typeof(Double))      return new R8TodbR8();
			if (t == typeof(Decimal))     return new DTodbR8();

#if FW2
			if (t == typeof(SByte?))      return new NI8TodbR8();
			if (t == typeof(Int16?))      return new NI16TodbR8();
			if (t == typeof(Int32?))      return new NI32TodbR8();
			if (t == typeof(Int64?))      return new NI64TodbR8();

			if (t == typeof(Byte?))       return new NU8TodbR8();
			if (t == typeof(UInt16?))     return new NU16TodbR8();
			if (t == typeof(UInt32?))     return new NU32TodbR8();
			if (t == typeof(UInt64?))     return new NU64TodbR8();

			if (t == typeof(Boolean?))    return new NBTodbR8();
			if (t == typeof(Char?))       return new NCTodbR8();
			if (t == typeof(Single?))     return new NR4TodbR8();
			if (t == typeof(Double?))     return new NR8TodbR8();
			if (t == typeof(Decimal?))    return new NDTodbR8();

#endif
			if (t == typeof(SqlString))   return new dbSTodbR8();

			if (t == typeof(SqlByte))     return new dbU8TodbR8();
			if (t == typeof(SqlInt16))    return new dbI16TodbR8();
			if (t == typeof(SqlInt32))    return new dbI32TodbR8();
			if (t == typeof(SqlInt64))    return new dbI64TodbR8();

			if (t == typeof(SqlSingle))   return new dbR4TodbR8();
			if (t == typeof(SqlDouble))   return new dbR8TodbR8();
			if (t == typeof(SqlDecimal))  return new dbDTodbR8();
			if (t == typeof(SqlMoney))    return new dbMTodbR8();

			if (t == typeof(SqlBoolean))  return new dbBTodbR8();
			if (t == typeof(SqlDateTime)) return new dbDTTodbR8();

			return null;
		}

		private static IValueMapper GetdbDMapper(Type t)
		{
			if (t == typeof(SByte))       return new I8TodbD();
			if (t == typeof(Int16))       return new I16TodbD();
			if (t == typeof(Int32))       return new I32TodbD();
			if (t == typeof(Int64))       return new I64TodbD();

			if (t == typeof(Byte))        return new U8TodbD();
			if (t == typeof(UInt16))      return new U16TodbD();
			if (t == typeof(UInt32))      return new U32TodbD();
			if (t == typeof(UInt64))      return new U64TodbD();

			if (t == typeof(Boolean))     return new BTodbD();
			if (t == typeof(Char))        return new CTodbD();
			if (t == typeof(Single))      return new R4TodbD();
			if (t == typeof(Double))      return new R8TodbD();
			if (t == typeof(Decimal))     return new DTodbD();

#if FW2
			if (t == typeof(SByte?))      return new NI8TodbD();
			if (t == typeof(Int16?))      return new NI16TodbD();
			if (t == typeof(Int32?))      return new NI32TodbD();
			if (t == typeof(Int64?))      return new NI64TodbD();

			if (t == typeof(Byte?))       return new NU8TodbD();
			if (t == typeof(UInt16?))     return new NU16TodbD();
			if (t == typeof(UInt32?))     return new NU32TodbD();
			if (t == typeof(UInt64?))     return new NU64TodbD();

			if (t == typeof(Boolean?))    return new NBTodbD();
			if (t == typeof(Char?))       return new NCTodbD();
			if (t == typeof(Single?))     return new NR4TodbD();
			if (t == typeof(Double?))     return new NR8TodbD();
			if (t == typeof(Decimal?))    return new NDTodbD();

#endif
			if (t == typeof(SqlString))   return new dbSTodbD();

			if (t == typeof(SqlByte))     return new dbU8TodbD();
			if (t == typeof(SqlInt16))    return new dbI16TodbD();
			if (t == typeof(SqlInt32))    return new dbI32TodbD();
			if (t == typeof(SqlInt64))    return new dbI64TodbD();

			if (t == typeof(SqlSingle))   return new dbR4TodbD();
			if (t == typeof(SqlDouble))   return new dbR8TodbD();
			if (t == typeof(SqlDecimal))  return new dbDTodbD();
			if (t == typeof(SqlMoney))    return new dbMTodbD();

			if (t == typeof(SqlBoolean))  return new dbBTodbD();

			return null;
		}

		private static IValueMapper GetdbMMapper(Type t)
		{
			if (t == typeof(SByte))       return new I8TodbM();
			if (t == typeof(Int16))       return new I16TodbM();
			if (t == typeof(Int32))       return new I32TodbM();
			if (t == typeof(Int64))       return new I64TodbM();

			if (t == typeof(Byte))        return new U8TodbM();
			if (t == typeof(UInt16))      return new U16TodbM();
			if (t == typeof(UInt32))      return new U32TodbM();
			if (t == typeof(UInt64))      return new U64TodbM();

			if (t == typeof(Boolean))     return new BTodbM();
			if (t == typeof(Char))        return new CTodbM();
			if (t == typeof(Single))      return new R4TodbM();
			if (t == typeof(Double))      return new R8TodbM();
			if (t == typeof(Decimal))     return new DTodbM();

#if FW2
			if (t == typeof(SByte?))      return new NI8TodbM();
			if (t == typeof(Int16?))      return new NI16TodbM();
			if (t == typeof(Int32?))      return new NI32TodbM();
			if (t == typeof(Int64?))      return new NI64TodbM();

			if (t == typeof(Byte?))       return new NU8TodbM();
			if (t == typeof(UInt16?))     return new NU16TodbM();
			if (t == typeof(UInt32?))     return new NU32TodbM();
			if (t == typeof(UInt64?))     return new NU64TodbM();

			if (t == typeof(Boolean?))    return new NBTodbM();
			if (t == typeof(Char?))       return new NCTodbM();
			if (t == typeof(Single?))     return new NR4TodbM();
			if (t == typeof(Double?))     return new NR8TodbM();
			if (t == typeof(Decimal?))    return new NDTodbM();

#endif
			if (t == typeof(SqlString))   return new dbSTodbM();

			if (t == typeof(SqlByte))     return new dbU8TodbM();
			if (t == typeof(SqlInt16))    return new dbI16TodbM();
			if (t == typeof(SqlInt32))    return new dbI32TodbM();
			if (t == typeof(SqlInt64))    return new dbI64TodbM();

			if (t == typeof(SqlSingle))   return new dbR4TodbM();
			if (t == typeof(SqlDouble))   return new dbR8TodbM();
			if (t == typeof(SqlDecimal))  return new dbDTodbM();
			if (t == typeof(SqlMoney))    return new dbMTodbM();

			if (t == typeof(SqlBoolean))  return new dbBTodbM();

			return null;
		}


		private static IValueMapper GetdbBMapper(Type t)
		{
			if (t == typeof(SByte))       return new I8TodbB();
			if (t == typeof(Int16))       return new I16TodbB();
			if (t == typeof(Int32))       return new I32TodbB();
			if (t == typeof(Int64))       return new I64TodbB();

			if (t == typeof(Byte))        return new U8TodbB();
			if (t == typeof(UInt16))      return new U16TodbB();
			if (t == typeof(UInt32))      return new U32TodbB();
			if (t == typeof(UInt64))      return new U64TodbB();

			if (t == typeof(Boolean))     return new BTodbB();
			if (t == typeof(Char))        return new CTodbB();
			if (t == typeof(Single))      return new R4TodbB();
			if (t == typeof(Double))      return new R8TodbB();
			if (t == typeof(Decimal))     return new DTodbB();

#if FW2
			if (t == typeof(SByte?))      return new NI8TodbB();
			if (t == typeof(Int16?))      return new NI16TodbB();
			if (t == typeof(Int32?))      return new NI32TodbB();
			if (t == typeof(Int64?))      return new NI64TodbB();

			if (t == typeof(Byte?))       return new NU8TodbB();
			if (t == typeof(UInt16?))     return new NU16TodbB();
			if (t == typeof(UInt32?))     return new NU32TodbB();
			if (t == typeof(UInt64?))     return new NU64TodbB();

			if (t == typeof(Boolean?))    return new NBTodbB();
			if (t == typeof(Char?))       return new NCTodbB();
			if (t == typeof(Single?))     return new NR4TodbB();
			if (t == typeof(Double?))     return new NR8TodbB();
			if (t == typeof(Decimal?))    return new NDTodbB();

#endif
			if (t == typeof(SqlString))   return new dbSTodbB();

			if (t == typeof(SqlByte))     return new dbU8TodbB();
			if (t == typeof(SqlInt16))    return new dbI16TodbB();
			if (t == typeof(SqlInt32))    return new dbI32TodbB();
			if (t == typeof(SqlInt64))    return new dbI64TodbB();

			if (t == typeof(SqlSingle))   return new dbR4TodbB();
			if (t == typeof(SqlDouble))   return new dbR8TodbB();
			if (t == typeof(SqlDecimal))  return new dbDTodbB();
			if (t == typeof(SqlMoney))    return new dbMTodbB();

			if (t == typeof(SqlBoolean))  return new dbBTodbB();

			return null;
		}

		private static IValueMapper GetdbGMapper(Type t)
		{
			if (t == typeof(Guid))     return new GTodbG();

#if FW2
			if (t == typeof(Guid?))    return new NGTodbG();

#endif
			if (t == typeof(SqlString))   return new dbSTodbG();
			if (t == typeof(SqlGuid))     return new dbGTodbG();

			return null;
		}

		private static IValueMapper GetdbDTMapper(Type t)
		{
			if (t == typeof(Int64))       return new I64TodbDT();
			if (t == typeof(Double))      return new R8TodbDT();

#if FW2
			if (t == typeof(Int64?))      return new NI64TodbDT();
			if (t == typeof(Double?))     return new NR8TodbDT();

#endif
			if (t == typeof(SqlString))   return new dbSTodbDT();
			if (t == typeof(SqlInt64))    return new dbI64TodbDT();
			if (t == typeof(SqlDouble))   return new dbR8TodbDT();
			if (t == typeof(SqlDateTime)) return new dbDTTodbDT();

			return null;
		}

		#endregion 
	}
}