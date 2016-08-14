using System;
using winapi;

namespace wlan
{
	/// <summary>
	/// Class contains fields required for conveying information about client change
	/// </summary>
	public class EventArgsClient :EventArgs
	{
		/// <summary>
		/// Keeps reference to given client object
		/// </summary>
		public Client Client
		{
			get;
			set;
		}

		/// <summary>
		/// Conveys type of client event
		/// </summary>
		public ClientEventType ClientEventType
		{
			get;
			private set;
		}

		/// <summary>
		/// Used when client is leaving or changing it's state
		/// </summary>
		public DOT11_MAC_ADDRESS Dot11MacAddress
		{
			get;
			set;
		}

		/// <summary>
		/// Constructs event argument
		/// </summary>
		/// <param name="client"></param>
		/// <param name="clientEventType"></param>
		public EventArgsClient(Client client, ClientEventType clientEventType)
		{
			this.Client = client;
			this.ClientEventType = clientEventType;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dot11MacAddress"></param>
		/// <param name="clientEventType"></param>
		public EventArgsClient(DOT11_MAC_ADDRESS dot11MacAddress, ClientEventType clientEventType)
		{
			this.Dot11MacAddress = dot11MacAddress;
			this.ClientEventType = clientEventType;
		}

		/*/// <summary>
		/// Creates an event meaning it is last event and consumer thread should terminate
		/// </summary>
		public EventArgsClient() { this.ClientEventType = ClientEventType.ProgramTermination; }*/
	}

	/// <summary>
	/// Enumerates event types for clients
	/// </summary>
	public enum ClientEventType
	{
		/// <summary>
		/// On client join
		/// </summary>
		Join,
		/// <summary>
		/// On client leave
		/// </summary>
		Leave,
		/// <summary>
		/// On client state change
		/// </summary>
		StateChange,
        /// <summary>
        /// Sent after connected client gets identified from arp and dns
        /// </summary>
        Identified
	}
}
