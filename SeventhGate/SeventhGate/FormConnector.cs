using System.Windows.Forms;
using System.Drawing;
using System;
using winapi;


namespace SeventhGate
{
	/// <summary>
	/// Class responsible for moving main and debug windows together when close enough side by side
	/// http://efreedom.com/Question/1-3429445/Move-Two-Windows-Forms-Together
	/// </summary>
	public class FormConnector :IDisposable
	{
		private Form mainForm;
		private Form gluedForm;
		private GluedPosition gluedPosition;
		private bool glued = true;

		private bool disposed;

		/// <summary>
		/// Constructs 
		/// </summary>
		/// <param name="mainForm"></param>
		public FormConnector(Form mainForm, Form gluedForm, GluedPosition gluedPosition)
		{
			this.mainForm = mainForm;
			this.gluedForm = gluedForm;
			this.gluedPosition = gluedPosition;

			if(gluedForm != null)
			{
				mainForm.LocationChanged += new EventHandler(mainWindow_LocationChanged);
				gluedForm.LocationChanged += new EventHandler(debugWindow_LocationChanged);
			}
		}

		private void debugWindow_LocationChanged(object sender, EventArgs e)
		{
			if(gluedPosition == GluedPosition.Left)
			{
				if(Math.Abs(gluedForm.Location.X + gluedForm.Width - mainForm.Location.X) < 100 && Math.Abs(mainForm.Location.Y - gluedForm.Location.Y) < 100)
				{
					gluedForm.Location = new Point(mainForm.Location.X - gluedForm.Width, mainForm.Location.Y);
					glued = true;
				}
				else
					glued = false;
			}
			else if(gluedPosition == GluedPosition.Right)
			{
				if(Math.Abs(mainForm.Location.X + mainForm.Width - gluedForm.Location.X) < 100 && Math.Abs(mainForm.Location.Y - gluedForm.Location.Y) < 100)
				{
					gluedForm.Location = new Point(mainForm.Location.X + mainForm.Width, mainForm.Location.Y);
					glued = true;
				}
				else
					glued = false;
			}
			else if(gluedPosition == GluedPosition.Top)
			{
				throw new NotImplementedException();
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		private void mainWindow_LocationChanged(object sender, EventArgs e)
		{
			if(gluedForm == null)
				return;

			if(glued == false || gluedForm.Visible == false)
				return;

			if(gluedPosition == GluedPosition.Left)
			{
				gluedForm.Location = new Point(mainForm.Location.X - gluedForm.Width, mainForm.Location.Y);
			}
			else if(gluedPosition == GluedPosition.Right)
			{
				gluedForm.Location = new Point(mainForm.Location.X + mainForm.Width, mainForm.Location.Y);
			}
			else if(gluedPosition == GluedPosition.Top)
			{
				throw new NotImplementedException();
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>
		/// Moves debug window appropriately
		/// </summary>
		public void ForceGlue()
		{
			if(gluedForm == null)
				return;

			if(gluedPosition == GluedPosition.Left)
			{
				gluedForm.Location = new Point(mainForm.Location.X - gluedForm.Width, mainForm.Location.Y);
			}
			else if(gluedPosition == GluedPosition.Right)
			{
				gluedForm.Location = new Point(mainForm.Location.X + mainForm.Width, mainForm.Location.Y);
			}
			else if(gluedPosition == GluedPosition.Top)
			{
				throw new NotImplementedException();
			}
			else
			{
				throw new NotImplementedException();
			}

			glued = true;
		}

		/// <summary>
		/// Performs cleanup
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			try
			{
				GC.SuppressFinalize(this);
			}
			catch(ArgumentNullException e)
			{
				Logger.LogException("FormConnector", "Dispose", "Error disposing", e);
			}
		}

		/// <summary>
		/// Performs clean-up
		/// </summary>
		private void Dispose(bool disposing)
		{
			if(!disposed)
			{
				if(disposing)
				{
					// Dispose managed resources.

					mainForm.LocationChanged -= mainWindow_LocationChanged;

					if(gluedForm != null)
						gluedForm.LocationChanged -= debugWindow_LocationChanged;

					mainForm = null;
					gluedForm = null;
				}

				// Clean up unmanaged resources here.
				disposed = true;
			}
		}

		//Disposable types should declare finalizer
		~FormConnector()
		{
			Dispose(false);
		}
	}

	/// <summary>
	/// Specifies in which position glue debug window
	/// </summary>
	public enum GluedPosition
	{
		/// <summary>
		/// Glue to the left
		/// </summary>
		Left,
		/// <summary>
		/// Glue to the right
		/// </summary>
		Right,
		/// <summary>
		/// Glue to the top
		/// </summary>
		Top,
		/// <summary>
		/// Glue to the bottom
		/// </summary>
		Bottom
	}
}
