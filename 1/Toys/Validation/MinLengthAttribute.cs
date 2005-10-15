using System;

namespace Rsdn.Framework.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class MinLengthAttribute : ValidatorBaseAttribute
	{
		public MinLengthAttribute(int minLength)
		{
			_value = minLength;
		}

		private int _value;
		public  int  Value
		{
			get { return _value; }
		}
	
		public override bool IsValid(ValidationContext context)
		{
			return context.IsNull(context) || context.Value.ToString().Length >= _value;
		}

		public override string GetErrorMessage(ValidationContext context)
		{
			return string.Format("'{0}' minimum length is {1}.",
				GetPropertyFriendlyName(context), Value);
		}
	}
}
