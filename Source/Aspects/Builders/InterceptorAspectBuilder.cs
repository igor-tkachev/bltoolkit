using System;
using System.Reflection;
using System.Reflection.Emit;

using BLToolkit.Reflection;
using BLToolkit.Reflection.Emit;
using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Aspects.Builders
{
	public class InterceptorAspectBuilder : AbstractTypeBuilderBase
	{
		public InterceptorAspectBuilder(
			Type interceptorType, InterceptType interceptType, string configString, int priority)
		{
			_interceptorType = interceptorType;
			_interceptType   = interceptType;
			_configString    = configString;
			_priority        = priority;
		}

		private readonly Type          _interceptorType;
		private readonly InterceptType _interceptType;
		private readonly string        _configString;
		private readonly int           _priority;

		public override int GetPriority(BuildContext context)
		{
			return _priority;
		}

		public override bool IsApplied(BuildContext context, AbstractTypeBuilderList builders)
		{
			if (_interceptorType == null && _interceptType == 0)
				return false;

			foreach (IAbstractTypeBuilder builder in builders)
			{
				InterceptorAspectBuilder interceptor = builder as InterceptorAspectBuilder;

				if (interceptor != null)
				{
					if (interceptor._interceptorType == null && interceptor._interceptType == 0)
						return false;

					if (builder == this)
						break;
				}
			}

			if (context.IsMethodOrProperty) switch (context.Step)
			{
				case BuildStep.Begin:   return true;
				case BuildStep.Before:  return (_interceptType & InterceptType.BeforeCall) != 0;
				case BuildStep.After:   return (_interceptType & InterceptType.AfterCall)  != 0;
				case BuildStep.Catch:   return (_interceptType & InterceptType.OnCatch)    != 0;
				case BuildStep.Finally: return (_interceptType & InterceptType.OnFinally)  != 0;
				case BuildStep.End:     return true;
			}

			return false;
		}

		public override bool IsCompatible(BuildContext context, IAbstractTypeBuilder typeBuilder)
		{
			InterceptorAspectBuilder builder = typeBuilder as InterceptorAspectBuilder;

			return builder == null || _interceptorType != builder._interceptorType;
		}

		public override void Build(BuildContext context)
		{
			if (context.Step == BuildStep.Begin || context.Step == BuildStep.End)
			{
				base.Build(context);
				return;
			}

			Context = context;

			FieldBuilder interceptor = GetInterceptorField();
			LocalBuilder info        = GetInfoField();
			EmitHelper   emit        = Context.MethodBuilder.Emitter;

			// Push ref & out parameters.
			//
			ParameterInfo[] parameters = Context.CurrentMethod.GetParameters();

			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo p = parameters[i];

				if (!p.ParameterType.IsByRef)
					continue;

				emit
					.ldloc      (info)
					.callvirt   (typeof(InterceptCallInfo).GetProperty("ParameterValues").GetGetMethod())
					.ldc_i4     (i)
					.ldargEx    (p, true)
					.stelem_ref
					.end()
					;
			}

			// Push return value.
			//
			if (Context.ReturnValue != null)
			{
				emit
					.ldloc          (info)
					.ldloc          (Context.ReturnValue)
					.boxIfValueType (Context.CurrentMethod.ReturnType)
					.callvirt       (typeof(InterceptCallInfo).GetProperty("ReturnValue").GetSetMethod())
					;
			}

			// Set Exception.
			//
			if (Context.Step == BuildStep.Catch)
			{
				emit
					.ldloc(info)
					.ldloc(Context.Exception)
					.callvirt(typeof(InterceptCallInfo).GetProperty("Exception").GetSetMethod())
					;
			}

			// Set config string.
			//
			emit
				.ldloc    (info)
				.ldstrEx  (_configString)
				.callvirt (typeof(InterceptCallInfo).GetProperty("ConfigString").GetSetMethod())
				;

			// Set interceptor ID.
			//
			emit
				.ldloc    (info)
				.ldc_i4   (ID)
				.callvirt (typeof(InterceptCallInfo).GetProperty("InterceptorID").GetSetMethod())
				;

			// Set intercept result.
			//
			emit
				.ldloc    (info)
				.ldc_i4   ((int)InterceptResult.Continue)
				.callvirt (typeof(InterceptCallInfo).GetProperty("InterceptResult").GetSetMethod())
				;

			// Set intercept type.
			//
			InterceptType interceptType;

			switch (Context.Step)
			{
				case BuildStep.Before:  interceptType = InterceptType.BeforeCall; break;
				case BuildStep.After:   interceptType = InterceptType.AfterCall;  break;
				case BuildStep.Catch:   interceptType = InterceptType.OnCatch;    break;
				case BuildStep.Finally: interceptType = InterceptType.OnFinally;  break;
				default:
					throw new InvalidOperationException();
			}

			emit
				.ldloc    (info)
				.ldc_i4   ((int)interceptType)
				.callvirt (typeof(InterceptCallInfo).GetProperty("InterceptType").GetSetMethod())

				// Call interceptor.
				//
				.ldsfld   (interceptor)
				.ldloc    (info)
				.callvirt (typeof(IInterceptor), "Intercept", typeof(InterceptCallInfo))
				;

			// Pop return value.
			//
			if (Context.ReturnValue != null)
			{
				emit
					.ldloc          (info)
					.callvirt       (typeof(InterceptCallInfo).GetProperty("ReturnValue").GetGetMethod())
					.CastFromObject (Context.CurrentMethod.ReturnType)
					.stloc          (Context.ReturnValue)
					;
			}

			// Pop ref & out parameters.
			//
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo p = parameters[i];

				if (!p.ParameterType.IsByRef)
					continue;

				Type type = p.ParameterType.GetElementType();

				emit
					.ldarg          (p)
					.ldloc          (info)
					.callvirt       (typeof(InterceptCallInfo).GetProperty("ParameterValues").GetGetMethod())
					.ldc_i4         (i)
					.ldelem_ref
					.CastFromObject (type)
					.stind          (type)
					;
			}

			// Check InterceptResult
			emit
				.ldloc    (info)
				.callvirt (typeof(InterceptCallInfo).GetProperty("InterceptResult").GetGetMethod())
				.ldc_i4   ((int)InterceptResult.Return)
				.beq      (Context.ReturnLabel)
				;
		}

		private static int _methodCounter;

		private LocalBuilder GetInfoField()
		{
			LocalBuilder field = (LocalBuilder)Context.Items["$BLToolkit.InfoField"];

			if (field == null)
			{
				// Create MethodInfo field.
				//
				FieldBuilder methodInfo = Context.CreatePrivateStaticField(
					"_methodInfo$" + Context.CurrentMethod.Name + ++_methodCounter, typeof(CallMethodInfo));

				EmitHelper emit = Context.MethodBuilder.Emitter;

				Label checkMethodInfo = emit.DefineLabel();

				emit
					.ldsfld    (methodInfo)
					.brtrue_s  (checkMethodInfo)
					.call      (typeof(MethodBase), "GetCurrentMethod")
					.castclass (typeof(MethodInfo))
					.newobj    (TypeHelper.GetConstructor(typeof(CallMethodInfo), typeof(MethodInfo)))
					.stsfld    (methodInfo)
					.MarkLabel (checkMethodInfo)
					;

				// Create & initialize the field.
				//
				field = emit.DeclareLocal(typeof(InterceptCallInfo));

				emit
					.newobj   (TypeHelper.GetDefaultConstructor(typeof(InterceptCallInfo)))
					.dup
					.ldarg_0
					.callvirt (typeof(InterceptCallInfo).GetProperty("Object").GetSetMethod())

					.dup
					.ldsfld   (methodInfo)
					.callvirt (typeof(InterceptCallInfo).GetProperty("CallMethodInfo").GetSetMethod())
					;

				ParameterInfo[] parameters = Context.CurrentMethod.GetParameters();

				for (int i = 0; i < parameters.Length; i++)
				{
					ParameterInfo p = parameters[i];

					if (p.ParameterType.IsByRef)
						continue;

					emit
						.dup
						.callvirt   (typeof(InterceptCallInfo).GetProperty("ParameterValues").GetGetMethod())
						.ldc_i4     (i)
						.ldargEx    (p, true)
						.stelem_ref
						.end()
						;
				}

				emit.stloc(field);
				Context.Items.Add("$BLToolkit.InfoField", field);
			}

			return field;
		}

		private FieldBuilder GetInterceptorField()
		{
			string       fieldName = "_interceptor$_" + _interceptorType.FullName;
			FieldBuilder field     = Context.GetField(fieldName);

			if (field == null)
			{
				field = Context.CreatePrivateStaticField(fieldName, typeof(IInterceptor));

				Context.TypeBuilder.TypeInitializer.Emitter
					.newobj    (TypeHelper.GetDefaultConstructor(_interceptorType))
					.castclass (typeof(IInterceptor))
					.stsfld    (field)
					;
			}

			return field;
		}

		protected override void BeginMethodBuild()
		{
			GetInfoField();
		}

		protected override void EndMethodBuild()
		{
			Context.Items.Remove("$BLToolkit.InfoField");
		}
	}
}
