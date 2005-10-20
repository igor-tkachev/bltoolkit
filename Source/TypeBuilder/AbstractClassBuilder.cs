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

			_builders.Add(_defaultTypeBuilder);
			_context.TypeBuilders = new TypeBuilderList();
		}

		private static DefaultTypeBuilder _defaultTypeBuilder = new DefaultTypeBuilder();

		private BuildContext    _context;
		private TypeBuilderList _builders;

		public void Build()
		{
			DefineNonAbstractType();

			_context.Element = BuildElement.Type;

			Build(BuildStep.Before, _builders);
			Build(BuildStep.Build,  _builders);

			DefineAbstractProperties();
			DefineAbstractMethods();
			OverrideVirtualProperties();
			OverrideVirtualMethods();
			DefineInterfaces();

			_context.Element = BuildElement.Type;

			Build(BuildStep.After, _builders);

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
			public BuilderComparer(BuildContext context)
			{
				_context = context;
			}

			BuildContext _context;

			public int Compare(ITypeBuilder x, ITypeBuilder y)
			{
				IAbstractTypeBuilder bx = (IAbstractTypeBuilder)x;
				IAbstractTypeBuilder by = (IAbstractTypeBuilder)y;

				return bx.GetPriority(_context) - by.GetPriority(_context);
			}
		}
#else
		class BuilderComparer : IComparer
		{
			public BuilderComparer(BuildContext context)
			{
				_context = context;
			}

			BuildContext _context;

			public int Compare(object x, object y)
			{
				IAbstractTypeBuilder bx = (IAbstractTypeBuilder)x;
				IAbstractTypeBuilder by = (IAbstractTypeBuilder)y;

				return bx.GetPriority(_context) - by.GetPriority(_context);
			}
		}
#endif

		private void Build(BuildStep step, TypeBuilderList builders)
		{
			_context.Step = step;
			_context.TypeBuilders.Clear();

			foreach (IAbstractTypeBuilder builder in builders)
				if (builder.IsApplied(_context))
					_context.TypeBuilders.Add(builder);

			if (_context.IsOverrideMethod || _context.IsOverrideProperty)
				builders.Add(_defaultTypeBuilder);

			TypeFactory.CheckCompatibility(_context, _context.TypeBuilders);

			_context.TypeBuilders.Sort(new BuilderComparer(_context));

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

		private bool IsApplied(BuildElement element, TypeBuilderList builders)
		{
			_context.Element = element;

			foreach (IAbstractTypeBuilder builder in builders)
			{
				_context.Step = BuildStep.Before;

				if (builder.IsApplied(_context))
					return true;

				_context.Step = BuildStep.Build;

				if (builder.IsApplied(_context))
					return true;

				_context.Step = BuildStep.After;

				if (builder.IsApplied(_context))
					return true;
			}

			return false;
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

			// Getter can be not defined. We will generate it anyway.
			//
			if (getter == null)
				getter = new FakeGetter(propertyInfo);

			if (getter != null)
			{
				builders = Combine(
					GetBuilders(getter.GetParameters()),
#if FW2
					GetBuilders(getter.ReturnParameter),
#else
					GetBuilders(new FakeParameterInfo(getter)),
#endif
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

				/*
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
				*/
			}

			_context.Element = BuildElement.AbstractGetter;

			Build(BuildStep.Before, builders);
			Build(BuildStep.Build,  builders);
			Build(BuildStep.After,  builders);

			_context.Element = BuildElement.VirtualGetter;

			Build(BuildStep.Before, builders);
			Build(BuildStep.Build,  builders);
			Build(BuildStep.After,  builders);

			EndEmitMethod();
		}

		private void DefineAbstractSetter(
			PropertyInfo propertyInfo, MethodInfo getter, MethodInfo setter, TypeBuilderList propertyBuilders)
		{
			TypeBuilderList builders;

			// Setter can be not defined. We will generate it anyway.
			//
			if (setter == null)
				setter = new FakeSetter(propertyInfo);

			if (setter != null)
			{
				builders = Combine(
					GetBuilders(setter.GetParameters()),
#if FW2
					GetBuilders(setter.ReturnParameter),
#else
					GetBuilders(new FakeParameterInfo(setter)),
#endif
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

				/*
				ParameterInfo[] indexParams = propertyInfo.GetIndexParameters();
				ParameterInfo[] parameters  = new ParameterInfo[indexParams.Length + 1];
				Type[]          paramTypes  = new Type[parameters.Length];

				//parameters[0] = getter.ReturnParameter;
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

				BeinEmitMethod(typeof(void), parameters);
				*/
			}

			_context.Element = BuildElement.AbstractSetter;

			Build(BuildStep.Before, builders);
			Build(BuildStep.Build,  builders);
			Build(BuildStep.After,  builders);

			_context.Element = BuildElement.VirtualSetter;

			Build(BuildStep.Before, builders);
			Build(BuildStep.Build,  builders);
			Build(BuildStep.After,  builders);

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
#if FW2
						GetBuilders(method.ReturnParameter),
#else
						GetBuilders(new FakeParameterInfo(method)),
#endif
						GetBuilders(method),
						_builders);

					BeginEmitMethod(method);

					_context.Element = BuildElement.AbstractMethod;

					Build(BuildStep.Before, builders);
					Build(BuildStep.Build,  builders);
					Build(BuildStep.After,  builders);

					_context.Element = BuildElement.VirtualMethod;

					Build(BuildStep.Before, builders);
					Build(BuildStep.Build,  builders);
					Build(BuildStep.After,  builders);

					EndEmitMethod();
				}
			}
		}

		private void OverrideVirtualProperties()
		{
			PropertyInfo[] props = _context.OriginalType.GetProperties(
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			foreach (PropertyInfo pi in props)
			{
				_context.CurrentProperty = pi;

				TypeBuilderList propertyBuilders = GetBuilders(pi);

				MethodInfo getter = pi.GetGetMethod(true);

				if (getter != null && getter.IsAbstract == false)
					OverrideGetter(pi, getter, propertyBuilders);

				MethodInfo setter = pi.GetSetMethod(true);

				if (setter != null && setter.IsAbstract == false)
					OverrideSetter(pi, getter, setter, propertyBuilders);
			}

			_context.CurrentProperty = null;
		}

		private void OverrideGetter(
			PropertyInfo pi, MethodInfo getter, TypeBuilderList propertyBuilders)
		{
			TypeBuilderList builders = Combine(
				GetBuilders(getter.GetParameters()),
#if FW2
				GetBuilders(getter.ReturnParameter),
#else
				GetBuilders(new FakeParameterInfo(getter)),
#endif
				GetBuilders(getter),
				propertyBuilders,
				_builders);

			if (IsApplied(BuildElement.VirtualGetter, builders))
			{
				BeginEmitMethod(getter);

				Build(BuildStep.Before, builders);
				Build(BuildStep.Build, builders);
				Build(BuildStep.After, builders);

				EndEmitMethod();
			}
		}

		private void OverrideSetter(
			PropertyInfo pi, MethodInfo getter, MethodInfo setter, TypeBuilderList propertyBuilders)
		{
			TypeBuilderList builders = Combine(
				GetBuilders(setter.GetParameters()),
#if FW2
				GetBuilders(setter.ReturnParameter),
#else
				GetBuilders(new FakeParameterInfo(setter)),
#endif
				GetBuilders(setter),
				propertyBuilders,
				_builders);

			if (IsApplied(BuildElement.VirtualSetter, builders))
			{
				BeginEmitMethod(setter);

				Build(BuildStep.Before, builders);
				Build(BuildStep.Build,  builders);
				Build(BuildStep.After,  builders);

				EndEmitMethod();
			}
		}

		private void OverrideVirtualMethods()
		{
			MethodInfo[] methods = _context.OriginalType.GetMethods(
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			foreach (MethodInfo method in methods)
			{
				if (!method.IsAbstract && (method.Attributes & MethodAttributes.SpecialName) == 0)
				{
					TypeBuilderList builders = Combine(
						GetBuilders(method.GetParameters()),
#if FW2
						GetBuilders(method.ReturnParameter),
#else
						GetBuilders(new FakeParameterInfo(method)),
#endif
						GetBuilders(method),
						_builders);

					if (IsApplied(BuildElement.VirtualMethod, builders))
					{
						BeginEmitMethod(method);

						Build(BuildStep.Before, builders);
						Build(BuildStep.Build,  builders);
						Build(BuildStep.After,  builders);

						EndEmitMethod();
					}
				}
			}
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

					_context.Element = BuildElement.InterfaceMethod;
					_context.Step    = BuildStep.Build;
					builder.Build(_context);

					EndEmitMethod();
				}

				_context.CurrentInterface = null;
			}
		}
	}
}
