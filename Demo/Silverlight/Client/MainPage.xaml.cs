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
				using (var dm = new DataModel())
				{
					var q =
						from p in dm.Parent
						where p.ParentID <= 5
						select p.ParentID.ToString();

					var text = string.Join(", ", q.ToList().ToArray());

					Dispatcher.BeginInvoke(() => OutputText.Text = text);
				}
			});
		}
	}
}
