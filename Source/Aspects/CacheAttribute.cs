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

		public CacheAttribute(string configString)
			: this(typeof(CacheAspect), configString)
		{
		}

		public CacheAttribute(int maxCacheTime)
			: this(typeof(CacheAspect), null)
		{
			MaxCacheTime = maxCacheTime;
		}

		public CacheAttribute(bool isWeak)
			: this(typeof(CacheAspect), null)
		{
			IsWeak = isWeak;
		}

		public CacheAttribute(int maxCacheTime, bool isWeak)
			: this(typeof(CacheAspect), null)
		{
			MaxCacheTime = maxCacheTime;
			IsWeak       = isWeak;
		}

		protected CacheAttribute(Type interceptorType, string configString)
			: base(
				interceptorType,
				InterceptType.BeforeCall | InterceptType.AfterCall,
				configString,
				TypeBuilderConsts.Priority.CacheAspect)
		{
		}

		private bool _hasMaxCacheTime;
		private int  _maxCacheTime;
		public  int   MaxCacheTime
		{
			get { return _maxCacheTime; }
			set { _maxCacheTime = value; _hasMaxCacheTime = true; }
		}

		public  int   MaxSeconds
		{
			get { return MaxCacheTime / 1000; }
			set { MaxCacheTime = value * 1000; }
		}

		public  int   MaxMinutes
		{
			get { return MaxCacheTime / 60 / 1000; }
			set { MaxCacheTime = value * 60 * 1000; }
		}

		private bool _hasIsWeak;
		private bool _isWeak;
		public  bool  IsWeak
		{
			get { return _isWeak; }
			set { _isWeak = value; _hasIsWeak = true; }
		}

		public override string ConfigString
		{
			get
			{
				string s = base.ConfigString;

				if (_hasMaxCacheTime) s += ";MaxCacheTime=" + MaxCacheTime;
				if (_hasIsWeak)       s += ";IsWeak="       + IsWeak;

				if (s != null && s.Length > 0 && s[0] == ';')
					s = s.Substring(1);

				return s;
			}
		}
	}
}
