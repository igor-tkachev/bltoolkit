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
	public class InstanceCacheAttribute : CacheAttribute
	{
		#region Constructors

		public InstanceCacheAttribute()
			: this(typeof(CacheAspect), null)
		{
		}

		public InstanceCacheAttribute(Type cacheAspectType, string configString)
			: base(cacheAspectType, configString)
		{
		}

		public InstanceCacheAttribute(Type interceptorType)
			: this(interceptorType, null)
		{
		}

		public InstanceCacheAttribute(Type interceptorType, int maxCacheTime)
			: this(interceptorType, null)
		{
			MaxCacheTime = maxCacheTime;
		}

		public InstanceCacheAttribute(Type interceptorType, bool isWeak)
			: this(interceptorType, null)
		{
			IsWeak = isWeak;
		}

		public InstanceCacheAttribute(Type interceptorType, int maxCacheTime, bool isWeak)
			: this(interceptorType, null)
		{
			MaxCacheTime = maxCacheTime;
			IsWeak       = isWeak;
		}

		public InstanceCacheAttribute(string configString)
			: this(typeof(CacheAspect), configString)
		{
		}

		public InstanceCacheAttribute(int maxCacheTime)
			: this(typeof(CacheAspect), maxCacheTime)
		{
		}

		public InstanceCacheAttribute(bool isWeak)
			: this(typeof(CacheAspect), isWeak)
		{
		}

		public InstanceCacheAttribute(int maxCacheTime, bool isWeak)
			: this(typeof(CacheAspect), maxCacheTime, isWeak)
		{
		}

		#endregion

		public override bool LocalInterceptor
		{
			get { return true; }
		}
	}
}
