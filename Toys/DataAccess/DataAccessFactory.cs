using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

using Rsdn.Framework.Data;
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

		private static void DefineCreateInstanceMethod(Type type, TypeBuilder typeBuilder)
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

			MethodBuilder methodBuilder = typeBuilder.DefineMethod(
				"CreateInstance",
				MethodAttributes.Public |
				MethodAttributes.Virtual |
				MethodAttributes.HideBySig,
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

					GenerateMethod(method, methodBuilder);
				}
			}

			return classBuilder.CreateType();
		}

		private static void GenerateMethod(MethodInfo methodInfo, MethodBuilder methodBuilder)
		{
			MapGenerator    gen          = new MapGenerator(methodBuilder.GetILGenerator());
			ParameterInfo[] parameters   = methodInfo.GetParameters();

			BindingFlags    bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			Type            baseType     = typeof(DataAccessorBase);
			Type            objectType   = null;
			ArrayList       paramList    = new ArrayList();

			bool            defManager   = true;
			LocalBuilder    lManager     = gen.DeclareLocal(typeof(DbManager));

			// Process parameters.
			//
			for (int i = 0; i < parameters.Length; i++)
			{
				Type pType = parameters[i].ParameterType;

				if (pType.IsValueType || pType == typeof(string))
				{
					paramList.Add(parameters[i]);
				}
				else if (pType == typeof(DbManager) || pType.IsSubclassOf(typeof(DbManager)))
				{
					if (defManager)
					{
						// Store DbManager.
						//
						defManager = false;
						gen
							.ldarg_s((byte)(parameters[i].Position + 1))
							.stloc(lManager)
							;
					}
				}
			}

			// Define object type.
			//
			if (objectType == null)
			{
				if (methodInfo.ReturnType.IsValueType || methodInfo.ReturnType == typeof(string))
				{

				}
				else
				{
					objectType = methodInfo.ReturnType;
				}
			}

			// Create DbManager.
			//
			if (defManager)
			{
				gen
					.ldarg_0
					.callvirt(baseType, "GetDbManager", bindingFlags)
					.stloc(lManager)
					.BeginExceptionBlock
					.EndGen()
					;
			}

			// Initialize object type.
			//
			LocalBuilder lObjType = gen.DeclareLocal(typeof(Type));

			if (objectType == null)
			{
				gen
					.ldnull
					.stloc(lObjType)
					;
			}
			else
			{
				gen
					.ldtoken(objectType)
					.call(typeof(Type), "GetTypeFromHandle", typeof(RuntimeTypeHandle))
					.stloc(lObjType)
					;
			}

			// Get Sproc name.
			//
			object[] attrs = methodInfo.GetCustomAttributes(typeof(SprocNameAttribute), true);

			if (attrs.Length == 0)
			{
				attrs = methodInfo.GetCustomAttributes(typeof(ActionNameAttribute), true);

				string actionName = attrs.Length == 0?
					methodInfo.Name: ((ActionNameAttribute)attrs[0]).Name;

				// Call GetSpName.
				//
				gen
					.ldloc(lManager)
					.ldarg_0
					.ldloc(lObjType)
					.ldstr(actionName)
					.callvirt(baseType, "GetSpName", bindingFlags, typeof(Type), typeof(string))
					;
			}
			else
			{
				gen
					.ldloc(lManager)
					.ldstr(((SprocNameAttribute)attrs[0]).Name)
					;
			}

			// Parameters.
			//
			LocalBuilder lParams = gen.DeclareLocal(typeof(object[]));

			gen
				.ldc_i4(paramList.Count)
				.newarr(typeof(object))
				.stloc(lParams)
				;

			attrs = methodInfo.DeclaringType.GetCustomAttributes(typeof(DiscoverParametersAttribute), true);

			bool discoverParams = attrs.Length == 0?
				false: ((DiscoverParametersAttribute)attrs[0]).Discover;

			attrs = methodInfo.GetCustomAttributes(typeof(DiscoverParametersAttribute), true);

			if (attrs.Length != 0)
				discoverParams = ((DiscoverParametersAttribute)attrs[0]).Discover;

			for (int i = 0; i < paramList.Count; i++)
			{
				ParameterInfo pi = (ParameterInfo)paramList[i];

				gen
					.ldloc(lParams)
					.ldc_i4(i)
					;

				if (discoverParams)
				{
					gen
						.ldarg_s((byte)(pi.Position + 1))
						.BoxIfValueType(pi.ParameterType)
						;
				}
				else
				{
					attrs = pi.GetCustomAttributes(typeof(ParamNameAttribute), true);

					string paramName = attrs.Length == 0?
						pi.Name: ((ParamNameAttribute)attrs[0]).Name;

					if (paramName[0] != '@')
						paramName = '@' + paramName;

					gen
						.ldloc_0
						.ldstr(paramName)
						.ldarg_s((byte)(pi.Position + 1))
						.BoxIfValueType(pi.ParameterType)
						.callvirt(typeof(DbManager), "Parameter", typeof(string), typeof(object))
						;
				}

				gen
					.stelem_ref
					.EndGen()
					;
			}

			// Call SetSpCommand.
			//
			gen
				.ldloc(lParams) 
				.callvirt(typeof(DbManager), "SetSpCommand", bindingFlags, typeof(string), typeof(object[]))
				;

			// Call Execute Method.
			//
			MethodInfo executeMethod;

			if (methodInfo.ReturnType == objectType)
			{
				executeMethod = typeof(DbManager).GetMethod("ExecuteObject", new Type[] { typeof(Type) });

				gen
					.ldloc(lObjType)
					;
			}
			else
			{
				executeMethod = typeof(DbManager).GetMethod("ExecuteNonQuery");
			}

			LocalBuilder lExec = gen.DeclareLocal(executeMethod.ReturnType);
			LocalBuilder lRet  = gen.DeclareLocal(methodInfo.ReturnType);

			gen
				.callvirt(executeMethod)
				.stloc(lExec)
				;

			// Finally block.
			//
			if (defManager)
			{
				Label fin = gen.DefineLabel();

				gen
					.BeginFinallyBlock
					.ldloc(lManager)
					.brfalse_s(fin)
					.ldloc(lManager)
					.callvirt(typeof(IDisposable).GetMethod("Dispose"))
					.MarkLabel(fin)
					.EndExceptionBlock
					.EndGen()
					;
			}

			// Return Value.
			//
			if (executeMethod.ReturnType == methodInfo.ReturnType ||
				executeMethod.ReturnType.IsSubclassOf(methodInfo.ReturnType))
			{
				gen
					.ldloc(lExec)
					.stloc(lRet)
					;
			}
			else if (methodInfo.ReturnType == objectType)
			{
				gen
					.ldloc(lExec)
					.castclass(objectType)
					.stloc(lRet)
					;
			}

			if (methodInfo.ReturnType != typeof(void))
			{
				gen
					.ldloc(lRet)
					;
			}

			gen
				.ret();
		}
	}
}
