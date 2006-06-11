using System;

namespace BLToolkit.Aspects
{
	[AttributeUsage(
		AttributeTargets.Class |
		AttributeTargets.Interface |
		AttributeTargets.Property |
		AttributeTargets.Method,
		AllowMultiple=true)]
	public class NoCounterAttribute : NoInterceptionAttribute
	{
		public NoCounterAttribute()
			: base(typeof(CounterAspect), InterceptType.BeforeCall | InterceptType.OnFinally)
		{
		}
	}
}
