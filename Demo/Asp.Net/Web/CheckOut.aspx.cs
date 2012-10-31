using System;
using System.Web.UI.WebControls;

using PetShop.BusinessLogic;
using PetShop.ObjectModel;

namespace PetShop.Web
{
	public partial class CheckOut : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (billingForm.Address == null)
				billingForm.Address = Profile.AccountInfo;
		}

		/// <summary>
		/// Process the order
		/// </summary>
		protected void wzdCheckOut_FinishButtonClick(object sender, WizardNavigationEventArgs e)
		{
			if (Profile.ShoppingCart.Items.Count > 0)
			{
				if (Profile.ShoppingCart.Count > 0)
				{
					// display ordered items
					CartListOrdered.Bind(Profile.ShoppingCart.Items);

					// display total and credit card information
					ltlTotalComplete.Text = ltlTotal.Text;
					ltlCreditCardComplete.Text = ltlCreditCard.Text;

					// create order
					Order order = new Order();

					order.UserID          = User.Identity.Name;
					order.OrderDate       = DateTime.Now;
					order.TotalPrice      = Profile.ShoppingCart.Total;
					order.ShippingAddress = shippingForm.Address;
					order.BillingAddress  = billingForm.Address;
					order.Lines           = Profile.ShoppingCart.GetOrderLineItems();
					order.CreditCard      = GetCreditCardInfo();

					new OrderManager().InsertOrder(order);

					// destroy cart
					Profile.ShoppingCart.Clear();
					Profile.Save();
				}
			}
			else
			{
				lblMsg.Text = "<p><br>Can not process the order. Your cart is empty.</p><p class=SignUpLabel><a class=linkNewUser href=Default.aspx>Continue shopping</a></p>";
				wzdCheckOut.Visible = false;
			}
		}

		/// <summary>
		/// Create CreditCardInfo object from user input
		/// </summary>
		private CreditCard GetCreditCardInfo()
		{
			CreditCard cc = new CreditCard();

			cc.Type       = WebUtility.InputText(listCCType.SelectedValue, 40);
			cc.Expiration = WebUtility.InputText(txtExpDate.Text,           7);
			cc.Number     = WebUtility.InputText(txtCCNumber.Text,         20);

			return cc;
		}

		/// <summary>
		/// Changing Wiszard steps
		/// </summary>
		protected void wzdCheckOut_ActiveStepChanged(object sender, EventArgs e)
		{
			if (wzdCheckOut.ActiveStepIndex == 3)
			{
				billingConfirm. Address = billingForm.Address;
				shippingConfirm.Address = shippingForm.Address;
				ltlTotal.Text           = Profile.ShoppingCart.Total.ToString("c");

				if (txtCCNumber.Text.Length > 4)
					ltlCreditCard.Text = txtCCNumber.Text.Substring(txtCCNumber.Text.Length - 4, 4);
			}
		}

		/// <summary>
		/// Handler for "Ship to Billing Addredd" checkbox.
		/// Prefill/Clear shipping address form.
		/// </summary>
		protected void chkShipToBilling_CheckedChanged(object sender, EventArgs e)
		{
			if (chkShipToBilling.Checked)
				shippingForm.Address = billingForm.Address;
		}

		/// <summary>
		/// Custom validator to check CC expiration date
		/// </summary>
		protected void ServerValidate(object source, ServerValidateEventArgs value)
		{
			DateTime dt;

			if (DateTime.TryParse(value.Value, out dt))
				value.IsValid = dt > DateTime.Now;
			else
				value.IsValid = false;
		}
	}
}
