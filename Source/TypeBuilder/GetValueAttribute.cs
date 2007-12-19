using System;

namespace BLToolkit.TypeBuilder
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public sealed class GetValueAttribute : Attribute
	{
	}
}
