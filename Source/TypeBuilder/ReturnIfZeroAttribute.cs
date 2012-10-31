using System;
using System.Diagnostics.CodeAnalysis;

namespace BLToolkit.TypeBuilder
{
	[SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes")]
	[AttributeUsage(AttributeTargets.ReturnValue)]
	public class ReturnIfZeroAttribute : Attribute
	{
	}
}
