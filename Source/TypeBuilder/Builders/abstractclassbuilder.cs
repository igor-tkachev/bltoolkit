using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

#if FW2
using System.Collections.Generic;
#endif

using BLToolkit.Reflection;
using BLToolkit.Reflection.Emit;

namespace BLToolkit.TypeBuilder.Builders
{
	class AbstractClassBuilder : ITypeBuilder
	{
		public string AssemblyNameSuffix
		{
			get { return TypeBuilderConsts.AssemblyNameSuffix; }
		}

		public Type Build(Type sourceType, AssemblyBuilderHelper assemblyBuilder)
		{
			_context  = new BuildContext(sourceType);
			_builders = new AbstractTypeBuilderList();

			_context.TypeBuilders    = GetBuilderList(_context.Type);
			_context.AssemblyBuilder = assemblyBuilder;

			_builders.AddRange(_context.TypeBuilders);
			_builders.Add(_defaultTypeBuilder);

			return Build();
		}

		private static AbstractTypeBuilderList GetBuilderList(TypeHelper type)
		{
			object[] attrs = type.GetAttributes(typeof(AbstractTypeBuilderAttribute));

			AbstractTypeBuilderList builders = new AbstractTypeBuilderList(attrs.Length);

			foreach (AbstractTypeBuilderAttribute attr in attrs)
			{
				if (attr.TypeBuilder != null)
				{
					IAbstractTypeBuilder builder = attr.TypeBuilder;

					builder.TargetElement = type;
					builders.Add(builder);
				}
			}

			return builders;
		}

		private static DefaultTypeBuilder _defaultTypeBuilder = new DefaultTypeBuilder();

		private BuildContext            _context;
		private AbstractTypeBuilderList _builders;

		private Type Build()
		{
			DefineNonAbstractType();

			_context.BuildElement = BuildElement.Type;

			Build(BuildStep.Before, _builders);
			Build(BuildStep.Build,  _builders);

			DefineAbstractProperties();
			DefineAbstractMethods();
			OverrideVirtualProperties();
			OverrideVirtualMethods();
			DefineInterfaces();

			_context.BuildElement = BuildElement.Type;

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
			return _context.TypeBuilder.Create();
		}

		private void CheckCompatibility(BuildContext context, AbstractTypeBuilderList builders)
		{
			for (int i = 0; i < builders.Count; i++)
			{
				IAbstractTypeBuilder cur = builders[i];

				if (cur == null)
					continue;

				for (int j = 0; j < builders.Count; j++)
				{
					IAbstractTypeBuilder test = builders[j];

					if (i == j || test == null)
						continue;

					if (cur.IsCompatible(context, test) == false)
						builders[j] = null;
				}
			}

			for (int i = 0; i < builders.Count; i++)
				if (builders[i] == null)
					builders.RemoveAt(i--);
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
								_context.Type.FullName, t.FullName);
						}
						else if (interfaces.Contains(t) == false)
						{
							interfaces.Add(t);
							_context.InterfaceMap.Add(t, tb);
						}
					}
				}
			}

			string typeName = _context.Type.FullName.Replace('+', '.');

			typeName = typeName.Substring(0, typeName.Length - _context.Type.Name.Length);
			typeName = typeName + "BLToolkitExtension." + _context.Type.Name;

			_context.TypeBuilder = _context.AssemblyBuilder.DefineType(
				typeName,
				TypeAttributes.Public | TypeAttributes.BeforeFieldInit,
				_context.Type,
				(Type[])interfaces.ToArray(typeof(Type)));

			if (_context.Type.IsSerializable)
				_context.TypeBuilder.SetCustomAttribute(typeof(SerializableAttribute));
		}

#if FW2
		class BuilderComparer : IComparer<IAbstractTypeBuilder>
		{
			public BuilderComparer(BuildContext context)
			{
				_context = context;
			}

			BuildContext _context;

			public int Compare(IAbstractTypeBuilder x, IAbstractTypeBuilder y)
			{
				return x.GetPriority(_context) - y.GetPriority(_context);
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

		private void Build(BuildStep step, AbstractTypeBuilderList builders)
		{
			_context.Step = step;
			_context.TypeBuilders.Clear();

			foreach (IAbstractTypeBuilder builder in builders)
				if (builder.IsApplied(_context))
					_context.TypeBuilders.Add(builder);

			if (_context.IsVirtualMethod || _context.IsVirtualProperty)
				_context.TypeBuilders.Add(_defaultTypeBuilder);

			CheckCompatibility(_context, _context.TypeBuilders);

			_context.TypeBuilders.Sort(new BuilderComparer(_context));

			for (int i = 0; i < _context.TypeBuilders.Count; i++)
				_context.TypeBuilders[i].Build(_context);
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

		private AbstractTypeBuilderList GetBuilders(object[] attributes, object target)
		{
			AbstractTypeBuilderList builders = new AbstractTypeBuilderList(attributes.Length);

			foreach (AbstractTypeBuilderAttribute attr in attributes)
			{
				IAbstractTypeBuilder builder = attr.TypeBuilder;

				((IAbstractTypeBuilder)builder).TargetElement = target;
				builders.Add(builder);
			}

			return builders;
		}

		private AbstractTypeBuilderList GetBuilders(MemberInfo memberInfo)
		{
			return GetBuilders(
				memberInfo.GetCustomAttributes(typeof(AbstractTypeBuilderAttribute), true), memberInfo);
		}

		private AbstractTypeBuilderList GetBuilders(ParameterInfo parameterInfo)
		{
			return GetBuilders(
				parameterInfo.GetCustomAttributes(typeof(AbstractTypeBuilderAttribute), true), parameterInfo);
		}

		private AbstractTypeBuilderList GetBuilders(ParameterInfo[] parameters)
		{
			AbstractTypeBuilderList builders = new AbstractTypeBuilderList();

			foreach (ParameterInfo pi in parameters)
			{
				object[] attributes = pi.GetCustomAttributes(typeof(AbstractTypeBuilderAttribute), true);

				foreach (AbstractTypeBuilderAttribute attr in attributes)
				{
					IAbstractTypeBuilder builder = attr.TypeBuilder;

					((IAbstractTypeBuilder)builder).TargetElement = pi;
					builders.Add(builder);
				}
			}

			return builders;
		}

		private AbstractTypeBuilderList Combine(params AbstractTypeBuilderList[] builders)
		{
			AbstractTypeBuilderList list = new AbstractTypeBuilderList();

			foreach (AbstractTypeBuilderList l in builders)
				list.AddRange(l);

			return list;
		}

		private bool IsApplied(BuildElement element, AbstractTypeBuilderList builders)
		{
			_context.BuildElement = element;

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
			PropertyInfo[] props = _context.Type.GetProperties(
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			foreach (PropertyInfo pi in props)
			{
				_context.CurrentProperty = pi;

				AbstractTypeBuilderList propertyBuilders = GetBuilders(pi);

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
			PropertyInfo propertyInfo, MethodInfo getter, AbstractTypeBuilderList propertyBuilders)
		{
			AbstractTypeBuilderList builders;

			// Getter can be not defined. We will generate it anyway.
			//
			if (getter == null)
				getter = new FakeGetter(propertyInfo);

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

			_context.BuildElement = BuildElement.AbstractGetter;

			Build(BuildStep.Before, builders);
			Build(BuildStep.Build,  builders);
			Build(BuildStep.After,  builders);

			EndEmitMethod();
		}

		private void DefineAbstractSetter(
			PropertyInfo            propertyInfo,
			MethodInfo              getter,
			MethodInfo              setter,
			AbstractTypeBuilderList propertyBuilders)
		{
			AbstractTypeBuilderList builders;

			// Setter can be not defined. We will generate it anyway.
			//
			if (setter == null)
				setter = new FakeSetter(propertyInfo);

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

			_context.BuildElement = BuildElement.AbstractSetter;

			Build(BuildStep.Before, builders);
			Build(BuildStep.Build,  builders);
			Build(BuildStep.After,  builders);

			EndEmitMethod();
		}

		private void DefineAbstractMethods()
		{
			MethodInfo[] methods = _context.Type.GetMethods(
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			foreach (MethodInfo method in methods)
			{
				if (method.IsAbstract && (method.Attributes & MethodAttributes.SpecialName) == 0)
				{
					AbstractTypeBuilderList builders = Combine(
						GetBuilders(method.GetParameters()),
#if FW2
						GetBuilders(method.ReturnParameter),
#else
						GetBuilders(new FakeParameterInfo(method)),
#endif
						GetBuilders(method),
						_builders);

					BeginEmitMethod(method);

					_context.BuildElement = BuildElement.AbstractMethod;

					Build(BuildStep.Before, builders);
					Build(BuildStep.Build,  builders);
					Build(BuildStep.After,  builders);

					_context.BuildElement = BuildElement.VirtualMethod;

					Build(BuildStep.Before, builders);
					Build(BuildStep.Build,  builders);
					Build(BuildStep.After,  builders);

					EndEmitMethod();
				}
			}
		}

		private void OverrideVirtualProperties()
		{
			PropertyInfo[] props = _context.Type.GetProperties(
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			foreach (PropertyInfo pi in props)
			{
				_context.CurrentProperty = pi;

				AbstractTypeBuilderList propertyBuilders = GetBuilders(pi);

				MethodInfo getter = pi.GetGetMethod(true);

				if (getter != null && getter.IsVirtual && getter.IsAbstract == false)
					OverrideGetter(pi, getter, propertyBuilders);

				MethodInfo setter = pi.GetSetMethod(true);

				if (setter != null && setter.IsVirtual && setter.IsAbstract == false)
					OverrideSetter(pi, getter, setter, propertyBuilders);
			}

			_context.CurrentProperty = null;
		}

		private void OverrideGetter(
			PropertyInfo pi, MethodInfo getter, AbstractTypeBuilderList propertyBuilders)
		{
			AbstractTypeBuilderList builders = Combine(
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
				Build(BuildStep.Build,  builders);
				Build(BuildStep.After,  builders);

				EndEmitMethod();
			}
		}

		private void OverrideSetter(
			PropertyInfo pi, MethodInfo getter, MethodInfo setter, AbstractTypeBuilderList propertyBuilders)
		{
			AbstractTypeBuilderList builders = Combine(
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
			MethodInfo[] methods = _context.Type.GetMethods(
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			foreach (MethodInfo method in methods)
			{
				if (method.IsVirtual &&
					method.IsAbstract == false &&
					(method.Attributes & MethodAttributes.SpecialName) == 0 &&
					method.DeclaringType != typeof(object))
				{
					AbstractTypeBuilderList builders = Combine(
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

					_context.BuildElement = BuildElement.InterfaceMethod;
					_context.Step         = BuildStep.Build;
					builder.Build(_context);

					EndEmitMethod();
				}

				_context.CurrentInterface = null;
			}
		}
	}
}
