using System;
using System.Reflection;

namespace Rsdn.Framework.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class MaxLengthAttribute : ValidatorBaseAttribute
	{
		public MaxLengthAttribute(int maxLength)
		{
			_value = maxLength;
		}

		private int _value;
		public  int  Value
		{
			get { return _value; }
		}
	
		public override bool IsValid(ValidationContext context)
		{
			return context.IsNull(context) || context.Value.ToString().Length <= _value;
		}

		public override string GetErrorMessage(ValidationContext context)
		{
			return string.Format("'{0}' maximum length is {1}.",
				GetPropertyFriendlyName(context), Value);
		}
	}
}
