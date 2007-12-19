using System;

namespace PetShop.Web
{
	public partial class UserProfile : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				BindUser();
		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			Profile.AccountInfo = AddressForm.Address;
			Profile.Save();

			lblMessage.Text = "Your profile information has been successfully updated.<br>&nbsp;";
		}

		private void BindUser()
		{
			AddressForm.Address = Profile.AccountInfo;
		}
	}
}
