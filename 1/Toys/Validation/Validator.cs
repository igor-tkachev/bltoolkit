using System;

using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.Validation
{
	public class Validator
	{
		public static void Validate(ValidationContext context)
		{
			if (context.IsNull == null)
				context.IsNull = new ValidationContext.IsNullHandler(IsNull);

			foreach (IMemberMapper mm in context.Descriptor)
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
			ValidationContext context = new ValidationContext();

			context.Object = obj;
			context.IsNull = isNull;

			Validate(context);
		}

		public static void Validate(object obj)
		{
			Validate(obj, null);
		}

		protected static bool IsNull(ValidationContext context)
		{
			return Map.IsNull(context.Value);
		}
	}
}
