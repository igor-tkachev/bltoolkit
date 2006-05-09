using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

using Rsdn.Framework.Formatting;

namespace WebGen
{
	delegate FileAction FileActionHandler(string ext);

	class Generator
	{
		string            _sourcePath;
		string            _destFolder;
		FileActionHandler _fileAction;

		public void Generate(
			List<string> createdFiles,
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

			GenerateContent(createdFiles, template, path, createIndex);
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

		private bool GenerateContent(
			List<string> createdFiles, string template, string[] path, bool createIndex)
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
									createdFiles.Add(destName);

									string source = GenerateSource(sr.ReadToEnd());
									string title  = Path.GetFileNameWithoutExtension(fileName);

									if (title == "index")
										title = Path.GetFileName(Path.GetDirectoryName(fileName));

									sw.WriteLine(string.Format(
										template,
										source,
										backPath,
										backLinks,
										title + " - "));
								}
								break;

							case ".cs":
								using (StreamWriter sw = File.CreateText(destName + ".htm"))
								{
									createdFiles.Add(destName + ".htm");

									string source = GenerateSource("<% " + fileName + " %>");
									sw.WriteLine(string.Format(
										template,
										source,
										backPath,
										backLinks,
										Path.GetFileNameWithoutExtension(fileName) + " - "));
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

				if (GenerateContent(createdFiles, template, newPath, createIndex))
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

					_fileAction(indexName);

					using (StreamWriter sw = File.CreateText(indexName))
					{
						createdFiles.Add(indexName);

						sw.WriteLine(string.Format(
							template,
							str,
							backPath,
							GeneratePath(path, backPath, "@@@").Replace(".@@@", ""),
							Path.GetFileNameWithoutExtension(destFolder) + " - "));
					}
				}

				return true;
			}

			return false;
		}

		private string GenerateSource(string text)
		{
			for (int
				 idx = text.IndexOf("<%"),
				 end = text.IndexOf("%>", idx + 2);
				 idx >= 0 &&
				 end >= 0;
				 idx = text.IndexOf("<%", end + 2),
				 end = text.IndexOf("%>", idx + 2))
			{
				string startSource = text.Substring(0, idx);
				string fileName    = text.Substring(idx + 2, end - idx - 2).Trim();
				string command     = "source";

				string[] cmds = fileName.Split('#');

				if (cmds.Length > 1)
				{
					command  = cmds[0].Trim().ToLower();
					fileName = cmds[1].Trim();
				}

				string endSource   = text.Substring(end + 2);
				string sourcePath  = Path.Combine(_sourcePath, fileName);
				string source;

				switch (command)
				{
					case "source": source = GetSourceCode(sourcePath, text); break;
					case "rss"   : source = GetNews      (sourcePath);       break;
					default      : throw new InvalidOperationException();
				}

				text = startSource + source + endSource;
			}

			return text;
		}

		private string GetNews(string sourcePath)
		{
			XmlDocument doc = new XmlDocument();

			doc.Load(sourcePath);

			string html   = "<table border='0' cellpadding='0' cellspacing='0'>";
			string @class = "";

			foreach (XmlNode item in doc.SelectNodes("rss/channel/item"))
			{
				html += string.Format(@"
<tr><td{0} colspan='2'><nobr><b>{1:MM/dd/yy}</nobr> <a href='{2}'>{3}</a></b></td></tr>
<tr><td>&nbsp;&nbsp;</td><td class='j'>{4}</td></tr>
",
					@class,
					DateTime.Parse(item.SelectSingleNode("pubDate").InnerText),
					item.SelectSingleNode("link").      InnerText,
					item.SelectSingleNode("title").      InnerText,
					item.SelectSingleNode("description").InnerText);

				@class = " class='p'";
			}

			html += "</table>";

			return html;
		}

		private static string GetSourceCode(string sourcePath, string source)
		{
			string code = "";

			using (StreamReader sr = File.OpenText(sourcePath))
				for (string s = sr.ReadLine(); s != null; s = sr.ReadLine())
					if (!s.StartsWith("//@"))
						code += s + "\n";

			switch (Path.GetExtension(sourcePath).ToLower())
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

			return code
				.Replace("\n",     "\r\n")
				.Replace("\r\r\n", "\r\n")
				.Replace("<table width='96%'>", "<table width='100%' class='code'>")
				.Replace("<pre>",  "<pre class='code'>")
				.Replace("[a]",    "<span class='a'>")
				.Replace("[/a]",   "</span>")
				;
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
