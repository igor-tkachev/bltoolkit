using System;

namespace BLToolkit.Aspects
{
	public class LogAttribute : InterceptorAttribute
	{
		public LogAttribute()
			: this(typeof(LoggingAspect), null)
		{
		}

		public LogAttribute(string parameters)
			: this(typeof(LoggingAspect), parameters)
		{
		}

		protected LogAttribute(Type interceptorType, string parameters)
			: base(interceptorType, InterceptType.OnCatch | InterceptType.OnFinally, parameters)
		{
		}
	}
}
