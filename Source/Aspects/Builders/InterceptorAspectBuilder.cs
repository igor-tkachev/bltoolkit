using System;
using System.Reflection;
using System.Reflection.Emit;

namespace BLToolkit.Aspects.Builders
{
	using Reflection;
	using TypeBuilder.Builders;

	public class InterceptorAspectBuilder : AbstractTypeBuilderBase
	{
		public InterceptorAspectBuilder(
			Type interceptorType, InterceptType interceptType, string configString, int priority, bool localInterceptor)
		{
			_interceptorType  = interceptorType;
			_interceptType    = interceptType;
			_configString     = configString;
			_priority         = priority;
			_localInterceptor = localInterceptor;
		}

		private readonly Type          _interceptorType;
		private readonly InterceptType _interceptType;
		private readonly string        _configString;
		private readonly int           _priority;
		private readonly bool          _localInterceptor;

		private          FieldBuilder  _interceptorField;
		private          LocalBuilder  _infoField;

		public override int GetPriority(BuildContext context)
		{
			return _priority;
		}

		public override bool IsApplied(BuildContext context, AbstractTypeBuilderList builders)
		{
			if (_interceptorType == null && _interceptType == 0)
				return false;

			foreach (var builder in builders)
			{
				var interceptor = builder as InterceptorAspectBuilder;

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
			var builder = typeBuilder as InterceptorAspectBuilder;

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

			var emit = Context.MethodBuilder.Emitter;

			// Push ref & out parameters.
			//
			var parameters = Context.CurrentMethod.GetParameters();

			for (var i = 0; i < parameters.Length; i++)
			{
				var p = parameters[i];

				if (!p.ParameterType.IsByRef)
					continue;

				emit
					.ldloc      (_infoField)
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
					.ldloc          (_infoField)
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
					.ldloc(_infoField)
					.ldloc(Context.Exception)
					.callvirt(typeof(InterceptCallInfo).GetProperty("Exception").GetSetMethod())
					;
			}

			// Set intercept result.
			//
			emit
				.ldloc    (_infoField)
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
				.ldloc    (_infoField)
				.ldc_i4   ((int)interceptType)
				.callvirt (typeof(InterceptCallInfo).GetProperty("InterceptType").GetSetMethod())

			// Call interceptor.
			//
				.LoadField(_interceptorField)
				.ldloc    (_infoField)
				.callvirt (typeof(IInterceptor), "Intercept", typeof(InterceptCallInfo))
				;

			// Pop return value.
			//
			if (Context.ReturnValue != null)
			{
				emit
					.ldloc          (_infoField)
					.callvirt       (typeof(InterceptCallInfo).GetProperty("ReturnValue").GetGetMethod())
					.CastFromObject (Context.CurrentMethod.ReturnType)
					.stloc          (Context.ReturnValue)
					;
			}

			// Pop ref & out parameters.
			//
			for (var i = 0; i < parameters.Length; i++)
			{
				var p = parameters[i];

				if (!p.ParameterType.IsByRef)
					continue;

				var type = p.ParameterType.GetElementType();

				emit
					.ldarg          (p)
					.ldloc          (_infoField)
					.callvirt       (typeof(InterceptCallInfo).GetProperty("ParameterValues").GetGetMethod())
					.ldc_i4         (i)
					.ldelem_ref
					.CastFromObject (type)
					.stind          (type)
					;
			}

			// Check InterceptResult
			emit
				.ldloc    (_infoField)
				.callvirt (typeof(InterceptCallInfo).GetProperty("InterceptResult").GetGetMethod())
				.ldc_i4   ((int)InterceptResult.Return)
				.beq      (Context.ReturnLabel)
				;
		}

		private static int _methodCounter;

		private LocalBuilder GetInfoField()
		{
			var field = Context.GetItem<LocalBuilder>("$BLToolkit.InfoField");

			if (field == null)
			{
				_methodCounter++;

				// Create MethodInfo field.
				//
				var methodInfo = Context.CreatePrivateStaticField(
					"_methodInfo$" + Context.CurrentMethod.Name + _methodCounter, typeof(CallMethodInfo));

				var emit = Context.MethodBuilder.Emitter;

				var checkMethodInfo = emit.DefineLabel();

				emit
					.LoadField (methodInfo)
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
					.LoadField(methodInfo)
					.callvirt (typeof(InterceptCallInfo).GetProperty("CallMethodInfo").GetSetMethod())
					;

				var parameters = Context.CurrentMethod.GetParameters();

				for (var i = 0; i < parameters.Length; i++)
				{
					var p = parameters[i];

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

				Context.Items.Add("$BLToolkit.MethodInfo", methodInfo);
				Context.Items.Add("$BLToolkit.InfoField",  field);
			}

			return field;
		}

		private FieldBuilder GetInterceptorField()
		{
			var fieldName = "_interceptor$" + _interceptorType.FullName + "$_" + Context.CurrentMethod.Name + _methodCounter;
			var field     = Context.GetField(fieldName);

			if (field == null)
			{
				// Create MethodInfo field.
				//
				field = _localInterceptor? 
					Context.CreatePrivateField      (fieldName, typeof(IInterceptor)):
					Context.CreatePrivateStaticField(fieldName, typeof(IInterceptor));

				var emit = Context.MethodBuilder.Emitter;

				var checkInterceptor = emit.DefineLabel();
				var methodInfo       = Context.GetItem<FieldBuilder>("$BLToolkit.MethodInfo");

				emit
					.LoadField (field)
					.brtrue_s  (checkInterceptor)
					;

					if (!field.IsStatic)
						emit.ldarg_0.end();

				emit
					.newobj    (TypeHelper.GetDefaultConstructor(_interceptorType))
					.castclass (typeof(IInterceptor))
					;

				if (field.IsStatic)
					emit.stsfld(field);
				else
					emit.stfld(field);

				emit
					.LoadField (field)
					.LoadField (methodInfo)
					.ldstrEx   (_configString ?? "")
					.callvirt  (typeof(IInterceptor), "Init", typeof(CallMethodInfo), typeof(string))

					.MarkLabel (checkInterceptor)
					;
			}

			return field;
		}

		protected override void BeginMethodBuild()
		{
			_infoField        = GetInfoField();
			_interceptorField = GetInterceptorField();
		}

		protected override void EndMethodBuild()
		{
			Context.Items.Remove("$BLToolkit.MethodInfo");
			Context.Items.Remove("$BLToolkit.InfoField");
		}
	}
}
