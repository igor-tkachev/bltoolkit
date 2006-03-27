using System;
using System.Windows.Forms;

using BLToolkit.Demo.Forms;

namespace BLToolkit.Demo
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			MainForm form = new MainForm();

			ToolStripManager.LoadSettings(form, "BLToolkit.Demo");

			form.FormClosing += delegate(object sender, FormClosingEventArgs e)
			{
				ToolStripManager.SaveSettings(form, "BLToolkit.Demo");
			};

			Application.Run(form);
		}
	}
}