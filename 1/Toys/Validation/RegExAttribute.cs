using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Rsdn.Framework.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class RegExAttribute : ValidatorBaseAttribute 
	{
		public RegExAttribute(string regex)
		{
			_value = regex;
		}

		private string _value;
		public  string  Value
		{
			get { return _value; }
		}

		public override bool IsValid(ValidationContext context)
		{
			return context.IsNull(context) || Regex.IsMatch(context.Value.ToString(), Value);
		}

		public override string GetErrorMessage(ValidationContext context)
		{
			return string.Format("'{0}' format is not valid.", GetPropertyFriendlyName(context));
		}
	}
}
