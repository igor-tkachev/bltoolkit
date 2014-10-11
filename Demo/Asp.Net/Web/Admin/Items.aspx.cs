using System;

using PetShop.BusinessLogic;

public partial class Admin_Items : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		binder.List = new ProductManager().GetAllItemList();
	}
}
