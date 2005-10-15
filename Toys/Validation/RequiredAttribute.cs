using System;

namespace Rsdn.Framework.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class RequiredAttribute : ValidatorBaseAttribute
	{
		public override bool IsValid(ValidationContext context)
		{
			return context.IsNull(context) == false;
		}

		public override string GetErrorMessage(ValidationContext context)
		{
			return string.Format("'{0}' is required.", GetPropertyFriendlyName(context));
		}
	}
}
