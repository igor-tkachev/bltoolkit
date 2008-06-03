using System;
using System.IO;

namespace DocGen
{
	partial class Program
	{
		static string template = Path.GetFullPath(@"..\..\content\WebTemplate.html");

		static FileAction FilterFile(string fileName)
		{
			string name = Path.GetFileName(fileName).ToLower();

			switch (name)
			{
				case "home.htm" : return FileAction.Skip;
				default         : return FileAction.Process;
			}
		}

		static void CreateTarget(FileItem files)
		{
			new Generator().Generate(
				new FileItem(),
				template, new string[] { "Source" }, destPath, @"..\..\..\..\", false, true,
				fileName =>
				{
					string name = Path.GetFileName(fileName).ToLower();

					if (name == "assemblyinfo.cs")
						return FileAction.Skip;

					string ext = Path.GetExtension(fileName).ToLower();

					switch (ext)
					{
						case ".cs": return FileAction.Process;
						default: return FileAction.Skip;
					}
				});

			CreateSitemap(files);
		}

		static void CreateSitemap(FileItem files)
		{
			string sm = "";

			foreach (var file in files.GetFiles())
			{
				string s = file.Name.Replace(destPath, "http://www.bltoolkit.net/").Replace("\\", "/");

				if (s == "http://www.bltoolkit.net/" + "index.htm")
					return;

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
