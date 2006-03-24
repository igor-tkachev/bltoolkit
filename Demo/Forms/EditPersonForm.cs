using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using BLToolkit.Demo.ObjectModel;

namespace BLToolkit.Demo.Forms
{
	public partial class EditPersonForm : Form
	{
		public EditPersonForm(Person person)
		{
			InitializeComponent();

			personBinder.Object = person;
		}
	}
}