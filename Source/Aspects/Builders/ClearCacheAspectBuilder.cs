using System;
using System.Reflection;
using System.Reflection.Emit;

using BLToolkit.Reflection.Emit;
using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Aspects.Builders
{
	public class ClearCacheAspectBuilder : AbstractTypeBuilderBase
	{
		#region Init

		public ClearCacheAspectBuilder(Type declaringType, string methodName, Type[] parameterTypes)
		{
			_declaringType  = declaringType;
			_methodName     = methodName;
			_parameterTypes = parameterTypes;
		}

		Type   _declaringType;
		string _methodName;
		Type[] _parameterTypes;

		#endregion

		#region Overrides

		public override int GetPriority(BuildContext context)
		{
			return TypeBuilderConsts.Priority.ClearCacheAspect;
		}

		public override bool IsCompatible(BuildContext context, IAbstractTypeBuilder typeBuilder)
		{
			return true;
		}

		public override bool IsApplied(BuildContext context, AbstractTypeBuilderList builders)
		{
			if (context == null) throw new ArgumentNullException("context");

			return context.IsFinallyStep && context.IsMethodOrProperty;
		}

		#endregion

		#region Build

		private static int _methodCounter;

		public override void Build(BuildContext context)
		{
			Context = context;

			FieldBuilder methodInfo = Context.CreatePrivateStaticField(
				"_methodInfo$ClearCacheAspect$" + ++_methodCounter, typeof(MethodInfo));

			EmitHelper emit = Context.MethodBuilder.Emitter;

			Label checkMethodInfo = emit.DefineLabel();

			emit
				.ldsfld   (methodInfo)
				.brtrue_s (checkMethodInfo)
				.ldarg_0
				.LoadType (_declaringType)
				.ldstr    (_methodName)
				;

			if (_parameterTypes == null || _parameterTypes.Length == 0)
			{
				emit.ldnull.end();
			}
			else
			{
				LocalBuilder field = emit.DeclareLocal(typeof(Type[]));

				emit
					.ldc_i4_ (_parameterTypes.Length)
					.newarr  (typeof(Type))
					.stloc   (field)
					;

				for (int i = 0; i < _parameterTypes.Length; i++)
				{
					emit
						.ldloc      (field)
						.ldc_i4     (i)
						.LoadType   (_parameterTypes[i])
						.stelem_ref
						.end()
						;
				}

				emit.ldloc(field);
			}

			emit
				.call      (typeof(ClearCacheAspect), "GetMethodInfo", typeof(object), typeof(Type), typeof(string), typeof(Type[]))
				.stsfld    (methodInfo)
				.MarkLabel (checkMethodInfo)
				.ldsfld    (methodInfo)
				.call      (typeof(CacheAspect), "ClearCache", typeof(MethodInfo))
				;
		}

		#endregion
	}
}
