using System;

namespace BLToolkit.TypeBuilder
{
	[AttributeUsage(AttributeTargets.ReturnValue)]
	public class ReturnIfTrueAttribute : ReturnIfNonZeroAttribute
	{
	}
}
