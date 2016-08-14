using System;
using winapi;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Timers;
using System.Threading;
using System.Security;

namespace wlan {

	/// <summary>
	/// Class used to set wireless Hosted Network parameters and start/stop the Hosted Network.
	/// 
	/// WlanManager static library is used to initialize settings
	/// for the wireless HostedNetwork application, start and stop the Hosted Network.
	/// This library is used to set wireless Hosted Network parameters (SSID and Key,
	/// for example), query the settings of Hosted Network, start and stop the Hosted Network,
	/// and  handle the notification events from Hosted network.
	/// </summary>
	public class WlanManager : IDisposable {

		private bool disposed;
		private uint serverVersion;
		private IntPtr wlanHandle;

		private WlanNotificationSource notificationSource;
		private Imports.WlanNotificationCallback wlanNotificationCallback;

		/// <summary>
		/// Stores references to current set of clients
		/// </summary>
		private List<Client> clients = new List<Client>();

		/// <summary>
		/// Enqueues events for clients
		/// </summary>
		private ThreadSafeQueue<EventArgsClient> clientEventsQueue = new ThreadSafeQueue<EventArgsClient>(10);

		/// <summary>
		/// Thread consuming events for clients
		/// </summary>
		private Thread clientEventsConsumer;

		/// <summary>
		/// Delegate for receiving notifications on client events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void ClientEventHandler(object sender, EventArgsClient e);

		/// <summary>
		/// Delegate for receiving notifications on network events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void NetworkEventHandler(object sender, EventArgsNetwork e);

		/// <summary>
		/// Raised on hosted network event
		/// </summary>
		public event NetworkEventHandler NetworkEvent;

		/// <summary>
		/// Raised on client event: join, leave, stateChange
		/// </summary>
		public event ClientEventHandler ClientEvent;

		/// <summary>
		/// Constructs WlanManager instance
		/// </summary>
		public WlanManager() {
			try {
				wlanNotificationCallback = new Imports.WlanNotificationCallback(OnNotification);

				serverVersion = OpenHandle(ref wlanHandle);
				Logger.LogDebug("WlanManager", "WlanManager", "Server version: " + serverVersion.ToString());

				RegisterVirtualStationNotification();

				notificationSource = RegisterNotification(wlanHandle);
				Logger.LogDebug("WlanManager", "WlanManager", "Notification source: " + notificationSource.ToString());

				InitSettings();

				clientEventsConsumer = new Thread(new ThreadStart(ClientEventsConsumer));
				clientEventsConsumer.Start();
			} catch(Win32Exception e) {
				Logger.LogException("WlanManager", "WlanManager", "Error initializing wlan manager", e);

				if(wlanHandle != null)
					Imports.WlanCloseHandle(wlanHandle, IntPtr.Zero);
			} catch(COMException e) {
				Logger.LogException("WlanManager", "WlanManager", "Error initializing wlan manager", e);

				if(wlanHandle != null)
					Imports.WlanCloseHandle(wlanHandle, IntPtr.Zero);
			}
		}

		#region imports

		private uint OpenHandle(ref IntPtr handle) {
			uint serverVersion;
			Utils.ThrowOnWin32Error(Imports.WlanOpenHandle(Imports.WLAN_CLIENT_VERSION_LONGHORN, IntPtr.Zero, out serverVersion, out wlanHandle));
			return serverVersion;
		}

		private WlanNotificationSource RegisterNotification(IntPtr handle) {
			WlanNotificationSource notificationSource;
			Utils.ThrowOnWin32Error(Imports.WlanRegisterNotification(handle, WlanNotificationSource.WLAN_NOTIFICATION_SOURCE_ALL, true, wlanNotificationCallback, IntPtr.Zero, IntPtr.Zero, out notificationSource));
			return notificationSource;
		}

		private void RegisterVirtualStationNotification() {
			Utils.ThrowOnWin32Error(Imports.WlanRegisterVirtualStationNotification(wlanHandle, true, IntPtr.Zero));
		}

		private WLAN_HOSTED_NETWORK_REASON InitSettings() {
			WLAN_HOSTED_NETWORK_REASON failReason;
			Utils.ThrowOnWin32Error(Imports.WlanHostedNetworkInitSettings(wlanHandle, out failReason, IntPtr.Zero));
			return failReason;
		}

		/// <summary>
		/// Forces hosted network start
		/// Makes the network start so that it will continue running even after the app exits
		/// DOES require be an Administrator
		/// </summary>
		/// <returns></returns>
		private WLAN_HOSTED_NETWORK_REASON ForceStart() {
			WLAN_HOSTED_NETWORK_REASON failReason;
			Utils.ThrowOnWin32Error(Imports.WlanHostedNetworkForceStart(wlanHandle, out failReason, IntPtr.Zero));
			return failReason;
		}

		/// <summary>
		/// Forces hosted network stop
		/// </summary>
		/// <returns></returns>
		private WLAN_HOSTED_NETWORK_REASON ForceStop() {
			WLAN_HOSTED_NETWORK_REASON failReason;
			Utils.ThrowOnWin32Error(Imports.WlanHostedNetworkForceStop(wlanHandle, out failReason, IntPtr.Zero));
			return failReason;
		}

		/// <summary>
		/// Connects to a network defined by a connection parameters structure.
		/// </summary>
		/// <returns></returns>
		private WLAN_HOSTED_NETWORK_REASON StartUsing() {
			WLAN_HOSTED_NETWORK_REASON failReason;
			Utils.ThrowOnWin32Error(Imports.WlanHostedNetworkStartUsing(wlanHandle, out failReason, IntPtr.Zero));
			return failReason;
		}

		/// <summary>
		/// Makes this app stop using the network.  If the network was started with WlanHostedNetworkStartUsing,
		/// then this might cause it to exit, if we were the last app using it.  If it was force started,
		/// then it will keep running whether we're here or not.
		/// </summary>
		/// <returns></returns>
		private WLAN_HOSTED_NETWORK_REASON StopUsing() {
			WLAN_HOSTED_NETWORK_REASON failReason;
			Utils.ThrowOnWin32Error(Imports.WlanHostedNetworkStopUsing(wlanHandle, out failReason, IntPtr.Zero));
			return failReason;
		}

		/// <summary>
		/// Used to set connection settings
		/// </summary>
		/// <param name="hostedNetworkSsid"></param>
		/// <param name="maxNumberOfPeers"></param>
		/// <returns></returns>
		public WLAN_HOSTED_NETWORK_REASON SetConnectionSettings(string hostedNetworkSsid, int maxNumberOfPeers) {

			WLAN_HOSTED_NETWORK_REASON failReason;
			WLAN_HOSTED_NETWORK_CONNECTION_SETTINGS settings = new WLAN_HOSTED_NETWORK_CONNECTION_SETTINGS();
			settings.hostedNetworkSSID = Utils.ConvertStringToSsid(hostedNetworkSsid);
			settings.dwMaxNumberOfPeers = (uint)maxNumberOfPeers;

			IntPtr settingsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(settings));
			Marshal.StructureToPtr(settings, settingsPtr, false);

			Utils.ThrowOnWin32Error(Imports.WlanHostedNetworkSetProperty(wlanHandle, WLAN_HOSTED_NETWORK_OPCODE.wlan_hosted_network_opcode_connection_settings, (uint)Marshal.SizeOf(settings), settingsPtr, out failReason, IntPtr.Zero));
			return failReason;
		}

		/// <summary>
		/// Requires elevated privileges
		/// </summary>
		/// <param name="enable"></param>
		public void SetEnabled(bool enable) {
			WLAN_HOSTED_NETWORK_REASON failReason;
			IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(enable));
			Marshal.StructureToPtr(enable, ptr, false);
			Utils.ThrowOnWin32Error(Imports.WlanHostedNetworkSetProperty(wlanHandle, WLAN_HOSTED_NETWORK_OPCODE.wlan_hosted_network_opcode_enable, (uint)Marshal.SizeOf(enable), ptr, out failReason, IntPtr.Zero));
		}

		//TODO could be enhanced to receive full list of client's states
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public WlanHostedNetworkStatusHeader QueryStatus() {
			IntPtr dataPtr;
			Utils.ThrowOnWin32Error(Imports.WlanHostedNetworkQueryStatus(wlanHandle, out dataPtr, IntPtr.Zero));
			return (WlanHostedNetworkStatusHeader)Marshal.PtrToStructure(dataPtr, typeof(WlanHostedNetworkStatusHeader));
		}

		/// <summary>
		/// Used to set secondary key for network
		/// </summary>
		/// <param name="passKey"></param>
		/// <returns></returns>
		public WLAN_HOSTED_NETWORK_REASON SetSecondaryKey(string passKey, bool isPassPhrase, bool isPersistent) {
			WLAN_HOSTED_NETWORK_REASON failReason;

			uint length = (uint)passKey.Length;
			if(isPassPhrase == true)
				length++;

			Utils.ThrowOnWin32Error(Imports.WlanHostedNetworkSetSecondaryKey(wlanHandle, length, passKey, isPassPhrase, isPersistent, out failReason, IntPtr.Zero));
			return failReason;
		}

		public WLAN_HOSTED_NETWORK_REASON QuerySecondaryKey(out uint length, out string passKey, out bool isPassPhrase, out bool isPersistent) {
			WLAN_HOSTED_NETWORK_REASON failReason;
			Utils.ThrowOnWin32Error(Imports.WlanHostedNetworkQuerySecondaryKey(wlanHandle, out length, out passKey, out isPassPhrase, out isPersistent, out failReason, IntPtr.Zero));
			return failReason;
		}

		/// <summary>
		/// Used to retrieve connection settings
		/// </summary>
		/// <param name="hostedNetworkSsid"></param>
		/// <param name="maxNumberOfPeers"></param>
		/// <returns></returns>
		public WLAN_HOSTED_NETWORK_CONNECTION_SETTINGS QueryConnectionSettings() {
			uint dataSize;
			IntPtr dataPtr;
			WLAN_OPCODE_VALUE_TYPE opcode;

			Utils.ThrowOnWin32Error(Imports.WlanHostedNetworkQueryProperty(wlanHandle, WLAN_HOSTED_NETWORK_OPCODE.wlan_hosted_network_opcode_connection_settings, out dataSize, out dataPtr, out opcode, IntPtr.Zero));
			return (WLAN_HOSTED_NETWORK_CONNECTION_SETTINGS)Marshal.PtrToStructure(dataPtr, typeof(WLAN_HOSTED_NETWORK_CONNECTION_SETTINGS));
		}

		#endregion

		#region starting / stopping network

		/// <summary>
		/// Used to start hosted network
		/// </summary>
		/// <returns></returns>
		public bool StartNetwork() {
			WLAN_HOSTED_NETWORK_REASON ret;

			try {
				if(IsStarted == true) {
					ret = ForceStop();
					//ret = StopUsing();

					if(ret != WLAN_HOSTED_NETWORK_REASON.wlan_hosted_network_reason_success) {
						Logger.LogError("WlanManager", "StartNetwork", "Error stopping connection " + ret.ToString());
						return false;
					}
				}

				ret = StartUsing();

				if(ret != WLAN_HOSTED_NETWORK_REASON.wlan_hosted_network_reason_success) {
					Logger.LogError("WlanManager", "StartNetwork", "Error initializing " + ret.ToString());
					ForceStop();
					//StopUsing();
					return false;
				}
				return true;
			} catch(Win32Exception e) {
				if(e.NativeErrorCode == 5023) {
					Logger.LogException("WlanManager", "StartNetwork", "Is your wifi card started?", e);
				} else {
					Logger.LogException("WlanManager", "StartNetwork", "Error starting hosted network", e);
				}
				return false;
			}
		}

		/// <summary>
		/// Used to stop hosted network
		/// </summary>
		/// <returns></returns>
		public bool StopNetwork() {
			try {
				foreach(Client client in clients) {
					if(client != null) {
						DnsInterface.ClearArpCache(client.IpAddress);
						client.Dispose();
					}
				}
				clients.Clear();

				WLAN_HOSTED_NETWORK_REASON ret = ForceStop();
				//WLAN_HOSTED_NETWORK_REASON ret = StopUsing();

				if(ret != WLAN_HOSTED_NETWORK_REASON.wlan_hosted_network_reason_success) {
					Logger.LogError("WlanManager", "StopNetwork", "Error stopping connection " + ret.ToString());
					return false;
				}

			} catch(Win32Exception e) {
				Logger.LogException("WlanManager", "StopNetwork", "Error stopping hosted network", e);
				return false;
			}

			return true;
		}

		#endregion

		/// <summary>
		/// Receives and dispatches notification messages on wlan hosted network
		/// </summary>
		/// <param name="notificationData"></param>
		/// <param name="context"></param>
		private void OnNotification(ref WLAN_NOTIFICATION_DATA notificationData, IntPtr context) {
			switch(notificationData.notificationSource) {
				case WlanNotificationSource.WLAN_NOTIFICATION_SOURCE_ACM:

					WLAN_NOTIFICATION_ACM acm_code = (WLAN_NOTIFICATION_ACM)notificationData.notificationCode;
					Logger.LogDebug("WlanManager", "OnNotification", "WLAN_NOTIFICATION_SOURCE_ACM " + acm_code.ToString());

					switch(acm_code) {
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_autoconf_enabled:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_autoconf_disabled:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_background_scan_enabled:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_background_scan_disabled:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_bss_type_change:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_power_setting_change:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_scan_complete:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_scan_fail:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_connection_start:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_connection_complete:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_connection_attempt_fail:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_filter_list_change:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_interface_arrival:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_interface_removal:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_profile_change:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_profile_name_change:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_profiles_exhausted:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_network_available:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_network_not_available:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_disconnected:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_disconnecting:
							break;
						case WLAN_NOTIFICATION_ACM.wlan_notification_acm_adhoc_network_state_change:
							break;
					}
					break;

				case WlanNotificationSource.WLAN_NOTIFICATION_SOURCE_MSM:

					WLAN_NOTIFICATION_MSM msm_code = (WLAN_NOTIFICATION_MSM)notificationData.notificationCode;
					Logger.LogDebug("WlanManager", "OnNotification", "WLAN_NOTIFICATION_SOURCE_MSM " + msm_code.ToString());

					switch(msm_code) {
						case WLAN_NOTIFICATION_MSM.wlan_notification_msm_adapter_operation_mode_change:
							break;
						case WLAN_NOTIFICATION_MSM.wlan_notification_msm_adapter_removal:
							break;
						case WLAN_NOTIFICATION_MSM.wlan_notification_msm_associated:
							break;
						case WLAN_NOTIFICATION_MSM.wlan_notification_msm_associating:
							break;
						case WLAN_NOTIFICATION_MSM.wlan_notification_msm_authenticating:
							break;
						case WLAN_NOTIFICATION_MSM.wlan_notification_msm_connected:
							break;
						case WLAN_NOTIFICATION_MSM.wlan_notification_msm_disassociating:
							break;
						case WLAN_NOTIFICATION_MSM.wlan_notification_msm_disconnected:
							break;
						case WLAN_NOTIFICATION_MSM.wlan_notification_msm_peer_join:
							break;
						case WLAN_NOTIFICATION_MSM.wlan_notification_msm_peer_leave:
							break;
						case WLAN_NOTIFICATION_MSM.wlan_notification_msm_radio_state_change:
							/*if(NetworkEvent != null)
								NetworkEvent(this, new EventArgsNetwork(NetworkEventType.WifiStopWhileNetworkOff));*/
							break;
						case WLAN_NOTIFICATION_MSM.wlan_notification_msm_roaming_start:
							break;
						case WLAN_NOTIFICATION_MSM.wlan_notification_msm_roaming_end:
							break;
						case WLAN_NOTIFICATION_MSM.wlan_notification_msm_signal_quality_change:
							Logger.LogInfo("WlanMgr", "OnNotification", "Signal quality changed");
							break;
					}

					break;
				//TODO add proper enums
				case WlanNotificationSource.WLAN_NOTIFICATION_SOURCE_IHV:
					Logger.LogDebug("WlanManager", "OnNotification", "WLAN_NOTIFICATION_SOURCE_IHV" + notificationData.notificationCode.ToString());
					break;



				case WlanNotificationSource.WLAN_NOTIFICATION_SOURCE_SECURITY:
					Logger.LogDebug("WlanManager", "OnNotification", "WLAN_NOTIFICATION_SOURCE_SECURITY" + notificationData.notificationCode.ToString());
					break;

				case WlanNotificationSource.WLAN_NOTIFICATION_SOURCE_HNWK:
					//Logger.LogDebug("WlanManager", "OnNotification", "WLAN_NOTIFICATION_SOURCE_HNWK " + notificationData.notificationCode.ToString());

					switch(notificationData.notificationCode) {
						case (int)WLAN_HOSTED_NETWORK_NOTIFICATION_CODE.wlan_hosted_network_state_change:

							WLAN_HOSTED_NETWORK_STATE_CHANGE nSC = (WLAN_HOSTED_NETWORK_STATE_CHANGE)Marshal.PtrToStructure(notificationData.dataPtr, typeof(WLAN_HOSTED_NETWORK_STATE_CHANGE));

							//wifi działa, włączanie sieci (etap drugi)
							if(nSC.OldState == WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_idle
								&& nSC.NewState == WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_active
								&& nSC.Reason == WLAN_HOSTED_NETWORK_REASON.wlan_hosted_network_reason_user_action) {
								Logger.LogDebug("WlanManager", "OnNotification", "Network started");

								if(NetworkEvent != null)
									NetworkEvent(this, new EventArgsNetwork(NetworkEventType.Start));
							}
								//wifi działa, wyłączanie sieci
							else if(nSC.OldState == WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_active
								&& nSC.NewState == WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_idle
								&& nSC.Reason == WLAN_HOSTED_NETWORK_REASON.wlan_hosted_network_reason_user_action) {
								Logger.LogDebug("WlanManager", "OnNotification", "Network stopped");

								if(NetworkEvent != null)
									NetworkEvent(this, new EventArgsNetwork(NetworkEventType.Stop));
							}
								//wifi nie działa, próba włączenia sieci
							else if(nSC.OldState == WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_unavailable
								&& nSC.NewState == WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_unavailable
								&& nSC.Reason == WLAN_HOSTED_NETWORK_REASON.wlan_hosted_network_reason_properties_change) {
								Logger.LogDebug("WlanManager", "OnNotification", "Network failed to start");

								if(NetworkEvent != null)
									NetworkEvent(this, new EventArgsNetwork(NetworkEventType.StartFail));
							}
								//włączanie wifi
							else if(nSC.OldState == WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_unavailable
								&& nSC.NewState == WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_idle
								&& nSC.Reason == WLAN_HOSTED_NETWORK_REASON.wlan_hosted_network_reason_miniport_started) {
								Logger.LogDebug("WlanManager", "OnNotification", "Wifi on");

								if(NetworkEvent != null)
									NetworkEvent(this, new EventArgsNetwork(NetworkEventType.WifiStart));
							}
								//wyłączanie wifi w trakcie, pierwsza faza
							else if(nSC.OldState == WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_active
								&& nSC.NewState == WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_idle
								&& nSC.Reason == WLAN_HOSTED_NETWORK_REASON.wlan_hosted_network_reason_miniport_stopped) {
								Logger.LogDebug("WlanManager", "OnNotification", "Wifi off while hosted network on");

								if(NetworkEvent != null)
									NetworkEvent(this, new EventArgsNetwork(NetworkEventType.WifiStopWhileNetworkOn));
							}
								//wyłączanie wifi, druga faza
							else if(nSC.OldState == WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_idle
								&& nSC.NewState == WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_unavailable
								&& nSC.Reason == WLAN_HOSTED_NETWORK_REASON.wlan_hosted_network_reason_miniport_stopped) {
								Logger.LogDebug("WlanManager", "OnNotification", "Wifi off");

								if(NetworkEvent != null)
									NetworkEvent(this, new EventArgsNetwork(NetworkEventType.WifiStopWhileNetworkOff));
							} else if(nSC.OldState == WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_idle
								  && nSC.NewState == WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_idle
								  && nSC.Reason == WLAN_HOSTED_NETWORK_REASON.wlan_hosted_network_reason_properties_change) {
								Logger.LogDebug("WlanManager", "OnNotification", "Network properties changed");
							} else {
								Logger.LogDebug("WlanManager", "OnNotification", "Unknown network change: " + nSC.OldState.ToString() + ", " + nSC.NewState.ToString() + ", " + nSC.Reason.ToString());
							}

							break;

						case (int)WLAN_HOSTED_NETWORK_NOTIFICATION_CODE.wlan_hosted_network_peer_state_change:

							WLAN_HOSTED_NETWORK_DATA_PEER_STATE_CHANGE pSC = (WLAN_HOSTED_NETWORK_DATA_PEER_STATE_CHANGE)Marshal.PtrToStructure(notificationData.dataPtr, typeof(WLAN_HOSTED_NETWORK_DATA_PEER_STATE_CHANGE));

							if(pSC.NewState.PeerAuthState == WLAN_HOSTED_NETWORK_PEER_AUTH_STATE.wlan_hosted_network_peer_state_authenticated) {
								Client client = new Client(pSC.NewState, this);
								Logger.LogDebug("WlanManager", "OnNotification", "Client " + client.MacAddress + " joined network");

								EventArgsClient eventArgsClient = new EventArgsClient(client, ClientEventType.Join);
								clientEventsQueue.AddItem(eventArgsClient);
							} else if(pSC.NewState.PeerAuthState == WLAN_HOSTED_NETWORK_PEER_AUTH_STATE.wlan_hosted_network_peer_state_invalid) {
								Logger.LogDebug("WlanManager", "OnNotification", "Client " + pSC.NewState.PeerMacAddress.ConvertToString() + " left network");

								EventArgsClient eventArgsClient = new EventArgsClient(pSC.NewState.PeerMacAddress, ClientEventType.Leave);
								clientEventsQueue.AddItem(eventArgsClient);
							} else {
								Logger.LogDebug("WlanManager", "OnNotification", "Client " + pSC.NewState.PeerMacAddress.ConvertToString() + " changed it's state");

								EventArgsClient eventArgsClient = new EventArgsClient(pSC.NewState.PeerMacAddress, ClientEventType.StateChange);
								clientEventsQueue.AddItem(eventArgsClient);
							}

							break;

						default:
							Logger.LogDebug("WlanManager", "OnNotification", "WLAN_NOTIFICATION_SOURCE_HNWK unknown notification code: " + notificationData.notificationCode.ToString());
							break;
					}

					break;

				case WlanNotificationSource.WLAN_NOTIFICATION_SOURCE_ONEX:
					Logger.LogDebug("WlanManager", "OnNotification", "WLAN_NOTIFICATION_SOURCE_ONEX: " + notificationData.notificationCode.ToString());
					break;

				default:
					Logger.LogDebug("WlanManager", "OnNotification", "Unknown: " + notificationData.notificationSource.ToString());
					break;
			}
		}

		#region "Properties"

		/// <summary>
		/// True if is started
		/// </summary>
		public bool IsStarted {
			get {
				try {
					return QueryStatus().HostedNetworkState == WLAN_HOSTED_NETWORK_STATE.wlan_hosted_network_active;
				} catch(Win32Exception e) {
					Logger.LogException("WlanManager", "IsStarted", "Error getting network status, returning false!", e);
					return false;
				}
			}
		}


		/// <summary>
		/// Not thread safe!!!
		/// </summary>
		public List<Client> Clients {
			get {
				return clients;
			}
		}

		#endregion

		public void ClientIdentified(EventArgsClient eventArgsClient) {
			if(ClientEvent != null)
				ClientEvent(this, eventArgsClient);
		}

		/// <summary>
		/// Method started as thread, consumes client events
		/// </summary>
		private void ClientEventsConsumer() {

			Logger.LogDebug("WlanManager", "ClientEventsConsumer", "Thread started");

			EventArgsClient eventArgsClient;

			while(true) {
				try {
					if(clientEventsQueue.RemoveItem(out eventArgsClient) == false)
						break;

					if(eventArgsClient.ClientEventType == ClientEventType.Join) {
						if(eventArgsClient.Client != null)
							clients.Add(eventArgsClient.Client);

						if(ClientEvent != null)
							ClientEvent(this, eventArgsClient);
					} else if(eventArgsClient.ClientEventType == ClientEventType.Leave) {
						Client leavingClient = null;

						try {
							leavingClient = clients.Find(i => i.MacAddress.ToUpperInvariant().Equals(eventArgsClient.Dot11MacAddress.ConvertToString().ToUpperInvariant()));
							eventArgsClient.Client = leavingClient;
						} catch(ArgumentNullException e) {
							Logger.LogException("WlanManager", "ClientEventsConsumer", "Error removing client", e);
						} catch(NullReferenceException e) {
							Logger.LogException("WlanManager", "ClientEventsConsumer", "Error removing client", e);
						}

						if(leavingClient != null)
							clients.Remove(leavingClient);

						if(ClientEvent != null)
							ClientEvent(this, eventArgsClient);

						leavingClient.Dispose();
					} else {
						Client changedClient = null;

						try {
							changedClient = clients.Find(i => i.MacAddress.ToUpperInvariant().Equals(eventArgsClient.Dot11MacAddress.ConvertToString().ToUpperInvariant()));
							eventArgsClient.Client = changedClient;
						} catch(ArgumentNullException e) {
							Logger.LogException("WlanManager", "ClientEventsConsumer", "Error updating client", e);
						} catch(NullReferenceException e) {
							Logger.LogException("WlanManager", "ClientEventsConsumer", "Error updating client", e);
						}

						if(ClientEvent != null)
							ClientEvent(this, eventArgsClient);
					}

					eventArgsClient = null;
				}
					//raised on Abort() method
				catch(ThreadAbortException) {
					break;
				}
			}
			Logger.LogDebug("WlanManager", "ClientEventsConsumer", "Thread stopped");
		}

		/// <summary>
		/// Performs cleanup
		/// </summary>
		public void Dispose() {
			Dispose(true);

			try {
				GC.SuppressFinalize(this);
			} catch(ArgumentNullException e) {
				Logger.LogException("WlanManager", "Dispose", "Error disposing", e);
			}
		}

		/// <summary>
		/// Performs clean-up
		/// </summary>
		private void Dispose(bool disposing) {
			if(!disposed) {
				if(disposing) {
					// Dispose managed resources.

					/*if(timerDnsUpdater != null)
					{
						timerDnsUpdater.Stop();
						timerDnsUpdater.Enabled = false;
						timerDnsUpdater.Elapsed -= timerDnsUpdater_Elapsed;
						timerDnsUpdater.Dispose();
					}*/

					//terminating client events consumer thread
					clientEventsQueue.EmptyQueue();

					if(IsStarted == true)
						//StopUsing();
						ForceStop();

					foreach(Client client in clients) {
						if(client != null) {
							DnsInterface.ClearArpCache(client.IpAddress);
							client.Dispose();
						}
					}

					clients.Clear();

					if(clientEventsConsumer != null) {
						if(clientEventsConsumer.IsAlive)
							try {
								clientEventsConsumer.Abort();
							} catch(SecurityException e) {
								Logger.LogException("WlanManager", "Dispose", "Error aborting clientEventsConsumer thread", e);
							} catch(ThreadStateException e) {
								Logger.LogException("WlanManager", "Dispose", "Error aborting clientEventsConsumer thread", e);
							}
					}
				}

				if(wlanNotificationCallback != null)
					wlanNotificationCallback -= this.OnNotification;

				// Clean up unmanaged resources here.
				if(wlanHandle != IntPtr.Zero)
					Imports.WlanCloseHandle(this.wlanHandle, IntPtr.Zero);

				disposed = true;
			}
		}

		//Disposable types should declare finalizer
		~WlanManager() {
			Dispose(false);
		}
	}
}
