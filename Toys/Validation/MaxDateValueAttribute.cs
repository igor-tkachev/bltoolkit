using System;
using System.Reflection;

namespace Rsdn.Framework.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class MaxDateValueAttribute : MaxValueAttribute
	{
		public MaxDateValueAttribute(int year, int month, int day)
			: base(new DateTime(year, month, day))
		{
		}

		public MaxDateValueAttribute(int year, int month, int day, bool isExclusive)
			: base(new DateTime(year, month, day), isExclusive)
		{
		}

		public override bool IsValid(ValidationContext context)
		{
			if (context.IsNull(context))
				return true;

			DateTime v = Convert.ToDateTime(context.Value);

			return (DateTime)Value > v || !IsExclusive && (DateTime)Value == v;
		}
	}
}
