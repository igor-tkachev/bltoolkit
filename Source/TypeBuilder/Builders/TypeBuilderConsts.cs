using System;

namespace BLToolkit.TypeBuilder.Builders
{
	public sealed class TypeBuilderConsts
	{
		public const int    NormalBuilderPriority = 0;
		public const int    NotNullAspectPriority = int.MaxValue / 2;
		public const string AssemblyNameSuffix    = "TypeBuilder";

		private TypeBuilderConsts()
		{
		}
	}
}
