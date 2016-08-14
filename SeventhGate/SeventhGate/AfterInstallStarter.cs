using System.ComponentModel;
using System.Collections;
using System;
using System.Windows.Forms;
using System.IO;

namespace SeventhGate
{
	[RunInstaller(true)]
	public partial class AfterInstallStarter :System.Configuration.Install.Installer
	{
		public AfterInstallStarter()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Method used to start SeventhGate after installation
		/// (only if user chose to start it)
		/// </summary>
		/// <param name="savedState"></param>
		public override void Commit(IDictionary savedState)
		{
			base.Commit(savedState);

			String args = Context.Parameters["Args"];
			if(String.IsNullOrEmpty(args))
			{
				return;
			}

			String[] argsArray;
			argsArray = args.Split(';');

			try
			{
				//System.Diagnostics.Process.Start(@argsArray[0], @"-debug");
				System.Diagnostics.Process.Start(@argsArray[0]);
			}
			catch(Win32Exception e)
			{
				MessageBox.Show(e.ToString());
			}
			catch(ObjectDisposedException e)
			{
				MessageBox.Show(e.ToString());
			}
			catch(FileNotFoundException e)
			{
				MessageBox.Show(e.ToString());
			}
		}
	}
}
