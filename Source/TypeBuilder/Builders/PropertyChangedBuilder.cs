using System;
using System.Reflection;
using System.Reflection.Emit;

using BLToolkit.Reflection.Emit;

namespace BLToolkit.TypeBuilder.Builders
{
	public class PropertyChangedBuilder : AbstractTypeBuilderBase
	{
		public override bool IsApplied(BuildContext context, AbstractTypeBuilderList builders)
		{
			if (context == null) throw new ArgumentNullException("context");

			return context.IsAfterStep && context.IsSetter;
		}

		protected override void AfterBuildAbstractSetter()
		{
			BuildSetter();
		}

		protected override void AfterBuildVirtualSetter()
		{
			BuildSetter();
		}

		private void BuildSetter()
		{
			InterfaceMapping im   = Context.Type.GetInterfaceMap(typeof(IPropertyChanged));
			MethodInfo       mi   = im.TargetMethods[0];
			FieldBuilder     ifb  = GetPropertyInfoField();
			EmitHelper       emit = Context.MethodBuilder.Emitter;

			if (mi.IsPublic)
			{
				emit
					.ldarg_0
					.ldsfld   (ifb)
					.callvirt (mi)
					;
			}
			else
			{
				emit
					.ldarg_0
					.castclass (typeof(IPropertyChanged))
					.ldsfld    (ifb)
					.callvirt  (im.InterfaceMethods[0])
					;
			}
		}
	}
}
