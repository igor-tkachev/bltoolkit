using System;
using BLToolkit.Properties;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder.Builders;

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
	public class CacheAttribute : InterceptorAttribute
	{
		#region Constructors

		public CacheAttribute()
			: this(typeof(CacheAspect), null)
		{
		}

		public CacheAttribute(Type cacheAspectType, string configString)
			: base(
				cacheAspectType,
				InterceptType.BeforeCall | InterceptType.AfterCall,
				configString,
				TypeBuilderConsts.Priority.CacheAspect)
		{
			if (!TypeHelper.IsSameOrParent(typeof(CacheAspect), cacheAspectType))
				throw new ArgumentException(Resources.CacheAttribute_ParentTypeConstraintViolated);
		}

		public CacheAttribute(Type interceptorType)
			: this(interceptorType, null)
		{
		}

		public CacheAttribute(Type interceptorType, int maxCacheTime)
			: this(interceptorType, null)
		{
			MaxCacheTime = maxCacheTime;
		}

		public CacheAttribute(Type interceptorType, bool isWeak)
			: this(interceptorType, null)
		{
			IsWeak = isWeak;
		}

		public CacheAttribute(Type interceptorType, int maxCacheTime, bool isWeak)
			: this(interceptorType, null)
		{
			MaxCacheTime = maxCacheTime;
			IsWeak       = isWeak;
		}

		public CacheAttribute(string configString)
			: this(typeof(CacheAspect), configString)
		{
		}

		public CacheAttribute(int maxCacheTime)
			: this(typeof(CacheAspect), maxCacheTime)
		{
		}

		public CacheAttribute(bool isWeak)
			: this(typeof(CacheAspect), isWeak)
		{
		}

		public CacheAttribute(int maxCacheTime, bool isWeak)
			: this(typeof(CacheAspect), maxCacheTime, isWeak)
		{
		}

		#endregion

		#region Properties

		private bool _hasMaxCacheTime;
		private int  _maxCacheTime;
		public  int   MaxCacheTime
		{
			get { return _maxCacheTime; }
			set { _maxCacheTime = value; _hasMaxCacheTime = true; }
		}

		public  int   MaxSeconds
		{
			get { return MaxCacheTime / 1000;  }
			set { MaxCacheTime = value * 1000; }
		}

		public  int   MaxMinutes
		{
			get { return MaxCacheTime / 60 / 1000;  }
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

				if (!string.IsNullOrEmpty(s) && s[0] == ';')
					s = s.Substring(1);

				return s;
			}
		}

		#endregion
	}
}
