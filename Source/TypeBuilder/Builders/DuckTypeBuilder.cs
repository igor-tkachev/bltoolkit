using System;
using System.Reflection;
using System.Reflection.Emit;
using BLToolkit.Patterns;
using BLToolkit.Properties;
using BLToolkit.Reflection.Emit;

namespace BLToolkit.TypeBuilder.Builders
{
	class DuckTypeBuilder : ITypeBuilder
	{
		public DuckTypeBuilder(Type interfaceType, Type objectType)
		{
			_interfaceType = interfaceType;
			_objectType    = objectType;
		}

		private readonly Type              _interfaceType;
		private readonly Type              _objectType;
		private          TypeBuilderHelper _typeBuilder;

		#region ITypeBuilder Members

		public string AssemblyNameSuffix
		{
			get { return "DuckType." + AbstractClassBuilder.GetTypeFullName(_interfaceType).Replace('+', '.'); }
		}

		public Type Build(Type sourceType, AssemblyBuilderHelper assemblyBuilder)
		{
			_typeBuilder = assemblyBuilder.DefineType(GetClassName(), typeof(DuckType), _interfaceType);

			if (!BuildMembers(_interfaceType))
				return null;

			foreach (Type t in _interfaceType.GetInterfaces())
				if (!BuildMembers(t))
					return null;

			return _typeBuilder.Create();
		}

		#endregion

		private string GetClassName()
		{
			return AbstractClassBuilder.GetTypeFullName(_objectType)
				.Replace('+', '.') + "$" + AssemblyNameSuffix;
		}

		private bool BuildMembers(Type interfaceType)
		{
			FieldInfo    objectField = typeof(DuckType).GetField("_object", BindingFlags.NonPublic | BindingFlags.Instance);
			BindingFlags flags       = BindingFlags.Public | BindingFlags.Instance
				| (DuckTyping.AllowStaticMembers? BindingFlags.Static | BindingFlags.FlattenHierarchy: 0);

			foreach (MethodInfo interfaceMethod in interfaceType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
			{
				ParameterInfo[] ips = interfaceMethod.GetParameters();
				MethodInfo      targetMethod = null;

				foreach (MethodInfo mi in _objectType.GetMethods(flags))
				{
					ParameterInfo[] ops = mi.GetParameters();

					if (mi.Name       == interfaceMethod.Name       &&
						mi.ReturnType == interfaceMethod.ReturnType &&
						ops.Length    == ips.Length)
					{
						targetMethod = mi;

						for (int i = 0; i < ips.Length && targetMethod != null; i++)
						{
							ParameterInfo ip = ips[i];
							ParameterInfo op = ops[i];

							if (ip.ParameterType != op.ParameterType ||
								ip.IsIn          != op.IsIn          ||
								ip.IsOut         != op.IsOut)
							{
								targetMethod = null;
							}
						}

						if (targetMethod != null)
							break;
					}
				}

				MethodBuilderHelper builder = _typeBuilder.DefineMethod(interfaceMethod);
				EmitHelper          emit    = builder.Emitter;

				if (targetMethod != null)
				{
					if (!targetMethod.IsStatic)
					{
						emit
							.ldarg_0
							.ldfld         (objectField)
							;

						if (_objectType.IsValueType)
						{
							// For value types we have to use stack.
							//
							LocalBuilder obj = emit.DeclareLocal(_objectType);

							emit
								.unbox_any (_objectType)
								.stloc     (obj)
								.ldloca    (obj)
								;
						}
						else
							emit
								.castclass (_objectType)
								;
					}

					foreach (ParameterInfo p in interfaceMethod.GetParameters())
						emit.ldarg(p);

					if (targetMethod.IsStatic)
						emit
							.call     (targetMethod)
							.ret();
					else
						emit
							.callvirt (targetMethod)
							.ret();
				}
				else
				{
					// Method or property was not found.
					// Insert an empty stub or stub that throws the NotImplementedException.
					//
					MustImplementAttribute attr = (MustImplementAttribute)
						Attribute.GetCustomAttribute(interfaceMethod, typeof (MustImplementAttribute));

					if (attr == null)
					{
						attr = (MustImplementAttribute)Attribute.GetCustomAttribute(
							interfaceMethod.DeclaringType, typeof (MustImplementAttribute));
						if (attr == null)
							attr = MustImplementAttribute.Default;
					}

					// When the member is marked as 'Required' throw a build-time exception.
					//
					if (attr.Implement)
					{
						if (attr.ThrowException)
							throw new TypeBuilderException(string.Format(
								Resources.TypeBuilder_PublicMethodMustBeImplemented,
								_objectType.FullName, interfaceMethod));
						else
						{
							// Implement == true, but ThrowException == false.
							// In this case the null pointer will be returned.
							// This mimics the 'as' operator behaviour.
							//
							return false;
						}
					}

					if (attr.ThrowException)
					{
						string message = attr.ExceptionMessage;

						if (message == null)
							message = string.Format(Resources.TypeBuilder_PublicMethodNotImplemented,
								_objectType.FullName, interfaceMethod);

						emit
							.ldstr  (message)
							.newobj (typeof(NotImplementedException), typeof(string))
							.@throw
							.end();
					}
					else
					{
						// Emit a 'do nothing' stub.
						//
						LocalBuilder returnValue = null;

						if (interfaceMethod.ReturnType != typeof(void))
						{
							returnValue = emit.DeclareLocal(interfaceMethod.ReturnType);
							emit.Init(returnValue);
						}

						// Initialize out parameters.
						//
						ParameterInfo[] parameters = interfaceMethod.GetParameters();

						if (parameters != null)
							emit.InitOutParameters(parameters);

						if (returnValue != null)
							emit.ldloc(returnValue);

						emit.ret();
					}
				}
			}

			return true;
		}
	}
}
