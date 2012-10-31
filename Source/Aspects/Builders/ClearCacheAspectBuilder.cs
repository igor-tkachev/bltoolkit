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

		private readonly Type   _declaringType;
		private readonly string _methodName;
		private readonly Type[] _parameterTypes;

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

			if (string.IsNullOrEmpty(_methodName))
			{
				FieldBuilder type = Context.CreatePrivateStaticField(
					"_type$ClearCacheAspect$" + ++_methodCounter, typeof(Type));

				EmitHelper emit = Context.MethodBuilder.Emitter;
				Label checkType = emit.DefineLabel();

				emit
					.ldsfld    (type)
					.brtrue_s  (checkType)
					.ldarg_0
					.LoadType  (_declaringType)
					.call      (typeof(ClearCacheAspect), "GetType", typeof(object), typeof(Type))
					.stsfld    (type)
					.MarkLabel (checkType)
					.ldsfld    (type)
					.call      (typeof(CacheAspect), "ClearCache", typeof(Type))
					;
			}
			else
			{
				FieldBuilder methodInfo = Context.CreatePrivateStaticField(
					"_methodInfo$ClearCacheAspect$" + ++_methodCounter, typeof(MethodInfo));

				EmitHelper emit = Context.MethodBuilder.Emitter;

				Label checkMethodInfo = emit.DefineLabel();

				emit
					.ldsfld   (methodInfo)
					.brtrue_s (checkMethodInfo)
					.ldarg_0
					.LoadType (_declaringType)
					.ldstrEx  (_methodName)
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
		}

		#endregion
	}
}
