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
			: base(
				interceptorType,
				InterceptType.OnCatch | InterceptType.OnFinally,
				parameters,
				TypeBuilderConsts.Priority.LoggingAspect)
		{
		}
	}
}
