using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

#if FW3
using System.Linq.Expressions;
#endif

using BLToolkit.Reflection;
using BLToolkit.Reflection.Emit;
using BLToolkit.TypeBuilder.Builders;
using BLToolkit.Properties;
using BLToolkit.Configuration;

namespace BLToolkit.TypeBuilder
{
	public static class TypeFactory
	{
		static TypeFactory()
		{
			BLToolkitSection section = BLToolkitSection.Instance;

			if (section != null)
			{
				TypeFactoryElement elm = section.TypeFactory;

				if (elm != null)
				{
					SaveTypes = elm.SaveTypes;
					SealTypes = elm.SealTypes;
					LoadTypes = elm.LoadTypes;

					SetGlobalAssembly(elm.AssemblyPath, elm.Version, elm.KeyFile);
				}
			}

			SecurityPermission perm = new SecurityPermission(SecurityPermissionFlag.ControlAppDomain);

#if FW4
			try
			{
				//var permissionSet = new PermissionSet(PermissionState.None);
				//permissionSet.AddPermission(perm);

				//if (permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
					SubscribeAssemblyResolver();
			}
			catch
			{
			}
#else
			if (SecurityManager.IsGranted(perm))
				SubscribeAssemblyResolver();
#endif
		}

		static void SubscribeAssemblyResolver()
		{
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
		}

		#region Create Assembly

		private static string                _globalAssemblyPath;
		private static string                _globalAssemblyKeyFile;
		private static Version               _globalAssemblyVersion;
		private static AssemblyBuilderHelper _globalAssembly;

		private static AssemblyBuilderHelper GlobalAssemblyBuilder
		{
			get
			{
				if (_globalAssembly == null && _globalAssemblyPath != null)
					_globalAssembly = new AssemblyBuilderHelper(_globalAssemblyPath, _globalAssemblyVersion, _globalAssemblyKeyFile);

				return _globalAssembly;
			}
		}

		private static bool _saveTypes;
		public  static bool  SaveTypes
		{
			get { return _saveTypes;  }
			set { _saveTypes = value; }
		}

		private static bool _sealTypes = true;
		public  static bool  SealTypes
		{
			get { return _sealTypes;  }
			set { _sealTypes = value; }
		}

		[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
		public static void SetGlobalAssembly(string path)
		{
			SetGlobalAssembly(path, null, null);
		}

		[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
		public static void SetGlobalAssembly(string path, Version version, string keyFile)
		{
			if (_globalAssembly != null)
				SaveGlobalAssembly();

			if (!string.IsNullOrEmpty(path))
				_globalAssemblyPath = path;

			_globalAssemblyVersion = version;
			_globalAssemblyKeyFile = keyFile;
		}

		public static void SaveGlobalAssembly()
		{
			if (_globalAssembly != null)
			{
				_globalAssembly.Save();

				WriteDebug("The global assembly saved in '{0}'.", _globalAssembly.Path);

				_globalAssembly        = null;
				_globalAssemblyPath    = null;
				_globalAssemblyVersion = null;
				_globalAssemblyKeyFile = null;
			}
		}

		private static AssemblyBuilderHelper GetAssemblyBuilder(Type type, string suffix)
		{
			AssemblyBuilderHelper ab = GlobalAssemblyBuilder;

			if (ab == null)
			{
				string assemblyDir = AppDomain.CurrentDomain.BaseDirectory;

				// Dynamic modules are locationless, so ignore them.
				// _ModuleBuilder is the base type for both
				// ModuleBuilder and InternalModuleBuilder classes.
				//
				if (!(type.Module is _ModuleBuilder))
					assemblyDir = Path.GetDirectoryName(type.Module.FullyQualifiedName);

				string fullName = type.FullName;

				if (type.IsGenericType)
					fullName = AbstractClassBuilder.GetTypeFullName(type);

				ab = new AssemblyBuilderHelper(assemblyDir + "\\" + fullName + "." + suffix + ".dll");
			}

			return ab;
		}

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		private static void SaveAssembly(AssemblyBuilderHelper assemblyBuilder, Type type)
		{
			if (!_saveTypes || _globalAssembly != null)
				return;
			try
			{
				assemblyBuilder.Save();

				WriteDebug("The '{0}' type saved in '{1}'.",
							type.FullName,
							assemblyBuilder.Path);
			}
			catch (Exception ex)
			{
				WriteDebug("Can't save the '{0}' assembly for the '{1}' type: {2}.",
							assemblyBuilder.Path,
							type.FullName,
							ex.Message);
			}
		}

		#endregion

		#region GetType

		private static readonly Hashtable _builtTypes = new Hashtable(10);
		private static readonly Hashtable _assemblies = new Hashtable(10);

		private static bool _loadTypes;
		public  static bool  LoadTypes
		{
			get { return _loadTypes;  }
			set { _loadTypes = value; }
		}

		public static Type GetType(object hashKey, Type sourceType, ITypeBuilder typeBuilder)
		{
			if (hashKey     == null) throw new ArgumentNullException("hashKey");
			if (sourceType  == null) throw new ArgumentNullException("sourceType");
			if (typeBuilder == null) throw new ArgumentNullException("typeBuilder");

			try
			{
				Hashtable builderTable = (Hashtable)_builtTypes[typeBuilder.GetType()];
				Type      type;

				if (builderTable != null)
				{
					type = (Type)builderTable[hashKey];

					if (type != null)
						return type;
				}

				lock (_builtTypes.SyncRoot)
				{
					builderTable = (Hashtable)_builtTypes[typeBuilder.GetType()];

					if (builderTable != null)
					{
						type = (Type)builderTable[hashKey];

						if (type != null)
							return type;
					}
					else
					{
						_builtTypes.Add(typeBuilder.GetType(), builderTable = new Hashtable());
					}

					if (_loadTypes)
					{
						Assembly originalAssembly = sourceType.Assembly;
						Assembly extensionAssembly;

						if (_assemblies.Contains(originalAssembly))
							extensionAssembly = (Assembly)_assemblies[originalAssembly];
						else
						{
							extensionAssembly = LoadExtensionAssembly(originalAssembly);
							_assemblies.Add(originalAssembly, extensionAssembly);
						}

						if (extensionAssembly != null)
						{
							type = extensionAssembly.GetType(typeBuilder.GetTypeName());

							if (type != null)
							{
								builderTable.Add(hashKey, type);
								return type;
							}
						}
					}

					AssemblyBuilderHelper assemblyBuilder = GetAssemblyBuilder(sourceType, typeBuilder.AssemblyNameSuffix);

					type = typeBuilder.Build(assemblyBuilder);

					if (type != null)
					{
						builderTable.Add(hashKey, type);
						SaveAssembly(assemblyBuilder, type);
					}

					return type;
				}
			}
			catch (TypeBuilderException)
			{
				throw;
			}
			catch (Exception ex)
			{
				// Convert an Exception to TypeBuilderException.
				//
				throw new TypeBuilderException(
					string.Format(Resources.TypeFactory_BuildFailed, sourceType.FullName), ex);
			}
		}

		public static Type GetType(Type sourceType)
		{
			return
				TypeHelper.IsScalar(sourceType) || sourceType.IsSealed ||
						(!sourceType.IsAbstract && sourceType.IsDefined(typeof(BLToolkitGeneratedAttribute), true)) ?
					sourceType:
					GetType(sourceType, sourceType, new AbstractClassBuilder(sourceType));
		}

#if FW3
		static class InstanceCreator<T>
		{
			public static readonly Func<T> CreateInstance =
				Expression.Lambda<Func<T>>(Expression.New(TypeFactory.GetType(typeof(T)))).Compile();
		}
#endif

		public static T CreateInstance<T>() where T: class
		{
#if FW3
			return InstanceCreator<T>.CreateInstance();
#else
			return (T)Activator.CreateInstance(GetType(typeof(T)));
#endif
		}

		#endregion

		#region Private Helpers

		private static Assembly LoadExtensionAssembly(Assembly originalAssembly)
		{
			if (originalAssembly is _AssemblyBuilder)
			{
				// This is a generated assembly. Even if it has a valid Location,
				// there is definitelly no extension assembly at this path.
				//
				return null;
			}

			try
			{
				string  originalAssemblyLocation = new Uri(originalAssembly.EscapedCodeBase).LocalPath;
				string extensionAssemblyLocation = Path.ChangeExtension(
					originalAssemblyLocation, "BLToolkitExtension.dll");

				if (File.GetLastWriteTime(originalAssemblyLocation) <= File.GetLastWriteTime(extensionAssemblyLocation))
					return Assembly.LoadFrom(extensionAssemblyLocation);

				Debug.WriteLineIf(File.Exists(extensionAssemblyLocation),
					string.Format("Extension assembly '{0}' is out of date. Please rebuild.",
						extensionAssemblyLocation), typeof(TypeAccessor).FullName);

				// Some good man may load this assembly already. Like IIS does it.
				//
				AssemblyName extensionAssemblyName = originalAssembly.GetName(true);
				extensionAssemblyName.Name += ".BLToolkitExtension";
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					// Note that assembly version and strong name are compared too.
					//
					if (AssemblyName.ReferenceMatchesDefinition(assembly.GetName(false), extensionAssemblyName))
						return assembly;
				}
			}
			catch (Exception ex)
			{
				// Extension exist, but can't be loaded for some reason.
				// Switch back to code generation
				//
				Debug.WriteLine(ex, typeof(TypeAccessor).FullName);
			}

			return null;
		}

		[System.Diagnostics.Conditional("DEBUG")]
		private static void WriteDebug(string format, params object[] parameters)
		{
			System.Diagnostics.Debug.WriteLine(string.Format(format, parameters));
		}

		#endregion

		#region Resolve Types

		/// <summary>
		/// Initializes AssemblyResolve hooks for the current <see cref="AppDomain"/>.
		/// </summary>
		public static void Init()
		{
			//
			// The code actually does nothing except an implicit call to the type constructor.
			//
		}

		private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			string   name      = args.Name;
			string[] nameParts = name.Split(',');

			if (nameParts.Length > 0 && nameParts[0].ToLower().EndsWith(".dll"))
			{
				nameParts[0] = nameParts[0].Substring(0, nameParts[0].Length - 4);
				name         = string.Join(",", nameParts);
			}

			lock (_builtTypes.SyncRoot)
			{
				foreach (Type type in _builtTypes.Keys)
					if (type.FullName == name)
						return type.Assembly;
			}

			int idx = name.IndexOf("." + TypeBuilderConsts.AssemblyNameSuffix);

			if (idx > 0)
			{
				string typeName = name.Substring(0, idx);

				Type type = Type.GetType(typeName);

				if (type == null)
				{
					Assembly[] ass = ((AppDomain)sender).GetAssemblies();

					// CLR can't find an assembly built on previous AssemblyResolve event.
					//
					for (int i = ass.Length - 1; i >= 0; i--)
					{
						if (string.Compare(ass[i].FullName, name) == 0)
							return ass[i];
					}

					for (int i = ass.Length - 1; i >= 0; i--)
					{
						Assembly a = ass[i];

						if (!(a is _AssemblyBuilder) &&
							(a.CodeBase.IndexOf("Microsoft.NET/Framework") > 0 || a.FullName.StartsWith("System."))) continue;

						type = a.GetType(typeName);

						if (type != null) break;

						foreach (Type t in a.GetTypes())
						{
							if (!t.IsAbstract)
								continue;

							if (t.FullName == typeName)
							{
								type = t;
							}
							else
							{
								if (t.FullName.IndexOf('+') > 0)
								{
									string s = typeName;

									while (type == null && (idx = s.LastIndexOf(".")) > 0)
									{
										s = s.Remove(idx, 1).Insert(idx, "+");

										if (t.FullName == s)
											type = t;
									}
								}
							}

							if (type != null) break;
						}

						if (type != null) break;
					}
				}

				if (type != null)
				{
					Type newType = GetType(type);

					if (newType.Assembly.FullName == name)
						return newType.Assembly;
				}
			}

			return null;
		}

		#endregion
	}
}
