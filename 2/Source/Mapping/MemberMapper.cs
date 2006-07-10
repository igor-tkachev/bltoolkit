using System;
using System.Data.SqlTypes;
using System.IO;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class MemberMapper
	{
		#region Init

		public virtual void Init(MapMemberInfo mapMemberInfo)
		{
			if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

			_mapMemberInfo  = mapMemberInfo;
			_name           = mapMemberInfo.Name;
			_type           = mapMemberInfo.Type;
			_memberName     = mapMemberInfo.MemberName;
			_memberAccessor = mapMemberInfo.MemberAccessor;
			_mappingSchema  = mapMemberInfo.MappingSchema;
		}

		internal static MemberMapper CreateMemberMapper(MapMemberInfo mi)
		{
			Type         type = mi.Type;
			MemberMapper mm   = null;

			if (type.IsPrimitive || type.IsEnum)
				mm = GetPrimitiveMemberMapper(mi);

#if FW2
			if (mm == null)
			{
				mm = GetNullableMemberMapper(mi);

				//if (mm != null)
				//    mi.IsNullable = true;
			}
#endif

			if (mm == null) mm = GetSimpleMemberMapper(mi);
			if (mm == null) mm = GetSqlTypeMemberMapper(mi);
			if (mm == null) mm = new DefaultMemberMapper();

			return mm;
		}

		#endregion

		#region Public Properties

		private MapMemberInfo _mapMemberInfo;
		public  MapMemberInfo  MapMemberInfo
		{
			get { return _mapMemberInfo; }
		}

		private int _ordinal;
		public  int  Ordinal
		{
			get { return _ordinal; }
		}

		internal void SetOrdinal(int ordinal)
		{
			_ordinal = ordinal;
		}

		private MemberAccessor _memberAccessor;
		public  MemberAccessor  MemberAccessor
		{
			get { return _memberAccessor; }
		}

		private MappingSchema _mappingSchema;
		public  MappingSchema  MappingSchema
		{
			get { return _mappingSchema; }
		}

		private string _name;
		public  string  Name
		{
			get { return _name; }
		}

		private string _memberName;
		public  string  MemberName
		{
			get { return _memberName; }
		}

		private Type  _type;
		public  Type   Type
		{
			get { return _type; }
		}

		#endregion

		#region Default Members (GetValue, SetValue)

		public virtual bool SupportsValue { get { return true; } }

		public virtual object GetValue(object o)
		{
			return _memberAccessor.GetValue(o);
		}

		public virtual bool     IsNull     (object o) { return GetValue(o) == null; }

		// Simple type getters.
		//
		[CLSCompliant(false)]
		public virtual SByte    GetSByte   (object o) { return _memberAccessor.GetSByte   (o); }
		public virtual Int16    GetInt16   (object o) { return _memberAccessor.GetInt16   (o); }
		public virtual Int32    GetInt32   (object o) { return _memberAccessor.GetInt32   (o); }
		public virtual Int64    GetInt64   (object o) { return _memberAccessor.GetInt64   (o); }

		public virtual Byte     GetByte    (object o) { return _memberAccessor.GetByte    (o); }
		[CLSCompliant(false)]
		public virtual UInt16   GetUInt16  (object o) { return _memberAccessor.GetUInt16  (o); }
		[CLSCompliant(false)]
		public virtual UInt32   GetUInt32  (object o) { return _memberAccessor.GetUInt32  (o); }
		[CLSCompliant(false)]
		public virtual UInt64   GetUInt64  (object o) { return _memberAccessor.GetUInt64  (o); }

		public virtual Boolean  GetBoolean (object o) { return _memberAccessor.GetBoolean (o); }
		public virtual Char     GetChar    (object o) { return _memberAccessor.GetChar    (o); }
		public virtual Single   GetSingle  (object o) { return _memberAccessor.GetSingle  (o); }
		public virtual Double   GetDouble  (object o) { return _memberAccessor.GetDouble  (o); }
		public virtual Decimal  GetDecimal (object o) { return _memberAccessor.GetDecimal (o); }
		public virtual Guid     GetGuid    (object o) { return _memberAccessor.GetGuid    (o); }
		public virtual DateTime GetDateTime(object o) { return _memberAccessor.GetDateTime(o); }

#if FW2
		// Nullable type getters.
		//
		[CLSCompliant(false)]
		public virtual SByte?    GetNullableSByte   (object o) { return _memberAccessor.GetNullableSByte   (o); }
		public virtual Int16?    GetNullableInt16   (object o) { return _memberAccessor.GetNullableInt16   (o); }
		public virtual Int32?    GetNullableInt32   (object o) { return _memberAccessor.GetNullableInt32   (o); }
		public virtual Int64?    GetNullableInt64   (object o) { return _memberAccessor.GetNullableInt64   (o); }

		public virtual Byte?     GetNullableByte    (object o) { return _memberAccessor.GetNullableByte    (o); }
		[CLSCompliant(false)]
		public virtual UInt16?   GetNullableUInt16  (object o) { return _memberAccessor.GetNullableUInt16  (o); }
		[CLSCompliant(false)]
		public virtual UInt32?   GetNullableUInt32  (object o) { return _memberAccessor.GetNullableUInt32  (o); }
		[CLSCompliant(false)]
		public virtual UInt64?   GetNullableUInt64  (object o) { return _memberAccessor.GetNullableUInt64  (o); }

		public virtual Boolean?  GetNullableBoolean (object o) { return _memberAccessor.GetNullableBoolean (o); }
		public virtual Char?     GetNullableChar    (object o) { return _memberAccessor.GetNullableChar    (o); }
		public virtual Single?   GetNullableSingle  (object o) { return _memberAccessor.GetNullableSingle  (o); }
		public virtual Double?   GetNullableDouble  (object o) { return _memberAccessor.GetNullableDouble  (o); }
		public virtual Decimal?  GetNullableDecimal (object o) { return _memberAccessor.GetNullableDecimal (o); }
		public virtual Guid?     GetNullableGuid    (object o) { return _memberAccessor.GetNullableGuid    (o); }
		public virtual DateTime? GetNullableDateTime(object o) { return _memberAccessor.GetNullableDateTime(o); }
#endif

		// SQL type getters.
		//
		public virtual SqlByte     GetSqlByte    (object o) { return _memberAccessor.GetSqlByte    (o); }
		public virtual SqlInt16    GetSqlInt16   (object o) { return _memberAccessor.GetSqlInt16   (o); }
		public virtual SqlInt32    GetSqlInt32   (object o) { return _memberAccessor.GetSqlInt32   (o); }
		public virtual SqlInt64    GetSqlInt64   (object o) { return _memberAccessor.GetSqlInt64   (o); }
		public virtual SqlSingle   GetSqlSingle  (object o) { return _memberAccessor.GetSqlSingle  (o); }
		public virtual SqlBoolean  GetSqlBoolean (object o) { return _memberAccessor.GetSqlBoolean (o); }
		public virtual SqlDouble   GetSqlDouble  (object o) { return _memberAccessor.GetSqlDouble  (o); }
		public virtual SqlDateTime GetSqlDateTime(object o) { return _memberAccessor.GetSqlDateTime(o); }
		public virtual SqlDecimal  GetSqlDecimal (object o) { return _memberAccessor.GetSqlDecimal (o); }
		public virtual SqlMoney    GetSqlMoney   (object o) { return _memberAccessor.GetSqlMoney   (o); }
		public virtual SqlGuid     GetSqlGuid    (object o) { return _memberAccessor.GetSqlGuid    (o); }
		public virtual SqlString   GetSqlString  (object o) { return _memberAccessor.GetSqlString  (o); }

		public virtual void SetValue(object o, object value)
		{
			_memberAccessor.SetValue(o, value);
		}

		public virtual void SetNull   (object o)                { SetValue(o, null); }

		// Simple type setters.
		//
		[CLSCompliant(false)]
		public virtual void SetSByte   (object o, SByte    value) { _memberAccessor.SetSByte   (o, value); }
		public virtual void SetInt16   (object o, Int16    value) { _memberAccessor.SetInt16   (o, value); }
		public virtual void SetInt32   (object o, Int32    value) { _memberAccessor.SetInt32   (o, value); }
		public virtual void SetInt64   (object o, Int64    value) { _memberAccessor.SetInt64   (o, value); }

		public virtual void SetByte    (object o, Byte     value) { _memberAccessor.SetByte    (o, value); }
		[CLSCompliant(false)]
		public virtual void SetUInt16  (object o, UInt16   value) { _memberAccessor.SetUInt16  (o, value); }
		[CLSCompliant(false)]
		public virtual void SetUInt32  (object o, UInt32   value) { _memberAccessor.SetUInt32  (o, value); }
		[CLSCompliant(false)]
		public virtual void SetUInt64  (object o, UInt64   value) { _memberAccessor.SetUInt64  (o, value); }

		public virtual void SetBoolean (object o, Boolean  value) { _memberAccessor.SetBoolean (o, value); }
		public virtual void SetChar    (object o, Char     value) { _memberAccessor.SetChar    (o, value); }
		public virtual void SetSingle  (object o, Single   value) { _memberAccessor.SetSingle  (o, value); }
		public virtual void SetDouble  (object o, Double   value) { _memberAccessor.SetDouble  (o, value); }
		public virtual void SetDecimal (object o, Decimal  value) { _memberAccessor.SetDecimal (o, value); }
		public virtual void SetGuid    (object o, Guid     value) { _memberAccessor.SetGuid    (o, value); }
		public virtual void SetDateTime(object o, DateTime value) { _memberAccessor.SetDateTime(o, value); }

#if FW2
		// Nullable type setters.
		//
		[CLSCompliant(false)]
		public virtual void SetNullableSByte   (object o, SByte?    value) { _memberAccessor.SetNullableSByte   (o, value); }
		public virtual void SetNullableInt16   (object o, Int16?    value) { _memberAccessor.SetNullableInt16   (o, value); }
		public virtual void SetNullableInt32   (object o, Int32?    value) { _memberAccessor.SetNullableInt32   (o, value); }
		public virtual void SetNullableInt64   (object o, Int64?    value) { _memberAccessor.SetNullableInt64   (o, value); }

		public virtual void SetNullableByte    (object o, Byte?     value) { _memberAccessor.SetNullableByte    (o, value); }
		[CLSCompliant(false)]
		public virtual void SetNullableUInt16  (object o, UInt16?   value) { _memberAccessor.SetNullableUInt16  (o, value); }
		[CLSCompliant(false)]
		public virtual void SetNullableUInt32  (object o, UInt32?   value) { _memberAccessor.SetNullableUInt32  (o, value); }
		[CLSCompliant(false)]
		public virtual void SetNullableUInt64  (object o, UInt64?   value) { _memberAccessor.SetNullableUInt64  (o, value); }

		public virtual void SetNullableBoolean (object o, Boolean?  value) { _memberAccessor.SetNullableBoolean (o, value); }
		public virtual void SetNullableChar    (object o, Char?     value) { _memberAccessor.SetNullableChar    (o, value); }
		public virtual void SetNullableSingle  (object o, Single?   value) { _memberAccessor.SetNullableSingle  (o, value); }
		public virtual void SetNullableDouble  (object o, Double?   value) { _memberAccessor.SetNullableDouble  (o, value); }
		public virtual void SetNullableDecimal (object o, Decimal?  value) { _memberAccessor.SetNullableDecimal (o, value); }
		public virtual void SetNullableGuid    (object o, Guid?     value) { _memberAccessor.SetNullableGuid    (o, value); }
		public virtual void SetNullableDateTime(object o, DateTime? value) { _memberAccessor.SetNullableDateTime(o, value); }
#endif

		// SQL type setters.
		//
		public virtual void SetSqlByte    (object o, SqlByte     value) { _memberAccessor.SetSqlByte    (o, value); }
		public virtual void SetSqlInt16   (object o, SqlInt16    value) { _memberAccessor.SetSqlInt16   (o, value); }
		public virtual void SetSqlInt32   (object o, SqlInt32    value) { _memberAccessor.SetSqlInt32   (o, value); }
		public virtual void SetSqlInt64   (object o, SqlInt64    value) { _memberAccessor.SetSqlInt64   (o, value); }
		public virtual void SetSqlSingle  (object o, SqlSingle   value) { _memberAccessor.SetSqlSingle  (o, value); }
		public virtual void SetSqlBoolean (object o, SqlBoolean  value) { _memberAccessor.SetSqlBoolean (o, value); }
		public virtual void SetSqlDouble  (object o, SqlDouble   value) { _memberAccessor.SetSqlDouble  (o, value); }
		public virtual void SetSqlDateTime(object o, SqlDateTime value) { _memberAccessor.SetSqlDateTime(o, value); }
		public virtual void SetSqlDecimal (object o, SqlDecimal  value) { _memberAccessor.SetSqlDecimal (o, value); }
		public virtual void SetSqlMoney   (object o, SqlMoney    value) { _memberAccessor.SetSqlMoney   (o, value); }
		public virtual void SetSqlGuid    (object o, SqlGuid     value) { _memberAccessor.SetSqlGuid    (o, value); }
		public virtual void SetSqlString  (object o, SqlString   value) { _memberAccessor.SetSqlString  (o, value); }

		#endregion

		#region Intermal Mappers

		#region Complex Mapper

		internal sealed class ComplexMapper : MemberMapper
		{
			public ComplexMapper(MemberMapper memberMapper)
			{
				_mapper = memberMapper;
			}

			MemberMapper _mapper;

			public override object GetValue(object o)
			{
				object obj = _memberAccessor.GetValue(o);
				return obj == null? null: _mapper.GetValue(obj);
			}

			public override void SetValue(object o, object value)
			{
				object obj = _memberAccessor.GetValue(o);

				if (obj != null)
					_mapper.SetValue(obj, value);
			}
		}

		#endregion

		#region Primitive Mappers

		private static MemberMapper GetPrimitiveMemberMapper(MapMemberInfo mi)
		{
			if (mi.MapValues != null)
				return null;

			bool n = mi.Nullable;

			Type type = mi.MemberAccessor.UnderlyingType;
 
			if (type == typeof(SByte))   return n? new SByteMapper.  Nullable(): new SByteMapper();
			if (type == typeof(Int16))   return n? new Int16Mapper.  Nullable(): new Int16Mapper();
			if (type == typeof(Int32))   return n? new Int32Mapper.  Nullable(): new Int32Mapper();
			if (type == typeof(Int64))   return n? new Int64Mapper.  Nullable(): new Int64Mapper();
			if (type == typeof(Byte))    return n? new ByteMapper.   Nullable(): new ByteMapper();
			if (type == typeof(UInt16))  return n? new UInt16Mapper. Nullable(): new UInt16Mapper();
			if (type == typeof(UInt32))  return n? new UInt32Mapper. Nullable(): new UInt32Mapper();
			if (type == typeof(UInt64))  return n? new UInt64Mapper. Nullable(): new UInt64Mapper();
			if (type == typeof(Single))  return n? new SingleMapper. Nullable(): new SingleMapper();
			if (type == typeof(Double))  return n? new DoubleMapper. Nullable(): new DoubleMapper();
			if (type == typeof(Char))    return n? new CharMapper.   Nullable(): new CharMapper();
			if (type == typeof(Boolean)) return n? new BooleanMapper.Nullable(): new BooleanMapper();

			throw new InvalidOperationException();
		}

		class SByteMapper : MemberMapper
		{
			protected SByte _nullValue;

			public override bool IsNull (object o) { return false; }
			public override void SetNull(object o) { _memberAccessor.SetSByte(o, _nullValue); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetSByte(
					o,
					value is SByte? (SByte)value:
					value == null?  _nullValue:
					                _mappingSchema.ConvertToSByte(value));
			}

			public override void Init(MapMemberInfo mapMemberInfo)
			{
				if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

				_nullValue = Convert.ToSByte(mapMemberInfo.NullValue);
				base.Init(mapMemberInfo);
			}

			public class Nullable : SByteMapper
			{
				public override bool IsNull(object o) { return GetSByte(o) == _nullValue; }

				public override object GetValue(object o)
				{
					SByte value = _memberAccessor.GetSByte(o);
					return value == _nullValue? null: (object)value;
				}
			}
		}

		class Int16Mapper : MemberMapper
		{
			protected Int16 _nullValue;

			public override bool IsNull (object o) { return false; }
			public override void SetNull(object o) { _memberAccessor.SetInt16(o, _nullValue); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetInt16(
					o,
					value is Int16? (Int16)value:
					value == null?  _nullValue:
					                _mappingSchema.ConvertToInt16(value));
			}

			public override void Init(MapMemberInfo mapMemberInfo)
			{
				if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

				_nullValue = Convert.ToInt16(mapMemberInfo.NullValue);
				base.Init(mapMemberInfo);
			}

			public class Nullable : Int16Mapper
			{
				public override bool IsNull(object o) { return GetInt16(o) == _nullValue; }

				public override object GetValue(object o)
				{
					Int16 value = _memberAccessor.GetInt16(o);
					return value == _nullValue? null: (object)value;
				}
			}
		}

		class Int32Mapper : MemberMapper
		{
			protected Int32 _nullValue;

			public override bool IsNull(object o) { return false; }
			public override void SetNull(object o) { _memberAccessor.SetInt32(o, _nullValue); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetInt32(
					o,
					value is Int32? (Int32)value:
					value == null?  _nullValue:
					                _mappingSchema.ConvertToInt32(value));
			}

			public override void Init(MapMemberInfo mapMemberInfo)
			{
				if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

				_nullValue = Convert.ToInt32(mapMemberInfo.NullValue);
				base.Init(mapMemberInfo);
			}

			public class Nullable : Int32Mapper
			{
				public override bool IsNull(object o) { return GetInt32(o) == _nullValue; }

				public override object GetValue(object o)
				{
					int value = _memberAccessor.GetInt32(o);
					return value == _nullValue? null: (object)value;
				}
			}
		}

		class Int64Mapper : MemberMapper
		{
			protected Int64 _nullValue;

			public override bool IsNull (object o) { return false; }
			public override void SetNull(object o) { _memberAccessor.SetInt64(o, _nullValue); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetInt64(
					o,
					value is Int64? (Int64)value:
					value == null?  _nullValue:
					                _mappingSchema.ConvertToInt64(value));
			}

			public override void Init(MapMemberInfo mapMemberInfo)
			{
				if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

				_nullValue = Convert.ToInt64(mapMemberInfo.NullValue);
				base.Init(mapMemberInfo);
			}

			public class Nullable : Int64Mapper
			{
				public override bool IsNull(object o) { return GetInt64(o) == _nullValue; }

				public override object GetValue(object o)
				{
					Int64 value = _memberAccessor.GetInt64(o);
					return value == _nullValue? null: (object)value;
				}
			}
		}

		class ByteMapper : MemberMapper
		{
			protected Byte _nullValue;

			public override bool IsNull (object o) { return false; }
			public override void SetNull(object o) { _memberAccessor.SetByte(o, _nullValue); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetByte(
					o,
					value is Byte? (Byte)value:
					value == null? _nullValue:
					               _mappingSchema.ConvertToByte(value));
			}

			public override void Init(MapMemberInfo mapMemberInfo)
			{
				if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

				_nullValue = Convert.ToByte(mapMemberInfo.NullValue);
				base.Init(mapMemberInfo);
			}

			public class Nullable : ByteMapper
			{
				public override bool IsNull(object o) { return GetByte(o) == _nullValue; }

				public override object GetValue(object o)
				{
					Byte value = _memberAccessor.GetByte(o);
					return value == _nullValue? null: (object)value;
				}
			}
		}

		class UInt16Mapper : MemberMapper
		{
			protected UInt16 _nullValue;

			public override bool IsNull (object o) { return false; }
			public override void SetNull(object o) { _memberAccessor.SetUInt16(o, _nullValue); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetUInt16(
					o,
					value is UInt16? (UInt16)value:
					value == null?   _nullValue:
					                 _mappingSchema.ConvertToUInt16(value));
			}

			public override void Init(MapMemberInfo mapMemberInfo)
			{
				if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

				_nullValue = Convert.ToUInt16(mapMemberInfo.NullValue);
				base.Init(mapMemberInfo);
			}

			public class Nullable : UInt16Mapper
			{
				public override bool IsNull(object o) { return GetUInt16(o) == _nullValue; }

				public override object GetValue(object o)
				{
					UInt16 value = _memberAccessor.GetUInt16(o);
					return value == _nullValue? null: (object)value;
				}
			}
		}

		class UInt32Mapper : MemberMapper
		{
			protected UInt32 _nullValue;

			public override bool IsNull (object o) { return false; }
			public override void SetNull(object o) { _memberAccessor.SetUInt32(o, _nullValue); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetUInt32(
					o,
					value is UInt32? (UInt32)value:
					value == null?   _nullValue:
					                 _mappingSchema.ConvertToUInt32(value));
			}

			public override void Init(MapMemberInfo mapMemberInfo)
			{
				if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

				_nullValue = Convert.ToUInt32(mapMemberInfo.NullValue);
				base.Init(mapMemberInfo);
			}

			public class Nullable : UInt32Mapper
			{
				public override bool IsNull(object o) { return GetUInt32(o) == _nullValue; }

				public override object GetValue(object o)
				{
					UInt32 value = _memberAccessor.GetUInt32(o);
					return value == _nullValue? null: (object)value;
				}
			}
		}

		class UInt64Mapper : MemberMapper
		{
			protected UInt64 _nullValue;

			public override bool IsNull (object o) { return false; }
			public override void SetNull(object o) { _memberAccessor.SetUInt64(o, _nullValue); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetUInt64(
					o,
					value is UInt64? (UInt64)value:
					value == null?   _nullValue:
					                 _mappingSchema.ConvertToUInt64(value));
			}

			public override void Init(MapMemberInfo mapMemberInfo)
			{
				if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

				_nullValue = Convert.ToUInt64(mapMemberInfo.NullValue);
				base.Init(mapMemberInfo);
			}

			public class Nullable : UInt64Mapper
			{
				public override bool IsNull(object o) { return GetUInt64(o) == _nullValue; }

				public override object GetValue(object o)
				{
					UInt64 value = _memberAccessor.GetUInt64(o);
					return value == _nullValue? null: (object)value;
				}
			}
		}

		class CharMapper : MemberMapper
		{
			protected Char _nullValue;

			public override bool IsNull (object o) { return false; }
			public override void SetNull(object o) { _memberAccessor.SetChar(o, _nullValue); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetChar(
					o,
					value is Char? (Char)value:
					value == null? _nullValue:
					               _mappingSchema.ConvertToChar(value));
			}

			public override void Init(MapMemberInfo mapMemberInfo)
			{
				if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

				_nullValue = Convert.ToChar(mapMemberInfo.NullValue);
				base.Init(mapMemberInfo);
			}

			public class Nullable : CharMapper
			{
				public override bool IsNull(object o) { return GetChar(o) == _nullValue; }

				public override object GetValue(object o)
				{
					Char value = _memberAccessor.GetChar(o);
					return value == _nullValue? null: (object)value;
				}
			}
		}

		class SingleMapper : MemberMapper
		{
			protected Single _nullValue;

			public override bool IsNull (object o) { return false; }
			public override void SetNull(object o) { _memberAccessor.SetSingle(o, _nullValue); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetSingle(
					o,
					value is Single? (Single)value:
					value == null?   _nullValue:
					                 _mappingSchema.ConvertToSingle(value));
			}

			public override void Init(MapMemberInfo mapMemberInfo)
			{
				if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

				_nullValue = Convert.ToSingle(mapMemberInfo.NullValue);
				base.Init(mapMemberInfo);
			}

			public class Nullable : SingleMapper
			{
				public override bool IsNull(object o) { return GetSingle(o) == _nullValue; }

				public override object GetValue(object o)
				{
					Single value = _memberAccessor.GetSingle(o);
					return value == _nullValue? null: (object)value;
				}
			}
		}

		class DoubleMapper : MemberMapper
		{
			protected Double _nullValue;

			public override bool IsNull (object o) { return false; }
			public override void SetNull(object o) { _memberAccessor.SetDouble(o, _nullValue); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetDouble(
					o,
					value is Double? (Double)value:
					value == null?   _nullValue:
					                 _mappingSchema.ConvertToDouble(value));
			}

			public override void Init(MapMemberInfo mapMemberInfo)
			{
				if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

				_nullValue = Convert.ToDouble(mapMemberInfo.NullValue);
				base.Init(mapMemberInfo);
			}

			public class Nullable : DoubleMapper
			{
				public override bool IsNull(object o) { return GetDouble(o) == _nullValue; }

				public override object GetValue(object o)
				{
					Double value = _memberAccessor.GetDouble(o);
					return value == _nullValue? null: (object)value;
				}
			}
		}

		class BooleanMapper : MemberMapper
		{
			protected Boolean _nullValue;

			public override bool IsNull (object o) { return false; }
			public override void SetNull(object o) { _memberAccessor.SetBoolean(o, _nullValue); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(
					o,
					value is Boolean? (Boolean)value:
					value == null?    _nullValue:
					                  _mappingSchema.ConvertToBoolean(value));
			}

			public override void Init(MapMemberInfo mapMemberInfo)
			{
				if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

				_nullValue = Convert.ToBoolean(mapMemberInfo.NullValue);
				base.Init(mapMemberInfo);
			}

			public class Nullable : BooleanMapper
			{
				public override bool IsNull(object o) { return GetBoolean(o) == _nullValue; }

				public override object GetValue(object o)
				{
					Boolean value = _memberAccessor.GetBoolean(o);
					return value == _nullValue? null: (object)value;
				}
			}
		}

		#endregion

		#region Simple Mappers

		private static MemberMapper GetSimpleMemberMapper(MapMemberInfo mi)
		{
			if (mi.MapValues != null)
				return null;

			bool n = mi.Nullable;

			Type type = mi.Type;

			if (type == typeof(String))
				if (mi.Trimmable) return n? new StringMapper.Trimmable.Nullable(): new StringMapper.Trimmable();
				else              return n? new StringMapper.Nullable(): new StringMapper();

			if (type == typeof(DateTime)) return n? new DateTimeMapper.Nullable(): new DateTimeMapper();
			if (type == typeof(Decimal))  return n? new DecimalMapper.Nullable(): new DecimalMapper();
			if (type == typeof(Guid))     return n? new GuidMapper.Nullable(): new GuidMapper();
			if (type == typeof(Stream))   return n? new StreamMapper.Nullable(): new StreamMapper();

			return null;
		}

		class StringMapper : MemberMapper
		{
			protected string _nullValue;

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(
					o,
					value is string? value:
					value == null?   _nullValue:
					                 _mappingSchema.ConvertToString(value));
			}

			public override void Init(MapMemberInfo mapMemberInfo)
			{
				if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

				if (mapMemberInfo.NullValue != null)
					_nullValue = Convert.ToString(mapMemberInfo.NullValue);

				base.Init(mapMemberInfo);
			}

			public class Nullable : StringMapper
			{
				public override object GetValue(object o)
				{
					object value = _memberAccessor.GetValue(o);
					return (string)value == _nullValue? null: value;
				}
			}

			public class Trimmable : StringMapper
			{
				public override void SetValue(object o, object value)
				{
					_memberAccessor.SetValue(
						o, value == null? _nullValue: _mappingSchema.ConvertToString(value).TrimEnd(_trim));
				}

				public new class Nullable : Trimmable
				{
					public override object GetValue(object o)
					{
						object value = _memberAccessor.GetValue(o);
						return (string)value == _nullValue? null: value;
					}
				}
			}
		}

		class DateTimeMapper : MemberMapper
		{
			protected DateTime _nullValue;

			public override bool IsNull (object o) { return false; }
			public override void SetNull(object o) { _memberAccessor.SetDateTime(o, _nullValue); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetDateTime(
					o,
					value is DateTime? (DateTime)value:
					value == null?     _nullValue:
					                   _mappingSchema.ConvertToDateTime(value));
			}

			public override void Init(MapMemberInfo mapMemberInfo)
			{
				if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

				_nullValue = Convert.ToDateTime(mapMemberInfo.NullValue);
				base.Init(mapMemberInfo);
			}

			public class Nullable : DateTimeMapper
			{
				public override bool IsNull(object o) { return GetDateTime(o) == _nullValue; }

				public override object GetValue(object o)
				{
					DateTime value = _memberAccessor.GetDateTime(o);
					return value == _nullValue? null: (object)value;
				}
			}
		}

		class DecimalMapper : MemberMapper
		{
			protected Decimal _nullValue;

			public override bool IsNull (object o) { return false; }
			public override void SetNull(object o) { _memberAccessor.SetDecimal(o, _nullValue); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetDecimal(
					o,
					value is Decimal? (Decimal)value:
					value == null?    _nullValue:
					                  _mappingSchema.ConvertToDecimal(value));
			}

			public override void Init(MapMemberInfo mapMemberInfo)
			{
				if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

				_nullValue = Convert.ToDecimal(mapMemberInfo.NullValue);
				base.Init(mapMemberInfo);
			}

			public class Nullable : DecimalMapper
			{
				public override bool IsNull(object o) { return GetDecimal(o) == _nullValue; }

				public override object GetValue(object o)
				{
					Decimal value = _memberAccessor.GetDecimal(o);
					return value == _nullValue? null: (object)value;
				}
			}
		}

		class GuidMapper : MemberMapper
		{
			protected Guid _nullValue;

			public override bool IsNull (object o) { return false; }
			public override void SetNull(object o) { _memberAccessor.SetGuid(o, _nullValue); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetGuid(
					o,
					value is Guid? (Guid)value:
					value == null? _nullValue:
					               _mappingSchema.ConvertToGuid(value));
			}

			public override void Init(MapMemberInfo mapMemberInfo)
			{
				if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

				if (mapMemberInfo.NullValue != null)
					_nullValue = mapMemberInfo.NullValue is Guid?
						(Guid)mapMemberInfo.NullValue: new Guid(mapMemberInfo.NullValue.ToString());

				base.Init(mapMemberInfo);
			}

			public class Nullable : GuidMapper
			{
				public override bool IsNull(object o) { return GetGuid(o) == _nullValue; }

				public override object GetValue(object o)
				{
					Guid value = _memberAccessor.GetGuid(o);
					return value == _nullValue? null: (object)value;
				}
			}
		}

		class StreamMapper : MemberMapper
		{
			protected Stream _nullValue;

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(
					o,
					value is Stream? value:
					value == null? _nullValue: _mappingSchema.ConvertToStream(value));
			}

			public override void Init(MapMemberInfo mapMemberInfo)
			{
				if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

				if (mapMemberInfo.NullValue != null)
					_nullValue = mapMemberInfo.MappingSchema.ConvertToStream(mapMemberInfo.NullValue);

				base.Init(mapMemberInfo);
			}

			public class Nullable : StreamMapper
			{
				public override object GetValue(object o)
				{
					object value = _memberAccessor.GetValue(o);
					return (Stream)value == _nullValue? null: value;
				}
			}
		}

		#endregion

#if FW2

		#region Nullable Mappers

		private static MemberMapper GetNullableMemberMapper(MapMemberInfo mi)
		{
			Type type = mi.Type;

			if (type.IsGenericType == false || mi.MapValues != null)
				return null;

			Type underlyingType = Nullable.GetUnderlyingType(type);

			if (underlyingType == null)
				return null;

			if (underlyingType.IsEnum)
			{
				underlyingType = Enum.GetUnderlyingType(underlyingType);

				if (underlyingType == typeof(SByte))    return new NullableSByteMapper. Enum();
				if (underlyingType == typeof(Int16))    return new NullableInt16Mapper. Enum();
				if (underlyingType == typeof(Int32))    return new NullableInt32Mapper. Enum();
				if (underlyingType == typeof(Int64))    return new NullableInt64Mapper. Enum();
				if (underlyingType == typeof(Byte))     return new NullableByteMapper.  Enum();
				if (underlyingType == typeof(UInt16))   return new NullableUInt16Mapper.Enum();
				if (underlyingType == typeof(UInt32))   return new NullableUInt32Mapper.Enum();
				if (underlyingType == typeof(UInt64))   return new NullableUInt64Mapper.Enum();
			}
			else
			{
				if (underlyingType == typeof(SByte))    return new NullableSByteMapper();
				if (underlyingType == typeof(Int16))    return new NullableInt16Mapper();
				if (underlyingType == typeof(Int32))    return new NullableInt32Mapper();
				if (underlyingType == typeof(Int64))    return new NullableInt64Mapper();
				if (underlyingType == typeof(Byte))     return new NullableByteMapper();
				if (underlyingType == typeof(UInt16))   return new NullableUInt16Mapper();
				if (underlyingType == typeof(UInt32))   return new NullableUInt32Mapper();
				if (underlyingType == typeof(UInt64))   return new NullableUInt64Mapper();
				if (underlyingType == typeof(Char))     return new NullableCharMapper();
				if (underlyingType == typeof(Single))   return new NullableSingleMapper();
				if (underlyingType == typeof(Boolean))  return new NullableBooleanMapper();
				if (underlyingType == typeof(Double))   return new NullableDoubleMapper();
				if (underlyingType == typeof(DateTime)) return new NullableDateTimeMapper();
				if (underlyingType == typeof(Decimal))  return new NullableDecimalMapper();
				if (underlyingType == typeof(Guid))     return new NullableGuidMapper();
			}

			return null;
		}

		abstract class NullableEnumMapper : MemberMapper
		{
			protected Type _memberType;
			protected Type _underlyingType;

			public override void Init(MapMemberInfo mapMemberInfo)
			{
				if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

				_memberType     = Nullable.GetUnderlyingType(mapMemberInfo.Type);
				_underlyingType = mapMemberInfo.MemberAccessor.UnderlyingType;

				base.Init(mapMemberInfo);
			}
		}

		class NullableInt16Mapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetNullableInt16(o) == null; }
			public override void SetNull(object o) { _memberAccessor.SetNullableInt16(o, null); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetNullableInt16(
					o, value == null || value is Int16? (Int16?)value: _mappingSchema.ConvertToNullableInt16(value));
			}

			public class Enum : NullableEnumMapper
			{
				public override void SetValue(object o, object value)
				{
					if (value != null)
					{
						Type valueType = value.GetType();
						
						if (valueType != _memberType)
						{
							if (valueType != _underlyingType)
								value = _mappingSchema.ConvertToNullableInt16(value);

							value = System.Enum.ToObject(_memberType, (Int16)value);
						}
					}

					_memberAccessor.SetValue(o, value);
				}
			}
		}

		class NullableInt32Mapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetNullableInt32(o) == null; }
			public override void SetNull(object o) { _memberAccessor.SetNullableInt32(o, null); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetNullableInt32(
					o, value == null || value is Int32? (Int32?)value: _mappingSchema.ConvertToNullableInt32(value));
			}

			public class Enum : NullableEnumMapper
			{
				public override void SetValue(object o, object value)
				{
					if (value != null)
					{
						Type valueType = value.GetType();
						
						if (valueType != _memberType)
						{
							if (valueType != _underlyingType)
								value = _mappingSchema.ConvertToNullableInt32(value);

							value = System.Enum.ToObject(_memberType, (Int32)value);
						}
					}

					_memberAccessor.SetValue(o, value);
				}
			}
		}

		class NullableSByteMapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetNullableSByte(o) == null; }
			public override void SetNull(object o) { _memberAccessor.SetNullableSByte(o, null); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetNullableSByte(
					o, value == null || value is SByte? (SByte?)value: _mappingSchema.ConvertToNullableSByte(value));
			}

			public class Enum : NullableEnumMapper
			{
				public override void SetValue(object o, object value)
				{
					if (value != null)
					{
						Type valueType = value.GetType();
						
						if (valueType != _memberType)
						{
							if (valueType != _underlyingType)
								value = _mappingSchema.ConvertToNullableSByte(value);

							value = System.Enum.ToObject(_memberType, (SByte)value);
						}
					}

					_memberAccessor.SetValue(o, value);
				}
			}
		}

		class NullableInt64Mapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetNullableInt64(o) == null; }
			public override void SetNull(object o) { _memberAccessor.SetNullableInt64(o, null); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetNullableInt64(
					o, value == null || value is Int64? (Int64?)value: _mappingSchema.ConvertToNullableInt64(value));
			}

			public class Enum : NullableEnumMapper
			{
				public override void SetValue(object o, object value)
				{
					if (value != null)
					{
						Type valueType = value.GetType();
						
						if (valueType != _memberType)
						{
							if (valueType != _underlyingType)
								value = _mappingSchema.ConvertToNullableInt64(value);

							value = System.Enum.ToObject(_memberType, (Int64)value);
						}
					}

					_memberAccessor.SetValue(o, value);
				}
			}
		}

		class NullableByteMapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetNullableByte(o) == null; }
			public override void SetNull(object o) { _memberAccessor.SetNullableByte(o, null); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetNullableByte(
					o, value == null || value is Byte? (Byte?)value: _mappingSchema.ConvertToNullableByte(value));
			}

			public class Enum : NullableEnumMapper
			{
				public override void SetValue(object o, object value)
				{
					if (value != null)
					{
						Type valueType = value.GetType();
						
						if (valueType != _memberType)
						{
							if (valueType != _underlyingType)
								value = _mappingSchema.ConvertToNullableByte(value);

							value = System.Enum.ToObject(_memberType, (Byte)value);
						}
					}

					_memberAccessor.SetValue(o, value);
				}
			}
		}

		class NullableUInt16Mapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetNullableUInt16(o) == null; }
			public override void SetNull(object o) { _memberAccessor.SetNullableUInt16(o, null); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetNullableUInt16(
					o, value == null || value is UInt16? (UInt16?)value: _mappingSchema.ConvertToNullableUInt16(value));
			}

			public class Enum : NullableEnumMapper
			{
				public override void SetValue(object o, object value)
				{
					if (value != null)
					{
						Type valueType = value.GetType();
						
						if (valueType != _memberType)
						{
							if (valueType != _underlyingType)
								value = _mappingSchema.ConvertToNullableUInt16(value);

							value = System.Enum.ToObject(_memberType, (UInt16)value);
						}
					}

					_memberAccessor.SetValue(o, value);
				}
			}
		}

		class NullableUInt32Mapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetNullableUInt32(o) == null; }
			public override void SetNull(object o) { _memberAccessor.SetNullableUInt32(o, null); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetNullableUInt32(
					o, value == null || value is UInt32? (UInt32?)value: _mappingSchema.ConvertToNullableUInt32(value));
			}

			public class Enum : NullableEnumMapper
			{
				public override void SetValue(object o, object value)
				{
					if (value != null)
					{
						Type valueType = value.GetType();
						
						if (valueType != _memberType)
						{
							if (valueType != _underlyingType)
								value = _mappingSchema.ConvertToNullableUInt32(value);

							value = System.Enum.ToObject(_memberType, (UInt32)value);
						}
					}

					_memberAccessor.SetValue(o, value);
				}
			}
		}

		class NullableUInt64Mapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetNullableUInt64(o) == null; }
			public override void SetNull(object o) { _memberAccessor.SetNullableUInt64(o, null); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetNullableUInt64(
					o, value == null || value is UInt64? (UInt64?)value: _mappingSchema.ConvertToNullableUInt64(value));
			}

			public class Enum : NullableEnumMapper
			{
				public override void SetValue(object o, object value)
				{
					if (value != null)
					{
						Type valueType = value.GetType();
						
						if (valueType != _memberType)
						{
							if (valueType != _underlyingType)
								value = _mappingSchema.ConvertToNullableUInt64(value);

							value = System.Enum.ToObject(_memberType, (UInt64)value);
						}
					}

					_memberAccessor.SetValue(o, value);
				}
			}
		}

		class NullableCharMapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetNullableChar(o) == null; }
			public override void SetNull(object o) { _memberAccessor.SetNullableChar(o, null); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetNullableChar(
					o, value == null || value is Char? (Char?)value: _mappingSchema.ConvertToNullableChar(value));
			}
		}

		class NullableDoubleMapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetNullableDouble(o) == null; }
			public override void SetNull(object o) { _memberAccessor.SetNullableDouble(o, null); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetNullableDouble(
					o, value == null || value is Double? (Double?)value: _mappingSchema.ConvertToNullableDouble(value));
			}
		}

		class NullableSingleMapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetNullableSingle(o) == null; }
			public override void SetNull(object o) { _memberAccessor.SetNullableSingle(o, null); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetNullableSingle(
					o, value == null || value is Single? (Single?)value: _mappingSchema.ConvertToNullableSingle(value));
			}
		}

		class NullableBooleanMapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetNullableBoolean(o) == null; }
			public override void SetNull(object o) { _memberAccessor.SetNullableBoolean(o, null); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetNullableBoolean(
					o, value == null || value is Boolean? (Boolean?)value: _mappingSchema.ConvertToNullableBoolean(value));
			}
		}

		class NullableDateTimeMapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetNullableDateTime(o) == null; }
			public override void SetNull(object o) { _memberAccessor.SetNullableDateTime(o, null); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetNullableDateTime(
					o, value == null || value is DateTime? (DateTime?)value: _mappingSchema.ConvertToNullableDateTime(value));
			}
		}

		class NullableDecimalMapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetNullableDecimal(o) == null; }
			public override void SetNull(object o) { _memberAccessor.SetNullableDecimal(o, null); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetNullableDecimal(
					o, value == null || value is Decimal? (Decimal?)value: _mappingSchema.ConvertToNullableDecimal(value));
			}
		}

		class NullableGuidMapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetNullableGuid(o) == null; }
			public override void SetNull(object o) { _memberAccessor.SetNullableGuid(o, null); }

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetNullableGuid(
					o, value == null || value is Guid? (Guid?)value: _mappingSchema.ConvertToNullableGuid(value));
			}
		}

		#endregion

#endif

		#region SqlTypes

		private static MemberMapper GetSqlTypeMemberMapper(MapMemberInfo mi)
		{
			Type type = mi.Type;

			if (TypeHelper.IsSameOrParent(typeof(INullable), type) == false)
				return null;

			bool d = mi.MapValues != null;

			if (type == typeof(SqlByte))     return d? new SqlByteMapper.    Default(): new SqlByteMapper();
			if (type == typeof(SqlInt16))    return d? new SqlInt16Mapper.   Default(): new SqlInt16Mapper();
			if (type == typeof(SqlInt32))    return d? new SqlInt32Mapper.   Default(): new SqlInt32Mapper();
			if (type == typeof(SqlInt64))    return d? new SqlInt64Mapper.   Default(): new SqlInt64Mapper();
			if (type == typeof(SqlSingle))   return d? new SqlSingleMapper.  Default(): new SqlSingleMapper();
			if (type == typeof(SqlBoolean))  return d? new SqlBooleanMapper. Default(): new SqlBooleanMapper();
			if (type == typeof(SqlDouble))   return d? new SqlDoubleMapper.  Default(): new SqlDoubleMapper();
			if (type == typeof(SqlDateTime)) return d? new SqlDateTimeMapper.Default(): new SqlDateTimeMapper();
			if (type == typeof(SqlDecimal))  return d? new SqlDecimalMapper. Default(): new SqlDecimalMapper();
			if (type == typeof(SqlMoney))    return d? new SqlMoneyMapper.   Default(): new SqlMoneyMapper();
			if (type == typeof(SqlGuid))     return d? new SqlGuidMapper.    Default(): new SqlGuidMapper();
			if (type == typeof(SqlString))   return d? new SqlStringMapper.  Default(): new SqlStringMapper();

			return null;
		}

		class SqlByteMapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetSqlByte(o).IsNull; }
			public override void SetNull(object o) { _memberAccessor.SetSqlByte(o, SqlByte.Null); }

			public override object GetValue(object o)
			{
				SqlByte value = _memberAccessor.GetSqlByte(o);
				return value.IsNull? null: (object)value.Value;
			}

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetSqlByte(
					o, value is SqlByte? (SqlByte)value: _mappingSchema.ConvertToSqlByte(value));
			}

			public class Default : SqlByteMapper
			{
				public override bool SupportsValue { get { return false; } }

				public override object GetValue(object o)
				{
					return MapTo(base.GetValue(o));
				}

				public override void SetValue(object o, object value)
				{
					base.SetValue(o, MapFrom(value));
				}
			}
		}

		class SqlInt16Mapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetSqlInt16(o).IsNull; }
			public override void SetNull(object o) { _memberAccessor.SetSqlInt16(o, SqlInt16.Null); }

			public override object GetValue(object o)
			{
				SqlInt16 value = _memberAccessor.GetSqlInt16(o);
				return value.IsNull? null: (object)value.Value;
			}

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetSqlInt16(
					o, value is SqlInt16? (SqlInt16)value: _mappingSchema.ConvertToSqlInt16(value));
			}

			public class Default : SqlInt16Mapper
			{
				public override bool SupportsValue { get { return false; } }

				public override object GetValue(object o)
				{
					return MapTo(base.GetValue(o));
				}

				public override void SetValue(object o, object value)
				{
					base.SetValue(o, MapFrom(value));
				}
			}
		}

		class SqlInt32Mapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetSqlInt32(o).IsNull; }
			public override void SetNull(object o) { _memberAccessor.SetSqlInt32(o, SqlInt32.Null); }

			public override object GetValue(object o)
			{
				SqlInt32 value = _memberAccessor.GetSqlInt32(o);
				return value.IsNull? null: (object)value.Value;
			}

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetSqlInt32(
					o, value is SqlInt32? (SqlInt32)value: _mappingSchema.ConvertToSqlInt32(value));
			}

			public class Default : SqlInt32Mapper
			{
				public override bool SupportsValue { get { return false; } }

				public override object GetValue(object o)
				{
					return MapTo(base.GetValue(o));
				}

				public override void SetValue(object o, object value)
				{
					base.SetValue(o, MapFrom(value));
				}
			}
		}

		class SqlInt64Mapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetSqlInt64(o).IsNull; }
			public override void SetNull(object o) { _memberAccessor.SetSqlInt64(o, SqlInt64.Null); }

			public override object GetValue(object o)
			{
				SqlInt64 value = _memberAccessor.GetSqlInt64(o);
				return value.IsNull? null: (object)value.Value;
			}

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetSqlInt64(
					o, value is SqlInt64? (SqlInt64)value: _mappingSchema.ConvertToSqlInt64(value));
			}

			public class Default : SqlInt64Mapper
			{
				public override bool SupportsValue { get { return false; } }

				public override object GetValue(object o)
				{
					return MapTo(base.GetValue(o));
				}

				public override void SetValue(object o, object value)
				{
					base.SetValue(o, MapFrom(value));
				}
			}
		}

		class SqlSingleMapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetSqlSingle(o).IsNull; }
			public override void SetNull(object o) { _memberAccessor.SetSqlSingle(o, SqlSingle.Null); }

			public override object GetValue(object o)
			{
				SqlSingle value = _memberAccessor.GetSqlSingle(o);
				return value.IsNull? null: (object)value.Value;
			}

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetSqlSingle(
					o, value is SqlSingle? (SqlSingle)value: _mappingSchema.ConvertToSqlSingle(value));
			}

			public class Default : SqlSingleMapper
			{
				public override bool SupportsValue { get { return false; } }

				public override object GetValue(object o)
				{
					return MapTo(base.GetValue(o));
				}

				public override void SetValue(object o, object value)
				{
					base.SetValue(o, MapFrom(value));
				}
			}
		}

		class SqlBooleanMapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetSqlBoolean(o).IsNull; }
			public override void SetNull(object o) { _memberAccessor.SetSqlBoolean(o, SqlBoolean.Null); }

			public override object GetValue(object o)
			{
				SqlBoolean value = _memberAccessor.GetSqlBoolean(o);
				return value.IsNull? null: (object)value.Value;
			}

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetSqlBoolean(
					o, value is SqlBoolean? (SqlBoolean)value: _mappingSchema.ConvertToSqlBoolean(value));
			}

			public class Default : SqlBooleanMapper
			{
				public override bool SupportsValue { get { return false; } }

				public override object GetValue(object o)
				{
					return MapTo(base.GetValue(o));
				}

				public override void SetValue(object o, object value)
				{
					base.SetValue(o, MapFrom(value));
				}
			}
		}

		class SqlDoubleMapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetSqlDouble(o).IsNull; }
			public override void SetNull(object o) { _memberAccessor.SetSqlDouble(o, SqlDouble.Null); }

			public override object GetValue(object o)
			{
				SqlDouble value = _memberAccessor.GetSqlDouble(o);
				return value.IsNull? null: (object)value.Value;
			}

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetSqlDouble(
					o, value is SqlDouble? (SqlDouble)value: _mappingSchema.ConvertToSqlDouble(value));
			}

			public class Default : SqlDoubleMapper
			{
				public override bool SupportsValue { get { return false; } }

				public override object GetValue(object o)
				{
					return MapTo(base.GetValue(o));
				}

				public override void SetValue(object o, object value)
				{
					base.SetValue(o, MapFrom(value));
				}
			}
		}

		class SqlDateTimeMapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetSqlDateTime(o).IsNull; }
			public override void SetNull(object o) { _memberAccessor.SetSqlDateTime(o, SqlDateTime.Null); }

			public override object GetValue(object o)
			{
				SqlDateTime value = _memberAccessor.GetSqlDateTime(o);
				return value.IsNull? null: (object)value.Value;
			}

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetSqlDateTime(
					o, value is SqlDateTime? (SqlDateTime)value: _mappingSchema.ConvertToSqlDateTime(value));
			}

			public class Default : SqlDateTimeMapper
			{
				public override bool SupportsValue { get { return false; } }

				public override object GetValue(object o)
				{
					return MapTo(base.GetValue(o));
				}

				public override void SetValue(object o, object value)
				{
					base.SetValue(o, MapFrom(value));
				}
			}
		}

		class SqlDecimalMapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetSqlDecimal(o).IsNull; }
			public override void SetNull(object o) { _memberAccessor.SetSqlDecimal(o, SqlDecimal.Null); }

			public override object GetValue(object o)
			{
				SqlDecimal value = _memberAccessor.GetSqlDecimal(o);
				return value.IsNull? null: (object)value.Value;
			}

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetSqlDecimal(
					o, value is SqlDecimal? (SqlDecimal)value: _mappingSchema.ConvertToSqlDecimal(value));
			}

			public class Default : SqlDecimalMapper
			{
				public override bool SupportsValue { get { return false; } }

				public override object GetValue(object o)
				{
					return MapTo(base.GetValue(o));
				}

				public override void SetValue(object o, object value)
				{
					base.SetValue(o, MapFrom(value));
				}
			}
		}

		class SqlMoneyMapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetSqlMoney(o).IsNull; }
			public override void SetNull(object o) { _memberAccessor.SetSqlMoney(o, SqlMoney.Null); }

			public override object GetValue(object o)
			{
				SqlMoney value = _memberAccessor.GetSqlMoney(o);
				return value.IsNull? null: (object)value.Value;
			}

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetSqlMoney(
					o, value is SqlMoney? (SqlMoney)value: _mappingSchema.ConvertToSqlMoney(value));
			}

			public class Default : SqlMoneyMapper
			{
				public override bool SupportsValue { get { return false; } }

				public override object GetValue(object o)
				{
					return MapTo(base.GetValue(o));
				}

				public override void SetValue(object o, object value)
				{
					base.SetValue(o, MapFrom(value));
				}
			}
		}

		class SqlGuidMapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetSqlGuid(o).IsNull; }
			public override void SetNull(object o) { _memberAccessor.SetSqlGuid(o, SqlGuid.Null); }

			public override object GetValue(object o)
			{
				SqlGuid value = _memberAccessor.GetSqlGuid(o);
				return value.IsNull? null: (object)value.Value;
			}

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetSqlGuid(
					o, value is SqlGuid? (SqlGuid)value: _mappingSchema.ConvertToSqlGuid(value));
			}

			public class Default : SqlGuidMapper
			{
				public override bool SupportsValue { get { return false; } }

				public override object GetValue(object o)
				{
					return MapTo(base.GetValue(o));
				}

				public override void SetValue(object o, object value)
				{
					base.SetValue(o, MapFrom(value));
				}
			}
		}

		class SqlStringMapper : MemberMapper
		{
			public override bool IsNull (object o) { return GetSqlString(o).IsNull; }
			public override void SetNull(object o) { _memberAccessor.SetSqlString(o, SqlString.Null); }

			public override object GetValue(object o)
			{
				SqlString value = _memberAccessor.GetSqlString(o);
				return value.IsNull? null: value.Value;
			}

			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetSqlString(
					o, value is SqlString? (SqlString)value: _mappingSchema.ConvertToSqlString(value));
			}

			public class Default : SqlStringMapper
			{
				public override bool SupportsValue { get { return false; } }

				public override object GetValue(object o)
				{
					return MapTo(base.GetValue(o));
				}

				public override void SetValue(object o, object value)
				{
					base.SetValue(o, MapFrom(value));
				}
			}
		}

		#endregion

		#endregion

		#region MapFrom, MapTo

		protected object MapFrom(object value)
		{
			return MapFrom(value, _mapMemberInfo);
		}

		static char[] _trim = { ' ' };

		protected object MapFrom(object value, MapMemberInfo mapInfo)
		{
			if (mapInfo == null) throw new ArgumentNullException("mapInfo");

			if (value == null)
				return mapInfo.NullValue;

			if (mapInfo.Trimmable && value is string)
				value = value.ToString().TrimEnd(_trim);

			if (mapInfo.MapValues != null)
			{
				IComparable comp = (IComparable)value;

				foreach (MapValue mv in mapInfo.MapValues)
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
				if (mapInfo.DefaultValue != null)
					return mapInfo.DefaultValue;
			}

			Type valueType  = value.GetType();
			Type memberType = mapInfo.Type;

			if (valueType != memberType)
			{
#if FW2
				if (memberType.IsGenericType)
				{
					Type underlyingType = Nullable.GetUnderlyingType(memberType);

					if (valueType == underlyingType)
						return value;

					memberType = underlyingType;
				}
#endif
				if (memberType.IsEnum)
				{
					Type underlyingType = mapInfo.MemberAccessor.UnderlyingType;

					if (valueType != underlyingType)
						value = _mappingSchema.ConvertChangeType(value, underlyingType);

					//value = Enum.Parse(type, Enum.GetName(type, value));
					value = Enum.ToObject(memberType, value);
				}
				else
				{
					value = _mappingSchema.ConvertChangeType(value, memberType);
				}
			}

			return value;
		}

		protected object MapTo(object value)
		{
			return MapTo(value, _mapMemberInfo);
		}

		protected static object MapTo(object value, MapMemberInfo mapInfo)
		{
			if (mapInfo == null) throw new ArgumentNullException("mapInfo");

			if (value == null)
				return null;

			if (mapInfo.Nullable && mapInfo.NullValue != null)
			{
				IComparable comp = (IComparable)value;

				try
				{
					if (comp.CompareTo(mapInfo.NullValue) == 0)
						return null;
				}
				catch
				{
				}
			}

			if (mapInfo.MapValues != null)
			{
				IComparable comp = (IComparable)value;

				foreach (MapValue mv in mapInfo.MapValues)
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

		#endregion
	}
}
