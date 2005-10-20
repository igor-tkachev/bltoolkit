using System;
using System.Reflection;
using System.Reflection.Emit;

namespace BLToolkit.TypeBuilder
{
	public class DefaultTypeBuilder : AbstractTypeBuilderBase
	{
		public override bool IsApplied(BuildContext context)
		{
			if (context.IsAbstractProperty && context.Step == BuildStep.Build)
			{
				return context.CurrentProperty.GetIndexParameters().Length == 0;
			}

			return false;
		}

		private FieldBuilder GetField(BuildContext context)
		{
			PropertyInfo propertyInfo = context.CurrentProperty;

			return context.GetField(
				"_" + propertyInfo.Name,
				propertyInfo.PropertyType,
				FieldAttributes.Private);
		}

		protected override void BuildAbstractGetter(BuildContext context)
		{
			FieldBuilder field = GetField(context);

			context.MethodBuilder.Emitter
				.ldarg_0
				.ldfld(field)
				.stloc(context.ReturnValue);
		}

		protected override void BuildAbstractSetter(BuildContext context)
		{
			FieldBuilder field = GetField(context);

			context.MethodBuilder.Emitter
				.ldarg_0
				.ldarg_1
				.stfld(field);
		}

		protected override void BuildVirtualGetter(BuildContext context)
		{
		}

		protected override void BuildVirtualSetter(BuildContext context)
		{
		}

		protected override void BuildVirtualMethod(BuildContext context)
		{
		}
	}
}
