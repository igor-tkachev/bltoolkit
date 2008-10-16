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
		public DuckTypeBuilder(MustImplementAttribute defaultAttribute, Type interfaceType, Type[] objectTypes)
		{
			_interfaceType    = interfaceType;
			_objectTypes      = objectTypes;
			_defaultAttribute = defaultAttribute;
		}

		private readonly Type                   _interfaceType;
		private readonly Type[]                 _objectTypes;
		private          TypeBuilderHelper      _typeBuilder;
		private readonly MustImplementAttribute _defaultAttribute;

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
			string name = "";

			foreach (Type t in _objectTypes)
			{
				if (t != null)
					name += AbstractClassBuilder.GetTypeFullName(t).Replace('+', '.');
				name += "$";
			}

			return name + AssemblyNameSuffix;
		}

		private bool BuildMembers(Type interfaceType)
		{
			FieldInfo    objectsField = typeof(DuckType).GetField("_objects", BindingFlags.NonPublic | BindingFlags.Instance);
			BindingFlags flags        = BindingFlags.Public | BindingFlags.Instance
				| (DuckTyping.AllowStaticMembers? BindingFlags.Static | BindingFlags.FlattenHierarchy: 0);

			foreach (MethodInfo interfaceMethod in interfaceType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
			{
				ParameterInfo[] ips = interfaceMethod.GetParameters();
				MethodInfo      targetMethod = null;
				int             typeIndex    = 0;

				for (; typeIndex < _objectTypes.Length; typeIndex++)
				{
					if (_objectTypes[typeIndex] == null)
						continue;

					foreach (MethodInfo mi in _objectTypes[typeIndex].GetMethods(flags))
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

					if (targetMethod != null)
						break;
				}

				MethodBuilderHelper builder = _typeBuilder.DefineMethod(interfaceMethod);
				EmitHelper          emit    = builder.Emitter;

				if (targetMethod != null)
				{
					if (!targetMethod.IsStatic)
					{
						emit
							.ldarg_0
							.ldfld         (objectsField)
							.ldc_i4        (typeIndex)
							.ldelem_ref
							.end()
							;

						if (_objectTypes[typeIndex].IsValueType)
						{
							// For value types we have to use stack.
							//
							LocalBuilder obj = emit.DeclareLocal(_objectTypes[typeIndex]);

							emit
								.unbox_any (_objectTypes[typeIndex])
								.stloc     (obj)
								.ldloca    (obj)
								;
						}
						else
							emit
								.castclass (_objectTypes[typeIndex])
								;
					}

					foreach (ParameterInfo p in interfaceMethod.GetParameters())
						emit.ldarg(p);

					if (targetMethod.IsStatic || targetMethod.IsFinal || targetMethod.DeclaringType.IsSealed)
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
							attr = _defaultAttribute;
					}

					// When the member is marked as 'Required' throw a build-time exception.
					//
					if (attr.Implement)
					{
						if (attr.ThrowException)
							throw new TypeBuilderException(string.Format(
								Resources.TypeBuilder_PublicMethodMustBeImplemented,
								_objectTypes.Length > 0 && _objectTypes[0] != null ? _objectTypes[0].FullName: "???",
								interfaceMethod));
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
								_objectTypes.Length > 0 && _objectTypes[0] != null ? _objectTypes[0].FullName: "???",
								interfaceMethod);

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
