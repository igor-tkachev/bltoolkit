using System;

using BLToolkit.Reflection;

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
			get { return _name;  }
		}

		private int _ordinal;
		public  int  Ordinal
		{
			get { return _ordinal;  }
		}

		internal void SetOrdinal(int ordinal)
		{
			_ordinal = ordinal;
		}

		public virtual void Init(string name, MemberAccessor memberAccessor)
		{
			_name           = name;
			_memberAccessor = memberAccessor;
		}

		public virtual object GetValue(object o)
		{
			return _memberAccessor.GetValue(o);
		}

		public virtual void SetValue(object o, object value)
		{
			_memberAccessor.SetValue(o, value);
		}

		internal static MemberMapper CreateMemberMapper(Type type)
		{
			MemberMapper mm = null;

			if (type.IsPrimitive)
			{
				mm = GetPrimitiveMemberMapper(type);
			}

			if (mm == null)
				mm = new MemberMapper();

			return mm;
		}

		#region Primitive Mappers

		private static MemberMapper GetPrimitiveMemberMapper(Type type)
		{
			if      (type == typeof(Int32))   return new MemberMapper.Int32Mapper();
			else if (type == typeof(Double))  return new MemberMapper.DoubleMapper();
			else if (type == typeof(Int16))   return new MemberMapper.Int16Mapper();
			else if (type == typeof(SByte))   return new MemberMapper.SByteMapper();
			else if (type == typeof(Int64))   return new MemberMapper.Int64Mapper();
			else if (type == typeof(Byte))    return new MemberMapper.ByteMapper();
			else if (type == typeof(UInt16))  return new MemberMapper.UInt16Mapper();
			else if (type == typeof(UInt32))  return new MemberMapper.UInt32Mapper();
			else if (type == typeof(UInt64))  return new MemberMapper.UInt64Mapper();
			else if (type == typeof(UInt64))  return new MemberMapper.CharMapper();
			else if (type == typeof(Single))  return new MemberMapper.SingleMapper();
			else if (type == typeof(Boolean)) return new MemberMapper.BooleanMapper();

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
	}
}
