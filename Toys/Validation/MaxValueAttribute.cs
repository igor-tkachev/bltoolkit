using System;

namespace Rsdn.Framework.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class MaxValueAttribute : ValidatorBaseAttribute
	{
		public MaxValueAttribute(object maxValue)
			: this (maxValue, false)
		{
		}

		public MaxValueAttribute(object maxValue, bool isExclusive)
		{
			_value       = maxValue;
			_isExclusive = isExclusive;
		}

		private         object _value;
		public  virtual object  Value
		{
			get { return _value;}
		}

		private bool _isExclusive;
		public  bool  IsExclusive
		{
			get { return _isExclusive;  }
			set { _isExclusive = value; }
		}

		public override bool IsValid(ValidationContext context)
		{
			if (context.IsNull(context))
				return true;

			object value = context.Value;

			if (Value is Int32)
			{
				Int32 v = Convert.ToInt32(value);
				return (Int32)Value > v || !IsExclusive && (Int32)Value == v;
			}

			if (Value is decimal)
			{
				decimal v = Convert.ToDecimal(value);
				return (decimal)Value > v || !IsExclusive && (decimal)Value == v;
			}

			if (Value is double)
			{
				double v = Convert.ToDouble(value);
				return (double)Value > v || !IsExclusive && (double)Value == v;
			}

			if (Value is Int64)
			{
				Int64 v = Convert.ToInt64(value);
				return (Int64)Value > v || !IsExclusive && (Int64)Value == v;
			}

			if (Value is float)
			{
				float v = Convert.ToSingle(value);
				return (float)Value > v || !IsExclusive && (float)Value == v;
			}

			if (Value is byte)
			{
				byte v = Convert.ToByte(value);
				return (byte)Value > v || !IsExclusive && (byte)Value == v;
			}

			if (Value is char)
			{
				char v = Convert.ToChar(value);
				return (char)Value > v || !IsExclusive && (char)Value == v;
			}

			if (Value is Int16)
			{
				Int16 v = Convert.ToInt16(value);
				return (Int16)Value > v || !IsExclusive && (Int16)Value == v;
			}

			if (Value is sbyte)
			{
				sbyte v = Convert.ToSByte(value);
				return (sbyte)Value > v || !IsExclusive && (sbyte)Value == v;
			}

			if (Value is UInt16)
			{
				UInt16 v = Convert.ToUInt16(value);
				return (UInt16)Value > v || !IsExclusive && (UInt16)Value == v;
			}

			if (Value is UInt32)
			{
				UInt32 v = Convert.ToUInt32(value);
				return (Int32)Value > v || !IsExclusive && (Int32)Value == v;
			}

			if (Value is UInt64)
			{
				UInt64 v = Convert.ToUInt64(value);
				return (UInt64)Value > v || !IsExclusive && (UInt64)Value == v;
			}

			return true;
		}

		public override string GetErrorMessage(ValidationContext context)
		{
			return string.Format("Maximum '{0}' value is {1}{2}.",
				GetPropertyFriendlyName(context),
				Value,
				IsExclusive? " exclusive": string.Empty);
		}
	}
}
