using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

//using Rsdn.Framework.Data;
using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.DataAccess
{
	public class DataAccessFactory
	{
		private static Hashtable _typeList = Hashtable.Synchronized(new Hashtable(10));

		public static object CreateInstance(Type type)
		{
			IObjectFactory factory = (IObjectFactory)_typeList[type];

			if (factory == null)
			{
				lock (_typeList.SyncRoot)
				{
					factory = (IObjectFactory)_typeList[type];

					if (factory == null)
					{
						factory = CreateFactory(type);
						_typeList[type] = factory;
					}
				}
			}

			return factory.CreateInstance();
		}

#if VER2
		public static T CreateInstance<T>()
		{
			return (T)CreateInstance(typeof(T));
		}
#endif

		private static IObjectFactory CreateFactory(Type type)
		{
			AssemblyName assemblyName = new AssemblyName();
			string       assemblyDir  = Path.GetDirectoryName(type.Module.FullyQualifiedName);
			string       fullName     = type.FullName.Replace('+', '.');

			assemblyName.Name = fullName + ".DataAccessor.dll";

			AssemblyBuilder assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(
				assemblyName,
				AssemblyBuilderAccess.RunAndSave,
				assemblyDir);

			ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

			if (type.IsAbstract)
				type = CreateNonAbstractType(type, moduleBuilder);

			TypeBuilder typeBuilder = moduleBuilder.DefineType(
				fullName + ".DataAccessor.$ObjectFactory",
				TypeAttributes.Public,
				typeof(object),
				new Type[] { typeof(IObjectFactory) });

			DefineCreateInstanceMethod(type, typeBuilder);

			Type factoryType = typeBuilder.CreateType();

			IObjectFactory factory = (IObjectFactory)Activator.CreateInstance(factoryType);

#if DEBUG
			try
			{
				assemblyBuilder.Save(assemblyName.Name);

				System.Diagnostics.Debug.WriteLine(
					string.Format("The '{0}' type saved in '{1}\\{2}'.",
					fullName,
					assemblyDir,
					assemblyName.Name));
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(
					string.Format("Can't save the '{0}' assembly for the '{1}' type in '{2}': {3}.", 
					assemblyName.Name,
					fullName,
					assemblyDir,
					ex.Message));
			}
#endif

			return factory;
		}

		internal static ConstructorInfo GetDefaultConstructor(Type type)
		{
			ConstructorInfo ci = type.GetConstructor(
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
				null,
				Type.EmptyTypes,
				null);

			if (ci == null || !ci.IsPublic)
			{
				throw new RsdnDataAccessException(
					string.Format("The '{0}' type must have the public default constructor.", type.Name));
			}

			return ci;
		}

		private static void DefineCreateInstanceMethod(Type type, TypeBuilder typeBuilder)
		{
			ConstructorInfo ci = GetDefaultConstructor(type);

			MethodBuilder methodBuilder = typeBuilder.DefineMethod(
				"CreateInstance",
				MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
				typeof(object),
				Type.EmptyTypes);

			typeBuilder.DefineMethodOverride(
				methodBuilder,
				typeof(IObjectFactory).GetMethod("CreateInstance"));

			MapGenerator gen = new MapGenerator(methodBuilder.GetILGenerator());

			gen
				.newobj(ci)
				.ret();
		}

		private static Type CreateNonAbstractType(Type type, ModuleBuilder moduleBuilder)
		{
			TypeBuilder classBuilder = moduleBuilder.DefineType(
				type.FullName.Replace('+', '.') + ".DataAccessor." + type.Name,
				TypeAttributes.Public | TypeAttributes.BeforeFieldInit,
				type);

			MemberInfo[] methods = type.GetMethods();

			foreach (MethodInfo method in methods)
			{
				if (method.IsAbstract)
				{
					ParameterInfo[] parameters = method.GetParameters();
					Type[]          pTypes     = new Type[parameters.Length];

					for (int i = 0; i < parameters.Length; i++)
						pTypes[i] = parameters[i].ParameterType;

					MethodBuilder methodBuilder = classBuilder.DefineMethod(
						method.Name,
						method.Attributes & ~MethodAttributes.Abstract,
						method.ReturnType,
						pTypes);

					classBuilder.DefineMethodOverride(methodBuilder, method);

					new MethodGenerator().Generate(method, methodBuilder);
				}
			}

			return classBuilder.CreateType();
		}
	}
}
