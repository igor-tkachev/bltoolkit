using System;
using System.Collections;
using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.Validation
{
	public class Validator
	{
		#region Validate

		public static void Validate(ValidationContext context)
		{
			foreach (IMemberMapper mm in context.Descriptor.AllMembers)
			{
				context.MemberMapper = mm;

				object[] attrs = mm.MemberInfo.GetCustomAttributes(typeof(ValidatorBaseAttribute), true);

				foreach (ValidatorBaseAttribute attr in attrs)
				{
					context.Value = mm.GetValue(context.Object);

					if (attr.IsValid(context) == false)
						throw new RsdnValidationException(
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

		private static bool IsNull(ValidationContext context)
		{
			return Map.IsNull(context.Value);
		}

		public static ValidationContext InitContext(
			ValidationContext context, object obj, ValidationContext.IsNullHandler isNull)
		{
			if (context == null)
				context = new ValidationContext();

			context.Object = obj;
			context.IsNull = isNull != null? isNull: new ValidationContext.IsNullHandler(IsNull);

			return context;
		}

		#endregion

		#region IsValid

		public static bool IsValid(ValidationContext context, string fieldName)
		{
			foreach (IMemberMapper mm in context.Descriptor.AllMembers)
			{
				context.MemberMapper = mm;

				if (fieldName == mm.OriginalName)
				{
					object[] attrs = mm.MemberInfo.GetCustomAttributes(typeof(ValidatorBaseAttribute), true);

					foreach (ValidatorBaseAttribute attr in attrs)
					{
						context.Value = mm.GetValue(context.Object);

						if (attr.IsValid(context) == false)
							return false;
					}

					return true;
				}
			}

			if (context.Descriptor.GetMember(fieldName) != null)
				return IsValid(context, fieldName);

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
			foreach (IMemberMapper mm in context.Descriptor.AllMembers)
			{
				context.MemberMapper = mm;

				if (fieldName == mm.OriginalName)
				{
					ArrayList messages = new ArrayList();

					object[] attrs = mm.MemberInfo.GetCustomAttributes(typeof(ValidatorBaseAttribute), true);

					foreach (ValidatorBaseAttribute attr in attrs)
					{
						context.Value = mm.GetValue(context.Object);
						messages.Add(attr.GetErrorMessage(context));
					}

					return (string[])messages.ToArray(typeof(string));
				}
			}

			if (context.Descriptor.GetMember(fieldName) != null)
				return GetErrorMessages(context, fieldName);

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
