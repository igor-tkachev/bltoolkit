using System;
using System.Web;
using System.Web.UI.WebControls;

namespace PetShop.Web
{
	public partial class MasterPage : System.Web.UI.MasterPage
	{
		protected void Page_PreRender(object sender, EventArgs e)
		{
			ltlHeader.Text = Page.Header.Title;
			Page.Header.Title = string.Format(".NET Pet Shop :: {0}", Page.Header.Title);
		}

		protected void btnSearch_Click(object sender, EventArgs e)
		{
			WebUtility.SearchRedirect(txtSearch.Text);
		}
	}
}
