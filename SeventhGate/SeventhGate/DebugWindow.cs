using winapi;
using System.Windows.Forms;
using System.ComponentModel;

namespace SeventhGate
{
	/// <summary>
	/// 
	/// </summary>
	public partial class DebugWindow :Form
	{
		private delegate void UpdateStringCallBack(EventArgsDebug e);
		private UpdateStringCallBack updateStringCallBack;

		/// <summary>
		/// Constructs Debug Window
		/// </summary>
		public DebugWindow()
		{
			InitializeComponent();

			updateStringCallBack = new UpdateStringCallBack(_OnUpdateDebugBox);
			FormClosing += new FormClosingEventHandler(this.DebugWindowClosing);
		}

		private void DebugWindowClosing(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			Hide();
		}

		/// <summary>
		/// Used to populate text box with next entries of debug informatio
		/// </summary>
		/// <param name="text"></param>
		public void Log(EventArgsDebug e)
		{
			if(textBoxDebug.IsDisposed == true)
				return;

			if(textBoxDebug.InvokeRequired)
			{
				this.Invoke(updateStringCallBack, new object[] { e });
			}
			else
			{
				_OnUpdateDebugBox(e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ex"></param>
		private void _OnUpdateDebugBox(EventArgsDebug e)
		{
			if(e.Level == DebugLevel.Info || e.Level == DebugLevel.Debug || e.Level == DebugLevel.Error)
			{
				textBoxDebug.AppendText(e.MessageLevelDebug);
			}

			else if(e.Level == DebugLevel.Exception)
			{
				textBoxDebug.AppendText(e.MessageLevelDebug);
				textBoxDebug.AppendText(e.ExceptionMessage);
				textBoxDebug.AppendText(e.ExceptionStackTrace);
			}
		}
	}
}
