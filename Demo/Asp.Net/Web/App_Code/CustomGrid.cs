using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PetShop.Web
{
	public class CustomGrid : Repeater
	{
		private static readonly Regex RX = new Regex(@"^&page=\d+", RegexOptions.Compiled);

		protected string _emptyText;
		private   IList  _dataSource;
		private   int    _pageSize = 10;
		private   int    _currentPageIndex;
		private   int    _itemCount;

		public override object DataSource
		{
			set
			{
				// This try catch block is to avoid issues with the VS.NET designer
				// The designer will try and bind a datasource which does not derive from ILIST
				try
				{
					_dataSource = (IList)value;
					ItemCount = _dataSource.Count;
				}
				catch
				{
					_dataSource = null;
					ItemCount = 0;
				}
			}
		}

		public            int PageSize         { get { return _pageSize;         } set { _pageSize         = value; } }
		protected virtual int ItemCount        { get { return _itemCount;        } set { _itemCount        = value; } }
		public    virtual int CurrentPageIndex { get { return _currentPageIndex; } set { _currentPageIndex = value; } }

		protected int    PageCount { get { return (ItemCount - 1) / _pageSize; } }
		public    string EmptyText {                 set { _emptyText = value; } }

		public void SetPage(int index)
		{
			OnPageIndexChanged(new DataGridPageChangedEventArgs(null, index));
		}

		protected override void OnLoad(EventArgs e)
		{
			if (Visible)
			{
				string page  = Context.Request["page"];
				int    index = page != null ? int.Parse(page) : 0;

				SetPage(index);
			}
		}

		/// <summary>
		/// Overridden method to control how the page is rendered
		/// </summary>
		/// <param name="writer"></param>
		protected override void Render(HtmlTextWriter writer)
		{
			// Check there is some data attached
			//
			if (ItemCount == 0)
			{
				writer.Write(_emptyText);
				return;
			}

			// Mask the query
			//
			string query = Context.Request.Url.Query.Replace("?", "&");
			query = RX.Replace(query, string.Empty);

			// Write out the first part of the control, the table header
			//
			writer.Write("<table cellpadding=0 cellspacing=0><tr><td colspan=2>");

			// Call the inherited method
			//
			base.Render(writer);

			// Write out a table row closure
			//
			writer.Write("</td></tr><tr><td class=paging align=left>");

			// Determin whether next and previous buttons are required Previous button?
			//
			if (_currentPageIndex > 0)
				writer.Write(string.Format("<a href=?page={0}>&#060;&nbsp;Previous</a>", _currentPageIndex - 1 + query));

			// Close the table data tag
			//
			writer.Write("</td><td align=right class=paging>");

			// Next button?
			//
			if (_currentPageIndex < PageCount)
				writer.Write(string.Format("<a href=?page={0}>More&nbsp;&#062;</a>", _currentPageIndex + 1 + query));

			// Close the table
			//
			writer.Write("</td></tr></table>");
		}

		protected override void OnDataBinding(EventArgs e)
		{
			// Work out which items we want to render to the page
			//
			int start = CurrentPageIndex * _pageSize;
			int size  = Math.Min(_pageSize, ItemCount - start);

			IList page = new ArrayList();

			// Add the relevant items from the datasource
			//
			for (int i = 0; i < size; i++)
				page.Add(_dataSource[start + i]);

			// Set the base objects datasource
			//
			base.DataSource = page;
			base.OnDataBinding(e);
		}

		public event DataGridPageChangedEventHandler PageIndexChanged;

		protected virtual void OnPageIndexChanged(DataGridPageChangedEventArgs e)
		{
			if (PageIndexChanged != null)
				PageIndexChanged(this, e);
		}
	}
}
