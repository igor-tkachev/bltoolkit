using System;
using System.IO;

namespace WebGen
{
	class Program
	{
		static void Main(string[] args)
		{
			string template = Path.GetFullPath(@"..\..\content\template.html");
			string rss      = Path.GetFullPath(@"..\..\content\rss.xml");
			string destPath = @"c:\temp\bltoolkit\";

			new Generator().Generate(
				template, new string[] {}, destPath, @"..\..\content", true, false,
				delegate(string fileName)
				{
					string name = Path.GetFileName(fileName).ToLower();

					switch (name)
					{
						case "rss.xml"      : return FileAction.Copy;
						case "template.html": return FileAction.Skip;
					}

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
