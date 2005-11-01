using System;

namespace BLToolkit.TypeBuilder.Builders
{
	public sealed class TypeBuilderPriority
	{
		public const int Normal        = 0;
		public const int NotNullAspect = int.MaxValue / 2;

		private TypeBuilderPriority()
		{
		}
	}
}
