using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

using BLToolkit.Reflection;
using BLToolkit.Reflection.Emit;

namespace BLToolkit.TypeBuilder
{
	public sealed class TypeFactory
	{
		private TypeFactory()
		{
		}

		private static string                _globalAssemblyPath;
		private static AssemblyBuilderHelper _globalAssembly;

		private static AssemblyBuilderHelper GlobalAssemblyBuilder
		{
			get
			{
				if (_globalAssembly == null && _globalAssemblyPath != null)
					_globalAssembly = new AssemblyBuilderHelper(_globalAssemblyPath);

				return _globalAssembly;
			}
		}

		private static bool _saveTypes;
		public  static bool  SaveTypes
		{
			get { return _saveTypes;  }
			set { _saveTypes = value; }
		}

		public static void SetGlobalAssembly(string path)
		{
			if (_globalAssembly != null)
				SaveGlobalAssembly();

			if (path != null || path.Length > 0)
				_globalAssemblyPath = path;
		}

		public static void SaveGlobalAssembly()
		{
			if (_globalAssembly != null)
			{
				_globalAssembly.Save();

				WriteDebug("The global assembly saved in '{0}'.", _globalAssembly.Path);

				_globalAssembly     = null;
				_globalAssemblyPath = null;
			}
		}

		private static Hashtable _builtTypes = Hashtable.Synchronized(new Hashtable(10));

		public static TypeBuilderInfo GetType(Type type, params ITypeBuilder[] defaultBuilders)
		{
			if (type == null) throw new ArgumentNullException("type");

			try
			{
				TypeBuilderInfo info = (TypeBuilderInfo)_builtTypes[type];

				if (info == null)
				{
					lock (_builtTypes.SyncRoot)
					{
						info = (TypeBuilderInfo)_builtTypes[type];

						if (info == null)
						{
							_builtTypes.Add(type, info = new TypeBuilderInfo(type));

							BuildType(info, defaultBuilders);

							info.FinalizeType();

							return info;
						}
					}
				}

				if (info.InProgress)
					lock (_builtTypes.SyncRoot) { }

				return info;
			}
			catch (TypeBuilderException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new TypeBuilderException(
					string.Format("Could not build the '{0}' type: {1}", type.FullName, ex.Message),
					ex);
			}
		}

		private static void Error(string format, params object[] parameters)
		{
			throw new TypeBuilderException(
				string.Format("Could not build the '{0}' type: " + format, parameters));
		}

		private static void WriteDebug(string format, params object[] parameters)
		{
			System.Diagnostics.Debug.WriteLine(string.Format(format, parameters));
		}

		private static ITypeBuilder[] GetBuilderList(TypeHelper type, ITypeBuilder[] defaultBuilders)
		{
			object[]  attrs    = type.GetAttributes(typeof(TypeBuilderAttribute));
			ArrayList builders = new ArrayList(attrs.Length + defaultBuilders.Length);

			foreach (TypeBuilderAttribute attr in attrs)
			{
				if (attr.TypeBuilder == null)
					Error("Attribute '{1}' returns null type builder.", type.FullName, attr.GetType().FullName);

				builders.Add(attr.TypeBuilder);
			}

			foreach (ITypeBuilder tb in defaultBuilders)
			{
				if (tb == null)
					Error("Default builder cannot be null.", type.FullName);

				builders.Add(tb);
			}

			// Check type builders' compatibility.
			//
			for (int i = 0; i < builders.Count; i++)
			{
				ITypeBuilder cur = (ITypeBuilder)builders[i];

				if (cur == null)
					continue;

				for (int j = 0; j < builders.Count; j++)
				{
					ITypeBuilder test = (ITypeBuilder)builders[j];

					if (i == j || test == null)
						continue;

					if (cur.IsCompatible(test) == false)
					{
						WriteDebug(
							"The '{0}' type builder in not compatible with the '{1}'. '{1}' is excluded from the type builders' list",
							cur. GetType().FullName,
							test.GetType().FullName);

						builders[j] = null;
					}
				}
			}

			for (int i = 0; i < builders.Count; i++)
				if (builders[i] == null)
					builders.RemoveAt(i--);

			if (builders.Count == 0)
				Error("No builders found to build the type.", type.FullName);

			return (ITypeBuilder[])builders.ToArray(typeof(ITypeBuilder));
		}

		private static void BuildType(TypeBuilderInfo info, ITypeBuilder[] defaultBuilders)
		{
			if (defaultBuilders == null)
				defaultBuilders = new ITypeBuilder[0];

			ITypeBuilder[] builders = GetBuilderList(info.OriginalType, defaultBuilders);
			
			info.SetTypeBuilders(builders);

			TypeBuilderContext context = new TypeBuilderContext();

			context.Info            = info;
			context.AssemblyBuilder = GetAssemblyBuilder(info.Type);

			if (info.OriginalType.IsAbstract)
				BuildNonAbstractType(context);

			SaveAssembly(context);
		}

		private static AssemblyBuilderHelper GetAssemblyBuilder(Type type)
		{
			AssemblyBuilderHelper ab = GlobalAssemblyBuilder;

			if (ab == null)
			{
				string assemblyDir = Path.GetDirectoryName(type.Module.FullyQualifiedName);

				ab = new AssemblyBuilderHelper(assemblyDir + "\\" + type.FullName + ".TypeBuilder.dll");
			}

			return ab;
		}

		private static void SaveAssembly(TypeBuilderContext context)
		{
			if (_globalAssembly != null)
				return;

			if (_saveTypes)
			{
				try
				{
					context.AssemblyBuilder.Save();

					WriteDebug("The '{0}' type saved in '{1}'.",
						context.Type.FullName,
						context.AssemblyBuilder.Path);
				}
				catch (Exception ex)
				{
					WriteDebug("Can't save the '{0}' assembly for the '{1}' type: {2}.", 
						context.AssemblyBuilder.Path,
						context.Type.FullName,
						ex.Message);
				}
			}
		}

		private static ArrayList GetAbstractBuilders(TypeBuilderContext context)
		{
			ArrayList builders = new ArrayList();

			foreach (ITypeBuilder tb in context.Info.TypeBuilders)
				if (tb is IAbstractTypeBuilder)
					builders.Add(tb);

			if (builders.Count == 0)
				Error("No builders found to build the abstract type.", context.Type.FullName);

			return builders;
		}

		private static void DefineNonAbstractType(TypeBuilderContext context, ArrayList abstractBuilders)
		{
			ArrayList interfaces = new ArrayList();

			foreach (IAbstractTypeBuilder tb in abstractBuilders)
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
							Error("The '{1}' must be an interface.", context.Type.FullName, t.FullName);
						}
						else if (interfaces.Contains(t) == false)
						{
							interfaces.Add(t);
							context.InterfaceMap.Add(t, tb);
						}
					}
				}
			}

			string typeName = context.Type.FullName.Replace('+', '.');

			typeName = typeName.Substring(0, typeName.Length - context.Type.Name.Length);
			typeName = typeName + "BLToolkitExtension." + context.Type.Name;

			context.TypeBuilder = context.AssemblyBuilder.DefineType(
				typeName,
				TypeAttributes.Public | TypeAttributes.BeforeFieldInit,
				context.Type,
				(Type[])interfaces.ToArray(typeof(Type)));

			context.Info.SetType(context.TypeBuilder);

			if (context.Type.IsSerializable)
				context.TypeBuilder.SetCustomAttribute(typeof(SerializableAttribute));
		}

		private static void DefineInterfaces (TypeBuilderContext context)
		{
			foreach (DictionaryEntry de in context.InterfaceMap)
			{
				context.CurrentInterface = (Type)de.Key;

				MethodInfo[] methods = context.CurrentInterface.GetMethods();

				foreach (MethodInfo m in methods)
				{
					context.MethodBuilder = context.TypeBuilder.DefineMethod(m);

					EmitHelper emit = context.MethodBuilder.Emitter;

					// Label right before return.
					//
					context.ReturnLabel = emit.DefineLabel();

					// Create return value.
					//
					if (m.ReturnType != typeof(void))
						context.ReturnValue = context.MethodBuilder.Emitter.DeclareLocal(m.ReturnType);

					// Call builder to build the method.
					//
					IAbstractTypeBuilder builder = (IAbstractTypeBuilder)de.Value;

					builder.BuildInterfaceMethod(context);

					// Prepare return.
					//
					emit.MarkLabel(context.ReturnLabel);

					if (context.ReturnValue != null)
						emit.ldloc(context.ReturnValue);

					emit.ret();

					// Cleanup the context.
					//
					context.ReturnValue   = null;
					context.MethodBuilder = null;
				}

				context.CurrentInterface = null;
			}
		}

		private static void BuildNonAbstractType(TypeBuilderContext context)
		{
			ArrayList builders = GetAbstractBuilders(context);

			DefineNonAbstractType(context, builders);

			foreach (IAbstractTypeBuilder tb in builders)
				tb.BeforeBuild(context);

			DefineInterfaces(context);

			foreach (IAbstractTypeBuilder tb in builders)
				tb.AfterBuild(context);

			context.Info.SetType(context.TypeBuilder.Create());
		}
	}
}
