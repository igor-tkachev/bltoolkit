using System;

namespace Rsdn.Framework.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
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
		public  virtual object GetValue(ValidationContext context)
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

			if (testValue is Int32)
			{
				Int32 v = Convert.ToInt32(contextValue);
				return (Int32)testValue < v || !IsExclusive && (Int32)testValue == v;
			}

			if (testValue is decimal)
			{
				decimal v = Convert.ToDecimal(contextValue);
				return (decimal)testValue < v || !IsExclusive && (decimal)testValue == v;
			}

			if (testValue is double)
			{
				double v = Convert.ToDouble(contextValue);
				return (double)testValue < v || !IsExclusive && (double)testValue == v;
			}

			if (testValue is Int64)
			{
				Int64 v = Convert.ToInt64(contextValue);
				return (Int64)testValue < v || !IsExclusive && (Int64)testValue == v;
			}

			if (testValue is float)
			{
				float v = Convert.ToSingle(contextValue);
				return (float)testValue < v || !IsExclusive && (float)testValue == v;
			}

			if (testValue is byte)
			{
				byte v = Convert.ToByte(contextValue);
				return (byte)testValue < v || !IsExclusive && (byte)testValue == v;
			}

			if (testValue is char)
			{
				char v = Convert.ToChar(contextValue);
				return (char)testValue < v || !IsExclusive && (char)testValue == v;
			}

			if (testValue is Int16)
			{
				Int16 v = Convert.ToInt16(contextValue);
				return (Int16)testValue < v || !IsExclusive && (Int16)testValue == v;
			}

			if (testValue is sbyte)
			{
				sbyte v = Convert.ToSByte(contextValue);
				return (sbyte)testValue < v || !IsExclusive && (sbyte)testValue == v;
			}

			if (testValue is UInt16)
			{
				UInt16 v = Convert.ToUInt16(contextValue);
				return (UInt16)testValue < v || !IsExclusive && (UInt16)testValue == v;
			}

			if (testValue is UInt32)
			{
				UInt32 v = Convert.ToUInt32(contextValue);
				return (Int32)testValue < v || !IsExclusive && (Int32)testValue == v;
			}

			if (testValue is UInt64)
			{
				UInt64 v = Convert.ToUInt64(contextValue);
				return (UInt64)testValue < v || !IsExclusive && (UInt64)testValue == v;
			}

			return true;
		}

		public override string GetErrorMessage(ValidationContext context)
		{
			return string.Format("Minimum '{0}' value is {1}{2}.",
				GetPropertyFriendlyName(context),
			    GetValue(context),
				IsExclusive? " exclusive": string.Empty);
		}
	}
}
