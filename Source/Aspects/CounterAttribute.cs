using System;

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
	public class CounterAttribute : InterceptorAttribute
	{
		public CounterAttribute()
			: this(typeof(CounterAspect), null)
		{
		}

		public CounterAttribute(string configString)
			: this(typeof(CounterAspect), configString)
		{
		}

		protected CounterAttribute(Type interceptorType, string configString)
			: base(
				interceptorType,
				InterceptType.BeforeCall | InterceptType.OnCatch | InterceptType.OnFinally,
				configString,
				TypeBuilderConsts.Priority.Normal)
		{
		}
	}
}
