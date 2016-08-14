using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SeventhGate
{
	/// <summary>
	/// Class contains all information required for conveying information about program update
	/// between threads
	/// </summary>
	public class EventArgsUpdate :EventArgs
	{
		/// <summary>
		/// Version retrieved from server
		/// </summary>
		public string CurrentVersion
		{
			get;
			set;
		}

		/// <summary>
		/// This program version
		/// </summary>
		public string YourVersion
		{
			get;
			set;
		}

		/// <summary>
		/// Enumeration describing outcome of update checking
		/// </summary>
		public UpdateAvailable UpdateAvailable
		{
			get;
			set;
		}

		/// <summary>
		/// String containing address of file to retrieve for udpate
		/// The string is kept in xml file on the server
		/// </summary>
		public string UpdateFileAddress
		{
			get;
			set;
		}
	}

	/// <summary>
	/// Enum used to describe outcome of update checking
	/// </summary>
	public enum UpdateAvailable
	{
		/// <summary>
		/// There is update available
		/// </summary>
		True,
		/// <summary>
		/// No update available
		/// </summary>
		False,
		/// <summary>
		/// Error while checking for updates
		/// </summary>
		Error
	}
}
