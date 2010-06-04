using System;
using System.Web.UI;

using PetShop.BusinessLogic;

namespace PetShop.Web
{
	public partial class Products : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Page.Title = new ProductManager().GetCategory(Request.QueryString["categoryId"]).Name;
		}
	}
}
