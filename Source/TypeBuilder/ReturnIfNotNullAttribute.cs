using System;

namespace BLToolkit.TypeBuilder
{
	[AttributeUsage(AttributeTargets.ReturnValue)]
	public sealed class ReturnIfNotNullAttribute : ReturnIfNonZeroAttribute
	{
	}
}
