using System;
using System.Collections.Generic;
using System.IO;

namespace DocGen
{
	class FileItem
	{
		public bool           IsFile;
		public string         Name;
		public List<FileItem> Items;
		public FileItem       Parent;
		public string         Group;
		public List<string>   Indexes   = new List<string>();
		public List<string>   NoIndexes = new List<string>();
		public bool           NoIndex;

		private string _title;
		public  string  Title
		{
			get { return _title ?? System.IO.Path.GetFileNameWithoutExtension(Name); }
			set { _title = value; }
		}

		private int _sortOrder;
		public  int  SortOrder
		{
			get
			{
				if (_sortOrder == 0 && Items != null)
					return Items[0].SortOrder;

				return _sortOrder;
			}

			set { _sortOrder = value; }
		}

		public string Path
		{
			get
			{
				return Name.Replace(Program.destPath, "").Replace("\\", "/");
			}
		}

		public IEnumerable<FileItem> GetFiles()
		{
			if (Items != null)
			{
				foreach (var item in Items)
				{
					if (item.IsFile)
						yield return item;

					if (item.Items != null)
						foreach (var i in item.GetFiles())
							yield return i;
				}
			}
		}

		public void Add(FileItem item)
		{
			item.Parent = this;

			if (Items == null)
				Items = new List<FileItem>();

			Items.Add(item);
		}

		public void Prepare()
		{
			if (Items != null)
			{
				List<FileItem> groups = new List<FileItem>();

				for (int i = 0; i < Items.Count; i++)
				{
					FileItem item = Items[i];

					if (item.Group != null && item.Group != Name)
					{
						FileItem group = groups.Find(file => file.Name == item.Group);

						if (group == null)
							groups.Add(group = new FileItem { Name = item.Group, SortOrder = item.SortOrder });

						group.Add(item);
						Items.RemoveAt(i);

						i--;
					}
				}

				Items.AddRange(groups);

				foreach (var item in Items)
					item.Prepare();

				Items.Sort((x, y) =>
				{
					string xname = x.Title.ToLower();
					string yname = y.Title.ToLower();

					if (xname == yname)                         return  0;
					if (x.Name.ToLower().EndsWith("index.htm")) return -1;
					if (y.Name.ToLower().EndsWith("index.htm")) return  1;

					if (x.SortOrder != 0 && y.SortOrder != 0)
						return x.SortOrder - y.SortOrder;

					if (x.SortOrder != 0) return -1;
					if (y.SortOrder != 0) return  1;

					return string.Compare(xname, yname);
				});
			}
		}
	}
}
