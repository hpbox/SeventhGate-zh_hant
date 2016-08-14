using winapi;
using System.Threading;
using System;

namespace wlan
{
	/// <summary>
	/// This is the notificationSource file that contains the Client class 
	/// used to provide access to information (MAC address and dataPtr, for example) on the 
	/// clients that are connected to the wireless Hosted Network.
	/// </summary>
	public class Client :IDisposable
	{
		private bool disposed;

		/// <summary>
		/// Stores state of client
		/// </summary>
		private WLAN_HOSTED_NETWORK_PEER_STATE wlanHostedNetworkPeerState;

		/// <summary>
		/// For calling method responsible for informing gui about client being identified
		/// </summary>
		private WlanManager wlanManager;

		/// <summary>
		/// Raising cyclic events for arp/dns identification
		/// </summary>
		private System.Timers.Timer timerDnsUpdater = new System.Timers.Timer();

		/// <summary>
		/// Gets or sets client ip address
		/// </summary>
		public string IpAddress;

		/// <summary>
		/// Gets or sets client device name
		/// </summary>
		public string ClientName;

		/// <summary>
		/// True if connected client is identified with arp
		/// </summary>
		public bool Identified = false;

		

		/// <summary>
		/// Gets mac address of connected station
		/// </summary>
		public string MacAddress
		{
			get
			{
				return wlanHostedNetworkPeerState.PeerMacAddress.ConvertToString();
			}
		}

		/// <summary>
		/// Value presented as a full identifier of client:
		/// hostname + ipaddress + MACaddress
		/// </summary>
		public string FullId
		{
			get
			{
				return ClientName + " (" + IpAddress + ", " + MacAddress + ")";
			}
		}

		/// <summary>
		/// Constructs new connected station
		/// </summary>
		/// <param name="wlanHostedNetworkPeerState">Set true if client joins network</param>
		/// <param name="retrieveDnsInfo"></param>
		public Client(WLAN_HOSTED_NETWORK_PEER_STATE wlanHostedNetworkPeerState, WlanManager wlanManager)
		{
			this.wlanHostedNetworkPeerState = wlanHostedNetworkPeerState;
			this.wlanManager = wlanManager;

			IpAddress = DnsInterface.stringIp;
			ClientName = DnsInterface.stringHost;

			timerDnsUpdater.Interval = 10000;
			timerDnsUpdater.Elapsed += new System.Timers.ElapsedEventHandler(timerDnsUpdater_Elapsed);
			timerDnsUpdater.Start();
		}

		/// <summary>
		/// Procedure responsible for finding this client's identification data
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerDnsUpdater_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			timerDnsUpdater.Stop();
			timerDnsUpdater.Interval = 5000;

			Logger.LogDebug("Client", "timerDnsUpdater_Elapsed", "Checking identification for client " + MacAddress + "...");

			if(DnsInterface.GetClientIpFromMac(MacAddress, ref IpAddress) == false)
			{
				if(timerDnsUpdater != null)
					timerDnsUpdater.Start();
				return;
			}

			if(DnsInterface.GetClientNameFromIp(IpAddress, ref ClientName) == false)
			{
				if(timerDnsUpdater != null)
					timerDnsUpdater.Start();
				return;
			}

			Identified = true;
			timerDnsUpdater.Dispose();

			Logger.LogDebug("Client", "timerDnsUpdater_Elapsed", "Client " + MacAddress + " identified");

			if(!disposed && wlanManager != null)
				wlanManager.ClientIdentified(new EventArgsClient(this, ClientEventType.Identified));
		}

		#region cleanup

		public void Dispose()
		{
			Dispose(true);

			try
			{
				GC.SuppressFinalize(this);
			}
			catch(ArgumentNullException e)
			{
				Logger.LogException("Client", "Dispose", "Error disposing", e);
			}

		}

		private void Dispose(bool disposing)
		{
			if(!disposed)
			{
				if(disposing)
				{
					// Dispose managed resources.
					if(timerDnsUpdater != null)
					{
						timerDnsUpdater.Stop();
						timerDnsUpdater.Dispose();
					}

					DnsInterface.ClearArpCache(IpAddress);
				}

				// Clean up unmanaged resources here.

				disposed = true;
			}
		}

		/// <summary>
		/// Finalizer
		/// </summary>
		~Client()
		{
			Dispose(false);
		}

		#endregion
	}
}
