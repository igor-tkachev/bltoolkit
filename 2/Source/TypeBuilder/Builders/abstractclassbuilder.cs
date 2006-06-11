using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

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
		private AbstractTypeBuilderList _usedBuilders = new AbstractTypeBuilderList();

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

			MethodInfo initMethod = _context.Type.GetMethod("InitInstance", typeof(InitContext));

			// Finalize constructors.
			//
			if (_context.TypeBuilder.IsDefaultConstructorDefined)
			{
				if (initMethod != null)
					_context.TypeBuilder.DefaultConstructor.Emitter
						.ldarg_0
						.ldnull
						.callvirt (initMethod)
						;

				_context.TypeBuilder.DefaultConstructor.Emitter.ret();
			}

			if (_context.TypeBuilder.IsInitConstructorDefined)
			{
				if (initMethod != null)
					_context.TypeBuilder.InitConstructor.Emitter
						.ldarg_0
						.ldarg_1
						.callvirt (initMethod)
						;

				_context.TypeBuilder.InitConstructor.Emitter.ret();
			}

			if (_context.TypeBuilder.IsTypeInitializerDefined)
				_context.TypeBuilder.TypeInitializer.Emitter.ret();

			// Create the type.
			//
			return _context.TypeBuilder.Create();
		}

		private static void CheckCompatibility(BuildContext context, AbstractTypeBuilderList builders)
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
				TypeAttributes.Public
				| TypeAttributes.BeforeFieldInit
				| (TypeFactory.SealTypes? TypeAttributes.Sealed: 0),
				_context.Type,
				(Type[])interfaces.ToArray(typeof(Type)));

			if (_context.Type.IsSerializable)
				_context.TypeBuilder.SetCustomAttribute(typeof(SerializableAttribute));

			/*
			Type type = typeof(XmlTypeAttribute);

			_context.TypeBuilder.SetCustomAttribute(
				type,
				new PropertyInfo[] {
					type.GetProperty("IncludeInSchema"),
					type.GetProperty("Namespace"),
					type.GetProperty("TypeName")
				},
				new object[] {
					false,
					"urn:bltoolkit",
					_context.Type.Name + "_"
				});

			type = typeof(XmlRootAttribute);

			_context.TypeBuilder.SetCustomAttribute(
				type,
				new PropertyInfo[] {
					type.GetProperty("Namespace"),
					type.GetProperty("ElementName")
				},
				new object[] {
					"urn:bltoolkit",
					_context.Type.Name + "_"
				});
			*/
		}

#if FW2
		class BuilderComparer : IComparer<IAbstractTypeBuilder>
		{
			public BuilderComparer(BuildContext context)
			{
				_context = context;
			}

			BuildContext _context;

			[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
			public int Compare(IAbstractTypeBuilder x, IAbstractTypeBuilder y)
			{
				return y.GetPriority(_context) - x.GetPriority(_context);
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
			{
				IAbstractTypeBuilder builder = _context.TypeBuilders[i];

				builder.Build(_context);

				if (!_usedBuilders.Contains(builder))
					_usedBuilders.Add(builder);
			}
		}

		private void BeginEmitMethod(MethodInfo method)
		{
			_context.CurrentMethod = method;
			_context.MethodBuilder = _context.TypeBuilder.DefineMethod(method);

			EmitHelper emit = _context.MethodBuilder.Emitter;

			// Label right before return and catch block.
			//
			_context.ReturnLabel = emit.DefineLabel();

			// Create return value.
			//
			if (method.ReturnType != typeof(void))
			{
				_context.ReturnValue = _context.MethodBuilder.Emitter.DeclareLocal(method.ReturnType);
				emit.Init(_context.ReturnValue);
			}

			// Initialize out parameters.
			//
			ParameterInfo[] parameters = method.GetParameters();

			if (parameters != null)
				emit.InitOutParameters(parameters);

			_usedBuilders.Clear();
		}

		private void EmitMethod(
			AbstractTypeBuilderList builders, MethodInfo methdoInfo, BuildElement buildElement)
		{
			_context.BuildElement = buildElement;

			bool isCatchBlockRequired   = false;
			bool isFinallyBlockRequired = false;

			foreach (IAbstractTypeBuilder builder in builders)
			{
				isCatchBlockRequired   = isCatchBlockRequired   || IsApplied(builder, BuildStep.Catch);
				isFinallyBlockRequired = isFinallyBlockRequired || IsApplied(builder, BuildStep.Finally);
			}

			BeginEmitMethod(methdoInfo);

			Build(BuildStep.Begin,  builders);

			EmitHelper emit        = _context.MethodBuilder.Emitter;
			Label      returnLabel = _context.ReturnLabel;

			// Begin catch block.
			//

			if (isCatchBlockRequired || isFinallyBlockRequired)
			{
				_context.ReturnLabel = emit.DefineLabel();
				emit.BeginExceptionBlock();
			}

			Build(BuildStep.Before, builders);
			Build(BuildStep.Build,  builders);
			Build(BuildStep.After,  builders);

			if (isCatchBlockRequired || isFinallyBlockRequired)
			{
				emit.MarkLabel(_context.ReturnLabel);
				_context.ReturnLabel = returnLabel;
			}

			// End catch block.
			//
			if (isCatchBlockRequired)
			{
				emit
					.BeginCatchBlock(typeof(Exception));

				_context.ReturnLabel = emit.DefineLabel();
				_context.Exception   = emit.DeclareLocal(typeof(Exception));

				emit
					.stloc (_context.Exception);

				Build(BuildStep.Catch, builders);

				emit
					.rethrow
					.end();

				emit.MarkLabel(_context.ReturnLabel);
				_context.ReturnLabel = returnLabel;
				_context.Exception   = null;
			}

			if (isFinallyBlockRequired)
			{
				emit.BeginFinallyBlock();
				_context.ReturnLabel = emit.DefineLabel();

				Build(BuildStep.Finally, builders);

				emit.MarkLabel(_context.ReturnLabel);
				_context.ReturnLabel = returnLabel;
			}

			if (isCatchBlockRequired || isFinallyBlockRequired)
				emit.EndExceptionBlock();

			Build(BuildStep.End, _usedBuilders);

			EndEmitMethod();
		}

		private void EndEmitMethod()
		{
			_usedBuilders.Clear();

			EmitHelper emit = _context.MethodBuilder.Emitter;

			// Prepare return.
			//
			emit.MarkLabel(_context.ReturnLabel);

			if (_context.ReturnValue != null)
				emit.ldloc(_context.ReturnValue);

			emit.ret();

			// Cleanup the context.
			//
			_context.ReturnValue   = null;
			_context.CurrentMethod = null;
			_context.MethodBuilder = null;
		}

		private static AbstractTypeBuilderList GetBuilders(object[] attributes, object target)
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

		private static AbstractTypeBuilderList GetBuilders(MemberInfo memberInfo)
		{
			return GetBuilders(
				memberInfo.GetCustomAttributes(typeof(AbstractTypeBuilderAttribute), true), memberInfo);
		}

		private static AbstractTypeBuilderList GetBuilders(ParameterInfo parameterInfo)
		{
			return GetBuilders(
				parameterInfo.GetCustomAttributes(typeof(AbstractTypeBuilderAttribute), true), parameterInfo);
		}

		private static AbstractTypeBuilderList GetBuilders(ParameterInfo[] parameters)
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

		private static AbstractTypeBuilderList Combine(params AbstractTypeBuilderList[] builders)
		{
			AbstractTypeBuilderList list = new AbstractTypeBuilderList();

			foreach (AbstractTypeBuilderList l in builders)
				list.AddRange(l);

			return list;
		}

		private bool IsApplied(IAbstractTypeBuilder builder, BuildStep buildStep)
		{
			_context.Step = buildStep;
			return builder.IsApplied(_context);
		}

		private bool IsApplied(BuildElement element, AbstractTypeBuilderList builders)
		{
			_context.BuildElement = element;

			foreach (IAbstractTypeBuilder builder in builders)
			{
				if (IsApplied(builder, BuildStep.Before))  return true;
				if (IsApplied(builder, BuildStep.Build))   return true;
				if (IsApplied(builder, BuildStep.After))   return true;
				if (IsApplied(builder, BuildStep.Catch))   return true;
				if (IsApplied(builder, BuildStep.Finally)) return true;
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
					DefineAbstractSetter(pi, setter, propertyBuilders);
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

			EmitMethod(builders, getter, BuildElement.AbstractGetter);
		}

		private void DefineAbstractSetter(
			PropertyInfo            propertyInfo,
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

			EmitMethod(builders, setter, BuildElement.AbstractSetter);
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

					EmitMethod(builders, method, BuildElement.AbstractMethod);
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
					OverrideGetter(getter, propertyBuilders);

				MethodInfo setter = pi.GetSetMethod(true);

				if (setter != null && setter.IsVirtual && setter.IsAbstract == false)
					OverrideSetter(setter, propertyBuilders);
			}

			_context.CurrentProperty = null;
		}

		private void OverrideGetter(MethodInfo getter, AbstractTypeBuilderList propertyBuilders)
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
				EmitMethod(builders, getter, BuildElement.VirtualGetter);
		}

		private void OverrideSetter(MethodInfo setter, AbstractTypeBuilderList propertyBuilders)
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
				EmitMethod(builders, setter, BuildElement.VirtualSetter);
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
						EmitMethod(builders, method, BuildElement.VirtualMethod);
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
