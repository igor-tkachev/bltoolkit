using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Text;
using BLToolkit.Reflection;

namespace BLToolkit.Data.Sql
{
	public class SqlDataType : ISqlExpression
	{
		#region Init

		public SqlDataType(SqlDbType dbType)
		{
			var defaultType = GetDataType(dbType);

			_dbType    = dbType;
			_type      = defaultType.Type;
			_length    = defaultType.Length;
			_precision = defaultType.Precision;
			_scale     = defaultType.Scale;
		}

		public SqlDataType(SqlDbType dbType, int length)
		{
			if (length <= 0) throw new ArgumentOutOfRangeException("length");

			_dbType = dbType;
			_type   = GetDataType(dbType).Type;
			_length = length;
		}

		public SqlDataType(SqlDbType dbType, int precision, int scale)
		{
			if (precision <= 0) throw new ArgumentOutOfRangeException("precision");
			if (scale     <  0) throw new ArgumentOutOfRangeException("scale");

			_dbType    = dbType;
			_type      = GetDataType(dbType).Type;
			_precision = precision;
			_scale     = scale;
		}

		public SqlDataType([JetBrains.Annotations.NotNull]Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			var defaultType = GetDataType(type);

			_dbType    = defaultType.DbType;
			_type      = type;
			_length    = defaultType.Length;
			_precision = defaultType.Precision;
			_scale     = defaultType.Scale;
		}

		public SqlDataType([JetBrains.Annotations.NotNull] Type type, int length)
		{
			if (type   == null) throw new ArgumentNullException      ("type");
			if (length <= 0)    throw new ArgumentOutOfRangeException("length");

			_dbType = GetDataType(type).DbType;
			_type   = type;
			_length = length;
		}

		public SqlDataType([JetBrains.Annotations.NotNull] Type type, int precision, int scale)
		{
			if (type  == null)  throw new ArgumentNullException      ("type");
			if (precision <= 0) throw new ArgumentOutOfRangeException("precision");
			if (scale     <  0) throw new ArgumentOutOfRangeException("scale");

			_dbType    = GetDataType(type).DbType;
			_type      = type;
			_precision = precision;
			_scale     = scale;
		}

		public SqlDataType(SqlDbType dbType, [JetBrains.Annotations.NotNull]Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			var defaultType = GetDataType(dbType);

			_dbType    = dbType;
			_type      = type;
			_length    = defaultType.Length;
			_precision = defaultType.Precision;
			_scale     = defaultType.Scale;
		}

		public SqlDataType(SqlDbType dbType, [JetBrains.Annotations.NotNull] Type type, int length)
		{
			if (type   == null) throw new ArgumentNullException      ("type");
			if (length <= 0)    throw new ArgumentOutOfRangeException("length");

			_dbType = dbType;
			_type   = type;
			_length = length;
		}

		public SqlDataType(SqlDbType dbType, [JetBrains.Annotations.NotNull] Type type, int precision, int scale)
		{
			if (type  == null)  throw new ArgumentNullException      ("type");
			if (precision <= 0) throw new ArgumentOutOfRangeException("precision");
			if (scale     <  0) throw new ArgumentOutOfRangeException("scale");

			_dbType    = dbType;
			_type      = type;
			_precision = precision;
			_scale     = scale;
		}

		#endregion

		#region Public Members

		readonly SqlDbType _dbType;    public SqlDbType DbType    { get { return _dbType;    } }
		readonly Type      _type;      public Type      Type      { get { return _type;      } }
		readonly int       _length;    public int       Length    { get { return _length;    } }
		readonly int       _precision; public int       Precision { get { return _precision; } }
		readonly int       _scale;     public int       Scale     { get { return _scale;     } }

		#endregion

		#region Static Members

		struct TypeInfo
		{
			public TypeInfo(SqlDbType dbType, int maxLength, int maxPrecision, int maxScale, int maxDisplaySize)
			{
				DbType         = dbType;
				MaxLength      = maxLength;
				MaxPrecision   = maxPrecision;
				MaxScale       = maxScale;
				MaxDisplaySize = maxDisplaySize;
			}

			public readonly SqlDbType DbType;
			public readonly int       MaxLength;
			public readonly int       MaxPrecision;
			public readonly int       MaxScale;
			public readonly int       MaxDisplaySize;
		}

		static TypeInfo[] SortTypeInfo(params TypeInfo[] info)
		{
			var maxIndex = 0;

			foreach (var typeInfo in info)
				if (maxIndex < (int)typeInfo.DbType)
					maxIndex = (int)typeInfo.DbType;

			var sortedInfo = new TypeInfo[maxIndex + 1];

			foreach (var typeInfo in info)
				sortedInfo[(int)typeInfo.DbType] = typeInfo;

			return sortedInfo;
		}

		static int Len(object obj)
		{
			return obj.ToString().Length;
		}

		static readonly TypeInfo[] _typeInfo = SortTypeInfo
		(
			//           DbType                         MaxLength           MaxPrecision               MaxScale       MaxDisplaySize
			//
			new TypeInfo(SqlDbType.BigInt,                      8,   Len( long.MaxValue),                     0,     Len( long.MinValue)),
			new TypeInfo(SqlDbType.Int,                         4,   Len(  int.MaxValue),                     0,     Len(  int.MinValue)),
			new TypeInfo(SqlDbType.SmallInt,                    2,   Len(short.MaxValue),                     0,     Len(short.MinValue)),
			new TypeInfo(SqlDbType.TinyInt,                     1,   Len( byte.MaxValue),                     0,     Len( byte.MaxValue)),
			new TypeInfo(SqlDbType.Bit,                         1,                     1,                     0,                       1),

			new TypeInfo(SqlDbType.Decimal,                    17, Len(decimal.MaxValue), Len(decimal.MaxValue), Len(decimal.MinValue)+1),
			new TypeInfo(SqlDbType.Money,                       8,                    19,                     4,                  19 + 2),
			new TypeInfo(SqlDbType.SmallMoney,                  4,                    10,                     4,                  10 + 2),
			new TypeInfo(SqlDbType.Float,                       8,                    15,                    15,              15 + 2 + 5),
			new TypeInfo(SqlDbType.Real,                        4,                     7,                     7,               7 + 2 + 4),

			new TypeInfo(SqlDbType.DateTime,                    8,                    -1,                    -1,                      23),
			new TypeInfo(SqlDbType.DateTime2,                   8,                    -1,                    -1,                      27),
			new TypeInfo(SqlDbType.SmallDateTime,               4,                    -1,                    -1,                      19),
			new TypeInfo(SqlDbType.Date,                        3,                    -1,                    -1,                      10),
			new TypeInfo(SqlDbType.Time,                        5,                    -1,                    -1,                      16),
			new TypeInfo(SqlDbType.DateTimeOffset,             10,                    -1,                    -1,                      34),

			new TypeInfo(SqlDbType.Char,                     8000,                    -1,                    -1,                    8000),
			new TypeInfo(SqlDbType.VarChar,                  8000,                    -1,                    -1,                    8000),
			new TypeInfo(SqlDbType.Text,             int.MaxValue,                    -1,                    -1,            int.MaxValue),
			new TypeInfo(SqlDbType.NChar,                    4000,                    -1,                    -1,                    4000),
			new TypeInfo(SqlDbType.NVarChar,                 4000,                    -1,                    -1,                    4000),
			new TypeInfo(SqlDbType.NText,            int.MaxValue,                    -1,                    -1,        int.MaxValue / 2),

			new TypeInfo(SqlDbType.Binary,                   8000,                    -1,                    -1,                      -1),
			new TypeInfo(SqlDbType.VarBinary,                8000,                    -1,                    -1,                      -1),
			new TypeInfo(SqlDbType.Image,            int.MaxValue,                    -1,                    -1,                      -1),

			new TypeInfo(SqlDbType.Timestamp,                   8,                    -1,                    -1,                      -1),
			new TypeInfo(SqlDbType.UniqueIdentifier,           16,                    -1,                    -1,                      36),

			new TypeInfo(SqlDbType.Variant,                    -1,                    -1,                    -1,                      -1),
			new TypeInfo(SqlDbType.Xml,                        -1,                    -1,                    -1,                      -1),
			new TypeInfo(SqlDbType.Udt,                        -1,                    -1,                    -1,                      -1),
			new TypeInfo(SqlDbType.Structured,                 -1,                    -1,                    -1,                      -1)
		);

		public static int GetMaxLength     (SqlDbType dbType) { return _typeInfo[(int)dbType].MaxLength;      }
		public static int GetMaxPrecision  (SqlDbType dbType) { return _typeInfo[(int)dbType].MaxPrecision;   }
		public static int GetMaxScale      (SqlDbType dbType) { return _typeInfo[(int)dbType].MaxScale;       }
		public static int GetMaxDisplaySize(SqlDbType dbType) { return _typeInfo[(int)dbType].MaxDisplaySize; }

		public static SqlDataType GetDataType(Type type)
		{
			var underlyingType = type;

			if (underlyingType.IsGenericType && underlyingType.GetGenericTypeDefinition() == typeof(Nullable<>))
				underlyingType = underlyingType.GetGenericArguments()[0];

			if (underlyingType.IsEnum)
				underlyingType = Enum.GetUnderlyingType(underlyingType);

			switch (Type.GetTypeCode(underlyingType))
			{
				case TypeCode.Boolean  : return Boolean;
				case TypeCode.Char     : return Char;
				case TypeCode.SByte    : return SByte;
				case TypeCode.Byte     : return Byte;
				case TypeCode.Int16    : return Int16;
				case TypeCode.UInt16   : return UInt16;
				case TypeCode.Int32    : return Int32;
				case TypeCode.UInt32   : return UInt32;
				case TypeCode.Int64    : return Int64;
				case TypeCode.UInt64   : return UInt64;
				case TypeCode.Single   : return Single;
				case TypeCode.Double   : return Double;
				case TypeCode.Decimal  : return Decimal;
				case TypeCode.DateTime : return DateTime;
				case TypeCode.String   : return String;
				case TypeCode.Object   :
					if (type == typeof(Guid))           return Guid;
					if (type == typeof(byte[]))         return ByteArray;
#if FW3
					if (type == typeof(System.Data.Linq.Binary)) return LinqBinary;
#endif
					if (type == typeof(char[]))         return CharArray;
					if (type == typeof(DateTimeOffset)) return DateTimeOffset;
					if (type == typeof(TimeSpan))       return TimeSpan;
					break;

				case TypeCode.DBNull   :
				case TypeCode.Empty    :
				default                : break;
			}

			if (type == typeof(SqlByte))     return SqlByte;
			if (type == typeof(SqlInt16))    return SqlInt16;
			if (type == typeof(SqlInt32))    return SqlInt32;
			if (type == typeof(SqlInt64))    return SqlInt64;
			if (type == typeof(SqlSingle))   return SqlSingle;
			if (type == typeof(SqlBoolean))  return SqlBoolean;
			if (type == typeof(SqlDouble))   return SqlDouble;
			if (type == typeof(SqlDateTime)) return SqlDateTime;
			if (type == typeof(SqlDecimal))  return SqlDecimal;
			if (type == typeof(SqlMoney))    return SqlMoney;
			if (type == typeof(SqlString))   return SqlString;
			if (type == typeof(SqlBinary))   return SqlBinary;
			if (type == typeof(SqlGuid))     return SqlGuid;
			if (type == typeof(SqlBytes))    return SqlBytes;
			if (type == typeof(SqlChars))    return SqlChars;
			if (type == typeof(SqlXml))      return SqlXml;

			return DbVariant;
		}

		public static SqlDataType GetDataType(SqlDbType type)
		{
			switch (type)
			{
				case SqlDbType.BigInt           : return DbBigInt;
				case SqlDbType.Binary           : return DbBinary;
				case SqlDbType.Bit              : return DbBit;
				case SqlDbType.Char             : return DbChar;
				case SqlDbType.DateTime         : return DbDateTime;
				case SqlDbType.Decimal          : return DbDecimal;
				case SqlDbType.Float            : return DbFloat;
				case SqlDbType.Image            : return DbImage;
				case SqlDbType.Int              : return DbInt;
				case SqlDbType.Money            : return DbMoney;
				case SqlDbType.NChar            : return DbNChar;
				case SqlDbType.NText            : return DbNText;
				case SqlDbType.NVarChar         : return DbNVarChar;
				case SqlDbType.Real             : return DbReal;
				case SqlDbType.UniqueIdentifier : return DbUniqueIdentifier;
				case SqlDbType.SmallDateTime    : return DbSmallDateTime;
				case SqlDbType.SmallInt         : return DbSmallInt;
				case SqlDbType.SmallMoney       : return DbSmallMoney;
				case SqlDbType.Text             : return DbText;
				case SqlDbType.Timestamp        : return DbTimestamp;
				case SqlDbType.TinyInt          : return DbTinyInt;
				case SqlDbType.VarBinary        : return DbVarBinary;
				case SqlDbType.VarChar          : return DbVarChar;
				case SqlDbType.Variant          : return DbVariant;
				case SqlDbType.Xml              : return DbXml;
				case SqlDbType.Udt              : return DbUdt;
				case SqlDbType.Structured       : return DbStructured;
				case SqlDbType.Date             : return DbDate;
				case SqlDbType.Time             : return DbTime;
				case SqlDbType.DateTime2        : return DbDateTime2;
				case SqlDbType.DateTimeOffset   : return DbDateTimeOffset;
			}

			throw new InvalidOperationException();
		}

		public static bool CanBeNull(Type type)
		{
			if (type.IsValueType == false ||
				type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) ||
				TypeHelper.IsSameOrParent(typeof(INullable), type))
				return true;

			return false;
		}

		#endregion

		#region Default Types

		SqlDataType(SqlDbType dbType, Type type, int length, int precision, int scale)
		{
			_dbType    = dbType;
			_type      = type;
			_length    = length;
			_precision = precision;
			_scale     = scale;
		}

		SqlDataType(SqlDbType dbType, Type type, Func<SqlDbType,int> length, int precision, int scale)
			: this(dbType, type, length(dbType), precision, scale)
		{
		}

		SqlDataType(SqlDbType dbType, Type type, int length, Func<SqlDbType,int> precision, int scale)
			: this(dbType, type, length, precision(dbType), scale)
		{
		}

		public static readonly SqlDataType DbBigInt           = new SqlDataType(SqlDbType.BigInt,           typeof(Int64),                  0, 0,                0);
		public static readonly SqlDataType DbInt              = new SqlDataType(SqlDbType.Int,              typeof(Int32),                  0, 0,                0);
		public static readonly SqlDataType DbSmallInt         = new SqlDataType(SqlDbType.SmallInt,         typeof(Int16),                  0, 0,                0);
		public static readonly SqlDataType DbTinyInt          = new SqlDataType(SqlDbType.TinyInt,          typeof(Byte),                   0, 0,                0);
		public static readonly SqlDataType DbBit              = new SqlDataType(SqlDbType.Bit,              typeof(Boolean),                0, 0,                0);

		public static readonly SqlDataType DbDecimal          = new SqlDataType(SqlDbType.Decimal,          typeof(Decimal),                0, GetMaxPrecision, 10);
		public static readonly SqlDataType DbMoney            = new SqlDataType(SqlDbType.Money,            typeof(Decimal),                0, GetMaxPrecision,  4);
		public static readonly SqlDataType DbSmallMoney       = new SqlDataType(SqlDbType.SmallMoney,       typeof(Decimal),                0, GetMaxPrecision,  4);
		public static readonly SqlDataType DbFloat            = new SqlDataType(SqlDbType.Float,            typeof(Double),                 0,               0,  0);
		public static readonly SqlDataType DbReal             = new SqlDataType(SqlDbType.Real,             typeof(Single),                 0,               0,  0);

		public static readonly SqlDataType DbDateTime         = new SqlDataType(SqlDbType.DateTime,         typeof(DateTime),               0,               0,  0);
		public static readonly SqlDataType DbDateTime2        = new SqlDataType(SqlDbType.DateTime2,        typeof(DateTime),               0,               0,  0);
		public static readonly SqlDataType DbSmallDateTime    = new SqlDataType(SqlDbType.SmallDateTime,    typeof(DateTime),               0,               0,  0);
		public static readonly SqlDataType DbDate             = new SqlDataType(SqlDbType.Date,             typeof(DateTime),               0,               0,  0);
		public static readonly SqlDataType DbTime             = new SqlDataType(SqlDbType.Time,             typeof(TimeSpan),               0,               0,  0);
		public static readonly SqlDataType DbDateTimeOffset   = new SqlDataType(SqlDbType.DateTimeOffset,   typeof(DateTimeOffset),         0,               0,  0);

		public static readonly SqlDataType DbChar             = new SqlDataType(SqlDbType.Char,             typeof(String),      GetMaxLength,               0,  0);
		public static readonly SqlDataType DbVarChar          = new SqlDataType(SqlDbType.VarChar,          typeof(String),      GetMaxLength,               0,  0);
		public static readonly SqlDataType DbText             = new SqlDataType(SqlDbType.Text,             typeof(String),      GetMaxLength,               0,  0);
		public static readonly SqlDataType DbNChar            = new SqlDataType(SqlDbType.NChar,            typeof(String),      GetMaxLength,               0,  0);
		public static readonly SqlDataType DbNVarChar         = new SqlDataType(SqlDbType.NVarChar,         typeof(String),      GetMaxLength,               0,  0);
		public static readonly SqlDataType DbNText            = new SqlDataType(SqlDbType.NText,            typeof(String),      GetMaxLength,               0,  0);

		public static readonly SqlDataType DbBinary           = new SqlDataType(SqlDbType.Binary,           typeof(Byte[]),      GetMaxLength,               0,  0);
		public static readonly SqlDataType DbVarBinary        = new SqlDataType(SqlDbType.VarBinary,        typeof(Byte[]),      GetMaxLength,               0,  0);
		public static readonly SqlDataType DbImage            = new SqlDataType(SqlDbType.Image,            typeof(Byte[]),      GetMaxLength,               0,  0);

		public static readonly SqlDataType DbTimestamp        = new SqlDataType(SqlDbType.Timestamp,        typeof(Byte[]),                 0,               0,  0);
		public static readonly SqlDataType DbUniqueIdentifier = new SqlDataType(SqlDbType.UniqueIdentifier, typeof(Guid),                   0,               0,  0);

		public static readonly SqlDataType DbVariant          = new SqlDataType(SqlDbType.Variant,          typeof(Object),                 0,               0,  0);
		public static readonly SqlDataType DbXml              = new SqlDataType(SqlDbType.Xml,              typeof(SqlXml),                 0,               0,  0);
		public static readonly SqlDataType DbUdt              = new SqlDataType(SqlDbType.Udt,              typeof(Object),                 0,               0,  0);
		public static readonly SqlDataType DbStructured       = new SqlDataType(SqlDbType.Structured,       typeof(Object),                 0,               0,  0);

		public static readonly SqlDataType Boolean            = DbBit;
		public static readonly SqlDataType Char               = new SqlDataType(SqlDbType.Char,             typeof(Char),                   1,               0,  0);
		public static readonly SqlDataType SByte              = new SqlDataType(SqlDbType.SmallInt,         typeof(SByte),                  0,               0,  0);
		public static readonly SqlDataType Byte               = DbTinyInt;
		public static readonly SqlDataType Int16              = DbSmallInt;
		public static readonly SqlDataType UInt16             = new SqlDataType(SqlDbType.Int,              typeof(UInt16),                 0,               0,  0);
		public static readonly SqlDataType Int32              = DbInt;
		public static readonly SqlDataType UInt32             = new SqlDataType(SqlDbType.BigInt,           typeof(UInt32),                 0,               0,  0);
		public static readonly SqlDataType Int64              = DbBigInt;
		public static readonly SqlDataType UInt64             = new SqlDataType(SqlDbType.Decimal,          typeof(UInt64),                 0, ulong.MaxValue.ToString().Length, 0);
		public static readonly SqlDataType Single             = DbReal;
		public static readonly SqlDataType Double             = DbFloat;
		public static readonly SqlDataType Decimal            = DbDecimal;
		public static readonly SqlDataType DateTime           = DbDateTime2;
		public static readonly SqlDataType String             = DbNVarChar;
		public static readonly SqlDataType Guid               = DbUniqueIdentifier;
		public static readonly SqlDataType ByteArray          = DbVarBinary;
		public static readonly SqlDataType LinqBinary         = DbVarBinary;
		public static readonly SqlDataType CharArray          = new SqlDataType(SqlDbType.NVarChar,         typeof(Char[]),      GetMaxLength,               0,  0);
		public static readonly SqlDataType DateTimeOffset     = DbDateTimeOffset;
		public static readonly SqlDataType TimeSpan           = DbTime;

		public static readonly SqlDataType SqlByte            = new SqlDataType(SqlDbType.TinyInt,          typeof(SqlByte),                0,               0,  0);
		public static readonly SqlDataType SqlInt16           = new SqlDataType(SqlDbType.SmallInt,         typeof(SqlInt16),               0,               0,  0);
		public static readonly SqlDataType SqlInt32           = new SqlDataType(SqlDbType.Int,              typeof(SqlInt32),               0,               0,  0);
		public static readonly SqlDataType SqlInt64           = new SqlDataType(SqlDbType.BigInt,           typeof(SqlInt64),               0,               0,  0);
		public static readonly SqlDataType SqlSingle          = new SqlDataType(SqlDbType.Real,             typeof(SqlSingle),              0,               0,  0);
		public static readonly SqlDataType SqlBoolean         = new SqlDataType(SqlDbType.Bit,              typeof(SqlBoolean),             0,               0,  0);
		public static readonly SqlDataType SqlDouble          = new SqlDataType(SqlDbType.Float,            typeof(SqlDouble),              0,               0,  0);
		public static readonly SqlDataType SqlDateTime        = new SqlDataType(SqlDbType.DateTime,         typeof(SqlDateTime),            0,               0,  0);
		public static readonly SqlDataType SqlDecimal         = new SqlDataType(SqlDbType.Decimal,          typeof(SqlDecimal),             0, GetMaxPrecision, 10);
		public static readonly SqlDataType SqlMoney           = new SqlDataType(SqlDbType.Money,            typeof(SqlMoney),               0, GetMaxPrecision,  4);
		public static readonly SqlDataType SqlString          = new SqlDataType(SqlDbType.NVarChar,         typeof(SqlString),   GetMaxLength,               0,  0);
		public static readonly SqlDataType SqlBinary          = new SqlDataType(SqlDbType.Binary,           typeof(SqlBinary),   GetMaxLength,               0,  0);
		public static readonly SqlDataType SqlGuid            = new SqlDataType(SqlDbType.UniqueIdentifier, typeof(SqlGuid),                0,               0,  0);
		public static readonly SqlDataType SqlBytes           = new SqlDataType(SqlDbType.Image,            typeof(SqlBytes),    GetMaxLength,               0,  0);
		public static readonly SqlDataType SqlChars           = new SqlDataType(SqlDbType.Text,             typeof(SqlChars),    GetMaxLength,               0,  0);
		public static readonly SqlDataType SqlXml             = new SqlDataType(SqlDbType.Xml,              typeof(SqlXml),                 0,               0,  0);

		#endregion

		#region Overrides

#if OVERRIDETOSTRING

		public override string ToString()
		{
			return ((IQueryElement)this).ToString(new StringBuilder(), new Dictionary<IQueryElement,IQueryElement>()).ToString();
		}

#endif

		#endregion

		#region ISqlExpression Members

		public int Precedence
		{
			get { return Sql.Precedence.Primary; }
		}

		public Type SystemType
		{
			get { return typeof (Type); }
		}

		#endregion

		#region ISqlExpressionWalkable Members

		ISqlExpression ISqlExpressionWalkable.Walk(bool skipColumns, Func<ISqlExpression,ISqlExpression> func)
		{
			return func(this);
		}

		#endregion

		#region IEquatable<ISqlExpression> Members

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			if (this == other)
				return true;

			var value = other as SqlDataType;
			return _type == value._type && _length == value._length && _precision == value._precision && _scale == value._scale;
		}

		#endregion

		#region ISqlExpression Members

		public bool CanBeNull()
		{
			return false;
		}

		#endregion

		#region ICloneableElement Members

		public ICloneableElement Clone(Dictionary<ICloneableElement, ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
		{
			if (!doClone(this))
				return this;

			ICloneableElement clone;

			if (!objectTree.TryGetValue(this, out clone))
				objectTree.Add(this, clone = new SqlDataType(_dbType, _type, _length, _precision, _scale));

			return clone;
		}

		#endregion

		#region IQueryElement Members

		public QueryElementType ElementType { get { return QueryElementType.SqlDataType; } }

		StringBuilder IQueryElement.ToString(StringBuilder sb, Dictionary<IQueryElement,IQueryElement> dic)
		{
			sb.Append(_dbType);

			if (_length != 0)
				sb.Append('(').Append(_length).Append(')');
			else if (_precision != 0)
				sb.Append('(').Append(_precision).Append(',').Append(_scale).Append(')');

			return sb;
		}

		#endregion
	}
}
