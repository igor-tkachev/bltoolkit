using System;
using System.Text.RegularExpressions;

namespace BLToolkit.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class RegExAttribute : ValidatorBaseAttribute 
	{
		public RegExAttribute(string regex)
		{
			_value = regex;
		}

		public RegExAttribute(string regex, string errorMessage)
			: this(regex)
		{
			ErrorMessage = errorMessage;
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

		public override string ErrorMessage
		{
			get { return base.ErrorMessage != null? base.ErrorMessage: "'{0}' format is not valid."; }
			set { base.ErrorMessage = value; }
		}
	}
}
