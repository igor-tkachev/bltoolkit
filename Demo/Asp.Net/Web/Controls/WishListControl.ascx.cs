using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PetShop.ObjectModel;

namespace PetShop.Web
{
	public partial class WishListControl : System.Web.UI.UserControl
	{
		/// <summary>
		/// Handle Page load event
		/// </summary>
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
			ICollection<CartItem> wishList = Profile.WishList.Items;

			if (wishList.Count > 0)
			{
				repWishList.DataSource = wishList;
				repWishList.DataBind();
			}
			else
			{
				repWishList.Visible = false;
				lblMsg.Text = "Your wish list is empty.";
			}
		}

		/// <summary>
		/// Handler for Delete/Move buttons
		/// </summary>
		protected void CartItem_Command(object sender, CommandEventArgs e)
		{
			switch (e.CommandName.ToString())
			{
				case "Del":
					Profile.WishList.Remove(e.CommandArgument.ToString());
					break;

				case "Move":
					Profile.WishList.Remove(e.CommandArgument.ToString());
					Profile.ShoppingCart.Add(e.CommandArgument.ToString());
					break;
			}

			Profile.Save();
			BindCart();
		}
	}
}
