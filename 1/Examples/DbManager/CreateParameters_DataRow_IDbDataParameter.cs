/// example:
/// db CreateParameters(DataRow,IDbDataParameter[])
/// comment:
/// The following example creates the <see cref="DataTable"/>, 
/// populates and adds one <see cref="DataRow"/> to the table.
/// Then it creates the <see cref="DbManager"/> and
/// calls the <see cref="SetCommand(string,IDbDataParameter[])"/> method passing a parameter list that is created 
/// by calling the <b>CreateParameters</b> method.
using System;
using System.Data;

using NUnit.Framework;

using Rsdn.Framework.Data;

namespace Examples_DbManager
{
	[TestFixture]
	public class CreateParameters_DataRow_IDbDataParameter
	{
		[Test]
		public void Test()
		{
			DataTable table = new DataTable();
            
			table.Columns.Add("CategoryID",   typeof(int));
			table.Columns.Add("CategoryName", typeof(string));
			table.Columns.Add("Description",  typeof(string));
            
			table.Rows.Add(new object[] { 0, "New category", null });
            
			using (DbManager db = new DbManager())
			{
				foreach (DataRow row in table.Rows)
				{
					db
						.SetCommand(@"
							INSERT INTO Categories (
								CategoryName, Description
							) VALUES (
								@CategoryName, @Description
							)",
							db.CreateParameters(row))
						.ExecuteNonQuery();
				}
			}
		}
	}
}
