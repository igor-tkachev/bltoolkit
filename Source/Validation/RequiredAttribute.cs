using System;

namespace BLToolkit.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class RequiredAttribute : ValidatorBaseAttribute
	{
		public RequiredAttribute()
		{
		}

		public RequiredAttribute(string errorMessage)
			: base(errorMessage)
		{
		}

		public override bool IsValid(ValidationContext context)
		{
			return context.IsNull(context) == false;
		}

		public override string ErrorMessage
		{
			get { return base.ErrorMessage ?? "'{0}' is required."; }
			set { base.ErrorMessage = value; }
		}
	}
}
