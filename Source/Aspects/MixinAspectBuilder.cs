using System;
using System.Reflection;

using BLToolkit.TypeBuilder.Builders;
using BLToolkit.Reflection.Emit;
using BLToolkit.TypeBuilder;

namespace BLToolkit.Aspects
{
	public class MixinAspectBuilder : AbstractTypeBuilderBase
	{
		public MixinAspectBuilder(Type targetInterface, string memberName)
		{
			_targetInterface = targetInterface;
			_memberName      = memberName;
		}

		private Type   _targetInterface;
		private string _memberName;

		public override bool IsApplied(BuildContext context)
		{
			return context.BuildElement == BuildElement.InterfaceMethod;
		}

		public override Type[] GetInterfaces()
		{
			return new Type[] { _targetInterface };
		}

		public override void Build(BuildContext context)
		{
			Context = context;

			EmitHelper      emit   = Context.MethodBuilder.Emitter;
			MethodInfo      method = Context.MethodBuilder.OverriddenMethod;
			ParameterInfo[] ps     = method.GetParameters();

			FieldInfo field = Context.Type.GetField(_memberName);

			if (field != null)
			{
				if (field.IsPrivate)
					throw new TypeBuilderException(string.Format(
						"Field '{0}.{1}' must be protected or public.",
						Context.Type.Name, _memberName));

				emit
					.ldarg_0
					.ldfld   (field)
					.brfalse (Context.ReturnLabel)
					.ldarg_0
					.ldfld   (field)
					;
			}
			else
			{
				PropertyInfo prop = Context.Type.GetProperty(_memberName);

				if (prop != null)
				{
					MethodInfo mi = prop.GetGetMethod(true);

					if (mi == null)
						throw new TypeBuilderException(string.Format(
							"Property '{0}.{1}' getter not found.",
							Context.Type.Name, _memberName));

					if (mi.IsPrivate)
						throw new TypeBuilderException(string.Format(
							"Property '{0}.{1}' getter must be protected or public.",
							Context.Type.Name, _memberName));

					emit
						.ldarg_0
						.callvirt (mi)
						.brfalse  (Context.ReturnLabel)
						.ldarg_0
						.callvirt (mi)
						;
				}
				else
				{
					throw new TypeBuilderException(string.Format(
						"Member '{0}.{1}' not found.",
						Context.Type.Name, _memberName));
				}
			}

			for (int i = 0; i < ps.Length; i++)
				emit.ldarg(i + 1);

			emit.callvirt(method);

			if (Context.ReturnValue != null)
				emit.stloc(Context.ReturnValue);
		}
	}
}
