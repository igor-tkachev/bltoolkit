using System;
using System.Web.UI.WebControls;

using PetShop.ObjectModel;

namespace PetShop.Web
{
	public partial class CreditCardForm : System.Web.UI.UserControl
	{
		/// <summary>
		/// Custom validator to check the expiration date
		/// </summary>
		protected void ServerValidate(object source, ServerValidateEventArgs value)
		{
			DateTime dt;

			if (DateTime.TryParse(value.Value, out dt))
				value.IsValid = dt > DateTime.Now;
			else
				value.IsValid = false;
		}

		/// <summary>
		/// Property to set/get credit card info
		/// </summary>
		public CreditCard CreditCard
		{
			get
			{
				CreditCard cc = new CreditCard();

				cc.Type       = WebUtility.InputText(listCctype.SelectedValue, 40);
				cc.Expiration = WebUtility.InputText(txtExpdate. Text,          7);
				cc.Number     = WebUtility.InputText(txtCcnumber.Text,         20);

				return cc;
			}

			set
			{
				if (value != null)
				{
					if (!string.IsNullOrEmpty(value.Number))     txtCcnumber.Text = value.Number;
					if (!string.IsNullOrEmpty(value.Expiration)) txtExpdate. Text = value.Expiration;
					if (!string.IsNullOrEmpty(value.Type))       listCctype.Items.FindByValue(value.Type).Selected = true;	  
				}
			}
		}
	}
}