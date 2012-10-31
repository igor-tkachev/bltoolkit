using System;
using System.Collections.Generic;
using System.Web.UI;

using PetShop.ObjectModel;

namespace PetShop.Web
{
	public partial class CartList : UserControl
	{
		public void Bind(ICollection<CartItem> cart)
		{
			if (cart != null)
			{
				repOrdered.DataSource = cart;
				repOrdered.DataBind();
			}
		}
	}
}