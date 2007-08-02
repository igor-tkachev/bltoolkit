using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using PetShop.BusinessLogic;

namespace PetShop.Web
{
	public partial class ItemsControl : UserControl
	{
		protected void PageChanged(object sender, DataGridPageChangedEventArgs e)
		{
			itemsGrid.CurrentPageIndex = e.NewPageIndex;

			string id = Request.QueryString["productId"];

			itemsGrid.DataSource = new ProductManager().GetItemListByProductID(id);
			itemsGrid.DataBind();
		}
	}
}
