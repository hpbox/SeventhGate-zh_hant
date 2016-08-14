using System;
using NETCONLib;
using winapi;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace ics {

	// TODO add port mapping options
	// TODO add firewall exception

	/// <summary>
	/// This is the source file that contains the IcsConnection class 
	/// used to enable and disable Internet Connection Sharing (ICS).
	/// Class represents physical network interface
	/// </summary>
	public class IcsConnection : IDisposable {

		private bool disposed;
		private INetSharingConfiguration Config;
		private INetConnectionProps Props;

		/// <summary>
		/// Constructs IcsConnection
		/// </summary>
		/// <param name="netSharingManager"></param>
		/// <param name="netConnection"></param>
		public IcsConnection(INetSharingManager netSharingManager, INetConnection netConnection) {
			try {
				Config = netSharingManager.get_INetSharingConfigurationForINetConnection(netConnection);
			} catch(Win32Exception e) {
				Logger.LogException("IcsMgr", "INetSharingConfiguration", "Win32Exception retrieving Config", e);
			} catch(COMException e) {
				Logger.LogException("IcsMgr", "INetSharingConfiguration", "COMException retrieving Config", e);
			} catch(Exception e) {
				Logger.LogException("IcsMgr", "INetSharingConfiguration", "Exception retrieving Config", e);
			}

			try {
				Props = netSharingManager.get_NetConnectionProps(netConnection);
			} catch(Win32Exception e) {
				Logger.LogException("IcsMgr", "INetConnectionProps", "Win32Exception retrieving Props", e);
			} catch(COMException e) {
				Logger.LogException("IcsMgr", "INetConnectionProps", "COMException retrieving Props", e);
			} catch(Exception e) {
				Logger.LogException("IcsMgr", "INetConnectionProps", "Exception retrieving Props", e);
			}
		}

		/// <summary>
		/// TRUE if sharing is currently enabled for this connection
		/// FALSE if not or could not access object
		/// </summary>
		public bool IsSharingEnabled {
			get {
				if(Config != null)
					return Config.SharingEnabled;
				else
					return false;
			}
		}

		/// <summary>
		/// TRUE if network interface is turned on
		/// FALSE if off or could not access object
		/// </summary>
		public bool IsOn {
			get {
				if(Props == null)
					return false;
				else
					return Props.Status != tagNETCON_STATUS.NCS_DISCONNECTED;
			}
		}



		/// <summary>
		/// TRUE if given connection supports ics
		/// FALSE if not or could not access object
		/// </summary>
		public bool IsSupported {
			get {
				if(Props == null)
					return false;
				else
					return ((
					   Props.MediaType == tagNETCON_MEDIATYPE.NCM_DIRECT
					   || Props.MediaType == tagNETCON_MEDIATYPE.NCM_ISDN
					   || Props.MediaType == tagNETCON_MEDIATYPE.NCM_LAN
					   || Props.MediaType == tagNETCON_MEDIATYPE.NCM_PHONE
					   || Props.MediaType == tagNETCON_MEDIATYPE.NCM_TUNNEL
					   || Props.MediaType == tagNETCON_MEDIATYPE.NCM_PPPOE
						) && (
					   Props.Status != tagNETCON_STATUS.NCS_DISCONNECTED
						));
			}
		}

		/// <summary>
		/// TRUE if currently connected
		/// FALSE if disconnected or could not access object
		/// </summary>
		public bool IsConnected {
			get {
				if(Props == null)
					return false;
				else
					return (Props.Status == tagNETCON_STATUS.NCS_CONNECTED);
			}
		}

		/// <summary>
		/// Network interface name for this connection
		/// NULL if no name or could not access object
		/// </summary>
		public string DeviceName {
			get {
				if(Props == null)
					return null;
				else
					return Props.DeviceName;
			}
		}

		/// <summary>
		/// Name of this connection
		/// NULL if no name or could not access object
		/// </summary>
		public string Name {
			get {
				if(Props == null)
					return null;
				else
					return Props.Name;
			}
		}

		/// <summary>
		/// GUID of this network interface
		/// Empty GUID if could not access object
		/// </summary>
		public Guid ConnectionGuid {
			get {

				if(Props == null)
					return Guid.Empty;

				Guid guid = Guid.Empty;
				try {
					guid = new Guid(Props.Guid);
				} catch(ArgumentNullException e) {
					Logger.LogException("IcsMgr", "ConnectionGuid", "ArgumentNullException creating GUID", e);
				} catch(FormatException e) {
					Logger.LogException("IcsMgr", "ConnectionGuid", "FormatException creating GUID", e);
				} catch(OverflowException e) {
					Logger.LogException("IcsMgr", "ConnectionGuid", "OverflowException creating GUID", e);
				}

				return guid;
			}
		}

		/// <summary>
		/// String representing media type of connection
		/// NULL if could not access object
		/// </summary>
		public string ConnectionMediaType {
			get {
				if(Props == null)
					return null;

				return Props.MediaType.ToString();
			}
		}

		/// <summary>
		/// String representing status of connection
		/// NULL if could not access object
		/// </summary>
		public string ConnectionStatus {
			get {
				if(Props == null)
					return null;

				return Props.Status.ToString();
			}
		}

		/// <summary>
		/// Used to enable sharing private of public sharing for given connection
		/// </summary>
		/// <param name="SharePublic">TRUE if public sharing, FALSE if private sharing</param>
		/// <returns>FALSE if could not get access to connection object, TRUE if enabling was attempted</returns>
		public bool EnableSharing(bool SharePublic) {
			DisableSharing();


			if(Config == null) {
				Logger.LogError("IcsConnection", "EnableSharing", "Could not access connection object");
				return false;
			}

			try {
				if(SharePublic)
					Config.EnableSharing(tagSHARINGCONNECTIONTYPE.ICSSHARINGTYPE_PUBLIC);
				else
					Config.EnableSharing(tagSHARINGCONNECTIONTYPE.ICSSHARINGTYPE_PRIVATE);
			} catch(Win32Exception e) {
				Logger.LogException("IcsMgr", "EnableSharing", "Win32Exception enabling sharing", e);
				return false;
			} catch(COMException e) {
				Logger.LogException("IcsMgr", "EnableSharing", "COMException enabling sharing", e);
				return false;
			} catch(Exception e) {
				Logger.LogException("IcsMgr", "EnableSharing", "Exception enabling sharing", e);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Used to disable connection sharing
		/// </summary>
		/// <returns>FALSE if could not get access to connection object
		/// TRUE if disabling was attempted!</returns>
		public bool DisableSharing() {
			if(Config == null) {
				Logger.LogError("IcsConnection", "DisableSharing", "Could not access connection object");
				return false;
			}

			try {
				//if(Config.SharingEnabled)
				Config.DisableSharing();
			} catch(Win32Exception e) {
				Logger.LogException("IcsMgr", "DisableSharing", "Win32Exception disabling sharing", e);
				return false;
			} catch(COMException e) {
				Logger.LogException("IcsMgr", "DisableSharing", "COMException disabling sharing", e);
				return false;
			} catch(Exception e) {
				Logger.LogException("IcsMgr", "DisableSharing", "Exception disabling sharing", e);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Enables internet firewall for connection
		/// </summary>
		/// <returns>
		/// TRUE if enabling was attempted
		/// FALSE if could not access object</returns>
		public bool EnableInternetFirewall() {

			if(Config == null) {
				Logger.LogError("IcsConnection", "EnableInternetFirewall", "Could not access connection object");
				return false;
			}

			try {
				Config.EnableInternetFirewall();
			} catch(Win32Exception e) {
				Logger.LogException("IcsMgr", "EnableInternetFirewall", "Win32Exception enabling firewall", e);
				return false;
			} catch(COMException e) {
				Logger.LogException("IcsMgr", "EnableInternetFirewall", "COMException enabling firewall", e);
				return false;
			} catch(Exception e) {
				Logger.LogException("IcsMgr", "EnableInternetFirewall", "Exception enabling firewall", e);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Disables internet firewall for connection
		/// </summary>
		/// <returns>TRUE if disabling was attempted
		/// FALSE if could not access object</returns>
		public bool DisableInternetFirewall() {

			if(Config == null) {
				Logger.LogError("IcsConnection", "DisableInternetFirewall", "Could not access connection object");
				return false;
			}

			try {
				//if(Config.InternetFirewallEnabled)
				Config.DisableInternetFirewall();
			} catch(Win32Exception e) {
				Logger.LogException("IcsMgr", "DisableInternetFirewall", "Win32Exception disabling firewall", e);
				return false;
			} catch(COMException e) {
				Logger.LogException("IcsMgr", "DisableInternetFirewall", "COMException disabling firewall", e);
				return false;
			} catch(Exception e) {
				Logger.LogException("IcsMgr", "DisableInternetFirewall", "Exception disabling firewall", e);
				return false;
			}

			return true;
		}

		#region cleanup

		public void Dispose() {
			Dispose(true);

			try {
				GC.SuppressFinalize(this);
			} catch(ArgumentNullException e) {
				Logger.LogException("IcsConnection", "Dispose", "Error disposing", e);
			}
		}

		private void Dispose(bool disposing) {
			if(!disposed) {
				if(disposing) {
					// Dispose managed resources here
				}

				// Clean up unmanaged resources here
				//NetSharingManager = null;
				//NetConnection = null;
				Config = null;
				Props = null;

				//Those turn off and remove network interfaces from the system
				//NetConnection.Disconnect();
				//NetConnection.Delete();

				disposed = true;
			}
		}

		/// <summary>
		/// Finalizer
		/// </summary>
		~IcsConnection() {
			Dispose(false);
		}

		#endregion
	}
}

