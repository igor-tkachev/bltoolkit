using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PetShop.ObjectModel;

namespace PetShop.Web
{
	public partial class ShoppingCartControl : System.Web.UI.UserControl
	{
		protected void Page_PreRender(object sender, EventArgs e)
		{
			if (!IsPostBack)
				BindCart();
		}

		/// <summary>
		/// Bind repeater to Cart object in Profile
		/// </summary>
		private void BindCart()
		{
			ICollection<CartItem> cart = Profile.ShoppingCart.Items;

			if (cart.Count > 0)
			{
				repShoppingCart.DataSource = cart;
				repShoppingCart.DataBind();

				PrintTotal();

				plhTotal.Visible = true;
			}
			else
			{
				repShoppingCart.Visible = false;
				plhTotal.       Visible = false;
				lblMsg.         Text    = "Your cart is empty.";
			}
		}

		/// <summary>
		/// Recalculate the total
		/// </summary>
		private void PrintTotal()
		{
			if (Profile.ShoppingCart.Items.Count > 0)
				ltlTotal.Text = Profile.ShoppingCart.Total.ToString("c");
		}

		/// <summary>
		/// Calculate total
		/// </summary>
		protected void BtnTotal_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			TextBox     txtQuantity;
			ImageButton btnDelete;
			int         qty = 0;

			foreach (RepeaterItem row in repShoppingCart.Items)
			{
				txtQuantity = (TextBox)    row.FindControl("txtQuantity");
				btnDelete   = (ImageButton)row.FindControl("btnDelete");

				if (int.TryParse(WebUtility.InputText(txtQuantity.Text, 10), out qty))
				{
					if (qty > 0)
						Profile.ShoppingCart.SetQuantity(btnDelete.CommandArgument, qty);
					else if (qty == 0)
						Profile.ShoppingCart.Remove(btnDelete.CommandArgument);
				}
			}

			Profile.Save();
			BindCart();
		}

		/// <summary>
		/// Handler for Delete/Move buttons
		/// </summary>
		protected void CartItem_Command(object sender, CommandEventArgs e)
		{
			switch (e.CommandName.ToString())
			{
				case "Del":
					Profile.ShoppingCart.Remove(e.CommandArgument.ToString());
					break;

				case "Move":
					Profile.ShoppingCart.Remove(e.CommandArgument.ToString());
					Profile.WishList.Add(e.CommandArgument.ToString());
					break;
			}

			Profile.Save();
			BindCart();
		}
	}
}
