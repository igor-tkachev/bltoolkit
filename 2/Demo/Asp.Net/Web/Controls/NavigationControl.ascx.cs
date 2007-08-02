using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using PetShop.BusinessLogic;

namespace PetShop.Web
{
	public partial class NavigationControl : UserControl
	{
		// Control layout property.
		//
		private   string _controlStyle;
		protected string  ControlStyle
		{
			get { return _controlStyle; }
		}

		// Get properties based on control consumer.
		//
		protected void GetControlStyle()
		{
			if (Request.ServerVariables["SCRIPT_NAME"].ToLower().IndexOf("default.aspx") > 0)
				_controlStyle = "navigationLinks";
			else
				_controlStyle = "mainNavigation";
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			GetControlStyle();
			BindCategories();

			// Select current category.
			//
			string categoryId = Request.QueryString["categoryId"];

			if (!string.IsNullOrEmpty(categoryId))
				SelectCategory(categoryId);
		}

		// Select current category.
		//
		private void SelectCategory(string categoryId)
		{
			foreach (RepeaterItem item in repCategories.Items)
			{
				HiddenField hidCategoryId = (HiddenField)item.FindControl("hidCategoryId");

				if (hidCategoryId.Value.ToLower() == categoryId.ToLower())
				{
					HyperLink lnkCategory = (HyperLink)item.FindControl("lnkCategory");

					lnkCategory.ForeColor = System.Drawing.Color.FromArgb(199, 116, 3);

					break;
				}
			}
		}

		// Bind categories.
		//
		private void BindCategories()
		{
			repCategories.DataSource = new ProductManager().GetCategoryList();
			repCategories.DataBind();
		}
	}
}