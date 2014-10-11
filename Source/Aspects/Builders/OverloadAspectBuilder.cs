using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using BLToolkit.Properties;
using BLToolkit.Reflection;
using BLToolkit.Reflection.Emit;
using BLToolkit.TypeBuilder;
using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Aspects.Builders
{
	public class OverloadAspectBuilder: AbstractTypeBuilderBase
	{
		private readonly string _overloadedMethodName;
		private readonly Type[] _parameterTypes;

		public OverloadAspectBuilder(string overloadedMethodName, Type[] parameterTypes)
		{
			_overloadedMethodName = overloadedMethodName;
			_parameterTypes       = parameterTypes;
		}

		public override int GetPriority(BuildContext context)
		{
			return TypeBuilderConsts.Priority.OverloadAspect;
		}

		public override bool IsCompatible(BuildContext context, IAbstractTypeBuilder typeBuilder)
		{
			if (context.IsBuildStep)
				return false;

			AbstractTypeBuilderList list = new AbstractTypeBuilderList(2);

			list.Add(this);
			list.Add(typeBuilder);

			BuildStep step = context.Step;

			try
			{
				context.Step = BuildStep.Build;

				return typeBuilder.IsApplied(context, list);
			}
			finally
			{
				context.Step = step;
			}
		}

		public override bool IsApplied(BuildContext context, AbstractTypeBuilderList builders)
		{
			if (context == null) throw new ArgumentNullException("context");

			return context.IsBuildStep && context.BuildElement == BuildElement.AbstractMethod;
		}

		protected override void BuildAbstractMethod()
		{
			MethodInfo    currentMethod = Context.CurrentMethod;
			string           methodName = _overloadedMethodName ?? currentMethod.Name;
			MethodInfo overloadedMethod = GetOverloadedMethod(methodName);

			if (overloadedMethod == null)
			{
				throw new TypeBuilderException(string.Format(
					Resources.OverloadAspectBuilder_NoOverloadedMethod,
						Context.Type.FullName, methodName));
			}

			EmitHelper                emit = Context.MethodBuilder.Emitter;
			List<ParameterInfo> parameters = new List<ParameterInfo>(currentMethod.GetParameters());

			if (!overloadedMethod.IsStatic)
				emit.ldarg_0.end();

			foreach (ParameterInfo param in overloadedMethod.GetParameters())
			{
				ParameterInfo currentMethodParameter = null;
				foreach (ParameterInfo p in parameters)
				{
					if (p.Name != param.Name)
						continue;

					currentMethodParameter = p;
					parameters.Remove(p);
					break;
				}

				if (currentMethodParameter != null)
				{
					emit.ldarg(currentMethodParameter);
				}
				else
				{
					Type type  = param.ParameterType;
					bool isRef = false;

					if (type.IsByRef)
					{
						type  = type.GetElementType();
						isRef = true;
					}

					if (type.IsValueType && !type.IsPrimitive)
					{
						LocalBuilder localBuilder = emit.DeclareLocal(type);

						emit
							.ldloca      (localBuilder)
							.initobj     (type)
							;

						if (isRef)
							emit.ldloca  (localBuilder);
						else
							emit.ldloc   (localBuilder);

					}
					else
					{
						if ((param.Attributes & ParameterAttributes.HasDefault) == 0 ||
							!emit.LoadWellKnownValue(param.DefaultValue))
						{
							emit.LoadInitValue(type);
						}

						if (isRef)
						{
							LocalBuilder localBuilder = emit.DeclareLocal(type);

							emit
								.stloc   (localBuilder)
								.ldloca  (localBuilder)
								;
						}
					}
				}
			}

			// Finally, call the method we override.
			//
			if (overloadedMethod.IsStatic || overloadedMethod.IsFinal)
				emit.call      (overloadedMethod);
			else
				emit.callvirt  (overloadedMethod);

			if (currentMethod.ReturnType != typeof(void))
				emit.stloc(Context.ReturnValue);
		}

		private MethodInfo GetOverloadedMethod(string methodName)
		{
			MethodInfo      currentMethod            = Context.CurrentMethod;
			MethodInfo      bestMatch                = null;
			int             bestMatchParametersCount = -1;
			ParameterInfo[] currentMethodParameters  = currentMethod.GetParameters();

			if (_parameterTypes != null)
			{
				bestMatch = Context.Type.GetMethod(methodName, _parameterTypes);
				return bestMatch != null && MatchParameters(currentMethodParameters, bestMatch.GetParameters()) >= 0? bestMatch: null;
			}

			const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Instance| BindingFlags.Public | BindingFlags.NonPublic;

			foreach (MethodInfo m in Context.Type.GetMethods(bindingFlags))
			{
				if (m.IsPrivate || m.Name != methodName || m.IsGenericMethod != currentMethod.IsGenericMethod)
					continue;

				if (!TypeHelper.CompareParameterTypes(m.ReturnType, currentMethod.ReturnType))
					continue;

				if (m.IsDefined(typeof(OverloadAttribute), true))
					continue;

				ParameterInfo[] overloadedMethodParameters = m.GetParameters();
				if (overloadedMethodParameters.Length <= bestMatchParametersCount)
					continue;

				int matchedParameters = MatchParameters(overloadedMethodParameters, currentMethodParameters);
				if (matchedParameters <= bestMatchParametersCount)
					continue;

				bestMatchParametersCount = matchedParameters;
				bestMatch                = m;
			}

			return bestMatch;
		}

		private static int MatchParameters(ParameterInfo[] parametersToMatch, ParameterInfo[] existingParameters)
		{
			int matchedParameters = 0;
			List<ParameterInfo> existingParametersList = new List<ParameterInfo>(existingParameters);
			foreach (ParameterInfo param in parametersToMatch)
			{
				foreach (ParameterInfo existing in existingParametersList)
				{
					if (existing.Name != param.Name)
						continue;

					if (!TypeHelper.CompareParameterTypes(param.ParameterType, existing.ParameterType))
						return -1;

					++matchedParameters;
					existingParametersList.Remove(existing);
					break;
				}
			}

			return matchedParameters;
		}
	}
}