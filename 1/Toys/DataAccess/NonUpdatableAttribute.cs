using System;

namespace Rsdn.Framework.DataAccess
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class NonUpdatableAttribute : Attribute
	{
	}
}
