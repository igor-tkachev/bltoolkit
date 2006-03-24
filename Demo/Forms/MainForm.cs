using System;
using System.Windows.Forms;

using BLToolkit.Reflection;

using BLToolkit.Demo.ObjectModel;
using BLToolkit.Demo.BusinessLogic;

namespace BLToolkit.Demo.Forms
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			personBinder.List = new PersonManager().SelectAll();
		}

		private void personGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow row = personGridView.Rows[e.RowIndex];

			Person person = (Person)row.DataBoundItem;
			Person clone  = (Person)person.Clone();

			if (new EditPersonForm(clone).ShowDialog() == DialogResult.OK)
			{
				if (clone.IsDirty)
				{
					try
					{
						new PersonManager().Update(clone);

						clone.CopyTo(person);
						person.AcceptChanges();
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}
	}
}