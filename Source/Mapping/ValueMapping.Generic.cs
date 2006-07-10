using System;
using System.Data.SqlTypes;

namespace BLToolkit.Mapping
{
	public static partial class ValueMapping
	{
		static class GetData<T>
		{
			public abstract class MB<Q>
			{
				public abstract Q Get(IMapDataSource s, object o, int i);
			}

			public static T Get(IMapDataSource s, object o, int i)
			{
				return I.Get(s, o, i);
			}

			public static readonly MB<T> I = GetGetter();
			static MB<T> GetGetter()
			{
				Type t = typeof(T);


				// Scalar Types.
				//
				if (t == typeof(SByte))        return (MB<T>)(object)(new I8());
				if (t == typeof(Int16))        return (MB<T>)(object)(new I16());
				if (t == typeof(Int32))        return (MB<T>)(object)(new I32());
				if (t == typeof(Int64))        return (MB<T>)(object)(new I64());

				if (t == typeof(Byte))         return (MB<T>)(object)(new U8());
				if (t == typeof(UInt16))       return (MB<T>)(object)(new U16());
				if (t == typeof(UInt32))       return (MB<T>)(object)(new U32());
				if (t == typeof(UInt64))       return (MB<T>)(object)(new U64());

				if (t == typeof(Single))       return (MB<T>)(object)(new R4());
				if (t == typeof(Double))       return (MB<T>)(object)(new R8());

				if (t == typeof(Boolean))      return (MB<T>)(object)(new B());
				if (t == typeof(Decimal))      return (MB<T>)(object)(new D());

				if (t == typeof(Char))         return (MB<T>)(object)(new C());
				if (t == typeof(Guid))         return (MB<T>)(object)(new G());
				if (t == typeof(DateTime))     return (MB<T>)(object)(new DT());

				// Nullable Types.
				//
				if (t == typeof(SByte?))       return (MB<T>)(object)(new NI8());
				if (t == typeof(Int16?))       return (MB<T>)(object)(new NI16());
				if (t == typeof(Int32?))       return (MB<T>)(object)(new NI32());
				if (t == typeof(Int64?))       return (MB<T>)(object)(new NI64());

				if (t == typeof(Byte?))        return (MB<T>)(object)(new NU8());
				if (t == typeof(UInt16?))      return (MB<T>)(object)(new NU16());
				if (t == typeof(UInt32?))      return (MB<T>)(object)(new NU32());
				if (t == typeof(UInt64?))      return (MB<T>)(object)(new NU64());

				if (t == typeof(Single?))      return (MB<T>)(object)(new NR4());
				if (t == typeof(Double?))      return (MB<T>)(object)(new NR8());

				if (t == typeof(Boolean?))     return (MB<T>)(object)(new NB());
				if (t == typeof(Decimal?))     return (MB<T>)(object)(new ND());

				if (t == typeof(Char?))        return (MB<T>)(object)(new NC());
				if (t == typeof(Guid?))        return (MB<T>)(object)(new NG());
				if (t == typeof(DateTime?))    return (MB<T>)(object)(new NDT());

				// SqlTypes.
				//
				if (t == typeof(SqlString))    return (MB<T>)(object)(new dbS());

				if (t == typeof(SqlByte))      return (MB<T>)(object)(new dbU8());
				if (t == typeof(SqlInt16))     return (MB<T>)(object)(new dbI16());
				if (t == typeof(SqlInt32))     return (MB<T>)(object)(new dbI32());
				if (t == typeof(SqlInt64))     return (MB<T>)(object)(new dbI64());

				if (t == typeof(SqlSingle))    return (MB<T>)(object)(new dbR4());
				if (t == typeof(SqlDouble))    return (MB<T>)(object)(new dbR8());
				if (t == typeof(SqlDecimal))   return (MB<T>)(object)(new dbD());
				if (t == typeof(SqlMoney))     return (MB<T>)(object)(new dbM());

				if (t == typeof(SqlBoolean))   return (MB<T>)(object)(new dbB());
				if (t == typeof(SqlGuid))      return (MB<T>)(object)(new dbG());
				if (t == typeof(SqlDateTime))  return (MB<T>)(object)(new dbDT());

				return null;
			}


			// Scalar Types.
			//
			sealed class I8          : MB<SByte>       { public override SByte       Get(IMapDataSource s, object o, int i) { return s.GetSByte       (o, i); } }
			sealed class I16         : MB<Int16>       { public override Int16       Get(IMapDataSource s, object o, int i) { return s.GetInt16       (o, i); } }
			sealed class I32         : MB<Int32>       { public override Int32       Get(IMapDataSource s, object o, int i) { return s.GetInt32       (o, i); } }
			sealed class I64         : MB<Int64>       { public override Int64       Get(IMapDataSource s, object o, int i) { return s.GetInt64       (o, i); } }

			sealed class U8          : MB<Byte>        { public override Byte        Get(IMapDataSource s, object o, int i) { return s.GetByte        (o, i); } }
			sealed class U16         : MB<UInt16>      { public override UInt16      Get(IMapDataSource s, object o, int i) { return s.GetUInt16      (o, i); } }
			sealed class U32         : MB<UInt32>      { public override UInt32      Get(IMapDataSource s, object o, int i) { return s.GetUInt32      (o, i); } }
			sealed class U64         : MB<UInt64>      { public override UInt64      Get(IMapDataSource s, object o, int i) { return s.GetUInt64      (o, i); } }

			sealed class R4          : MB<Single>      { public override Single      Get(IMapDataSource s, object o, int i) { return s.GetSingle      (o, i); } }
			sealed class R8          : MB<Double>      { public override Double      Get(IMapDataSource s, object o, int i) { return s.GetDouble      (o, i); } }

			sealed class B           : MB<Boolean>     { public override Boolean     Get(IMapDataSource s, object o, int i) { return s.GetBoolean     (o, i); } }
			sealed class D           : MB<Decimal>     { public override Decimal     Get(IMapDataSource s, object o, int i) { return s.GetDecimal     (o, i); } }

			sealed class C           : MB<Char>        { public override Char        Get(IMapDataSource s, object o, int i) { return s.GetChar        (o, i); } }
			sealed class G           : MB<Guid>        { public override Guid        Get(IMapDataSource s, object o, int i) { return s.GetGuid        (o, i); } }
			sealed class DT          : MB<DateTime>    { public override DateTime    Get(IMapDataSource s, object o, int i) { return s.GetDateTime    (o, i); } }

			// Nullable Types.
			//
			sealed class NI8         : MB<SByte?>      { public override SByte?      Get(IMapDataSource s, object o, int i) { return s.GetNullableSByte      (o, i); } }
			sealed class NI16        : MB<Int16?>      { public override Int16?      Get(IMapDataSource s, object o, int i) { return s.GetNullableInt16      (o, i); } }
			sealed class NI32        : MB<Int32?>      { public override Int32?      Get(IMapDataSource s, object o, int i) { return s.GetNullableInt32      (o, i); } }
			sealed class NI64        : MB<Int64?>      { public override Int64?      Get(IMapDataSource s, object o, int i) { return s.GetNullableInt64      (o, i); } }

			sealed class NU8         : MB<Byte?>       { public override Byte?       Get(IMapDataSource s, object o, int i) { return s.GetNullableByte       (o, i); } }
			sealed class NU16        : MB<UInt16?>     { public override UInt16?     Get(IMapDataSource s, object o, int i) { return s.GetNullableUInt16     (o, i); } }
			sealed class NU32        : MB<UInt32?>     { public override UInt32?     Get(IMapDataSource s, object o, int i) { return s.GetNullableUInt32     (o, i); } }
			sealed class NU64        : MB<UInt64?>     { public override UInt64?     Get(IMapDataSource s, object o, int i) { return s.GetNullableUInt64     (o, i); } }

			sealed class NR4         : MB<Single?>     { public override Single?     Get(IMapDataSource s, object o, int i) { return s.GetNullableSingle     (o, i); } }
			sealed class NR8         : MB<Double?>     { public override Double?     Get(IMapDataSource s, object o, int i) { return s.GetNullableDouble     (o, i); } }

			sealed class NB          : MB<Boolean?>    { public override Boolean?    Get(IMapDataSource s, object o, int i) { return s.GetNullableBoolean    (o, i); } }
			sealed class ND          : MB<Decimal?>    { public override Decimal?    Get(IMapDataSource s, object o, int i) { return s.GetNullableDecimal    (o, i); } }

			sealed class NC          : MB<Char?>       { public override Char?       Get(IMapDataSource s, object o, int i) { return s.GetNullableChar       (o, i); } }
			sealed class NG          : MB<Guid?>       { public override Guid?       Get(IMapDataSource s, object o, int i) { return s.GetNullableGuid       (o, i); } }
			sealed class NDT         : MB<DateTime?>   { public override DateTime?   Get(IMapDataSource s, object o, int i) { return s.GetNullableDateTime   (o, i); } }

			// SqlTypes.
			//
			sealed class dbS         : MB<SqlString>   { public override SqlString   Get(IMapDataSource s, object o, int i) { return s.GetSqlString   (o, i); } }

			sealed class dbU8        : MB<SqlByte>     { public override SqlByte     Get(IMapDataSource s, object o, int i) { return s.GetSqlByte     (o, i); } }
			sealed class dbI16       : MB<SqlInt16>    { public override SqlInt16    Get(IMapDataSource s, object o, int i) { return s.GetSqlInt16    (o, i); } }
			sealed class dbI32       : MB<SqlInt32>    { public override SqlInt32    Get(IMapDataSource s, object o, int i) { return s.GetSqlInt32    (o, i); } }
			sealed class dbI64       : MB<SqlInt64>    { public override SqlInt64    Get(IMapDataSource s, object o, int i) { return s.GetSqlInt64    (o, i); } }

			sealed class dbR4        : MB<SqlSingle>   { public override SqlSingle   Get(IMapDataSource s, object o, int i) { return s.GetSqlSingle   (o, i); } }
			sealed class dbR8        : MB<SqlDouble>   { public override SqlDouble   Get(IMapDataSource s, object o, int i) { return s.GetSqlDouble   (o, i); } }
			sealed class dbD         : MB<SqlDecimal>  { public override SqlDecimal  Get(IMapDataSource s, object o, int i) { return s.GetSqlDecimal  (o, i); } }
			sealed class dbM         : MB<SqlMoney>    { public override SqlMoney    Get(IMapDataSource s, object o, int i) { return s.GetSqlMoney    (o, i); } }

			sealed class dbB         : MB<SqlBoolean>  { public override SqlBoolean  Get(IMapDataSource s, object o, int i) { return s.GetSqlBoolean  (o, i); } }
			sealed class dbG         : MB<SqlGuid>     { public override SqlGuid     Get(IMapDataSource s, object o, int i) { return s.GetSqlGuid     (o, i); } }
			sealed class dbDT        : MB<SqlDateTime> { public override SqlDateTime Get(IMapDataSource s, object o, int i) { return s.GetSqlDateTime (o, i); } }
		}

		static class SetData<T>
		{
			public abstract class MB<Q>
			{
				public abstract void Set(IMapDataDestination d, object o, int i, Q v);
			}

			public static void Set(IMapDataDestination d, object o, int i, T v)
			{
				I.Set(d, o, i, v);
			}

			public static readonly MB<T> I = GetSetter();
			static MB<T> GetSetter()
			{
				Type t = typeof(T);


				// Scalar Types.
				//
				if (t == typeof(SByte))        return (MB<T>)(object)(new I8());
				if (t == typeof(Int16))        return (MB<T>)(object)(new I16());
				if (t == typeof(Int32))        return (MB<T>)(object)(new I32());
				if (t == typeof(Int64))        return (MB<T>)(object)(new I64());

				if (t == typeof(Byte))         return (MB<T>)(object)(new U8());
				if (t == typeof(UInt16))       return (MB<T>)(object)(new U16());
				if (t == typeof(UInt32))       return (MB<T>)(object)(new U32());
				if (t == typeof(UInt64))       return (MB<T>)(object)(new U64());

				if (t == typeof(Single))       return (MB<T>)(object)(new R4());
				if (t == typeof(Double))       return (MB<T>)(object)(new R8());

				if (t == typeof(Boolean))      return (MB<T>)(object)(new B());
				if (t == typeof(Decimal))      return (MB<T>)(object)(new D());

				if (t == typeof(Char))         return (MB<T>)(object)(new C());
				if (t == typeof(Guid))         return (MB<T>)(object)(new G());
				if (t == typeof(DateTime))     return (MB<T>)(object)(new DT());

				// Nullable Types.
				//
				if (t == typeof(SByte?))       return (MB<T>)(object)(new NI8());
				if (t == typeof(Int16?))       return (MB<T>)(object)(new NI16());
				if (t == typeof(Int32?))       return (MB<T>)(object)(new NI32());
				if (t == typeof(Int64?))       return (MB<T>)(object)(new NI64());

				if (t == typeof(Byte?))        return (MB<T>)(object)(new NU8());
				if (t == typeof(UInt16?))      return (MB<T>)(object)(new NU16());
				if (t == typeof(UInt32?))      return (MB<T>)(object)(new NU32());
				if (t == typeof(UInt64?))      return (MB<T>)(object)(new NU64());

				if (t == typeof(Single?))      return (MB<T>)(object)(new NR4());
				if (t == typeof(Double?))      return (MB<T>)(object)(new NR8());

				if (t == typeof(Boolean?))     return (MB<T>)(object)(new NB());
				if (t == typeof(Decimal?))     return (MB<T>)(object)(new ND());

				if (t == typeof(Char?))        return (MB<T>)(object)(new NC());
				if (t == typeof(Guid?))        return (MB<T>)(object)(new NG());
				if (t == typeof(DateTime?))    return (MB<T>)(object)(new NDT());

				// SqlTypes.
				//
				if (t == typeof(SqlString))    return (MB<T>)(object)(new dbS());

				if (t == typeof(SqlByte))      return (MB<T>)(object)(new dbU8());
				if (t == typeof(SqlInt16))     return (MB<T>)(object)(new dbI16());
				if (t == typeof(SqlInt32))     return (MB<T>)(object)(new dbI32());
				if (t == typeof(SqlInt64))     return (MB<T>)(object)(new dbI64());

				if (t == typeof(SqlSingle))    return (MB<T>)(object)(new dbR4());
				if (t == typeof(SqlDouble))    return (MB<T>)(object)(new dbR8());
				if (t == typeof(SqlDecimal))   return (MB<T>)(object)(new dbD());
				if (t == typeof(SqlMoney))     return (MB<T>)(object)(new dbM());

				if (t == typeof(SqlBoolean))   return (MB<T>)(object)(new dbB());
				if (t == typeof(SqlGuid))      return (MB<T>)(object)(new dbG());
				if (t == typeof(SqlDateTime))  return (MB<T>)(object)(new dbDT());

				return null;
			}


			// Scalar Types.
			//
			sealed class I8          : MB<SByte>       { public override  void Set(IMapDataDestination d, object o, int i, SByte       v) { d.SetSByte       (o, i, v); } }
			sealed class I16         : MB<Int16>       { public override  void Set(IMapDataDestination d, object o, int i, Int16       v) { d.SetInt16       (o, i, v); } }
			sealed class I32         : MB<Int32>       { public override  void Set(IMapDataDestination d, object o, int i, Int32       v) { d.SetInt32       (o, i, v); } }
			sealed class I64         : MB<Int64>       { public override  void Set(IMapDataDestination d, object o, int i, Int64       v) { d.SetInt64       (o, i, v); } }

			sealed class U8          : MB<Byte>        { public override  void Set(IMapDataDestination d, object o, int i, Byte        v) { d.SetByte        (o, i, v); } }
			sealed class U16         : MB<UInt16>      { public override  void Set(IMapDataDestination d, object o, int i, UInt16      v) { d.SetUInt16      (o, i, v); } }
			sealed class U32         : MB<UInt32>      { public override  void Set(IMapDataDestination d, object o, int i, UInt32      v) { d.SetUInt32      (o, i, v); } }
			sealed class U64         : MB<UInt64>      { public override  void Set(IMapDataDestination d, object o, int i, UInt64      v) { d.SetUInt64      (o, i, v); } }

			sealed class R4          : MB<Single>      { public override  void Set(IMapDataDestination d, object o, int i, Single      v) { d.SetSingle      (o, i, v); } }
			sealed class R8          : MB<Double>      { public override  void Set(IMapDataDestination d, object o, int i, Double      v) { d.SetDouble      (o, i, v); } }

			sealed class B           : MB<Boolean>     { public override  void Set(IMapDataDestination d, object o, int i, Boolean     v) { d.SetBoolean     (o, i, v); } }
			sealed class D           : MB<Decimal>     { public override  void Set(IMapDataDestination d, object o, int i, Decimal     v) { d.SetDecimal     (o, i, v); } }

			sealed class C           : MB<Char>        { public override  void Set(IMapDataDestination d, object o, int i, Char        v) { d.SetChar        (o, i, v); } }
			sealed class G           : MB<Guid>        { public override  void Set(IMapDataDestination d, object o, int i, Guid        v) { d.SetGuid        (o, i, v); } }
			sealed class DT          : MB<DateTime>    { public override  void Set(IMapDataDestination d, object o, int i, DateTime    v) { d.SetDateTime    (o, i, v); } }

			// Nullable Types.
			//
			sealed class NI8         : MB<SByte?>      { public override  void Set(IMapDataDestination d, object o, int i, SByte?      v) { d.SetNullableSByte      (o, i, v); } }
			sealed class NI16        : MB<Int16?>      { public override  void Set(IMapDataDestination d, object o, int i, Int16?      v) { d.SetNullableInt16      (o, i, v); } }
			sealed class NI32        : MB<Int32?>      { public override  void Set(IMapDataDestination d, object o, int i, Int32?      v) { d.SetNullableInt32      (o, i, v); } }
			sealed class NI64        : MB<Int64?>      { public override  void Set(IMapDataDestination d, object o, int i, Int64?      v) { d.SetNullableInt64      (o, i, v); } }

			sealed class NU8         : MB<Byte?>       { public override  void Set(IMapDataDestination d, object o, int i, Byte?       v) { d.SetNullableByte       (o, i, v); } }
			sealed class NU16        : MB<UInt16?>     { public override  void Set(IMapDataDestination d, object o, int i, UInt16?     v) { d.SetNullableUInt16     (o, i, v); } }
			sealed class NU32        : MB<UInt32?>     { public override  void Set(IMapDataDestination d, object o, int i, UInt32?     v) { d.SetNullableUInt32     (o, i, v); } }
			sealed class NU64        : MB<UInt64?>     { public override  void Set(IMapDataDestination d, object o, int i, UInt64?     v) { d.SetNullableUInt64     (o, i, v); } }

			sealed class NR4         : MB<Single?>     { public override  void Set(IMapDataDestination d, object o, int i, Single?     v) { d.SetNullableSingle     (o, i, v); } }
			sealed class NR8         : MB<Double?>     { public override  void Set(IMapDataDestination d, object o, int i, Double?     v) { d.SetNullableDouble     (o, i, v); } }

			sealed class NB          : MB<Boolean?>    { public override  void Set(IMapDataDestination d, object o, int i, Boolean?    v) { d.SetNullableBoolean    (o, i, v); } }
			sealed class ND          : MB<Decimal?>    { public override  void Set(IMapDataDestination d, object o, int i, Decimal?    v) { d.SetNullableDecimal    (o, i, v); } }

			sealed class NC          : MB<Char?>       { public override  void Set(IMapDataDestination d, object o, int i, Char?       v) { d.SetNullableChar       (o, i, v); } }
			sealed class NG          : MB<Guid?>       { public override  void Set(IMapDataDestination d, object o, int i, Guid?       v) { d.SetNullableGuid       (o, i, v); } }
			sealed class NDT         : MB<DateTime?>   { public override  void Set(IMapDataDestination d, object o, int i, DateTime?   v) { d.SetNullableDateTime   (o, i, v); } }

			// SqlTypes.
			//
			sealed class dbS         : MB<SqlString>   { public override  void Set(IMapDataDestination d, object o, int i, SqlString   v) { d.SetSqlString   (o, i, v); } }

			sealed class dbU8        : MB<SqlByte>     { public override  void Set(IMapDataDestination d, object o, int i, SqlByte     v) { d.SetSqlByte     (o, i, v); } }
			sealed class dbI16       : MB<SqlInt16>    { public override  void Set(IMapDataDestination d, object o, int i, SqlInt16    v) { d.SetSqlInt16    (o, i, v); } }
			sealed class dbI32       : MB<SqlInt32>    { public override  void Set(IMapDataDestination d, object o, int i, SqlInt32    v) { d.SetSqlInt32    (o, i, v); } }
			sealed class dbI64       : MB<SqlInt64>    { public override  void Set(IMapDataDestination d, object o, int i, SqlInt64    v) { d.SetSqlInt64    (o, i, v); } }

			sealed class dbR4        : MB<SqlSingle>   { public override  void Set(IMapDataDestination d, object o, int i, SqlSingle   v) { d.SetSqlSingle   (o, i, v); } }
			sealed class dbR8        : MB<SqlDouble>   { public override  void Set(IMapDataDestination d, object o, int i, SqlDouble   v) { d.SetSqlDouble   (o, i, v); } }
			sealed class dbD         : MB<SqlDecimal>  { public override  void Set(IMapDataDestination d, object o, int i, SqlDecimal  v) { d.SetSqlDecimal  (o, i, v); } }
			sealed class dbM         : MB<SqlMoney>    { public override  void Set(IMapDataDestination d, object o, int i, SqlMoney    v) { d.SetSqlMoney    (o, i, v); } }

			sealed class dbB         : MB<SqlBoolean>  { public override  void Set(IMapDataDestination d, object o, int i, SqlBoolean  v) { d.SetSqlBoolean  (o, i, v); } }
			sealed class dbG         : MB<SqlGuid>     { public override  void Set(IMapDataDestination d, object o, int i, SqlGuid     v) { d.SetSqlGuid     (o, i, v); } }
			sealed class dbDT        : MB<SqlDateTime> { public override  void Set(IMapDataDestination d, object o, int i, SqlDateTime v) { d.SetSqlDateTime (o, i, v); } }
		}
	}
}
