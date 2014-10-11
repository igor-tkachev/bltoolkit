using System;

namespace BLToolkit.TypeBuilder
{
	///<summary>
	/// Indicates that a field, property or method can be treated as a value getter.
	///</summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
	public sealed class GetValueAttribute : Attribute
	{
	}
}
