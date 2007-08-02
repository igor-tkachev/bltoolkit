using System;
using System.Web;
using System.Web.UI.WebControls;

using PetShop.BusinessLogic;

namespace PetShop.Web
{
	public partial class ProductsControl : System.Web.UI.UserControl
	{
		protected void PageChanged(object sender, DataGridPageChangedEventArgs e)
		{
			productsList.CurrentPageIndex = e.NewPageIndex;

			string id = Request.QueryString["categoryId"];

			productsList.DataSource = new ProductManager().GetProductListByCategoryID(id);
			productsList.DataBind();
		}
	}
}
