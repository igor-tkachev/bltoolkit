using System;
using System.Linq;
using System.Threading;
using System.Windows.Controls;

namespace Client
{
	public partial class MainPage : UserControl
	{
		public MainPage()
		{
			InitializeComponent();

			ThreadPool.QueueUserWorkItem(_ =>
			{
				try
				{
					using (var dm = new DataModel())
					{
						var q =
							from c in dm.Categories
							where  !c.CategoryName.StartsWith("Con")
							orderby c.CategoryName
							select  c.CategoryName;

						var text = string.Join("\n", q.ToArray());

						Dispatcher.BeginInvoke(() => OutputText.Text = text);
					}
				}
				catch (Exception ex)
				{
					Dispatcher.BeginInvoke(() => OutputText.Text = ex.Message);
				}

				//new ServiceReference1.TestLinqWebServiceSoap();
			});
		}
	}
}
