/// example:
/// db ExecuteDictionary(string,Type)
/// comment:
/// The following example demonstrates how to use the <b>ExecuteDictionary</b> method.
using System;
using System.Collections;

using NUnit.Framework;

using Rsdn.Framework.Data;

namespace Examples_DbManager
{
	[TestFixture]
	public class ExecuteDictionary_string_Type
	{
		public class Category
		{
			public int    ID;
			public string Name;
			public string Description;
		}

		[Test]
		public void Test()
		{
			using (DbManager db = new DbManager())
			{
				Hashtable ht = db
					.SetCommand(@"
						SELECT
							CategoryID   as ID,
							CategoryName as Name,
							Description  as Description
						FROM Categories")
					.ExecuteDictionary("ID", typeof(Category));

				foreach (Category category in ht.Values)
				{
					Console.WriteLine("ID  : {0}\nName: {1}\nDesc: {2}",
						category.ID, category.Name, category.Description);
					Console.WriteLine();
				}
			}
		}
	}
}
