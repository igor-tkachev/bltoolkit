using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using PetShop.BusinessLogic;

namespace PetShop.Web
{
	public partial class SearchControl : UserControl
	{
		protected void PageChanged(object sender, DataGridPageChangedEventArgs e)
		{
			searchList.CurrentPageIndex = e.NewPageIndex;

			string keywordKey = Request.QueryString["keywords"];

			searchList.DataSource = new ProductManager().SearchProducts(keywordKey.Split());
			searchList.DataBind();
		}
	}
}
