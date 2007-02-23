using System;
using System.Reflection;

namespace Rsdn.Framework.Validation
{
	public abstract class ValidatorBaseAttribute : Attribute
	{
		public ValidatorBaseAttribute()
		{
		}

		public ValidatorBaseAttribute(string errorMessage)
		{
			_errorMessage = errorMessage;
		}

		private        string _errorMessage;
		public virtual string  ErrorMessage
		{
			get { return _errorMessage;  }
			set { _errorMessage = value; }
		}

		public abstract bool   IsValid        (ValidationContext context);

		public virtual string GetErrorMessage(ValidationContext context)
		{
			return string.Format(ErrorMessage, GetPropertyFriendlyName(context));
		}

		protected string GetPropertyFriendlyName(ValidationContext context)
		{
			MemberInfo mi        = context.MemberInfo;
			string     className = mi.DeclaringType.Name;

			object[] attrs = mi.DeclaringType.GetCustomAttributes(typeof(FriendlyNameAttribute), true);

			if (attrs.Length > 0)
				className = ((FriendlyNameAttribute)attrs[0]).Name;

			string fieldName =
				className == null || className.Length == 0? mi.Name: className + "." + mi.Name;

			attrs = mi.GetCustomAttributes(typeof(FriendlyNameAttribute), true);

			if (attrs.Length > 0)
				fieldName = ((FriendlyNameAttribute)attrs[0]).Name;

			return fieldName;
		}
	}
}
