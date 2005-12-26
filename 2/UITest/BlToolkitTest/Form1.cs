using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BlToolkitTest
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			List<Person> list = new List<Person>();

			typedBindingList1.List = list;

			bindingSource1.DataSource = list;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			
		}
	}
}