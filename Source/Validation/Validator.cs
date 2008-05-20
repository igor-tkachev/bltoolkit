using System;
using System.Collections.Generic;
using System.ComponentModel;

using BLToolkit.Reflection;

namespace BLToolkit.Validation
{
	public class Validator
	{
		#region Validate

		public static void Validate(ValidationContext context)
		{
			foreach (MemberAccessor ma in context.TypeAccessor)
			{
				ValidatorBaseAttribute[] attrs = ma.GetAttributes<ValidatorBaseAttribute>();

				if (attrs == null)
					continue;

				context.MemberAccessor = ma;
				context.Value          = ma.GetValue(context.Object);

				for (int i = 0; i < attrs.Length; i++)
				{
					ValidatorBaseAttribute attr = attrs[i];
					if (attr.IsValid(context) == false)
						throw new ValidationException(attr.GetErrorMessage(context));
				}
			}
		}

		public static void Validate(object obj, ValidationContext.IsNullHandler isNull)
		{
			Validate(InitContext(null, obj, null, isNull));
		}

		public static void Validate(object obj)
		{
			Validate(obj, null);
		}

		#endregion

		#region Protected Members

		private static bool IsNullInternal(ValidationContext context)
		{
			if (context.Value == null)
				return true;

			if (context.NullValue is DBNull)
				return false;

			return context.NullValue.Equals(context.Value);
		}

		public static ValidationContext InitContext(
			ValidationContext  context,
			object             obj,
			PropertyDescriptor pd,
			ValidationContext.IsNullHandler isNull)
		{
			if (context == null)
				context = new ValidationContext();

			context.Object = obj;
			context.IsNull = isNull ?? new ValidationContext.IsNullHandler(IsNullInternal);
			context.PropertyDescriptor = pd;

			return context;
		}

		#endregion

		#region IsValid

		public static bool IsValid(ValidationContext context, string fieldName)
		{
			ValidatorBaseAttribute[] attrs = null;
			object                   value = null;

			if (context.PropertyDescriptor != null)
			{
				value = context.PropertyDescriptor.GetValue(context.Object);

				List<ValidatorBaseAttribute> list = null;

				foreach (object o in context.PropertyDescriptor.Attributes)
				{
					if (o is ValidatorBaseAttribute)
					{
						if (list == null)
							list = new List<ValidatorBaseAttribute>();

						list.Add((ValidatorBaseAttribute)o);
					}
				}

				if (list != null)
					attrs = list.ToArray();
			}
			else
			{
				context.MemberAccessor = context.TypeAccessor[fieldName];

				if (context.MemberAccessor != null)
				{
					value = context.MemberAccessor.GetValue(context.Object);
					attrs = context.MemberAccessor.GetAttributes<ValidatorBaseAttribute>();
				}
			}

			if (attrs != null)
			{
				context.Value = value;

				for (int i = 0; i < attrs.Length; i++)
				{
					if (!attrs[i].IsValid(context))
						return false;
				}
			}

			return true;
		}

		public static bool IsValid(object obj, string fieldName, ValidationContext.IsNullHandler isNull)
		{
			return IsValid(InitContext(null, obj, null, isNull), fieldName);
		}

		public static bool IsValid(object obj, PropertyDescriptor pd, ValidationContext.IsNullHandler isNull)
		{
			return IsValid(InitContext(null, obj, pd, isNull), pd.Name);
		}

		public static bool IsValid(object obj, string fieldName)
		{
			return IsValid(obj, fieldName, null);
		}

		public static bool IsValid(object obj, PropertyDescriptor pd)
		{
			return IsValid(obj, pd, null);
		}

		#endregion

		#region GetErrorMessages

		public static string[] GetErrorMessages(ValidationContext context, string fieldName)
		{
			context.MemberAccessor = context.TypeAccessor[fieldName];

			if (context.MemberAccessor != null)
			{
				List<string>          messages = new List<string>();
				ValidatorBaseAttribute[] attrs = context.MemberAccessor.GetAttributes<ValidatorBaseAttribute>();

				if (attrs != null)
				{
					context.Value = context.MemberAccessor.GetValue(context.Object);

					for (int i = 0; i < attrs.Length; i++)
						messages.Add(attrs[i].GetErrorMessage(context));

					return messages.ToArray();
				}
			}

			return new string[0];
		}

		public static string[] GetErrorMessages(
			object obj, string fieldName, ValidationContext.IsNullHandler isNull)
		{
			return GetErrorMessages(InitContext(null, obj, null, isNull), fieldName);
		}

		public static string[] GetErrorMessages(
			object obj, PropertyDescriptor pd, ValidationContext.IsNullHandler isNull)
		{
			return GetErrorMessages(InitContext(null, obj, pd, isNull), pd.Name);
		}

		public static string[] GetErrorMessages(object obj, string fieldName)
		{
			return GetErrorMessages(obj, fieldName, null);
		}

		public static string[] GetErrorMessages(object obj, PropertyDescriptor pd)
		{
			return GetErrorMessages(obj, pd, null);
		}

		#endregion
	}
}
