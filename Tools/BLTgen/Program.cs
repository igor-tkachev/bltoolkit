using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

using BLToolkit.Mapping;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace BLTgen
{
	public class Arguments
	{
		[MapField(""), Description("source assembly location")]
		public string SourceAssembly;

		[MapField("B"), Description("Base type names to include (default: none). Example: /B:*EntityBase;SomeNamespace.*Base")]
		public string BaseTypes;

		[MapField("O"), Description("Output directory name (default: target assembly location). Example: /O:C:\\Temp")]
		public string OutputDirectory;

		[MapField("I"), Description("Type names to include (default: all). Example: /I:*Accessor;SomeNamespace.*;OtherNamespace.*")]
		public string Include;

		[MapField("X"), Description("Type names to exclude (default: none). Example: /X:SomeNamespace.SomeType")]
		public string Exclude;

		[MapField("K"), Description("The key pair that is used to create a strong name signature for the output assembly (default: none). Example: /K:C:\\SomePath\\key.snk")]
		public string KeyPairFile;

		[MapField("V"), Description("The version of the output assembly (same as source assembly by default). Example: /V:1.2.3.4")]
		public string Version;

		[MapField("D"), Description("Detailed output (default: false). Example: /D")]
		public string Debug;
	}

	class Program
	{
		public static void Main(string[] args)
		{
			Arguments parsedArgs = new Arguments();

			Map.MapSourceToDestination(new StringListMapper(args), args,
				Map.GetObjectMapper(typeof(Arguments)), parsedArgs);

			WriteBanner();

			if (string.IsNullOrEmpty(parsedArgs.SourceAssembly))
				Usage();
			else
				GenerateExtensionAssembly(parsedArgs);
		}

		private static void GenerateExtensionAssembly(Arguments parsedArgs)
		{
			bool     verbose                  = parsedArgs.Debug != null;
			Assembly sourceAsm                = Assembly.LoadFrom(parsedArgs.SourceAssembly);
			string   extensionAssemblyPath    = GetOutputAssemblyLocation(sourceAsm.Location, parsedArgs.OutputDirectory);
			Version  extensionAssemblyVersion = parsedArgs.Version != null? new Version(parsedArgs.Version): sourceAsm.GetName().Version;

			if (verbose)
				Console.WriteLine("{0} =>{1}{2}", sourceAsm.Location, Environment.NewLine, extensionAssemblyPath);

			if (!Directory.Exists(parsedArgs.OutputDirectory))
				Directory.CreateDirectory(parsedArgs.OutputDirectory);

			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

			TypeFactory.SaveTypes = true;
			TypeFactory.SetGlobalAssembly(extensionAssemblyPath, extensionAssemblyVersion, parsedArgs.KeyPairFile);

			Type[] typesToProcess = sourceAsm.GetExportedTypes();
			typesToProcess = FilterBaseTypes(typesToProcess, parsedArgs.BaseTypes);
			typesToProcess = FilterTypes(typesToProcess, parsedArgs.Include, true);
			typesToProcess = FilterTypes(typesToProcess, parsedArgs.Exclude, false);

			if (typesToProcess.Length > 0)
			{
				foreach (Type t in typesToProcess)
				{
					if (verbose)
						Console.Write(GetFullTypeName(t));

					// We cannot create accessors for generic definitions
					//
					if (t.IsGenericTypeDefinition)
					{
						if (verbose)
							Console.WriteLine(" - skipping. Generic Definition");

						continue;
					}

					if (verbose)
						Console.WriteLine();

					try
					{
						TypeAccessor.GetAccessor(t);
					}
					catch (Exception e)
					{
						if (verbose)
							Console.WriteLine(e);
					}
				}

				TypeFactory.SaveGlobalAssembly();
			}
			else if (verbose)
				Console.WriteLine("No types to process.");
		}

		static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (assembly.GetName(false).FullName == args.Name)
					return assembly;
			}

			return null;
		}

		private static Type[] FilterBaseTypes(Type[] types, string pattern)
		{
			if (string.IsNullOrEmpty(pattern))
				return types;

			Regex re = new Regex("^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace(";", "$|") + "$");

			return Array.FindAll(types, delegate(Type t)
			{
				for (Type bt = t.BaseType; bt != null; bt = bt.BaseType)
				{
					if (re.Match(GetFullTypeName(bt)).Success)
						return true;
				}
				return false;
			});
		}

		private static Type[] FilterTypes(Type[] types, string pattern, bool include)
		{
			if (string.IsNullOrEmpty(pattern))
				return types;

			Regex re = new Regex("^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace(";", "$|") + "$");

			return Array.FindAll(types, delegate(Type t)
			{
				return re.Match(GetFullTypeName(t)).Success == include;
			});
		}

		// System.Type.FullName may be null under some conditions. See
		// http://blogs.msdn.com/haibo_luo/archive/2006/02/17/534480.aspx
		//
		private static string GetFullTypeName(Type t)
		{
			string fullName = t.FullName;

			if (fullName != null)
				return fullName;

			if (t.DeclaringType != null)
				return GetFullTypeName(t.DeclaringType) + "+" + t.Name;

			fullName = t.Namespace;
			if (fullName != null)
				fullName += ".";

			fullName += t.Name;

			return fullName;
		}

		private static string GetOutputAssemblyLocation(string sourceAssembly, string outputDirectory)
		{
			if (string.IsNullOrEmpty(outputDirectory))
				outputDirectory = Path.GetDirectoryName(sourceAssembly);

			string fileName = Path.ChangeExtension(Path.GetFileName(sourceAssembly), "BLToolkitExtension.dll");
			return Path.Combine(Path.GetFullPath(outputDirectory), fileName);
		}

		#region Usage

		private static void WriteBanner()
		{
			Assembly asm = Assembly.GetExecutingAssembly();
			AssemblyDescriptionAttribute descriptionAttribute = (AssemblyDescriptionAttribute)
				Attribute.GetCustomAttribute(asm, typeof (AssemblyDescriptionAttribute));
			AssemblyCopyrightAttribute copyrightAttribute = (AssemblyCopyrightAttribute)
				Attribute.GetCustomAttribute(asm, typeof(AssemblyCopyrightAttribute));

			Console.WriteLine("{0}, Version {1}", descriptionAttribute.Description, asm.GetName().Version);
			Console.WriteLine(copyrightAttribute.Copyright);
			Console.WriteLine();
		}

		private static string ExecutableName
		{
			get { return Path.GetFileName(new Uri(Assembly.GetEntryAssembly().EscapedCodeBase).AbsolutePath); }
		}

		private static string GetDescription(MemberMapper mm)
		{
			DescriptionAttribute desc = mm.MemberAccessor.GetAttribute<DescriptionAttribute>();

			return (null != desc) ? desc.Description : string.Empty;
		}

		private static void Usage()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Arguments));

			Console.Write("Usage: {0}", ExecutableName);

			foreach (MemberMapper mm in om)
			{
				if (0 == mm.Name.Length)
					Console.Write(" <{0}>", GetDescription(mm));
				else
					Console.Write(" /{0}:", mm.Name);
			}

			Console.WriteLine();
			Console.WriteLine("Options:");

			foreach (MemberMapper mm in om)
			{
				if (0 != mm.Name.Length)
					Console.WriteLine("\t{0}: {1}", mm.Name, GetDescription(mm));
			}

			Console.WriteLine();
		}

		#endregion
	}
}
