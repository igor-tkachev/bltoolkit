using System;
using System.Web.UI;

using PetShop.BusinessLogic;

namespace PetShop.Web
{
	public partial class Items : PageBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Page.Title = new ProductManager().GetProduct(Request.QueryString["productId"]).Name;
		}
	}
}
