using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Security.Permissions;

using BLToolkit.Reflection.Emit;
using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.TypeBuilder
{
	public sealed class TypeFactory
	{
		private TypeFactory()
		{
		}

		#region Create Assembly

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

		private static bool _sealTypes = true;
		public  static bool  SealTypes
		{
			get { return _sealTypes;  }
			set { _sealTypes = value; }
		}

		[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
		public static void SetGlobalAssembly(string path)
		{
			if (_globalAssembly != null)
				SaveGlobalAssembly();

			if (path != null && path.Length > 0)
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

		private static AssemblyBuilderHelper GetAssemblyBuilder(Type type, string suffix)
		{
			AssemblyBuilderHelper ab = GlobalAssemblyBuilder;

			if (ab == null)
			{
				string assemblyDir = AppDomain.CurrentDomain.BaseDirectory;

				try
				{
					if (type.Module.FullyQualifiedName != null &&
						type.Module.FullyQualifiedName.Length > 0 &&
						type.Module.FullyQualifiedName != "<Unknown>")
					{
						assemblyDir = Path.GetDirectoryName(type.Module.FullyQualifiedName);
					}
				}
				catch
				{
				}

				string fullName = type.FullName;

#if FW2
				if (type.IsGenericType)
					fullName = AbstractClassBuilder.GetTypeFullName(type);
#endif

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

		private static Hashtable _builtTypes = new Hashtable(10);

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

		private static bool _isInit;

		[EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public static void Init()
		{
			if (_isInit == false)
			{
				_isInit = true;

				AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
			}
		}

		private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			lock (_builtTypes.SyncRoot)
			{
				foreach (Type type in _builtTypes.Keys)
					if (type.FullName == args.Name)
						return type.Assembly;
			}

			int idx = args.Name.IndexOf("." + TypeBuilderConsts.AssemblyNameSuffix + ".dll");

			if (idx > 0)
			{
				string typeName = args.Name.Substring(0, idx);

				Type type = Type.GetType(typeName);

				if (type == null)
				{
					foreach (Assembly a in ((AppDomain)sender).GetAssemblies())
					{
						if (a.CodeBase.IndexOf("Microsoft.NET/Framework") > 0)
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

		static TypeFactory()
		{
			Init();
		}

		#endregion
	}
}
