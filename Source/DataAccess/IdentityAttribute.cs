using System;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class IdentityAttribute : NonUpdatableAttribute
	{
		public IdentityAttribute() : base(true, true, true)
		{
		}
	}
}
