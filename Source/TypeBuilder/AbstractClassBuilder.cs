using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

#if FW2
using System.Collections.Generic;
#endif

using BLToolkit.Reflection.Emit;

namespace BLToolkit.TypeBuilder
{
	class AbstractClassBuilder
	{
		public AbstractClassBuilder(BuildContext context)
		{
			_context  = context;
			_builders = new TypeBuilderList();

			foreach (ITypeBuilder tb in _context.TypeBuilders)
			{
				if (tb is IAbstractTypeBuilder)
				{
					((IAbstractTypeBuilder)tb).TargetElement = context.Type;
					_builders.Add(tb);
				}
			}

			_context.TypeBuilders = new TypeBuilderList();
		}

		private BuildContext    _context;
		private TypeBuilderList _builders;

		public void Build()
		{
			DefineNonAbstractType();

			Build(BuildOperation.BeginBuild, _builders);

			DefineAbstractProperties();
			DefineAbstractMethods();
			OverrideVirtualProperties();
			OverrideVirtualMethods();
			DefineInterfaces();

			Build(BuildOperation.EndBuild, _builders);

			// Finalize constructors.
			//
			if (_context.TypeBuilder.IsDefaultConstructorDefined)
				_context.TypeBuilder.DefaultConstructor.Emitter.ret();

			if (_context.TypeBuilder.IsInitConstructorDefined)
				_context.TypeBuilder.InitConstructor.Emitter.ret();

			if (_context.TypeBuilder.IsTypeInitializerDefined)
				_context.TypeBuilder.TypeInitializer.Emitter.ret();

			// Create the type.
			//
			_context.Type = _context.TypeBuilder.Create();

			_context.TypeBuilders = _builders;
		}

		private void DefineNonAbstractType()
		{
			ArrayList interfaces = new ArrayList();

			foreach (IAbstractTypeBuilder tb in _builders)
			{
				Type[] types = tb.GetInterfaces();

				if (types != null)
				{
					foreach (Type t in types)
					{
						if (t == null)
							continue;

						if (t.IsInterface == false)
						{
							TypeFactory.Error("The '{1}' must be an interface.",
								_context.OriginalType.FullName, t.FullName);
						}
						else if (interfaces.Contains(t) == false)
						{
							interfaces.Add(t);
							_context.InterfaceMap.Add(t, tb);
						}
					}
				}
			}

			string typeName = _context.OriginalType.FullName.Replace('+', '.');

			typeName = typeName.Substring(0, typeName.Length - _context.OriginalType.Name.Length);
			typeName = typeName + "BLToolkitExtension." + _context.OriginalType.Name;

			_context.TypeBuilder = _context.AssemblyBuilder.DefineType(
				typeName,
				TypeAttributes.Public | TypeAttributes.BeforeFieldInit,
				_context.OriginalType,
				(Type[])interfaces.ToArray(typeof(Type)));

			if (_context.OriginalType.IsSerializable)
				_context.TypeBuilder.SetCustomAttribute(typeof(SerializableAttribute));
		}

#if FW2
		class BuilderComparer : IComparer<ITypeBuilder>
		{
			public BuilderComparer(BuildOperation operation)
			{
				_operation = operation;
			}

			BuildOperation _operation;

			public int Compare(ITypeBuilder x, ITypeBuilder y)
			{
				IAbstractTypeBuilder bx = (IAbstractTypeBuilder)x;
				IAbstractTypeBuilder by = (IAbstractTypeBuilder)y;

				return bx.GetPriority(_operation) - by.GetPriority(_operation);
			}
		}
#else
		class BuilderComparer : IComparer
		{
			public BuilderComparer(BuildOperation operation)
			{
				_operation = operation;
			}

			BuildOperation _operation;

			public int Compare(object x, object y)
			{
				IAbstractTypeBuilder bx = (IAbstractTypeBuilder)x;
				IAbstractTypeBuilder by = (IAbstractTypeBuilder)y;

				return bx.GetPriority(_operation) - by.GetPriority(_operation);
			}
		}
#endif

		private void Build(BuildOperation operation, TypeBuilderList builders)
		{
			_context.BuildOperation = operation;
			_context.TypeBuilders.Clear();
			_context.TypeBuilders.AddRange(builders);

			TypeFactory.CheckCompatibility(_context, _context.TypeBuilders);

			_context.TypeBuilders.Sort(new BuilderComparer(operation));

			for (int i = 0; i < _context.TypeBuilders.Count; i++)
				((IAbstractTypeBuilder)_context.TypeBuilders[i]).Build(_context);
		}

		private void BeginEmitMethod(MethodInfo method)
		{
			_context.MethodBuilder = _context.TypeBuilder.DefineMethod(method);

			BeginEmitMethod(method.ReturnType, method.GetParameters());
		}

		private void BeginEmitMethod(Type returnType, ParameterInfo[] parameters)
		{
			EmitHelper emit = _context.MethodBuilder.Emitter;

			// Label right before return.
			//
			_context.ReturnLabel = emit.DefineLabel();

			// Create return value.
			//
			if (returnType != typeof(void))
			{
				_context.ReturnValue = _context.MethodBuilder.Emitter.DeclareLocal(returnType);
				emit.Init(_context.ReturnValue);
			}

			// Initialize out parameters.
			//
			if (parameters != null)
				emit.InitOutParameters(parameters);
		}

		private void EndEmitMethod()
		{
			EmitHelper emit = _context.MethodBuilder.Emitter;

			// Prepare return.
			//
			emit.MarkLabel(_context.ReturnLabel);

			if (_context.ReturnValue != null)
				emit.ldloc(_context.ReturnValue);

			emit.ret();

			// Cleanup the context.
			//
			_context.ReturnValue = null;
			_context.MethodBuilder = null;
		}

		private TypeBuilderList GetBuilders(object[] attributes, object target)
		{
			TypeBuilderList builders = new TypeBuilderList(attributes.Length);

			foreach (TypeBuilderAttribute attr in attributes)
			{
				if (attr.TypeBuilder is IAbstractTypeBuilder)
				{
					((IAbstractTypeBuilder)attr.TypeBuilder).TargetElement = target;
					builders.Add(attr.TypeBuilder);
				}
			}

			return builders;
		}

		private TypeBuilderList GetBuilders(MemberInfo memberInfo)
		{
			return GetBuilders(
				memberInfo.GetCustomAttributes(typeof(TypeBuilderAttribute), true), memberInfo);
		}

		private TypeBuilderList GetBuilders(ParameterInfo parameterInfo)
		{
			return GetBuilders(
				parameterInfo.GetCustomAttributes(typeof(TypeBuilderAttribute), true), parameterInfo);
		}

		private TypeBuilderList GetBuilders(ParameterInfo[] parameters)
		{
			TypeBuilderList builders = new TypeBuilderList();

			foreach (ParameterInfo pi in parameters)
			{
				object[] attributes = pi.GetCustomAttributes(typeof(TypeBuilderAttribute), true);

				foreach (TypeBuilderAttribute attr in attributes)
				{
					if (attr.TypeBuilder is IAbstractTypeBuilder)
					{
						((IAbstractTypeBuilder)attr.TypeBuilder).TargetElement = pi;
						builders.Add(attr.TypeBuilder);
					}
				}
			}

			return builders;
		}

		private TypeBuilderList Combine(params TypeBuilderList[] builders)
		{
			TypeBuilderList list = new TypeBuilderList();

			foreach (TypeBuilderList l in builders)
				list.AddRange(l);

			return list;
		}

		private void DefineAbstractProperties()
		{
			PropertyInfo[] props = _context.OriginalType.GetProperties(
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			foreach (PropertyInfo pi in props)
			{
				_context.CurrentProperty = pi;

				TypeBuilderList propertyBuilders = GetBuilders(pi);

				MethodInfo getter = pi.GetGetMethod(true);
				MethodInfo setter = pi.GetSetMethod(true);

				if (getter != null && getter.IsAbstract ||
					setter != null && setter.IsAbstract)
				{
					DefineAbstractGetter(pi, getter, propertyBuilders);
					DefineAbstractSetter(pi, getter, setter, propertyBuilders);
				}
			}

			_context.CurrentProperty = null;
		}

		private void DefineAbstractGetter(
			PropertyInfo propertyInfo, MethodInfo getter, TypeBuilderList propertyBuilders)
		{
			TypeBuilderList builders;

			if (getter != null)
			{
				builders = Combine(
					GetBuilders(getter.GetParameters()),
					/////GetBuilders(getter.ReturnParameter),
					GetBuilders(getter),
					propertyBuilders,
					_builders);

				BeginEmitMethod(getter);
			}
			else
			{
				// Getter can be not defined. We will generate it anyway.
				//
				builders = Combine(propertyBuilders, _builders);

				ParameterInfo[] parameters = propertyInfo.GetIndexParameters();
				Type[]          paramTypes = new Type[parameters.Length];

				for (int i = 0; i < parameters.Length; i++)
					paramTypes[i] = parameters[i].ParameterType;

				_context.MethodBuilder = _context.TypeBuilder.DefineMethod(
					"get_" + propertyInfo.Name,
					MethodAttributes.Public |
					MethodAttributes.Virtual |
					MethodAttributes.HideBySig |
					MethodAttributes.SpecialName,
					propertyInfo.PropertyType,
					paramTypes);

				BeginEmitMethod(propertyInfo.PropertyType, parameters);
			}

			Build(BuildOperation.BeginBuildAbstractGetter, builders);
			Build(BuildOperation.BuildAbstractGetter,      builders);
			Build(BuildOperation.EndBuildAbstractGetter,   builders);

			EndEmitMethod();
		}

		private void DefineAbstractSetter(
			PropertyInfo propertyInfo, MethodInfo getter, MethodInfo setter, TypeBuilderList propertyBuilders)
		{
			TypeBuilderList builders;

			if (setter != null)
			{
				builders = Combine(
					GetBuilders(setter.GetParameters()),
					/////GetBuilders(setter.ReturnParameter),
					GetBuilders(setter),
					propertyBuilders,
					_builders);

				BeginEmitMethod(setter);
			}
			else
			{
				// Setter can be not defined. We will generate it anyway.
				//
				builders = Combine(propertyBuilders, _builders);

				ParameterInfo[] indexParams = propertyInfo.GetIndexParameters();
				ParameterInfo[] parameters  = new ParameterInfo[indexParams.Length + 1];
				Type[]          paramTypes  = new Type[parameters.Length];

				/////parameters[0] = getter.ReturnParameter;
				indexParams.CopyTo(parameters, 1);

				for (int i = 0; i < parameters.Length; i++)
					paramTypes[i] = parameters[i].ParameterType;

				_context.MethodBuilder = _context.TypeBuilder.DefineMethod(
					"set_" + propertyInfo.Name,
					MethodAttributes.Public |
					MethodAttributes.Virtual |
					MethodAttributes.HideBySig |
					MethodAttributes.SpecialName,
					typeof(void),
					paramTypes);

				BeginEmitMethod(typeof(void), parameters);
			}

			Build(BuildOperation.BeginBuildAbstractSetter, builders);
			Build(BuildOperation.BuildAbstractSetter,      builders);
			Build(BuildOperation.EndBuildAbstractSetter,   builders);

			EndEmitMethod();
		}

		private void DefineAbstractMethods()
		{
			MethodInfo[] methods = _context.OriginalType.GetMethods(
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			foreach (MethodInfo method in methods)
			{
				if (method.IsAbstract && (method.Attributes & MethodAttributes.SpecialName) == 0)
				{
					TypeBuilderList builders = Combine(
						GetBuilders(method.GetParameters()),
						/////GetBuilders(method.ReturnParameter),
						GetBuilders(method),
						_builders);

					BeginEmitMethod(method);

					Build(BuildOperation.BeginBuildAbstractMethod, builders);
					Build(BuildOperation.BuildAbstractMethod,      builders);
					Build(BuildOperation.EndBuildAbstractMethod,   builders);

					EndEmitMethod();
				}
			}
		}

		private void OverrideVirtualProperties()
		{
		}

		private void OverrideVirtualMethods()
		{
		}

		private void DefineInterfaces()
		{
			foreach (DictionaryEntry de in _context.InterfaceMap)
			{
				_context.CurrentInterface = (Type)de.Key;

				MethodInfo[] methods = _context.CurrentInterface.GetMethods();

				foreach (MethodInfo m in methods)
				{
					BeginEmitMethod(m);

					// Call builder to build the method.
					//
					IAbstractTypeBuilder builder = (IAbstractTypeBuilder)de.Value;

					_context.BuildOperation = BuildOperation.BuildInterfaceMethod;
					builder.Build(_context);

					EndEmitMethod();
				}

				_context.CurrentInterface = null;
			}
		}
	}
}
