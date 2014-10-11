using System;

namespace BLToolkit.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class MaxLengthAttribute : ValidatorBaseAttribute
	{
		public MaxLengthAttribute(int maxLength)
		{
			_value = maxLength;
		}

		public MaxLengthAttribute(int maxLength, string errorMessage)
			: this(maxLength)
		{
			ErrorMessage = errorMessage;
		}

		private readonly int _value;
		public           int  Value
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
