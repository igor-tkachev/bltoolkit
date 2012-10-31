using System;

namespace BLToolkit.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class MinValueAttribute : ValidatorBaseAttribute
	{
		public MinValueAttribute(object minValue)
			: this(minValue, false)
		{
		}

		public MinValueAttribute(object minValue, string errorMessage)
			: this(minValue, false, errorMessage)
		{
		}

		public MinValueAttribute(object minValue, bool isExclusive)
		{
			_value       = minValue;
			_isExclusive = isExclusive;
		}

		public MinValueAttribute(object minValue, bool isExclusive, string errorMessage)
			: this(minValue, isExclusive)
		{
			ErrorMessage = errorMessage;
		}

		private readonly object _value;
		public  virtual  object GetValue(ValidationContext context)
		{
			return _value;
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

			object contextValue = context.Value;
			object testValue    = GetValue(context);

			if (contextValue is Int32)
			{
				Int32 tv = Convert.ToInt32(testValue);
				return tv < (Int32)contextValue || !IsExclusive && tv == (Int32)contextValue;
			}

			if (contextValue is decimal)
			{
				decimal tv = Convert.ToDecimal(testValue);
				return tv < (decimal)contextValue || !IsExclusive && tv == (decimal)contextValue;
			}

			if (contextValue is double)
			{
				double tv = Convert.ToDouble(testValue);
				return tv < (double)contextValue || !IsExclusive && tv == (double)contextValue;
			}

			if (contextValue is float)
			{
				float tv = Convert.ToSingle(testValue);
				return tv < (float)contextValue || !IsExclusive && tv == (float)contextValue;
			}

			if (contextValue is byte)
			{
				byte tv = Convert.ToByte(testValue);
				return tv < (byte)contextValue || !IsExclusive && tv == (byte)contextValue;
			}

			if (contextValue is char)
			{
				char tv = Convert.ToChar(testValue);
				return tv < (char)contextValue || !IsExclusive && tv == (char)contextValue;
			}

			if (contextValue is Int16)
			{
				Int16 tv = Convert.ToInt16(testValue);
				return tv < (Int16)contextValue || !IsExclusive && tv == (Int16)contextValue;
			}

			if (contextValue is sbyte)
			{
				sbyte tv = Convert.ToSByte(testValue);
				return tv < (sbyte)contextValue || !IsExclusive && tv == (sbyte)contextValue;
			}

			if (contextValue is UInt16)
			{
				UInt16 tv = Convert.ToUInt16(testValue);
				return tv < (UInt16)contextValue || !IsExclusive && tv == (UInt16)contextValue;
			}

			if (contextValue is UInt32)
			{
				UInt32 tv = Convert.ToUInt32(testValue);
				return tv < (UInt32)contextValue || !IsExclusive && tv == (UInt32)contextValue;
			}

			if (contextValue is Int64)
			{
				Int64 tv = Convert.ToInt64(testValue);
				return tv < (Int64)contextValue || !IsExclusive && tv == (Int64)contextValue;
			}

			if (contextValue is UInt64)
			{
				UInt64 tv = Convert.ToUInt64(testValue);
				return tv < (UInt64)contextValue || !IsExclusive && tv == (UInt64)contextValue;
			}

			return true;
		}

		public override string ErrorMessage
		{
			get { return base.ErrorMessage ?? "Minimum value for '{0}' is {1}{2}."; }
			set { base.ErrorMessage = value; }
		}

		public override string GetErrorMessage(ValidationContext context)
		{
			return string.Format(ErrorMessage,
				GetPropertyFriendlyName(context),
				GetValue(context),
				IsExclusive? " exclusive": string.Empty);
		}
	}
}
