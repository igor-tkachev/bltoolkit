using System;
using System.IO;

namespace WebGen
{
	class Generator
	{
		string _sourcePath;
		string _destFolder;

		public void Generate(string sourcePath, string destFolder)
		{
			_sourcePath = Path.GetFullPath(sourcePath);
			_destFolder = Path.GetFullPath(destFolder);

			CleanUp();
			CreateDestFolder();

			string template = null;

			using (StreamReader sr = File.OpenText(Path.Combine(_sourcePath, "template.html")))
				template = sr.ReadToEnd();

			GenerateContent(template, "", "");
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
					try { File.Delete(file); } catch {}

				foreach (string dir in Directory.GetDirectories(path))
				{
					clean(dir);
					try { Directory.Delete(dir); } catch {}
				}
			};

			clean(_destFolder);
		}

		private void GenerateContent(string template, string folder, string backPath)
		{
			string destFolder = Path.Combine(_destFolder, folder);

			if (Directory.Exists(destFolder) == false)
				Directory.CreateDirectory(destFolder);

			string   sourcePath  = Path.Combine(_sourcePath, folder);
			string[] sourceFiles = Directory.GetFiles(sourcePath);

			foreach (string fileName in sourceFiles)
			{
				if (fileName.ToLower().EndsWith("template.html"))
					continue;

				string destName = Path.Combine(destFolder, Path.GetFileName(fileName));

				using (StreamWriter sw = File.CreateText(destName))
				using (StreamReader sr = File.OpenText  (fileName))
				{
					Console.WriteLine(destName);

					string source = sr.ReadToEnd();

					switch (Path.GetExtension(destName).ToLower())
					{
						case ".css": 
							sw.Write(source);
							break;

						case ".htm":
							sw.WriteLine(string.Format(template, source, backPath));
							break;
					}
				}
			}

			string[] dirs = Directory.GetDirectories(sourcePath);

			foreach (string dir in dirs)
				GenerateContent(template, dir.Substring(_sourcePath.Length + 1), backPath + "../");
		}
	}
}
