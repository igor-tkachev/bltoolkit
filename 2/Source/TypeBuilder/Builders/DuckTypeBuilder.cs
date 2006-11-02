using System;

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
			get { return "DuckType." + _interfaceType.FullName.Replace('+', '.'); }
		}

		public Type Build(Type sourceType, AssemblyBuilderHelper assemblyBuilder)
		{
			_typeBuilder = assemblyBuilder.DefineType(GetClassName(), typeof(DuckType), _interfaceType);

			BuildMembers();

			return _typeBuilder.Create();
		}

		#endregion

		private string GetClassName()
		{
			return _objectType.FullName.Replace('+', '.')
#if !FW2
				.Replace('[', '_').Replace(']', '_')
#endif
				+ "." + AssemblyNameSuffix;
		}

		private void BuildMembers()
		{
			FieldInfo objectField = typeof(DuckType).GetField("_object", BindingFlags.NonPublic | BindingFlags.Instance);

			foreach (MethodInfo interfaceMethod in _interfaceType.GetMethods())
			{
				MethodInfo targetMethod = null;

				foreach (MethodInfo mi in _objectType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
				{
					ParameterInfo[] ips = interfaceMethod.GetParameters();
					ParameterInfo[] ops = mi.             GetParameters();

					if (mi.IsPublic && mi.IsStatic == false &&
						mi.Name       == interfaceMethod.Name       &&
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

				if (targetMethod == null)
					throw new TypeBuilderException(string.Format(
						"Type '{0}' must implement public method '{1}'", _objectType.FullName, interfaceMethod));

				MethodBuilderHelper builder = _typeBuilder.DefineMethod(interfaceMethod);
				EmitHelper          emit    = builder.Emitter;

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
		}
	}
}
