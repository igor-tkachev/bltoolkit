using System;

namespace BLToolkit.TypeBuilder
{
	public class AbstractPropertyBuilder : AbstractTypeBuilderBase
	{
		public override bool IsApplied(BuildContext context)
		{
			return context.IsAbstractProperty && context.Step == BuildStep.Build;
		}

		protected override void BuildAbstractGetter(BuildContext context)
		{
		}

		protected override void BuildAbstractSetter(BuildContext context)
		{
		}
	}
}
