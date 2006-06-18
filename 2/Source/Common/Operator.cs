using System;
using System.Data.SqlTypes;

namespace BLToolkit.Common
{
	public static class Operator<T>
	{
		public delegate T    TwoArgOperation (T op1, T op2);
		public delegate bool BoolRetOperation(T op1, T op2);
		public delegate T    OneArgOperation (T op);

		private static TwoArgOperation Conv<V>(Operator<V>.TwoArgOperation op)
		{
			return (TwoArgOperation)(object)op;
		}

		private static BoolRetOperation BConv<V>(Operator<V>.BoolRetOperation op)
		{
			return (BoolRetOperation)(object)op;
		}

		private static OneArgOperation OConv<V>(Operator<V>.OneArgOperation op)
		{
			return (OneArgOperation)(object)op;
		}

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
			if (t == typeof(SByte))      return Conv<SByte>     (delegate(SByte      a, SByte      b) { return (SByte)  (a - b); });
			if (t == typeof(Int16))      return Conv<Int16>     (delegate(Int16      a, Int16      b) { return (Int16)  (a - b); });
			if (t == typeof(Int32))      return Conv<Int32>     (delegate(Int32      a, Int32      b) { return           a - b;  });
			if (t == typeof(Int64))      return Conv<Int64>     (delegate(Int64      a, Int64      b) { return           a - b;  });

			if (t == typeof(Byte))       return Conv<Byte>      (delegate(Byte       a, Byte       b) { return (Byte)   (a - b); });
			if (t == typeof(UInt16))     return Conv<UInt16>    (delegate(UInt16     a, UInt16     b) { return (UInt16) (a - b); });
			if (t == typeof(UInt32))     return Conv<UInt32>    (delegate(UInt32     a, UInt32     b) { return           a - b;  });
			if (t == typeof(UInt64))     return Conv<UInt64>    (delegate(UInt64     a, UInt64     b) { return           a - b;  });

			if (t == typeof(Char))       return Conv<Char>      (delegate(Char       a, Char       b) { return (Char)   (a - b); });
			if (t == typeof(Single))     return Conv<Single>    (delegate(Single     a, Single     b) { return           a - b;  });
			if (t == typeof(Double))     return Conv<Double>    (delegate(Double     a, Double     b) { return           a - b;  });

			if (t == typeof(Decimal))    return Conv<Decimal>   (delegate(Decimal    a, Decimal    b) { return           a - b;  });
			if (t == typeof(TimeSpan))   return Conv<TimeSpan>  (delegate(TimeSpan   a, TimeSpan   b) { return           a - b;  });

			// Nullable types.
			//
			if (t == typeof(SByte?))     return Conv<SByte?>    (delegate(SByte?     a, SByte?     b) { return (SByte?) (a - b); });
			if (t == typeof(Int16?))     return Conv<Int16?>    (delegate(Int16?     a, Int16?     b) { return (Int16?) (a - b); });
			if (t == typeof(Int32?))     return Conv<Int32?>    (delegate(Int32?     a, Int32?     b) { return           a - b;  });
			if (t == typeof(Int64?))     return Conv<Int64?>    (delegate(Int64?     a, Int64?     b) { return           a - b;  });

			if (t == typeof(Byte?))      return Conv<Byte?>     (delegate(Byte?      a, Byte?      b) { return (Byte?)  (a - b); });
			if (t == typeof(UInt16?))    return Conv<UInt16?>   (delegate(UInt16?    a, UInt16?    b) { return (UInt16?)(a - b); });
			if (t == typeof(UInt32?))    return Conv<UInt32?>   (delegate(UInt32?    a, UInt32?    b) { return           a - b;  });
			if (t == typeof(UInt64?))    return Conv<UInt64?>   (delegate(UInt64?    a, UInt64?    b) { return           a - b;  });

			if (t == typeof(Char?))      return Conv<Char?>     (delegate(Char?      a, Char?      b) { return (Char?)  (a - b); });
			if (t == typeof(Single?))    return Conv<Single?>   (delegate(Single?    a, Single?    b) { return           a - b;  });
			if (t == typeof(Double?))    return Conv<Double?>   (delegate(Double?    a, Double?    b) { return           a - b;  });

			if (t == typeof(Decimal?))   return Conv<Decimal?>  (delegate(Decimal?   a, Decimal?   b) { return           a - b;  });
			if (t == typeof(TimeSpan?))  return Conv<TimeSpan?> (delegate(TimeSpan?  a, TimeSpan?  b) { return           a - b;  });

			// Sql types.
			//
			if (t == typeof(SqlInt16))   return Conv<SqlInt16>  (delegate(SqlInt16   a, SqlInt16   b) { return          a - b;  });
			if (t == typeof(SqlInt32))   return Conv<SqlInt32>  (delegate(SqlInt32   a, SqlInt32   b) { return          a - b;  });
			if (t == typeof(SqlInt64))   return Conv<SqlInt64>  (delegate(SqlInt64   a, SqlInt64   b) { return          a - b;  });

			if (t == typeof(SqlByte))    return Conv<SqlByte>   (delegate(SqlByte    a, SqlByte    b) { return          a - b;  });

			if (t == typeof(SqlSingle))  return Conv<SqlSingle> (delegate(SqlSingle  a, SqlSingle  b) { return          a - b;  });
			if (t == typeof(SqlDouble))  return Conv<SqlDouble> (delegate(SqlDouble  a, SqlDouble  b) { return          a - b;  });
			if (t == typeof(SqlDecimal)) return Conv<SqlDecimal>(delegate(SqlDecimal a, SqlDecimal b) { return          a - b;  });
			if (t == typeof(SqlMoney))   return Conv<SqlMoney>  (delegate(SqlMoney   a, SqlMoney   b) { return          a - b;  });

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
			if (t == typeof(SByte))      return Conv<SByte>     (delegate(SByte      a, SByte      b) { return (SByte)  (a * b); });
			if (t == typeof(Int16))      return Conv<Int16>     (delegate(Int16      a, Int16      b) { return (Int16)  (a * b); });
			if (t == typeof(Int32))      return Conv<Int32>     (delegate(Int32      a, Int32      b) { return           a * b;  });
			if (t == typeof(Int64))      return Conv<Int64>     (delegate(Int64      a, Int64      b) { return           a * b;  });

			if (t == typeof(Byte))       return Conv<Byte>      (delegate(Byte       a, Byte       b) { return (Byte)   (a * b); });
			if (t == typeof(UInt16))     return Conv<UInt16>    (delegate(UInt16     a, UInt16     b) { return (UInt16) (a * b); });
			if (t == typeof(UInt32))     return Conv<UInt32>    (delegate(UInt32     a, UInt32     b) { return           a * b;  });
			if (t == typeof(UInt64))     return Conv<UInt64>    (delegate(UInt64     a, UInt64     b) { return           a * b;  });

			if (t == typeof(Char))       return Conv<Char>      (delegate(Char       a, Char       b) { return (Char)   (a * b); });
			if (t == typeof(Single))     return Conv<Single>    (delegate(Single     a, Single     b) { return           a * b;  });
			if (t == typeof(Double))     return Conv<Double>    (delegate(Double     a, Double     b) { return           a * b;  });

			if (t == typeof(Decimal))    return Conv<Decimal>   (delegate(Decimal    a, Decimal    b) { return           a * b;  });

			// Nullable types.
			//
			if (t == typeof(SByte?))     return Conv<SByte?>    (delegate(SByte?     a, SByte?     b) { return (SByte?) (a * b); });
			if (t == typeof(Int16?))     return Conv<Int16?>    (delegate(Int16?     a, Int16?     b) { return (Int16?) (a * b); });
			if (t == typeof(Int32?))     return Conv<Int32?>    (delegate(Int32?     a, Int32?     b) { return           a * b;  });
			if (t == typeof(Int64?))     return Conv<Int64?>    (delegate(Int64?     a, Int64?     b) { return           a * b;  });

			if (t == typeof(Byte?))      return Conv<Byte?>     (delegate(Byte?      a, Byte?      b) { return (Byte?)  (a * b); });
			if (t == typeof(UInt16?))    return Conv<UInt16?>   (delegate(UInt16?    a, UInt16?    b) { return (UInt16?)(a * b); });
			if (t == typeof(UInt32?))    return Conv<UInt32?>   (delegate(UInt32?    a, UInt32?    b) { return           a * b;  });
			if (t == typeof(UInt64?))    return Conv<UInt64?>   (delegate(UInt64?    a, UInt64?    b) { return           a * b;  });

			if (t == typeof(Char?))      return Conv<Char?>     (delegate(Char?      a, Char?      b) { return (Char?)  (a * b); });
			if (t == typeof(Single?))    return Conv<Single?>   (delegate(Single?    a, Single?    b) { return           a * b;  });
			if (t == typeof(Double?))    return Conv<Double?>   (delegate(Double?    a, Double?    b) { return           a * b;  });

			if (t == typeof(Decimal?))   return Conv<Decimal?>  (delegate(Decimal?   a, Decimal?   b) { return           a * b;  });

			// Sql types.
			//
			if (t == typeof(SqlInt16))   return Conv<SqlInt16>  (delegate(SqlInt16   a, SqlInt16   b) { return           a * b;  });
			if (t == typeof(SqlInt32))   return Conv<SqlInt32>  (delegate(SqlInt32   a, SqlInt32   b) { return           a * b;  });
			if (t == typeof(SqlInt64))   return Conv<SqlInt64>  (delegate(SqlInt64   a, SqlInt64   b) { return           a * b;  });

			if (t == typeof(SqlByte))    return Conv<SqlByte>   (delegate(SqlByte    a, SqlByte    b) { return           a * b;  });

			if (t == typeof(SqlSingle))  return Conv<SqlSingle> (delegate(SqlSingle  a, SqlSingle  b) { return           a * b;  });
			if (t == typeof(SqlDouble))  return Conv<SqlDouble> (delegate(SqlDouble  a, SqlDouble  b) { return           a * b;  });
			if (t == typeof(SqlDecimal)) return Conv<SqlDecimal>(delegate(SqlDecimal a, SqlDecimal b) { return           a * b;  });
			if (t == typeof(SqlMoney))   return Conv<SqlMoney>  (delegate(SqlMoney   a, SqlMoney   b) { return           a * b;  });

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
			if (t == typeof(SByte))      return Conv<SByte>     (delegate(SByte      a, SByte      b) { return (SByte)  (a / b); });
			if (t == typeof(Int16))      return Conv<Int16>     (delegate(Int16      a, Int16      b) { return (Int16)  (a / b); });
			if (t == typeof(Int32))      return Conv<Int32>     (delegate(Int32      a, Int32      b) { return           a / b;  });
			if (t == typeof(Int64))      return Conv<Int64>     (delegate(Int64      a, Int64      b) { return           a / b;  });

			if (t == typeof(Byte))       return Conv<Byte>      (delegate(Byte       a, Byte       b) { return (Byte)   (a / b); });
			if (t == typeof(UInt16))     return Conv<UInt16>    (delegate(UInt16     a, UInt16     b) { return (UInt16) (a / b); });
			if (t == typeof(UInt32))     return Conv<UInt32>    (delegate(UInt32     a, UInt32     b) { return           a / b;  });
			if (t == typeof(UInt64))     return Conv<UInt64>    (delegate(UInt64     a, UInt64     b) { return           a / b;  });

			if (t == typeof(Char))       return Conv<Char>      (delegate(Char       a, Char       b) { return (Char)   (a / b); });
			if (t == typeof(Single))     return Conv<Single>    (delegate(Single     a, Single     b) { return           a / b;  });
			if (t == typeof(Double))     return Conv<Double>    (delegate(Double     a, Double     b) { return           a / b;  });

			if (t == typeof(Decimal))    return Conv<Decimal>   (delegate(Decimal    a, Decimal    b) { return           a / b;  });

			// Nullable types.
			//
			if (t == typeof(SByte?))     return Conv<SByte?>    (delegate(SByte?     a, SByte?     b) { return (SByte?) (a / b); });
			if (t == typeof(Int16?))     return Conv<Int16?>    (delegate(Int16?     a, Int16?     b) { return (Int16?) (a / b); });
			if (t == typeof(Int32?))     return Conv<Int32?>    (delegate(Int32?     a, Int32?     b) { return           a / b;  });
			if (t == typeof(Int64?))     return Conv<Int64?>    (delegate(Int64?     a, Int64?     b) { return           a / b;  });

			if (t == typeof(Byte?))      return Conv<Byte?>     (delegate(Byte?      a, Byte?      b) { return (Byte?)  (a / b); });
			if (t == typeof(UInt16?))    return Conv<UInt16?>   (delegate(UInt16?    a, UInt16?    b) { return (UInt16?)(a / b); });
			if (t == typeof(UInt32?))    return Conv<UInt32?>   (delegate(UInt32?    a, UInt32?    b) { return           a / b;  });
			if (t == typeof(UInt64?))    return Conv<UInt64?>   (delegate(UInt64?    a, UInt64?    b) { return           a / b;  });

			if (t == typeof(Char?))      return Conv<Char?>     (delegate(Char?      a, Char?      b) { return (Char?)  (a / b); });
			if (t == typeof(Single?))    return Conv<Single?>   (delegate(Single?    a, Single?    b) { return           a / b;  });
			if (t == typeof(Double?))    return Conv<Double?>   (delegate(Double?    a, Double?    b) { return           a / b;  });

			if (t == typeof(Decimal?))   return Conv<Decimal?>  (delegate(Decimal?   a, Decimal?   b) { return           a / b;  });

			// Sql types.
			//
			if (t == typeof(SqlInt16))   return Conv<SqlInt16>  (delegate(SqlInt16   a, SqlInt16   b) { return           a / b;  });
			if (t == typeof(SqlInt32))   return Conv<SqlInt32>  (delegate(SqlInt32   a, SqlInt32   b) { return           a / b;  });
			if (t == typeof(SqlInt64))   return Conv<SqlInt64>  (delegate(SqlInt64   a, SqlInt64   b) { return           a / b;  });

			if (t == typeof(SqlByte))    return Conv<SqlByte>   (delegate(SqlByte    a, SqlByte    b) { return           a / b;  });

			if (t == typeof(SqlSingle))  return Conv<SqlSingle> (delegate(SqlSingle  a, SqlSingle  b) { return           a / b;  });
			if (t == typeof(SqlDouble))  return Conv<SqlDouble> (delegate(SqlDouble  a, SqlDouble  b) { return           a / b;  });
			if (t == typeof(SqlDecimal)) return Conv<SqlDecimal>(delegate(SqlDecimal a, SqlDecimal b) { return           a / b;  });
			if (t == typeof(SqlMoney))   return Conv<SqlMoney>  (delegate(SqlMoney   a, SqlMoney   b) { return           a / b;  });

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
			if (t == typeof(SByte))      return Conv<SByte>     (delegate(SByte      a, SByte      b) { return (SByte)  (a % b); });
			if (t == typeof(Int16))      return Conv<Int16>     (delegate(Int16      a, Int16      b) { return (Int16)  (a % b); });
			if (t == typeof(Int32))      return Conv<Int32>     (delegate(Int32      a, Int32      b) { return           a % b;  });
			if (t == typeof(Int64))      return Conv<Int64>     (delegate(Int64      a, Int64      b) { return           a % b;  });

			if (t == typeof(Byte))       return Conv<Byte>      (delegate(Byte       a, Byte       b) { return (Byte)   (a % b); });
			if (t == typeof(UInt16))     return Conv<UInt16>    (delegate(UInt16     a, UInt16     b) { return (UInt16) (a % b); });
			if (t == typeof(UInt32))     return Conv<UInt32>    (delegate(UInt32     a, UInt32     b) { return           a % b;  });
			if (t == typeof(UInt64))     return Conv<UInt64>    (delegate(UInt64     a, UInt64     b) { return           a % b;  });

			if (t == typeof(Char))       return Conv<Char>      (delegate(Char       a, Char       b) { return (Char)   (a % b); });
			if (t == typeof(Single))     return Conv<Single>    (delegate(Single     a, Single     b) { return           a % b;  });
			if (t == typeof(Double))     return Conv<Double>    (delegate(Double     a, Double     b) { return           a % b;  });

			if (t == typeof(Decimal))    return Conv<Decimal>   (delegate(Decimal    a, Decimal    b) { return           a % b;  });

			// Nullable types.
			//
			if (t == typeof(SByte?))     return Conv<SByte?>    (delegate(SByte?     a, SByte?     b) { return (SByte?) (a % b); });
			if (t == typeof(Int16?))     return Conv<Int16?>    (delegate(Int16?     a, Int16?     b) { return (Int16?) (a % b); });
			if (t == typeof(Int32?))     return Conv<Int32?>    (delegate(Int32?     a, Int32?     b) { return           a % b;  });
			if (t == typeof(Int64?))     return Conv<Int64?>    (delegate(Int64?     a, Int64?     b) { return           a % b;  });

			if (t == typeof(Byte?))      return Conv<Byte?>     (delegate(Byte?      a, Byte?      b) { return (Byte?)  (a % b); });
			if (t == typeof(UInt16?))    return Conv<UInt16?>   (delegate(UInt16?    a, UInt16?    b) { return (UInt16?)(a % b); });
			if (t == typeof(UInt32?))    return Conv<UInt32?>   (delegate(UInt32?    a, UInt32?    b) { return           a % b;  });
			if (t == typeof(UInt64?))    return Conv<UInt64?>   (delegate(UInt64?    a, UInt64?    b) { return           a % b;  });

			if (t == typeof(Char?))      return Conv<Char?>     (delegate(Char?      a, Char?      b) { return (Char?)  (a % b); });
			if (t == typeof(Single?))    return Conv<Single?>   (delegate(Single?    a, Single?    b) { return           a % b;  });
			if (t == typeof(Double?))    return Conv<Double?>   (delegate(Double?    a, Double?    b) { return           a % b;  });

			if (t == typeof(Decimal?))   return Conv<Decimal?>  (delegate(Decimal?   a, Decimal?   b) { return           a % b;  });

			// Sql types.
			//
			if (t == typeof(SqlInt16))   return Conv<SqlInt16>  (delegate(SqlInt16   a, SqlInt16   b) { return           a % b;  });
			if (t == typeof(SqlInt32))   return Conv<SqlInt32>  (delegate(SqlInt32   a, SqlInt32   b) { return           a % b;  });
			if (t == typeof(SqlInt64))   return Conv<SqlInt64>  (delegate(SqlInt64   a, SqlInt64   b) { return           a % b;  });

			if (t == typeof(SqlByte))    return Conv<SqlByte>   (delegate(SqlByte    a, SqlByte    b) { return           a % b;  });

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
			if (t == typeof(SByte))      return Conv<SByte>     (delegate(SByte      a, SByte      b) { return (SByte)  (a & b); });
			if (t == typeof(Int16))      return Conv<Int16>     (delegate(Int16      a, Int16      b) { return (Int16)  (a & b); });
			if (t == typeof(Int32))      return Conv<Int32>     (delegate(Int32      a, Int32      b) { return           a & b;  });
			if (t == typeof(Int64))      return Conv<Int64>     (delegate(Int64      a, Int64      b) { return           a & b;  });

			if (t == typeof(Byte))       return Conv<Byte>      (delegate(Byte       a, Byte       b) { return (Byte)   (a & b); });
			if (t == typeof(UInt16))     return Conv<UInt16>    (delegate(UInt16     a, UInt16     b) { return (UInt16) (a & b); });
			if (t == typeof(UInt32))     return Conv<UInt32>    (delegate(UInt32     a, UInt32     b) { return           a & b;  });
			if (t == typeof(UInt64))     return Conv<UInt64>    (delegate(UInt64     a, UInt64     b) { return           a & b;  });

			if (t == typeof(Char))       return Conv<Char>      (delegate(Char       a, Char       b) { return (Char)   (a & b); });

			// Nullable types.
			//
			if (t == typeof(SByte?))     return Conv<SByte?>    (delegate(SByte?     a, SByte?     b) { return (SByte?) (a & b); });
			if (t == typeof(Int16?))     return Conv<Int16?>    (delegate(Int16?     a, Int16?     b) { return (Int16?) (a & b); });
			if (t == typeof(Int32?))     return Conv<Int32?>    (delegate(Int32?     a, Int32?     b) { return           a & b;  });
			if (t == typeof(Int64?))     return Conv<Int64?>    (delegate(Int64?     a, Int64?     b) { return           a & b;  });

			if (t == typeof(Byte?))      return Conv<Byte?>     (delegate(Byte?      a, Byte?      b) { return (Byte?)  (a & b); });
			if (t == typeof(UInt16?))    return Conv<UInt16?>   (delegate(UInt16?    a, UInt16?    b) { return (UInt16?)(a & b); });
			if (t == typeof(UInt32?))    return Conv<UInt32?>   (delegate(UInt32?    a, UInt32?    b) { return           a & b;  });
			if (t == typeof(UInt64?))    return Conv<UInt64?>   (delegate(UInt64?    a, UInt64?    b) { return           a & b;  });

			if (t == typeof(Char?))      return Conv<Char?>     (delegate(Char?      a, Char?      b) { return (Char?)  (a & b); });

			// Sql types.
			//
			if (t == typeof(SqlInt16))   return Conv<SqlInt16>  (delegate(SqlInt16   a, SqlInt16   b) { return           a & b;  });
			if (t == typeof(SqlInt32))   return Conv<SqlInt32>  (delegate(SqlInt32   a, SqlInt32   b) { return           a & b;  });
			if (t == typeof(SqlInt64))   return Conv<SqlInt64>  (delegate(SqlInt64   a, SqlInt64   b) { return           a & b;  });

			if (t == typeof(SqlByte))    return Conv<SqlByte>   (delegate(SqlByte    a, SqlByte    b) { return           a & b;  });

			if (t == typeof(SqlBoolean)) return Conv<SqlBoolean>(delegate(SqlBoolean a, SqlBoolean b) { return           a & b;  });

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
			if (t == typeof(SByte))      return Conv<SByte>     (delegate(SByte      a, SByte      b) { return (SByte)  (a | b); });
			if (t == typeof(Int16))      return Conv<Int16>     (delegate(Int16      a, Int16      b) { return (Int16)  (a | b); });
			if (t == typeof(Int32))      return Conv<Int32>     (delegate(Int32      a, Int32      b) { return           a | b;  });
			if (t == typeof(Int64))      return Conv<Int64>     (delegate(Int64      a, Int64      b) { return           a | b;  });

			if (t == typeof(Byte))       return Conv<Byte>      (delegate(Byte       a, Byte       b) { return (Byte)   (a | b); });
			if (t == typeof(UInt16))     return Conv<UInt16>    (delegate(UInt16     a, UInt16     b) { return (UInt16) (a | b); });
			if (t == typeof(UInt32))     return Conv<UInt32>    (delegate(UInt32     a, UInt32     b) { return           a | b;  });
			if (t == typeof(UInt64))     return Conv<UInt64>    (delegate(UInt64     a, UInt64     b) { return           a | b;  });

			if (t == typeof(Char))       return Conv<Char>      (delegate(Char       a, Char       b) { return (Char)   (a | b); });

			// Nullable types.
			//
			if (t == typeof(SByte?))     return Conv<SByte?>    (delegate(SByte?     a, SByte?     b) { return (SByte?) (a | b); });
			if (t == typeof(Int16?))     return Conv<Int16?>    (delegate(Int16?     a, Int16?     b) { return (Int16?) (a | b); });
			if (t == typeof(Int32?))     return Conv<Int32?>    (delegate(Int32?     a, Int32?     b) { return           a | b;  });
			if (t == typeof(Int64?))     return Conv<Int64?>    (delegate(Int64?     a, Int64?     b) { return           a | b;  });

			if (t == typeof(Byte?))      return Conv<Byte?>     (delegate(Byte?      a, Byte?      b) { return (Byte?)  (a | b); });
			if (t == typeof(UInt16?))    return Conv<UInt16?>   (delegate(UInt16?    a, UInt16?    b) { return (UInt16?)(a | b); });
			if (t == typeof(UInt32?))    return Conv<UInt32?>   (delegate(UInt32?    a, UInt32?    b) { return           a | b;  });
			if (t == typeof(UInt64?))    return Conv<UInt64?>   (delegate(UInt64?    a, UInt64?    b) { return           a | b;  });

			if (t == typeof(Char?))      return Conv<Char?>     (delegate(Char?      a, Char?      b) { return (Char?)  (a | b); });

			// Sql types.
			//
			if (t == typeof(SqlInt16))   return Conv<SqlInt16>  (delegate(SqlInt16   a, SqlInt16   b) { return           a | b;  });
			if (t == typeof(SqlInt32))   return Conv<SqlInt32>  (delegate(SqlInt32   a, SqlInt32   b) { return           a | b;  });
			if (t == typeof(SqlInt64))   return Conv<SqlInt64>  (delegate(SqlInt64   a, SqlInt64   b) { return           a | b;  });

			if (t == typeof(SqlByte))    return Conv<SqlByte>   (delegate(SqlByte    a, SqlByte    b) { return           a | b;  });

			if (t == typeof(SqlBoolean)) return Conv<SqlBoolean>(delegate(SqlBoolean a, SqlBoolean b) { return           a | b;  });

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
			if (t == typeof(SByte))      return Conv<SByte>     (delegate(SByte      a, SByte      b) { return (SByte)  (a ^ b); });
			if (t == typeof(Int16))      return Conv<Int16>     (delegate(Int16      a, Int16      b) { return (Int16)  (a ^ b); });
			if (t == typeof(Int32))      return Conv<Int32>     (delegate(Int32      a, Int32      b) { return           a ^ b;  });
			if (t == typeof(Int64))      return Conv<Int64>     (delegate(Int64      a, Int64      b) { return           a ^ b;  });

			if (t == typeof(Byte))       return Conv<Byte>      (delegate(Byte       a, Byte       b) { return (Byte)   (a ^ b); });
			if (t == typeof(UInt16))     return Conv<UInt16>    (delegate(UInt16     a, UInt16     b) { return (UInt16) (a ^ b); });
			if (t == typeof(UInt32))     return Conv<UInt32>    (delegate(UInt32     a, UInt32     b) { return           a ^ b;  });
			if (t == typeof(UInt64))     return Conv<UInt64>    (delegate(UInt64     a, UInt64     b) { return           a ^ b;  });

			if (t == typeof(Char))       return Conv<Char>      (delegate(Char       a, Char       b) { return (Char)   (a ^ b); });

			// Nullable types.
			//
			if (t == typeof(SByte?))     return Conv<SByte?>    (delegate(SByte?     a, SByte?     b) { return (SByte?) (a ^ b); });
			if (t == typeof(Int16?))     return Conv<Int16?>    (delegate(Int16?     a, Int16?     b) { return (Int16?) (a ^ b); });
			if (t == typeof(Int32?))     return Conv<Int32?>    (delegate(Int32?     a, Int32?     b) { return           a ^ b;  });
			if (t == typeof(Int64?))     return Conv<Int64?>    (delegate(Int64?     a, Int64?     b) { return           a ^ b;  });

			if (t == typeof(Byte?))      return Conv<Byte?>     (delegate(Byte?      a, Byte?      b) { return (Byte?)  (a ^ b); });
			if (t == typeof(UInt16?))    return Conv<UInt16?>   (delegate(UInt16?    a, UInt16?    b) { return (UInt16?)(a ^ b); });
			if (t == typeof(UInt32?))    return Conv<UInt32?>   (delegate(UInt32?    a, UInt32?    b) { return           a ^ b;  });
			if (t == typeof(UInt64?))    return Conv<UInt64?>   (delegate(UInt64?    a, UInt64?    b) { return           a ^ b;  });

			if (t == typeof(Char?))      return Conv<Char?>     (delegate(Char?      a, Char?      b) { return (Char?)  (a ^ b); });

			// Sql types.
			//
			if (t == typeof(SqlInt16))   return Conv<SqlInt16>  (delegate(SqlInt16   a, SqlInt16   b) { return           a | b;  });
			if (t == typeof(SqlInt32))   return Conv<SqlInt32>  (delegate(SqlInt32   a, SqlInt32   b) { return           a | b;  });
			if (t == typeof(SqlInt64))   return Conv<SqlInt64>  (delegate(SqlInt64   a, SqlInt64   b) { return           a | b;  });

			if (t == typeof(SqlByte))    return Conv<SqlByte>   (delegate(SqlByte    a, SqlByte    b) { return           a | b;  });

			if (t == typeof(SqlBoolean)) return Conv<SqlBoolean>(delegate(SqlBoolean a, SqlBoolean b) { return           a | b;  });

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
			if (t == typeof(SByte))    return OConv<SByte>   (delegate(SByte    a) { return (SByte) (-a); });
			if (t == typeof(Int16))    return OConv<Int16>   (delegate(Int16    a) { return (Int16) (-a); });
			if (t == typeof(Int32))    return OConv<Int32>   (delegate(Int32    a) { return          -a;  });
			if (t == typeof(Int64))    return OConv<Int64>   (delegate(Int64    a) { return          -a;  });

			// Nullable types.
			//
			if (t == typeof(SByte?))   return OConv<SByte?>  (delegate(SByte?   a) { return (SByte?) -a;  });
			if (t == typeof(Int16?))   return OConv<Int16?>  (delegate(Int16?   a) { return (Int16?) -a;  });
			if (t == typeof(Int32?))   return OConv<Int32?>  (delegate(Int32?   a) { return          -a;  });
			if (t == typeof(Int64?))   return OConv<Int64?>  (delegate(Int64?   a) { return          -a;  });

			// Sql types.
			//
			if (t == typeof(SqlInt16)) return OConv<SqlInt16>(delegate(SqlInt16 a) { return          -a;  });
			if (t == typeof(SqlInt32)) return OConv<SqlInt32>(delegate(SqlInt32 a) { return          -a;  });
			if (t == typeof(SqlInt64)) return OConv<SqlInt64>(delegate(SqlInt64 a) { return          -a;  });

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
			if (t == typeof(SByte))      return OConv<SByte>     (delegate(SByte      a) { return (SByte)  ~a; });
			if (t == typeof(Int16))      return OConv<Int16>     (delegate(Int16      a) { return (Int16)  ~a; });
			if (t == typeof(Int32))      return OConv<Int32>     (delegate(Int32      a) { return          ~a; });
			if (t == typeof(Int64))      return OConv<Int64>     (delegate(Int64      a) { return          ~a; });

			if (t == typeof(Byte))       return OConv<Byte>      (delegate(Byte       a) { return (Byte)   ~a; });
			if (t == typeof(UInt16))     return OConv<UInt16>    (delegate(UInt16     a) { return (UInt16) ~a; });
			if (t == typeof(UInt32))     return OConv<UInt32>    (delegate(UInt32     a) { return          ~a; });
			if (t == typeof(UInt64))     return OConv<UInt64>    (delegate(UInt64     a) { return          ~a; });

			if (t == typeof(Char))       return OConv<Char>      (delegate(Char       a) { return (Char)   ~a; });

			// Nullable types.
			//
			if (t == typeof(SByte?))     return OConv<SByte?>    (delegate(SByte?     a) { return (SByte?) ~a; });
			if (t == typeof(Int16?))     return OConv<Int16?>    (delegate(Int16?     a) { return (Int16?) ~a; });
			if (t == typeof(Int32?))     return OConv<Int32?>    (delegate(Int32?     a) { return          ~a; });
			if (t == typeof(Int64?))     return OConv<Int64?>    (delegate(Int64?     a) { return          ~a; });

			if (t == typeof(Byte?))      return OConv<Byte?>     (delegate(Byte?      a) { return (Byte?)  ~a; });
			if (t == typeof(UInt16?))    return OConv<UInt16?>   (delegate(UInt16?    a) { return (UInt16?)~a; });
			if (t == typeof(UInt32?))    return OConv<UInt32?>   (delegate(UInt32?    a) { return          ~a; });
			if (t == typeof(UInt64?))    return OConv<UInt64?>   (delegate(UInt64?    a) { return          ~a; });

			if (t == typeof(Char?))      return OConv<Char?>     (delegate(Char?      a) { return (Char?)  ~a; });

			// Sql types.
			//
			if (t == typeof(SqlInt16))   return OConv<SqlInt16>  (delegate(SqlInt16   a) { return          ~a; });
			if (t == typeof(SqlInt32))   return OConv<SqlInt32>  (delegate(SqlInt32   a) { return          ~a; });
			if (t == typeof(SqlInt64))   return OConv<SqlInt64>  (delegate(SqlInt64   a) { return          ~a; });

			if (t == typeof(SqlByte))    return OConv<SqlByte>   (delegate(SqlByte    a) { return          ~a; });

			if (t == typeof(SqlBoolean)) return OConv<SqlBoolean>(delegate(SqlBoolean a) { return          ~a; });

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
			if (t == typeof(String))      return BConv<String>     (delegate(String      a, String      b) { return  a == b; });

			if (t == typeof(SByte))       return BConv<SByte>      (delegate(SByte       a, SByte       b) { return  a == b; });
			if (t == typeof(Int16))       return BConv<Int16>      (delegate(Int16       a, Int16       b) { return  a == b; });
			if (t == typeof(Int32))       return BConv<Int32>      (delegate(Int32       a, Int32       b) { return  a == b; });
			if (t == typeof(Int64))       return BConv<Int64>      (delegate(Int64       a, Int64       b) { return  a == b; });

			if (t == typeof(Byte))        return BConv<Byte>       (delegate(Byte        a, Byte        b) { return  a == b; });
			if (t == typeof(UInt16))      return BConv<UInt16>     (delegate(UInt16      a, UInt16      b) { return  a == b; });
			if (t == typeof(UInt32))      return BConv<UInt32>     (delegate(UInt32      a, UInt32      b) { return  a == b; });
			if (t == typeof(UInt64))      return BConv<UInt64>     (delegate(UInt64      a, UInt64      b) { return  a == b; });

			if (t == typeof(bool))        return BConv<bool>       (delegate(bool        a, bool        b) { return  a == b; });
			if (t == typeof(Char))        return BConv<Char>       (delegate(Char        a, Char        b) { return  a == b; });
			if (t == typeof(Single))      return BConv<Single>     (delegate(Single      a, Single      b) { return  a == b; });
			if (t == typeof(Double))      return BConv<Double>     (delegate(Double      a, Double      b) { return  a == b; });

			if (t == typeof(Decimal))     return BConv<Decimal>    (delegate(Decimal     a, Decimal     b) { return  a == b; });
			if (t == typeof(DateTime))    return BConv<DateTime>   (delegate(DateTime    a, DateTime    b) { return  a == b; });
			if (t == typeof(TimeSpan))    return BConv<TimeSpan>   (delegate(TimeSpan    a, TimeSpan    b) { return  a == b; });
			if (t == typeof(Guid))        return BConv<Guid>       (delegate(Guid        a, Guid        b) { return  a == b; });

			// Nullable types.
			//
			if (t == typeof(SByte?))      return BConv<SByte?>     (delegate(SByte?      a, SByte?      b) { return  a == b; });
			if (t == typeof(Int16?))      return BConv<Int16?>     (delegate(Int16?      a, Int16?      b) { return  a == b; });
			if (t == typeof(Int32?))      return BConv<Int32?>     (delegate(Int32?      a, Int32?      b) { return  a == b; });
			if (t == typeof(Int64?))      return BConv<Int64?>     (delegate(Int64?      a, Int64?      b) { return  a == b; });

			if (t == typeof(Byte?))       return BConv<Byte?>      (delegate(Byte?       a, Byte?       b) { return  a == b; });
			if (t == typeof(UInt16?))     return BConv<UInt16?>    (delegate(UInt16?     a, UInt16?     b) { return  a == b; });
			if (t == typeof(UInt32?))     return BConv<UInt32?>    (delegate(UInt32?     a, UInt32?     b) { return  a == b; });
			if (t == typeof(UInt64?))     return BConv<UInt64?>    (delegate(UInt64?     a, UInt64?     b) { return  a == b; });

			if (t == typeof(bool?))       return BConv<bool?>      (delegate(bool?       a, bool?       b) { return  a == b; });
			if (t == typeof(Char?))       return BConv<Char?>      (delegate(Char?       a, Char?       b) { return  a == b; });
			if (t == typeof(Single?))     return BConv<Single?>    (delegate(Single?     a, Single?     b) { return  a == b; });
			if (t == typeof(Double?))     return BConv<Double?>    (delegate(Double?     a, Double?     b) { return  a == b; });

			if (t == typeof(Decimal?))    return BConv<Decimal?>   (delegate(Decimal?    a, Decimal?    b) { return  a == b; });
			if (t == typeof(DateTime?))   return BConv<DateTime?>  (delegate(DateTime?   a, DateTime?   b) { return  a == b; });
			if (t == typeof(TimeSpan?))   return BConv<TimeSpan?>  (delegate(TimeSpan?   a, TimeSpan?   b) { return  a == b; });
			if (t == typeof(Guid?))       return BConv<Guid?>      (delegate(Guid?       a, Guid?       b) { return  a == b; });

			// Sql types.
			//
			if (t == typeof(SqlString))   return BConv<SqlString>  (delegate(SqlString   a, SqlString   b) { return (a == b).IsTrue; });

			if (t == typeof(SqlInt16))    return BConv<SqlInt16>   (delegate(SqlInt16    a, SqlInt16    b) { return (a == b).IsTrue; });
			if (t == typeof(SqlInt32))    return BConv<SqlInt32>   (delegate(SqlInt32    a, SqlInt32    b) { return (a == b).IsTrue; });
			if (t == typeof(SqlInt64))    return BConv<SqlInt64>   (delegate(SqlInt64    a, SqlInt64    b) { return (a == b).IsTrue; });

			if (t == typeof(SqlByte))     return BConv<SqlByte>    (delegate(SqlByte     a, SqlByte     b) { return (a == b).IsTrue; });

			if (t == typeof(SqlSingle))   return BConv<SqlSingle>  (delegate(SqlSingle   a, SqlSingle   b) { return (a == b).IsTrue; });
			if (t == typeof(SqlDouble))   return BConv<SqlDouble>  (delegate(SqlDouble   a, SqlDouble   b) { return (a == b).IsTrue; });
			if (t == typeof(SqlDecimal))  return BConv<SqlDecimal> (delegate(SqlDecimal  a, SqlDecimal  b) { return (a == b).IsTrue; });
			if (t == typeof(SqlMoney))    return BConv<SqlMoney>   (delegate(SqlMoney    a, SqlMoney    b) { return (a == b).IsTrue; });

			if (t == typeof(SqlBoolean))  return BConv<SqlBoolean> (delegate(SqlBoolean  a, SqlBoolean  b) { return (a == b).IsTrue; });
			if (t == typeof(SqlBinary))   return BConv<SqlBinary>  (delegate(SqlBinary   a, SqlBinary   b) { return (a == b).IsTrue; });
			if (t == typeof(SqlDateTime)) return BConv<SqlDateTime>(delegate(SqlDateTime a, SqlDateTime b) { return (a == b).IsTrue; });
			if (t == typeof(SqlGuid))     return BConv<SqlGuid>    (delegate(SqlGuid     a, SqlGuid     b) { return (a == b).IsTrue; });

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
			if (t == typeof(String))      return BConv<String>     (delegate(String      a, String      b) { return  a != b; });

			if (t == typeof(SByte))       return BConv<SByte>      (delegate(SByte       a, SByte       b) { return  a != b; });
			if (t == typeof(Int16))       return BConv<Int16>      (delegate(Int16       a, Int16       b) { return  a != b; });
			if (t == typeof(Int32))       return BConv<Int32>      (delegate(Int32       a, Int32       b) { return  a != b; });
			if (t == typeof(Int64))       return BConv<Int64>      (delegate(Int64       a, Int64       b) { return  a != b; });

			if (t == typeof(Byte))        return BConv<Byte>       (delegate(Byte        a, Byte        b) { return  a != b; });
			if (t == typeof(UInt16))      return BConv<UInt16>     (delegate(UInt16      a, UInt16      b) { return  a != b; });
			if (t == typeof(UInt32))      return BConv<UInt32>     (delegate(UInt32      a, UInt32      b) { return  a != b; });
			if (t == typeof(UInt64))      return BConv<UInt64>     (delegate(UInt64      a, UInt64      b) { return  a != b; });

			if (t == typeof(bool))        return BConv<bool>       (delegate(bool        a, bool        b) { return  a != b; });
			if (t == typeof(Char))        return BConv<Char>       (delegate(Char        a, Char        b) { return  a != b; });
			if (t == typeof(Single))      return BConv<Single>     (delegate(Single      a, Single      b) { return  a != b; });
			if (t == typeof(Double))      return BConv<Double>     (delegate(Double      a, Double      b) { return  a != b; });

			if (t == typeof(Decimal))     return BConv<Decimal>    (delegate(Decimal     a, Decimal     b) { return  a != b; });
			if (t == typeof(DateTime))    return BConv<DateTime>   (delegate(DateTime    a, DateTime    b) { return  a != b; });
			if (t == typeof(TimeSpan))    return BConv<TimeSpan>   (delegate(TimeSpan    a, TimeSpan    b) { return  a != b; });
			if (t == typeof(Guid))        return BConv<Guid>       (delegate(Guid        a, Guid        b) { return  a != b; });

			// Nullable types.
			//
			if (t == typeof(SByte?))      return BConv<SByte?>     (delegate(SByte?      a, SByte?      b) { return  a != b; });
			if (t == typeof(Int16?))      return BConv<Int16?>     (delegate(Int16?      a, Int16?      b) { return  a != b; });
			if (t == typeof(Int32?))      return BConv<Int32?>     (delegate(Int32?      a, Int32?      b) { return  a != b; });
			if (t == typeof(Int64?))      return BConv<Int64?>     (delegate(Int64?      a, Int64?      b) { return  a != b; });

			if (t == typeof(Byte?))       return BConv<Byte?>      (delegate(Byte?       a, Byte?       b) { return  a != b; });
			if (t == typeof(UInt16?))     return BConv<UInt16?>    (delegate(UInt16?     a, UInt16?     b) { return  a != b; });
			if (t == typeof(UInt32?))     return BConv<UInt32?>    (delegate(UInt32?     a, UInt32?     b) { return  a != b; });
			if (t == typeof(UInt64?))     return BConv<UInt64?>    (delegate(UInt64?     a, UInt64?     b) { return  a != b; });

			if (t == typeof(bool?))       return BConv<bool?>      (delegate(bool?       a, bool?       b) { return  a != b; });
			if (t == typeof(Char?))       return BConv<Char?>      (delegate(Char?       a, Char?       b) { return  a != b; });
			if (t == typeof(Single?))     return BConv<Single?>    (delegate(Single?     a, Single?     b) { return  a != b; });
			if (t == typeof(Double?))     return BConv<Double?>    (delegate(Double?     a, Double?     b) { return  a != b; });

			if (t == typeof(Decimal?))    return BConv<Decimal?>   (delegate(Decimal?    a, Decimal?    b) { return  a != b; });
			if (t == typeof(DateTime?))   return BConv<DateTime?>  (delegate(DateTime?   a, DateTime?   b) { return  a != b; });
			if (t == typeof(TimeSpan?))   return BConv<TimeSpan?>  (delegate(TimeSpan?   a, TimeSpan?   b) { return  a != b; });
			if (t == typeof(Guid?))       return BConv<Guid?>      (delegate(Guid?       a, Guid?       b) { return  a != b; });

			// Sql types.
			//
			if (t == typeof(SqlString))   return BConv<SqlString>  (delegate(SqlString   a, SqlString   b) { return (a != b).IsTrue; });

			if (t == typeof(SqlInt16))    return BConv<SqlInt16>   (delegate(SqlInt16    a, SqlInt16    b) { return (a != b).IsTrue; });
			if (t == typeof(SqlInt32))    return BConv<SqlInt32>   (delegate(SqlInt32    a, SqlInt32    b) { return (a != b).IsTrue; });

			if (t == typeof(SqlByte))     return BConv<SqlByte>    (delegate(SqlByte     a, SqlByte     b) { return (a != b).IsTrue; });

			if (t == typeof(SqlSingle))   return BConv<SqlSingle>  (delegate(SqlSingle   a, SqlSingle   b) { return (a != b).IsTrue; });
			if (t == typeof(SqlDouble))   return BConv<SqlDouble>  (delegate(SqlDouble   a, SqlDouble   b) { return (a != b).IsTrue; });
			if (t == typeof(SqlDecimal))  return BConv<SqlDecimal> (delegate(SqlDecimal  a, SqlDecimal  b) { return (a != b).IsTrue; });
			if (t == typeof(SqlMoney))    return BConv<SqlMoney>   (delegate(SqlMoney    a, SqlMoney    b) { return (a != b).IsTrue; });

			if (t == typeof(SqlBoolean))  return BConv<SqlBoolean> (delegate(SqlBoolean  a, SqlBoolean  b) { return (a != b).IsTrue; });
			if (t == typeof(SqlBinary))   return BConv<SqlBinary>  (delegate(SqlBinary   a, SqlBinary   b) { return (a != b).IsTrue; });
			if (t == typeof(SqlDateTime)) return BConv<SqlDateTime>(delegate(SqlDateTime a, SqlDateTime b) { return (a != b).IsTrue; });
			if (t == typeof(SqlGuid))     return BConv<SqlGuid>    (delegate(SqlGuid     a, SqlGuid     b) { return (a != b).IsTrue; });

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
			if (t == typeof(SByte))       return BConv<SByte>      (delegate(SByte       a, SByte       b) { return  a > b; });
			if (t == typeof(Int16))       return BConv<Int16>      (delegate(Int16       a, Int16       b) { return  a > b; });
			if (t == typeof(Int32))       return BConv<Int32>      (delegate(Int32       a, Int32       b) { return  a > b; });
			if (t == typeof(Int64))       return BConv<Int64>      (delegate(Int64       a, Int64       b) { return  a > b; });

			if (t == typeof(Byte))        return BConv<Byte>       (delegate(Byte        a, Byte        b) { return  a > b; });
			if (t == typeof(UInt16))      return BConv<UInt16>     (delegate(UInt16      a, UInt16      b) { return  a > b; });
			if (t == typeof(UInt32))      return BConv<UInt32>     (delegate(UInt32      a, UInt32      b) { return  a > b; });
			if (t == typeof(UInt64))      return BConv<UInt64>     (delegate(UInt64      a, UInt64      b) { return  a > b; });

			if (t == typeof(Char))        return BConv<Char>       (delegate(Char        a, Char        b) { return  a > b; });
			if (t == typeof(Single))      return BConv<Single>     (delegate(Single      a, Single      b) { return  a > b; });
			if (t == typeof(Double))      return BConv<Double>     (delegate(Double      a, Double      b) { return  a > b; });

			if (t == typeof(Decimal))     return BConv<Decimal>    (delegate(Decimal     a, Decimal     b) { return  a > b; });
			if (t == typeof(DateTime))    return BConv<DateTime>   (delegate(DateTime    a, DateTime    b) { return  a > b; });
			if (t == typeof(TimeSpan))    return BConv<TimeSpan>   (delegate(TimeSpan    a, TimeSpan    b) { return  a > b; });

			// Nullable types.
			//
			if (t == typeof(SByte?))      return BConv<SByte?>     (delegate(SByte?      a, SByte?      b) { return  a > b; });
			if (t == typeof(Int16?))      return BConv<Int16?>     (delegate(Int16?      a, Int16?      b) { return  a > b; });
			if (t == typeof(Int32?))      return BConv<Int32?>     (delegate(Int32?      a, Int32?      b) { return  a > b; });
			if (t == typeof(Int64?))      return BConv<Int64?>     (delegate(Int64?      a, Int64?      b) { return  a > b; });

			if (t == typeof(Byte?))       return BConv<Byte?>      (delegate(Byte?       a, Byte?       b) { return  a > b; });
			if (t == typeof(UInt16?))     return BConv<UInt16?>    (delegate(UInt16?     a, UInt16?     b) { return  a > b; });
			if (t == typeof(UInt32?))     return BConv<UInt32?>    (delegate(UInt32?     a, UInt32?     b) { return  a > b; });
			if (t == typeof(UInt64?))     return BConv<UInt64?>    (delegate(UInt64?     a, UInt64?     b) { return  a > b; });

			if (t == typeof(Char?))       return BConv<Char?>      (delegate(Char?       a, Char?       b) { return  a > b; });
			if (t == typeof(Single?))     return BConv<Single?>    (delegate(Single?     a, Single?     b) { return  a > b; });
			if (t == typeof(Double?))     return BConv<Double?>    (delegate(Double?     a, Double?     b) { return  a > b; });

			if (t == typeof(Decimal?))    return BConv<Decimal?>   (delegate(Decimal?    a, Decimal?    b) { return  a > b; });
			if (t == typeof(DateTime?))   return BConv<DateTime?>  (delegate(DateTime?   a, DateTime?   b) { return  a > b; });
			if (t == typeof(TimeSpan?))   return BConv<TimeSpan?>  (delegate(TimeSpan?   a, TimeSpan?   b) { return  a > b; });

			// Sql types.
			//
			if (t == typeof(SqlString))   return BConv<SqlString>  (delegate(SqlString   a, SqlString   b) { return (a > b).IsTrue; });

			if (t == typeof(SqlInt16))    return BConv<SqlInt16>   (delegate(SqlInt16    a, SqlInt16    b) { return (a > b).IsTrue; });
			if (t == typeof(SqlInt32))    return BConv<SqlInt32>   (delegate(SqlInt32    a, SqlInt32    b) { return (a > b).IsTrue; });

			if (t == typeof(SqlByte))     return BConv<SqlByte>    (delegate(SqlByte     a, SqlByte     b) { return (a > b).IsTrue; });

			if (t == typeof(SqlSingle))   return BConv<SqlSingle>  (delegate(SqlSingle   a, SqlSingle   b) { return (a > b).IsTrue; });
			if (t == typeof(SqlDouble))   return BConv<SqlDouble>  (delegate(SqlDouble   a, SqlDouble   b) { return (a > b).IsTrue; });
			if (t == typeof(SqlDecimal))  return BConv<SqlDecimal> (delegate(SqlDecimal  a, SqlDecimal  b) { return (a > b).IsTrue; });
			if (t == typeof(SqlMoney))    return BConv<SqlMoney>   (delegate(SqlMoney    a, SqlMoney    b) { return (a > b).IsTrue; });

			if (t == typeof(SqlBoolean))  return BConv<SqlBoolean> (delegate(SqlBoolean  a, SqlBoolean  b) { return (a > b).IsTrue; });
			if (t == typeof(SqlBinary))   return BConv<SqlBinary>  (delegate(SqlBinary   a, SqlBinary   b) { return (a > b).IsTrue; });
			if (t == typeof(SqlDateTime)) return BConv<SqlDateTime>(delegate(SqlDateTime a, SqlDateTime b) { return (a > b).IsTrue; });
			if (t == typeof(SqlGuid))     return BConv<SqlGuid>    (delegate(SqlGuid     a, SqlGuid     b) { return (a > b).IsTrue; });

			return delegate(T a,T b) { return !a.Equals(b); };
		}

		#endregion

		#region (a >= b) GreaterThanOrEqual

		public  static BoolRetOperation GreaterThanOrEqual = GetGreaterThanOrEqualOperator();
		private static BoolRetOperation GetGreaterThanOrEqualOperator()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(SByte))       return BConv<SByte>      (delegate(SByte       a, SByte       b) { return  a >= b; });
			if (t == typeof(Int16))       return BConv<Int16>      (delegate(Int16       a, Int16       b) { return  a >= b; });
			if (t == typeof(Int32))       return BConv<Int32>      (delegate(Int32       a, Int32       b) { return  a >= b; });
			if (t == typeof(Int64))       return BConv<Int64>      (delegate(Int64       a, Int64       b) { return  a >= b; });

			if (t == typeof(Byte))        return BConv<Byte>       (delegate(Byte        a, Byte        b) { return  a >= b; });
			if (t == typeof(UInt16))      return BConv<UInt16>     (delegate(UInt16      a, UInt16      b) { return  a >= b; });
			if (t == typeof(UInt32))      return BConv<UInt32>     (delegate(UInt32      a, UInt32      b) { return  a >= b; });
			if (t == typeof(UInt64))      return BConv<UInt64>     (delegate(UInt64      a, UInt64      b) { return  a >= b; });

			if (t == typeof(Char))        return BConv<Char>       (delegate(Char        a, Char        b) { return  a >= b; });
			if (t == typeof(Single))      return BConv<Single>     (delegate(Single      a, Single      b) { return  a >= b; });
			if (t == typeof(Double))      return BConv<Double>     (delegate(Double      a, Double      b) { return  a >= b; });

			if (t == typeof(Decimal))     return BConv<Decimal>    (delegate(Decimal     a, Decimal     b) { return  a >= b; });
			if (t == typeof(DateTime))    return BConv<DateTime>   (delegate(DateTime    a, DateTime    b) { return  a >= b; });
			if (t == typeof(TimeSpan))    return BConv<TimeSpan>   (delegate(TimeSpan    a, TimeSpan    b) { return  a >= b; });

			// Nullable types.
			//
			if (t == typeof(SByte?))      return BConv<SByte?>     (delegate(SByte?      a, SByte?      b) { return  a >= b; });
			if (t == typeof(Int16?))      return BConv<Int16?>     (delegate(Int16?      a, Int16?      b) { return  a >= b; });
			if (t == typeof(Int32?))      return BConv<Int32?>     (delegate(Int32?      a, Int32?      b) { return  a >= b; });
			if (t == typeof(Int64?))      return BConv<Int64?>     (delegate(Int64?      a, Int64?      b) { return  a >= b; });

			if (t == typeof(Byte?))       return BConv<Byte?>      (delegate(Byte?       a, Byte?       b) { return  a >= b; });
			if (t == typeof(UInt16?))     return BConv<UInt16?>    (delegate(UInt16?     a, UInt16?     b) { return  a >= b; });
			if (t == typeof(UInt32?))     return BConv<UInt32?>    (delegate(UInt32?     a, UInt32?     b) { return  a >= b; });
			if (t == typeof(UInt64?))     return BConv<UInt64?>    (delegate(UInt64?     a, UInt64?     b) { return  a >= b; });

			if (t == typeof(Char?))       return BConv<Char?>      (delegate(Char?       a, Char?       b) { return  a >= b; });
			if (t == typeof(Single?))     return BConv<Single?>    (delegate(Single?     a, Single?     b) { return  a >= b; });
			if (t == typeof(Double?))     return BConv<Double?>    (delegate(Double?     a, Double?     b) { return  a >= b; });

			if (t == typeof(Decimal?))    return BConv<Decimal?>   (delegate(Decimal?    a, Decimal?    b) { return  a >= b; });
			if (t == typeof(DateTime?))   return BConv<DateTime?>  (delegate(DateTime?   a, DateTime?   b) { return  a >= b; });
			if (t == typeof(TimeSpan?))   return BConv<TimeSpan?>  (delegate(TimeSpan?   a, TimeSpan?   b) { return  a >= b; });

			// Sql types.
			//
			if (t == typeof(SqlString))   return BConv<SqlString>  (delegate(SqlString   a, SqlString   b) { return (a >= b).IsTrue; });

			if (t == typeof(SqlInt16))    return BConv<SqlInt16>   (delegate(SqlInt16    a, SqlInt16    b) { return (a >= b).IsTrue; });
			if (t == typeof(SqlInt32))    return BConv<SqlInt32>   (delegate(SqlInt32    a, SqlInt32    b) { return (a >= b).IsTrue; });

			if (t == typeof(SqlByte))     return BConv<SqlByte>    (delegate(SqlByte     a, SqlByte     b) { return (a >= b).IsTrue; });

			if (t == typeof(SqlSingle))   return BConv<SqlSingle>  (delegate(SqlSingle   a, SqlSingle   b) { return (a >= b).IsTrue; });
			if (t == typeof(SqlDouble))   return BConv<SqlDouble>  (delegate(SqlDouble   a, SqlDouble   b) { return (a >= b).IsTrue; });
			if (t == typeof(SqlDecimal))  return BConv<SqlDecimal> (delegate(SqlDecimal  a, SqlDecimal  b) { return (a >= b).IsTrue; });
			if (t == typeof(SqlMoney))    return BConv<SqlMoney>   (delegate(SqlMoney    a, SqlMoney    b) { return (a >= b).IsTrue; });

			if (t == typeof(SqlBoolean))  return BConv<SqlBoolean> (delegate(SqlBoolean  a, SqlBoolean  b) { return (a >= b).IsTrue; });
			if (t == typeof(SqlBinary))   return BConv<SqlBinary>  (delegate(SqlBinary   a, SqlBinary   b) { return (a >= b).IsTrue; });
			if (t == typeof(SqlDateTime)) return BConv<SqlDateTime>(delegate(SqlDateTime a, SqlDateTime b) { return (a >= b).IsTrue; });
			if (t == typeof(SqlGuid))     return BConv<SqlGuid>    (delegate(SqlGuid     a, SqlGuid     b) { return (a >= b).IsTrue; });

			return delegate(T a,T b) { return !a.Equals(b); };
		}

		#endregion

		#region (a < b)  LessThan 

		public  static BoolRetOperation LessThan = GetLessThanOperator();
		private static BoolRetOperation GetLessThanOperator()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(SByte))       return BConv<SByte>      (delegate(SByte       a, SByte       b) { return  a < b; });
			if (t == typeof(Int16))       return BConv<Int16>      (delegate(Int16       a, Int16       b) { return  a < b; });
			if (t == typeof(Int32))       return BConv<Int32>      (delegate(Int32       a, Int32       b) { return  a < b; });
			if (t == typeof(Int64))       return BConv<Int64>      (delegate(Int64       a, Int64       b) { return  a < b; });

			if (t == typeof(Byte))        return BConv<Byte>       (delegate(Byte        a, Byte        b) { return  a < b; });
			if (t == typeof(UInt16))      return BConv<UInt16>     (delegate(UInt16      a, UInt16      b) { return  a < b; });
			if (t == typeof(UInt32))      return BConv<UInt32>     (delegate(UInt32      a, UInt32      b) { return  a < b; });
			if (t == typeof(UInt64))      return BConv<UInt64>     (delegate(UInt64      a, UInt64      b) { return  a < b; });

			if (t == typeof(Char))        return BConv<Char>       (delegate(Char        a, Char        b) { return  a < b; });
			if (t == typeof(Single))      return BConv<Single>     (delegate(Single      a, Single      b) { return  a < b; });
			if (t == typeof(Double))      return BConv<Double>     (delegate(Double      a, Double      b) { return  a < b; });

			if (t == typeof(Decimal))     return BConv<Decimal>    (delegate(Decimal     a, Decimal     b) { return  a < b; });
			if (t == typeof(DateTime))    return BConv<DateTime>   (delegate(DateTime    a, DateTime    b) { return  a < b; });
			if (t == typeof(TimeSpan))    return BConv<TimeSpan>   (delegate(TimeSpan    a, TimeSpan    b) { return  a < b; });

			// Nullable types.
			//
			if (t == typeof(SByte?))      return BConv<SByte?>     (delegate(SByte?      a, SByte?      b) { return  a < b; });
			if (t == typeof(Int16?))      return BConv<Int16?>     (delegate(Int16?      a, Int16?      b) { return  a < b; });
			if (t == typeof(Int32?))      return BConv<Int32?>     (delegate(Int32?      a, Int32?      b) { return  a < b; });
			if (t == typeof(Int64?))      return BConv<Int64?>     (delegate(Int64?      a, Int64?      b) { return  a < b; });

			if (t == typeof(Byte?))       return BConv<Byte?>      (delegate(Byte?       a, Byte?       b) { return  a < b; });
			if (t == typeof(UInt16?))     return BConv<UInt16?>    (delegate(UInt16?     a, UInt16?     b) { return  a < b; });
			if (t == typeof(UInt32?))     return BConv<UInt32?>    (delegate(UInt32?     a, UInt32?     b) { return  a < b; });
			if (t == typeof(UInt64?))     return BConv<UInt64?>    (delegate(UInt64?     a, UInt64?     b) { return  a < b; });

			if (t == typeof(Char?))       return BConv<Char?>      (delegate(Char?       a, Char?       b) { return  a < b; });
			if (t == typeof(Single?))     return BConv<Single?>    (delegate(Single?     a, Single?     b) { return  a < b; });
			if (t == typeof(Double?))     return BConv<Double?>    (delegate(Double?     a, Double?     b) { return  a < b; });

			if (t == typeof(Decimal?))    return BConv<Decimal?>   (delegate(Decimal?    a, Decimal?    b) { return  a < b; });
			if (t == typeof(DateTime?))   return BConv<DateTime?>  (delegate(DateTime?   a, DateTime?   b) { return  a < b; });
			if (t == typeof(TimeSpan?))   return BConv<TimeSpan?>  (delegate(TimeSpan?   a, TimeSpan?   b) { return  a < b; });

			// Sql types.
			//
			if (t == typeof(SqlString))   return BConv<SqlString>  (delegate(SqlString   a, SqlString   b) { return (a < b).IsTrue; });

			if (t == typeof(SqlInt16))    return BConv<SqlInt16>   (delegate(SqlInt16    a, SqlInt16    b) { return (a < b).IsTrue; });
			if (t == typeof(SqlInt32))    return BConv<SqlInt32>   (delegate(SqlInt32    a, SqlInt32    b) { return (a < b).IsTrue; });

			if (t == typeof(SqlByte))     return BConv<SqlByte>    (delegate(SqlByte     a, SqlByte     b) { return (a < b).IsTrue; });

			if (t == typeof(SqlSingle))   return BConv<SqlSingle>  (delegate(SqlSingle   a, SqlSingle   b) { return (a < b).IsTrue; });
			if (t == typeof(SqlDouble))   return BConv<SqlDouble>  (delegate(SqlDouble   a, SqlDouble   b) { return (a < b).IsTrue; });
			if (t == typeof(SqlDecimal))  return BConv<SqlDecimal> (delegate(SqlDecimal  a, SqlDecimal  b) { return (a < b).IsTrue; });
			if (t == typeof(SqlMoney))    return BConv<SqlMoney>   (delegate(SqlMoney    a, SqlMoney    b) { return (a < b).IsTrue; });

			if (t == typeof(SqlBoolean))  return BConv<SqlBoolean> (delegate(SqlBoolean  a, SqlBoolean  b) { return (a < b).IsTrue; });
			if (t == typeof(SqlBinary))   return BConv<SqlBinary>  (delegate(SqlBinary   a, SqlBinary   b) { return (a < b).IsTrue; });
			if (t == typeof(SqlDateTime)) return BConv<SqlDateTime>(delegate(SqlDateTime a, SqlDateTime b) { return (a < b).IsTrue; });
			if (t == typeof(SqlGuid))     return BConv<SqlGuid>    (delegate(SqlGuid     a, SqlGuid     b) { return (a < b).IsTrue; });

			return delegate(T a,T b) { return !a.Equals(b); };
		}

		#endregion

		#region (a <= b) LessThanOrEqual 

		public  static BoolRetOperation LessThanOrEqual = GetLessThanOrEqualOperator();
		private static BoolRetOperation GetLessThanOrEqualOperator()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(SByte))       return BConv<SByte>      (delegate(SByte       a, SByte       b) { return  a <= b; });
			if (t == typeof(Int16))       return BConv<Int16>      (delegate(Int16       a, Int16       b) { return  a <= b; });
			if (t == typeof(Int32))       return BConv<Int32>      (delegate(Int32       a, Int32       b) { return  a <= b; });
			if (t == typeof(Int64))       return BConv<Int64>      (delegate(Int64       a, Int64       b) { return  a <= b; });

			if (t == typeof(Byte))        return BConv<Byte>       (delegate(Byte        a, Byte        b) { return  a <= b; });
			if (t == typeof(UInt16))      return BConv<UInt16>     (delegate(UInt16      a, UInt16      b) { return  a <= b; });
			if (t == typeof(UInt32))      return BConv<UInt32>     (delegate(UInt32      a, UInt32      b) { return  a <= b; });
			if (t == typeof(UInt64))      return BConv<UInt64>     (delegate(UInt64      a, UInt64      b) { return  a <= b; });

			if (t == typeof(Char))        return BConv<Char>       (delegate(Char        a, Char        b) { return  a <= b; });
			if (t == typeof(Single))      return BConv<Single>     (delegate(Single      a, Single      b) { return  a <= b; });
			if (t == typeof(Double))      return BConv<Double>     (delegate(Double      a, Double      b) { return  a <= b; });

			if (t == typeof(Decimal))     return BConv<Decimal>    (delegate(Decimal     a, Decimal     b) { return  a <= b; });
			if (t == typeof(DateTime))    return BConv<DateTime>   (delegate(DateTime    a, DateTime    b) { return  a <= b; });
			if (t == typeof(TimeSpan))    return BConv<TimeSpan>   (delegate(TimeSpan    a, TimeSpan    b) { return  a <= b; });

			// Nullable types.
			//
			if (t == typeof(SByte?))      return BConv<SByte?>     (delegate(SByte?      a, SByte?      b) { return  a <= b; });
			if (t == typeof(Int16?))      return BConv<Int16?>     (delegate(Int16?      a, Int16?      b) { return  a <= b; });
			if (t == typeof(Int32?))      return BConv<Int32?>     (delegate(Int32?      a, Int32?      b) { return  a <= b; });
			if (t == typeof(Int64?))      return BConv<Int64?>     (delegate(Int64?      a, Int64?      b) { return  a <= b; });

			if (t == typeof(Byte?))       return BConv<Byte?>      (delegate(Byte?       a, Byte?       b) { return  a <= b; });
			if (t == typeof(UInt16?))     return BConv<UInt16?>    (delegate(UInt16?     a, UInt16?     b) { return  a <= b; });
			if (t == typeof(UInt32?))     return BConv<UInt32?>    (delegate(UInt32?     a, UInt32?     b) { return  a <= b; });
			if (t == typeof(UInt64?))     return BConv<UInt64?>    (delegate(UInt64?     a, UInt64?     b) { return  a <= b; });

			if (t == typeof(Char?))       return BConv<Char?>      (delegate(Char?       a, Char?       b) { return  a <= b; });
			if (t == typeof(Single?))     return BConv<Single?>    (delegate(Single?     a, Single?     b) { return  a <= b; });
			if (t == typeof(Double?))     return BConv<Double?>    (delegate(Double?     a, Double?     b) { return  a <= b; });

			if (t == typeof(Decimal?))    return BConv<Decimal?>   (delegate(Decimal?    a, Decimal?    b) { return  a <= b; });
			if (t == typeof(DateTime?))   return BConv<DateTime?>  (delegate(DateTime?   a, DateTime?   b) { return  a <= b; });
			if (t == typeof(TimeSpan?))   return BConv<TimeSpan?>  (delegate(TimeSpan?   a, TimeSpan?   b) { return  a <= b; });

			// Sql types.
			//
			if (t == typeof(SqlString))   return BConv<SqlString>  (delegate(SqlString   a, SqlString   b) { return (a <= b).IsTrue; });

			if (t == typeof(SqlInt16))    return BConv<SqlInt16>   (delegate(SqlInt16    a, SqlInt16    b) { return (a <= b).IsTrue; });
			if (t == typeof(SqlInt32))    return BConv<SqlInt32>   (delegate(SqlInt32    a, SqlInt32    b) { return (a <= b).IsTrue; });

			if (t == typeof(SqlByte))     return BConv<SqlByte>    (delegate(SqlByte     a, SqlByte     b) { return (a <= b).IsTrue; });

			if (t == typeof(SqlSingle))   return BConv<SqlSingle>  (delegate(SqlSingle   a, SqlSingle   b) { return (a <= b).IsTrue; });
			if (t == typeof(SqlDouble))   return BConv<SqlDouble>  (delegate(SqlDouble   a, SqlDouble   b) { return (a <= b).IsTrue; });
			if (t == typeof(SqlDecimal))  return BConv<SqlDecimal> (delegate(SqlDecimal  a, SqlDecimal  b) { return (a <= b).IsTrue; });
			if (t == typeof(SqlMoney))    return BConv<SqlMoney>   (delegate(SqlMoney    a, SqlMoney    b) { return (a <= b).IsTrue; });

			if (t == typeof(SqlBoolean))  return BConv<SqlBoolean> (delegate(SqlBoolean  a, SqlBoolean  b) { return (a <= b).IsTrue; });
			if (t == typeof(SqlBinary))   return BConv<SqlBinary>  (delegate(SqlBinary   a, SqlBinary   b) { return (a <= b).IsTrue; });
			if (t == typeof(SqlDateTime)) return BConv<SqlDateTime>(delegate(SqlDateTime a, SqlDateTime b) { return (a <= b).IsTrue; });
			if (t == typeof(SqlGuid))     return BConv<SqlGuid>    (delegate(SqlGuid     a, SqlGuid     b) { return (a <= b).IsTrue; });

			return delegate(T a,T b) { return !a.Equals(b); };
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
