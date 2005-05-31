using System;
using System.IO;

namespace ExampleGenerator
{
	class Generator
	{
		static void ProcessFile(StreamWriter sw, string fileName)
		{
			using (StreamReader sr = File.OpenText(fileName))
			{
				string ln = sr.ReadLine();

				if (ln.StartsWith("/// example:"))
				{
					Console.WriteLine("{0}", fileName);

					string[] path = sr.ReadLine().Split(new char[]{' '});

					sw.WriteLine("<{0} name=\"{1}\">", path[1], path[2]);
					sw.WriteLine("<example>");

					ln = sr.ReadLine();

					if (ln.StartsWith("/// comment:"))
					{
						for (ln = sr.ReadLine(); ln.StartsWith("///"); ln = sr.ReadLine())
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

					sw.WriteLine("<code>");
					sw.WriteLine(ln);
					sw.WriteLine("</code>");
					sw.WriteLine("</example>");
					sw.WriteLine("</{0}>", path[1]);
					sw.WriteLine();
				}
			}
		}

		static void ScanDirectory(StreamWriter sw, string path)
		{
			string[] files = Directory.GetFiles(path, "*.cs");

			foreach (string file in files)
				ProcessFile(sw, file);

			string[] dirs = Directory.GetDirectories(path);

			foreach (string dir in dirs)
				ScanDirectory(sw, dir);
		}

		[STAThread]
		static void Main()
		{
			string path = @"..\..\..\..\Examples\Mapping";
			string xml  = @"..\..\..\..\Source\Mapping\Examples.xml";

			using (StreamWriter sw = File.CreateText(xml))
			{
				sw.WriteLine(@"<?xml version=""1.0"" encoding=""utf-8"" ?>");
				sw.WriteLine(@"<examples>");
				sw.WriteLine();

				ScanDirectory(sw, path);

				sw.WriteLine();
				sw.WriteLine(@"</examples>");
			}

			path = @"..\..\..\..\Examples\DbManager";
			xml  = @"..\..\..\..\Source\Examples.xml";

			using (StreamWriter sw = File.CreateText(xml))
			{
				sw.WriteLine(@"<?xml version=""1.0"" encoding=""utf-8"" ?>");
				sw.WriteLine(@"<examples>");
				sw.WriteLine();

				ScanDirectory(sw, path);

				sw.WriteLine();
				sw.WriteLine(@"</examples>");
			}
		}
	}
}
