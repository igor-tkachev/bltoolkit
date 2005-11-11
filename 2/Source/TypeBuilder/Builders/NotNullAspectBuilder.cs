using System;
using System.Reflection;
using System.Reflection.Emit;

using BLToolkit.Reflection.Emit;

namespace BLToolkit.TypeBuilder.Builders
{
	public class NotNullAspectBuilder : AbstractTypeBuilderBase
	{
		public NotNullAspectBuilder(string message)
		{
			_message = message;
		}

		private string _message;

		public override int GetPriority(BuildContext context)
		{
			return TypeBuilderConsts.NotNullAspectPriority;
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
				EmitHelper   emit  = context.MethodBuilder.Emitter;
				Label        label = emit.DefineLabel();
				LocalBuilder lb    = emit.DeclareLocal(typeof(object));

				string message = _message != null? string.Format(_message, pi.Name): string.Empty;

				emit
					.ldarg(pi)
					.brtrue_s(label)
					;

				if (message.Length == 0)
				{
					emit
						.ldstr(pi.Name)
						.newobj(typeof(ArgumentNullException), typeof(string))
						;
				}
				else
				{
					emit
						.ldnull
						.ldstr(message)
						.newobj(typeof(ArgumentNullException), typeof(string), typeof(string))
						;
				}

				emit
					.@throw
					.MarkLabel(label)
					;
			}
		}
	}
}
