using System;
using System.Reflection;

namespace Rsdn.Framework.Validation
{
	[AttributeUsage(AttributeTargets.Property)]
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

		public override void Validate(object value, MemberInfo mi)
		{
			if ( IsExclusive && (DateTime)Value <= Convert.ToDateTime(value)  ||
				!IsExclusive && (DateTime)Value <  Convert.ToDateTime(value))
			{
				ThrowException(mi);
			}
		}
	}
}
