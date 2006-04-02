using System;
using System.IO;
using System.Collections.Generic;

using Rsdn.Framework.Formatting;
using System.Globalization;

namespace WebGen
{
	delegate FileAction FileActionHandler(string ext);

	class Generator
	{
		string            _sourcePath;
		string            _destFolder;
		FileActionHandler _fileAction;

		public void Generate(
			string templateFileName, string[] path,
			string destFolder, string sourcePath,
			bool cleanUp, bool createIndex, FileActionHandler fileAction)
		{
			_sourcePath = Path.GetFullPath(sourcePath);
			_destFolder = Path.GetFullPath(destFolder);
			_fileAction = fileAction;

			if (cleanUp)
				CleanUp();

			CreateDestFolder();

			string template = null;

			using (StreamReader sr = File.OpenText(templateFileName))
				template = sr.ReadToEnd();

			GenerateContent(template, path, createIndex);
		}

		private void CreateDestFolder()
		{
			if (Directory.Exists(_destFolder) == false)
				Directory.CreateDirectory(_destFolder);
		}

		private void CleanUp()
		{
			if (Directory.Exists(_destFolder) == false)
				return;

			Action<string> clean = null; clean = delegate(string path)
			{
				foreach (string file in Directory.GetFiles(path))
					if (Path.GetExtension(file).ToLower() != ".zip")
						try { File.Delete(file); } catch {}

				foreach (string dir in Directory.GetDirectories(path))
				{
					clean(dir);
					try { Directory.Delete(dir); } catch {}
				}
			};

			clean(_destFolder);
		}

		private bool GenerateContent(string template, string[] path, bool createIndex)
		{
			string folder     = string.Join("/", path);
			string destFolder = Path.Combine(_destFolder, folder);
			string backPath   = "";

			for (int i = 0; i < path.Length; i++)
				backPath += "../";

			string   sourcePath  = Path.Combine(_sourcePath, folder);
			string[] sourceFiles = Directory.GetFiles(sourcePath);

			List<string> files   = new List<string>();
			List<string> folders = new List<string>();

			foreach (string fileName in sourceFiles)
			{
				if (fileName.ToLower().EndsWith("template.html"))
					continue;

				string backLinks = GeneratePath(path, backPath, fileName);

				string destName = Path.Combine(destFolder, Path.GetFileName(fileName));
				string ext      = Path.GetExtension(destName).ToLower();

				Console.WriteLine(destName);

				switch (_fileAction(fileName))
				{
					case FileAction.Skip:
						break;

					case FileAction.Copy:
						File.Copy(fileName, destName, true);
						break;

					case FileAction.Process:
						if (Directory.Exists(destFolder) == false)
							Directory.CreateDirectory(destFolder);

						switch (ext)
						{
							case ".htm":
							case ".html":
								using (StreamWriter sw = File.CreateText(destName))
								using (StreamReader sr = File.OpenText(fileName))
								{
									string source = GenerateSource(sr.ReadToEnd());
									sw.WriteLine(string.Format(template, source, backPath, backLinks));
								}
								break;

							case ".cs":
								using (StreamWriter sw = File.CreateText(destName + ".htm"))
								{
									string source = GenerateSource("<% " + fileName + " %>");
									sw.WriteLine(string.Format(template, source, backPath, backLinks));
								}
								break;
						}

						files.Add(fileName);

						break;
				}
			}

			string[] dirs    = Directory.GetDirectories(sourcePath);
			string[] newPath = new string[path.Length + 1];

			path.CopyTo(newPath, 0);

			foreach (string dir in dirs)
			{
				string[] dirList = dir.Split('/', '\\');
				string   dirName = dirList[dirList.Length - 1];

				if (dirName == "_svn")
					continue;

				newPath[path.Length] = dirName;

				if (GenerateContent(template, newPath, createIndex))
					folders.Add(dir);
			}

			if (files.Count > 0 || folders.Count > 0)
			{
				string indexName = destFolder + "/index.htm";

				if (createIndex && File.Exists(indexName) == false)
				{
					string str = "";

					folders.Sort();

					foreach (string s in folders)
						str += string.Format("&#8226; <a href='{0}/index.htm'>{0}</a><br>\n",
							Path.GetFileName(s));

					if (str.Length > 0)
						str += "<br>";

					files.Sort();

					foreach (string s in files)
						str += string.Format(
							s.EndsWith(".htm",  true, CultureInfo.CurrentCulture) ||
							s.EndsWith(".html", true, CultureInfo.CurrentCulture)?
								"&#8226; <a href='{0}'>{0}</a><br>":
								"&#8226; <a href='{0}.htm'>{0}</a><br>\n",
							Path.GetFileName(s));

					using (StreamWriter sw = File.CreateText(indexName))
					{
						sw.WriteLine(string.Format(
							template,
							str,
							backPath,
							GeneratePath(path, backPath, "@@@").Replace(".@@@", "")));
					}
				}

				return true;
			}

			return false;
		}

		private string GenerateSource(string source)
		{
			for (int
				 idx = source.IndexOf("<%"),
				 end = source.IndexOf("%>", idx + 2);
				 idx >= 0 &&
				 end >= 0;
				 idx = source.IndexOf("<%", end + 2),
				 end = source.IndexOf("%>", idx + 2))
			{
				string startSource = source.Substring(0, idx);
				string fileName    = source.Substring(idx + 2, end - idx - 2).Trim();
				string endSource   = source.Substring(end + 2);
				string sourcePath  = Path.Combine(_sourcePath, fileName);
				string code        = "";

				using (StreamReader sr = File.OpenText(sourcePath))
					for (string s = sr.ReadLine(); s != null; s = sr.ReadLine())
						if (!s.StartsWith("//@"))
							code += s + "\n";

				switch (Path.GetExtension(fileName).ToLower())
				{
					case ".cs":     code = "[c#]"   + code + "[/c#]";   break;
					case ".vb":     code = "[vb]"   + code + "[/vb]";   break;
					case ".xml":
					case ".config": code = "[xml]"  + code + "[/xml]";  break;
					case ".sql":    code = "[sql]"  + code + "[/sql]";  break;
					default   :     code = "[code]" + code + "[/code]"; break;
				}

				code = code
					.Replace("/*[", "[")
					.Replace("]*/", "]")
					;

				code = new TextFormatter().Format(code, false);

				if (source.IndexOf("<a name='Person'></a>") >= 0)
					code = code
						.Replace("&lt;Person&gt;", "&lt;<a class=m href=#Person>Person</a>&gt;")
						.Replace("    Person ",    "    <a class='m' href=#Person>Person</a> ")
						.Replace(" Person()",      " <a class='m' href=#Person>Person</a>()")
						.Replace("(Person ",       "(<a class='m' href=#Person>Person</a> ")
						;

				source =
					startSource +
					code
						.Replace("\n",     "\r\n")
						.Replace("\r\r\n", "\r\n")
						.Replace("<table width='96%'>", "<table width='100%' class='code'>")
						.Replace("<pre>",  "<pre class='code'>")
						.Replace("[a]",    "<span class='a'>")
						.Replace("[/a]",   "</span>")
						+
					endSource;
			}

			return source;
		}

		private string GeneratePath(string[] path, string backPath, string fileName)
		{
			if (path.Length == 0)
				return "";

			string backLinks = "";

			switch (path[0])
			{
				case "Doc":
					backLinks += string.Format(
						"<br><nobr>&nbsp;&nbsp;<small><a class='m' href='{0}index.htm'>BLToolkit</a>",
						backPath);

					for (int i = 1; i < path.Length; i++)
					{
						string parent = "";

						for (int j = i + 1; j < path.Length; j++)
							parent += "../";

						backLinks += string.Format(".<a class='m' href='{0}index.htm'>{1}</a>", parent, path[i]);
					}

					backLinks += "<small></nobr></br>";

					break;

				case "Source":
					backLinks += string.Format(
						"<br><nobr>&nbsp;&nbsp;<small><a class='m' href='{0}Source/index.htm'>BLToolkit</a>",
						backPath);

					for (int i = 1; i < path.Length; i++)
					{
						string parent = "";

						for (int j = i + 1; j < path.Length; j++)
							parent += "../";

						backLinks += string.Format(".<a class='m' href='{0}index.htm'>{1}</a>", parent, path[i]);
					}

					backLinks += "." + Path.GetFileNameWithoutExtension(fileName);
					backLinks += "<small></nobr></br>";

					break;
			}

			return backLinks;
		}
	}
}
