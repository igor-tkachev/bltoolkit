using System;
using System.Windows.Forms;

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

		private void Edit(Person person)
		{
			EditPersonForm.Edit(person, delegate(Person p)
			{
				new PersonManager().Update(p);
			});
		}

		private void personGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow row = personGridView.Rows[e.RowIndex];

			Edit((Person)row.DataBoundItem);
		}

		private void edit_Click(object sender, EventArgs e)
		{
			if (personGridView.CurrentRow != null)
				Edit((Person)personGridView.CurrentRow.DataBoundItem);
		}

		private void new_Click(object sender, EventArgs e)
		{
			Person person = EditPersonForm.EditNew(delegate(Person p)
			{
				new PersonManager().Insert(p);
			});

			if (person != null)
				personBinder.List.Add(person);
		}

		private void delete_Click(object sender, EventArgs e)
		{
			if (personGridView.CurrentRow != null)
			{
				Person person = (Person)personGridView.CurrentRow.DataBoundItem;

				try
				{
					UseWaitCursor = true;
					new PersonManager().Delete(person);
					UseWaitCursor = false;

					personBinder.List.Remove(person);
				}
				catch (Exception ex)
				{
					UseWaitCursor = false;
					MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void exit_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}