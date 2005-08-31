using System;
using System.Reflection;

namespace Rsdn.Framework.Validation
{
	[AttributeUsage(AttributeTargets.Property)]
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
	
		public override void Validate(object value, MemberInfo mi)
		{
			if (value != null && value.ToString().Length > _value)
				throw new RsdnValidationException(
					string.Format("'{0}' maximum length is {1}.",
						GetPropertyFriendlyName(mi), Value));
		}
	}
}
