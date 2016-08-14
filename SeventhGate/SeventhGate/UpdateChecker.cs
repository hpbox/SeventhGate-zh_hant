using System;
using winapi;
using System.Xml;
using System.Net;
using System.ComponentModel;

namespace SeventhGate
{
	//TODO revise class
	/// <summary>
	/// Method for asynchronous starting update thread in non-blocking mode
	/// http://www.java2s.com/Code/CSharp/Thread/AsynchronousCallsReturnValues.htm
	/// </summary>
	//public class UpdateChecker :IDisposable
	public static class UpdateChecker
	{
		private static WebClient webClient = new WebClient();
		private static XmlDocument xmlDocument;
		private static bool requiresUpdating;
		private static string updateFileAddress;
		private static Version serverCoreVersion;

		public static void CheckForUpdate(object sender, DoWorkEventArgs e)
		{
			EventArgsUpdate eventUpdateArg = new EventArgsUpdate();
			eventUpdateArg.UpdateAvailable = UpdateAvailable.Error;
			eventUpdateArg.YourVersion = Constants.ProgramVersion.ToString();

			e.Result = eventUpdateArg;

			try
			{
				xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(webClient.DownloadString(Constants.UpdateUri));
			}
			catch(WebException ex)
			{
				Logger.LogException("UpdateChecker", "CheckForUpdate", "檢查更新時發生錯誤，請稍後再重試，或至官方網站獲取更新", ex);
				return;
			}
			catch(NotSupportedException ex)
			{
				Logger.LogException("UpdateChecker", "CheckForUpdate", "檢查更新時發生錯誤，請稍後再重試，或至官方網站獲取更新", ex);
				return;
			}
			catch(XmlException ex)
			{
				Logger.LogException("UpdateChecker", "CheckForUpdate", "檢查更新時發生錯誤，請稍後再重試，或至官方網站獲取更新", ex);
				return;
			}

			foreach(XmlElement item in xmlDocument.GetElementsByTagName("Value"))
			{
				if(item.ParentNode.Name.Equals(Constants.UpdateCoreTag))
				{
					try
					{
						serverCoreVersion = new Version(item.InnerText);
					}
					catch(ArgumentNullException ex)
					{
						Logger.LogException("UpdateChecker", "CheckForUpdate", "檢查更新時發生錯誤，請稍後再重試，或至官方網站獲取更新", ex);
						return;
					}
					catch(ArgumentOutOfRangeException ex)
					{
						Logger.LogException("UpdateChecker", "CheckForUpdate", "檢查更新時發生錯誤，請稍後再重試，或至官方網站獲取更新", ex);
						return;
					}
					catch(ArgumentException ex)
					{
						Logger.LogException("UpdateChecker", "CheckForUpdate", "檢查更新時發生錯誤，請稍後再重試，或至官方網站獲取更新", ex);
						return;
					}
					catch(FormatException ex)
					{
						Logger.LogException("UpdateChecker", "CheckForUpdate", "檢查更新時發生錯誤，請稍後再重試，或至官方網站獲取更新", ex);
						return;
					}
					catch(OverflowException ex)
					{
						Logger.LogException("UpdateChecker", "CheckForUpdate", "檢查更新時發生錯誤，請稍後再重試，或至官方網站獲取更新", ex);
						return;
					}

					requiresUpdating = (Constants.ProgramVersion.CompareTo(serverCoreVersion) == -1);
				}
				else if(item.ParentNode.Name.Equals(Constants.UpdateLinkTag))
				{
					updateFileAddress = item.InnerText;

					if(Uri.IsWellFormedUriString(updateFileAddress, UriKind.RelativeOrAbsolute) == false)
					{
						Logger.LogError("UpdateChecker", "CheckForUpdate", "檢查更新時發生錯誤，請稍後再重試，或至官方網站獲取更新");
						return;
					}
				}
			}

			if(requiresUpdating == true)
			{
				Logger.LogDebug("UpdateChecker", "CheckForUpdate", "有新的版本可供使用: " + serverCoreVersion.ToString());

				eventUpdateArg.UpdateAvailable = UpdateAvailable.True;
				eventUpdateArg.CurrentVersion = serverCoreVersion.ToString();
				eventUpdateArg.YourVersion = Constants.ProgramVersion.ToString();
				eventUpdateArg.UpdateFileAddress = updateFileAddress;
			}
			else
				eventUpdateArg.UpdateAvailable = UpdateAvailable.False;

			CleanConnection();
		}

		/// <summary>
		/// Used for cleanup
		/// </summary>
		public static void CleanConnection()
		{
			serverCoreVersion = null;

			xmlDocument = null;

			if(webClient != null)
				webClient.Dispose();
		}

		public static void Dispose()
		{
			if(webClient != null)
				webClient.Dispose();
		}
	}
}
