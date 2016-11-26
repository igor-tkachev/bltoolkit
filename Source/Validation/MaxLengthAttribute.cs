using System;

namespace BLToolkit.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class MaxLengthAttribute : ValidatorBaseAttribute
	{
		public MaxLengthAttribute(Int64 maxLength)
		{
			_value = maxLength;
		}

		public MaxLengthAttribute(Int64 maxLength, string errorMessage)
			: this(maxLength)
		{
			ErrorMessage = errorMessage;
		}

		private readonly Int64 _value;
		public           Int64  Value
		{
			get { return _value; }
		}
	
		public override bool IsValid(ValidationContext context)
		{
			return context.IsNull(context) || context.Value.ToString().Length <= _value;
		}

		public override string ErrorMessage
		{
			get { return base.ErrorMessage ?? "'{0}' maximum length is {1}."; }
			set { base.ErrorMessage = value; }
		}

		public override string GetErrorMessage(ValidationContext context)
		{
			return string.Format(ErrorMessage, GetPropertyFriendlyName(context), Value);
		}
	}
}
