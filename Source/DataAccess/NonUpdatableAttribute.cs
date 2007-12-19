using System;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class NonUpdatableAttribute : Attribute
	{
	}
}
