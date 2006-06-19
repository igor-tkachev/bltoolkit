using System;
using System.Data.SqlTypes;

namespace BLToolkit.Common
{
	public static class Operator<T>
	{
		public delegate T    TwoArgOperation (T op1, T op2);
		public delegate bool BoolRetOperation(T op1, T op2);
		public delegate T    OneArgOperation (T op);

		#region (a + b)  Addition

		public  static TwoArgOperation Addition = GetAdditionOperator();
		private static TwoArgOperation GetAdditionOperator()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(String))     return (TwoArgOperation)(object)(Operator<String>.    TwoArgOperation)(delegate(String     a, String     b) { return           a + b;  });

			if (t == typeof(SByte))      return (TwoArgOperation)(object)(Operator<SByte>.     TwoArgOperation)(delegate(SByte      a, SByte      b) { return (SByte)  (a + b); });
			if (t == typeof(Int16))      return (TwoArgOperation)(object)(Operator<Int16>.     TwoArgOperation)(delegate(Int16      a, Int16      b) { return (Int16)  (a + b); });
			if (t == typeof(Int32))      return (TwoArgOperation)(object)(Operator<Int32>.     TwoArgOperation)(delegate(Int32      a, Int32      b) { return           a + b;  });
			if (t == typeof(Int64))      return (TwoArgOperation)(object)(Operator<Int64>.     TwoArgOperation)(delegate(Int64      a, Int64      b) { return           a + b;  });

			if (t == typeof(Byte))       return (TwoArgOperation)(object)(Operator<Byte>.      TwoArgOperation)(delegate(Byte       a, Byte       b) { return (Byte)   (a + b); });
			if (t == typeof(UInt16))     return (TwoArgOperation)(object)(Operator<UInt16>.    TwoArgOperation)(delegate(UInt16     a, UInt16     b) { return (UInt16) (a + b); });
			if (t == typeof(UInt32))     return (TwoArgOperation)(object)(Operator<UInt32>.    TwoArgOperation)(delegate(UInt32     a, UInt32     b) { return           a + b;  });
			if (t == typeof(UInt64))     return (TwoArgOperation)(object)(Operator<UInt64>.    TwoArgOperation)(delegate(UInt64     a, UInt64     b) { return           a + b;  });

			if (t == typeof(Char))       return (TwoArgOperation)(object)(Operator<Char>.      TwoArgOperation)(delegate(Char       a, Char       b) { return (Char)   (a + b); });
			if (t == typeof(Single))     return (TwoArgOperation)(object)(Operator<Single>.    TwoArgOperation)(delegate(Single     a, Single     b) { return           a + b;  });
			if (t == typeof(Double))     return (TwoArgOperation)(object)(Operator<Double>.    TwoArgOperation)(delegate(Double     a, Double     b) { return           a + b;  });

			if (t == typeof(Decimal))    return (TwoArgOperation)(object)(Operator<Decimal>.   TwoArgOperation)(delegate(Decimal    a, Decimal    b) { return           a + b;  });
			if (t == typeof(TimeSpan))   return (TwoArgOperation)(object)(Operator<TimeSpan>.  TwoArgOperation)(delegate(TimeSpan   a, TimeSpan   b) { return           a + b;  });

			// Nullable types.
			//
			if (t == typeof(SByte?))     return (TwoArgOperation)(object)(Operator<SByte?>.    TwoArgOperation)(delegate(SByte?     a, SByte?     b) { return (SByte?) (a + b); });
			if (t == typeof(Int16?))     return (TwoArgOperation)(object)(Operator<Int16?>.    TwoArgOperation)(delegate(Int16?     a, Int16?     b) { return (Int16?) (a + b); });
			if (t == typeof(Int32?))     return (TwoArgOperation)(object)(Operator<Int32?>.    TwoArgOperation)(delegate(Int32?     a, Int32?     b) { return           a + b;  });
			if (t == typeof(Int64?))     return (TwoArgOperation)(object)(Operator<Int64?>.    TwoArgOperation)(delegate(Int64?     a, Int64?     b) { return           a + b;  });

			if (t == typeof(Byte?))      return (TwoArgOperation)(object)(Operator<Byte?>.     TwoArgOperation)(delegate(Byte?      a, Byte?      b) { return (Byte?)  (a + b); });
			if (t == typeof(UInt16?))    return (TwoArgOperation)(object)(Operator<UInt16?>.   TwoArgOperation)(delegate(UInt16?    a, UInt16?    b) { return (UInt16?)(a + b); });
			if (t == typeof(UInt32?))    return (TwoArgOperation)(object)(Operator<UInt32?>.   TwoArgOperation)(delegate(UInt32?    a, UInt32?    b) { return           a + b;  });
			if (t == typeof(UInt64?))    return (TwoArgOperation)(object)(Operator<UInt64?>.   TwoArgOperation)(delegate(UInt64?    a, UInt64?    b) { return           a + b;  });

			if (t == typeof(Char?))      return (TwoArgOperation)(object)(Operator<Char?>.     TwoArgOperation)(delegate(Char?      a, Char?      b) { return (Char?)  (a + b); });
			if (t == typeof(Single?))    return (TwoArgOperation)(object)(Operator<Single?>.   TwoArgOperation)(delegate(Single?    a, Single?    b) { return           a + b;  });
			if (t == typeof(Double?))    return (TwoArgOperation)(object)(Operator<Double?>.   TwoArgOperation)(delegate(Double?    a, Double?    b) { return           a + b;  });

			if (t == typeof(Decimal?))   return (TwoArgOperation)(object)(Operator<Decimal?>.  TwoArgOperation)(delegate(Decimal?   a, Decimal?   b) { return           a + b;  });
			if (t == typeof(TimeSpan?))  return (TwoArgOperation)(object)(Operator<TimeSpan?>. TwoArgOperation)(delegate(TimeSpan?  a, TimeSpan?  b) { return           a + b;  });

			// Sql types.
			//
			if (t == typeof(SqlString))  return (TwoArgOperation)(object)(Operator<SqlString>. TwoArgOperation)(delegate(SqlString  a, SqlString  b) { return           a + b;  });

			if (t == typeof(SqlInt16))   return (TwoArgOperation)(object)(Operator<SqlInt16>.  TwoArgOperation)(delegate(SqlInt16   a, SqlInt16   b) { return           a + b;  });
			if (t == typeof(SqlInt32))   return (TwoArgOperation)(object)(Operator<SqlInt32>.  TwoArgOperation)(delegate(SqlInt32   a, SqlInt32   b) { return           a + b;  });
			if (t == typeof(SqlInt64))   return (TwoArgOperation)(object)(Operator<SqlInt64>.  TwoArgOperation)(delegate(SqlInt64   a, SqlInt64   b) { return           a + b;  });

			if (t == typeof(SqlByte))    return (TwoArgOperation)(object)(Operator<SqlByte>.   TwoArgOperation)(delegate(SqlByte    a, SqlByte    b) { return           a + b;  });

			if (t == typeof(SqlSingle))  return (TwoArgOperation)(object)(Operator<SqlSingle>. TwoArgOperation)(delegate(SqlSingle  a, SqlSingle  b) { return           a + b;  });
			if (t == typeof(SqlDouble))  return (TwoArgOperation)(object)(Operator<SqlDouble>. TwoArgOperation)(delegate(SqlDouble  a, SqlDouble  b) { return           a + b;  });
			if (t == typeof(SqlDecimal)) return (TwoArgOperation)(object)(Operator<SqlDecimal>.TwoArgOperation)(delegate(SqlDecimal a, SqlDecimal b) { return           a + b;  });
			if (t == typeof(SqlMoney))   return (TwoArgOperation)(object)(Operator<SqlMoney>.  TwoArgOperation)(delegate(SqlMoney   a, SqlMoney   b) { return           a + b;  });

			if (t == typeof(SqlBinary))  return (TwoArgOperation)(object)(Operator<SqlBinary>. TwoArgOperation)(delegate(SqlBinary  a, SqlBinary  b) { return           a + b;  });

			return delegate(T a,T b) { throw new InvalidOperationException(); };
		}

		#endregion

		#region (a - b)  Subtraction

		public  static TwoArgOperation Subtraction = GetSubtractionOperator();
		private static TwoArgOperation GetSubtractionOperator()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(SByte))      return (TwoArgOperation)(object)(Operator<SByte>.      TwoArgOperation)(delegate(SByte      a, SByte      b) { return (SByte)  (a - b); });
			if (t == typeof(Int16))      return (TwoArgOperation)(object)(Operator<Int16>.      TwoArgOperation)(delegate(Int16      a, Int16      b) { return (Int16)  (a - b); });
			if (t == typeof(Int32))      return (TwoArgOperation)(object)(Operator<Int32>.      TwoArgOperation)(delegate(Int32      a, Int32      b) { return           a - b;  });
			if (t == typeof(Int64))      return (TwoArgOperation)(object)(Operator<Int64>.      TwoArgOperation)(delegate(Int64      a, Int64      b) { return           a - b;  });

			if (t == typeof(Byte))       return (TwoArgOperation)(object)(Operator<Byte>.       TwoArgOperation)(delegate(Byte       a, Byte       b) { return (Byte)   (a - b); });
			if (t == typeof(UInt16))     return (TwoArgOperation)(object)(Operator<UInt16>.     TwoArgOperation)(delegate(UInt16     a, UInt16     b) { return (UInt16) (a - b); });
			if (t == typeof(UInt32))     return (TwoArgOperation)(object)(Operator<UInt32>.     TwoArgOperation)(delegate(UInt32     a, UInt32     b) { return           a - b;  });
			if (t == typeof(UInt64))     return (TwoArgOperation)(object)(Operator<UInt64>.     TwoArgOperation)(delegate(UInt64     a, UInt64     b) { return           a - b;  });

			if (t == typeof(Char))       return (TwoArgOperation)(object)(Operator<Char>.       TwoArgOperation)(delegate(Char       a, Char       b) { return (Char)   (a - b); });
			if (t == typeof(Single))     return (TwoArgOperation)(object)(Operator<Single>.     TwoArgOperation)(delegate(Single     a, Single     b) { return           a - b;  });
			if (t == typeof(Double))     return (TwoArgOperation)(object)(Operator<Double>.     TwoArgOperation)(delegate(Double     a, Double     b) { return           a - b;  });

			if (t == typeof(Decimal))    return (TwoArgOperation)(object)(Operator<Decimal>.    TwoArgOperation)(delegate(Decimal    a, Decimal    b) { return           a - b;  });
			if (t == typeof(TimeSpan))   return (TwoArgOperation)(object)(Operator<TimeSpan>.   TwoArgOperation)(delegate(TimeSpan   a, TimeSpan   b) { return           a - b;  });

			// Nullable types.
			//
			if (t == typeof(SByte?))     return (TwoArgOperation)(object)(Operator<SByte?>.     TwoArgOperation)(delegate(SByte?     a, SByte?     b) { return (SByte?) (a - b); });
			if (t == typeof(Int16?))     return (TwoArgOperation)(object)(Operator<Int16?>.     TwoArgOperation)(delegate(Int16?     a, Int16?     b) { return (Int16?) (a - b); });
			if (t == typeof(Int32?))     return (TwoArgOperation)(object)(Operator<Int32?>.     TwoArgOperation)(delegate(Int32?     a, Int32?     b) { return           a - b;  });
			if (t == typeof(Int64?))     return (TwoArgOperation)(object)(Operator<Int64?>.     TwoArgOperation)(delegate(Int64?     a, Int64?     b) { return           a - b;  });

			if (t == typeof(Byte?))      return (TwoArgOperation)(object)(Operator<Byte?>.      TwoArgOperation)(delegate(Byte?      a, Byte?      b) { return (Byte?)  (a - b); });
			if (t == typeof(UInt16?))    return (TwoArgOperation)(object)(Operator<UInt16?>.    TwoArgOperation)(delegate(UInt16?    a, UInt16?    b) { return (UInt16?)(a - b); });
			if (t == typeof(UInt32?))    return (TwoArgOperation)(object)(Operator<UInt32?>.    TwoArgOperation)(delegate(UInt32?    a, UInt32?    b) { return           a - b;  });
			if (t == typeof(UInt64?))    return (TwoArgOperation)(object)(Operator<UInt64?>.    TwoArgOperation)(delegate(UInt64?    a, UInt64?    b) { return           a - b;  });

			if (t == typeof(Char?))      return (TwoArgOperation)(object)(Operator<Char?>.      TwoArgOperation)(delegate(Char?      a, Char?      b) { return (Char?)  (a - b); });
			if (t == typeof(Single?))    return (TwoArgOperation)(object)(Operator<Single?>.    TwoArgOperation)(delegate(Single?    a, Single?    b) { return           a - b;  });
			if (t == typeof(Double?))    return (TwoArgOperation)(object)(Operator<Double?>.    TwoArgOperation)(delegate(Double?    a, Double?    b) { return           a - b;  });

			if (t == typeof(Decimal?))   return (TwoArgOperation)(object)(Operator<Decimal?>.   TwoArgOperation)(delegate(Decimal?   a, Decimal?   b) { return           a - b;  });
			if (t == typeof(TimeSpan?))  return (TwoArgOperation)(object)(Operator<TimeSpan?>.  TwoArgOperation)(delegate(TimeSpan?  a, TimeSpan?  b) { return           a - b;  });

			// Sql types.
			//
			if (t == typeof(SqlInt16))   return (TwoArgOperation)(object)(Operator<SqlInt16>.   TwoArgOperation)(delegate(SqlInt16   a, SqlInt16   b) { return          a - b;  });
			if (t == typeof(SqlInt32))   return (TwoArgOperation)(object)(Operator<SqlInt32>.   TwoArgOperation)(delegate(SqlInt32   a, SqlInt32   b) { return          a - b;  });
			if (t == typeof(SqlInt64))   return (TwoArgOperation)(object)(Operator<SqlInt64>.   TwoArgOperation)(delegate(SqlInt64   a, SqlInt64   b) { return          a - b;  });

			if (t == typeof(SqlByte))    return (TwoArgOperation)(object)(Operator<SqlByte>.    TwoArgOperation)(delegate(SqlByte    a, SqlByte    b) { return          a - b;  });

			if (t == typeof(SqlSingle))  return (TwoArgOperation)(object)(Operator<SqlSingle>.  TwoArgOperation)(delegate(SqlSingle  a, SqlSingle  b) { return          a - b;  });
			if (t == typeof(SqlDouble))  return (TwoArgOperation)(object)(Operator<SqlDouble>.  TwoArgOperation)(delegate(SqlDouble  a, SqlDouble  b) { return          a - b;  });
			if (t == typeof(SqlDecimal)) return (TwoArgOperation)(object)(Operator<SqlDecimal>. TwoArgOperation)(delegate(SqlDecimal a, SqlDecimal b) { return          a - b;  });
			if (t == typeof(SqlMoney))   return (TwoArgOperation)(object)(Operator<SqlMoney>.   TwoArgOperation)(delegate(SqlMoney   a, SqlMoney   b) { return          a - b;  });

			return delegate(T a,T b) { throw new InvalidOperationException(); };
		}

		#endregion

		#region (a * b)  Multiply

		public  static TwoArgOperation Multiply = GetMultiplyOperator();
		private static TwoArgOperation GetMultiplyOperator()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(SByte))      return (TwoArgOperation)(object)(Operator<SByte>.      TwoArgOperation)     (delegate(SByte      a, SByte      b) { return (SByte)  (a * b); });
			if (t == typeof(Int16))      return (TwoArgOperation)(object)(Operator<Int16>.      TwoArgOperation)     (delegate(Int16      a, Int16      b) { return (Int16)  (a * b); });
			if (t == typeof(Int32))      return (TwoArgOperation)(object)(Operator<Int32>.      TwoArgOperation)     (delegate(Int32      a, Int32      b) { return           a * b;  });
			if (t == typeof(Int64))      return (TwoArgOperation)(object)(Operator<Int64>.      TwoArgOperation)     (delegate(Int64      a, Int64      b) { return           a * b;  });

			if (t == typeof(Byte))       return (TwoArgOperation)(object)(Operator<Byte>.       TwoArgOperation)      (delegate(Byte       a, Byte       b) { return (Byte)   (a * b); });
			if (t == typeof(UInt16))     return (TwoArgOperation)(object)(Operator<UInt16>.     TwoArgOperation)    (delegate(UInt16     a, UInt16     b) { return (UInt16) (a * b); });
			if (t == typeof(UInt32))     return (TwoArgOperation)(object)(Operator<UInt32>.     TwoArgOperation)    (delegate(UInt32     a, UInt32     b) { return           a * b;  });
			if (t == typeof(UInt64))     return (TwoArgOperation)(object)(Operator<UInt64>.     TwoArgOperation)    (delegate(UInt64     a, UInt64     b) { return           a * b;  });

			if (t == typeof(Char))       return (TwoArgOperation)(object)(Operator<Char>.       TwoArgOperation)      (delegate(Char       a, Char       b) { return (Char)   (a * b); });
			if (t == typeof(Single))     return (TwoArgOperation)(object)(Operator<Single>.     TwoArgOperation)    (delegate(Single     a, Single     b) { return           a * b;  });
			if (t == typeof(Double))     return (TwoArgOperation)(object)(Operator<Double>.     TwoArgOperation)    (delegate(Double     a, Double     b) { return           a * b;  });

			if (t == typeof(Decimal))    return (TwoArgOperation)(object)(Operator<Decimal>.    TwoArgOperation)   (delegate(Decimal    a, Decimal    b) { return           a * b;  });

			// Nullable types.
			//
			if (t == typeof(SByte?))     return (TwoArgOperation)(object)(Operator<SByte?>.     TwoArgOperation)    (delegate(SByte?     a, SByte?     b) { return (SByte?) (a * b); });
			if (t == typeof(Int16?))     return (TwoArgOperation)(object)(Operator<Int16?>.     TwoArgOperation)    (delegate(Int16?     a, Int16?     b) { return (Int16?) (a * b); });
			if (t == typeof(Int32?))     return (TwoArgOperation)(object)(Operator<Int32?>.     TwoArgOperation)    (delegate(Int32?     a, Int32?     b) { return           a * b;  });
			if (t == typeof(Int64?))     return (TwoArgOperation)(object)(Operator<Int64?>.     TwoArgOperation)    (delegate(Int64?     a, Int64?     b) { return           a * b;  });

			if (t == typeof(Byte?))      return (TwoArgOperation)(object)(Operator<Byte?>.      TwoArgOperation)     (delegate(Byte?      a, Byte?      b) { return (Byte?)  (a * b); });
			if (t == typeof(UInt16?))    return (TwoArgOperation)(object)(Operator<UInt16?>.    TwoArgOperation)   (delegate(UInt16?    a, UInt16?    b) { return (UInt16?)(a * b); });
			if (t == typeof(UInt32?))    return (TwoArgOperation)(object)(Operator<UInt32?>.    TwoArgOperation)   (delegate(UInt32?    a, UInt32?    b) { return           a * b;  });
			if (t == typeof(UInt64?))    return (TwoArgOperation)(object)(Operator<UInt64?>.    TwoArgOperation)   (delegate(UInt64?    a, UInt64?    b) { return           a * b;  });

			if (t == typeof(Char?))      return (TwoArgOperation)(object)(Operator<Char?>.      TwoArgOperation)     (delegate(Char?      a, Char?      b) { return (Char?)  (a * b); });
			if (t == typeof(Single?))    return (TwoArgOperation)(object)(Operator<Single?>.    TwoArgOperation)   (delegate(Single?    a, Single?    b) { return           a * b;  });
			if (t == typeof(Double?))    return (TwoArgOperation)(object)(Operator<Double?>.    TwoArgOperation)   (delegate(Double?    a, Double?    b) { return           a * b;  });

			if (t == typeof(Decimal?))   return (TwoArgOperation)(object)(Operator<Decimal?>.   TwoArgOperation)  (delegate(Decimal?   a, Decimal?   b) { return           a * b;  });

			// Sql types.
			//
			if (t == typeof(SqlInt16))   return (TwoArgOperation)(object)(Operator<SqlInt16>.   TwoArgOperation)  (delegate(SqlInt16   a, SqlInt16   b) { return           a * b;  });
			if (t == typeof(SqlInt32))   return (TwoArgOperation)(object)(Operator<SqlInt32>.   TwoArgOperation)  (delegate(SqlInt32   a, SqlInt32   b) { return           a * b;  });
			if (t == typeof(SqlInt64))   return (TwoArgOperation)(object)(Operator<SqlInt64>.   TwoArgOperation)  (delegate(SqlInt64   a, SqlInt64   b) { return           a * b;  });

			if (t == typeof(SqlByte))    return (TwoArgOperation)(object)(Operator<SqlByte>.    TwoArgOperation)   (delegate(SqlByte    a, SqlByte    b) { return           a * b;  });

			if (t == typeof(SqlSingle))  return (TwoArgOperation)(object)(Operator<SqlSingle>.  TwoArgOperation) (delegate(SqlSingle  a, SqlSingle  b) { return           a * b;  });
			if (t == typeof(SqlDouble))  return (TwoArgOperation)(object)(Operator<SqlDouble>.  TwoArgOperation) (delegate(SqlDouble  a, SqlDouble  b) { return           a * b;  });
			if (t == typeof(SqlDecimal)) return (TwoArgOperation)(object)(Operator<SqlDecimal>. TwoArgOperation)(delegate(SqlDecimal a, SqlDecimal b) { return           a * b;  });
			if (t == typeof(SqlMoney))   return (TwoArgOperation)(object)(Operator<SqlMoney>.   TwoArgOperation)  (delegate(SqlMoney   a, SqlMoney   b) { return           a * b;  });

			return delegate(T a,T b) { throw new InvalidOperationException(); };
		}

		#endregion

		#region (a / b)  Division

		public  static TwoArgOperation Division = GetDivisionOperator();
		private static TwoArgOperation GetDivisionOperator()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(SByte))      return (TwoArgOperation)(object)(Operator<SByte>.      TwoArgOperation)     (delegate(SByte      a, SByte      b) { return (SByte)  (a / b); });
			if (t == typeof(Int16))      return (TwoArgOperation)(object)(Operator<Int16>.      TwoArgOperation)     (delegate(Int16      a, Int16      b) { return (Int16)  (a / b); });
			if (t == typeof(Int32))      return (TwoArgOperation)(object)(Operator<Int32>.      TwoArgOperation)     (delegate(Int32      a, Int32      b) { return           a / b;  });
			if (t == typeof(Int64))      return (TwoArgOperation)(object)(Operator<Int64>.      TwoArgOperation)     (delegate(Int64      a, Int64      b) { return           a / b;  });

			if (t == typeof(Byte))       return (TwoArgOperation)(object)(Operator<Byte>.       TwoArgOperation)      (delegate(Byte       a, Byte       b) { return (Byte)   (a / b); });
			if (t == typeof(UInt16))     return (TwoArgOperation)(object)(Operator<UInt16>.     TwoArgOperation)    (delegate(UInt16     a, UInt16     b) { return (UInt16) (a / b); });
			if (t == typeof(UInt32))     return (TwoArgOperation)(object)(Operator<UInt32>.     TwoArgOperation)    (delegate(UInt32     a, UInt32     b) { return           a / b;  });
			if (t == typeof(UInt64))     return (TwoArgOperation)(object)(Operator<UInt64>.     TwoArgOperation)    (delegate(UInt64     a, UInt64     b) { return           a / b;  });

			if (t == typeof(Char))       return (TwoArgOperation)(object)(Operator<Char>.       TwoArgOperation)      (delegate(Char       a, Char       b) { return (Char)   (a / b); });
			if (t == typeof(Single))     return (TwoArgOperation)(object)(Operator<Single>.     TwoArgOperation)    (delegate(Single     a, Single     b) { return           a / b;  });
			if (t == typeof(Double))     return (TwoArgOperation)(object)(Operator<Double>.     TwoArgOperation)    (delegate(Double     a, Double     b) { return           a / b;  });

			if (t == typeof(Decimal))    return (TwoArgOperation)(object)(Operator<Decimal>.    TwoArgOperation)   (delegate(Decimal    a, Decimal    b) { return           a / b;  });

			// Nullable types.
			//
			if (t == typeof(SByte?))     return (TwoArgOperation)(object)(Operator<SByte?>.     TwoArgOperation)    (delegate(SByte?     a, SByte?     b) { return (SByte?) (a / b); });
			if (t == typeof(Int16?))     return (TwoArgOperation)(object)(Operator<Int16?>.     TwoArgOperation)    (delegate(Int16?     a, Int16?     b) { return (Int16?) (a / b); });
			if (t == typeof(Int32?))     return (TwoArgOperation)(object)(Operator<Int32?>.     TwoArgOperation)    (delegate(Int32?     a, Int32?     b) { return           a / b;  });
			if (t == typeof(Int64?))     return (TwoArgOperation)(object)(Operator<Int64?>.     TwoArgOperation)    (delegate(Int64?     a, Int64?     b) { return           a / b;  });

			if (t == typeof(Byte?))      return (TwoArgOperation)(object)(Operator<Byte?>.      TwoArgOperation)     (delegate(Byte?      a, Byte?      b) { return (Byte?)  (a / b); });
			if (t == typeof(UInt16?))    return (TwoArgOperation)(object)(Operator<UInt16?>.    TwoArgOperation)   (delegate(UInt16?    a, UInt16?    b) { return (UInt16?)(a / b); });
			if (t == typeof(UInt32?))    return (TwoArgOperation)(object)(Operator<UInt32?>.    TwoArgOperation)   (delegate(UInt32?    a, UInt32?    b) { return           a / b;  });
			if (t == typeof(UInt64?))    return (TwoArgOperation)(object)(Operator<UInt64?>.    TwoArgOperation)   (delegate(UInt64?    a, UInt64?    b) { return           a / b;  });

			if (t == typeof(Char?))      return (TwoArgOperation)(object)(Operator<Char?>.      TwoArgOperation)     (delegate(Char?      a, Char?      b) { return (Char?)  (a / b); });
			if (t == typeof(Single?))    return (TwoArgOperation)(object)(Operator<Single?>.    TwoArgOperation)   (delegate(Single?    a, Single?    b) { return           a / b;  });
			if (t == typeof(Double?))    return (TwoArgOperation)(object)(Operator<Double?>.    TwoArgOperation)   (delegate(Double?    a, Double?    b) { return           a / b;  });

			if (t == typeof(Decimal?))   return (TwoArgOperation)(object)(Operator<Decimal?>.   TwoArgOperation)  (delegate(Decimal?   a, Decimal?   b) { return           a / b;  });

			// Sql types.
			//
			if (t == typeof(SqlInt16))   return (TwoArgOperation)(object)(Operator<SqlInt16>.   TwoArgOperation)  (delegate(SqlInt16   a, SqlInt16   b) { return           a / b;  });
			if (t == typeof(SqlInt32))   return (TwoArgOperation)(object)(Operator<SqlInt32>.   TwoArgOperation)  (delegate(SqlInt32   a, SqlInt32   b) { return           a / b;  });
			if (t == typeof(SqlInt64))   return (TwoArgOperation)(object)(Operator<SqlInt64>.   TwoArgOperation)  (delegate(SqlInt64   a, SqlInt64   b) { return           a / b;  });

			if (t == typeof(SqlByte))    return (TwoArgOperation)(object)(Operator<SqlByte>.    TwoArgOperation)   (delegate(SqlByte    a, SqlByte    b) { return           a / b;  });

			if (t == typeof(SqlSingle))  return (TwoArgOperation)(object)(Operator<SqlSingle>.  TwoArgOperation) (delegate(SqlSingle  a, SqlSingle  b) { return           a / b;  });
			if (t == typeof(SqlDouble))  return (TwoArgOperation)(object)(Operator<SqlDouble>.  TwoArgOperation) (delegate(SqlDouble  a, SqlDouble  b) { return           a / b;  });
			if (t == typeof(SqlDecimal)) return (TwoArgOperation)(object)(Operator<SqlDecimal>. TwoArgOperation)(delegate(SqlDecimal a, SqlDecimal b) { return           a / b;  });
			if (t == typeof(SqlMoney))   return (TwoArgOperation)(object)(Operator<SqlMoney>.   TwoArgOperation)  (delegate(SqlMoney   a, SqlMoney   b) { return           a / b;  });

			return delegate(T a,T b) { throw new InvalidOperationException(); };
		}

		#endregion

		#region (a % b)  Modulus

		public  static TwoArgOperation Modulus = GetModulusOperator();
		private static TwoArgOperation GetModulusOperator()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(SByte))      return (TwoArgOperation)(object)(Operator<SByte>.      TwoArgOperation)     (delegate(SByte      a, SByte      b) { return (SByte)  (a % b); });
			if (t == typeof(Int16))      return (TwoArgOperation)(object)(Operator<Int16>.      TwoArgOperation)     (delegate(Int16      a, Int16      b) { return (Int16)  (a % b); });
			if (t == typeof(Int32))      return (TwoArgOperation)(object)(Operator<Int32>.      TwoArgOperation)     (delegate(Int32      a, Int32      b) { return           a % b;  });
			if (t == typeof(Int64))      return (TwoArgOperation)(object)(Operator<Int64>.      TwoArgOperation)     (delegate(Int64      a, Int64      b) { return           a % b;  });

			if (t == typeof(Byte))       return (TwoArgOperation)(object)(Operator<Byte>.       TwoArgOperation)      (delegate(Byte       a, Byte       b) { return (Byte)   (a % b); });
			if (t == typeof(UInt16))     return (TwoArgOperation)(object)(Operator<UInt16>.     TwoArgOperation)    (delegate(UInt16     a, UInt16     b) { return (UInt16) (a % b); });
			if (t == typeof(UInt32))     return (TwoArgOperation)(object)(Operator<UInt32>.     TwoArgOperation)    (delegate(UInt32     a, UInt32     b) { return           a % b;  });
			if (t == typeof(UInt64))     return (TwoArgOperation)(object)(Operator<UInt64>.     TwoArgOperation)    (delegate(UInt64     a, UInt64     b) { return           a % b;  });

			if (t == typeof(Char))       return (TwoArgOperation)(object)(Operator<Char>.       TwoArgOperation)      (delegate(Char       a, Char       b) { return (Char)   (a % b); });
			if (t == typeof(Single))     return (TwoArgOperation)(object)(Operator<Single>.     TwoArgOperation)    (delegate(Single     a, Single     b) { return           a % b;  });
			if (t == typeof(Double))     return (TwoArgOperation)(object)(Operator<Double>.     TwoArgOperation)    (delegate(Double     a, Double     b) { return           a % b;  });

			if (t == typeof(Decimal))    return (TwoArgOperation)(object)(Operator<Decimal>.    TwoArgOperation)   (delegate(Decimal    a, Decimal    b) { return           a % b;  });

			// Nullable types.
			//
			if (t == typeof(SByte?))     return (TwoArgOperation)(object)(Operator<SByte?>.     TwoArgOperation)    (delegate(SByte?     a, SByte?     b) { return (SByte?) (a % b); });
			if (t == typeof(Int16?))     return (TwoArgOperation)(object)(Operator<Int16?>.     TwoArgOperation)    (delegate(Int16?     a, Int16?     b) { return (Int16?) (a % b); });
			if (t == typeof(Int32?))     return (TwoArgOperation)(object)(Operator<Int32?>.     TwoArgOperation)    (delegate(Int32?     a, Int32?     b) { return           a % b;  });
			if (t == typeof(Int64?))     return (TwoArgOperation)(object)(Operator<Int64?>.     TwoArgOperation)    (delegate(Int64?     a, Int64?     b) { return           a % b;  });

			if (t == typeof(Byte?))      return (TwoArgOperation)(object)(Operator<Byte?>.      TwoArgOperation)     (delegate(Byte?      a, Byte?      b) { return (Byte?)  (a % b); });
			if (t == typeof(UInt16?))    return (TwoArgOperation)(object)(Operator<UInt16?>.    TwoArgOperation)   (delegate(UInt16?    a, UInt16?    b) { return (UInt16?)(a % b); });
			if (t == typeof(UInt32?))    return (TwoArgOperation)(object)(Operator<UInt32?>.    TwoArgOperation)   (delegate(UInt32?    a, UInt32?    b) { return           a % b;  });
			if (t == typeof(UInt64?))    return (TwoArgOperation)(object)(Operator<UInt64?>.    TwoArgOperation)   (delegate(UInt64?    a, UInt64?    b) { return           a % b;  });

			if (t == typeof(Char?))      return (TwoArgOperation)(object)(Operator<Char?>.      TwoArgOperation)     (delegate(Char?      a, Char?      b) { return (Char?)  (a % b); });
			if (t == typeof(Single?))    return (TwoArgOperation)(object)(Operator<Single?>.    TwoArgOperation)   (delegate(Single?    a, Single?    b) { return           a % b;  });
			if (t == typeof(Double?))    return (TwoArgOperation)(object)(Operator<Double?>.    TwoArgOperation)   (delegate(Double?    a, Double?    b) { return           a % b;  });

			if (t == typeof(Decimal?))   return (TwoArgOperation)(object)(Operator<Decimal?>.   TwoArgOperation)  (delegate(Decimal?   a, Decimal?   b) { return           a % b;  });

			// Sql types.
			//
			if (t == typeof(SqlInt16))   return (TwoArgOperation)(object)(Operator<SqlInt16>.   TwoArgOperation)  (delegate(SqlInt16   a, SqlInt16   b) { return           a % b;  });
			if (t == typeof(SqlInt32))   return (TwoArgOperation)(object)(Operator<SqlInt32>.   TwoArgOperation)  (delegate(SqlInt32   a, SqlInt32   b) { return           a % b;  });
			if (t == typeof(SqlInt64))   return (TwoArgOperation)(object)(Operator<SqlInt64>.   TwoArgOperation)  (delegate(SqlInt64   a, SqlInt64   b) { return           a % b;  });

			if (t == typeof(SqlByte))    return (TwoArgOperation)(object)(Operator<SqlByte>.    TwoArgOperation)   (delegate(SqlByte    a, SqlByte    b) { return           a % b;  });

			return delegate(T a,T b) { throw new InvalidOperationException(); };
		}

		#endregion

		#region (a & b)  BitwiseAnd

		public  static TwoArgOperation BitwiseAnd = GetBitwiseAndOperator();
		private static TwoArgOperation GetBitwiseAndOperator()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(SByte))      return (TwoArgOperation)(object)(Operator<SByte>.      TwoArgOperation)     (delegate(SByte      a, SByte      b) { return (SByte)  (a & b); });
			if (t == typeof(Int16))      return (TwoArgOperation)(object)(Operator<Int16>.      TwoArgOperation)     (delegate(Int16      a, Int16      b) { return (Int16)  (a & b); });
			if (t == typeof(Int32))      return (TwoArgOperation)(object)(Operator<Int32>.      TwoArgOperation)     (delegate(Int32      a, Int32      b) { return           a & b;  });
			if (t == typeof(Int64))      return (TwoArgOperation)(object)(Operator<Int64>.      TwoArgOperation)     (delegate(Int64      a, Int64      b) { return           a & b;  });

			if (t == typeof(Byte))       return (TwoArgOperation)(object)(Operator<Byte>.       TwoArgOperation)      (delegate(Byte       a, Byte       b) { return (Byte)   (a & b); });
			if (t == typeof(UInt16))     return (TwoArgOperation)(object)(Operator<UInt16>.     TwoArgOperation)    (delegate(UInt16     a, UInt16     b) { return (UInt16) (a & b); });
			if (t == typeof(UInt32))     return (TwoArgOperation)(object)(Operator<UInt32>.     TwoArgOperation)    (delegate(UInt32     a, UInt32     b) { return           a & b;  });
			if (t == typeof(UInt64))     return (TwoArgOperation)(object)(Operator<UInt64>.     TwoArgOperation)    (delegate(UInt64     a, UInt64     b) { return           a & b;  });

			if (t == typeof(Char))       return (TwoArgOperation)(object)(Operator<Char>.       TwoArgOperation)      (delegate(Char       a, Char       b) { return (Char)   (a & b); });

			// Nullable types.
			//
			if (t == typeof(SByte?))     return (TwoArgOperation)(object)(Operator<SByte?>.     TwoArgOperation)    (delegate(SByte?     a, SByte?     b) { return (SByte?) (a & b); });
			if (t == typeof(Int16?))     return (TwoArgOperation)(object)(Operator<Int16?>.     TwoArgOperation)    (delegate(Int16?     a, Int16?     b) { return (Int16?) (a & b); });
			if (t == typeof(Int32?))     return (TwoArgOperation)(object)(Operator<Int32?>.     TwoArgOperation)    (delegate(Int32?     a, Int32?     b) { return           a & b;  });
			if (t == typeof(Int64?))     return (TwoArgOperation)(object)(Operator<Int64?>.     TwoArgOperation)    (delegate(Int64?     a, Int64?     b) { return           a & b;  });

			if (t == typeof(Byte?))      return (TwoArgOperation)(object)(Operator<Byte?>.      TwoArgOperation)     (delegate(Byte?      a, Byte?      b) { return (Byte?)  (a & b); });
			if (t == typeof(UInt16?))    return (TwoArgOperation)(object)(Operator<UInt16?>.    TwoArgOperation)   (delegate(UInt16?    a, UInt16?    b) { return (UInt16?)(a & b); });
			if (t == typeof(UInt32?))    return (TwoArgOperation)(object)(Operator<UInt32?>.    TwoArgOperation)   (delegate(UInt32?    a, UInt32?    b) { return           a & b;  });
			if (t == typeof(UInt64?))    return (TwoArgOperation)(object)(Operator<UInt64?>.    TwoArgOperation)   (delegate(UInt64?    a, UInt64?    b) { return           a & b;  });

			if (t == typeof(Char?))      return (TwoArgOperation)(object)(Operator<Char?>.      TwoArgOperation)     (delegate(Char?      a, Char?      b) { return (Char?)  (a & b); });

			// Sql types.
			//
			if (t == typeof(SqlInt16))   return (TwoArgOperation)(object)(Operator<SqlInt16>.   TwoArgOperation)  (delegate(SqlInt16   a, SqlInt16   b) { return           a & b;  });
			if (t == typeof(SqlInt32))   return (TwoArgOperation)(object)(Operator<SqlInt32>.   TwoArgOperation)  (delegate(SqlInt32   a, SqlInt32   b) { return           a & b;  });
			if (t == typeof(SqlInt64))   return (TwoArgOperation)(object)(Operator<SqlInt64>.   TwoArgOperation)  (delegate(SqlInt64   a, SqlInt64   b) { return           a & b;  });

			if (t == typeof(SqlByte))    return (TwoArgOperation)(object)(Operator<SqlByte>.    TwoArgOperation)   (delegate(SqlByte    a, SqlByte    b) { return           a & b;  });

			if (t == typeof(SqlBoolean)) return (TwoArgOperation)(object)(Operator<SqlBoolean>. TwoArgOperation)(delegate(SqlBoolean a, SqlBoolean b) { return           a & b;  });

			return delegate(T a,T b) { throw new InvalidOperationException(); };
		}

		#endregion

		#region (a | b)  BitwiseOr

		public  static TwoArgOperation BitwiseOr = GetBitwiseOrOperator();
		private static TwoArgOperation GetBitwiseOrOperator()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(SByte))      return (TwoArgOperation)(object)(Operator<SByte>.      TwoArgOperation)     (delegate(SByte      a, SByte      b) { return (SByte)  (a | b); });
			if (t == typeof(Int16))      return (TwoArgOperation)(object)(Operator<Int16>.      TwoArgOperation)     (delegate(Int16      a, Int16      b) { return (Int16)  (a | b); });
			if (t == typeof(Int32))      return (TwoArgOperation)(object)(Operator<Int32>.      TwoArgOperation)     (delegate(Int32      a, Int32      b) { return           a | b;  });
			if (t == typeof(Int64))      return (TwoArgOperation)(object)(Operator<Int64>.      TwoArgOperation)     (delegate(Int64      a, Int64      b) { return           a | b;  });

			if (t == typeof(Byte))       return (TwoArgOperation)(object)(Operator<Byte>.       TwoArgOperation)      (delegate(Byte       a, Byte       b) { return (Byte)   (a | b); });
			if (t == typeof(UInt16))     return (TwoArgOperation)(object)(Operator<UInt16>.     TwoArgOperation)    (delegate(UInt16     a, UInt16     b) { return (UInt16) (a | b); });
			if (t == typeof(UInt32))     return (TwoArgOperation)(object)(Operator<UInt32>.     TwoArgOperation)    (delegate(UInt32     a, UInt32     b) { return           a | b;  });
			if (t == typeof(UInt64))     return (TwoArgOperation)(object)(Operator<UInt64>.     TwoArgOperation)    (delegate(UInt64     a, UInt64     b) { return           a | b;  });

			if (t == typeof(Char))       return (TwoArgOperation)(object)(Operator<Char>.       TwoArgOperation)      (delegate(Char       a, Char       b) { return (Char)   (a | b); });

			// Nullable types.
			//
			if (t == typeof(SByte?))     return (TwoArgOperation)(object)(Operator<SByte?>.     TwoArgOperation)    (delegate(SByte?     a, SByte?     b) { return (SByte?) (a | b); });
			if (t == typeof(Int16?))     return (TwoArgOperation)(object)(Operator<Int16?>.     TwoArgOperation)    (delegate(Int16?     a, Int16?     b) { return (Int16?) (a | b); });
			if (t == typeof(Int32?))     return (TwoArgOperation)(object)(Operator<Int32?>.     TwoArgOperation)    (delegate(Int32?     a, Int32?     b) { return           a | b;  });
			if (t == typeof(Int64?))     return (TwoArgOperation)(object)(Operator<Int64?>.     TwoArgOperation)    (delegate(Int64?     a, Int64?     b) { return           a | b;  });

			if (t == typeof(Byte?))      return (TwoArgOperation)(object)(Operator<Byte?>.      TwoArgOperation)     (delegate(Byte?      a, Byte?      b) { return (Byte?)  (a | b); });
			if (t == typeof(UInt16?))    return (TwoArgOperation)(object)(Operator<UInt16?>.    TwoArgOperation)   (delegate(UInt16?    a, UInt16?    b) { return (UInt16?)(a | b); });
			if (t == typeof(UInt32?))    return (TwoArgOperation)(object)(Operator<UInt32?>.    TwoArgOperation)   (delegate(UInt32?    a, UInt32?    b) { return           a | b;  });
			if (t == typeof(UInt64?))    return (TwoArgOperation)(object)(Operator<UInt64?>.    TwoArgOperation)   (delegate(UInt64?    a, UInt64?    b) { return           a | b;  });

			if (t == typeof(Char?))      return (TwoArgOperation)(object)(Operator<Char?>.      TwoArgOperation)     (delegate(Char?      a, Char?      b) { return (Char?)  (a | b); });

			// Sql types.
			//
			if (t == typeof(SqlInt16))   return (TwoArgOperation)(object)(Operator<SqlInt16>.   TwoArgOperation)  (delegate(SqlInt16   a, SqlInt16   b) { return           a | b;  });
			if (t == typeof(SqlInt32))   return (TwoArgOperation)(object)(Operator<SqlInt32>.   TwoArgOperation)  (delegate(SqlInt32   a, SqlInt32   b) { return           a | b;  });
			if (t == typeof(SqlInt64))   return (TwoArgOperation)(object)(Operator<SqlInt64>.   TwoArgOperation)  (delegate(SqlInt64   a, SqlInt64   b) { return           a | b;  });

			if (t == typeof(SqlByte))    return (TwoArgOperation)(object)(Operator<SqlByte>.    TwoArgOperation)   (delegate(SqlByte    a, SqlByte    b) { return           a | b;  });

			if (t == typeof(SqlBoolean)) return (TwoArgOperation)(object)(Operator<SqlBoolean>. TwoArgOperation)(delegate(SqlBoolean a, SqlBoolean b) { return           a | b;  });

			return delegate(T a,T b) { throw new InvalidOperationException(); };
		}

		#endregion

		#region (a ^ b)  ExclusiveOr

		public  static TwoArgOperation ExclusiveOr = GetExclusiveOrOperator();
		private static TwoArgOperation GetExclusiveOrOperator()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(SByte))      return (TwoArgOperation)(object)(Operator<SByte>.      TwoArgOperation)     (delegate(SByte      a, SByte      b) { return (SByte)  (a ^ b); });
			if (t == typeof(Int16))      return (TwoArgOperation)(object)(Operator<Int16>.      TwoArgOperation)     (delegate(Int16      a, Int16      b) { return (Int16)  (a ^ b); });
			if (t == typeof(Int32))      return (TwoArgOperation)(object)(Operator<Int32>.      TwoArgOperation)     (delegate(Int32      a, Int32      b) { return           a ^ b;  });
			if (t == typeof(Int64))      return (TwoArgOperation)(object)(Operator<Int64>.      TwoArgOperation)     (delegate(Int64      a, Int64      b) { return           a ^ b;  });

			if (t == typeof(Byte))       return (TwoArgOperation)(object)(Operator<Byte>.       TwoArgOperation)      (delegate(Byte       a, Byte       b) { return (Byte)   (a ^ b); });
			if (t == typeof(UInt16))     return (TwoArgOperation)(object)(Operator<UInt16>.     TwoArgOperation)    (delegate(UInt16     a, UInt16     b) { return (UInt16) (a ^ b); });
			if (t == typeof(UInt32))     return (TwoArgOperation)(object)(Operator<UInt32>.     TwoArgOperation)    (delegate(UInt32     a, UInt32     b) { return           a ^ b;  });
			if (t == typeof(UInt64))     return (TwoArgOperation)(object)(Operator<UInt64>.     TwoArgOperation)    (delegate(UInt64     a, UInt64     b) { return           a ^ b;  });

			if (t == typeof(Char))       return (TwoArgOperation)(object)(Operator<Char>.       TwoArgOperation)      (delegate(Char       a, Char       b) { return (Char)   (a ^ b); });

			// Nullable types.
			//
			if (t == typeof(SByte?))     return (TwoArgOperation)(object)(Operator<SByte?>.     TwoArgOperation)    (delegate(SByte?     a, SByte?     b) { return (SByte?) (a ^ b); });
			if (t == typeof(Int16?))     return (TwoArgOperation)(object)(Operator<Int16?>.     TwoArgOperation)    (delegate(Int16?     a, Int16?     b) { return (Int16?) (a ^ b); });
			if (t == typeof(Int32?))     return (TwoArgOperation)(object)(Operator<Int32?>.     TwoArgOperation)    (delegate(Int32?     a, Int32?     b) { return           a ^ b;  });
			if (t == typeof(Int64?))     return (TwoArgOperation)(object)(Operator<Int64?>.     TwoArgOperation)    (delegate(Int64?     a, Int64?     b) { return           a ^ b;  });

			if (t == typeof(Byte?))      return (TwoArgOperation)(object)(Operator<Byte?>.      TwoArgOperation)     (delegate(Byte?      a, Byte?      b) { return (Byte?)  (a ^ b); });
			if (t == typeof(UInt16?))    return (TwoArgOperation)(object)(Operator<UInt16?>.    TwoArgOperation)   (delegate(UInt16?    a, UInt16?    b) { return (UInt16?)(a ^ b); });
			if (t == typeof(UInt32?))    return (TwoArgOperation)(object)(Operator<UInt32?>.    TwoArgOperation)   (delegate(UInt32?    a, UInt32?    b) { return           a ^ b;  });
			if (t == typeof(UInt64?))    return (TwoArgOperation)(object)(Operator<UInt64?>.    TwoArgOperation)   (delegate(UInt64?    a, UInt64?    b) { return           a ^ b;  });

			if (t == typeof(Char?))      return (TwoArgOperation)(object)(Operator<Char?>.      TwoArgOperation)     (delegate(Char?      a, Char?      b) { return (Char?)  (a ^ b); });

			// Sql types.
			//
			if (t == typeof(SqlInt16))   return (TwoArgOperation)(object)(Operator<SqlInt16>.   TwoArgOperation)  (delegate(SqlInt16   a, SqlInt16   b) { return           a | b;  });
			if (t == typeof(SqlInt32))   return (TwoArgOperation)(object)(Operator<SqlInt32>.   TwoArgOperation)  (delegate(SqlInt32   a, SqlInt32   b) { return           a | b;  });
			if (t == typeof(SqlInt64))   return (TwoArgOperation)(object)(Operator<SqlInt64>.   TwoArgOperation)  (delegate(SqlInt64   a, SqlInt64   b) { return           a | b;  });

			if (t == typeof(SqlByte))    return (TwoArgOperation)(object)(Operator<SqlByte>.    TwoArgOperation)   (delegate(SqlByte    a, SqlByte    b) { return           a | b;  });

			if (t == typeof(SqlBoolean)) return (TwoArgOperation)(object)(Operator<SqlBoolean>. TwoArgOperation)(delegate(SqlBoolean a, SqlBoolean b) { return           a | b;  });

			return delegate(T a,T b) { throw new InvalidOperationException(); };
		}

		#endregion

		#region (-a)     UnaryNegation

		public  static OneArgOperation UnaryNegation = GetUnaryNegationOperator();
		private static OneArgOperation GetUnaryNegationOperator()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(SByte))    return (OneArgOperation)(object)(Operator<SByte>.   OneArgOperation)(delegate(SByte    a) { return (SByte) (-a); });
			if (t == typeof(Int16))    return (OneArgOperation)(object)(Operator<Int16>.   OneArgOperation)(delegate(Int16    a) { return (Int16) (-a); });
			if (t == typeof(Int32))    return (OneArgOperation)(object)(Operator<Int32>.   OneArgOperation)(delegate(Int32    a) { return          -a;  });
			if (t == typeof(Int64))    return (OneArgOperation)(object)(Operator<Int64>.   OneArgOperation)(delegate(Int64    a) { return          -a;  });

			// Nullable types.
			//
			if (t == typeof(SByte?))   return (OneArgOperation)(object)(Operator<SByte?>.  OneArgOperation)(delegate(SByte?   a) { return (SByte?) -a;  });
			if (t == typeof(Int16?))   return (OneArgOperation)(object)(Operator<Int16?>.  OneArgOperation)(delegate(Int16?   a) { return (Int16?) -a;  });
			if (t == typeof(Int32?))   return (OneArgOperation)(object)(Operator<Int32?>.  OneArgOperation)(delegate(Int32?   a) { return          -a;  });
			if (t == typeof(Int64?))   return (OneArgOperation)(object)(Operator<Int64?>.  OneArgOperation)(delegate(Int64?   a) { return          -a;  });

			// Sql types.
			//
			if (t == typeof(SqlInt16)) return (OneArgOperation)(object)(Operator<SqlInt16>.OneArgOperation)(delegate(SqlInt16 a) { return          -a;  });
			if (t == typeof(SqlInt32)) return (OneArgOperation)(object)(Operator<SqlInt32>.OneArgOperation)(delegate(SqlInt32 a) { return          -a;  });
			if (t == typeof(SqlInt64)) return (OneArgOperation)(object)(Operator<SqlInt64>.OneArgOperation)(delegate(SqlInt64 a) { return          -a;  });

			return delegate(T a) { throw new InvalidOperationException(); };
		}

		#endregion

		#region (~a)     OnesComplement

		public  static OneArgOperation OnesComplement = GetOnesComplementOperator();
		private static OneArgOperation GetOnesComplementOperator()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(SByte))      return (OneArgOperation)(object)(Operator<SByte>.     OneArgOperation)(delegate(SByte      a) { return (SByte)  ~a; });
			if (t == typeof(Int16))      return (OneArgOperation)(object)(Operator<Int16>.     OneArgOperation)(delegate(Int16      a) { return (Int16)  ~a; });
			if (t == typeof(Int32))      return (OneArgOperation)(object)(Operator<Int32>.     OneArgOperation)(delegate(Int32      a) { return          ~a; });
			if (t == typeof(Int64))      return (OneArgOperation)(object)(Operator<Int64>.     OneArgOperation)(delegate(Int64      a) { return          ~a; });

			if (t == typeof(Byte))       return (OneArgOperation)(object)(Operator<Byte>.      OneArgOperation)(delegate(Byte       a) { return (Byte)   ~a; });
			if (t == typeof(UInt16))     return (OneArgOperation)(object)(Operator<UInt16>.    OneArgOperation)(delegate(UInt16     a) { return (UInt16) ~a; });
			if (t == typeof(UInt32))     return (OneArgOperation)(object)(Operator<UInt32>.    OneArgOperation)(delegate(UInt32     a) { return          ~a; });
			if (t == typeof(UInt64))     return (OneArgOperation)(object)(Operator<UInt64>.    OneArgOperation)(delegate(UInt64     a) { return          ~a; });

			if (t == typeof(Char))       return (OneArgOperation)(object)(Operator<Char>.      OneArgOperation)(delegate(Char       a) { return (Char)   ~a; });

			// Nullable types.
			//
			if (t == typeof(SByte?))     return (OneArgOperation)(object)(Operator<SByte?>.    OneArgOperation)(delegate(SByte?     a) { return (SByte?) ~a; });
			if (t == typeof(Int16?))     return (OneArgOperation)(object)(Operator<Int16?>.    OneArgOperation)(delegate(Int16?     a) { return (Int16?) ~a; });
			if (t == typeof(Int32?))     return (OneArgOperation)(object)(Operator<Int32?>.    OneArgOperation)(delegate(Int32?     a) { return          ~a; });
			if (t == typeof(Int64?))     return (OneArgOperation)(object)(Operator<Int64?>.    OneArgOperation)(delegate(Int64?     a) { return          ~a; });

			if (t == typeof(Byte?))      return (OneArgOperation)(object)(Operator<Byte?>.     OneArgOperation)(delegate(Byte?      a) { return (Byte?)  ~a; });
			if (t == typeof(UInt16?))    return (OneArgOperation)(object)(Operator<UInt16?>.   OneArgOperation)(delegate(UInt16?    a) { return (UInt16?)~a; });
			if (t == typeof(UInt32?))    return (OneArgOperation)(object)(Operator<UInt32?>.   OneArgOperation)(delegate(UInt32?    a) { return          ~a; });
			if (t == typeof(UInt64?))    return (OneArgOperation)(object)(Operator<UInt64?>.   OneArgOperation)(delegate(UInt64?    a) { return          ~a; });

			if (t == typeof(Char?))      return (OneArgOperation)(object)(Operator<Char?>.     OneArgOperation)(delegate(Char?      a) { return (Char?)  ~a; });

			// Sql types.
			//
			if (t == typeof(SqlInt16))   return (OneArgOperation)(object)(Operator<SqlInt16>.  OneArgOperation)(delegate(SqlInt16   a) { return          ~a; });
			if (t == typeof(SqlInt32))   return (OneArgOperation)(object)(Operator<SqlInt32>.  OneArgOperation)(delegate(SqlInt32   a) { return          ~a; });
			if (t == typeof(SqlInt64))   return (OneArgOperation)(object)(Operator<SqlInt64>.  OneArgOperation)(delegate(SqlInt64   a) { return          ~a; });

			if (t == typeof(SqlByte))    return (OneArgOperation)(object)(Operator<SqlByte>.   OneArgOperation)(delegate(SqlByte    a) { return          ~a; });

			if (t == typeof(SqlBoolean)) return (OneArgOperation)(object)(Operator<SqlBoolean>.OneArgOperation)(delegate(SqlBoolean a) { return          ~a; });

			return delegate(T a) { throw new InvalidOperationException(); };
		}

		#endregion

		#region (a == b) Equality

		public  static BoolRetOperation Equality = GetEqualityOperator();
		private static BoolRetOperation GetEqualityOperator()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(String))      return (BoolRetOperation)(object)(Operator<String>.     BoolRetOperation)(delegate(String      a, String      b) { return  a == b; });

			if (t == typeof(SByte))       return (BoolRetOperation)(object)(Operator<SByte>.      BoolRetOperation)(delegate(SByte       a, SByte       b) { return  a == b; });
			if (t == typeof(Int16))       return (BoolRetOperation)(object)(Operator<Int16>.      BoolRetOperation)(delegate(Int16       a, Int16       b) { return  a == b; });
			if (t == typeof(Int32))       return (BoolRetOperation)(object)(Operator<Int32>.      BoolRetOperation)(delegate(Int32       a, Int32       b) { return  a == b; });
			if (t == typeof(Int64))       return (BoolRetOperation)(object)(Operator<Int64>.      BoolRetOperation)(delegate(Int64       a, Int64       b) { return  a == b; });

			if (t == typeof(Byte))        return (BoolRetOperation)(object)(Operator<Byte>.       BoolRetOperation)(delegate(Byte        a, Byte        b) { return  a == b; });
			if (t == typeof(UInt16))      return (BoolRetOperation)(object)(Operator<UInt16>.     BoolRetOperation)(delegate(UInt16      a, UInt16      b) { return  a == b; });
			if (t == typeof(UInt32))      return (BoolRetOperation)(object)(Operator<UInt32>.     BoolRetOperation)(delegate(UInt32      a, UInt32      b) { return  a == b; });
			if (t == typeof(UInt64))      return (BoolRetOperation)(object)(Operator<UInt64>.     BoolRetOperation)(delegate(UInt64      a, UInt64      b) { return  a == b; });

			if (t == typeof(bool))        return (BoolRetOperation)(object)(Operator<bool>.       BoolRetOperation)(delegate(bool        a, bool        b) { return  a == b; });
			if (t == typeof(Char))        return (BoolRetOperation)(object)(Operator<Char>.       BoolRetOperation)(delegate(Char        a, Char        b) { return  a == b; });
			if (t == typeof(Single))      return (BoolRetOperation)(object)(Operator<Single>.     BoolRetOperation)(delegate(Single      a, Single      b) { return  a == b; });
			if (t == typeof(Double))      return (BoolRetOperation)(object)(Operator<Double>.     BoolRetOperation)(delegate(Double      a, Double      b) { return  a == b; });

			if (t == typeof(Decimal))     return (BoolRetOperation)(object)(Operator<Decimal>.    BoolRetOperation)(delegate(Decimal     a, Decimal     b) { return  a == b; });
			if (t == typeof(DateTime))    return (BoolRetOperation)(object)(Operator<DateTime>.   BoolRetOperation)(delegate(DateTime    a, DateTime    b) { return  a == b; });
			if (t == typeof(TimeSpan))    return (BoolRetOperation)(object)(Operator<TimeSpan>.   BoolRetOperation)(delegate(TimeSpan    a, TimeSpan    b) { return  a == b; });
			if (t == typeof(Guid))        return (BoolRetOperation)(object)(Operator<Guid>.       BoolRetOperation)(delegate(Guid        a, Guid        b) { return  a == b; });

			// Nullable types.
			//
			if (t == typeof(SByte?))      return (BoolRetOperation)(object)(Operator<SByte?>.     BoolRetOperation)(delegate(SByte?      a, SByte?      b) { return  a == b; });
			if (t == typeof(Int16?))      return (BoolRetOperation)(object)(Operator<Int16?>.     BoolRetOperation)(delegate(Int16?      a, Int16?      b) { return  a == b; });
			if (t == typeof(Int32?))      return (BoolRetOperation)(object)(Operator<Int32?>.     BoolRetOperation)(delegate(Int32?      a, Int32?      b) { return  a == b; });
			if (t == typeof(Int64?))      return (BoolRetOperation)(object)(Operator<Int64?>.     BoolRetOperation)(delegate(Int64?      a, Int64?      b) { return  a == b; });

			if (t == typeof(Byte?))       return (BoolRetOperation)(object)(Operator<Byte?>.      BoolRetOperation)(delegate(Byte?       a, Byte?       b) { return  a == b; });
			if (t == typeof(UInt16?))     return (BoolRetOperation)(object)(Operator<UInt16?>.    BoolRetOperation)(delegate(UInt16?     a, UInt16?     b) { return  a == b; });
			if (t == typeof(UInt32?))     return (BoolRetOperation)(object)(Operator<UInt32?>.    BoolRetOperation)(delegate(UInt32?     a, UInt32?     b) { return  a == b; });
			if (t == typeof(UInt64?))     return (BoolRetOperation)(object)(Operator<UInt64?>.    BoolRetOperation)(delegate(UInt64?     a, UInt64?     b) { return  a == b; });

			if (t == typeof(bool?))       return (BoolRetOperation)(object)(Operator<bool?>.      BoolRetOperation)(delegate(bool?       a, bool?       b) { return  a == b; });
			if (t == typeof(Char?))       return (BoolRetOperation)(object)(Operator<Char?>.      BoolRetOperation)(delegate(Char?       a, Char?       b) { return  a == b; });
			if (t == typeof(Single?))     return (BoolRetOperation)(object)(Operator<Single?>.    BoolRetOperation)(delegate(Single?     a, Single?     b) { return  a == b; });
			if (t == typeof(Double?))     return (BoolRetOperation)(object)(Operator<Double?>.    BoolRetOperation)(delegate(Double?     a, Double?     b) { return  a == b; });

			if (t == typeof(Decimal?))    return (BoolRetOperation)(object)(Operator<Decimal?>.   BoolRetOperation)(delegate(Decimal?    a, Decimal?    b) { return  a == b; });
			if (t == typeof(DateTime?))   return (BoolRetOperation)(object)(Operator<DateTime?>.  BoolRetOperation)(delegate(DateTime?   a, DateTime?   b) { return  a == b; });
			if (t == typeof(TimeSpan?))   return (BoolRetOperation)(object)(Operator<TimeSpan?>.  BoolRetOperation)(delegate(TimeSpan?   a, TimeSpan?   b) { return  a == b; });
			if (t == typeof(Guid?))       return (BoolRetOperation)(object)(Operator<Guid?>.      BoolRetOperation)(delegate(Guid?       a, Guid?       b) { return  a == b; });

			// Sql types.
			//
			if (t == typeof(SqlString))   return (BoolRetOperation)(object)(Operator<SqlString>.  BoolRetOperation)(delegate(SqlString   a, SqlString   b) { return (a == b).IsTrue; });

			if (t == typeof(SqlInt16))    return (BoolRetOperation)(object)(Operator<SqlInt16>.   BoolRetOperation)(delegate(SqlInt16    a, SqlInt16    b) { return (a == b).IsTrue; });
			if (t == typeof(SqlInt32))    return (BoolRetOperation)(object)(Operator<SqlInt32>.   BoolRetOperation)(delegate(SqlInt32    a, SqlInt32    b) { return (a == b).IsTrue; });
			if (t == typeof(SqlInt64))    return (BoolRetOperation)(object)(Operator<SqlInt64>.   BoolRetOperation)(delegate(SqlInt64    a, SqlInt64    b) { return (a == b).IsTrue; });

			if (t == typeof(SqlByte))     return (BoolRetOperation)(object)(Operator<SqlByte>.    BoolRetOperation)(delegate(SqlByte     a, SqlByte     b) { return (a == b).IsTrue; });

			if (t == typeof(SqlSingle))   return (BoolRetOperation)(object)(Operator<SqlSingle>.  BoolRetOperation)(delegate(SqlSingle   a, SqlSingle   b) { return (a == b).IsTrue; });
			if (t == typeof(SqlDouble))   return (BoolRetOperation)(object)(Operator<SqlDouble>.  BoolRetOperation)(delegate(SqlDouble   a, SqlDouble   b) { return (a == b).IsTrue; });
			if (t == typeof(SqlDecimal))  return (BoolRetOperation)(object)(Operator<SqlDecimal>. BoolRetOperation)(delegate(SqlDecimal  a, SqlDecimal  b) { return (a == b).IsTrue; });
			if (t == typeof(SqlMoney))    return (BoolRetOperation)(object)(Operator<SqlMoney>.   BoolRetOperation)(delegate(SqlMoney    a, SqlMoney    b) { return (a == b).IsTrue; });

			if (t == typeof(SqlBoolean))  return (BoolRetOperation)(object)(Operator<SqlBoolean>. BoolRetOperation)(delegate(SqlBoolean  a, SqlBoolean  b) { return (a == b).IsTrue; });
			if (t == typeof(SqlBinary))   return (BoolRetOperation)(object)(Operator<SqlBinary>.  BoolRetOperation)(delegate(SqlBinary   a, SqlBinary   b) { return (a == b).IsTrue; });
			if (t == typeof(SqlDateTime)) return (BoolRetOperation)(object)(Operator<SqlDateTime>.BoolRetOperation)(delegate(SqlDateTime a, SqlDateTime b) { return (a == b).IsTrue; });
			if (t == typeof(SqlGuid))     return (BoolRetOperation)(object)(Operator<SqlGuid>.    BoolRetOperation)(delegate(SqlGuid     a, SqlGuid     b) { return (a == b).IsTrue; });

			return delegate(T a,T b) { return a.Equals(b); };
		}

		#endregion

		#region (a != b) Inequality

		public  static BoolRetOperation Inequality = GetInequalityOperator();
		private static BoolRetOperation GetInequalityOperator()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(String))      return (BoolRetOperation)(object)(Operator<String>.     BoolRetOperation)(delegate(String      a, String      b) { return  a != b; });

			if (t == typeof(SByte))       return (BoolRetOperation)(object)(Operator<SByte>.      BoolRetOperation)(delegate(SByte       a, SByte       b) { return  a != b; });
			if (t == typeof(Int16))       return (BoolRetOperation)(object)(Operator<Int16>.      BoolRetOperation)(delegate(Int16       a, Int16       b) { return  a != b; });
			if (t == typeof(Int32))       return (BoolRetOperation)(object)(Operator<Int32>.      BoolRetOperation)(delegate(Int32       a, Int32       b) { return  a != b; });
			if (t == typeof(Int64))       return (BoolRetOperation)(object)(Operator<Int64>.      BoolRetOperation)(delegate(Int64       a, Int64       b) { return  a != b; });

			if (t == typeof(Byte))        return (BoolRetOperation)(object)(Operator<Byte>.       BoolRetOperation)(delegate(Byte        a, Byte        b) { return  a != b; });
			if (t == typeof(UInt16))      return (BoolRetOperation)(object)(Operator<UInt16>.     BoolRetOperation)(delegate(UInt16      a, UInt16      b) { return  a != b; });
			if (t == typeof(UInt32))      return (BoolRetOperation)(object)(Operator<UInt32>.     BoolRetOperation)(delegate(UInt32      a, UInt32      b) { return  a != b; });
			if (t == typeof(UInt64))      return (BoolRetOperation)(object)(Operator<UInt64>.     BoolRetOperation)(delegate(UInt64      a, UInt64      b) { return  a != b; });

			if (t == typeof(bool))        return (BoolRetOperation)(object)(Operator<bool>.       BoolRetOperation)(delegate(bool        a, bool        b) { return  a != b; });
			if (t == typeof(Char))        return (BoolRetOperation)(object)(Operator<Char>.       BoolRetOperation)(delegate(Char        a, Char        b) { return  a != b; });
			if (t == typeof(Single))      return (BoolRetOperation)(object)(Operator<Single>.     BoolRetOperation)(delegate(Single      a, Single      b) { return  a != b; });
			if (t == typeof(Double))      return (BoolRetOperation)(object)(Operator<Double>.     BoolRetOperation)(delegate(Double      a, Double      b) { return  a != b; });

			if (t == typeof(Decimal))     return (BoolRetOperation)(object)(Operator<Decimal>.    BoolRetOperation)(delegate(Decimal     a, Decimal     b) { return  a != b; });
			if (t == typeof(DateTime))    return (BoolRetOperation)(object)(Operator<DateTime>.   BoolRetOperation)(delegate(DateTime    a, DateTime    b) { return  a != b; });
			if (t == typeof(TimeSpan))    return (BoolRetOperation)(object)(Operator<TimeSpan>.   BoolRetOperation)(delegate(TimeSpan    a, TimeSpan    b) { return  a != b; });
			if (t == typeof(Guid))        return (BoolRetOperation)(object)(Operator<Guid>.       BoolRetOperation)(delegate(Guid        a, Guid        b) { return  a != b; });

			// Nullable types.
			//
			if (t == typeof(SByte?))      return (BoolRetOperation)(object)(Operator<SByte?>.     BoolRetOperation)(delegate(SByte?      a, SByte?      b) { return  a != b; });
			if (t == typeof(Int16?))      return (BoolRetOperation)(object)(Operator<Int16?>.     BoolRetOperation)(delegate(Int16?      a, Int16?      b) { return  a != b; });
			if (t == typeof(Int32?))      return (BoolRetOperation)(object)(Operator<Int32?>.     BoolRetOperation)(delegate(Int32?      a, Int32?      b) { return  a != b; });
			if (t == typeof(Int64?))      return (BoolRetOperation)(object)(Operator<Int64?>.     BoolRetOperation)(delegate(Int64?      a, Int64?      b) { return  a != b; });

			if (t == typeof(Byte?))       return (BoolRetOperation)(object)(Operator<Byte?>.      BoolRetOperation)(delegate(Byte?       a, Byte?       b) { return  a != b; });
			if (t == typeof(UInt16?))     return (BoolRetOperation)(object)(Operator<UInt16?>.    BoolRetOperation)(delegate(UInt16?     a, UInt16?     b) { return  a != b; });
			if (t == typeof(UInt32?))     return (BoolRetOperation)(object)(Operator<UInt32?>.    BoolRetOperation)(delegate(UInt32?     a, UInt32?     b) { return  a != b; });
			if (t == typeof(UInt64?))     return (BoolRetOperation)(object)(Operator<UInt64?>.    BoolRetOperation)(delegate(UInt64?     a, UInt64?     b) { return  a != b; });

			if (t == typeof(bool?))       return (BoolRetOperation)(object)(Operator<bool?>.      BoolRetOperation)(delegate(bool?       a, bool?       b) { return  a != b; });
			if (t == typeof(Char?))       return (BoolRetOperation)(object)(Operator<Char?>.      BoolRetOperation)(delegate(Char?       a, Char?       b) { return  a != b; });
			if (t == typeof(Single?))     return (BoolRetOperation)(object)(Operator<Single?>.    BoolRetOperation)(delegate(Single?     a, Single?     b) { return  a != b; });
			if (t == typeof(Double?))     return (BoolRetOperation)(object)(Operator<Double?>.    BoolRetOperation)(delegate(Double?     a, Double?     b) { return  a != b; });

			if (t == typeof(Decimal?))    return (BoolRetOperation)(object)(Operator<Decimal?>.   BoolRetOperation)(delegate(Decimal?    a, Decimal?    b) { return  a != b; });
			if (t == typeof(DateTime?))   return (BoolRetOperation)(object)(Operator<DateTime?>.  BoolRetOperation)(delegate(DateTime?   a, DateTime?   b) { return  a != b; });
			if (t == typeof(TimeSpan?))   return (BoolRetOperation)(object)(Operator<TimeSpan?>.  BoolRetOperation)(delegate(TimeSpan?   a, TimeSpan?   b) { return  a != b; });
			if (t == typeof(Guid?))       return (BoolRetOperation)(object)(Operator<Guid?>.      BoolRetOperation)(delegate(Guid?       a, Guid?       b) { return  a != b; });

			// Sql types.
			//
			if (t == typeof(SqlString))   return (BoolRetOperation)(object)(Operator<SqlString>.  BoolRetOperation)(delegate(SqlString   a, SqlString   b) { return (a != b).IsTrue; });

			if (t == typeof(SqlInt16))    return (BoolRetOperation)(object)(Operator<SqlInt16>.   BoolRetOperation)(delegate(SqlInt16    a, SqlInt16    b) { return (a != b).IsTrue; });
			if (t == typeof(SqlInt32))    return (BoolRetOperation)(object)(Operator<SqlInt32>.   BoolRetOperation)(delegate(SqlInt32    a, SqlInt32    b) { return (a != b).IsTrue; });

			if (t == typeof(SqlByte))     return (BoolRetOperation)(object)(Operator<SqlByte>.    BoolRetOperation)(delegate(SqlByte     a, SqlByte     b) { return (a != b).IsTrue; });

			if (t == typeof(SqlSingle))   return (BoolRetOperation)(object)(Operator<SqlSingle>.  BoolRetOperation)(delegate(SqlSingle   a, SqlSingle   b) { return (a != b).IsTrue; });
			if (t == typeof(SqlDouble))   return (BoolRetOperation)(object)(Operator<SqlDouble>.  BoolRetOperation)(delegate(SqlDouble   a, SqlDouble   b) { return (a != b).IsTrue; });
			if (t == typeof(SqlDecimal))  return (BoolRetOperation)(object)(Operator<SqlDecimal>. BoolRetOperation)(delegate(SqlDecimal  a, SqlDecimal  b) { return (a != b).IsTrue; });
			if (t == typeof(SqlMoney))    return (BoolRetOperation)(object)(Operator<SqlMoney>.   BoolRetOperation)(delegate(SqlMoney    a, SqlMoney    b) { return (a != b).IsTrue; });

			if (t == typeof(SqlBoolean))  return (BoolRetOperation)(object)(Operator<SqlBoolean>. BoolRetOperation)(delegate(SqlBoolean  a, SqlBoolean  b) { return (a != b).IsTrue; });
			if (t == typeof(SqlBinary))   return (BoolRetOperation)(object)(Operator<SqlBinary>.  BoolRetOperation)(delegate(SqlBinary   a, SqlBinary   b) { return (a != b).IsTrue; });
			if (t == typeof(SqlDateTime)) return (BoolRetOperation)(object)(Operator<SqlDateTime>.BoolRetOperation)(delegate(SqlDateTime a, SqlDateTime b) { return (a != b).IsTrue; });
			if (t == typeof(SqlGuid))     return (BoolRetOperation)(object)(Operator<SqlGuid>.    BoolRetOperation)(delegate(SqlGuid     a, SqlGuid     b) { return (a != b).IsTrue; });

			return delegate(T a,T b) { return !a.Equals(b); };
		}

		#endregion

		#region (a > b)  GreaterThan

		public  static BoolRetOperation GreaterThan = GetGreaterThanOperator();
		private static BoolRetOperation GetGreaterThanOperator()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(SByte))       return (BoolRetOperation)(object)(Operator<SByte>.      BoolRetOperation)(delegate(SByte       a, SByte       b) { return  a > b; });
			if (t == typeof(Int16))       return (BoolRetOperation)(object)(Operator<Int16>.      BoolRetOperation)(delegate(Int16       a, Int16       b) { return  a > b; });
			if (t == typeof(Int32))       return (BoolRetOperation)(object)(Operator<Int32>.      BoolRetOperation)(delegate(Int32       a, Int32       b) { return  a > b; });
			if (t == typeof(Int64))       return (BoolRetOperation)(object)(Operator<Int64>.      BoolRetOperation)(delegate(Int64       a, Int64       b) { return  a > b; });

			if (t == typeof(Byte))        return (BoolRetOperation)(object)(Operator<Byte>.       BoolRetOperation)(delegate(Byte        a, Byte        b) { return  a > b; });
			if (t == typeof(UInt16))      return (BoolRetOperation)(object)(Operator<UInt16>.     BoolRetOperation)(delegate(UInt16      a, UInt16      b) { return  a > b; });
			if (t == typeof(UInt32))      return (BoolRetOperation)(object)(Operator<UInt32>.     BoolRetOperation)(delegate(UInt32      a, UInt32      b) { return  a > b; });
			if (t == typeof(UInt64))      return (BoolRetOperation)(object)(Operator<UInt64>.     BoolRetOperation)(delegate(UInt64      a, UInt64      b) { return  a > b; });

			if (t == typeof(Char))        return (BoolRetOperation)(object)(Operator<Char>.       BoolRetOperation)(delegate(Char        a, Char        b) { return  a > b; });
			if (t == typeof(Single))      return (BoolRetOperation)(object)(Operator<Single>.     BoolRetOperation)(delegate(Single      a, Single      b) { return  a > b; });
			if (t == typeof(Double))      return (BoolRetOperation)(object)(Operator<Double>.     BoolRetOperation)(delegate(Double      a, Double      b) { return  a > b; });

			if (t == typeof(Decimal))     return (BoolRetOperation)(object)(Operator<Decimal>.    BoolRetOperation)(delegate(Decimal     a, Decimal     b) { return  a > b; });
			if (t == typeof(DateTime))    return (BoolRetOperation)(object)(Operator<DateTime>.   BoolRetOperation)(delegate(DateTime    a, DateTime    b) { return  a > b; });
			if (t == typeof(TimeSpan))    return (BoolRetOperation)(object)(Operator<TimeSpan>.   BoolRetOperation)(delegate(TimeSpan    a, TimeSpan    b) { return  a > b; });

			// Nullable types.
			//
			if (t == typeof(SByte?))      return (BoolRetOperation)(object)(Operator<SByte?>.     BoolRetOperation)(delegate(SByte?      a, SByte?      b) { return  a > b; });
			if (t == typeof(Int16?))      return (BoolRetOperation)(object)(Operator<Int16?>.     BoolRetOperation)(delegate(Int16?      a, Int16?      b) { return  a > b; });
			if (t == typeof(Int32?))      return (BoolRetOperation)(object)(Operator<Int32?>.     BoolRetOperation)(delegate(Int32?      a, Int32?      b) { return  a > b; });
			if (t == typeof(Int64?))      return (BoolRetOperation)(object)(Operator<Int64?>.     BoolRetOperation)(delegate(Int64?      a, Int64?      b) { return  a > b; });

			if (t == typeof(Byte?))       return (BoolRetOperation)(object)(Operator<Byte?>.      BoolRetOperation)(delegate(Byte?       a, Byte?       b) { return  a > b; });
			if (t == typeof(UInt16?))     return (BoolRetOperation)(object)(Operator<UInt16?>.    BoolRetOperation)(delegate(UInt16?     a, UInt16?     b) { return  a > b; });
			if (t == typeof(UInt32?))     return (BoolRetOperation)(object)(Operator<UInt32?>.    BoolRetOperation)(delegate(UInt32?     a, UInt32?     b) { return  a > b; });
			if (t == typeof(UInt64?))     return (BoolRetOperation)(object)(Operator<UInt64?>.    BoolRetOperation)(delegate(UInt64?     a, UInt64?     b) { return  a > b; });

			if (t == typeof(Char?))       return (BoolRetOperation)(object)(Operator<Char?>.      BoolRetOperation)(delegate(Char?       a, Char?       b) { return  a > b; });
			if (t == typeof(Single?))     return (BoolRetOperation)(object)(Operator<Single?>.    BoolRetOperation)(delegate(Single?     a, Single?     b) { return  a > b; });
			if (t == typeof(Double?))     return (BoolRetOperation)(object)(Operator<Double?>.    BoolRetOperation)(delegate(Double?     a, Double?     b) { return  a > b; });

			if (t == typeof(Decimal?))    return (BoolRetOperation)(object)(Operator<Decimal?>.   BoolRetOperation)(delegate(Decimal?    a, Decimal?    b) { return  a > b; });
			if (t == typeof(DateTime?))   return (BoolRetOperation)(object)(Operator<DateTime?>.  BoolRetOperation)(delegate(DateTime?   a, DateTime?   b) { return  a > b; });
			if (t == typeof(TimeSpan?))   return (BoolRetOperation)(object)(Operator<TimeSpan?>.  BoolRetOperation)(delegate(TimeSpan?   a, TimeSpan?   b) { return  a > b; });

			// Sql types.
			//
			if (t == typeof(SqlString))   return (BoolRetOperation)(object)(Operator<SqlString>.  BoolRetOperation)(delegate(SqlString   a, SqlString   b) { return (a > b).IsTrue; });

			if (t == typeof(SqlInt16))    return (BoolRetOperation)(object)(Operator<SqlInt16>.   BoolRetOperation)(delegate(SqlInt16    a, SqlInt16    b) { return (a > b).IsTrue; });
			if (t == typeof(SqlInt32))    return (BoolRetOperation)(object)(Operator<SqlInt32>.   BoolRetOperation)(delegate(SqlInt32    a, SqlInt32    b) { return (a > b).IsTrue; });

			if (t == typeof(SqlByte))     return (BoolRetOperation)(object)(Operator<SqlByte>.    BoolRetOperation)(delegate(SqlByte     a, SqlByte     b) { return (a > b).IsTrue; });

			if (t == typeof(SqlSingle))   return (BoolRetOperation)(object)(Operator<SqlSingle>.  BoolRetOperation)(delegate(SqlSingle   a, SqlSingle   b) { return (a > b).IsTrue; });
			if (t == typeof(SqlDouble))   return (BoolRetOperation)(object)(Operator<SqlDouble>.  BoolRetOperation)(delegate(SqlDouble   a, SqlDouble   b) { return (a > b).IsTrue; });
			if (t == typeof(SqlDecimal))  return (BoolRetOperation)(object)(Operator<SqlDecimal>. BoolRetOperation)(delegate(SqlDecimal  a, SqlDecimal  b) { return (a > b).IsTrue; });
			if (t == typeof(SqlMoney))    return (BoolRetOperation)(object)(Operator<SqlMoney>.   BoolRetOperation)(delegate(SqlMoney    a, SqlMoney    b) { return (a > b).IsTrue; });

			if (t == typeof(SqlBoolean))  return (BoolRetOperation)(object)(Operator<SqlBoolean>. BoolRetOperation)(delegate(SqlBoolean  a, SqlBoolean  b) { return (a > b).IsTrue; });
			if (t == typeof(SqlBinary))   return (BoolRetOperation)(object)(Operator<SqlBinary>.  BoolRetOperation)(delegate(SqlBinary   a, SqlBinary   b) { return (a > b).IsTrue; });
			if (t == typeof(SqlDateTime)) return (BoolRetOperation)(object)(Operator<SqlDateTime>.BoolRetOperation)(delegate(SqlDateTime a, SqlDateTime b) { return (a > b).IsTrue; });
			if (t == typeof(SqlGuid))     return (BoolRetOperation)(object)(Operator<SqlGuid>.    BoolRetOperation)(delegate(SqlGuid     a, SqlGuid     b) { return (a > b).IsTrue; });

			return delegate(T a,T b) { throw new InvalidOperationException(); };
		}

		#endregion

		#region (a >= b) GreaterThanOrEqual

		public  static BoolRetOperation GreaterThanOrEqual = GetGreaterThanOrEqualOperator();
		private static BoolRetOperation GetGreaterThanOrEqualOperator()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(SByte))       return (BoolRetOperation)(object)(Operator<SByte>.      BoolRetOperation)(delegate(SByte       a, SByte       b) { return  a >= b; });
			if (t == typeof(Int16))       return (BoolRetOperation)(object)(Operator<Int16>.      BoolRetOperation)(delegate(Int16       a, Int16       b) { return  a >= b; });
			if (t == typeof(Int32))       return (BoolRetOperation)(object)(Operator<Int32>.      BoolRetOperation)(delegate(Int32       a, Int32       b) { return  a >= b; });
			if (t == typeof(Int64))       return (BoolRetOperation)(object)(Operator<Int64>.      BoolRetOperation)(delegate(Int64       a, Int64       b) { return  a >= b; });

			if (t == typeof(Byte))        return (BoolRetOperation)(object)(Operator<Byte>.       BoolRetOperation)(delegate(Byte        a, Byte        b) { return  a >= b; });
			if (t == typeof(UInt16))      return (BoolRetOperation)(object)(Operator<UInt16>.     BoolRetOperation)(delegate(UInt16      a, UInt16      b) { return  a >= b; });
			if (t == typeof(UInt32))      return (BoolRetOperation)(object)(Operator<UInt32>.     BoolRetOperation)(delegate(UInt32      a, UInt32      b) { return  a >= b; });
			if (t == typeof(UInt64))      return (BoolRetOperation)(object)(Operator<UInt64>.     BoolRetOperation)(delegate(UInt64      a, UInt64      b) { return  a >= b; });

			if (t == typeof(Char))        return (BoolRetOperation)(object)(Operator<Char>.       BoolRetOperation)(delegate(Char        a, Char        b) { return  a >= b; });
			if (t == typeof(Single))      return (BoolRetOperation)(object)(Operator<Single>.     BoolRetOperation)(delegate(Single      a, Single      b) { return  a >= b; });
			if (t == typeof(Double))      return (BoolRetOperation)(object)(Operator<Double>.     BoolRetOperation)(delegate(Double      a, Double      b) { return  a >= b; });

			if (t == typeof(Decimal))     return (BoolRetOperation)(object)(Operator<Decimal>.    BoolRetOperation)(delegate(Decimal     a, Decimal     b) { return  a >= b; });
			if (t == typeof(DateTime))    return (BoolRetOperation)(object)(Operator<DateTime>.   BoolRetOperation)(delegate(DateTime    a, DateTime    b) { return  a >= b; });
			if (t == typeof(TimeSpan))    return (BoolRetOperation)(object)(Operator<TimeSpan>.   BoolRetOperation)(delegate(TimeSpan    a, TimeSpan    b) { return  a >= b; });

			// Nullable types.
			//
			if (t == typeof(SByte?))      return (BoolRetOperation)(object)(Operator<SByte?>.     BoolRetOperation)(delegate(SByte?      a, SByte?      b) { return  a >= b; });
			if (t == typeof(Int16?))      return (BoolRetOperation)(object)(Operator<Int16?>.     BoolRetOperation)(delegate(Int16?      a, Int16?      b) { return  a >= b; });
			if (t == typeof(Int32?))      return (BoolRetOperation)(object)(Operator<Int32?>.     BoolRetOperation)(delegate(Int32?      a, Int32?      b) { return  a >= b; });
			if (t == typeof(Int64?))      return (BoolRetOperation)(object)(Operator<Int64?>.     BoolRetOperation)(delegate(Int64?      a, Int64?      b) { return  a >= b; });

			if (t == typeof(Byte?))       return (BoolRetOperation)(object)(Operator<Byte?>.      BoolRetOperation)(delegate(Byte?       a, Byte?       b) { return  a >= b; });
			if (t == typeof(UInt16?))     return (BoolRetOperation)(object)(Operator<UInt16?>.    BoolRetOperation)(delegate(UInt16?     a, UInt16?     b) { return  a >= b; });
			if (t == typeof(UInt32?))     return (BoolRetOperation)(object)(Operator<UInt32?>.    BoolRetOperation)(delegate(UInt32?     a, UInt32?     b) { return  a >= b; });
			if (t == typeof(UInt64?))     return (BoolRetOperation)(object)(Operator<UInt64?>.    BoolRetOperation)(delegate(UInt64?     a, UInt64?     b) { return  a >= b; });

			if (t == typeof(Char?))       return (BoolRetOperation)(object)(Operator<Char?>.      BoolRetOperation)(delegate(Char?       a, Char?       b) { return  a >= b; });
			if (t == typeof(Single?))     return (BoolRetOperation)(object)(Operator<Single?>.    BoolRetOperation)(delegate(Single?     a, Single?     b) { return  a >= b; });
			if (t == typeof(Double?))     return (BoolRetOperation)(object)(Operator<Double?>.    BoolRetOperation)(delegate(Double?     a, Double?     b) { return  a >= b; });

			if (t == typeof(Decimal?))    return (BoolRetOperation)(object)(Operator<Decimal?>.   BoolRetOperation)(delegate(Decimal?    a, Decimal?    b) { return  a >= b; });
			if (t == typeof(DateTime?))   return (BoolRetOperation)(object)(Operator<DateTime?>.  BoolRetOperation)(delegate(DateTime?   a, DateTime?   b) { return  a >= b; });
			if (t == typeof(TimeSpan?))   return (BoolRetOperation)(object)(Operator<TimeSpan?>.  BoolRetOperation)(delegate(TimeSpan?   a, TimeSpan?   b) { return  a >= b; });

			// Sql types.
			//
			if (t == typeof(SqlString))   return (BoolRetOperation)(object)(Operator<SqlString>.  BoolRetOperation)(delegate(SqlString   a, SqlString   b) { return (a >= b).IsTrue; });

			if (t == typeof(SqlInt16))    return (BoolRetOperation)(object)(Operator<SqlInt16>.   BoolRetOperation)(delegate(SqlInt16    a, SqlInt16    b) { return (a >= b).IsTrue; });
			if (t == typeof(SqlInt32))    return (BoolRetOperation)(object)(Operator<SqlInt32>.   BoolRetOperation)(delegate(SqlInt32    a, SqlInt32    b) { return (a >= b).IsTrue; });

			if (t == typeof(SqlByte))     return (BoolRetOperation)(object)(Operator<SqlByte>.    BoolRetOperation)(delegate(SqlByte     a, SqlByte     b) { return (a >= b).IsTrue; });

			if (t == typeof(SqlSingle))   return (BoolRetOperation)(object)(Operator<SqlSingle>.  BoolRetOperation)(delegate(SqlSingle   a, SqlSingle   b) { return (a >= b).IsTrue; });
			if (t == typeof(SqlDouble))   return (BoolRetOperation)(object)(Operator<SqlDouble>.  BoolRetOperation)(delegate(SqlDouble   a, SqlDouble   b) { return (a >= b).IsTrue; });
			if (t == typeof(SqlDecimal))  return (BoolRetOperation)(object)(Operator<SqlDecimal>. BoolRetOperation)(delegate(SqlDecimal  a, SqlDecimal  b) { return (a >= b).IsTrue; });
			if (t == typeof(SqlMoney))    return (BoolRetOperation)(object)(Operator<SqlMoney>.   BoolRetOperation)(delegate(SqlMoney    a, SqlMoney    b) { return (a >= b).IsTrue; });

			if (t == typeof(SqlBoolean))  return (BoolRetOperation)(object)(Operator<SqlBoolean>. BoolRetOperation)(delegate(SqlBoolean  a, SqlBoolean  b) { return (a >= b).IsTrue; });
			if (t == typeof(SqlBinary))   return (BoolRetOperation)(object)(Operator<SqlBinary>.  BoolRetOperation)(delegate(SqlBinary   a, SqlBinary   b) { return (a >= b).IsTrue; });
			if (t == typeof(SqlDateTime)) return (BoolRetOperation)(object)(Operator<SqlDateTime>.BoolRetOperation)(delegate(SqlDateTime a, SqlDateTime b) { return (a >= b).IsTrue; });
			if (t == typeof(SqlGuid))     return (BoolRetOperation)(object)(Operator<SqlGuid>.    BoolRetOperation)(delegate(SqlGuid     a, SqlGuid     b) { return (a >= b).IsTrue; });

			return delegate(T a,T b) { throw new InvalidOperationException(); };
		}

		#endregion

		#region (a < b)  LessThan 

		public  static BoolRetOperation LessThan = GetLessThanOperator();
		private static BoolRetOperation GetLessThanOperator()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(SByte))       return (BoolRetOperation)(object)(Operator<SByte>.      BoolRetOperation)(delegate(SByte       a, SByte       b) { return  a < b; });
			if (t == typeof(Int16))       return (BoolRetOperation)(object)(Operator<Int16>.      BoolRetOperation)(delegate(Int16       a, Int16       b) { return  a < b; });
			if (t == typeof(Int32))       return (BoolRetOperation)(object)(Operator<Int32>.      BoolRetOperation)(delegate(Int32       a, Int32       b) { return  a < b; });
			if (t == typeof(Int64))       return (BoolRetOperation)(object)(Operator<Int64>.      BoolRetOperation)(delegate(Int64       a, Int64       b) { return  a < b; });

			if (t == typeof(Byte))        return (BoolRetOperation)(object)(Operator<Byte>.       BoolRetOperation)(delegate(Byte        a, Byte        b) { return  a < b; });
			if (t == typeof(UInt16))      return (BoolRetOperation)(object)(Operator<UInt16>.     BoolRetOperation)(delegate(UInt16      a, UInt16      b) { return  a < b; });
			if (t == typeof(UInt32))      return (BoolRetOperation)(object)(Operator<UInt32>.     BoolRetOperation)(delegate(UInt32      a, UInt32      b) { return  a < b; });
			if (t == typeof(UInt64))      return (BoolRetOperation)(object)(Operator<UInt64>.     BoolRetOperation)(delegate(UInt64      a, UInt64      b) { return  a < b; });

			if (t == typeof(Char))        return (BoolRetOperation)(object)(Operator<Char>.       BoolRetOperation)(delegate(Char        a, Char        b) { return  a < b; });
			if (t == typeof(Single))      return (BoolRetOperation)(object)(Operator<Single>.     BoolRetOperation)(delegate(Single      a, Single      b) { return  a < b; });
			if (t == typeof(Double))      return (BoolRetOperation)(object)(Operator<Double>.     BoolRetOperation)(delegate(Double      a, Double      b) { return  a < b; });

			if (t == typeof(Decimal))     return (BoolRetOperation)(object)(Operator<Decimal>.    BoolRetOperation)(delegate(Decimal     a, Decimal     b) { return  a < b; });
			if (t == typeof(DateTime))    return (BoolRetOperation)(object)(Operator<DateTime>.   BoolRetOperation)(delegate(DateTime    a, DateTime    b) { return  a < b; });
			if (t == typeof(TimeSpan))    return (BoolRetOperation)(object)(Operator<TimeSpan>.   BoolRetOperation)(delegate(TimeSpan    a, TimeSpan    b) { return  a < b; });

			// Nullable types.
			//
			if (t == typeof(SByte?))      return (BoolRetOperation)(object)(Operator<SByte?>.     BoolRetOperation)(delegate(SByte?      a, SByte?      b) { return  a < b; });
			if (t == typeof(Int16?))      return (BoolRetOperation)(object)(Operator<Int16?>.     BoolRetOperation)(delegate(Int16?      a, Int16?      b) { return  a < b; });
			if (t == typeof(Int32?))      return (BoolRetOperation)(object)(Operator<Int32?>.     BoolRetOperation)(delegate(Int32?      a, Int32?      b) { return  a < b; });
			if (t == typeof(Int64?))      return (BoolRetOperation)(object)(Operator<Int64?>.     BoolRetOperation)(delegate(Int64?      a, Int64?      b) { return  a < b; });

			if (t == typeof(Byte?))       return (BoolRetOperation)(object)(Operator<Byte?>.      BoolRetOperation)(delegate(Byte?       a, Byte?       b) { return  a < b; });
			if (t == typeof(UInt16?))     return (BoolRetOperation)(object)(Operator<UInt16?>.    BoolRetOperation)(delegate(UInt16?     a, UInt16?     b) { return  a < b; });
			if (t == typeof(UInt32?))     return (BoolRetOperation)(object)(Operator<UInt32?>.    BoolRetOperation)(delegate(UInt32?     a, UInt32?     b) { return  a < b; });
			if (t == typeof(UInt64?))     return (BoolRetOperation)(object)(Operator<UInt64?>.    BoolRetOperation)(delegate(UInt64?     a, UInt64?     b) { return  a < b; });

			if (t == typeof(Char?))       return (BoolRetOperation)(object)(Operator<Char?>.      BoolRetOperation)(delegate(Char?       a, Char?       b) { return  a < b; });
			if (t == typeof(Single?))     return (BoolRetOperation)(object)(Operator<Single?>.    BoolRetOperation)(delegate(Single?     a, Single?     b) { return  a < b; });
			if (t == typeof(Double?))     return (BoolRetOperation)(object)(Operator<Double?>.    BoolRetOperation)(delegate(Double?     a, Double?     b) { return  a < b; });

			if (t == typeof(Decimal?))    return (BoolRetOperation)(object)(Operator<Decimal?>.   BoolRetOperation)(delegate(Decimal?    a, Decimal?    b) { return  a < b; });
			if (t == typeof(DateTime?))   return (BoolRetOperation)(object)(Operator<DateTime?>.  BoolRetOperation)(delegate(DateTime?   a, DateTime?   b) { return  a < b; });
			if (t == typeof(TimeSpan?))   return (BoolRetOperation)(object)(Operator<TimeSpan?>.  BoolRetOperation)(delegate(TimeSpan?   a, TimeSpan?   b) { return  a < b; });

			// Sql types.
			//
			if (t == typeof(SqlString))   return (BoolRetOperation)(object)(Operator<SqlString>.  BoolRetOperation)(delegate(SqlString   a, SqlString   b) { return (a < b).IsTrue; });

			if (t == typeof(SqlInt16))    return (BoolRetOperation)(object)(Operator<SqlInt16>.   BoolRetOperation)(delegate(SqlInt16    a, SqlInt16    b) { return (a < b).IsTrue; });
			if (t == typeof(SqlInt32))    return (BoolRetOperation)(object)(Operator<SqlInt32>.   BoolRetOperation)(delegate(SqlInt32    a, SqlInt32    b) { return (a < b).IsTrue; });

			if (t == typeof(SqlByte))     return (BoolRetOperation)(object)(Operator<SqlByte>.    BoolRetOperation)(delegate(SqlByte     a, SqlByte     b) { return (a < b).IsTrue; });

			if (t == typeof(SqlSingle))   return (BoolRetOperation)(object)(Operator<SqlSingle>.  BoolRetOperation)(delegate(SqlSingle   a, SqlSingle   b) { return (a < b).IsTrue; });
			if (t == typeof(SqlDouble))   return (BoolRetOperation)(object)(Operator<SqlDouble>.  BoolRetOperation)(delegate(SqlDouble   a, SqlDouble   b) { return (a < b).IsTrue; });
			if (t == typeof(SqlDecimal))  return (BoolRetOperation)(object)(Operator<SqlDecimal>. BoolRetOperation)(delegate(SqlDecimal  a, SqlDecimal  b) { return (a < b).IsTrue; });
			if (t == typeof(SqlMoney))    return (BoolRetOperation)(object)(Operator<SqlMoney>.   BoolRetOperation)(delegate(SqlMoney    a, SqlMoney    b) { return (a < b).IsTrue; });

			if (t == typeof(SqlBoolean))  return (BoolRetOperation)(object)(Operator<SqlBoolean>. BoolRetOperation)(delegate(SqlBoolean  a, SqlBoolean  b) { return (a < b).IsTrue; });
			if (t == typeof(SqlBinary))   return (BoolRetOperation)(object)(Operator<SqlBinary>.  BoolRetOperation)(delegate(SqlBinary   a, SqlBinary   b) { return (a < b).IsTrue; });
			if (t == typeof(SqlDateTime)) return (BoolRetOperation)(object)(Operator<SqlDateTime>.BoolRetOperation)(delegate(SqlDateTime a, SqlDateTime b) { return (a < b).IsTrue; });
			if (t == typeof(SqlGuid))     return (BoolRetOperation)(object)(Operator<SqlGuid>.    BoolRetOperation)(delegate(SqlGuid     a, SqlGuid     b) { return (a < b).IsTrue; });

			return delegate(T a,T b) { throw new InvalidOperationException(); };
		}

		#endregion

		#region (a <= b) LessThanOrEqual 

		public  static BoolRetOperation LessThanOrEqual = GetLessThanOrEqualOperator();
		private static BoolRetOperation GetLessThanOrEqualOperator()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(SByte))       return (BoolRetOperation)(object)(Operator<SByte>.      BoolRetOperation)(delegate(SByte       a, SByte       b) { return  a <= b; });
			if (t == typeof(Int16))       return (BoolRetOperation)(object)(Operator<Int16>.      BoolRetOperation)(delegate(Int16       a, Int16       b) { return  a <= b; });
			if (t == typeof(Int32))       return (BoolRetOperation)(object)(Operator<Int32>.      BoolRetOperation)(delegate(Int32       a, Int32       b) { return  a <= b; });
			if (t == typeof(Int64))       return (BoolRetOperation)(object)(Operator<Int64>.      BoolRetOperation)(delegate(Int64       a, Int64       b) { return  a <= b; });

			if (t == typeof(Byte))        return (BoolRetOperation)(object)(Operator<Byte>.       BoolRetOperation)(delegate(Byte        a, Byte        b) { return  a <= b; });
			if (t == typeof(UInt16))      return (BoolRetOperation)(object)(Operator<UInt16>.     BoolRetOperation)(delegate(UInt16      a, UInt16      b) { return  a <= b; });
			if (t == typeof(UInt32))      return (BoolRetOperation)(object)(Operator<UInt32>.     BoolRetOperation)(delegate(UInt32      a, UInt32      b) { return  a <= b; });
			if (t == typeof(UInt64))      return (BoolRetOperation)(object)(Operator<UInt64>.     BoolRetOperation)(delegate(UInt64      a, UInt64      b) { return  a <= b; });

			if (t == typeof(Char))        return (BoolRetOperation)(object)(Operator<Char>.       BoolRetOperation)(delegate(Char        a, Char        b) { return  a <= b; });
			if (t == typeof(Single))      return (BoolRetOperation)(object)(Operator<Single>.     BoolRetOperation)(delegate(Single      a, Single      b) { return  a <= b; });
			if (t == typeof(Double))      return (BoolRetOperation)(object)(Operator<Double>.     BoolRetOperation)(delegate(Double      a, Double      b) { return  a <= b; });

			if (t == typeof(Decimal))     return (BoolRetOperation)(object)(Operator<Decimal>.    BoolRetOperation)(delegate(Decimal     a, Decimal     b) { return  a <= b; });
			if (t == typeof(DateTime))    return (BoolRetOperation)(object)(Operator<DateTime>.   BoolRetOperation)(delegate(DateTime    a, DateTime    b) { return  a <= b; });
			if (t == typeof(TimeSpan))    return (BoolRetOperation)(object)(Operator<TimeSpan>.   BoolRetOperation)(delegate(TimeSpan    a, TimeSpan    b) { return  a <= b; });

			// Nullable types.
			//
			if (t == typeof(SByte?))      return (BoolRetOperation)(object)(Operator<SByte?>.     BoolRetOperation)(delegate(SByte?      a, SByte?      b) { return  a <= b; });
			if (t == typeof(Int16?))      return (BoolRetOperation)(object)(Operator<Int16?>.     BoolRetOperation)(delegate(Int16?      a, Int16?      b) { return  a <= b; });
			if (t == typeof(Int32?))      return (BoolRetOperation)(object)(Operator<Int32?>.     BoolRetOperation)(delegate(Int32?      a, Int32?      b) { return  a <= b; });
			if (t == typeof(Int64?))      return (BoolRetOperation)(object)(Operator<Int64?>.     BoolRetOperation)(delegate(Int64?      a, Int64?      b) { return  a <= b; });

			if (t == typeof(Byte?))       return (BoolRetOperation)(object)(Operator<Byte?>.      BoolRetOperation)(delegate(Byte?       a, Byte?       b) { return  a <= b; });
			if (t == typeof(UInt16?))     return (BoolRetOperation)(object)(Operator<UInt16?>.    BoolRetOperation)(delegate(UInt16?     a, UInt16?     b) { return  a <= b; });
			if (t == typeof(UInt32?))     return (BoolRetOperation)(object)(Operator<UInt32?>.    BoolRetOperation)(delegate(UInt32?     a, UInt32?     b) { return  a <= b; });
			if (t == typeof(UInt64?))     return (BoolRetOperation)(object)(Operator<UInt64?>.    BoolRetOperation)(delegate(UInt64?     a, UInt64?     b) { return  a <= b; });

			if (t == typeof(Char?))       return (BoolRetOperation)(object)(Operator<Char?>.      BoolRetOperation)(delegate(Char?       a, Char?       b) { return  a <= b; });
			if (t == typeof(Single?))     return (BoolRetOperation)(object)(Operator<Single?>.    BoolRetOperation)(delegate(Single?     a, Single?     b) { return  a <= b; });
			if (t == typeof(Double?))     return (BoolRetOperation)(object)(Operator<Double?>.    BoolRetOperation)(delegate(Double?     a, Double?     b) { return  a <= b; });

			if (t == typeof(Decimal?))    return (BoolRetOperation)(object)(Operator<Decimal?>.   BoolRetOperation)(delegate(Decimal?    a, Decimal?    b) { return  a <= b; });
			if (t == typeof(DateTime?))   return (BoolRetOperation)(object)(Operator<DateTime?>.  BoolRetOperation)(delegate(DateTime?   a, DateTime?   b) { return  a <= b; });
			if (t == typeof(TimeSpan?))   return (BoolRetOperation)(object)(Operator<TimeSpan?>.  BoolRetOperation)(delegate(TimeSpan?   a, TimeSpan?   b) { return  a <= b; });

			// Sql types.
			//
			if (t == typeof(SqlString))   return (BoolRetOperation)(object)(Operator<SqlString>.  BoolRetOperation)(delegate(SqlString   a, SqlString   b) { return (a <= b).IsTrue; });

			if (t == typeof(SqlInt16))    return (BoolRetOperation)(object)(Operator<SqlInt16>.   BoolRetOperation)(delegate(SqlInt16    a, SqlInt16    b) { return (a <= b).IsTrue; });
			if (t == typeof(SqlInt32))    return (BoolRetOperation)(object)(Operator<SqlInt32>.   BoolRetOperation)(delegate(SqlInt32    a, SqlInt32    b) { return (a <= b).IsTrue; });

			if (t == typeof(SqlByte))     return (BoolRetOperation)(object)(Operator<SqlByte>.    BoolRetOperation)(delegate(SqlByte     a, SqlByte     b) { return (a <= b).IsTrue; });

			if (t == typeof(SqlSingle))   return (BoolRetOperation)(object)(Operator<SqlSingle>.  BoolRetOperation)(delegate(SqlSingle   a, SqlSingle   b) { return (a <= b).IsTrue; });
			if (t == typeof(SqlDouble))   return (BoolRetOperation)(object)(Operator<SqlDouble>.  BoolRetOperation)(delegate(SqlDouble   a, SqlDouble   b) { return (a <= b).IsTrue; });
			if (t == typeof(SqlDecimal))  return (BoolRetOperation)(object)(Operator<SqlDecimal>. BoolRetOperation)(delegate(SqlDecimal  a, SqlDecimal  b) { return (a <= b).IsTrue; });
			if (t == typeof(SqlMoney))    return (BoolRetOperation)(object)(Operator<SqlMoney>.   BoolRetOperation)(delegate(SqlMoney    a, SqlMoney    b) { return (a <= b).IsTrue; });

			if (t == typeof(SqlBoolean))  return (BoolRetOperation)(object)(Operator<SqlBoolean>. BoolRetOperation)(delegate(SqlBoolean  a, SqlBoolean  b) { return (a <= b).IsTrue; });
			if (t == typeof(SqlBinary))   return (BoolRetOperation)(object)(Operator<SqlBinary>.  BoolRetOperation)(delegate(SqlBinary   a, SqlBinary   b) { return (a <= b).IsTrue; });
			if (t == typeof(SqlDateTime)) return (BoolRetOperation)(object)(Operator<SqlDateTime>.BoolRetOperation)(delegate(SqlDateTime a, SqlDateTime b) { return (a <= b).IsTrue; });
			if (t == typeof(SqlGuid))     return (BoolRetOperation)(object)(Operator<SqlGuid>.    BoolRetOperation)(delegate(SqlGuid     a, SqlGuid     b) { return (a <= b).IsTrue; });

			return delegate(T a,T b) { throw new InvalidOperationException(); };
		}

		#endregion
	}

	public static class Operator
	{
		public static T    Addition          <T>(T a, T b) { return Operator<T>.Addition          (a, b); }
		public static T    Subtraction       <T>(T a, T b) { return Operator<T>.Subtraction       (a, b); }
		public static T    Multiply          <T>(T a, T b) { return Operator<T>.Multiply          (a, b); }
		public static T    Division          <T>(T a, T b) { return Operator<T>.Division          (a, b); }
		public static T    Modulus           <T>(T a, T b) { return Operator<T>.Modulus           (a, b); }

		public static T    BitwiseAnd        <T>(T a, T b) { return Operator<T>.BitwiseAnd        (a, b); }
		public static T    BitwiseOr         <T>(T a, T b) { return Operator<T>.BitwiseOr         (a, b); }
		public static T    ExclusiveOr       <T>(T a, T b) { return Operator<T>.ExclusiveOr       (a, b); }

		public static T    UnaryNegation     <T>(T a)      { return Operator<T>.UnaryNegation     (a);    }
		public static T    OnesComplement    <T>(T a)      { return Operator<T>.OnesComplement    (a);    }

		public static bool Equality          <T>(T a, T b) { return Operator<T>.Equality          (a, b); }
		public static bool Inequality        <T>(T a, T b) { return Operator<T>.Inequality        (a, b); }
		public static bool GreaterThan       <T>(T a, T b) { return Operator<T>.GreaterThan       (a, b); }
		public static bool GreaterThanOrEqual<T>(T a, T b) { return Operator<T>.GreaterThanOrEqual(a, b); }
		public static bool LessThan          <T>(T a, T b) { return Operator<T>.LessThan          (a, b); }
		public static bool LessThanOrEqual   <T>(T a, T b) { return Operator<T>.LessThanOrEqual   (a, b); }
	}
}
