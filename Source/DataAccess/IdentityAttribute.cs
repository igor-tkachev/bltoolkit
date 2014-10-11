using System;

namespace BLToolkit.DataAccess
{
	[Serializable]
	[AttributeUsage(
		AttributeTargets.Field | AttributeTargets.Property |
		AttributeTargets.Class | AttributeTargets.Interface,
		AllowMultiple = true)]
	public class IdentityAttribute : NonUpdatableAttribute
	{
		public IdentityAttribute() : base(true, true, true)
		{
		}
	}
}
