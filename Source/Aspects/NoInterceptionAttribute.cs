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
	public class NoInterceptionAttribute : InterceptorAttribute
	{
		public NoInterceptionAttribute()
			: base(null, 0)
		{
		}

		public NoInterceptionAttribute(Type interceptorType, InterceptType interceptType)
			: base(interceptorType, interceptType)
		{
		}

		public override IAbstractTypeBuilder TypeBuilder
		{
			get { return new NoInterceptionAspectBuilder(InterceptorType, InterceptType); }
		}

		private class NoInterceptionAspectBuilder : Builders.InterceptorAspectBuilder
		{
			public NoInterceptionAspectBuilder(Type interceptorType, InterceptType interceptType)
				: base(interceptorType, interceptType, null, TypeBuilderConsts.Priority.Normal, false)
			{
			}

			public override void Build(BuildContext context)
			{
				if (context.Step == BuildStep.Begin || context.Step == BuildStep.End)
					base.Build(context);
			}
		}
	}
}
