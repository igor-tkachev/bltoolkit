using System;
using System.Data.SqlTypes;
using System.Reflection;

using JetBrains.Annotations;

namespace BLToolkit.Reflection
{
	public abstract class StorageMemberAccessor : MemberAccessor
	{
		protected StorageMemberAccessor(TypeAccessor typeAccessor, MemberInfo memberInfo, [NotNull] string storage)
			: base(typeAccessor, memberInfo)
		{
			if (storage == null) throw new ArgumentNullException("storage");

			_storage = storage;
		}

		#region Public Properties

		private readonly string _storage;
		public           string  Storage
		{
			get { return _storage; }
		}

		public override bool HasGetter { get { return true; } }
		public override bool HasSetter { get { return true; } }

		#endregion

		static public StorageMemberAccessor GetMemberAccessor(TypeAccessor typeAccessor, MemberInfo memberInfo, [NotNull] string storage)
		{
			if (storage == null) throw new ArgumentNullException("storage");

			var type = memberInfo is PropertyInfo?
					((PropertyInfo)memberInfo).PropertyType:
					((FieldInfo)   memberInfo).FieldType;

			var underlyingType = type;
			var isNullable     = false;

			if (underlyingType.IsGenericType && underlyingType.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				underlyingType = underlyingType.GetGenericArguments()[0];
				isNullable     = true;
			}

			if (underlyingType.IsEnum)
				underlyingType = Enum.GetUnderlyingType(underlyingType);

			if (isNullable)
			{
				switch (Type.GetTypeCode(underlyingType))
				{
					case TypeCode.Boolean  : return new NullableAccessor<Boolean> (typeAccessor, memberInfo, storage);
					case TypeCode.Char     : return new NullableAccessor<Char>    (typeAccessor, memberInfo, storage);
					case TypeCode.SByte    : return new NullableAccessor<SByte>   (typeAccessor, memberInfo, storage);
					case TypeCode.Byte     : return new NullableAccessor<Byte>    (typeAccessor, memberInfo, storage);
					case TypeCode.Int16    : return new NullableAccessor<Int16>   (typeAccessor, memberInfo, storage);
					case TypeCode.UInt16   : return new NullableAccessor<UInt16>  (typeAccessor, memberInfo, storage);
					case TypeCode.Int32    : return new NullableAccessor<Int32>   (typeAccessor, memberInfo, storage);
					case TypeCode.UInt32   : return new NullableAccessor<UInt32>  (typeAccessor, memberInfo, storage);
					case TypeCode.Int64    : return new NullableAccessor<Int64>   (typeAccessor, memberInfo, storage);
					case TypeCode.UInt64   : return new NullableAccessor<UInt64>  (typeAccessor, memberInfo, storage);
					case TypeCode.Single   : return new NullableAccessor<Single>  (typeAccessor, memberInfo, storage);
					case TypeCode.Double   : return new NullableAccessor<Double>  (typeAccessor, memberInfo, storage);
					case TypeCode.Decimal  : return new NullableAccessor<Decimal> (typeAccessor, memberInfo, storage);
					case TypeCode.DateTime : return new NullableAccessor<DateTime>(typeAccessor, memberInfo, storage);
					case TypeCode.Object   :
						if (type == typeof(Guid))           return new NullableAccessor<Guid>          (typeAccessor, memberInfo, storage);
						if (type == typeof(DateTimeOffset)) return new NullableAccessor<DateTimeOffset>(typeAccessor, memberInfo, storage);
						if (type == typeof(TimeSpan))       return new NullableAccessor<TimeSpan>      (typeAccessor, memberInfo, storage);
						break;
					default                : break;
				}
			}
			else
			{
				switch (Type.GetTypeCode(underlyingType))
				{
					case TypeCode.Boolean  : return new SimpleAccessor<Boolean> (typeAccessor, memberInfo, storage);
					case TypeCode.Char     : return new SimpleAccessor<Char>    (typeAccessor, memberInfo, storage);
					case TypeCode.SByte    : return new SimpleAccessor<SByte>   (typeAccessor, memberInfo, storage);
					case TypeCode.Byte     : return new SimpleAccessor<Byte>    (typeAccessor, memberInfo, storage);
					case TypeCode.Int16    : return new SimpleAccessor<Int16>   (typeAccessor, memberInfo, storage);
					case TypeCode.UInt16   : return new SimpleAccessor<UInt16>  (typeAccessor, memberInfo, storage);
					case TypeCode.Int32    : return new SimpleAccessor<Int32>   (typeAccessor, memberInfo, storage);
					case TypeCode.UInt32   : return new SimpleAccessor<UInt32>  (typeAccessor, memberInfo, storage);
					case TypeCode.Int64    : return new SimpleAccessor<Int64>   (typeAccessor, memberInfo, storage);
					case TypeCode.UInt64   : return new SimpleAccessor<UInt64>  (typeAccessor, memberInfo, storage);
					case TypeCode.Single   : return new SimpleAccessor<Single>  (typeAccessor, memberInfo, storage);
					case TypeCode.Double   : return new SimpleAccessor<Double>  (typeAccessor, memberInfo, storage);
					case TypeCode.Decimal  : return new SimpleAccessor<Decimal> (typeAccessor, memberInfo, storage);
					case TypeCode.DateTime : return new SimpleAccessor<DateTime>(typeAccessor, memberInfo, storage);
					case TypeCode.Object   :
						if (type == typeof(Guid))           return new SimpleAccessor<Guid>          (typeAccessor, memberInfo, storage);
						if (type == typeof(DateTimeOffset)) return new SimpleAccessor<DateTimeOffset>(typeAccessor, memberInfo, storage);
						if (type == typeof(TimeSpan))       return new SimpleAccessor<TimeSpan>      (typeAccessor, memberInfo, storage);
						break;
					default                : break;
				}
			}

			if (type == typeof(SqlByte))     return new SimpleAccessor<SqlByte>    (typeAccessor, memberInfo, storage);
			if (type == typeof(SqlInt16))    return new SimpleAccessor<SqlInt16>   (typeAccessor, memberInfo, storage);
			if (type == typeof(SqlInt32))    return new SimpleAccessor<SqlInt32>   (typeAccessor, memberInfo, storage);
			if (type == typeof(SqlInt64))    return new SimpleAccessor<SqlInt64>   (typeAccessor, memberInfo, storage);
			if (type == typeof(SqlSingle))   return new SimpleAccessor<SqlSingle>  (typeAccessor, memberInfo, storage);
			if (type == typeof(SqlBoolean))  return new SimpleAccessor<SqlBoolean> (typeAccessor, memberInfo, storage);
			if (type == typeof(SqlDouble))   return new SimpleAccessor<SqlDouble>  (typeAccessor, memberInfo, storage);
			if (type == typeof(SqlDateTime)) return new SimpleAccessor<SqlDateTime>(typeAccessor, memberInfo, storage);
			if (type == typeof(SqlDecimal))  return new SimpleAccessor<SqlDecimal> (typeAccessor, memberInfo, storage);
			if (type == typeof(SqlMoney))    return new SimpleAccessor<SqlMoney>   (typeAccessor, memberInfo, storage);
			if (type == typeof(SqlString))   return new SimpleAccessor<SqlString>  (typeAccessor, memberInfo, storage);
			if (type == typeof(SqlBinary))   return new SimpleAccessor<SqlBinary>  (typeAccessor, memberInfo, storage);
			if (type == typeof(SqlGuid))     return new SimpleAccessor<SqlGuid>    (typeAccessor, memberInfo, storage);
			if (type == typeof(SqlBytes))    return new SimpleAccessor<SqlBytes>   (typeAccessor, memberInfo, storage);
			if (type == typeof(SqlChars))    return new SimpleAccessor<SqlChars>   (typeAccessor, memberInfo, storage);
			if (type == typeof(SqlXml))      return new SimpleAccessor<SqlXml>     (typeAccessor, memberInfo, storage);

			return null;
		}

		class SimpleAccessor<T> : StorageMemberAccessor
		{
			public SimpleAccessor(TypeAccessor typeAccessor, MemberInfo memberInfo, string storage)
				: base(typeAccessor, memberInfo, storage)
			{
			}
		}

		class NullableAccessor<T> : StorageMemberAccessor
			where T : struct
		{
			public NullableAccessor(TypeAccessor typeAccessor, MemberInfo memberInfo, string storage)
				: base(typeAccessor, memberInfo, storage)
			{
			}
		}
	}
}
