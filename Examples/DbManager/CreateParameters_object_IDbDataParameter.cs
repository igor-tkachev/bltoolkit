/// example:
/// db CreateParameters(object,IDbDataParameter[])
/// comment:
/// The following example creates a business object, creates the <see cref="DbManager"/>, and
/// calls the <see cref="SetCommand(string,IDbDataParameter[])"/> method passing a parameter list that is created 
/// by calling the <b>CreateParameters</b> method.
using System;

using NUnit.Framework;

using Rsdn.Framework.Data;

namespace Examples_DbManager
{
	[TestFixture]
	public class CreateParameters_object_IDbDataParameter
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
			Category category = new Category();

			category.Name = "New category";

			using (DbManager db = new DbManager())
			{
				db
					.SetCommand(@"
						INSERT INTO Categories (
							CategoryName, Description
						) VALUES (
							@Name, @Description
						)",
						db.CreateParameters(category))
					.ExecuteNonQuery();
			}
		}
	}
}
