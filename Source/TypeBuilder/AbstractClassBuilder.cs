using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

using BLToolkit.Reflection.Emit;

namespace BLToolkit.TypeBuilder
{
	class AbstractClassBuilder
	{
		public AbstractClassBuilder(BuildContext context)
		{
			_context = context;
		}

		private BuildContext _context;
		private ArrayList          _builders;

		public void Build()
		{
			GetAbstractBuilders();
			DefineNonAbstractType();

			Build(BuildOperation.BeforeBuild, _builders);

			DefineAbstractProperties();
			DefineAbstractMethods();
			OverrideVirtualProperties();
			OverrideVirtualMethods();
			DefineInterfaces();

			Build(BuildOperation.AfterBuild, _builders);

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
			_context.Info.SetType(_context.TypeBuilder.Create());
		}

		private void GetAbstractBuilders()
		{
			_builders = new ArrayList();

			foreach (ITypeBuilder tb in _context.Info.TypeBuilders)
				if (tb is IAbstractTypeBuilder)
					_builders.Add(tb);
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

			_context.Info.SetType(_context.TypeBuilder);

			if (_context.Type.IsSerializable)
				_context.TypeBuilder.SetCustomAttribute(typeof(SerializableAttribute));
		}

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

		private void Build(BuildOperation operation, ArrayList builders)
		{
			_context.BuildOperation = operation;

			builders.Sort(new BuilderComparer(operation));

			foreach (IAbstractTypeBuilder tb in builders)
				tb.Build(_context);
		}

		private void BeginEmitMethod(MethodInfo method)
		{
			_context.MethodBuilder = _context.TypeBuilder.DefineMethod(method);

			EmitHelper emit = _context.MethodBuilder.Emitter;

			// Label right before return.
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
			emit.InitOutParameters(method.GetParameters());
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
			_context.ReturnValue   = null;
			_context.MethodBuilder = null;
		}

		private void DefineAbstractProperties()
		{
		}

		private void DefineAbstractMethods()
		{
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
