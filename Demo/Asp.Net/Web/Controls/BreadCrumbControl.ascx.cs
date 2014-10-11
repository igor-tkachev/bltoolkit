using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using PetShop.BusinessLogic;

namespace PetShop.Web
{
	public partial class BreadCrumbControl : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			string categoryId = Request.QueryString["categoryId"];

			if (!string.IsNullOrEmpty(categoryId))
			{
				ProcessHomePageLink();

				// Process Product page link
				//
				HtmlAnchor lnkProducts = new HtmlAnchor();

				lnkProducts.InnerText = new ProductManager().GetCategory(categoryId).Name;
				lnkProducts.HRef      = string.Format("~/Products.aspx?page=0&categoryId={0}", categoryId);

				plhControl.Controls.Add(lnkProducts);

				string productId = Request.QueryString["productId"];

				if (!string.IsNullOrEmpty(productId))
				{
					// Process Item page link
					//
					plhControl.Controls.Add(GetDivider());

					HtmlAnchor lnkItemDetails = new HtmlAnchor();

					lnkItemDetails.InnerText = new ProductManager().GetProduct(productId).Name;
					lnkItemDetails.HRef      = string.Format("~/Items.aspx?categoryId={0}&productId={1}", categoryId, productId);

					plhControl.Controls.Add(lnkItemDetails);
				}
			}
			else
			{
				int len = Request.Url.Segments.Length;

				if (len >= 2 && Request.Url.Segments[len-2].TrimEnd('/', '\\').ToLower() == "admin")
				{
					ProcessHomePageLink();

					HtmlAnchor a = new HtmlAnchor();

					a.InnerText = Request.Url.Segments[len - 1].Split('.')[0];
					a.HRef      = Request.Url.PathAndQuery;

					plhControl.Controls.Add(a);
				}
			}
		}

		private void ProcessHomePageLink()
		{
			HtmlAnchor lnkHome = new HtmlAnchor();

			lnkHome.InnerText = "Home";
			lnkHome.HRef      = "~/Default.aspx";

			plhControl.Controls.Add(lnkHome);
			plhControl.Controls.Add(GetDivider());
		}

		/// <summary>
		/// Create a breadcrumb nodes divider
		/// </summary>
		/// <returns>Literal control containing formatted divider</returns>
		private Literal GetDivider()
		{
			Literal ltlDivider = new Literal();

			ltlDivider.Text = "&nbsp;&#062;&nbsp;";

			return ltlDivider;
		}
	}
}