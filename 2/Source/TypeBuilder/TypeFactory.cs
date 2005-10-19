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

		public static BuildContext GetType(Type type, params ITypeBuilder[] defaultBuilders)
		{
			if (type == null) throw new ArgumentNullException("type");

			try
			{
				BuildContext context = (BuildContext)_builtTypes[type];

				if (context == null)
				{
					lock (_builtTypes.SyncRoot)
					{
						context = (BuildContext)_builtTypes[type];

						if (context == null)
						{
							_builtTypes.Add(type, context = new BuildContext(type));

							BuildType(context, defaultBuilders);

							context.InProgress = false;

							return context;
						}
					}
				}

				if (context.InProgress)
					lock (_builtTypes.SyncRoot) { }

				return context;
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

		private static void BuildType(BuildContext context, ITypeBuilder[] defaultBuilders)
		{
			if (defaultBuilders == null)
				defaultBuilders = new ITypeBuilder[0];

			TypeBuilderList builders = GetBuilderList(context.OriginalType, defaultBuilders);

			context.TypeBuilders    = builders;
			context.AssemblyBuilder = GetAssemblyBuilder(context.OriginalType);

			if (context.OriginalType.IsAbstract)
				new AbstractClassBuilder(context).Build();

			SaveAssembly(context);
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

		private static TypeBuilderList GetBuilderList(TypeHelper type, ITypeBuilder[] defaultBuilders)
		{
			object[]        attrs    = type.GetAttributes(typeof(TypeBuilderAttribute));
			TypeBuilderList builders = new TypeBuilderList(attrs.Length + defaultBuilders.Length);

			foreach (TypeBuilderAttribute attr in attrs)
				if (attr.TypeBuilder != null)
					builders.Add(attr.TypeBuilder);

			foreach (ITypeBuilder tb in defaultBuilders)
				if (tb != null)
					builders.Add(tb);

			return builders;
		}

		internal static void CheckCompatibility(BuildContext context, TypeBuilderList builders)
		{
			for (int i = 0; i < builders.Count; i++)
			{
				ITypeBuilder cur = builders[i];

				if (cur == null)
					continue;

				for (int j = 0; j < builders.Count; j++)
				{
					ITypeBuilder test = builders[j];

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
						context.OriginalType.FullName,
						context.AssemblyBuilder.Path);
				}
				catch (Exception ex)
				{
					WriteDebug("Can't save the '{0}' assembly for the '{1}' type: {2}.", 
						context.AssemblyBuilder.Path,
						context.OriginalType.FullName,
						ex.Message);
				}
			}
		}
	}
}

