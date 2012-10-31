using System;

namespace BLToolkit.Mapping
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class NotNullAttribute : NullableAttribute
	{
		public NotNullAttribute()
			: base(false)
		{
		}
	}
}
