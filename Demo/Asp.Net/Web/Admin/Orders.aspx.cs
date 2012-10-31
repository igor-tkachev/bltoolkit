using System;

using PetShop.BusinessLogic;
using PetShop.ObjectModel;

public partial class Admin_Orders : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		binder.List = new OrderManager().GetAllOrderList();
	}

	protected static string FormatAddress(Address addr)
	{
		return string.Format(
			"{0} {1}<br>{2} {3}<br>{4}, {5} {6}, {7}",
			addr.FirstName,
			addr.LastName,
			addr.Line1,
			addr.Line2,
			addr.City,
			addr.State,
			addr.Zip,
			addr.Country);
	}
}
