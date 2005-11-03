using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

using BLToolkit.Reflection;
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

		private static AssemblyBuilderHelper GetAssemblyBuilder(Type type, string suffix)
		{
			AssemblyBuilderHelper ab = GlobalAssemblyBuilder;

			if (ab == null)
			{
				string assemblyDir = Path.GetDirectoryName(type.Module.FullyQualifiedName);

				ab = new AssemblyBuilderHelper(assemblyDir + "\\" + type.FullName + "." + suffix + ".dll");
			}

			return ab;
		}

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

		public static Type GetType(Type sourceType, ITypeBuilder typeBuilder)
		{
			if (sourceType  == null) throw new ArgumentNullException("sourceType");
			if (typeBuilder == null) throw new ArgumentNullException("typeBuilder");

			try
			{
				Hashtable builderTable = (Hashtable)_builtTypes[typeBuilder.GetType()];
				Type      type;

				if (builderTable != null)
				{
					type = (Type)builderTable[sourceType];

					if (type != null)
						return type;
				}

				lock (_builtTypes.SyncRoot)
				{
					builderTable = (Hashtable)_builtTypes[typeBuilder.GetType()];

					if (builderTable != null)
					{
						type = (Type)builderTable[sourceType];

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

					builderTable.Add(sourceType, type);

					SaveAssembly(assemblyBuilder, type);

					return type;
				}
			}
			catch (TypeBuilderException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new TypeBuilderException(
					string.Format("Could not build the '{0}' type: {1}", sourceType.FullName, ex.Message),
					ex);
			}
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
	}
}

