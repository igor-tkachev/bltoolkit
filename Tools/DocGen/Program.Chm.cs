using System;
using System.Diagnostics;
using System.IO;

namespace DocGen
{
	partial class Program
	{
		static readonly string _template = Path.GetFullPath(@"..\..\content\ChmTemplate.html");

		static FileAction FilterFile(string fileName)
		{
			var name = Path.GetFileName(fileName).ToLower();

			switch (name)
			{
				case "download.htm" :
				case "robots.txt"   : return FileAction.Skip;
				default             :
					if (fileName.ToLower().EndsWith("\\content\\index.htm"))
						return FileAction.Skip;
					
					return FileAction.Process;
			}
		}

		static void CreateTarget(FileItem root)
		{
			CreateProjectFile(root);
			CreateIndexFile  ();
			CreateContentFile(root);

			var hcc = ProgramFilesx86() + @"\HTML Help Workshop\hhc.exe";

			Process.Start(hcc, destPath + "BLToolkit.hhp").WaitForExit();
			Process.Start(Path.GetFullPath(@"..\..\..\..\Source\Doc\") + "BLToolkit.chm");
		}

		static string ProgramFilesx86()
		{
			if (IntPtr.Size == 8 || !String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")))
				return Environment.GetEnvironmentVariable("ProgramFiles(x86)");

			return Environment.GetEnvironmentVariable("ProgramFiles");
		}

		static void CreateProjectFile(FileItem root)
		{
			using (var sw = File.CreateText(destPath + "BLToolkit.hhp"))
			{
				sw.WriteLine("[FILES]");

				foreach (var file in root.GetFiles())
				{
					var s = file.Path;
					sw.WriteLine(s);
				}

				var path = Path.GetFullPath(@"..\..\..\..\Source\Doc\");

				sw.WriteLine(@"
[OPTIONS]
Title=BLToolkit
Auto Index=Yes
Compatibility=1.1 or later
Compiled file={0}BLToolkit.chm
Default Window=MsdnHelp
Default topic=Home.htm
Display compile progress=No
Error log file=BLToolkit.log
Full-text search=Yes
Index file=BLToolkit.hhk
Language=0x409 English (United States)
Contents file=BLToolkit.hhc

[WINDOWS]
MsdnHelp=""Business Logic Toolkit for .NET Help"",""BLToolkit.hhc"",""BLToolkit.hhk"",""Home.htm"",""Home.htm"",,,,,0x63520,250,0x387e,[50,25,850,625],0x1020000,,,,,,0

[INFOTYPES]
",
					path);
			}
		}

		private static void CreateIndexFile()
		{
			using (var sw = File.CreateText(destPath + "BLToolkit.hhk"))
			{
				sw.WriteLine("<HTML><HEAD></HEAD><BODY><UL>");

				foreach (var index in IndexItem.Index)
				{
					sw.WriteLine(
						@"<LI><OBJECT type=""text/sitemap""><param name=""Name"" value=""{0}"">",
						index.Name);

					if (index.Files.Count == 0)
						throw new InvalidDataException(string.Format("Index '{0}' is empty.", index.Name));

					foreach (var file in index.Files)
					{
						if (file.Path == "Home.htm")
							continue;

						sw.WriteLine(@"
	<param name=""Name"" value=""{0}"">
	<param name=""Local"" value=""{1}"">",
							file.Title, file.Path);
					}

					sw.WriteLine("</OBJECT>");
				}

				sw.WriteLine("</UL></BODY></HTML>");
			}
		}

		private static void CreateContent(FileItem dir, TextWriter sw)
		{
			if (dir.Items == null)
				return;

			var index = dir.Items.Find(i => i.Name.ToLower().EndsWith("index.htm"));

			if (index == null)
			{
				sw.WriteLine(
					@"
<LI><OBJECT type=""text/sitemap"">
	<param name=""Name"" value=""{0}"">
	</OBJECT>
<UL>",
					dir.Title);
			}
			else
			{
				sw.WriteLine(
					@"
<LI><OBJECT type=""text/sitemap"">
	<param name=""Name"" value=""{0}"">
	<param name=""Local"" value=""{1}"">
	</OBJECT>
<UL>",
					index.Title, index.Name);
			}

			foreach (var file in dir.Items)
			{
				if (file.Name.ToLower().EndsWith("index.htm"))
					continue;

				if (file.IsFile)
				{
					sw.WriteLine(@"
<LI><OBJECT type=""text/sitemap"">
	<param name=""Name"" value=""{0}"">
	<param name=""Local"" value=""{1}"">
	</OBJECT>",
					             file.Title, file.Name);

				}
				else
					CreateContent(file, sw);
			}

			sw.WriteLine("</UL>");
		}

		private static void CreateContentFile(FileItem root)
		{
			using (var sw = File.CreateText(destPath + "BLToolkit.hhc"))
			{
				sw.WriteLine(@"
<HTML>
<HEAD>
</HEAD><BODY>
<UL>
	<LI> <OBJECT type=""text/sitemap"">
		<param name=""Name"" value=""Home"">
		<param name=""Local"" value=""Home.htm"">
		</OBJECT>
	<LI> <OBJECT type=""text/sitemap"">
		<param name=""Name"" value=""License"">
		<param name=""Local"" value=""License.htm"">
		</OBJECT>");

				foreach (var file in root.Items)
				{
					if (!file.IsFile && file.Name == "Doc")
					{
						file.Title = "Documentation";
						CreateContent(file, sw);
						break;
					}
				}

				sw.WriteLine(@"
</UL>
</BODY></HTML>
");
			}
		}
	}
}
