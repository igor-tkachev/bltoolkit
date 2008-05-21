using System;
using System.IO;
using System.Collections.Generic;

namespace WebGen
{
	class Program
	{
		static string template = Path.GetFullPath(@"..\..\content\template.html");
		static string rss      = Path.GetFullPath(@"..\..\content\rss.xml");
		static string destPath = @"c:\temp\bltoolkit\";

		static void Main(string[] args)
		{
			List<string> files = new List<string>();

			new Generator().Generate(
				files,
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
						case ".cs":
						case ".config":
						case ".xml":
						case ".sql": return FileAction.Skip;
						case ".htm": return FileAction.Process;
						default    : return FileAction.Copy;
					}
				});

//return;
			new Generator().Generate(
				new List<string>(), // files,
				template, new string[] { "Source" }, destPath, @"..\..\..\..\", false, true,
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

			CreateSitemap(files);
		}

		static void CreateSitemap(List<string> files)
		{
			string sm = "";

			foreach (string file in files)
			{
				string s = file.Replace(destPath, "http://www.bltoolkit.net/").Replace("\\", "/");

				if (s == "http://www.bltoolkit.net/" + "index.htm")
					continue;

				sm += string.Format(@"
	<url>
		<loc>{0}</loc>
		<lastmod>{1:yyyy-MM-dd}</lastmod>
		<changefreq>weekly</changefreq>
	</url>",
					s, DateTime.Now);
			}

			using (StreamWriter sw = File.CreateText(destPath + "sitemap.xml"))
			{
				sw.WriteLine(string.Format(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<urlset xmlns=""http://www.google.com/schemas/sitemap/0.84"">
	<url>
		<loc>http://www.bltoolkit.net/</loc>
		<lastmod>{0:yyyy-MM-dd}</lastmod>
		<changefreq>weekly</changefreq>
	</url>{1}
</urlset>",
					DateTime.Now, sm));
			}
		}
	}
}
