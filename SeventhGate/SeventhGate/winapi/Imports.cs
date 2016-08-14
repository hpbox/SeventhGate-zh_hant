using System.Runtime.InteropServices;
using System;
using System.Text;

namespace winapi {

	/// <summary>
	/// Class exposing entrypoints to library Wlanapi.dll
	/// </summary>
	/// <see cref="http://msdn.microsoft.com/en-us/library/ms706274(v=VS.85).aspx"/>
	public static class Imports {

		/// <summary>
		/// Defines Windows version as XP for native methods
		/// </summary>
		public const uint WLAN_CLIENT_VERSION_XP_SP2 = 1;

		/// <summary>
		/// Defines Windows version as Vista/7 for native methods
		/// </summary>
		public const uint WLAN_CLIENT_VERSION_LONGHORN = 2;

		/// <summary>
		/// Defines the callback function which accepts WLAN notifications.
		/// </summary>
		public delegate void WlanNotificationCallback(ref WLAN_NOTIFICATION_DATA notificationData, IntPtr context);

		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanOpenHandle(
			[In] UInt32 clientVersion,
			[In, Out] IntPtr pReserved,
			[Out] out UInt32 negotiatedVersion,
			[Out] out IntPtr clientHandle);

		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanCloseHandle(
			[In] IntPtr clientHandle,
			[In, Out] IntPtr pReserved);

		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanEnumInterfaces(
			[In] IntPtr clientHandle,
			[In, Out] IntPtr pReserved,
			[Out] out IntPtr ppInterfaceList);

		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanQueryInterface(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In] WLAN_INTF_OPCODE opCode,
			[In, Out] IntPtr pReserved,
			[Out] out int dataSize,
			[Out] out IntPtr ppData,
			[Out] out WLAN_OPCODE_VALUE_TYPE wlanOpcodeValueType);

		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanSetInterface(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In] WLAN_INTF_OPCODE opCode,
			[In] uint dataSize,
			[In] IntPtr pData,
			[In, Out] IntPtr pReserved);

		/// <param name="pDot11Ssid">Not supported on Windows XP SP2: must be a <c>null</c> reference.</param>
		/// <param name="pIeData">Not supported on Windows XP SP2: must be a <c>null</c> reference.</param>
		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanScan(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In] IntPtr pDot11Ssid,
			[In] IntPtr pIeData,
			[In, Out] IntPtr pReserved);

		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanGetAvailableNetworkList(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In] WlanGetAvailableNetworkListFlags flags,
			[In, Out] IntPtr reservedPtr,
			[Out] out IntPtr availableNetworkListPtr);

		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanSetProfile(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In] WlanGetProfileFlags flags,
			[In, MarshalAs(UnmanagedType.LPWStr)] string profileXml,
			[In, Optional, MarshalAs(UnmanagedType.LPWStr)] string allUserProfileSecurity,
			[In] bool overwrite,
			[In] IntPtr pReserved,
			[Out] out WLAN_REASON_CODE reasonCode);

		/// <param name="flags">Not supported on Windows XP SP2: must be a <c>null</c> reference.</param>
		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanGetProfile(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In, MarshalAs(UnmanagedType.LPWStr)] string profileName,
			[In] IntPtr pReserved,
			[Out] out IntPtr profileXml,
			ref WlanGetProfileFlags flags,
			[Out, Optional] out WlanGetProfileAccessFlags grantedAccess);

		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanGetProfileList(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In] IntPtr pReserved,
			[Out] out IntPtr profileList);

		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern void WlanFreeMemory(IntPtr pMemory);

		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanReasonCodeToString(
			[In] WLAN_REASON_CODE reasonCode,
			[In] int bufferSize,
			[In, Out] StringBuilder stringBuffer,
			IntPtr pReserved);

		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanRegisterNotification(
			[In] IntPtr clientHandle,
			[In] WlanNotificationSource notifSource,
			[In] bool ignoreDuplicate,
			[In] WlanNotificationCallback funcCallback,
			[In] IntPtr callbackContext,
			[In] IntPtr reserved,
			[Out] out WlanNotificationSource prevNotifSource);

		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanConnect(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In] ref WLAN_CONNECTION_PARAMETERS connectionParameters,
			IntPtr pReserved);

		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanDeleteProfile(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In, MarshalAs(UnmanagedType.LPWStr)] string profileName,
			IntPtr reservedPtr);

		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanDisconnect(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			IntPtr pReserved);

		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanGetNetworkBssList(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In] IntPtr dot11SsidInt,
			[In] DOT11_BSS_TYPE dot11BssType,
			[In] bool securityEnabled,
			IntPtr reservedPtr,
			[Out] out IntPtr wlanBssList);

		/// Return Type: DWORD->unsigned int
		///hClientHandle: HANDLE->void*
		///pFailReason: PWLAN_HOSTED_NETWORK_REASON->_WLAN_HOSTED_NETWORK_REASON*
		///pvReserved: PVOID->void*
		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanHostedNetworkForceStop(
			[In] IntPtr hClientHandle,
			out WLAN_HOSTED_NETWORK_REASON pFailReason,
			IntPtr pvReserved);

		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanHostedNetworkInitSettings(
			[In] IntPtr hClientHandle,
			out WLAN_HOSTED_NETWORK_REASON pFailReason,
			IntPtr pvReserved);

		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanHostedNetworkQueryStatus(
			[In] IntPtr hClientHandle,
			[Out] out IntPtr status,
			[In, Out] IntPtr pvReserved);

		// http://msdn.microsoft.com/en-us/library/dd439491%28VS.85%29.aspx
		// TODO Doesn't work
		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanHostedNetworkQueryProperty(
			[In] IntPtr hClientHandle,
			WLAN_HOSTED_NETWORK_OPCODE OpCode,
			out uint pdwDataSize,
			out IntPtr ppvData,
			out WLAN_OPCODE_VALUE_TYPE pWlanOpcodeValueType,
			IntPtr pvReserved);

		/// Return Type: DWORD->unsigned int
		///hClientHandle: HANDLE->void*
		///pdwKeyLength: DWORD->unsigned int
		///ppucKeyData: PUCHAR*
		///pbIsPassPhrase: PBOOL->BOOL*
		///pbPersistent: PBOOL->BOOL*
		///pFailReason: PWLAN_HOSTED_NETWORK_REASON->_WLAN_HOSTED_NETWORK_REASON*
		///pvReserved: PVOID->void*
		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanHostedNetworkQuerySecondaryKey(
			IntPtr hClientHandle,
			out uint pdwKeyLength,
			out String ppucKeyData,
			[Out] out bool pbIsPassPhrase,
			[Out] out bool pbPersistent,
			out WLAN_HOSTED_NETWORK_REASON pFailReason,
			IntPtr pvReserved);

		/// Return Type: DWORD->unsigned int
		///hClientHandle: HANDLE->void*
		///pFailReason: PWLAN_HOSTED_NETWORK_REASON->_WLAN_HOSTED_NETWORK_REASON*
		///pvReserved: PVOID->void*
		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanHostedNetworkRefreshSecuritySettings(
			[In] IntPtr hClientHandle,
			out WLAN_HOSTED_NETWORK_REASON pFailReason,
			IntPtr pvReserved);

		/// <see cref="http://msdn.microsoft.com/en-us/library/dd439495%28VS.85%29.aspx"/>
		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanHostedNetworkSetProperty(
			[In] IntPtr hClientHandle,
			WLAN_HOSTED_NETWORK_OPCODE OpCode,
			uint dwDataSize,
			[In] IntPtr pvData,
			[Out] out WLAN_HOSTED_NETWORK_REASON pFailReason,
			IntPtr pvReserved);

		/// Return Type: DWORD->unsigned int
		///hClientHandle: HANDLE->void*
		///pFailReason: PWLAN_HOSTED_NETWORK_REASON->_WLAN_HOSTED_NETWORK_REASON*
		///pvReserved: PVOID->void*
		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanHostedNetworkForceStart(
			[In] IntPtr hClientHandle,
			out WLAN_HOSTED_NETWORK_REASON pFailReason,
			IntPtr pvReserved);

		/// Return Type: DWORD->unsigned int
		///hClientHandle: HANDLE->void*
		///dwKeyLength: DWORD->unsigned int
		///pucKeyData: PUCHAR->UCHAR*
		///bIsPassPhrase: BOOL->int
		///bPersistent: BOOL->int
		///pFailReason: PWLAN_HOSTED_NETWORK_REASON->_WLAN_HOSTED_NETWORK_REASON*
		///pvReserved: PVOID->void*
		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanHostedNetworkSetSecondaryKey(
			[In] IntPtr hClientHandle,
			uint dwKeyLength,
			[In] [MarshalAsAttribute(UnmanagedType.LPStr)] string pucKeyData,
			[MarshalAsAttribute(UnmanagedType.Bool)] bool bIsPassPhrase,
			[MarshalAsAttribute(UnmanagedType.Bool)] bool bPersistent,
			[Out] out WLAN_HOSTED_NETWORK_REASON pFailReason,
			IntPtr pvReserved);

		/// Return Type: DWORD->unsigned int
		///hClientHandle: HANDLE->void*
		///pFailReason: PWLAN_HOSTED_NETWORK_REASON->_WLAN_HOSTED_NETWORK_REASON*
		///pvReserved: PVOID->void*
		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanHostedNetworkStartUsing(
			[In] IntPtr hClientHandle,
			[Out] out WLAN_HOSTED_NETWORK_REASON pFailReason,
			IntPtr pvReserved);

		/// Return Type: DWORD->unsigned int
		///hClientHandle: HANDLE->void*
		///pFailReason: PWLAN_HOSTED_NETWORK_REASON->_WLAN_HOSTED_NETWORK_REASON*
		///pvReserved: PVOID->void*
		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanHostedNetworkStopUsing(
			[In] IntPtr hClientHandle,
			out WLAN_HOSTED_NETWORK_REASON pFailReason,
			IntPtr pvReserved);

		/// <summary>
		/// http://msdn.microsoft.com/en-us/library/dd815253%28VS.85%29.aspx
		/// </summary>
		/// <param name="hClientHandle"></param>
		/// <param name="bRegister"></param>
		/// <param name="pvReserved"></param>
		/// <returns></returns>
		[DllImport("wlanapi.dll", SetLastError = true)]
		public static extern int WlanRegisterVirtualStationNotification(
			[In] IntPtr hClientHandle,
			bool bRegister,
			IntPtr pvReserved);
	}
}
