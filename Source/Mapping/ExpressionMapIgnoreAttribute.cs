using System;

namespace BLToolkit.Mapping
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public sealed class ExpressionMapIgnoreAttribute : Attribute
	{
		public ExpressionMapIgnoreAttribute()
		{
			Ignore = true;
		}

		public ExpressionMapIgnoreAttribute(bool ignore)
		{
			Ignore = ignore;
		}

		public bool Ignore { get; set; }
	}
}
