using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System;
using winapi;

namespace SeventhGate
{
	/// <summary>
	/// Class serves general program constants
	/// </summary>
	public static class Constants
	{
		private static DesktopApi.TaskBarEdge taskbarEdge = new DesktopApi.TaskBarEdge();
		private static int taskbarWidth;
		private static int taskbarHeight;

		/// <summary>
		/// Gives main window width
		/// </summary>
		public static int MainWindowWidth = 485;

		/// <summary>
		/// Gives main window height
		/// </summary>
		public static int MainWindowHeight = 796;

		/// <summary>
		/// Property gives X position to show main window at.
		/// Takes into account screen resolution and tasbar layout
		/// </summary>
		public static int MainWindowXPosition
		{
			get
			{
				DesktopApi.GetTaskBarInfo(out taskbarEdge, out taskbarHeight, out taskbarWidth);
				if(taskbarEdge == DesktopApi.TaskBarEdge.Bottom || taskbarEdge == DesktopApi.TaskBarEdge.Top)
				{
					return SystemInformation.PrimaryMonitorSize.Width - MainWindowWidth;
				}
				else if(taskbarEdge == DesktopApi.TaskBarEdge.Left)
				{
					return taskbarHeight;
				}
				else if(taskbarEdge == DesktopApi.TaskBarEdge.Right)
				{
					return SystemInformation.PrimaryMonitorSize.Width - taskbarHeight - MainWindowWidth;
				}
				return 0;
			}
		}

		/// <summary>
		/// Property gives Y position to show main window at.
		/// Takes into account screen resolution and tasbar layout
		/// </summary>
		public static int MainWindowYPosition
		{
			get
			{
				DesktopApi.GetTaskBarInfo(out taskbarEdge, out taskbarHeight, out taskbarWidth);
				if(taskbarEdge == DesktopApi.TaskBarEdge.Left || taskbarEdge == DesktopApi.TaskBarEdge.Right)
				{
					return SystemInformation.PrimaryMonitorSize.Height - MainWindowHeight;
				}
				else if(taskbarEdge == DesktopApi.TaskBarEdge.Top)
				{
					return taskbarHeight;
				}
				else if(taskbarEdge == DesktopApi.TaskBarEdge.Bottom)
				{
					return SystemInformation.PrimaryMonitorSize.Height - taskbarHeight - MainWindowHeight;
				}
				return 0;
			}
		}

		public static Color ColorTextBoxWrongInput
		{
			get
			{
				return Color.FromArgb(255, 191, 191);
			}
		}

		public static Color ColorTextBoxCorrectInput
		{
			get
			{
				return Color.White;
			}
		}

		public static readonly int NetworkNameMax = 32;
		public static readonly int NetworkNameMin = 1;
		public static readonly int NetworkPasswordMax = 63;
		public static readonly int NetworkPassMin = 8;

		public static readonly string StringNetworkNameLabelToolTip = "您欲建立的無線網路名稱(SSID)，長度必須是 1 ~ 32 個字元";
		public static readonly string StringNetworkNameToolTip = "長度是 1 ~ 32 個字元的無線網路名稱(SSID)";
		public static readonly string StringNetworkPassLabelToolTip = "WLAN_NOTIFICATION_SOURCE_SECURITY 用以連接網路必須的密碼\n密碼長度須為 8 ~ 63 個字元";
		public static readonly string StringNetworkPassToolTip = "長度為 8 ~ 63 個字元的密碼";
		public static readonly string StringPeersNumLabelToolTip = "設定同時允許連接的最大用戶數目";
		public static readonly string StringPeersNumToolTip = "本數值依本機無線裝置的規格而有所不同，建議允許 1 ~ 5 個用戶";

		public static readonly string StringAllConnectionsToolTip = "顯示所有網路連線";
		public static readonly string StringAllActiveConnectionsToolTip = "顯示作用中的網路連線";
		public static readonly string StringCheckUpdatesToolTip = "程式啟動時，自動從網路檢查更新";
		public static readonly string StringAnimateTrayIconToolTip = "無線網路啟動時，閃爍工作列圖示";
		public static readonly string StringClientsListToolTip = "目前連接的用戶列表";
		public static readonly string StringStartStopButtonToolTip = "啟動/停止虛擬無線路由器";
		public static readonly string StringPasswordCharButtonToolTip = "顯示/隱藏密碼";

		public static readonly string ConfigStringFileName = "ConfigStore.config";
		public static readonly string ConfigStringNetworkName = "NetworkName";
		public static readonly string ConfigStringNetworkPass = "NetworkPass";
		public static readonly string ConfigStringNetworkPassShown = "NetworkPassShown";
		public static readonly string ConfigStringMaxPeersNum = "MaxPeersNum";
		public static readonly string ConfigStringDebugWindow = "ShowDebugWindow";
		public static readonly string ConfigStringCheckUpdatesAtStart = "CheckUpdateAtStart";
		public static readonly string ConfigStringAnimateTrayIcon = "AnimateTrayIcon";
		public static readonly string ConfigStringLastUsedConnection = "LastUsedConnection";
		public static readonly string ConfigStringStartMinimized = "StartMinimized";

		public static readonly Uri UriHelp = new Uri("http://seventhgate.codeplex.com/");
		public static readonly Uri UriAbout = new Uri("http://seventhgate.codeplex.com/");
		public static readonly Uri UpdateUri = new Uri("http://home.elka.pw.edu.pl/~pgawron/include/SeventhGate/CurrentVersion.xml");
		public static readonly Uri UriTwitter = new Uri("http://twitter.com/home?status=Turn your Windows 7/2008R2 laptop into a Wi-Fi Hotspot using open-source SeventhGate. Check it out at http://seventhgate.codeplex.com/");
		public static readonly string UpdateCoreTag = "Core";
		public static readonly string UpdateLinkTag = "Link";

		public static readonly Version ProgramVersion = Assembly.GetExecutingAssembly().GetName().Version;

		public static readonly double NotifyIconAnimInterval = 800;

		#region Assembly Attribute Accessors

		public static string AssemblyTitle
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if(attributes.Length > 0)
				{
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					if(titleAttribute.Title != "")
					{
						return titleAttribute.Title;
					}
				}
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public static string AssemblyVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		public static string AssemblyDescription
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				if(attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyDescriptionAttribute)attributes[0]).Description;
			}
		}

		public static string AssemblyProduct
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if(attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public static string AssemblyCopyright
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if(attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		public static string AssemblyCompany
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if(attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}
		#endregion

		/// <summary>
		/// Used for class cleanup
		/// </summary>
		public static void Dispose()
		{
		}
	}
}
