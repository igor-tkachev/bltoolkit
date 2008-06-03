using System;
using System.IO;

namespace DocGen
{
	partial class Program
	{
		public static string rss      = Path.GetFullPath(@"..\..\content\rss.xml");
		public static string destPath = @"c:\temp\bltoolkit\";

		static void Main(string[] args)
		{
			FileItem root = new FileItem();

			new Generator().Generate(
				root,
				template, new string[] {}, destPath, @"..\..\content", true, false,
				fileName =>
				{
					string name = Path.GetFileName(fileName).ToLower();

					switch (name)
					{
						case "rss.xml"         : return FileAction.Copy;
						case "chmtemplate.html": return FileAction.Skip;
						case "webtemplate.html": return FileAction.Skip;
					}

					FileAction fileAction = FilterFile(fileName);

					if (fileAction != FileAction.Process)
						return fileAction;

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

			root.Prepare();

			CreateTarget(root);
		}
	}
}
