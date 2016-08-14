using System.Runtime.InteropServices;
using System;

namespace winapi
{
	/// <summary>
	/// The DOT11_MAC_ADDRESS types are used to define an IEEE
	/// media access control (MAC) address.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	public struct DOT11_MAC_ADDRESS
	{
		/// <summary>
		/// A MAC address in unicast, multicast, or broadcast format. 
		/// </summary>
		public byte one;
		public byte two;
		public byte three;
		public byte four;
		public byte five;
		public byte six;
	}

	/// <summary>
	/// A DOT11_SSID structure contains the SSID of an interface.
	/// The SSID that is specified by the SSID member is not
	/// a null-terminated ASCII string. The length of the SSID
	/// is determined by the uSSIDLength member.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct DOT11_SSID
	{
		/// <summary>
		/// The length, in bytes, of the SSID array. 
		/// </summary>
		public uint uSSIDLength;
		/// <summary>
		/// The SSID. DOT11_SSID_MAX_LENGTH is set to 32.
		/// </summary>
		//[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		//public string SSID;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public byte[] SSID;
	}

	/*/// <summary>
	/// The header of an array of information about available networks.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	internal struct WlanAvailableNetworkListHeader
	{
		/// <summary>
		/// Contains the number of <see cref=""/> items following the header.
		/// </summary>
		public uint numberOfItems;
		/// <summary>
		/// The index of the current item. The index of the first item is 0.
		/// </summary>
		public uint index;
	}*/

	/// <summary>
	/// Contains information about an available wireless network.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct WLAN_AVAILABLE_NETWORK
	{
		/// <summary>
		/// Contains the profile name associated with the network.
		/// If the network doesn't have a profile, this member will be empty.
		/// If multiple profiles are associated with the network, there will be multiple entries with the same SSID in the visible network list. Profile names are case-sensitive.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string profileName;
		/// <summary>
		/// Contains the SSID of the visible wireless network.
		/// </summary>
		public DOT11_SSID dot11Ssid;
		/// <summary>
		/// Specifies whether the network is an infrastructure or an ad-hoc one.
		/// </summary>
		public DOT11_BSS_TYPE dot11BssType;
		/// <summary>
		/// Indicates the number of BSSIDs in the network.
		/// </summary>
		public uint numberOfBssids;
		/// <summary>
		/// Indicates whether the network is connectable.
		/// </summary>
		public bool networkConnectable;
		/// <summary>
		/// Indicates why a network cannot be connected to. This member is only valid when <see cref="networkConnectable"/> is <c>false</c>.
		/// </summary>
		public WLAN_REASON_CODE wlanNotConnectableReason;
		/// <summary>
		/// The number of PHY types supported on available networks.
		/// The maximum value of this field is 8. If more than 8 PHY types are supported, <see cref="morePhyTypes"/> must be set to <c>true</c>.
		/// </summary>
		private uint numberOfPhyTypes;
		/// <summary>
		/// Contains an array of <see cref="DOT11_PHY_TYPE"/> values that represent the PHY types supported by the available networks.
		/// When <see cref="numberOfPhyTypes"/> is greater than 8, this array contains only the first 8 PHY types.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		private DOT11_PHY_TYPE[] dot11PhyTypes;
		/// <summary>
		/// Gets the <see cref="DOT11_PHY_TYPE"/> values that represent the PHY types supported by the available networks.
		/// </summary>
		public DOT11_PHY_TYPE[] Dot11PhyTypes
		{
			get
			{
				DOT11_PHY_TYPE[] ret = new DOT11_PHY_TYPE[numberOfPhyTypes];
				Array.Copy(dot11PhyTypes, ret, numberOfPhyTypes);
				return ret;
			}
		}
		/// <summary>
		/// Specifies if there are more than 8 PHY types supported.
		/// When this member is set to <c>true</c>, an application must call <see cref="WlanClient.WlanInterface.GetNetworkBssList"/> to get the complete list of PHY types.
		/// <see cref="WLAN_BSS_ENTRY.phyId"/> contains the PHY type for an entry.
		/// </summary>
		public bool morePhyTypes;
		/// <summary>
		/// A percentage value that represents the signal quality of the network.
		/// This field contains a value between 0 and 100.
		/// A value of 0 implies an actual RSSI signal strength of -100 dbm.
		/// A value of 100 implies an actual RSSI signal strength of -50 dbm.
		/// You can calculate the RSSI signal strength value for values between 1 and 99 using linear interpolation.
		/// </summary>
		public uint wlanSignalQuality;
		/// <summary>
		/// Indicates whether security is enabled on the network.
		/// </summary>
		public bool securityEnabled;
		/// <summary>
		/// Indicates the default authentication algorithm used to join this network for the first time.
		/// </summary>
		public DOT11_AUTH_ALGORITHM dot11DefaultAuthAlgorithm;
		/// <summary>
		/// Indicates the default cipher algorithm to be used when joining this network.
		/// </summary>
		public DOT11_CIPHER_ALGORITHM dot11DefaultCipherAlgorithm;
		/// <summary>
		/// Contains various flags specifying characteristics of the available network.
		/// </summary>
		public WlanAvailableNetworkFlags flags;
		/// <summary>
		/// Reserved for future use. Must be set to NULL.
		/// </summary>
		uint reserved;
	}

	/// <summary>
	/// Contains information provided when registering for WLAN notifications.
	/// </summary>
	/// <remarks>
	/// Corresponds to the native <c>WLAN_NOTIFICATION_DATA</c> type.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct WLAN_NOTIFICATION_DATA
	{
		/// <summary>
		/// Specifies where the notification comes from.
		/// </summary>
		/// <remarks>
		/// On Windows XP SP2, this field must be set to <see cref="WlanNotificationSource.WLAN_NOTIFICATION_SOURCE_NONE"/>, <see cref="WlanNotificationSource.WLAN_NOTIFICATION_SOURCE_ALL"/> or <see cref="WlanNotificationSource.WLAN_NOTIFICATION_SOURCE_ACM"/>.
		/// </remarks>
		public WlanNotificationSource notificationSource;
		/// <summary>
		/// Indicates the type of notification. The value of this field indicates what type of associated data will be present in <see cref="dataPtr"/>.
		/// </summary>
		public int notificationCode;
		/// <summary>
		/// Indicates which interface the notification is for.
		/// </summary>
		public Guid interfaceGuid;
		/// <summary>
		/// Specifies the size of <see cref="dataPtr"/>, in bytes.
		/// </summary>
		public int dataSize;
		/// <summary>
		/// Pointer to additional data needed for the notification, as indicated by <see cref="notificationCode"/>.
		/// </summary>
		public IntPtr dataPtr;

		/*/// <summary>
		/// Gets the notification code (in the correct enumeration type) according to the notification source.
		/// </summary>
		public object NotificationCode
		{
			get
			{
				if(notificationSource == WlanNotificationSource.WLAN_NOTIFICATION_SOURCE_MSM)
					return (WLAN_NOTIFICATION_MSM)notificationCode;
				else if(notificationSource == WlanNotificationSource.WLAN_NOTIFICATION_SOURCE_ACM)
					return (WLAN_NOTIFICATION_ACM)notificationCode;
				else
					return notificationCode;
			}

		}*/
	}

	/// <summary>
	/// Specifies the parameters used when using the <see cref="WlanConnect"/> function.
	/// </summary>
	/// <remarks>
	/// Corresponds to the native <c>WLAN_CONNECTION_PARAMETERS</c> type.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct WLAN_CONNECTION_PARAMETERS
	{
		/// <summary>
		/// Specifies the mode of connection.
		/// </summary>
		public WLAN_CONNECTION_MODE wlanConnectionMode;
		/// <summary>
		/// Specifies the profile being used for the connection.
		/// The contents of the field depend on the <see cref="wlanConnectionMode"/>:
		/// <list type="table">
		/// <listheader>
		/// <term>Value of <see cref="wlanConnectionMode"/></term>
		/// <description>Contents of the profile string</description>
		/// </listheader>
		/// <item>
		/// <term><see cref="WLAN_CONNECTION_MODE.Profile"/></term>
		/// <description>The name of the profile used for the connection.</description>
		/// </item>
		/// <item>
		/// <term><see cref="WLAN_CONNECTION_MODE.TemporaryProfile"/></term>
		/// <description>The XML representation of the profile used for the connection.</description>
		/// </item>
		/// <item>
		/// <term><see cref="WLAN_CONNECTION_MODE.DiscoverySecure"/>, <see cref="WLAN_CONNECTION_MODE.DiscoveryUnsecure"/> or <see cref="WLAN_CONNECTION_MODE.Auto"/></term>
		/// <description><c>null</c></description>
		/// </item>
		/// </list>
		/// </summary>
		[MarshalAs(UnmanagedType.LPWStr)]
		public string profile;
		/// <summary>
		/// Pointer to a <see cref="DOT11_SSID"/> structure that specifies the SSID of the network to connect to.
		/// This field is optional. When set to <c>null</c>, all SSIDs in the profile will be tried.
		/// This field must not be <c>null</c> if <see cref="wlanConnectionMode"/> is set to <see cref="WLAN_CONNECTION_MODE.DiscoverySecure"/> or <see cref="WLAN_CONNECTION_MODE.DiscoveryUnsecure"/>.
		/// </summary>
		public IntPtr dot11SsidPtr;
		/// <summary>
		/// Pointer to a <see cref="Dot11BssidList"/> structure that contains the list of basic service set (BSS) identifiers desired for the connection.
		/// </summary>
		/// <remarks>
		/// On Windows XP SP2, must be set to <c>null</c>.
		/// </remarks>
		public IntPtr desiredBssidListPtr;
		/// <summary>
		/// A <see cref="DOT11_BSS_TYPE"/> value that indicates the BSS type of the network. If a profile is provided, this BSS type must be the same as the one in the profile.
		/// </summary>
		public DOT11_BSS_TYPE dot11BssType;
		/// <summary>
		/// Specifies ocnnection parameters.
		/// </summary>
		/// <remarks>
		/// On Windows XP SP2, must be set to 0.
		/// </remarks>
		public WlanConnectionParametersFlags flags;
	}

	/*[StructLayout(LayoutKind.Sequential)]
	internal struct WlanBssListHeader
	{
		internal uint totalSize;
		internal uint numberOfItems;
	}*/

	/// <summary>
	/// Contains information about a basic service set (BSS).
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct WLAN_BSS_ENTRY
	{
		/// <summary>
		/// Contains the SSID of the access point (AP) associated with the BSS.
		/// </summary>
		public DOT11_SSID dot11Ssid;
		/// <summary>
		/// The identifier of the PHY on which the AP is operating.
		/// </summary>
		public uint phyId;
		/// <summary>
		/// Contains the BSS identifier.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
		public byte[] dot11Bssid;
		/// <summary>
		/// Specifies whether the network is infrastructure or ad hoc.
		/// </summary>
		public DOT11_BSS_TYPE dot11BssType;
		public DOT11_PHY_TYPE dot11BssPhyType;
		/// <summary>
		/// The received signal strength in dBm.
		/// </summary>
		public int rssi;
		/// <summary>
		/// The link quality reported by the driver. Ranges from 0-100.
		/// </summary>
		public uint linkQuality;
		/// <summary>
		/// If 802.11d is not implemented, the network interface card (NIC) must set this field to TRUE. If 802.11d is implemented (but not necessarily enabled), the NIC must set this field to TRUE if the BSS operation complies with the configured regulatory domain.
		/// </summary>
		public bool inRegDomain;
		/// <summary>
		/// Contains the beacon interval value from the beacon packet or probe response.
		/// </summary>
		public ushort beaconPeriod;
		/// <summary>
		/// The timestamp from the beacon packet or probe response.
		/// </summary>
		public ulong timestamp;
		/// <summary>
		/// The host timestamp value when the beacon or probe response is received.
		/// </summary>
		public ulong hostTimestamp;
		/// <summary>
		/// The capability value from the beacon packet or probe response.
		/// </summary>
		public ushort capabilityInformation;
		/// <summary>
		/// The frequency of the center channel, in kHz.
		/// </summary>
		public uint chCenterFrequency;
		/// <summary>
		/// Contains the set of data transfer rates supported by the BSS.
		/// </summary>
		public WLAN_RATE_SET wlanRateSet;
		/// <summary>
		/// The offset of the information element (IE) data blob.
		/// </summary>
		public uint ieOffset;
		/// <summary>
		/// The size of the IE data blob, in bytes.
		/// </summary>
		public uint ieSize;
	}

	/// <summary>
	/// Contains the set of supported data rates.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct WLAN_RATE_SET
	{
		/// <summary>
		/// The length, in bytes, of <see cref="rateSet"/>.
		/// </summary>
		private uint rateSetLength;
		/// <summary>
		/// An array of supported data transfer rates.
		/// If the rate is a basic rate, the first bit of the rate value is set to 1.
		/// A basic rate is the data transfer rate that all stations in a basic service set (BSS) can use to receive frames from the wireless medium.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 126)]
		private ushort[] rateSet;

		public ushort[] Rates
		{
			get
			{
				ushort[] rates = new ushort[rateSetLength / sizeof(ushort)];
				Array.Copy(rateSet, rates, rates.Length);
				return rates;
			}
		}

		/// <summary>
		/// CalculateS the data transfer rate in Mbps for an arbitrary supported rate.
		/// </summary>
		/// <param name="rate"></param>
		/// <returns></returns>
		public double GetRateInMbps(int rate)
		{
			return (rateSet[rate] & 0x7FFF) * 0.5;
		}
	}

	/// <summary>
	/// Contains information about connection related notifications.
	/// </summary>
	/// <remarks>
	/// Corresponds to the native <c>WLAN_CONNECTION_NOTIFICATION_DATA</c> type.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct WLAN_CONNECTION_NOTIFICATION_DATA
	{
		/// <remarks>
		/// On Windows XP SP 2, only <see cref="WLAN_CONNECTION_MODE.Profile"/> is supported.
		/// </remarks>
		public WLAN_CONNECTION_MODE wlanConnectionMode;
		/// <summary>
		/// The name of the profile used for the connection. Profile names are case-sensitive.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string profileName;
		/// <summary>
		/// The SSID of the association.
		/// </summary>
		public DOT11_SSID dot11Ssid;
		/// <summary>
		/// The BSS network type.
		/// </summary>
		public DOT11_BSS_TYPE dot11BssType;
		/// <summary>
		/// Indicates whether security is enabled for this connection.
		/// </summary>
		public bool securityEnabled;
		/// <summary>
		/// Indicates the reason for an operation failure.
		/// This field has a value of <see cref="WLAN_REASON_CODE.WLAN_REASON_CODE_SUCCESS"/> for all connection-related notifications except <see cref="WLAN_NOTIFICATION_ACM.ConnectionComplete"/>.
		/// If the connection fails, this field indicates the reason for the failure.
		/// </summary>
		public WLAN_REASON_CODE wlanReasonCode;
		/// <summary>
		/// This field contains the XML presentation of the profile used for discovery, if the connection succeeds.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
		public string profileXml;
	}

	/// <summary>
	/// Contains association attributes for a connection
	/// </summary>
	/// <remarks>
	/// Corresponds to the native <c>WLAN_ASSOCIATION_ATTRIBUTES</c> type.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct WLAN_ASSOCIATION_ATTRIBUTES
	{
		/// <summary>
		/// The SSID of the association.
		/// </summary>
		public DOT11_SSID dot11Ssid;
		/// <summary>
		/// Specifies whether the network is infrastructure or ad hoc.
		/// </summary>
		public DOT11_BSS_TYPE dot11BssType;
		/// <summary>
		/// The BSSID of the association.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
		public byte[] dot11Bssid;
		/// <summary>
		/// The physical type of the association.
		/// </summary>
		public DOT11_PHY_TYPE dot11PhyType;
		/// <summary>
		/// The position of the <see cref="DOT11_PHY_TYPE"/> value in the structure containing the list of PHY types.
		/// </summary>
		public uint dot11PhyIndex;
		/// <summary>
		/// A percentage value that represents the signal quality of the network.
		/// This field contains a value between 0 and 100.
		/// A value of 0 implies an actual RSSI signal strength of -100 dbm.
		/// A value of 100 implies an actual RSSI signal strength of -50 dbm.
		/// You can calculate the RSSI signal strength value for values between 1 and 99 using linear interpolation.
		/// </summary>
		public uint wlanSignalQuality;
		/// <summary>
		/// The receiving rate of the association.
		/// </summary>
		public uint rxRate;
		/// <summary>
		/// The transmission rate of the association.
		/// </summary>
		public uint txRate;

		//TODO Define proper class
		/*/// <summary>
		/// Gets the BSSID of the associated access point.
		/// </summary>
		/// <value>The BSSID.</value>
		public PhysicalAddress Dot11Bssid
		{
			get
			{
				return new PhysicalAddress(dot11Bssid);
			}
		}*/
	}

	/// <summary>
	/// Defines the security attributes for a wireless connection.
	/// </summary>
	/// <remarks>
	/// Corresponds to the native <c>WLAN_ASSOCIATION_ATTRIBUTES</c> type.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct WLAN_SECURITY_ATTRIBUTES
	{
		/// <summary>
		/// Indicates whether security is enabled for this connection.
		/// </summary>
		[MarshalAs(UnmanagedType.Bool)]
		public bool securityEnabled;
		[MarshalAs(UnmanagedType.Bool)]
		public bool oneXEnabled;
		/// <summary>
		/// The authentication algorithm.
		/// </summary>
		public DOT11_AUTH_ALGORITHM dot11AuthAlgorithm;
		/// <summary>
		/// The cipher algorithm.
		/// </summary>
		public DOT11_CIPHER_ALGORITHM dot11CipherAlgorithm;
	}

	/// <summary>
	/// Defines the attributes of a wireless connection.
	/// </summary>
	/// <remarks>
	/// Corresponds to the native <c>WLAN_CONNECTION_ATTRIBUTES</c> type.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct WLAN_CONNECTION_ATTRIBUTES
	{
		/// <summary>
		/// The state of the interface.
		/// </summary>
		public WLAN_INTERFACE_STATE isState;
		/// <summary>
		/// The mode of the connection.
		/// </summary>
		public WLAN_CONNECTION_MODE wlanConnectionMode;
		/// <summary>
		/// The name of the profile used for the connection. Profile names are case-sensitive.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string profileName;
		/// <summary>
		/// The attributes of the association.
		/// </summary>
		public WLAN_ASSOCIATION_ATTRIBUTES wlanAssociationAttributes;
		/// <summary>
		/// The security attributes of the connection.
		/// </summary>
		public WLAN_SECURITY_ATTRIBUTES wlanSecurityAttributes;
	}

	/// <summary>
	/// Contains information about a LAN interface.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct WLAN_INTERFACE_INFO
	{
		/// <summary>
		/// The GUID of the interface.
		/// </summary>
		public Guid interfaceGuid;
		/// <summary>
		/// The description of the interface.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string interfaceDescription;
		/// <summary>
		/// The current state of the interface.
		/// </summary>
		public WLAN_INTERFACE_STATE isState;
	}

	/// <summary>
	/// The header of the list returned by <see cref="WlanEnumInterfaces"/>.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct WlanInterfaceInfoListHeader
	{
		public uint numberOfItems;
		public uint index;
	}

	/// <summary>
	/// The header of the list returned by <see cref="WlanGetProfileList"/>.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct WlanProfileInfoListHeader
	{
		public uint numberOfItems;
		public uint index;
	}

	/// <summary>
	/// Contains basic information about a profile.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct WLAN_PROFILE_INFO
	{
		/// <summary>
		/// The name of the profile. This value may be the name of a domain if the profile is for provisioning. Profile names are case-sensitive.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string profileName;
		/// <summary>
		/// Profile flags.
		/// </summary>
		public WlanGetProfileFlags profileFlags;
	}

	/// <summary>
	/// http://msdn.microsoft.com/en-us/library/dd439507%28VS.85%29.aspx
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct WLAN_HOSTED_NETWORK_SECURITY_SETTINGS
	{
		/// <summary>
		/// Authorization Algorithm
		/// </summary>
		public DOT11_AUTH_ALGORITHM dot11AuthAlgorithm;
		/// <summary>
		/// CipherAlgorithm
		/// </summary>
		public DOT11_CIPHER_ALGORITHM dot11DefaultCipherAlgorithm;
	}

	/// <summary>
	/// The WLAN_HOSTED_NETWORK_CONNECTION_SETTINGS structure contains
	/// information about the connection settings on the wireless Hosted Network.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct WLAN_HOSTED_NETWORK_CONNECTION_SETTINGS
	{
		/// <summary>
		/// The SSID associated with the wireless Hosted Network.
		/// </summary>
		public DOT11_SSID hostedNetworkSSID;
		/// <summary>
		/// The maximum number of concurrent peers allowed by the wireless Hosted Network.
		/// </summary>
		//public uint dwMaxNumberOfPeers;
		public UInt32 dwMaxNumberOfPeers;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct WlanHostedNetworkStatusHeader
	{
		public WLAN_HOSTED_NETWORK_STATE HostedNetworkState;
		public Guid IPDeviceID;
		public DOT11_MAC_ADDRESS wlanHostedNetworkBSSID;
		public DOT11_PHY_TYPE dot11PhyType;
		public uint ChannelFrequency;
		public uint NumberOfPeers;
		//public WLAN_HOSTED_NETWORK_PEER_STATE[] PeerList;
	}

	/// <summary>
	/// The WLAN_HOSTED_NETWORK_DATA_PEER_STATE_CHANGE structure contains
	/// information about a network state change for a data peer on the wireless Hosted Network.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct WLAN_HOSTED_NETWORK_DATA_PEER_STATE_CHANGE
	{
		/// <summary>
		/// The previous network state for a data peer on the wireless Hosted Network.
		/// </summary>
		public WLAN_HOSTED_NETWORK_PEER_STATE OldState;
		/// <summary>
		/// The current network state for a data peer on the wireless Hosted Network.
		/// </summary>
		public WLAN_HOSTED_NETWORK_PEER_STATE NewState;
		/// <summary>
		/// The reason for the network state change for the data peer.
		/// </summary>
		public WLAN_HOSTED_NETWORK_REASON Reason;
	}

	/// <summary>
	/// The WLAN_HOSTED_NETWORK_PEER_STATE structure contains
	/// information about the peer state for a peer on the wireless Hosted Network.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct WLAN_HOSTED_NETWORK_PEER_STATE
	{
		/// <summary>
		/// The MAC address of the peer being described.
		/// </summary>
		public DOT11_MAC_ADDRESS PeerMacAddress;
		/// <summary>
		/// The current authentication state of this peer.
		/// </summary>
		public WLAN_HOSTED_NETWORK_PEER_AUTH_STATE PeerAuthState;
	}

	/// <summary>
	/// The WLAN_HOSTED_NETWORK_RADIO_STATE structure contains
	/// information about the radio state on the wireless Hosted Network.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct WLAN_HOSTED_NETWORK_RADIO_STATE
	{
		/// <summary>
		/// The software radio state of the wireless Hosted Network.
		/// </summary>
		public DOT11_RADIO_STATE dot11SoftwareRadioState;
		/// <summary>
		/// The hardware radio state of the wireless Hosted Network.
		/// </summary>
		public DOT11_RADIO_STATE dot11HardwareRadioState;
	}

	/// <summary>
	/// The WLAN_HOSTED_NETWORK_STATE_CHANGE structure contains
	/// information about a network state change on the wireless Hosted Network.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct WLAN_HOSTED_NETWORK_STATE_CHANGE
	{
		/// <summary>
		/// The previous network state on the wireless Hosted Network.
		/// </summary>
		public WLAN_HOSTED_NETWORK_STATE OldState;
		/// <summary>
		/// The current network state on the wireless Hosted Network.
		/// </summary>
		public WLAN_HOSTED_NETWORK_STATE NewState;
		/// <summary>
		/// The reason for the network state change.
		/// </summary>
		public WLAN_HOSTED_NETWORK_REASON Reason;
	}
}
