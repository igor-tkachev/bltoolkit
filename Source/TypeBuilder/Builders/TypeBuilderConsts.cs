using System;

namespace BLToolkit.TypeBuilder.Builders
{
	public sealed class TypeBuilderConsts
	{
		public sealed class Priority
		{
			private Priority()
			{
			}

			public const int Normal        = 0;
			public const int NotNullAspect = int.MaxValue / 2;
			public const int LoggingAspect = Normal;
			public const int DataAccessor  = int.MinValue / 2;
			public const int CacheAspect   = int.MinValue / 2;
			public const int PropChange    = int.MinValue + 1000000;
		}

		public const string AssemblyNameSuffix = "TypeBuilder";

		private TypeBuilderConsts()
		{
		}
	}
}
