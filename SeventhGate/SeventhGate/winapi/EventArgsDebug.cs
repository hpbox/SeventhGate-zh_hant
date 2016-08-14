using System;

namespace winapi
{
	/// <summary>
	/// Extends EventArgs class to convey additional info about information
	/// to be logged
	/// </summary>
	public class EventArgsDebug :EventArgs
	{
		/// <summary>
		/// Constructs event argument with default values
		/// </summary>
		public EventArgsDebug()
		{
			this.Level = DebugLevel.Info;
			this.MessageLevelDebug = null;
			this.MessageLevelNormal = null;
			this.ExceptionMessage = null;
			this.ExceptionStackTrace = null;
		}

		/// <summary>
		/// Message displayed on debug window
		/// </summary>
		public string MessageLevelDebug
		{
			get;
			set;
		}

		/// <summary>
		/// Message diplayed in status window
		/// </summary>
		public string MessageLevelNormal
		{
			get;
			set;
		}

		/// <summary>
		/// Message of exception (if any)
		/// </summary>
		public string ExceptionMessage
		{
			get;
			set;
		}

		/// <summary>
		/// Stack trace of exception (if any)
		/// </summary>
		public string ExceptionStackTrace
		{
			get;
			set;
		}

		/// <summary>
		/// Defines debug level
		/// </summary>
		public DebugLevel Level
		{
			get;
			set;
		}
	}

	/// <summary>
	/// Describes 4 levels of log being written
	/// </summary>
	public enum DebugLevel
	{
		/// <summary>
		/// Informational level is shown on main text box
		/// </summary>
		Info,
		/// <summary>
		/// Goes only to debug window
		/// </summary>
		Debug,
		/// <summary>
		/// Goes both to main and debug windows
		/// </summary>
		Error,
		/// <summary>
		/// Goes both to main and debug windows
		/// </summary>
		Exception
	}
}
