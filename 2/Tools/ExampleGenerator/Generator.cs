using System;
using System.IO;

namespace ExampleGenerator
{
	class Generator
	{
		class Lang
		{
			public Lang(string name, string extension, string comment)
			{
				Name      = name;
				Extension = extension;
				Comment   = comment;
			}

			public string Name;
			public string Extension;
			public string Comment;
		}

		static void ProcessFile(StreamWriter sw, string fileName, Lang lang)
		{
			using (StreamReader sr = File.OpenText(fileName))
			{
				string ln = sr.ReadLine();

				if (ln.StartsWith(lang.Comment + " example:"))
				{
					Console.WriteLine("{0}", fileName);

					string[] path = sr.ReadLine().Split(new char[]{' '});

					sw.WriteLine("<{0} name=\"{1}\">", path[1], path[2]);
					sw.WriteLine("<example>");

					ln = sr.ReadLine();

					if (ln.StartsWith(lang.Comment + " comment:"))
					{
						for (ln = sr.ReadLine(); ln.StartsWith(lang.Comment); ln = sr.ReadLine())
						{
							ln = ln.Substring(3);

							if (ln.Length > 0 && ln[0] == ' ')
								ln = ln.Substring(1);

							sw.WriteLine(ln);
						}
					}

					ln += "\n" + sr.ReadToEnd();
					ln.Trim();

					while (ln.Length > 0 && (ln[ln.Length-1] == '\r' || ln[ln.Length-1] == '\n'))
						ln = ln.Substring(0, ln.Length - 1).TrimEnd();

					ln = ln.Replace("<", "&lt;").Replace(">", "&gt;");

					sw.WriteLine("<code lang=\"{0}\">", lang.Name);
					sw.WriteLine(ln);
					sw.WriteLine("</code>");
					sw.WriteLine("</example>");
					sw.WriteLine("</{0}>", path[1]);
					sw.WriteLine();
				}
			}
		}

		static void ScanDirectory(StreamWriter sw, string path, Lang lang)
		{
			string[] files = Directory.GetFiles(path, lang.Extension);

			foreach (string file in files)
				ProcessFile(sw, file, lang);

			string[] dirs = Directory.GetDirectories(path);

			foreach (string dir in dirs)
				ScanDirectory(sw, dir, lang);
		}

		[STAThread]
		static void Main(string[] args)
		{
			string path = null;
			string xml  = null;

			foreach (string arg in args)
			{
				if (arg.ToLower().StartsWith("/path:"))
					path = arg.Substring(6);

				if (arg.ToLower().StartsWith("/xml:"))
					xml = arg.Substring(5);
			}

			//Console.WriteLine("/path:{0} /xml:{1}", path, xml);

			if (path == null || xml == null)
			{
				Console.WriteLine(@"Use: ExampleGenerator /path:..\Examples /xml:..\Source\Examples.xml");
			}

			using (StreamWriter sw = File.CreateText(xml))
			{
				sw.WriteLine(@"<?xml version=""1.0"" encoding=""utf-8"" ?>");
				sw.WriteLine(@"<examples>");
				sw.WriteLine();

				ScanDirectory(sw, path, new Lang("C#", "*.cs", "//@"));
				ScanDirectory(sw, path, new Lang("VB", "*.vb", "''@"));

				sw.WriteLine();
				sw.WriteLine(@"</examples>");
			}
		}
	}
}
