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
			Application.Run(new MainForm());
		}
	}
}