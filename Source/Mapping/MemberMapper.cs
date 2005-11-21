using System;
using System.Data.SqlTypes;

using BLToolkit.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace BLToolkit.Mapping
{
	public class MemberMapper
	{
		public MemberMapper()
		{
		}

		private MemberAccessor _memberAccessor;
		public  MemberAccessor  MemberAccessor
		{
			get { return _memberAccessor; }
		}

		private string _name;
		public  string  Name
		{
			get { return _name; }
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

		public virtual void Init(MapMemberInfo mapMemberInfo)
		{
			if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

			_name           = mapMemberInfo.Name;
			_memberAccessor = mapMemberInfo.MemberAccessor;
		}

		public virtual object GetValue(object o)
		{
			return _memberAccessor.GetValue(o);
		}

		public virtual void SetValue(object o, object value)
		{
			_memberAccessor.SetValue(o, value);
		}

		internal static MemberMapper CreateMemberMapper(MapMemberInfo mi)
		{
			Type         type = mi.MemberAccessor.Type;
			MemberMapper mm   = null;

			if (type.IsPrimitive)
				mm = GetPrimitiveMemberMapper(type);

			if (mm == null)
				mm = GetSimpleMemberMapper(mi);

#if FW2
			if (mm == null && type.IsGenericType)
				mm = GetNullableMemberMapper(type);
#endif

			if (mm == null)
				mm = new MemberMapper();

			return mm;
		}

		#region Simple Mappers

		private static MemberMapper GetSimpleMemberMapper(MapMemberInfo mi)
		{
			Type type = mi.MemberAccessor.Type;

			if (type == typeof(String))
				if (mi.IsTrimmable) return new TrimmableStringMapper();
				else                return new StringMapper();

			if (type == typeof(DateTime)) return new DateTimeMapper();
			if (type == typeof(Decimal))  return new DecimalMapper();
			if (type == typeof(Guid))     return new GuidMapper();

			return null;
		}

		class StringMapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, value == null? string.Empty: value.ToString());
			}
		}

		class TrimmableStringMapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, value == null? string.Empty: value.ToString().TrimEnd(null));
			}
		}

		class DateTimeMapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, Convert.ToDateTime(value));
			}
		}

		class DecimalMapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, Convert.ToDecimal(value));
			}
		}

		class GuidMapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				if (value is Guid)
					_memberAccessor.SetValue(o, value);
				else if (value == null)
					_memberAccessor.SetValue(o, Guid.Empty);
				else
					_memberAccessor.SetValue(o, new Guid(value.ToString()));
			}
		}

		#endregion

		#region Primitive Mappers

		private static MemberMapper GetPrimitiveMemberMapper(Type type)
		{
			if (type == typeof(Int32))   return new Int32Mapper();
			if (type == typeof(Double))  return new DoubleMapper();
			if (type == typeof(Int16))   return new Int16Mapper();
			if (type == typeof(SByte))   return new SByteMapper();
			if (type == typeof(Int64))   return new Int64Mapper();
			if (type == typeof(Byte))    return new ByteMapper();
			if (type == typeof(UInt16))  return new UInt16Mapper();
			if (type == typeof(UInt32))  return new UInt32Mapper();
			if (type == typeof(UInt64))  return new UInt64Mapper();
			if (type == typeof(UInt64))  return new CharMapper();
			if (type == typeof(Single))  return new SingleMapper();
			if (type == typeof(Boolean)) return new BooleanMapper();

			throw new InvalidOperationException();
		}

		class Int16Mapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, Convert.ToInt16(value));
			}
		}

		class Int32Mapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, Convert.ToInt32(value));
			}
		}

		class SByteMapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, Convert.ToSByte(value));
			}
		}

		class Int64Mapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, Convert.ToInt64(value));
			}
		}

		class ByteMapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, Convert.ToByte(value));
			}
		}

		class UInt16Mapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, Convert.ToUInt16(value));
			}
		}

		class UInt32Mapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, Convert.ToUInt32(value));
			}
		}

		class UInt64Mapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, Convert.ToUInt64(value));
			}
		}

		class CharMapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, Convert.ToChar(value));
			}
		}

		class DoubleMapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, Convert.ToDouble(value));
			}
		}

		class SingleMapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, Convert.ToSingle(value));
			}
		}

		class BooleanMapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, Convert.ToBoolean(value));
			}
		}

		#endregion

#if FW2

		#region Nullable Mappers

		private static MemberMapper GetNullableMemberMapper(Type type)
		{
			if (type == typeof(Int32?))    return new NullableInt32Mapper();
			if (type == typeof(Double?))   return new NullableDoubleMapper();
			if (type == typeof(Int16?))    return new NullableInt16Mapper();
			if (type == typeof(SByte?))    return new NullableSByteMapper();
			if (type == typeof(Int64?))    return new NullableInt64Mapper();
			if (type == typeof(Byte?))     return new NullableByteMapper();
			if (type == typeof(UInt16?))   return new NullableUInt16Mapper();
			if (type == typeof(UInt32?))   return new NullableUInt32Mapper();
			if (type == typeof(UInt64?))   return new NullableUInt64Mapper();
			if (type == typeof(UInt64?))   return new NullableCharMapper();
			if (type == typeof(Single?))   return new NullableSingleMapper();
			if (type == typeof(Boolean?))  return new NullableBooleanMapper();
			if (type == typeof(DateTime?)) return new NullableDateTimeMapper();
			if (type == typeof(Decimal?))  return new NullableDecimalMapper();
			if (type == typeof(Guid?))     return new NullableGuidMapper();

			return null;
		}

		class NullableInt16Mapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, value == null || value is Int16? ? value: Convert.ToInt16(value));
			}
		}

		class NullableInt32Mapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, value == null || value is Int32? ? value: Convert.ToInt32(value));
			}
		}

		class NullableSByteMapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, value == null || value is SByte? ? value: Convert.ToSByte(value));
			}
		}

		class NullableInt64Mapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, value == null || value is Int64? ? value: Convert.ToInt64(value));
			}
		}

		class NullableByteMapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, value == null || value is Byte? ? value: Convert.ToByte(value));
			}
		}

		class NullableUInt16Mapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, value == null || value is UInt16? ? value: Convert.ToUInt16(value));
			}
		}

		class NullableUInt32Mapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, value == null || value is UInt32? ? value: Convert.ToUInt32(value));
			}
		}

		class NullableUInt64Mapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, value == null || value is UInt64? ? value: Convert.ToUInt64(value));
			}
		}

		class NullableCharMapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, value == null || value is Char? ? value: Convert.ToChar(value));
			}
		}

		class NullableDoubleMapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, value == null || value is Double? ? value: Convert.ToDouble(value));
			}
		}

		class NullableSingleMapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, value == null || value is Single? ? value: Convert.ToSingle(value));
			}
		}

		class NullableBooleanMapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, value == null || value is Boolean? ? value: Convert.ToBoolean(value));
			}
		}

		class NullableDateTimeMapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, value == null || value is DateTime? ? value: Convert.ToDateTime(value));
			}
		}

		class NullableDecimalMapper : MemberMapper
		{
			public override void SetValue(object o, object value)
			{
				_memberAccessor.SetValue(o, value == null || value is Decimal? ? value: Convert.ToDecimal(value));
			}
		}

		class NullableGuidMapper : MemberMapper
		{
			[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
			public override void SetValue(object o, object value)
			{
				if (value == null || value is Guid? || value is Guid)
					_memberAccessor.SetValue(o, value);
				else
					_memberAccessor.SetValue(o, new Guid(value.ToString()));
			}
		}

		#endregion

#endif
	}
}
