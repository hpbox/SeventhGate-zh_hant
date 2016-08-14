using System;
using wlan;
using ics;
using System.Collections.ObjectModel;
using winapi;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace SeventhGate {

	//TODO revise class
	/// <summary>
	/// Class delivers high level wrapper for ICS and WLAN class libraries
	/// </summary>
	public class WlanWrapper : IDisposable {

		#region members and initialization

		private bool disposed;

		private WlanManager wlanManager;
		private IcsManager icsManager;

		/// <summary>
		/// Client events channel
		/// </summary>
		public event WlanManager.ClientEventHandler WClientEvent;

		/// <summary>
		/// Network events channel
		/// </summary>
		public event WlanManager.NetworkEventHandler WNetworkEvent;

		/// <summary>
		/// Used in asynchronous network starting
		/// </summary>
		public string NetworkSsid {
			get;
			set;
		}

		/// <summary>
		/// Used in asynchronous network starting
		/// </summary>
		public string NetworkPassword {
			get;
			set;
		}

		/// <summary>
		/// Used in asynchronous network starting
		/// </summary>
		public int NetworkMaxClient {
			get;
			set;
		}

		/// <summary>
		/// Returns amount of private connections
		/// </summary>
		/// <returns></returns>
		public int PrivateConnectionsCount {
			get {
				return icsManager.PrivateConnectionsCount;
			}
		}

		/// <summary>
		/// Constructs WLAN wrapper
		/// </summary>
		public WlanWrapper() {
			wlanManager = new WlanManager();
			icsManager = new IcsManager();
			wlanManager.ClientEvent += new WlanManager.ClientEventHandler(wlanManager_ClientEvent);
			wlanManager.NetworkEvent += new WlanManager.NetworkEventHandler(wlanManager_NetworkEvent);
		}
		#endregion

		#region callbacks

		private void wlanManager_NetworkEvent(object sender, EventArgsNetwork e) {
			//Logger.LogDebug("WlanWrapper", "wlanManager_NetworkEvent", "Network event");
			if(WNetworkEvent != null)
				WNetworkEvent(this, e);
		}

		private void wlanManager_ClientEvent(object sender, EventArgsClient e) {
			//Logger.LogDebug("WlanWrapper", "wlanManager_ClientEvent", "Client event");
			if(WClientEvent != null)
				WClientEvent(this, e);
		}
		#endregion

		#region starting/stopping publicConnection

		/// <summary>
		/// Method used to start network asynchronously
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="ex"></param>
		public void StartNetworkAsynch(object sender, DoWorkEventArgs e) {

			BackgroundWorker worker = null;
			IcsConnection publicConnection = null;
			IcsConnection virtualPrivateConnection = null;

			e.Result = bool.FalseString;

			try {
				worker = (BackgroundWorker)sender;
			} catch(InvalidCastException ex) {
				Logger.LogException("WlanWrapper", "StartNetworkAsynch", "嘗試啟動網路時發生錯誤", ex);
				return;
			}

			worker.ReportProgress(10);

			try {
				publicConnection = (IcsConnection)e.Argument;
			} catch(InvalidCastException ex) {
                Logger.LogException("WlanWrapper", "StartNetworkAsynch", "嘗試啟動網路時發生錯誤", ex);
				worker.Dispose();
				return;
			}

			worker.ReportProgress(20);

			if(!publicConnection.IsOn) {
				Logger.LogError("WlanWrapper", "StartNetworkAsynch", publicConnection.Name + " 已經停止，請將其啟動");
				return;
			}

			worker.ReportProgress(30);

			virtualPrivateConnection = icsManager.VirtualPrivateConnection();

			if(virtualPrivateConnection == null) {
                Logger.LogError("WlanWrapper", "StartNetworkAsynch", "嘗試啟動網路時發生錯誤");
				return;
			}

			worker.ReportProgress(40);

			if(!virtualPrivateConnection.IsOn) {
                Logger.LogError("WlanWrapper", "StartNetworkAsynch", virtualPrivateConnection.Name + " 已經停止，請將其啟動");
				return;
			}

			worker.ReportProgress(50);

			if(!SetConnectionSettings(NetworkSsid, NetworkMaxClient)) {
				Logger.LogError("WlanWrapper", "StartNetworkAsynch", "無法設定 publicConnection 的參數");
				return;
			}

			worker.ReportProgress(60);

			if(!SetPassword(NetworkPassword)) {
                Logger.LogError("WlanWrapper", "StartNetworkAsynch", "無法設定 publicConnection 的參數");
				return;
			}

			worker.ReportProgress(70);

			if(!icsManager.DisableIcsOnAll()) {
				Logger.LogError("WlanWrapper", "StartNetworkAsynch", "準備 ICS 服務時發生錯誤");
			}

			worker.ReportProgress(80);

			if(!icsManager.EnableIcs(publicConnection, virtualPrivateConnection)) {
				Logger.LogError("WlanWrapper", "StartNetworkAsynch", "啟動 ICS 服務時發生錯誤");
				return;
			}

			worker.ReportProgress(90);

			if(wlanManager.StartNetwork() == false) {
                Logger.LogError("WlanWrapper", "StartNetworkAsynch", "嘗試啟動網路時發生錯誤");
				return;
			}

			worker.ReportProgress(100);
			e.Result = bool.TrueString;
		}

		/// <summary>
		/// Method used to start network synchronously
		/// </summary>
		/// <param name="publicConnection"></param>
		/// <returns></returns>
		public bool StartNetworkSynch(IcsConnection publicConnection) {

			IcsConnection virtualPrivateConnection;

			if(!publicConnection.IsOn) {
                Logger.LogError("WlanWrapper", "StartNetworkSynch", publicConnection.Name + " 已經停止，請將其啟動");
				return false;
			}

			virtualPrivateConnection = icsManager.VirtualPrivateConnection();

			if(virtualPrivateConnection == null) {
				Logger.LogError("WlanWrapper", "StartNetworkSynch", "嘗試啟動網路時發生錯誤");
				return false;
			}

			if(!virtualPrivateConnection.IsOn) {
                Logger.LogError("WlanWrapper", "StartNetworkSynch", virtualPrivateConnection.Name + " 已經停止，請將其啟動");
				return false;
			}

			if(!SetConnectionSettings(NetworkSsid, NetworkMaxClient)) {
				Logger.LogError("WlanWrapper", "StartNetworkSynch", "無法設定 publicConnection 的參數");
				return false;
			}

			if(!SetPassword(NetworkPassword)) {
				Logger.LogError("WlanWrapper", "StartNetworkSynch", "無法設定 publicConnection 的參數");
				return false;
			}

			if(!icsManager.DisableIcsOnAll()) {
				Logger.LogError("WlanWrapper", "StartNetworkSynch", "準備 ICS 服務時發生錯誤");
				return false;
			}

			if(!icsManager.EnableIcs(publicConnection, virtualPrivateConnection)) {
				Logger.LogError("WlanWrapper", "StartNetworkSynch", "啟動 ICS 服務時發生錯誤");
				return false;
			}

			return wlanManager.StartNetwork();
		}

		/// <summary>
		/// Used to stop shared publicConnection
		/// </summary>
		/// <returns></returns>
		public bool StopNetwork() {

			//WLAN_HOSTED_NETWORK_CONNECTION_SETTINGS wlanHostedNetworkConnectionSettings = wlanManager.QueryConnectionSettings();

			if(wlanManager.StopNetwork() == false) {
				Logger.LogError("WlanWrapper", "StopNetwork", "嘗試停止網路時發生錯誤");
				return false;
			}

			return true;
		}

		/// <summary>
		/// True if publicConnection is already started
		/// </summary>
		/// <returns></returns>
		public bool IsStarted {
			get {
				return wlanManager.IsStarted;
			}
		}

		#endregion

		#region publicConnection settings

		/// <summary>
		/// Method used to set network parameters
		/// </summary>
		/// <param name="ssid"></param>
		/// <param name="maxPeersNum"></param>
		/// <returns></returns>
		public bool SetConnectionSettings(string ssid, int maxPeersNum) {
			bool result = false;
			WLAN_HOSTED_NETWORK_REASON ret;

			try {
				ret = wlanManager.SetConnectionSettings(ssid, maxPeersNum);

				if(ret != WLAN_HOSTED_NETWORK_REASON.wlan_hosted_network_reason_success)
					Logger.LogError("WlanWrapper", "SetConnectionSettings", "WLAN_HOSTED_NETWORK_REASON = " + ret.ToString());
				else
					result = true;
			} catch(Win32Exception e) {
				Logger.LogException("WlanWrapper", "SetConnectionSettings", "設定 publicConnection 時發生錯誤", e);
			}

			return result;
		}

		/// <summary>
		/// Used to set secondary key password
		/// </summary>
		/// <param name="password"></param>
		/// <returns></returns>
		public bool SetPassword(string password) {
			WLAN_HOSTED_NETWORK_REASON ret;

			try {
				ret = wlanManager.SetSecondaryKey(password, true, true);
				if(ret != WLAN_HOSTED_NETWORK_REASON.wlan_hosted_network_reason_success) {
					Logger.LogError("WlanWrapper", "SetPassword", "WLAN_HOSTED_NETWORK_REASON = " + ret.ToString());
					return false;
				}

				return true;
			} catch(Win32Exception e) {
				Logger.LogException("WlanWrapper", "SetPassword", "設定密碼不成功", e);
				return false;
			}
		}

		/// <summary>
		/// Shows connection information in human-readable form
		/// </summary>
		public string NetworkInfo {
			
			get {
				WlanHostedNetworkStatusHeader status;
				int cnt = 0;

				while(cnt++ < 3) {
					try {
						Thread.Sleep(1000);
						status = wlanManager.QueryStatus();

						string stat = "Interface GUID:\r\n    " + status.IPDeviceID.ToString() + "\r\n";
						stat += "DOT11_MAC_ADDRESS: " + status.wlanHostedNetworkBSSID.ConvertToString() + "\r\n";
						stat += "DOT11_PHY_TYPE: " + status.dot11PhyType.ToString() + "\r\n";
						stat += "ChannelFrequency: " + status.ChannelFrequency.ToString() + "\r\n";
						stat += "NumberOfPeers: " + status.NumberOfPeers.ToString();

						Logger.LogInfo("WlanWrapper", "NetworkInfo", "網路連線詳細資料已獲取");

						return stat;
					} catch(Win32Exception e) {
						Logger.LogException("WlanWrapper", "NetworkInfo", "無法取得網路連線的詳細資料，重試中...", e);
						Thread.Sleep(1000);
					}
				}

				Logger.LogInfo("WlanWrapper", "NetworkInfo", "無法取得網路連線詳細資料");

				return "Network info not available";
			}
		}

		#endregion

		#region listings

		/// <summary>
		/// Collection of all available network helperConnections
		/// NULL if error occured
		/// </summary>
		public Collection<IcsConnection> AllConnections {
			get {
				return icsManager.IcsConnections;
			}
		}

		/// <summary>
		/// Returns collection of all active network helperConnections
		/// </summary>
		/// <returns></returns>
		public Collection<IcsConnection> ActiveConnections {
			get {
				return icsManager.IcsActiveConnections;
			}
		}

		/// <summary>
		/// Returns list of connected clients
		/// </summary>
		public List<Client> Clients {
			get {
				return wlanManager.Clients;
			}
		}

		#endregion

		#region cleanup

		public void Dispose() {
			Dispose(true);
			try {
				GC.SuppressFinalize(this);
			} catch(ArgumentNullException e) {
				Logger.LogException("WlanWrapper", "Dispose", "Error disposing", e);
			}

		}

		private void Dispose(bool disposing) {
			if(!disposed) {
				if(disposing) {
					// Dispose managed resources.
					if(IsStarted)
						StopNetwork();

					if(wlanManager != null) {
						wlanManager.ClientEvent -= wlanManager_ClientEvent;
						wlanManager.NetworkEvent -= wlanManager_NetworkEvent;

						wlanManager.Dispose();
					}

					if(icsManager != null) {
						icsManager.Dispose();
					}
				}

				// Clean up unmanaged resources here.
				disposed = true;
			}
		}

		~WlanWrapper() {
			Dispose(false);
		}

		#endregion
	}
}
