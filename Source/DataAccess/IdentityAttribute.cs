using System;

namespace BLToolkit.DataAccess
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class IdentityAttribute : NonUpdatableAttribute
	{
		public IdentityAttribute() : base(true, true, true)
		{
		}
	}
}
