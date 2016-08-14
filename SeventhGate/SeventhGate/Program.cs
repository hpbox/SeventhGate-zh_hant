using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SeventhGate
{
	static class Program
	{
		/// <summary>
		/// The main entry previousPoint for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			try
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new MainWindow());
			}
			catch(Exception e)
			{
				
				throw;
			}
		}
	}
}
