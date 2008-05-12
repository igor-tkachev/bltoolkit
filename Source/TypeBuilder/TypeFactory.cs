using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using BLToolkit.Reflection.Emit;
using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.TypeBuilder
{
	public static class TypeFactory
	{
		static TypeFactory()
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
			if (_globalAssembly != null)
				return;

			if (_saveTypes)
			{
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
		}

		#endregion

		#region GetType

		private static readonly Hashtable _builtTypes = new Hashtable(10);

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

					AssemblyBuilderHelper assemblyBuilder =
						GetAssemblyBuilder(sourceType, typeBuilder.AssemblyNameSuffix);

					type = typeBuilder.Build(sourceType, assemblyBuilder);

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
				throw new TypeBuilderException(string.Format(
					"Could not build the '{0}' type: {1}", sourceType.FullName, ex.Message), ex);
			}
		}

		public static Type GetType(Type sourceType, ITypeBuilder typeBuilder)
		{
			return GetType(sourceType, sourceType, typeBuilder);
		}

		#endregion

		#region Private Helpers

		internal static void Error(string format, params object[] parameters)
		{
			throw new TypeBuilderException(
				string.Format("Could not build the '{0}' type: " + format, parameters));
		}

		private static void WriteDebug(string format, params object[] parameters)
		{
			System.Diagnostics.Debug.WriteLine(string.Format(format, parameters));
		}

		#endregion

		#region Resolve Types

		[Obsolete("Do not call this method directly.")]
		public static void Init()
		{
			// The code has been moved to the type constructor.
		}

		private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			lock (_builtTypes.SyncRoot)
			{
				foreach (Type type in _builtTypes.Keys)
					if (type.FullName == args.Name)
						return type.Assembly;
			}

			int idx = args.Name.IndexOf("." + TypeBuilderConsts.AssemblyNameSuffix);

			if (idx > 0)
			{
				string typeName = args.Name.Substring(0, idx);

				Type type = Type.GetType(typeName);

				if (type == null)
				{
					Assembly[] ass = ((AppDomain)sender).GetAssemblies();

					for (int i = ass.Length - 1; i >= 0; i--)
					{
						Assembly a = ass[i];

						if (!(a is _AssemblyBuilder))
							if (a.CodeBase.IndexOf("Microsoft.NET/Framework") > 0 || a.FullName.StartsWith("System."))
								continue;

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
					Type newType = GetType(type, new AbstractClassBuilder());

					if (newType.Assembly.FullName == args.Name)
						return newType.Assembly;
				}
			}

			return null;
		}

		#endregion
	}
}
