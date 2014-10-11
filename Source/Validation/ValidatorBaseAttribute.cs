using System;
using System.Reflection;
using System.ComponentModel;

namespace BLToolkit.Validation
{
	public abstract class ValidatorBaseAttribute : Attribute
	{
		protected ValidatorBaseAttribute()
		{
		}

		protected ValidatorBaseAttribute(string errorMessage)
		{
			_errorMessage = errorMessage;
		}

		private        string _errorMessage;
		public virtual string  ErrorMessage
		{
			get { return _errorMessage;  }
			set { _errorMessage = value; }
		}

		public abstract bool IsValid(ValidationContext context);

		public virtual string GetErrorMessage(ValidationContext context)
		{
			return string.Format(ErrorMessage, GetPropertyFriendlyName(context));
		}

		protected virtual string GetPropertyFriendlyName(ValidationContext context)
		{
			MemberInfo mi        = context.MemberInfo;
			string     className = mi.DeclaringType.Name;
			string     fieldName = mi.Name;

			// Get class name.
			//
			object[] attrs = mi.DeclaringType.GetCustomAttributes(typeof(FriendlyNameAttribute), true);

			if (attrs.Length > 0)
				className = ((FriendlyNameAttribute)attrs[0]).Name;
			else
			{
				attrs = mi.DeclaringType.GetCustomAttributes(typeof(DisplayNameAttribute), true);

				if (attrs.Length > 0)
					className = ((DisplayNameAttribute)attrs[0]).DisplayName;
			}

			// Get field name.
			//
			attrs = mi.GetCustomAttributes(typeof(FriendlyNameAttribute), true);

			if (attrs.Length > 0)
				fieldName = ((FriendlyNameAttribute)attrs[0]).Name;
			else
			{
				attrs = mi.GetCustomAttributes(typeof(DisplayNameAttribute), true);

				if (attrs.Length > 0)
					fieldName = ((DisplayNameAttribute)attrs[0]).DisplayName;
			}

			return string.IsNullOrEmpty(className)? fieldName: className + "." + fieldName;
		}
	}
}
