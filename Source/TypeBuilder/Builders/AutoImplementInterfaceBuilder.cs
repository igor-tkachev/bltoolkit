using System;

using BLToolkit.Reflection;

namespace BLToolkit.TypeBuilder.Builders
{
	class AutoImplementInterfaceBuilder : DefaultTypeBuilder
	{
		public override Type[] GetInterfaces()
		{
			return new Type[] { (TypeHelper)TargetElement };
		}

		public override bool IsApplied(BuildContext context, AbstractTypeBuilderList builders)
		{
			return
				(context.IsAfterStep || context.IsEndStep) &&
				(context.IsAbstractProperty || context.IsAbstractMethod);
		}

		protected override void EndMethodBuild()
		{
			
		}
	}
}
