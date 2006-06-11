using System;

namespace BLToolkit.Aspects
{
	[AttributeUsage(
		AttributeTargets.Class |
		AttributeTargets.Interface |
		AttributeTargets.Property |
		AttributeTargets.Method,
		AllowMultiple=true)]
	public class NoLogAttribute : NoInterceptionAttribute
	{
		public NoLogAttribute()
			: base(typeof(LoggingAspect), InterceptType.OnCatch | InterceptType.OnFinally)
		{
		}
	}
}
