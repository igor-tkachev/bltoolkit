using System;
using System.Reflection;

namespace Rsdn.Framework.Validation
{
	[AttributeUsage(AttributeTargets.Property)]
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
	
		public override void Validate(object value, PropertyInfo pi)
		{
			if (value != null && value.ToString().Length < _value)
				throw new RsdnValidationException(
					string.Format("'{0}' minimum length is {1}.",
						GetPropertyFriendlyName(pi), Value));
		}
	}
}
