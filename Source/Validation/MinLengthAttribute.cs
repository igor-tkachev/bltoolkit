using System;

namespace BLToolkit.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class MinLengthAttribute : ValidatorBaseAttribute
	{
		public MinLengthAttribute(int minLength)
		{
			_value = minLength;
		}

		public MinLengthAttribute(int minLength, string errorMessage)
			: this(minLength)
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
			return context.IsNull(context) || context.Value.ToString().Length >= _value;
		}

		public override string ErrorMessage
		{
			get { return base.ErrorMessage ?? "'{0}' minimum length is {1}."; }
			set { base.ErrorMessage = value; }
		}

		public override string GetErrorMessage(ValidationContext context)
		{
			return string.Format(ErrorMessage, GetPropertyFriendlyName(context), Value);
		}
	}
}
