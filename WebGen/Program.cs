using System;
using System.IO;

namespace WebGen
{
	class Program
	{
		static void Main(string[] args)
		{
			string template = Path.GetFullPath(@"..\..\content\template.html");
			string destPath = @"c:\temp\bltoolkit\";

			new Generator().Generate(
				template, new string[] {}, destPath, @"..\..\content", true, false,
				delegate(string fileName)
				{
					string ext = Path.GetExtension(fileName).ToLower();

					switch (ext)
					{
						case ".config":
						case ".xml":
						case ".sql": return FileAction.Skip;
						case ".htm": return FileAction.Process;
						default    : return FileAction.Copy;
					}
				});

			new Generator().Generate(
				template, new string[] { "Source" }, destPath, @"..\..\..\", false, true,
				delegate(string fileName)
				{
					string name = Path.GetFileName(fileName).ToLower();

					if (name == "assemblyinfo.cs")
						return FileAction.Skip;

					string ext  = Path.GetExtension(fileName).ToLower();

					switch (ext)
					{
						case ".cs": return FileAction.Process;
						default   : return FileAction.Skip;
					}
				});
		}
	}
}
