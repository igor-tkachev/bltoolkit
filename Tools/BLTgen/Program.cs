using System;
using System.Collections;
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

		[MapField("O"), Description("Output directory name (default: target assembly location). Example: /O:c:\\Temp")]
		public string OutputDirectory;

		[MapField("I"), Description("Type names to include (default: all). Example: /I:*Accessor;SomeNamespace.*;OtherNamespace.*")]
		public string Include;

		[MapField("X"), Description("Type names to exclude (default: none). Example: /X:SomeNamespace.SomeType")]
		public string Exclude;

		[MapField("V"), Description("Verbose output (default: false). Example: /V")]
		public string Verbose;
	}

	class Program
	{
		public static void Main(string[] args)
		{
			Arguments parsedArgs = new Arguments();

			Map.MapSourceToDestination(new StringListMapper(args), args,
				Map.GetObjectMapper(typeof(Arguments)), parsedArgs);

			WriteBanner();

			if (null == parsedArgs.SourceAssembly || 0 == parsedArgs.SourceAssembly.Length)
				Usage();
			else
				GenerateExtensionAssembly(parsedArgs);
		}

		private static void GenerateExtensionAssembly(Arguments parsedArgs)
		{
			bool     verbose               = null != parsedArgs.Verbose;
			Assembly sourceAsm             = Assembly.LoadFrom(parsedArgs.SourceAssembly);
			string   extensionAssemblyPath = GetOutputAssemblyLocation(sourceAsm.Location, parsedArgs.OutputDirectory);

			if (verbose)
				Console.WriteLine("{0} =>{1}{2}", sourceAsm.Location, Environment.NewLine, extensionAssemblyPath);

			TypeFactory.SaveTypes = true;
			TypeFactory.SetGlobalAssembly(extensionAssemblyPath);

			Type[] typesToProcess = sourceAsm.GetExportedTypes();
			typesToProcess = FilterTypes(typesToProcess, parsedArgs.Include, true);
			typesToProcess = FilterTypes(typesToProcess, parsedArgs.Exclude, false);

			if (typesToProcess.Length > 0)
			{
				foreach (Type t in typesToProcess)
				{
					if (verbose)
						Console.WriteLine(t.FullName);

					TypeAccessor.GetAccessor(t);
				}

				TypeFactory.SaveGlobalAssembly();
			}
			else if (verbose)
				Console.WriteLine("No types to process.");
		}

		private static Type[] FilterTypes(Type[] types, string pattern, bool include)
		{
			if (null == pattern || 0 == pattern.Length)
				return types;

			Regex     re            = new Regex("^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace(";", "$|") + "$");
			ArrayList filteredTypes = new ArrayList(types.Length);

			foreach (Type t in types)
			{
				if (re.Match(t.FullName).Success == include)
					filteredTypes.Add(t);
			}

			return (Type[])filteredTypes.ToArray(typeof(Type));
		}

		private static string GetOutputAssemblyLocation(string sourceAssembly, string outputDirectory)
		{
			if (null == outputDirectory || 0 == outputDirectory.Length)
				outputDirectory = Path.GetDirectoryName(sourceAssembly);

			string fileName = Path.GetFileNameWithoutExtension(sourceAssembly) +
					".BLToolkitExtension" + Path.GetExtension(sourceAssembly);

			return Path.Combine(outputDirectory, fileName);
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
			get { return Path.GetFileName(Assembly.GetEntryAssembly().Location); }
		}

		private static string GetDescription(MemberMapper mm)
		{
			DescriptionAttribute desc = ((DescriptionAttribute)
				mm.MemberAccessor.GetAttribute(typeof(DescriptionAttribute)));

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
