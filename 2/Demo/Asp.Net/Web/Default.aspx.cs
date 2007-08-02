using System;
using System.Web.UI;

namespace PetShop.Web
{
	public partial class Default : Page
	{
		/// <summary>
		/// Redirect to Search page
		/// </summary>
		protected void btnSearch_Click(object sender, EventArgs e)
		{
			WebUtility.SearchRedirect(txtSearch.Text);
		}
	}
}
