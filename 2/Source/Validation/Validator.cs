using System;
using System.Collections;

using BLToolkit.Reflection;

namespace BLToolkit.Validation
{
	public class Validator
	{
		#region Validate

		public static void Validate(ValidationContext context)
		{
			ArrayList list = new ArrayList();

			foreach (MemberAccessor ma in context.TypeAccessor)
			{
				context.MemberAccessor = ma;

				object[] attrs = ma.GetAttributes(typeof(ValidatorBaseAttribute));

				if (attrs == null)
					continue;

				foreach (ValidatorBaseAttribute attr in attrs)
				{
					context.Value = ma.GetValue(context.Object);

					if (attr.IsValid(context) == false)
						throw new ValidationException(
							attr.GetErrorMessage(context));
				}
			}
		}

		public static void Validate(object obj, ValidationContext.IsNullHandler isNull)
		{
			Validate(InitContext(null, obj, isNull));
		}

		public static void Validate(object obj)
		{
			Validate(obj, null);
		}

		#endregion

		#region Protected Members

		private static bool IsNullInternal(ValidationContext context)
		{
			return TypeAccessor.IsNull(context.Value.GetType());
		}

		public static ValidationContext InitContext(
			ValidationContext context, object obj, ValidationContext.IsNullHandler isNull)
		{
			if (context == null)
				context = new ValidationContext();

			context.Object = obj;
			context.IsNull = isNull != null? isNull: new ValidationContext.IsNullHandler(IsNullInternal);

			return context;
		}

		#endregion

		#region IsValid

		public static bool IsValid(ValidationContext context, string fieldName)
		{
			context.MemberAccessor = context.TypeAccessor[fieldName];

			if (context.MemberAccessor != null)
			{
				object[] attrs = context.MemberAccessor.GetAttributes(typeof(ValidatorBaseAttribute));

				if (attrs != null)
				{
					foreach (ValidatorBaseAttribute attr in attrs)
					{
						context.Value = context.MemberAccessor.GetValue(context.Object);

						if (attr.IsValid(context) == false)
							return false;
					}
				}
			}

			return true;
		}

		public static bool IsValid(object obj, string fieldName, ValidationContext.IsNullHandler isNull)
		{
			return IsValid(InitContext(null, obj, isNull), fieldName);
		}

		public static bool IsValid(object obj, string fieldName)
		{
			return IsValid(obj, fieldName, null);
		}

		#endregion

		#region GetErrorMessages

		public static string[] GetErrorMessages(ValidationContext context, string fieldName)
		{
			context.MemberAccessor = context.TypeAccessor[fieldName];

			if (context.MemberAccessor != null)
			{
				ArrayList messages = new ArrayList();
				object[]  attrs    = context.MemberAccessor.GetAttributes(typeof(ValidatorBaseAttribute));

				if (attrs != null)
				{
					foreach (ValidatorBaseAttribute attr in attrs)
					{
						context.Value = context.MemberAccessor.GetValue(context.Object);
						messages.Add(attr.GetErrorMessage(context));
					}

					return (string[])messages.ToArray(typeof(string));
				}
			}

			return new string[0];
		}

		public static string[] GetErrorMessages(object obj, string fieldName, ValidationContext.IsNullHandler isNull)
		{
			return GetErrorMessages(InitContext(null, obj, isNull), fieldName);
		}

		public static string[] GetErrorMessages(object obj, string fieldName)
		{
			return GetErrorMessages(obj, fieldName, null);
		}

		#endregion
	}
}
