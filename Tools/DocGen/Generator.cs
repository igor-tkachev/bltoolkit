using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

using Rsdn.Framework.Formatting;

namespace DocGen
{
	delegate FileAction FileActionHandler(string ext);

	partial class Generator
	{
		string            _sourcePath;
		string            _destFolder;
		FileActionHandler _fileAction;

		public void Generate(
			FileItem          createdFiles,
			string            templateFileName,
			string[]          path,
			string            destFolder,
			string            sourcePath,
			bool              cleanUp,
			bool              createIndex,
			FileActionHandler fileAction)
		{
			_sourcePath = Path.GetFullPath(sourcePath);
			_destFolder = Path.GetFullPath(destFolder);
			_fileAction = fileAction;

			if (cleanUp)
				CleanUp();

			CreateDestFolder();

			string template = File.ReadAllText(templateFileName);

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

		static readonly Regex ct_item1 = new Regex(@"<ct_item\s*link\=(?<link>.*?)\s*label=['""](?<label>.*?)['""]\s*/>");
		static readonly Regex ct_item2 = new Regex(@"<ct_item\s*link\=(?<link>.*?)\s*label=['""](?<label>.*?)['""]\s*>(?<text>.*?)</ct_item>", RegexOptions.Singleline);
		static readonly Regex ct_item3 = new Regex(@"<mt_item\s*link\=(?<link>.*?)\s*label=['""](?<label>.*?)['""]\s*>(?<text>.*?)</mt_item>", RegexOptions.Singleline);
		static readonly Regex ct_item4 = new Regex(@"<ct_item2\s*link1\=(?<link1>.*?)\s*label1=['""](?<label1>.*?)['""]\s*link2\=(?<link2>.*?)\s*label2=['""](?<label2>.*?)['""]\s*/>", RegexOptions.Singleline);
		static readonly Regex ct_item5 = new Regex(@"<ct_item3\s*link1\=(?<link1>.*?)\s*label1=['""](?<label1>.*?)['""]\s*link2\=(?<link2>.*?)\s*label2=['""](?<label2>.*?)['""]\s*link3\=(?<link3>.*?)\s*label3=['""](?<label3>.*?)['""]\s*/>", RegexOptions.Singleline);

		private bool GenerateContent(
			FileItem createdFiles, string template, string[] path, bool createIndex)
		{
			string folder     = string.Join("/", path);
			string destFolder = Path.Combine(_destFolder, folder);
			string backPath   = "";

			for (int i = 0; i < path.Length; i++)
				backPath += "../";

			string   sourcePath  = Path.Combine(_sourcePath, folder);
			string[] sourceFiles = Directory.GetFiles(sourcePath);

			var files   = new List<string>();
			var folders = new List<string>();

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
									FileItem item = new FileItem { IsFile = true, Name = destName };
									createdFiles.Add(item);

									string source = sr.ReadToEnd();

									source = source
										.Replace("<ct_table>",  "<table border='0' cellpadding='0' cellspacing='0'>")
										.Replace("<ct_hr>",     "<ct_mg><tr><td colspan='5' class='hr'><img width='1' height='1' alt=''/></td></tr><ct_mg>")
										.Replace("<ct_text>",   "<tr><td colspan='5'>")
										.Replace("</ct_text>",  "</td></tr><ct_mg>")
										.Replace("<ct_mg>",     "<tr><td colspan='5' class='sp'><img width='1' height='1' alt=''/></td></tr>")
										.Replace("</ct_table>", "</table>")
										;

									source = ct_item1.Replace(source, @"<tr><td nowrap colspan='5'>&#8226; <a href=${link}>${label}</a></td></tr>");
									source = ct_item2.Replace(source, @"<tr><td nowrap>&#8226; <a href=${link}>${label}</a></td><td>&nbsp;&nbsp;&nbsp;</td><td class='j' colspan='3'>${text}</td></tr>");
									source = ct_item3.Replace(source, @"<tr><td nowrap class='p'>&#8226; <a href=${link}>${label}</a></td><td></td><td class='pj' colspan='3'>${text}</td></tr>");
									source = ct_item4.Replace(source, @"<tr><td nowrap>&#8226; <a href=${link1}>${label1}</a></td><td>&nbsp;&nbsp;&nbsp;</td><td nowrap colspan='3'>&#8226; <a href=${link2}>${label2}</a></td></tr>");
									source = ct_item5.Replace(source, @"<tr><td nowrap>&#8226; <a href=${link1}>${label1}</a></td><td>&nbsp;&nbsp;&nbsp;</td><td nowrap>&#8226; <a href=${link2}>${label2}</a></td><td>&nbsp;&nbsp;&nbsp;</td><td nowrap>&#8226; <a href=${link3}>${label3}</a></td></tr>");

									if (_modifySourceLinks)
									{
										source = source
											.Replace("href=\"..\\..\\..\\Source\\", "target=_blank href=\"/Source/")
											.Replace("href='..\\..\\..\\Source\\",  "target=_blank href='/Source/")
											.Replace("<a href=\"http://", "<a target=_blank href=\"http://")
											.Replace("<a href='http://",  "<a target=_blank href='http://")
											;
									}

									string title  = item.Title;

									if (title == "index")
									{
										title = Path.GetFileName(Path.GetDirectoryName(fileName));

										if (title == "content")
											title = "";
										else
											item.Title = title;
									}

									source = GenerateSource(source, item);
									title  = item.Title;

									if (title.Length > 0 && _addDashToTitle)
										title += " - ";

									sw.WriteLine(string.Format(
										template,
										source,
										backPath,
										backLinks,
										title));

									if (item.NoIndex == false)
									{
										source = source
											.Replace("<span class='a'>", "")
											.Replace("</span>", "")
											.Replace("&lt;", "<")
											.Replace("&gt;", ">")
											;

										foreach (var index in IndexItem.Index)
											if (!item.NoIndexes.Contains(index.Name))
												foreach (string s in index.Text)
													if (source.IndexOf(s) >= 0)
													{
														index.Files.Add(item);
														break;
													}

										foreach (var s in item.Indexes)
										{
											IndexItem index = IndexItem.Index.Find(i => i.Name == s);

											if (index == null)
												IndexItem.Index.Add(new IndexItem(s));

											if (index.Files.Contains(item) == false)
												index.Files.Add(item);
										}
									}
								}

								break;

							case ".cs":
								using (StreamWriter sw = File.CreateText(destName + ".htm"))
								{
									createdFiles.Add(new FileItem { IsFile = true, Name = destName + ".htm" });

									string source = GenerateSource("<% " + fileName + " %>", null);

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

				// Skip Subversion folders.
				//
				if (dirName == "_svn" || dirName == ".svn")
					continue;

				newPath[path.Length] = dirName;

				FileItem item = new FileItem { IsFile = false, Name = dirName};

				createdFiles.Add(item);

				if (GenerateContent(item, template, newPath, createIndex))
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
						createdFiles.Add(new FileItem { IsFile = true, Name = indexName });

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

		private string GenerateSource(string text, FileItem item)
		{
			for (int
				 idx = text.IndexOf("<%"),
				 end = text.IndexOf("%>", idx + 2);
				 idx >= 0 &&
				 end >= 0;
				 idx = text.IndexOf("<%", idx + 2),
				 end = text.IndexOf("%>", idx + 2))
			{
				string startSource = text.Substring(0, idx);
				string source      = text.Substring(idx + 2, end - idx - 2).Trim();
				string command     = "source";

				int cmdIdx = source.IndexOf('#');

				if (cmdIdx >= 0)
				{
					command = source.Substring(0, cmdIdx).Trim().ToLower();
					source  = source.Substring(cmdIdx+1).Trim();
				}

				switch (command)
				{
					case "source"  : source = GetSourceCodeFromPath(Path.Combine(_sourcePath, source), text); break;
					case "rss"     : source = GetNews              (Path.Combine(_sourcePath, source));       break;
					case "txt"     :
					case "cs"      :
					case "sql"     : source = GetSourceCode(source, "." + command, text);                     break;
					case "title"   : item.Title     = source;            source = "";                         break;
					case "order"   : item.SortOrder = int.Parse(source); source = "";                         break;
					case "group"   : item.Group     = source;            source = "";                         break;
					case "index"   : item.Indexes.Add(source);           source = "";                         break;
					case "noindex" :
						if (source.Length == 0)
							item.NoIndex = true;
						else
							item.NoIndexes.Add(source);
							
						source = "";
						break;

					default        : throw new InvalidOperationException();
				}

				text = startSource + source + text.Substring(end + 2);
			}

			return text;
		}

		private static string GetNews(string sourcePath)
		{
			XmlDocument doc = new XmlDocument();

			doc.Load(sourcePath);

			string html   = "<table border='0' cellpadding='0' cellspacing='0'>";
			string @class = "";
			int    i      = 0;

			foreach (XmlNode item in doc.SelectNodes("rss/channel/item"))
			{
				html += string.Format(@"
<tr><td{0} colspan='2'><nobr><b>{1:MM/dd/yy}</nobr></b> <a href='{2}'>{3}</a></td></tr>
<tr><td>&nbsp;&nbsp;</td><td class='j'>{4}</td></tr>
",
					@class,
					DateTime.Parse(item.SelectSingleNode("pubDate").InnerText),
					item.SelectSingleNode("link").       InnerText,
					item.SelectSingleNode("title").      InnerText,
					item.SelectSingleNode("description").InnerText);

				@class = " class='p'";

				if (++i == 20)
					break;
			}

			html += "</table>";

			return html;
		}

		private static string GetSourceCode(string code, string ext, string source)
		{
			switch (ext)
			{
				case ".cpp":    code = "[c]"    + code + "[/c]";    break;
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
					.Replace(", Person&gt;",   ", <a class=m href=#Person>Person</a>&gt;")
					.Replace("  Person ",      "  <a class='m' href=#Person>Person</a> ")
					.Replace(" Person()",      " <a class='m' href=#Person>Person</a>()")
					.Replace("(Person ",       "(<a class='m' href=#Person>Person</a> ")
					;

			return code
				.Replace("\n",      "\r\n")
				.Replace("\r\r\n",  "\r\n")
				.Replace("<table width='96%'>", "<table width='100%' class='code'>")
				.Replace("<pre>",   "<pre class='code'>")
				.Replace("[a]",     "<span class='a'>")
				.Replace("[/a]",    "</span>")
				.Replace("[link]",  "<a ")
				.Replace("[/link]", "</a>")
				.Replace("[file]",  "href='/Source/")
				.Replace("[/file]", ".htm'>")
				.Replace("&lt;!--", "<span class='com'>&lt;!--")
				.Replace("--&gt;",  "--&gt;</span>")
				;
		}

		private static string GetSourceCodeFromPath(string sourcePath, string source)
		{
			string code = "";

			using (StreamReader sr = File.OpenText(sourcePath))
				for (string s = sr.ReadLine(); s != null; s = sr.ReadLine())
					if (!s.StartsWith("//@") && !s.StartsWith("''@"))
						code += s + "\n";

			return GetSourceCode(code, Path.GetExtension(sourcePath).ToLower(), source);
		}

		private string GeneratePath(string[] path, string backPath, string fileName)
		{
			if (path.Length == 0)
				return "";

			string backLinks = "";
			string parent    = "";
			string name      = Path.GetFileNameWithoutExtension(fileName);

			switch (path[0])
			{
				case "Doc":
					backLinks += string.Format(
						"<br><nobr>&nbsp;&nbsp;<small><a class='m' href='{0}Doc/index.htm'>Doc</a>",
						backPath);

					for (int i = 1; i < path.Length; i++)
					{
						parent = "";

						for (int j = i + 1; j < path.Length; j++)
							parent += "../";

						backLinks += string.Format(".<a class='m' href='{0}index.htm'>{1}</a>", parent, path[i]);
					}

					if (name.ToLower() != "index")
					{
						backLinks += string.Format(".<a class='m' href='{0}{1}'>{2}</a>",
							parent, Path.GetFileName(fileName), name);
					}

					backLinks += "<small></nobr></br>";

					break;

				case "Source":
					backLinks += string.Format(
						"<br><nobr>&nbsp;&nbsp;<small><a class='m' href='{0}Source/index.htm'>Source</a>",
						backPath);

					for (int i = 1; i < path.Length; i++)
					{
						parent = "";

						for (int j = i + 1; j < path.Length; j++)
							parent += "../";

						backLinks += string.Format(".<a class='m' href='{0}index.htm'>{1}</a>", parent, path[i]);
					}

					if (name.ToLower() != "@@@")
					{
						backLinks += string.Format(".<a class='m' href='{0}{1}.htm'>{1}</a>",
							parent, Path.GetFileName(fileName));
					}

					backLinks += "<small></nobr></br>";

					break;
			}

			return backLinks;
		}
	}
}
