using System;
using System.Linq;
using System.Web.UI;

using BLToolkit.Data;
using BLToolkit.Data.Linq;

namespace Partial.Trust.Asp.Net
{
	using Components;

	public partial class _Default : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			var da   = PersonDataAccessor.CreateInstance();
			var list = da.GetPersonList();

			Label1.Text = list[0].ContactName;

			var q =
				from c in new Table<Customers>()
				where c.CustomerID == list[0].CustomerID
				select c.ContactName;

			Label2.Text = q.First();

			using (var db = new DbManager())
				Label3.Text = _compiledQuery(db, list[0].CustomerID).ToList().First();
		}

		static readonly Func<DbManager,string,IQueryable<string>> _compiledQuery =
			CompiledQuery.Compile((DbManager db, string id) => 
				from c in db.GetTable<Customers>()
				where c.CustomerID == id
				select c.ContactName);
	}
}
