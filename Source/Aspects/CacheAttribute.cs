using System;

using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Aspects
{
	[AttributeUsage(
		AttributeTargets.Class |
		AttributeTargets.Interface |
		AttributeTargets.Property |
		AttributeTargets.Method,
		AllowMultiple=true)]
	public class CacheAttribute : InterceptorAttribute
	{
		public CacheAttribute()
			: this(typeof(CacheAspect), null)
		{
		}

		public CacheAttribute(int maxCacheTime)
			: this(typeof(CacheAspect), "MaxCacheTime=" + maxCacheTime)
		{
		}

		public CacheAttribute(string configString)
			: this(typeof(CacheAspect), configString)
		{
		}

		protected CacheAttribute(Type interceptorType, string configString)
			: base(
				interceptorType,
				InterceptType.BeforeCall | InterceptType.AfterCall,
				configString,
				TypeBuilderConsts.Priority.CacheAspect)
		{
		}
	}
}
