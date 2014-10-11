using System;

namespace BLToolkit.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class MaxDateValueAttribute : MaxValueAttribute
	{
		public MaxDateValueAttribute(int year, int month, int day)
			: base(new DateTime(year, month, day))
		{
		}

		public MaxDateValueAttribute(int year, int month, int day, string errorMessage)
			: this(year, month, day)
		{
			ErrorMessage = errorMessage;
		}

		public MaxDateValueAttribute(int year, int month, int day, bool isExclusive)
			: base(new DateTime(year, month, day), isExclusive)
		{
		}

		public MaxDateValueAttribute(int year, int month, int day, bool isExclusive, string errorMessage)
			: this(year, month, day, isExclusive)
		{
			ErrorMessage = errorMessage;
		}

		public override bool IsValid(ValidationContext context)
		{
			if (context.IsNull(context))
				return true;

			DateTime contextValue = Convert.ToDateTime(context.Value);
			DateTime testValue    = (DateTime)GetValue(context);

			return testValue > contextValue || !IsExclusive && testValue == contextValue;
		}
	}
}
