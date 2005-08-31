using System;
using System.Reflection;

namespace Rsdn.Framework.Validation
{
	[AttributeUsage(AttributeTargets.Property)]
	public class RequiredAttribute : ValidatorBaseAttribute
	{
		public override void Validate(object value, MemberInfo mi)
		{
			if (value == null ||
				(value is string   && value.ToString().Length == 0) ||
				(value is DateTime && (DateTime)value == DateTime.MinValue))
			{
				throw new RsdnValidationException(
					string.Format("'{0}' is required.", GetPropertyFriendlyName(mi)));
			}
		}
	}
}
