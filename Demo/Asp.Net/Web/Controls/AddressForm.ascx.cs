using System;
using System.Text.RegularExpressions;

using PetShop.ObjectModel;

namespace PetShop.Web
{
	public partial class AddressForm : System.Web.UI.UserControl
	{
		/// <summary>
		/// Control property to set or get the address
		/// </summary>
		public Address Address
		{
			get
			{
				// Return null if control is empty.
				//
				if (string.IsNullOrEmpty(txtFirstName.Text) &&
					string.IsNullOrEmpty(txtLastName. Text) &&
					string.IsNullOrEmpty(txtAddress1. Text) &&
					string.IsNullOrEmpty(txtAddress2. Text) &&
					string.IsNullOrEmpty(txtCity.     Text) &&
					string.IsNullOrEmpty(txtZip.      Text) &&
					string.IsNullOrEmpty(txtEmail.    Text) &&
					string.IsNullOrEmpty(txtPhone.    Text))
					return null;

				Address addr = new Address();

				// Make sure we clean the input.
				//
				addr.FirstName = WebUtility.InputText(txtFirstName.Text, 50);
				addr.LastName  = WebUtility.InputText(txtLastName. Text, 50);
				addr.Line1     = WebUtility.InputText(txtAddress1. Text, 50);
				addr.Line2     = WebUtility.InputText(txtAddress2. Text, 50);
				addr.City      = WebUtility.InputText(txtCity.     Text, 50);
				addr.Zip       = WebUtility.InputText(txtZip.      Text, 10);
				addr.Phone     = WebUtility.InputText(WebUtility.CleanNonWord(txtPhone.Text), 10);
				addr.Email     = WebUtility.InputText(txtEmail.Text, 80);
				addr.State     = WebUtility.InputText(listState.  SelectedItem.Value,  2);
				addr.Country   = WebUtility.InputText(listCountry.SelectedItem.Value, 50);

				return addr;
			}

			set
			{
				if (value != null)
				{
					txtFirstName.Text = value.FirstName;
					txtLastName. Text = value.LastName;
					txtAddress1. Text = value.Line1;
					txtAddress2. Text = value.Line2;
					txtCity.     Text = value.City;
					txtZip.      Text = value.Zip;
					txtPhone.    Text = value.Phone;
					txtEmail.    Text = value.Email;

					if (!string.IsNullOrEmpty(value.State))
					{
						listState.ClearSelection();
						listState.SelectedValue = value.State;
					}

					if (!string.IsNullOrEmpty(value.Country))
					{
						listCountry.ClearSelection();
						listCountry.SelectedValue = value.Country;
					}
				}
			} 
		}
	}
}
