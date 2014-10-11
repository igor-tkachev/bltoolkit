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
	public class NoLogAttribute : NoInterceptionAttribute
	{
		public NoLogAttribute()
			: base(typeof(LoggingAspect), InterceptType.OnCatch | InterceptType.OnFinally)
		{
		}
	}
}
