using System;
using System.Reflection;
using System.Reflection.Emit;

using BLToolkit.Reflection.Emit;

namespace BLToolkit.TypeBuilder.Builders
{
	class NotNullAspectBuilder : AbstractTypeBuilderBase
	{
		public NotNullAspectBuilder()
		{
		}

		public override int GetPriority(BuildContext context)
		{
			return TypeBuilderPriority.NotNullAspect;
		}

		public override bool IsApplied(BuildContext context)
		{
			return context.IsBeforeStep && context.BuildElement != BuildElement.Type;
		}

		public override void Build(BuildContext context)
		{
			ParameterInfo pi = (ParameterInfo)TargetElement;

			if (pi.ParameterType.IsValueType == false)
			{
				EmitHelper emit  = context.MethodBuilder.Emitter;
				Label      label = emit.DefineLabel();
				LocalBuilder lb = emit.DeclareLocal(typeof(object));

				emit
					.ldarg     (pi)
					.brtrue_s  (label)
					.ldstr     (pi.Name)
					.newobj    (typeof(ArgumentNullException), typeof(string))
					.@throw
					.MarkLabel (label)
					;
			}
		}
	}
}
