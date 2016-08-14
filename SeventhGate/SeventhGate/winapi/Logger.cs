using System;

namespace winapi
{
	/// <summary>
	/// Logging class
	/// </summary>
	public static class Logger
	{
		/// <summary>
		/// Delegate for receiving events conveying debug event arguments
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void DebugEventHandler(object sender, EventArgsDebug e);

		/// <summary>
		/// Debug event
		/// </summary>
		public static event DebugEventHandler DebugEvent;

		/// <summary>
		/// Logs message of level DebugLevel.Info (informative)
		/// </summary>
		/// <param name="message"></param>
		public static void LogInfo(string className, string methodName, string message)
		{
			if(DebugEvent == null)
				return;

			EventArgsDebug eventDebugArg = new EventArgsDebug();
			eventDebugArg.Level = DebugLevel.Info;
			eventDebugArg.MessageLevelNormal = message + "\n";
			eventDebugArg.MessageLevelDebug = DateTime.Now.ToString("HH:mm:ss") + ": " + className + ":" + methodName + ": " + message + "\n";

			DebugEvent(null, eventDebugArg);

			eventDebugArg = null;
		}

		/// <summary>
		/// Logs message of level DebugLevel.Debug (debug)
		/// </summary>
		/// <param name="className"></param>
		/// <param name="methodName"></param>
		/// <param name="message"></param>
		public static void LogDebug(string className, string methodName, string message)
		{
			if(DebugEvent == null)
				return;

			EventArgsDebug eventDebugArg = new EventArgsDebug();
			eventDebugArg.Level = DebugLevel.Debug;
			eventDebugArg.MessageLevelNormal = message + "\n";
			eventDebugArg.MessageLevelDebug = DateTime.Now.ToString("HH:mm:ss") + ": " + className + ":" + methodName + ": " + message + "\n";

			DebugEvent(null, eventDebugArg);

			eventDebugArg = null;
		}

		/// <summary>
		/// Logs error of level DebugLevel.Error (error information, not critical)
		/// </summary>
		/// <param name="message"></param>
		public static void LogError(string className, string methodName, string message)
		{
			if(DebugEvent == null)
				return;

			EventArgsDebug eventDebugArg = new EventArgsDebug();
			eventDebugArg.Level = DebugLevel.Error;
			eventDebugArg.MessageLevelNormal = message + "\n";
			eventDebugArg.MessageLevelDebug = DateTime.Now.ToString("HH:mm:ss") + ": " + className + ":" + methodName + ": " + message + "\n";

			DebugEvent(null, eventDebugArg);

			eventDebugArg = null;
		}

		/// <summary>
		/// Logs error of level DebugLevel.Exception (exception information, critical)
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public static void LogException(string className, string methodName, string message, Exception e)
		{
			if(DebugEvent == null)
				return;

			EventArgsDebug eventDebugArg = new EventArgsDebug();
			eventDebugArg.Level = DebugLevel.Exception;
			eventDebugArg.MessageLevelNormal = message + "\n";
			eventDebugArg.MessageLevelDebug = DateTime.Now.ToString("HH:mm:ss") + ": " + className + ":" + methodName + ": " + message + "\n";
			eventDebugArg.ExceptionMessage = e.Message + "\n";
			eventDebugArg.ExceptionStackTrace = e.StackTrace + "\n";

			DebugEvent(null, eventDebugArg);

			eventDebugArg = null;
		}
	}
}
