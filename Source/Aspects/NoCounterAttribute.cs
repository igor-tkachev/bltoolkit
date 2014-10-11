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
	public class NoCounterAttribute : NoInterceptionAttribute
	{
		public NoCounterAttribute()
			: base(typeof(CounterAspect), InterceptType.BeforeCall | InterceptType.OnFinally)
		{
		}
	}
}
