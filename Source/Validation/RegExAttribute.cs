using System;
using System.Text.RegularExpressions;

namespace BLToolkit.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class RegExAttribute : ValidatorBaseAttribute 
	{
		public RegExAttribute(string pattern)
			:this(pattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled)
		{
		}

		public RegExAttribute(string pattern, RegexOptions options)
		{
			_pattern = pattern;
			_options = options;
		}

		public RegExAttribute(string pattern, string errorMessage)
			: this(pattern)
		{
			ErrorMessage = errorMessage;
		}

		public RegExAttribute(string pattern, RegexOptions options, string errorMessage)
			:this(pattern, options)
		{
			ErrorMessage = errorMessage;
		}

		[Obsolete("Use RegExAttribute.Pattern instead.")]
		public string Value { get { return Pattern; } }

		private readonly string _pattern;
		public           string  Pattern { get { return _pattern; } }

		private readonly RegexOptions _options;
		public           RegexOptions  Options { get { return _options; } }

		[NonSerialized]
		private Regex _validator;
		public  Regex  Validator
		{
			get
			{
				if (_validator == null)
					_validator = new Regex(_pattern, _options);

				return _validator;
			}
		}

		public override bool IsValid(ValidationContext context)
		{
			if (context.IsNull(context))
				return true;

			Match match = Validator.Match(context.Value.ToString());

			return match.Success && match.Value == context.Value.ToString();
		}

		public override string ErrorMessage
		{
			get { return base.ErrorMessage != null? base.ErrorMessage: "'{0}' format is not valid."; }
			set { base.ErrorMessage = value; }
		}
	}
}
