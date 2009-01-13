using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Web.Services.Protocols;
using BLToolkit.DataAccess;
using BLToolkit.Reflection;
using BLToolkit.Reflection.Emit;
using BLToolkit.TypeBuilder.Builders;

namespace Demo.WebServices.Client.WebClient
{
	public class WebClientTypeBuilder : AbstractTypeBuilderBase
	{
		private readonly CustomAttributeBuilder _soapDocumentAttributeBuilder
			= new CustomAttributeBuilder(TypeHelper.GetDefaultConstructor(typeof(SoapDocumentMethodAttribute)), Type.EmptyTypes);

		public override bool IsApplied(BuildContext context, AbstractTypeBuilderList builders)
		{
			return context.IsBuildStep && context.IsAbstractMethod;
		}

		protected override void BuildAbstractMethod()
		{
			EmitHelper          emit             = Context.MethodBuilder.Emitter;
			List<ParameterInfo> parameters       = new List<ParameterInfo>();
			ParameterInfo       callback         = null;
			ParameterInfo       exceptionHandler = null;

			foreach (ParameterInfo p in Context.CurrentMethod.GetParameters())
			{
				Type   parameterType = p.ParameterType;
				string paramTypeName = parameterType.Name.Split('`')[0];

				if (paramTypeName == "Action")
				{
					if (parameterType.IsGenericType && parameterType.GetGenericArguments()[0] == typeof(Exception))
						exceptionHandler = p;
					else
						callback = p;
				}
				else
					parameters.Add(p);
			}

			EmitMethodName(emit);

			if (callback == null)
			{
				EmitSyncCall(parameters, emit);
			}
			else
			{
				EmitAsyncCall(parameters, emit, callback, exceptionHandler);
			}
		}

		private void EmitSyncCall(List<ParameterInfo> parameters, EmitHelper emit)
		{
			Context.MethodBuilder.MethodBuilder.SetCustomAttribute(_soapDocumentAttributeBuilder);

			bool withOutParameters = EmitParameters(emit, parameters);
			bool callGeneric = !withOutParameters && Context.CurrentMethod.ReturnType != typeof(void);

			// Use Invoke<T>() for methods with return value but
			// Invoke() for methods with no return value or with out/ref parameters.
			//
			MethodInfo invoke = TypeHelper.GetMethod(
				typeof(WebClientBase),
				callGeneric,
				"Invoke",
				BindingFlags.Public | BindingFlags.Instance);

			if (callGeneric)
			{
				emit
					.call   (invoke.MakeGenericMethod(Context.CurrentMethod.ReturnType))
					.stloc  (Context.ReturnValue)
					;
			}
			else
			{
				if (withOutParameters)
				{
					LocalBuilder ret = emit.DeclareLocal(typeof(object[]));
					Label       exit = emit.DefineLabel();

					emit
						.call       (invoke)
						.dup
						.stloc      (ret)
						.brfalse_s  (exit)
						;

					int idx = 0;

					if (Context.CurrentMethod.ReturnType != typeof(void))
					{
						emit
							.ldloc          (ret)
							.ldc_i4_0
							.ldelem_ref
							.CastFromObject (Context.CurrentMethod.ReturnType)
							.stloc          (Context.ReturnValue)
							;

						++idx;
					}

					for (int i = 0; i < parameters.Count; ++i)
					{
						ParameterInfo pi = parameters[i];
						Type        type = pi.ParameterType;

						if (!type.IsByRef)
							continue;

						// Get ride of ref
						//
						type = type.GetElementType();

						emit
							.ldarg          (pi)
							.ldloc          (ret)
							.ldc_i4_        (idx)
							.ldelem_ref
							.CastFromObject (type)
							.stind          (type)
							;

						++idx;
					}

					emit.MarkLabel(exit);
				}
				else
				{
					emit
						.call  (invoke)
						.pop
						.end   ()
						;
				}
			}
		}

		private void EmitAsyncCall(List<ParameterInfo> parameters, EmitHelper emit, ParameterInfo callback, ParameterInfo exceptionCallback)
		{
			if (Context.CurrentMethod.IsDefined(typeof(UpToDateAttribute), true))
			{
				EmitCookie(emit);
			}
			else
			{
				emit.ldnull.end();
			}

			if (exceptionCallback != null)
			{
				emit.ldarg(exceptionCallback);
			}
			else
			{
				emit.ldnull.end();
			}

			emit.ldarg(callback);

			EmitParameters(emit, parameters);

			emit
				.call(typeof(WebClientBase), "InvokeAsync", typeof(string), typeof(AsyncCallState), typeof(Action<Exception>), typeof(Delegate), typeof(object[]))
				;
		}

		private void EmitMethodName(EmitHelper emit)
		{
			object[] attrs = Context.CurrentMethod.GetCustomAttributes(typeof(ActionNameAttribute), true);

			string methodName = attrs == null || attrs.Length == 0?
				Context.CurrentMethod.Name: ((ActionNameAttribute)attrs[0]).Name;

			emit
				.ldarg_0
				.ldstr     (methodName)
				;
		}

		private void EmitCookie(EmitHelper emit)
		{
			string fieldName = "_cookie$" + Context.CurrentMethod.Name;

			FieldBuilder field = Context.GetField(fieldName) ??
				Context.CreateField(fieldName, typeof(AsyncCallState), FieldAttributes.Private);

			Label checkCookie = emit.DefineLabel();

			emit
				.ldarg_0
				.ldfld      (field)
				.dup
				.brtrue_s   (checkCookie)
				.pop
				.ldarg_0
				.dup
				.newobj     (typeof(AsyncCallState))
				.stfld      (field)
				.ldfld      (field)
				.MarkLabel  (checkCookie)
				;
		}

		private bool EmitParameters(EmitHelper emit, List<ParameterInfo> parameters)
		{
			bool hasOutRefParameters = false;

			int count = parameters.Count;
			for (int i = 0; i < count; i++)
			{
				if (parameters[i].IsOut)
				{
					count--;
					hasOutRefParameters = true;
				}
			}

			emit
				.ldc_i4_(count)
				.newarr(typeof(object))
				;

			int idx = 0;
			for (int i = 0; i < parameters.Count; i++)
			{
				ParameterInfo pi = parameters[i];

				if (pi.IsOut)
				{
					// Output-only parameter
					//
					continue;
				}

				if (pi.ParameterType.IsByRef)
					hasOutRefParameters = true;

				emit
					.dup
					.ldc_i4_    (idx)
					.ldargEx    (pi, true)
					.stelem_ref
					.end()
					;

				++idx;
			}

			return hasOutRefParameters;
		}
	}
}