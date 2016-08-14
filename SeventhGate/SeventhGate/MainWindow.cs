using System.Windows.Forms;
using System.ComponentModel;
using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Timers;
using System.Globalization;
using System.Diagnostics;
using System.Security;
using System.Resources;
using winapi;
using wlan;
using ics;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace SeventhGate {
	//TODO user guide
	//TODO WPS: menu&interface + saving password in plaintext to config file is surely not safe
	//TODO MAC filtering - possible?
	//TODO Port forwarding
	//TODO option to start network automatically after program start
	//TODO option to turn computer off some time after all devices disconnect
	//TODO installer - add quick launch icon
	//TODO add hiding window on start when minimized
	//TODO show only message box and quit if other instance already running
	//TODO add firewalling from IcsConnection

	public partial class MainWindow : Form {
		/// <summary>
		/// Decides whether network will be started asynchronously or synchronously
		/// </summary>
		private static bool AsynchStart = true;

		private static ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
		private static ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
		private static ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
		private static ToolStripSeparator toolStripSeparator3 = new ToolStripSeparator();

		private static ToolStripMenuItem menuItem1 = new ToolStripMenuItem();
		private static ToolStripMenuItem menuItem3 = new ToolStripMenuItem();
		private static ToolStripMenuItem menuItem4 = new ToolStripMenuItem();
		private static ToolStripMenuItem menuItem5 = new ToolStripMenuItem();
		private static ToolStripMenuItem menuItem6 = new ToolStripMenuItem();
		private static ToolStripMenuItem menuItem7 = new ToolStripMenuItem();

		private static ToolTip toolTipNetworkName = new ToolTip();
		private static ToolTip toolTipNetworkName1 = new ToolTip();
		private static ToolTip toolTipNetworkPass = new ToolTip();
		private static ToolTip toolTipNetworkPass1 = new ToolTip();
		private static ToolTip toolTipPeersNum = new ToolTip();
		private static ToolTip toolTipPeersNum1 = new ToolTip();
		private static ToolTip toolTipAllNetworks = new ToolTip();
		private static ToolTip toolTipActiveNetworks = new ToolTip();
		private static ToolTip toolTipCheckUpdates = new ToolTip();
		private static ToolTip toolTipAnimateTrayIcon = new ToolTip();
		private static ToolTip toolTipClientsList = new ToolTip();
		private static ToolTip toolTipPasswordCharButton = new ToolTip();
		private static ToolTip toolTipStartStopButton = new ToolTip();

		private static Icon[] notifyIconsSet = new Icon[5];
		private static System.Timers.Timer timerNotifyIcon;
		private static NotifyIcon notifyIcon = new NotifyIcon();
		private static int notifyIconAnimCounter;

		private WlanWrapper wlanWrapper;

		private delegate void UpdateStringCallBack(string s);
		private UpdateStringCallBack updateStatusBoxCallBack;

		private delegate void UpdateEmptyCallBack();
		private UpdateEmptyCallBack updateClientListCallBack;

		private delegate void UpdateBoolCallBack(bool b);
		private UpdateBoolCallBack updateUIWhileChangingCallBack;
		private UpdateBoolCallBack updateUIWhileStartedCallBack;

		private static DebugWindow debugWindow;
		private static FormConnector formConnector;

		private static System.Timers.Timer timerUpdateChecker;

		private static BackgroundWorker startNetworkWorker;
		private static BackgroundWorker updateCheckerWorker;

		public MainWindow() {
			try {
				InitializeComponent();

				Constants.MainWindowWidth = Width;
				Constants.MainWindowHeight = Height;

				Location = new Point(Constants.MainWindowXPosition, Constants.MainWindowYPosition);
				FormClosing += new FormClosingEventHandler(this.MainWindowClosing);
				SystemEvents.SessionEnded += new SessionEndedEventHandler(SystemEvents_SessionEnded);

				updateStatusBoxCallBack = new UpdateStringCallBack(_OnUpdateStatusBox);
				Logger.DebugEvent += new Logger.DebugEventHandler(Logger_DebugEvent);
				checkBoxDebugWindow.CheckedChanged += new System.EventHandler(checkBoxDebugWindow_CheckedChanged);
				checkBoxStartMinimized.CheckedChanged += new System.EventHandler(checkBoxStartMinimized_CheckedChanged);
				checkBoxCheckUpdatesAtStart.CheckedChanged += new System.EventHandler(checkBoxCheckUpdatesAtStart_CheckedChanged);

				InitializeVisuals();
				InitializeConfig();
				InitializeCmdParams();

				if(CheckEnvironment() == false)
					return;

				wlanWrapper.WClientEvent += new WlanManager.ClientEventHandler(wlanWrapper_WClientEvent);
				wlanWrapper.WNetworkEvent += new WlanManager.NetworkEventHandler(wlanWrapper_WNetworkEvent);

				this.buttonStartStop.Click += new System.EventHandler(this.buttonStartStop_Click);
				this.textBoxMaxPeersNum.TextChanged += new System.EventHandler(this.textBoxPeersNum_TextChanged);
				this.textBoxMaxPeersNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPeersNum_KeyPress);
				this.buttonPasswordChar.Click += new System.EventHandler(this.buttonPasswordChar_Click);
				this.textBoxNetworkPass.TextChanged += new System.EventHandler(this.textBoxNetworkPass_TextChanged);
				this.textBoxNetworkName.TextChanged += new System.EventHandler(this.textBoxNetworkName_TextChanged);

				this.checkBoxAnimateTray.Enabled = true;
				this.checkBoxAnimateTray.CheckedChanged += new System.EventHandler(this.checkBoxAnimateTray_CheckedChanged);


				this.radioButtonConnectionsAll.CheckedChanged += new System.EventHandler(radioButtonConnectionsAll_CheckedChanged);
				this.radioButtonConnectionsActive.CheckedChanged += new System.EventHandler(radioButtonConnectionsActive_CheckedChanged);

				timerNotifyIcon = new System.Timers.Timer();
				timerNotifyIcon.Elapsed += new ElapsedEventHandler(timerNotifyIcon_Elapsed);
				timerNotifyIcon.Interval = Constants.NotifyIconAnimInterval;

				radioButtonConnectionsActive_CheckedChanged(null, null);
				MarkLastUsedConnection();

				startNetworkWorker = new BackgroundWorker();
				startNetworkWorker.WorkerReportsProgress = true;
				startNetworkWorker.DoWork += new DoWorkEventHandler(wlanWrapper.StartNetworkAsynch);
				startNetworkWorker.ProgressChanged += new ProgressChangedEventHandler(startNetworkWorker_ProgressChanged);
				startNetworkWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(startNetworkWorker_RunWorkerCompleted);

				updateClientListCallBack = new UpdateEmptyCallBack(_OnUpdateClientsList);
				updateUIWhileChangingCallBack = new UpdateBoolCallBack(UI_StateChanging);
				updateUIWhileStartedCallBack = new UpdateBoolCallBack(UI_StateChanged);

				Logger.LogInfo("MainWindow", "MainWindow", "SeventhGate 已啟動");
			} catch(Exception e) {
				//For testing purposes only, shouldn't be here finally
				Logger.LogInfo("MainWindow", "MainWindow", "SeventhGate 初始化失敗:\n" + e.Message + "\n" + e.StackTrace);
			}
		}


		#region program initialization methods

		/// <summary>
		/// Method assigning proper values to some UI elements
		/// </summary>
		private void InitializeVisuals() {
			notifyIconsSet[0] = SeventhGate.Properties.Resources.antenna01;
			notifyIconsSet[1] = SeventhGate.Properties.Resources.antenna02;
			notifyIconsSet[2] = SeventhGate.Properties.Resources.antenna03;
			notifyIconsSet[3] = SeventhGate.Properties.Resources.antenna04;
			notifyIconsSet[4] = SeventhGate.Properties.Resources.antenna_stopped;

			notifyIcon.Icon = notifyIconsSet[4];
			notifyIcon.Visible = true;
			notifyIcon.Text = Constants.AssemblyTitle;
			notifyIcon.MouseClick += new MouseEventHandler(notifyIcon_MouseClick);

			menuItem1.Text = "顯示 SeventhGate 視窗";
			menuItem1.Click += new EventHandler(ContextMenuItemClick);
			menuItem3.Text = "Wi-Fi 保護設定(WPS)";
			menuItem3.Enabled = false;
			menuItem3.Click += new EventHandler(ContextMenuItemClick);
			menuItem4.Text = "說明";
			menuItem4.Click += new EventHandler(ContextMenuItemClick);
			menuItem5.Text = "關於...";
			menuItem5.Click += new EventHandler(ContextMenuItemClick);
			menuItem6.Text = "檢查更新...";
			menuItem6.Click += new EventHandler(ContextMenuItemClick);
			menuItem7.Text = "結束 SeventhGate";
			menuItem7.Click += new EventHandler(ContextMenuItemClick);

			contextMenuStrip.Items.Add(menuItem1);
			contextMenuStrip.Items.Add(toolStripSeparator1);
			contextMenuStrip.Items.Add(menuItem3);
			contextMenuStrip.Items.Add(toolStripSeparator2);
			contextMenuStrip.Items.Add(menuItem4);
			contextMenuStrip.Items.Add(menuItem5);
			contextMenuStrip.Items.Add(menuItem6);
			contextMenuStrip.Items.Add(toolStripSeparator3);
			contextMenuStrip.Items.Add(menuItem7);

			notifyIcon.ContextMenuStrip = contextMenuStrip;

			toolTipNetworkName.IsBalloon = true;
			toolTipNetworkName.SetToolTip(textBoxNetworkName, Constants.StringNetworkNameLabelToolTip);

			toolTipNetworkName1.IsBalloon = true;
			toolTipNetworkName1.SetToolTip(label1, Constants.StringNetworkNameLabelToolTip);

			toolTipNetworkPass.IsBalloon = true;
			toolTipNetworkPass.SetToolTip(textBoxNetworkPass, Constants.StringNetworkPassLabelToolTip);

			toolTipNetworkPass1.IsBalloon = true;
			toolTipNetworkPass1.SetToolTip(label2, Constants.StringNetworkPassLabelToolTip);

			toolTipPeersNum.IsBalloon = true;
			toolTipPeersNum.SetToolTip(label3, Constants.StringPeersNumLabelToolTip);

			toolTipPeersNum1.IsBalloon = true;
			toolTipPeersNum1.SetToolTip(textBoxMaxPeersNum, Constants.StringPeersNumToolTip);

			toolTipAllNetworks.IsBalloon = true;
			toolTipAllNetworks.SetToolTip(radioButtonConnectionsAll, Constants.StringAllConnectionsToolTip);

			toolTipActiveNetworks.IsBalloon = true;
			toolTipActiveNetworks.SetToolTip(radioButtonConnectionsActive, Constants.StringAllActiveConnectionsToolTip);

			toolTipCheckUpdates.IsBalloon = true;
			toolTipCheckUpdates.SetToolTip(checkBoxCheckUpdatesAtStart, Constants.StringCheckUpdatesToolTip);

			toolTipAnimateTrayIcon.IsBalloon = true;
			toolTipAnimateTrayIcon.SetToolTip(checkBoxAnimateTray, Constants.StringAnimateTrayIconToolTip);

			toolTipClientsList.IsBalloon = true;
			toolTipClientsList.SetToolTip(listBoxClients, Constants.StringClientsListToolTip);

			toolTipStartStopButton.IsBalloon = true;
			toolTipStartStopButton.SetToolTip(buttonStartStop, Constants.StringStartStopButtonToolTip);

			toolTipPasswordCharButton.IsBalloon = true;
			toolTipPasswordCharButton.SetToolTip(buttonPasswordChar, Constants.StringPasswordCharButtonToolTip);

			labelAssemblyTitle.Text = Constants.AssemblyTitle;
			labelVersion.Text = "版本 " + Constants.AssemblyVersion;
			labelCompanyName.Text = Constants.AssemblyCompany;
			labelCompanyName.Click += new EventHandler(labelCompanyName_Click);
			labelCopyright.Text = Constants.AssemblyCopyright;
			labelProduct.Text = Constants.AssemblyProduct;
			textBoxDescription.Text = Constants.AssemblyDescription;

			panelTwitter.Click += new EventHandler(panelTwitter_Click);
			//toolTipTwitter.ForeColor = Color.FromArgb(193, 223, 237);

			checkBoxDebugWindow.Visible = false;
			TopMost = true;
		}

		/// <summary>
		/// Method responsible for environment check at program startup
		/// </summary>
		/// <returns></returns>
		private bool CheckEnvironment() {
			Process processCurrent = Process.GetCurrentProcess();
			Process[] processAlike = Process.GetProcessesByName(processCurrent.ProcessName);
			bool alreadyExists = false;

			if(processAlike.Length > 1) {
				alreadyExists = true;
				Logger.LogInfo("MainWindow", "InitializeVisuals", "已經有另一個 SeventhGate 實體正在執行，本軟體僅允許同時執行單一實體");
			}

			foreach(Process pp in processAlike) {
				pp.Close();
				pp.Dispose();
			}

			if(processCurrent != null) {
				//processCurrent.Close();
				processCurrent.Dispose();
			}

			if(alreadyExists == true) {
				UI_DisableAllElements();
				return false;
			}

			if(System.Environment.OSVersion.Version.Major != 6 || System.Environment.OSVersion.Version.Minor != 1) {
				Logger.LogInfo("MainWindow", "InitializeVisuals", "您的 Windows 版本 (" + System.Environment.OSVersion.ToString() + ") 不支援虛擬 WiFi 路由器");
				UI_DisableAllElements();
				return false;
			}

			wlanWrapper = new WlanWrapper();

			if(wlanWrapper.PrivateConnectionsCount < 1) {
				Logger.LogInfo("MainWindow", "InitializeVisuals", "系統沒有作用中的虛擬 WiFi 裝置");
				UI_DisableAllElements();
				return false;
			}

			return true;
		}

		/// <summary>
		/// Method performing config file initialization
		/// </summary>
		private void InitializeConfig() {
			if(ConfigStore.Load(Constants.ConfigStringFileName) == false) {
				Logger.LogInfo("MainWindow", "InitializeApplication", "建立新的配置檔案");
				ConfigStore.NewConfig();
			} else {
				Logger.LogInfo("MainWindow", "InitializeApplication", "已載入配置檔案");
			}

			textBoxNetworkName.Text = ConfigStore.Get(Constants.ConfigStringNetworkName).Value.ToString();
			textBoxNetworkPass.Text = ConfigStore.Get(Constants.ConfigStringNetworkPass).Value.ToString();
			buttonPasswordChar_Click(null, null);
			textBoxMaxPeersNum.Text = ConfigStore.Get(Constants.ConfigStringMaxPeersNum).Value.ToString();

			if(Convert.ToBoolean(ConfigStore.GetValue(Constants.ConfigStringStartMinimized)) == false) {
				checkBoxStartMinimized.Checked = false;
			} else {
				checkBoxStartMinimized.Checked = true;
				//WindowState = FormWindowState.Minimized;
				Hide();
			}

			if(Convert.ToBoolean(ConfigStore.GetValue(Constants.ConfigStringCheckUpdatesAtStart)) == true) {
				checkBoxCheckUpdatesAtStart.Checked = true;

				timerUpdateChecker = new System.Timers.Timer(10000);
				timerUpdateChecker.Enabled = true;
				timerUpdateChecker.Elapsed += new ElapsedEventHandler(CheckForUpdate);
				timerUpdateChecker.Start();
			} else {
				checkBoxCheckUpdatesAtStart.Checked = false;
			}

			checkBoxDebugWindow.Checked = Convert.ToBoolean(ConfigStore.GetValue(Constants.ConfigStringDebugWindow));
			checkBoxAnimateTray.Checked = Convert.ToBoolean(ConfigStore.GetValue(Constants.ConfigStringAnimateTrayIcon));
		}

		private void MarkLastUsedConnection() {
			object o = ConfigStore.GetValue(Constants.ConfigStringLastUsedConnection);
			if(o == null)
				return;
			string lastUsedConnection = o.ToString();
			if(lastUsedConnection.Length > 0) {
				foreach(IcsConnection conn in listBoxNetworkConnections.Items) {
					if(conn.Name.ToUpperInvariant().Equals(lastUsedConnection.ToUpperInvariant())) {
						listBoxNetworkConnections.SelectedItem = conn;
						return;
					}
				}
			}
		}

		/// <summary>
		/// Method checking whether there were any commandline parameters
		/// </summary>
		private void InitializeCmdParams() {
			checkBoxDebugWindow.Visible = false;

			if(Environment.GetCommandLineArgs().Length == 1) {
				checkBoxDebugWindow.Visible = false;
				return;
			} else if(Environment.GetCommandLineArgs().Contains("-debug")) {
				checkBoxDebugWindow.Visible = true;
				debugWindow = new DebugWindow();
				debugWindow.TopMost = true;

				formConnector = new FormConnector(this, debugWindow, GluedPosition.Left);
				debugWindow.FormClosing += new FormClosingEventHandler(debugWindow_FormClosing);

				if(Convert.ToBoolean(ConfigStore.GetValue(Constants.ConfigStringDebugWindow)) == true) {
					debugWindow.Show();
					formConnector.ForceGlue();
				} else {
					debugWindow.Select();
				}

				return;
			}
		}

		#endregion

		#region UI interaction methods and callbacks

		/// <summary>
		/// Event handler responds to clicking company name label
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		void labelCompanyName_Click(object sender, EventArgs e) {
			OpenUri(Constants.UriAbout.ToString());
		}

		void panelTwitter_Click(object sender, EventArgs e) {
			OpenUri(Constants.UriTwitter.ToString());
		}


		/// <summary>
		/// Appends text to status box
		/// </summary>
		/// <param name="text"></param>
		private void _OnUpdateStatusBox(string text) {
			textBoxStatus.AppendText(text);
		}

		/// <summary>
		/// Updates visible list of clients in client list box
		/// </summary>
		private void _OnUpdateClientsList() {
			listBoxClients.Items.Clear();

			foreach(Client client in wlanWrapper.Clients)
				listBoxClients.Items.Add(client.FullId);

			textBoxInfo.Clear();
			textBoxInfo.Text = wlanWrapper.NetworkInfo;
		}

		private void UI_StateChanging(bool starting) {
			buttonStartStop.Enabled = !starting;
			textBoxNetworkName.Enabled = !starting;
			textBoxNetworkPass.Enabled = !starting;
			textBoxMaxPeersNum.Enabled = !starting;
			progressBar.Visible = starting;
		}

		private void UI_StateChanged(bool started) {
			if(started == true) {
				buttonStartStop.Enabled = true;
				buttonStartStop.Text = "停止";
				StartTrayIconAnimation();
			} else {
				buttonStartStop.Enabled = true;
				buttonStartStop.Text = "啟動";
				timerNotifyIcon.Enabled = false;
				StopTrayIconAnimation();
				listBoxClients.Items.Clear();
				textBoxInfo.Clear();
			}
			progressBar.Visible = false;
		}

		/// <summary>
		/// Method responsible for disabling all UI elements
		/// </summary>
		private void UI_DisableAllElements() {
			panel1.Enabled = false;
			textBoxNetworkName.Enabled = false;
			textBoxNetworkPass.Enabled = false;
			textBoxMaxPeersNum.Enabled = false;
			buttonPasswordChar.Enabled = false;
			buttonStartStop.Enabled = false;
		}

		/// <summary>
		/// Shows main window
		/// </summary>
		private void ShowWindow() {
			WindowState = FormWindowState.Normal;
			Location = new Point(Constants.MainWindowXPosition, Constants.MainWindowYPosition);

			if(formConnector != null)
				formConnector.ForceGlue();

			Show();


			if(debugWindow != null && checkBoxDebugWindow.Checked == true) {
				debugWindow.Show();
				debugWindow.BringToFront();
				formConnector.ForceGlue();
			}

			BringToFront();
		}

		/// <summary>
		/// Method responsible for responding to notify icon context menu strip buttons
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		private void ContextMenuItemClick(object sender, EventArgs e) {
			//pokazanie programu
			if(sender.Equals(menuItem1)) {
				ShowWindow();
			}
			//WPS
			if(sender.Equals(menuItem3)) {
			}
				//help
			else if(sender.Equals(menuItem4)) {
				OpenUri(Constants.UriHelp.ToString());
			}
				//about
			else if(sender.Equals(menuItem5)) {
				ShowWindow();
				tabControl.SelectTab(tabAbout);
			}
				//updates
			else if(sender.Equals(menuItem6)) {
				ShowWindow();
				CheckForUpdate(null, null);
			}
				//zamkniecie programu
			else if(sender.Equals(menuItem7)) {
				CloseProgram();
			}
		}

		/// <summary>
		/// Method starts timer responsible for changing notifyicon icon
		/// </summary>
		private void StartTrayIconAnimation() {
			if(Convert.ToBoolean(ConfigStore.GetValue(Constants.ConfigStringAnimateTrayIcon)) == true) {
				timerNotifyIcon.Enabled = true;
				timerNotifyIcon.Start();
			} else {
				timerNotifyIcon.Enabled = false;
				timerNotifyIcon.Stop();

				notifyIcon.Icon = notifyIconsSet[3];
			}
		}

		private void StopTrayIconAnimation() {
			timerNotifyIcon.Enabled = false;
			timerNotifyIcon.Stop();

			notifyIcon.Icon = notifyIconsSet[4];
		}

		#endregion

		#region main window event handlers (non-network)

		/// <summary>
		/// Handles system shutdown event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void SystemEvents_SessionEnded(object sender, SessionEndedEventArgs e) {
			if(e.Reason == SessionEndReasons.Logoff)
				return;

			CloseProgram();
		}

		/// <summary>
		/// Event handler for finish of background updates check
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void updateCheckerWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			EventArgsUpdate eventArgsUpdate = (EventArgsUpdate)e.Result;

			Logger.LogInfo("MainWindow", "updateChecker_UpdateCheckedEvent", "程式版本: " + eventArgsUpdate.YourVersion);

			if(eventArgsUpdate.UpdateAvailable == UpdateAvailable.False) {
				Logger.LogInfo("MainWindow", "updateChecker_UpdateCheckedEvent", "沒有可用的更新");
				return;
			} else if(eventArgsUpdate.UpdateAvailable == UpdateAvailable.Error)
				return;

			string message = "當前程式版本: " + eventArgsUpdate.YourVersion
						+ "\n最新版本: " + eventArgsUpdate.CurrentVersion
						+ "\n是否要下載最新版本的軟體?";

			if(MessageBox.Show(message, "SeventhGate - 有新的軟體更新", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK) {
				Logger.LogInfo("MainWindow", "updateChecker_UpdateCheckedEvent", "正在下載軟體更新...");
				OpenUri(eventArgsUpdate.UpdateFileAddress);
			} else {
				Logger.LogInfo("UpdateChecker", "CheckForUpdate", "下載擱置，您可以隨時重新開始下載");
			}
		}

		/// <summary>
		/// Method responds to interaction with debug window check box
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		private void checkBoxDebugWindow_CheckedChanged(object sender, EventArgs e) {
			if(checkBoxDebugWindow.Checked == true) {
				if(debugWindow != null) {
					debugWindow.Show();
					formConnector.ForceGlue();
				}
				ConfigStore.Set(Constants.ConfigStringDebugWindow, "true");
			} else {
				if(debugWindow != null)
					debugWindow.Hide();

				ConfigStore.Set(Constants.ConfigStringDebugWindow, "false");
			}
		}

		/// <summary>
		/// Method responds to interaction with start minimized check box
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		private void checkBoxStartMinimized_CheckedChanged(object sender, System.EventArgs e) {
			if(checkBoxStartMinimized.Checked == true) {
				ConfigStore.Set(Constants.ConfigStringStartMinimized, "true");
			} else {
				ConfigStore.Set(Constants.ConfigStringStartMinimized, "false");
			}
		}

		/// <summary>
		/// Event handler modifies state of debug window checkbox on debug window close
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		private void debugWindow_FormClosing(object sender, EventArgs e) {
			checkBoxDebugWindow.Checked = false;
		}

		/// <summary>
		/// Responds to changing checked state of check for updates at start
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		private void checkBoxCheckUpdatesAtStart_CheckedChanged(object sender, EventArgs e) {
			if(checkBoxCheckUpdatesAtStart.Checked == true) {
				ConfigStore.Set(Constants.ConfigStringCheckUpdatesAtStart, "true");
			} else {
				ConfigStore.Set(Constants.ConfigStringCheckUpdatesAtStart, "false");
			}
		}

		/// <summary>
		/// Responds to changing checked state of notifyicon animation
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		void checkBoxAnimateTray_CheckedChanged(object sender, System.EventArgs e) {
			if(checkBoxAnimateTray.Checked == true) {
				ConfigStore.Set(Constants.ConfigStringAnimateTrayIcon, "true");

				if(wlanWrapper.IsStarted)
					StartTrayIconAnimation();
			} else {
				ConfigStore.Set(Constants.ConfigStringAnimateTrayIcon, "false");

				if(wlanWrapper.IsStarted)
					StartTrayIconAnimation();
			}
		}

		/// <summary>
		/// Avoids process closing
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		private void MainWindowClosing(object sender, CancelEventArgs e) {
			e.Cancel = true;
			Hide();

			if(debugWindow != null && debugWindow.Visible)
				debugWindow.Hide();
		}

		/// <summary>
		/// Responds to single click on notify icon
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		void notifyIcon_MouseClick(object sender, MouseEventArgs e) {
			if(e.Button == System.Windows.Forms.MouseButtons.Right)
				return;

			if(Visible == true) {
				Hide();
				if(debugWindow != null && checkBoxDebugWindow.Checked == true) {
					debugWindow.Hide();
				}
			} else
				ShowWindow();
		}

		/// <summary>
		/// On main status box logs only events with info level
		/// Reroutes all events to debug window
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		private void Logger_DebugEvent(object sender, EventArgsDebug e) {
			if(e.Level != DebugLevel.Debug) {
				if(textBoxStatus.InvokeRequired) {
					this.Invoke(updateStatusBoxCallBack, new object[] { e.MessageLevelNormal });
				} else {
					textBoxStatus.AppendText(e.MessageLevelNormal);
				}
			}

			if(debugWindow != null)
				debugWindow.Log(e);
		}

		/// <summary>
		/// Responds to event animating notify icon
		/// </summary>
		/// <param name="notificationSource"></param>
		/// <param name="ex"></param>
		private void timerNotifyIcon_Elapsed(object source, ElapsedEventArgs e) {
			if(notifyIconAnimCounter < 3)
				notifyIcon.Icon = notifyIconsSet[notifyIconAnimCounter++];
			else {
				notifyIcon.Icon = notifyIconsSet[notifyIconAnimCounter];
				notifyIconAnimCounter = 0;
			}
		}

		/// <summary>
		/// Event handler for all radio button check/uncheck
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		private void radioButtonConnectionsAll_CheckedChanged(object sender, EventArgs e) {
			if(radioButtonConnectionsAll.Checked == false)
				return;

			listBoxNetworkConnections.DataSource = wlanWrapper.AllConnections;
			listBoxNetworkConnections.DisplayMember = "Name";
			listBoxNetworkConnections.SelectionMode = SelectionMode.None;
		}

		/// <summary>
		/// Event handler for active helperConnections radio button check/uncheck
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		private void radioButtonConnectionsActive_CheckedChanged(object sender, EventArgs e) {
			if(radioButtonConnectionsActive.Checked == false)
				return;

			listBoxNetworkConnections.DataSource = wlanWrapper.ActiveConnections;
			listBoxNetworkConnections.DisplayMember = "Name";
			listBoxNetworkConnections.SelectionMode = SelectionMode.One;
		}

		/// <summary>
		/// Used to validate network Ssid
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		private void textBoxNetworkName_TextChanged(object sender, EventArgs e) {
			if(textBoxNetworkName.Text.Length < Constants.NetworkNameMin || textBoxNetworkName.Text.Length > Constants.NetworkNameMax) {
				textBoxNetworkName.BackColor = Constants.ColorTextBoxWrongInput;
				labelSharingDescription.Text = Constants.StringNetworkNameToolTip + " (" + textBoxNetworkName.TextLength.ToString(CultureInfo.InvariantCulture.NumberFormat) + " characters)";
			} else {
				textBoxNetworkName.BackColor = Constants.ColorTextBoxCorrectInput;
				labelSharingDescription.Text = "";

				if(textBoxNetworkPass.BackColor == Constants.ColorTextBoxWrongInput)
					labelSharingDescription.Text = Constants.StringNetworkPassToolTip + " (" + textBoxNetworkPass.TextLength.ToString(CultureInfo.InvariantCulture.NumberFormat) + " characters)";

				if(textBoxMaxPeersNum.BackColor == Constants.ColorTextBoxWrongInput)
					labelSharingDescription.Text = Constants.StringPeersNumToolTip;
			}
		}

		/// <summary>
		/// Used to validate network password
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		private void textBoxNetworkPass_TextChanged(object sender, EventArgs e) {
			if(textBoxNetworkPass.Text.Length < Constants.NetworkPassMin || textBoxNetworkPass.Text.Length > Constants.NetworkPasswordMax) {
				textBoxNetworkPass.BackColor = Constants.ColorTextBoxWrongInput;
				labelSharingDescription.Text = Constants.StringNetworkPassToolTip + " (" + textBoxNetworkPass.TextLength.ToString(CultureInfo.InvariantCulture.NumberFormat) + " characters)";
			} else {
				textBoxNetworkPass.BackColor = Constants.ColorTextBoxCorrectInput;
				labelSharingDescription.Text = "";

				if(textBoxNetworkName.BackColor == Constants.ColorTextBoxWrongInput)
					labelSharingDescription.Text = Constants.StringNetworkNameToolTip + " (" + textBoxNetworkName.TextLength.ToString(CultureInfo.InvariantCulture.NumberFormat) + " characters)";

				if(textBoxMaxPeersNum.BackColor == Constants.ColorTextBoxWrongInput)
					labelSharingDescription.Text = Constants.StringPeersNumToolTip;
			}
		}

		/// <summary>
		/// Handles peers num user input
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		private void textBoxPeersNum_TextChanged(object sender, EventArgs e) {
			if(textBoxMaxPeersNum.TextLength < 1) {
				textBoxMaxPeersNum.BackColor = Constants.ColorTextBoxWrongInput;
				labelSharingDescription.Text = Constants.StringPeersNumToolTip;
			} else {
				textBoxMaxPeersNum.BackColor = Constants.ColorTextBoxCorrectInput;
				labelSharingDescription.Text = "";
				ConfigStore.Set(Constants.ConfigStringMaxPeersNum, textBoxMaxPeersNum.Text);

				if(textBoxNetworkName.BackColor == Constants.ColorTextBoxWrongInput)
					labelSharingDescription.Text = Constants.StringNetworkNameToolTip + " (" + textBoxNetworkName.TextLength.ToString(CultureInfo.InvariantCulture.NumberFormat) + " characters)";

				if(textBoxNetworkPass.BackColor == Constants.ColorTextBoxWrongInput)
					labelSharingDescription.Text = Constants.StringNetworkPassToolTip + " (" + textBoxNetworkPass.TextLength.ToString(CultureInfo.InvariantCulture.NumberFormat) + " characters)";
			}
		}

		/// <summary>
		/// Method responsible for handling password characters showing/hiding
		/// </summary>
		/// <param name="sender">If null, it is program startup. Else, button was clicked</param>
		/// <param name="ex"></param>
		private void buttonPasswordChar_Click(object sender, EventArgs e) {
			string shown = ConfigStore.Get(Constants.ConfigStringNetworkPassShown).Value;

			if(shown.Equals(bool.TrueString) && sender != null) {
				buttonPasswordChar.Text = "顯示";
				textBoxNetworkPass.PasswordChar = '●';
				ConfigStore.Set(Constants.ConfigStringNetworkPassShown, bool.FalseString);
			} else if(shown.Equals(bool.FalseString) && sender != null) {
				buttonPasswordChar.Text = "隱藏";
				textBoxNetworkPass.PasswordChar = '\0';
				ConfigStore.Set(Constants.ConfigStringNetworkPassShown, bool.TrueString);
			} else if(shown.Equals(bool.TrueString) && sender == null) {
				buttonPasswordChar.Text = "隱藏";
				textBoxNetworkPass.PasswordChar = '\0';
			} else if((shown.Equals(bool.FalseString) && sender == null)) {
				buttonPasswordChar.Text = "顯示";
				textBoxNetworkPass.PasswordChar = '●';
			} else {
				Logger.LogError("MainWindow", "buttonPasswordChar_Click", "未知的比較字串: " + shown);
			}
		}

		/// <summary>
		/// Controls user input in peers number (numbers and backspace only)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		private void textBoxPeersNum_KeyPress(object sender, KeyPressEventArgs e) {
			e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != '\b';
		}

		#endregion

		#region strictly network events

		/// <summary>
		/// Event handler serving network start finish
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		void startNetworkWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			string result = (string)e.Result;
			if(result.Equals(bool.FalseString)) {
				UI_StateChanging(false);
				return;
			}

			UI_StateChanged(true);
			Logger.LogInfo("MainWindow", "startNetworkWorker_RunWorkerCompleted", "主控網路已開始");

			textBoxInfo.Text = wlanWrapper.NetworkInfo;
		}

		/// <summary>
		/// Event handler responding to progress changes of network starting process
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		void startNetworkWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
			Logger.LogDebug("MainWindow", "startNetworkWorker_ProgressChanged", "進度 " + e.ProgressPercentage.ToString());
			progressBar.Value = e.ProgressPercentage;
		}

		/// <summary>
		/// Event handler for start/stop button click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		private void buttonStartStop_Click(object sender, System.EventArgs e) {
			if(wlanWrapper.IsStarted == false) {
				if(!radioButtonConnectionsActive.Checked
					|| listBoxNetworkConnections.SelectedItem == null) {
					Logger.LogError("MainWindow", "buttonStartStop_Click", "選擇要分享的網路連線");
					return;
				}

				if(textBoxNetworkName.BackColor == Constants.ColorTextBoxWrongInput
					|| textBoxNetworkPass.BackColor == Constants.ColorTextBoxWrongInput
					|| textBoxMaxPeersNum.BackColor == Constants.ColorTextBoxWrongInput) {
					Logger.LogError("MainWindow", "buttonStartStop_Click", "請先設定正確的網路參數");
					return;
				}

				int maxClientNum = 0;

				try {
					maxClientNum = Convert.ToInt32(textBoxMaxPeersNum.Text);
				} catch(FormatException ex) {
					Logger.LogException("MainWindow", "buttonStartStop_Click", "不被接受的用戶數限制", ex);
					return;
				} catch(OverflowException ex) {
                    Logger.LogException("MainWindow", "buttonStartStop_Click", "不被接受的用戶數限制", ex);
					return;
				}

				IcsConnection publicConnection;

				try {
					publicConnection = (IcsConnection)listBoxNetworkConnections.SelectedItem;
				} catch(InvalidCastException ex) {
					Logger.LogException("MainWindow", "buttonStartStop_Click", "嘗試取得分享的網路連線時發生錯誤", ex);
					return;
				}

				ConfigStore.Set(Constants.ConfigStringLastUsedConnection, publicConnection.Name);
				Logger.LogInfo("MainWindow", "buttonStartStop_Click", "主控網路已開始，建立在 " + publicConnection.Name + "...");
				UI_StateChanging(true);

				wlanWrapper.NetworkSsid = textBoxNetworkName.Text;
				wlanWrapper.NetworkPassword = textBoxNetworkPass.Text;
				wlanWrapper.NetworkMaxClient = maxClientNum;

				if(AsynchStart == true) {
					//asynchronous network start

					if(startNetworkWorker.IsBusy == false) {
						startNetworkWorker.RunWorkerAsync(publicConnection);
					} else {
						Logger.LogError("MainWindow", "buttonStartStop_Click", "無法呼叫網路啟動器");
						return;
					}
				} else {
					//synchronous network start

					if(wlanWrapper.StartNetworkSynch(publicConnection) == false) {
						UI_StateChanging(false);
						Logger.LogError("MainWindow", "buttonStartStop_Click", "無法開始主控網路");
					} else {
						UI_StateChanged(true);
						Logger.LogInfo("MainWindow", "buttonStartStop_Click", "主控網路已停止");
					}
				}
			} else {
				Logger.LogInfo("MainWindow", "buttonStartStop_Click", "正在停止主控網路...");

				UI_StateChanging(false);

				if(wlanWrapper.StopNetwork() == false) {
					Logger.LogError("MainWindow", "buttonStartStop_Click", "嘗試停止主控網路時發生錯誤");
					UI_StateChanging(false);
					return;
				}

				UI_StateChanged(false);

                Logger.LogInfo("MainWindow", "buttonStartStop_Click", "主控網路已停止");
			}
		}

		/// <summary>
		/// Event handler for events regarding network
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		void wlanWrapper_WNetworkEvent(object sender, wlan.EventArgsNetwork e) {
			if(e.NetworkEventType == NetworkEventType.WifiStart) {
				Logger.LogInfo("MainWindow", "wlanWrapper_WNetworkEvent", "WiFi 介面卡已關閉");
			} else if(e.NetworkEventType == NetworkEventType.WifiStopWhileNetworkOff) {
				Logger.LogInfo("MainWindow", "wlanWrapper_WNetworkEvent", "WiFi 介面卡已啟動");
			} else if(e.NetworkEventType == NetworkEventType.WifiStopWhileNetworkOn) {
				Logger.LogInfo("MainWindow", "wlanWrapper_WNetworkEvent", "WiFi 介面卡在網路連線作用中時被關閉!");
				Logger.LogInfo("MainWindow", "wlanWrapper_WNetworkEvent", "正在停止主控網路...");

				Invoke(updateUIWhileChangingCallBack, new object[] { false });

				if(wlanWrapper.StopNetwork() == false) {
                    Logger.LogError("MainWindow", "wlanWrapper_WNetworkEvent", "嘗試停止主控網路時發生錯誤");
					return;
				}

				Invoke(updateUIWhileStartedCallBack, new object[] { false });

				Logger.LogInfo("MainWindow", "wlanWrapper_WNetworkEvent", "主控網路已停止");
			} else if(e.NetworkEventType == NetworkEventType.Start) {
				//already getting info from invoking methods
			} else if(e.NetworkEventType == NetworkEventType.StartFail) {
				//already getting info from invoking methods
			} else if(e.NetworkEventType == NetworkEventType.Stop) {
				//already getting info from invoking methods
			}
		}

		/// <summary>
		/// Event handler for events regarding clients
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		void wlanWrapper_WClientEvent(object sender, wlan.EventArgsClient e) {
			if(e.ClientEventType == ClientEventType.Join)
				Logger.LogInfo("MainWindow", "wlanWrapper_WClientEvent", "用戶 " + e.Client.MacAddress + " 加入了網路");
			else if(e.ClientEventType == ClientEventType.Leave)
                Logger.LogInfo("MainWindow", "wlanWrapper_WClientEvent", "用戶 " + e.Client.MacAddress + " 離開了網路");
			else if(e.ClientEventType == ClientEventType.StateChange)
                Logger.LogInfo("MainWindow", "wlanWrapper_WClientEvent", "用戶 " + e.Client.MacAddress + " 已改變狀態");
			else if(e.ClientEventType == ClientEventType.Identified)
                Logger.LogInfo("MainWindow", "wlanWrapper_WClientEvent", "用戶 " + e.Client.MacAddress + " 已驗證");


			if(listBoxClients.InvokeRequired) {
				this.Invoke(updateClientListCallBack, null);
			} else {
				_OnUpdateClientsList();
			}
		}

		#endregion

		#region helper methods

		/// <summary>
		/// Method tries to start new process to open given uri
		/// </summary>
		/// <param name="uriAddress"></param>
		private void OpenUri(string uriAddress) {
			Process process = null;

			try {
				process = Process.Start(uriAddress);
			} catch(Win32Exception ex) {
				Logger.LogException("MainWindow", "OpenUri", "無法打開 \"" + uriAddress + "\"", ex);
				/*if(ex.ErrorCode == -2147467259)
					Logger.LogError("MainWindow", "OpenUri", "No browser available");
				else
					Logger.LogException("MainWindow", "OpenUri", "WLAN_NOTIFICATION_SOURCE_HNWK error", ex);*/
			} catch(ObjectDisposedException ex) {
                Logger.LogException("MainWindow", "OpenUri", "無法打開 \"" + uriAddress + "\"", ex);
			} catch(FileNotFoundException ex) {
                Logger.LogException("MainWindow", "OpenUri", "無法打開 \"" + uriAddress + "\"", ex);
			} finally {
				if(process != null) {
					process.Close();
					process.Dispose();
				}
			}
		}

		/// <summary>
		/// Method starts new thread to check for updates
		/// </summary>
		private void CheckForUpdate(object source, ElapsedEventArgs e) {
			if(timerUpdateChecker != null) {
				timerUpdateChecker.Stop();
				timerUpdateChecker.Enabled = false;
				timerUpdateChecker.Elapsed -= CheckForUpdate;
				timerUpdateChecker.Dispose();
			}

			if(updateCheckerWorker == null) {
				updateCheckerWorker = new BackgroundWorker();
				updateCheckerWorker.WorkerReportsProgress = false;
				updateCheckerWorker.DoWork += new DoWorkEventHandler(UpdateChecker.CheckForUpdate);
				updateCheckerWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(updateCheckerWorker_RunWorkerCompleted);
			}

			if(updateCheckerWorker != null && updateCheckerWorker.IsBusy == false) {
				updateCheckerWorker.RunWorkerAsync();
			} else {
				Logger.LogError("MainWindow", "CheckForUpdate", "更新程式忙碌中或不存在");
				return;
			}
		}

		#endregion

		#region cleanup

		/// <summary>
		/// Method performs cleanup on program exit
		/// </summary>
		private void CloseProgram() {
			if(debugWindow != null) {
				debugWindow.FormClosing -= debugWindow_FormClosing;

				if(debugWindow.Visible == true)
					debugWindow.Hide();

				debugWindow.Close();
				debugWindow.Dispose();
			}

			if(formConnector != null)
				formConnector.Dispose();

			if(wlanWrapper != null) {
				if(wlanWrapper.StopNetwork() == false)
					Logger.LogError("MainWindow", "ContextMenuItemClick", "嘗試停止主控網路時發生錯誤");

				wlanWrapper.WClientEvent -= wlanWrapper_WClientEvent;
				wlanWrapper.WNetworkEvent -= wlanWrapper_WNetworkEvent;

				wlanWrapper.Dispose();
			}

			if(startNetworkWorker != null) {
				startNetworkWorker.DoWork -= wlanWrapper.StartNetworkAsynch;
				startNetworkWorker.ProgressChanged -= startNetworkWorker_ProgressChanged;
				startNetworkWorker.RunWorkerCompleted -= startNetworkWorker_RunWorkerCompleted;
				startNetworkWorker.Dispose();
			}

			if(updateCheckerWorker != null) {
				updateCheckerWorker.DoWork -= UpdateChecker.CheckForUpdate;
				updateCheckerWorker.RunWorkerCompleted -= updateCheckerWorker_RunWorkerCompleted;
				updateCheckerWorker.Dispose();
			}

			UpdateChecker.Dispose();

			ConfigStore.Set("NetworkName", textBoxNetworkName.Text);
			ConfigStore.Set("NetworkPass", textBoxNetworkPass.Text);
			ConfigStore.Save(Constants.ConfigStringFileName);

			if(labelCompanyName != null)
				labelCompanyName.Click -= labelCompanyName_Click;

			if(panelTwitter != null)
				panelTwitter.Click -= panelTwitter_Click;

			if(timerNotifyIcon != null) {
				timerNotifyIcon.Enabled = false;
				timerNotifyIcon.Stop();
				timerNotifyIcon.Elapsed -= timerNotifyIcon_Elapsed;
				timerNotifyIcon.Dispose();
			}

			if(notifyIcon != null) {
				notifyIcon.MouseClick -= notifyIcon_MouseClick;
				notifyIcon.Visible = false;
				notifyIcon.Dispose();
			}

			foreach(Icon i in notifyIconsSet)
				i.Dispose();

			if(toolTipNetworkName != null)
				toolTipNetworkName.Dispose();

			if(toolTipNetworkName1 != null)
				toolTipNetworkName1.Dispose();

			if(toolTipNetworkPass != null)
				toolTipNetworkPass.Dispose();

			if(toolTipNetworkPass1 != null)
				toolTipNetworkPass1.Dispose();

			if(toolTipPeersNum != null)
				toolTipPeersNum.Dispose();

			if(toolTipPeersNum1 != null)
				toolTipPeersNum1.Dispose();

			if(toolTipAllNetworks != null)
				toolTipAllNetworks.Dispose();

			if(toolTipActiveNetworks != null)
				toolTipActiveNetworks.Dispose();

			if(toolTipCheckUpdates != null)
				toolTipCheckUpdates.Dispose();

			if(toolTipAnimateTrayIcon != null)
				toolTipAnimateTrayIcon.Dispose();

			if(toolTipClientsList != null)
				toolTipClientsList.Dispose();

			if(toolTipStartStopButton != null)
				toolTipStartStopButton.Dispose();

			if(toolTipPasswordCharButton != null)
				toolTipPasswordCharButton.Dispose();

			if(contextMenuStrip != null) {
				contextMenuStrip.Visible = false;
				contextMenuStrip.Dispose();
			}

			if(menuItem1 != null) {
				menuItem1.Visible = false;
				menuItem1.Dispose();
			}

			if(menuItem3 != null) {
				menuItem3.Visible = false;
				menuItem3.Dispose();
			}

			if(menuItem4 != null) {
				menuItem4.Visible = false;
				menuItem4.Dispose();
			}

			if(menuItem5 != null) {
				menuItem5.Visible = false;
				menuItem5.Dispose();
			}

			if(menuItem6 != null) {
				menuItem6.Visible = false;
				menuItem6.Dispose();
			}

			if(menuItem7 != null) {
				menuItem7.Visible = false;
				menuItem7.Dispose();
			}

			if(toolStripSeparator1 != null)
				toolStripSeparator1.Dispose();

			if(toolStripSeparator2 != null)
				toolStripSeparator2.Dispose();

			if(toolStripSeparator3 != null)
				toolStripSeparator3.Dispose();

			FormClosing -= this.MainWindowClosing;
			SystemEvents.SessionEnded -= SystemEvents_SessionEnded;

			Logger.DebugEvent -= Logger_DebugEvent;

			Constants.Dispose();

			Application.Exit();
			this.Close();
		}

		#endregion
	}


	/*
	 * Linker: adding environment variable LINK with value /fixed:no
	 * http://msdn.microsoft.com/en-us/library/k7137bfe(VS.80).aspx
	 * 
	 * Nie można załadować pliku lub zestawu 'Interop.NETCONLib,
	 * Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
	 * lub jednej z jego zależności. Próbowano załadować program
	 * w niepoprawnym formacie. - set all linking project to x86, not AnyCPU
	 */

	/*
	 * Team explorer:
	 * Start > Settings > Control Panel > WLAN_PROFILE_USER Accounts >
	 *	Zarządzaj poświadczeniami > Poświadczenia system Windows | Poświadczenia rodzajowe
	 * Server: tfs03.codeplex.com
	 * Username: ***
	 * Password: ***
	 */
}
