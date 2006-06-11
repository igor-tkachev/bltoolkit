using System;

namespace BLToolkit.Aspects
{
	[AttributeUsage(
		AttributeTargets.Class |
		AttributeTargets.Interface |
		AttributeTargets.Property |
		AttributeTargets.Method,
		AllowMultiple=true)]
	public class NoCacheAttribute : NoInterceptionAttribute
	{
		public NoCacheAttribute()
			: base(typeof(CacheAspect), InterceptType.BeforeCall | InterceptType.AfterCall)
		{
		}
	}
}
