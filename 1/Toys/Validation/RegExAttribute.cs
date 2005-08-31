using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Rsdn.Framework.Validation
{
	[AttributeUsage(AttributeTargets.Property)]
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

		public override void Validate(object value, MemberInfo mi)
		{
			if (value != null && Regex.IsMatch(value.ToString(), Value) == false)
				throw new RsdnValidationException(
					string.Format("'{0}' format is not valid.", GetPropertyFriendlyName(mi)));
		}
	}
}
