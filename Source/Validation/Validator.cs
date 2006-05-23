using System;
using System.Collections;
using System.ComponentModel;

using BLToolkit.Mapping;
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

			if (context.NullValue == null)
			{
				ObjectMapper om = Map.GetObjectMapper(context.Object.GetType());
				MemberMapper mm = om[context.MemberName, true];

				context.NullValue =
					mm != null && mm.MapMemberInfo.Nullable && mm.MapMemberInfo.NullValue != null?
						mm.MapMemberInfo.NullValue:
						TypeAccessor.GetNullValue(context.Value.GetType());

				if (context.NullValue == null)
					context.NullValue = DBNull.Value;
			}

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
			context.IsNull = isNull != null? isNull: new ValidationContext.IsNullHandler(IsNullInternal);
			context.PropertyDescriptor = pd;

			return context;
		}

		#endregion

		#region IsValid

		public static bool IsValid(ValidationContext context, string fieldName)
		{
			object[] attrs = null;
			object   value = null;

			if (context.PropertyDescriptor != null)
			{
				value = context.PropertyDescriptor.GetValue(context.Object);

				ArrayList list = null;

				foreach (object o in context.PropertyDescriptor.Attributes)
				{
					if (o is ValidatorBaseAttribute)
					{
						if (list == null)
							list = new ArrayList();

						list.Add(o);
					}
				}

				if (list != null)
				{
					attrs = new object[list.Count];
					list.CopyTo(attrs);
				}
			}
			else
			{
				context.MemberAccessor = context.TypeAccessor[fieldName];

				if (context.MemberAccessor != null)
				{
					value = context.MemberAccessor.GetValue(context.Object);
					attrs = context.MemberAccessor.GetAttributes(typeof(ValidatorBaseAttribute));
				}
			}

			if (attrs != null)
			{
				foreach (ValidatorBaseAttribute attr in attrs)
				{
					context.Value = value;

					if (attr.IsValid(context) == false)
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
