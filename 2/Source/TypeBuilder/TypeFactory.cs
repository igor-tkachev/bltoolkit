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

		internal static void Error(string format, params object[] parameters)
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

			BuildContext context = new BuildContext();

			context.Info            = info;
			context.AssemblyBuilder = GetAssemblyBuilder(info.Type);

			if (info.OriginalType.IsAbstract)
				new AbstractClassBuilder(context).Build();

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

		private static void SaveAssembly(BuildContext context)
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
	}
}
