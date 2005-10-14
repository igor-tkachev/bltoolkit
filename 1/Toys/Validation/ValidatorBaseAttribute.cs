using System;
using System.Reflection;

namespace Rsdn.Framework.Validation
{
	public abstract class ValidatorBaseAttribute : Attribute
	{
		public abstract bool   IsValid        (ValidationContext context);
		public abstract string GetErrorMessage(ValidationContext context);

		protected string GetPropertyFriendlyName(ValidationContext context)
		{
			MemberInfo mi        = context.MemberInfo;
			string     className = mi.DeclaringType.Name;

			object[] attrs = mi.DeclaringType.GetCustomAttributes(typeof(FriendlyNameAttribute), true);

			if (attrs.Length > 0)
				className = ((FriendlyNameAttribute)attrs[0]).Name;

			string fieldName = className + "." + mi.Name;

			attrs = mi.GetCustomAttributes(typeof(FriendlyNameAttribute), true);

			if (attrs.Length > 0)
				fieldName = ((FriendlyNameAttribute)attrs[0]).Name;

			return fieldName;
		}
	}
}
