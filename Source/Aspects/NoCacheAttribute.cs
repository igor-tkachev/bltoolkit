using System;

namespace BLToolkit.Aspects
{
	/// <summary>
	/// http://www.bltoolkit.net/Doc/Aspects/index.htm
	/// </summary>
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
