using System;
using System.Reflection.Emit;
using BLToolkit.Patterns;
using BLToolkit.Reflection.Emit;
using System.Reflection;

namespace BLToolkit.TypeBuilder.Builders
{
	class DuckTypeBuilder : ITypeBuilder
	{
		public DuckTypeBuilder(Type interfaceType, Type objectType)
		{
			_interfaceType = interfaceType;
			_objectType    = objectType;
		}

		Type              _interfaceType;
		Type              _objectType;
		TypeBuilderHelper _typeBuilder;

		#region ITypeBuilder Members

		public string AssemblyNameSuffix
		{
			get { return "DuckType." + AbstractClassBuilder.GetTypeFullName(_interfaceType).Replace('+', '.'); }
		}

		public Type Build(Type sourceType, AssemblyBuilderHelper assemblyBuilder)
		{
			_typeBuilder = assemblyBuilder.DefineType(GetClassName(), typeof(DuckType), _interfaceType);

			BuildMembers(_interfaceType);

			foreach (Type t in _interfaceType.GetInterfaces())
				BuildMembers(t);

			return _typeBuilder.Create();
		}

		#endregion

		private string GetClassName()
		{
			return AbstractClassBuilder.GetTypeFullName(_objectType).Replace('+', '.')
#if !FW2
				.Replace('[', '_').Replace(']', '_')
#endif
				+ "." + AssemblyNameSuffix;
		}

		private void BuildMembers(Type interfaceType)
		{
			FieldInfo objectField = typeof(DuckType).GetField("_object", BindingFlags.NonPublic | BindingFlags.Instance);

			foreach (MethodInfo interfaceMethod in interfaceType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
			{
				ParameterInfo[] ips = interfaceMethod.GetParameters();
				MethodInfo      targetMethod = null;

				foreach (MethodInfo mi in _objectType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
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
					emit
						.ldarg_0
						.ldfld    (objectField)
						.castclass(_objectType)
						;

					foreach (ParameterInfo p in interfaceMethod.GetParameters())
						emit.ldarg(p);

					emit
						.callvirt(targetMethod)
						.ret();
				}
				else
				{
					// Method or property was not found.
					// Insert an empty stub or stub that throws the NotImplementedException.
					//
					MustImplementAttribute attr = (MustImplementAttribute)Attribute.GetCustomAttribute(interfaceMethod, typeof (MustImplementAttribute));
					if (attr == null)
					{
						attr = (MustImplementAttribute)Attribute.GetCustomAttribute(interfaceMethod.DeclaringType, typeof (MustImplementAttribute));
						if (attr == null)
							attr = MustImplementAttribute.Default;
					}

					// When the member is marked as 'Required' throw a build-time exception.
					//
					if (attr.Implement)
						throw new TypeBuilderException(string.Format(
							"Type '{0}' must implement required public method '{1}'", _objectType.FullName, interfaceMethod));

					if (attr.ThrowException)
					{
						string message = attr.ExceptionMessage;
						if (message == null)
							message = string.Format("Type '{0}' does not implement public method '{1}'", _objectType.FullName, interfaceMethod);

						emit
							.ldstr     (message)
							.newobj    (typeof(NotImplementedException), typeof(string))
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
		}
	}
}
