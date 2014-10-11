using System;

using PetShop.ObjectModel;

namespace PetShop.Web
{
	public partial class AddressConfirm : System.Web.UI.UserControl
	{
		/// <summary>
		///	Control property to set the address
		/// </summary>
		public Address Address
		{
			set
			{
				if (value != null)
				{
					ltlFirstName.Text = value.FirstName;
					ltlLastName. Text = value.LastName;
					ltlAddress1. Text = value.Line1;
					ltlAddress2. Text = value.Line2;
					ltlCity.     Text = value.City;
					ltlZip.      Text = value.Zip;
					ltlState.    Text = value.State;
					ltlCountry.  Text = value.Country;
					ltlPhone.    Text = value.Phone;
					ltlEmail.    Text = value.Email;
				}
			}
		}
	}
}
