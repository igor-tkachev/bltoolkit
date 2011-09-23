using System;
using System.Linq;
using System.Threading;
using System.Windows.Controls;

namespace Client
{
	public partial class MainPage : UserControl
	{
		public class Data
		{
			public string Name;
			public int    Sum;
		}

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

						(from t in dm.Categories
						group t by t.CategoryName into g
						select new Data
						{
							Name = g.Key,
							Sum  = g.Sum(a => a.CategoryID)
						}).ToList();

						var text = string.Join("\n", q.ToArray());

						Dispatcher.BeginInvoke(() => OutputText.Text = text);

						dm.BeginBatch();

						dm.CommitBatch();
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
