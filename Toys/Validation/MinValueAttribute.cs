using System;
using System.Reflection;

namespace Rsdn.Framework.Validation
{
	[AttributeUsage(AttributeTargets.Property)]
	public class MinValueAttribute : ValidatorBaseAttribute
	{
		public MinValueAttribute(object minValue)
			: this (minValue, false)
		{
		}

		public MinValueAttribute(object minValue, bool isExclusive)
		{
			_value       = minValue;
			_isExclusive = isExclusive;
		}

		private object _value;
		public  object  Value
		{
			get { return _value;}
		}

		private bool _isExclusive;
		public  bool  IsExclusive
		{
			get { return _isExclusive;  }
			set { _isExclusive = value; }
		}

		protected void ThrowException(MemberInfo mi)
		{
			throw new RsdnValidationException(
				string.Format("Minimum {0} value is {1}{2}.",
				GetPropertyFriendlyName(mi),
				Value,
				IsExclusive? " exclusive": string.Empty));
		}

		public override void Validate(object value, MemberInfo mi)
		{
			if (Value is byte &&
			    ( IsExclusive && (byte)Value    >= Convert.ToByte(value)  ||
			     !IsExclusive && (byte)Value    >  Convert.ToByte(value)) ||
			    Value is char &&
			    ( IsExclusive && (char)Value    >= Convert.ToByte(value)  ||
			     !IsExclusive && (char)Value    >  Convert.ToByte(value)) ||
			    Value is decimal &&
			    ( IsExclusive && (decimal)Value >= Convert.ToDecimal(value)  ||
			     !IsExclusive && (decimal)Value >  Convert.ToDecimal(value)) ||
			    Value is double &&
			    ( IsExclusive && (double)Value  >= Convert.ToDouble(value)  ||
			     !IsExclusive && (double)Value  >  Convert.ToDouble(value)) ||
			    Value is Int16 &&
			    ( IsExclusive && (Int16)Value   >= Convert.ToInt16(value)  ||
			     !IsExclusive && (Int16)Value   >  Convert.ToInt16(value)) ||
			    Value is Int32 &&
			    ( IsExclusive && (Int32)Value   >= Convert.ToInt32(value)  ||
			     !IsExclusive && (Int32)Value   >  Convert.ToInt32(value)) ||
			    Value is Int64 &&
			    ( IsExclusive && (Int64)Value   >= Convert.ToInt64(value)  ||
			     !IsExclusive && (Int64)Value   >  Convert.ToInt64(value)) ||
			    Value is sbyte &&
			    ( IsExclusive && (sbyte)Value   >= Convert.ToSByte(value)  ||
			     !IsExclusive && (sbyte)Value   >  Convert.ToSByte(value)) ||
			    Value is float &&
			    ( IsExclusive && (float)Value   >= Convert.ToSingle(value)  ||
			     !IsExclusive && (float)Value   >  Convert.ToSingle(value)) ||
			    Value is UInt16 &&
			    ( IsExclusive && (UInt16)Value  >= Convert.ToUInt16(value)  ||
			     !IsExclusive && (UInt16)Value  >  Convert.ToUInt16(value)) ||
			    Value is UInt32 &&
			    ( IsExclusive && (Int32)Value   >= Convert.ToUInt32(value)  ||
			     !IsExclusive && (Int32)Value   >  Convert.ToUInt32(value)) ||
			    Value is UInt64 &&
			    ( IsExclusive && (UInt64)Value  >= Convert.ToUInt64(value)  ||
			     !IsExclusive && (UInt64)Value  >  Convert.ToUInt64(value))
				)
			{
				ThrowException(mi);
			}
		}
	}
}
