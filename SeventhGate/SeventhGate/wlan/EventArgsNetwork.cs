using System;

namespace wlan
{
	/// <summary>
	/// Class contains fields required for conveying information about hosted network change
	/// </summary>
	public class EventArgsNetwork :EventArgs
	{
		/// <summary>
		/// Determines type of network event argument
		/// </summary>
		public NetworkEventType NetworkEventType
		{
			get;
			private set;
		}

		/// <summary>
		/// Constructs network event argument
		/// </summary>
		/// <param name="networkEventType"></param>
		public EventArgsNetwork(NetworkEventType networkEventType)
		{
			this.NetworkEventType = networkEventType;
		}

		/*public EventArgsNetwork()
		{
		}*/
	}

	/// <summary>
	/// Enumerates event types for hosted network
	/// </summary>
	public enum NetworkEventType
	{
		/// <summary>
		/// When network is started
		/// </summary>
		Start,
		/// <summary>
		/// When network is stopped
		/// </summary>
		Stop,
		/// <summary>
		/// When network fails to start
		/// </summary>
		StartFail,
		/// <summary>
		/// When wifi starts
		/// </summary>
		WifiStart,
		/// <summary>
		/// When wifi is stopped wile network operating
		/// </summary>
		WifiStopWhileNetworkOn,
		/// <summary>
		/// When wifi is stopped while network not operating
		/// </summary>
		WifiStopWhileNetworkOff
	}
}
