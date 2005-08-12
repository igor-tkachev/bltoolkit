using System;
using System.Reflection;

namespace Rsdn.Framework.Validation
{
	public abstract class ValidatorBaseAttribute : Attribute
	{
		public abstract void Validate(object value, PropertyInfo pi);

		protected string GetPropertyFriendlyName(PropertyInfo pi)
		{
			string className = pi.DeclaringType.Name;

			object[] attrs = pi.DeclaringType.GetCustomAttributes(typeof(FriendlyNameAttribute), true);

			if (attrs.Length > 0)
				className = ((FriendlyNameAttribute)attrs[0]).Name;

			string fieldName = className + "." + pi.Name;

			attrs = pi.GetCustomAttributes(typeof(FriendlyNameAttribute), true);

			if (attrs.Length > 0)
				fieldName = ((FriendlyNameAttribute)attrs[0]).Name;

			return fieldName;
		}
	}
}
