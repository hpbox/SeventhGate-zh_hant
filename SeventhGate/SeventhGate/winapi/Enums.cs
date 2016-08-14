using System;
using System.Runtime.InteropServices;

namespace winapi {
	/// <summary>
	/// The WLAN_INTF_OPCODE enumerated type defines various
	/// opcodes used to set and query parameters on a wireless interface.
	/// </summary>
	/// <see cref="http://msdn.microsoft.com/en-us/library/ms706886(VS.85).aspx"/>
	/// <remarks>
	/// Corresponds to the native <c>WLAN_INTF_OPCODE</c> type.
	/// </remarks>
	public enum WLAN_INTF_OPCODE {
		wlan_intf_opcode_autoconf_start = 0x000000000,
		/// <summary>
		/// Opcode used to set or query whether auto config is enabled.
		/// </summary>
		wlan_intf_opcode_autoconf_enabled,
		/// <summary>
		/// Opcode used to set or query whether background scan is enabled.
		/// </summary>
		wlan_intf_opcode_background_scan_enabled,
		/// <summary>
		/// Opcode used to set or query the media streaming mode of the driver.
		/// </summary>
		wlan_intf_opcode_media_streaming_mode,
		/// <summary>
		/// Opcode used to set or query the radio state.
		/// </summary>
		wlan_intf_opcode_radio_state,
		/// <summary>
		/// Opcode used to set or query the BSS type of the interface.
		/// </summary>
		wlan_intf_opcode_bss_type,
		/// <summary>
		/// Opcode used to query the state of the interface.
		/// </summary>
		wlan_intf_opcode_interface_state,
		/// <summary>
		/// Opcode used to query information about the current connection of the interface.
		/// </summary>
		wlan_intf_opcode_current_connection,
		/// <summary>
		/// Opcose used to query the current channel on which the wireless interface is operating.
		/// </summary>
		wlan_intf_opcode_channel_number,
		/// <summary>
		/// Opcode used to query the supported auth/cipher pairs for infrastructure mode.
		/// </summary>
		wlan_intf_opcode_supported_infrastructure_auth_cipher_pairs,
		/// <summary>
		/// Opcode used to query the supported auth/cipher pairs for ad hoc mode.
		/// </summary>
		wlan_intf_opcode_supported_adhoc_auth_cipher_pairs,
		/// <summary>
		/// Opcode used to query the list of supported country or region strings.
		/// </summary>
		wlan_intf_opcode_supported_country_or_region_string_list,
		/// <summary>
		/// Opcode used to set or query the current operation mode of the wireless interface.
		/// </summary>
		wlan_intf_opcode_current_operation_mode,
		wlan_intf_opcode_supported_safe_mode,
		wlan_intf_opcode_certified_safe_mode,
		wlan_intf_opcode_hosted_network_capable,
		wlan_intf_opcode_autoconf_end = 0x0fffffff,
		wlan_intf_opcode_msm_start = 0x10000100,
		/// <summary>
		/// Opcode used to query driver statistics.
		/// </summary>
		wlan_intf_opcode_statistics,
		/// <summary>
		/// Opcode used to query the received signal strength.
		/// </summary>
		wlan_intf_opcode_rssi,
		wlan_intf_opcode_msm_end = 0x1fffffff,
		wlan_intf_opcode_security_start = 0x20010000,
		wlan_intf_opcode_security_end = 0x2fffffff,
		wlan_intf_opcode_ihv_start = 0x30000000,
		wlan_intf_opcode_ihv_end = 0x3fffffff
	}

	/// <summary>
	/// Specifies the origin of automatic configuration (auto config) settings.
	/// </summary>
	/// <see cref="http://msdn.microsoft.com/en-us/library/ms706910(VS.85).aspx"/>
	/// <remarks>
	/// Corresponds to the native <c>WLAN_OPCODE_VALUE_TYPE</c> type.
	/// </remarks>
	public enum WLAN_OPCODE_VALUE_TYPE {
		/// <summary>
		/// The auto config settings were queried, but the origin of the settings was not determined.
		/// </summary>
		wlan_opcode_value_type_query_only = 0,
		/// <summary>
		/// The auto config settings were set by group policy.
		/// </summary>
		wlan_opcode_value_type_set_by_group_policy,
		/// <summary>
		/// The auto config settings were set by the user.
		/// </summary>
		wlan_opcode_value_type_set_by_user,
		/// <summary>
		/// The auto config settings are invalid.
		/// </summary>
		wlan_opcode_value_type_invalid
	}

	/// <summary>
	/// Defines a wireless LAN authentication algorithm.
	/// </summary>
	/// <see cref="http://msdn.microsoft.com/en-us/library/ms705989(VS.85).aspx"/>
	/// <remarks>
	/// Corresponds to the native <c>DOT11_AUTH_ALGORITHM</c> type.
	/// </remarks>
	public enum DOT11_AUTH_ALGORITHM : uint {
		/// <summary>
		/// Specifies an IEEE 802.11 Open System authentication algorithm.
		/// </summary>
		DOT11_AUTH_ALGO_80211_OPEN = 1,
		/// <summary>
		/// Specifies an 802.11 Shared Key authentication algorithm that requires the use of a pre-shared Wired Equivalent Privacy (WEP) key for the 802.11 authentication.
		/// </summary>
		DOT11_AUTH_ALGO_80211_SHARED_KEY = 2,
		/// <summary>
		/// Specifies a Wi-Fi Protected Access (WPA) algorithm. IEEE 802.1X port authentication is performed by the supplicant, authenticator, and authentication server. Cipher keys are dynamically derived through the authentication process.
		/// <para>This algorithm is valid only for BSS types of <see cref="DOT11_BSS_TYPE.Infrastructure"/>.</para>
		/// <para>When the WPA algorithm is enabled, the 802.11 station will associate only with an access point whose beacon or probe responses contain the authentication suite of type 1 (802.1X) within the WPA information element (IE).</para>
		/// </summary>
		DOT11_AUTH_ALGO_WPA = 3,
		/// <summary>
		/// Specifies a WPA algorithm that uses preshared keys (PSK). IEEE 802.1X port authentication is performed by the supplicant and authenticator. Cipher keys are dynamically derived through a preshared key that is used on both the supplicant and authenticator.
		/// <para>This algorithm is valid only for BSS types of <see cref="DOT11_BSS_TYPE.Infrastructure"/>.</para>
		/// <para>When the WPA PSK algorithm is enabled, the 802.11 station will associate only with an access point whose beacon or probe responses contain the authentication suite of type 2 (preshared key) within the WPA IE.</para>
		/// </summary>
		DOT11_AUTH_ALGO_WPA_PSK = 4,
		/// <summary>
		/// This value is not supported.
		/// </summary>
		DOT11_AUTH_ALGO_WPA_NONE = 5,
		/// <summary>
		/// Specifies an 802.11i Robust WLAN_NOTIFICATION_SOURCE_SECURITY Network Association (RSNA) algorithm. WPA2 is one such algorithm. IEEE 802.1X port authentication is performed by the supplicant, authenticator, and authentication server. Cipher keys are dynamically derived through the authentication process.
		/// <para>This algorithm is valid only for BSS types of <see cref="DOT11_BSS_TYPE.Infrastructure"/>.</para>
		/// <para>When the RSNA algorithm is enabled, the 802.11 station will associate only with an access point whose beacon or probe responses contain the authentication suite of type 1 (802.1X) within the RSN IE.</para>
		/// </summary>
		DOT11_AUTH_ALGO_RSNA = 6,
		/// <summary>
		/// Specifies an 802.11i RSNA algorithm that uses PSK. IEEE 802.1X port authentication is performed by the supplicant and authenticator. Cipher keys are dynamically derived through a preshared key that is used on both the supplicant and authenticator.
		/// <para>This algorithm is valid only for BSS types of <see cref="DOT11_BSS_TYPE.Infrastructure"/>.</para>
		/// <para>When the RSNA PSK algorithm is enabled, the 802.11 station will associate only with an access point whose beacon or probe responses contain the authentication suite of type 2(preshared key) within the RSN IE.</para>
		/// </summary>
		DOT11_AUTH_ALGO_RSNA_PSK = 7,
		/// <summary>
		/// Indicates the start of the range that specifies proprietary authentication algorithms that are developed by an WLAN_NOTIFICATION_SOURCE_IHV.
		/// </summary>
		/// <remarks>
		/// This enumerator is valid only when the miniport driver is operating in Extensible Station (ExtSTA) mode.
		/// </remarks>
		DOT11_AUTH_ALGO_IHV_START = 0x80000000,
		/// <summary>
		/// Indicates the end of the range that specifies proprietary authentication algorithms that are developed by an WLAN_NOTIFICATION_SOURCE_IHV.
		/// </summary>
		/// <remarks>
		/// This enumerator is valid only when the miniport driver is operating in Extensible Station (ExtSTA) mode.
		/// </remarks>
		DOT11_AUTH_ALGO_IHV_END = 0xffffffff
	}

	/// <summary>
	/// Defines a cipher algorithm for data encryption and decryption.
	/// </summary>
	/// <remarks>
	/// Corresponds to the native <c>DOT11_CIPHER_ALGORITHM</c> type.
	/// </remarks>
	/// <see cref="http://msdn.microsoft.com/en-us/library/ms706003(VS.85).aspx"/>
	public enum DOT11_CIPHER_ALGORITHM : uint {
		/// <summary>
		/// Specifies that no cipher algorithm is enabled or supported.
		/// </summary>
		DOT11_CIPHER_ALGO_NONE = 0x00,
		/// <summary>
		/// Specifies a Wired Equivalent Privacy (WEP) algorithm, which is the RC4-based algorithm that is specified in the 802.11-1999 standard. This enumerator specifies the WEP cipher algorithm with a 40-bit cipher key.
		/// </summary>
		DOT11_CIPHER_ALGO_WEP40 = 0x01,
		/// <summary>
		/// Specifies a Temporal Key Integrity Protocol (TKIP) algorithm, which is the RC4-based cipher suite that is based on the algorithms that are defined in the WPA specification and IEEE 802.11i-2004 standard. This cipher also uses the Michael Message Integrity Code (MIC) algorithm for forgery protection.
		/// </summary>
		DOT11_CIPHER_ALGO_TKIP = 0x02,
		/// <summary>
		/// Specifies an AES-CCMP algorithm, as specified in the IEEE 802.11i-2004 standard and RFC 3610. Advanced Encryption Standard (AES) is the encryption algorithm defined in FIPS PUB 197.
		/// </summary>
		DOT11_CIPHER_ALGO_CCMP = 0x04,
		/// <summary>
		/// Specifies a WEP cipher algorithm with a 104-bit cipher key.
		/// </summary>
		DOT11_CIPHER_ALGO_WEP104 = 0x05,
		/// <summary>
		/// Specifies a Robust WLAN_NOTIFICATION_SOURCE_SECURITY Network (RSN) Use Group Key cipher suite. For more information about the Use Group Key cipher suite, refer to Clause 7.3.2.9.1 of the IEEE 802.11i-2004 standard.
		/// </summary>
		DOT11_CIPHER_ALGO_WPA_USE_GROUP = 0x100,
		/// <summary>
		/// Specifies a Wifi Protected Access (WPA) Use Group Key cipher suite. For more information about the Use Group Key cipher suite, refer to Clause 7.3.2.9.1 of the IEEE 802.11i-2004 standard.
		/// </summary>
		DOT11_CIPHER_ALGO_RSN_USE_GROUP = 0x100,
		/// <summary>
		/// Specifies a WEP cipher algorithm with a cipher key of any length.
		/// </summary>
		DOT11_CIPHER_ALGO_WEP = 0x101,
		/// <summary>
		/// Specifies the start of the range that is used to define proprietary cipher algorithms that are developed by an independent hardware vendor (WLAN_NOTIFICATION_SOURCE_IHV).
		/// </summary>
		DOT11_CIPHER_ALGO_IHV_START = 0x80000000,
		/// <summary>
		/// Specifies the end of the range that is used to define proprietary cipher algorithms that are developed by an WLAN_NOTIFICATION_SOURCE_IHV.
		/// </summary>
		DOT11_CIPHER_ALGO_IHV_END = 0xffffffff
	}

	/// <summary>
	/// Defines a basic service set (BSS) network type.
	/// </summary>
	/// <remarks>
	/// Corresponds to the native <c>DOT11_BSS_TYPE</c> type.
	/// </remarks>
	/// <see cref="http://msdn.microsoft.com/en-us/library/ms706001(VS.85).aspx"/>
	public enum DOT11_BSS_TYPE {
		/// <summary>
		/// Specifies an infrastructure BSS network.
		/// </summary>
		dot11_BSS_type_infrastructure = 1,
		/// <summary>
		/// Specifies an independent BSS (IBSS) network.
		/// </summary>
		dot11_BSS_type_independent = 2,
		/// <summary>
		/// Specifies either infrastructure or IBSS network.
		/// </summary>
		dot11_BSS_type_any = 3
	}

	/// <summary>
	/// Defines an 802.11 PHY and media type.
	/// </summary>
	/// <remarks>
	/// Corresponds to the native <c>DOT11_PHY_TYPE</c> type.
	/// </remarks>
	public enum DOT11_PHY_TYPE : uint {
		/// <summary>
		/// Specifies an unknown or uninitialized PHY type.
		/// </summary>
		dot11_phy_type_unknown = 0,
		/// <summary>
		/// Specifies any PHY type.
		/// </summary>
		dot11_phy_type_any = dot11_phy_type_unknown,
		/// <summary>
		/// Specifies a frequency-hopping spread-spectrum (FHSS) PHY. Bluetooth devices can use FHSS or an adaptation of FHSS.
		/// </summary>
		dot11_phy_type_fhss = 1,
		/// <summary>
		/// Specifies a direct sequence spread spectrum (DSSS) PHY.
		/// </summary>
		dot11_phy_type_dsss = 2,
		/// <summary>
		/// Specifies an infrared (IR) baseband PHY.
		/// </summary>
		dot11_phy_type_irbaseband = 3,
		/// <summary>
		/// Specifies an orthogonal frequency division multiplexing (OFDM) PHY. 802.11a devices can use OFDM.
		/// </summary>
		dot11_phy_type_ofdm = 4,
		/// <summary>
		/// Specifies a high-rate DSSS (HRDSSS) PHY.
		/// </summary>
		dot11_phy_type_hrdsss = 5,
		/// <summary>
		/// Specifies an extended rate PHY (ERP). 802.11g devices can use ERP.
		/// </summary>
		dot11_phy_type_erp = 6,
		dot11_phy_type_ht = 7,
		/// <summary>
		/// Specifies the start of the range that is used to define PHY types that are developed by an independent hardware vendor (WLAN_NOTIFICATION_SOURCE_IHV).
		/// </summary>
		dot11_phy_type_IHV_start = 0x80000000,
		/// <summary>
		/// Specifies the end of the range that is used to define PHY types that are developed by an independent hardware vendor (WLAN_NOTIFICATION_SOURCE_IHV).
		/// </summary>
		dot11_phy_type_IHV_end = 0xffffffff
	}

	/// <summary>
	/// Defines the types of WLAN_NOTIFICATION_SOURCE_ACM (<see cref="WlanNotificationSource.WLAN_NOTIFICATION_SOURCE_ACM"/>) notifications.
	/// </summary>
	/// <remarks>
	/// The enumeration identifiers correspond to the native <c>wlan_notification_acm_</c> identifiers.
	/// On Windows XP SP2, only the <c>ConnectionComplete</c> and <c>wlan_interface_state_disconnected</c> notifications are available.
	/// </remarks>
	public enum WLAN_NOTIFICATION_ACM {
		wlan_notification_acm_start = 0x00000000,
		wlan_notification_acm_autoconf_enabled,
		wlan_notification_acm_autoconf_disabled,
		wlan_notification_acm_background_scan_enabled,
		wlan_notification_acm_background_scan_disabled,
		wlan_notification_acm_bss_type_change,
		wlan_notification_acm_power_setting_change,
		wlan_notification_acm_scan_complete,
		wlan_notification_acm_scan_fail,
		wlan_notification_acm_connection_start,
		wlan_notification_acm_connection_complete,
		wlan_notification_acm_connection_attempt_fail,
		wlan_notification_acm_filter_list_change,
		wlan_notification_acm_interface_arrival,
		wlan_notification_acm_interface_removal,
		wlan_notification_acm_profile_change,
		wlan_notification_acm_profile_name_change,
		wlan_notification_acm_profiles_exhausted,
		wlan_notification_acm_network_not_available,
		wlan_notification_acm_network_available,
		wlan_notification_acm_disconnecting,
		wlan_notification_acm_disconnected,
		wlan_notification_acm_adhoc_network_state_change,
		wlan_notification_acm_end
	}

	/// <summary>
	/// Defines the types of an WLAN_NOTIFICATION_SOURCE_MSM (<see cref="WlanNotificationSource.WLAN_NOTIFICATION_SOURCE_MSM"/>) notifications.
	/// </summary>
	/// <remarks>
	/// The enumeration identifiers correspond to the native <c>wlan_notification_msm_</c> identifiers.
	/// </remarks>
	public enum WLAN_NOTIFICATION_MSM {
		wlan_notification_msm_start = 0X00000000,
		wlan_notification_msm_associating,
		wlan_notification_msm_associated,
		wlan_notification_msm_authenticating,
		wlan_notification_msm_connected,
		wlan_notification_msm_roaming_start,
		wlan_notification_msm_roaming_end,
		wlan_notification_msm_radio_state_change,
		wlan_notification_msm_signal_quality_change,
		wlan_notification_msm_disassociating,
		wlan_notification_msm_disconnected,
		wlan_notification_msm_peer_join,
		wlan_notification_msm_peer_leave,
		wlan_notification_msm_adapter_removal,
		wlan_notification_msm_adapter_operation_mode_change,
		wlan_notification_msm_end
	}

	/// <summary>
	/// Defines the mode of connection.
	/// </summary>
	/// <remarks>
	/// Corresponds to the native <c>WLAN_CONNECTION_MODE</c> type.
	/// </remarks>
	public enum WLAN_CONNECTION_MODE {
		/// <summary>
		/// A profile will be used to make the connection.
		/// </summary>
		wlan_connection_mode_profile = 0,
		/// <summary>
		/// A temporary profile will be used to make the connection.
		/// </summary>
		wlan_connection_mode_temporary_profile,
		/// <summary>
		/// Secure discovery will be used to make the connection.
		/// </summary>
		wlan_connection_mode_discovery_secure,
		/// <summary>
		/// Unsecure discovery will be used to make the connection.
		/// </summary>
		wlan_connection_mode_discovery_unsecure,
		/// <summary>
		/// A connection will be made automatically, generally using a persistent profile.
		/// </summary>
		wlan_connection_mode_auto,
		/// <summary>
		/// Not used.
		/// </summary>
		wlan_connection_mode_invalid
	}

	/*/// <summary>
	/// The connection state of an ad hoc network.
	/// </summary>
	public enum WLAN_ADHOC_NETWORK_STATE
	{
		/// <summary>
		/// The ad hoc network has been formed, but no client or host is connected to the network.
		/// </summary>
		wlan_adhoc_network_state_formed = 0,
		/// <summary>
		/// A client or host is connected to the ad hoc network.
		/// </summary>
		wlan_adhoc_network_state_connected = 1
	}*/

	/// <summary>
	/// Defines reasons for a failure of a WLAN operation.
	/// </summary>
	/// <remarks>
	/// Corresponds to the native reason code identifiers (<c>WLAN_REASON_CODE_xxx</c> identifiers).
	/// </remarks>
	public enum WLAN_REASON_CODE {
		WLAN_REASON_CODE_SUCCESS = 0,
		// general codes
		WLAN_REASON_CODE_UNKNOWN = 0x10000 + 1,

		WLAN_REASON_CODE_RANGE_SIZE = 0x10000,
		WLAN_REASON_CODE_BASE = 0x10000 + WLAN_REASON_CODE_RANGE_SIZE,

		// range for Auto Config
		//
		WLAN_REASON_CODE_AC_BASE = 0x10000 + WLAN_REASON_CODE_RANGE_SIZE,
		WLAN_REASON_CODE_AC_CONNECT_BASE = (WLAN_REASON_CODE_AC_BASE + WLAN_REASON_CODE_RANGE_SIZE / 2),
		WLAN_REASON_CODE_AC_END = (WLAN_REASON_CODE_AC_BASE + WLAN_REASON_CODE_RANGE_SIZE - 1),

		// range for profile manager
		// it has profile adding failure reason codes, but may not have 
		// connection reason codes
		//
		WLAN_REASON_CODE_PROFILE_BASE = 0x10000 + (7 * WLAN_REASON_CODE_RANGE_SIZE),
		WLAN_REASON_CODE_PROFILE_CONNECT_BASE = (WLAN_REASON_CODE_PROFILE_BASE + WLAN_REASON_CODE_RANGE_SIZE / 2),
		WLAN_REASON_CODE_PROFILE_END = (WLAN_REASON_CODE_PROFILE_BASE + WLAN_REASON_CODE_RANGE_SIZE - 1),

		// range for WLAN_NOTIFICATION_SOURCE_MSM
		//
		WLAN_REASON_CODE_MSM_BASE = 0x10000 + (2 * WLAN_REASON_CODE_RANGE_SIZE),
		WLAN_REASON_CODE_MSM_CONNECT_BASE = (WLAN_REASON_CODE_MSM_BASE + WLAN_REASON_CODE_RANGE_SIZE / 2),
		WLAN_REASON_CODE_MSM_END = (WLAN_REASON_CODE_MSM_BASE + WLAN_REASON_CODE_RANGE_SIZE - 1),

		// range for MSMSEC
		//
		WLAN_REASON_CODE_MSMSEC_BASE = 0x10000 + (3 * WLAN_REASON_CODE_RANGE_SIZE),
		WLAN_REASON_CODE_MSMSEC_CONNECT_BASE = (WLAN_REASON_CODE_MSMSEC_BASE + WLAN_REASON_CODE_RANGE_SIZE / 2),
		WLAN_REASON_CODE_MSMSEC_END = (WLAN_REASON_CODE_MSMSEC_BASE + WLAN_REASON_CODE_RANGE_SIZE - 1),

		// AC network incompatible reason codes
		//
		WLAN_REASON_CODE_NETWORK_NOT_COMPATIBLE = (WLAN_REASON_CODE_AC_BASE + 1),
		WLAN_REASON_CODE_PROFILE_NOT_COMPATIBLE = (WLAN_REASON_CODE_AC_BASE + 2),

		// AC connect reason code
		//
		WLAN_REASON_CODE_NO_AUTO_CONNECTION = (WLAN_REASON_CODE_AC_CONNECT_BASE + 1),
		WLAN_REASON_CODE_NOT_VISIBLE = (WLAN_REASON_CODE_AC_CONNECT_BASE + 2),
		WLAN_REASON_CODE_GP_DENIED = (WLAN_REASON_CODE_AC_CONNECT_BASE + 3),
		WLAN_REASON_CODE_USER_DENIED = (WLAN_REASON_CODE_AC_CONNECT_BASE + 4),
		WLAN_REASON_CODE_BSS_TYPE_NOT_ALLOWED = (WLAN_REASON_CODE_AC_CONNECT_BASE + 5),
		WLAN_REASON_CODE_IN_FAILED_LIST = (WLAN_REASON_CODE_AC_CONNECT_BASE + 6),
		WLAN_REASON_CODE_IN_BLOCKED_LIST = (WLAN_REASON_CODE_AC_CONNECT_BASE + 7),
		WLAN_REASON_CODE_SSID_LIST_TOO_LONG = (WLAN_REASON_CODE_AC_CONNECT_BASE + 8),
		WLAN_REASON_CODE_CONNECT_CALL_FAIL = (WLAN_REASON_CODE_AC_CONNECT_BASE + 9),
		WLAN_REASON_CODE_SCAN_CALL_FAIL = (WLAN_REASON_CODE_AC_CONNECT_BASE + 10),
		WLAN_REASON_CODE_NETWORK_NOT_AVAILABLE = (WLAN_REASON_CODE_AC_CONNECT_BASE + 11),
		WLAN_REASON_CODE_PROFILE_CHANGED_OR_DELETED = (WLAN_REASON_CODE_AC_CONNECT_BASE + 12),
		WLAN_REASON_CODE_KEY_MISMATCH = (WLAN_REASON_CODE_AC_CONNECT_BASE + 13),
		WLAN_REASON_CODE_USER_NOT_RESPOND = (WLAN_REASON_CODE_AC_CONNECT_BASE + 14),
		WLAN_REASON_CODE_AP_PROFILE_NOT_ALLOWED_FOR_CLIENT = (WLAN_REASON_CODE_AC_CONNECT_BASE + 15),
		WLAN_REASON_CODE_AP_PROFILE_NOT_ALLOWED = (WLAN_REASON_CODE_AC_CONNECT_BASE + 16),

		// Profile validation errors
		//
		WLAN_REASON_CODE_INVALID_PROFILE_SCHEMA = (WLAN_REASON_CODE_PROFILE_BASE + 1),
		WLAN_REASON_CODE_PROFILE_MISSING = (WLAN_REASON_CODE_PROFILE_BASE + 2),
		WLAN_REASON_CODE_INVALID_PROFILE_NAME = (WLAN_REASON_CODE_PROFILE_BASE + 3),
		WLAN_REASON_CODE_INVALID_PROFILE_TYPE = (WLAN_REASON_CODE_PROFILE_BASE + 4),
		WLAN_REASON_CODE_INVALID_PHY_TYPE = (WLAN_REASON_CODE_PROFILE_BASE + 5),
		WLAN_REASON_CODE_MSM_SECURITY_MISSING = (WLAN_REASON_CODE_PROFILE_BASE + 6),
		WLAN_REASON_CODE_IHV_SECURITY_NOT_SUPPORTED = (WLAN_REASON_CODE_PROFILE_BASE + 7),
		WLAN_REASON_CODE_IHV_OUI_MISMATCH = (WLAN_REASON_CODE_PROFILE_BASE + 8),
		// WLAN_NOTIFICATION_SOURCE_IHV OUI not present but there is WLAN_NOTIFICATION_SOURCE_IHV settings in profile
		WLAN_REASON_CODE_IHV_OUI_MISSING = (WLAN_REASON_CODE_PROFILE_BASE + 9),
		// WLAN_NOTIFICATION_SOURCE_IHV OUI is present but there is no WLAN_NOTIFICATION_SOURCE_IHV settings in profile
		WLAN_REASON_CODE_IHV_SETTINGS_MISSING = (WLAN_REASON_CODE_PROFILE_BASE + 10),
		// both/conflict MSMSec and WLAN_NOTIFICATION_SOURCE_IHV security settings exist in profile 
		WLAN_REASON_CODE_CONFLICT_SECURITY = (WLAN_REASON_CODE_PROFILE_BASE + 11),
		// no WLAN_NOTIFICATION_SOURCE_IHV or MSMSec security settings in profile
		WLAN_REASON_CODE_SECURITY_MISSING = (WLAN_REASON_CODE_PROFILE_BASE + 12),
		WLAN_REASON_CODE_INVALID_BSS_TYPE = (WLAN_REASON_CODE_PROFILE_BASE + 13),
		WLAN_REASON_CODE_INVALID_ADHOC_CONNECTION_MODE = (WLAN_REASON_CODE_PROFILE_BASE + 14),
		WLAN_REASON_CODE_NON_BROADCAST_SET_FOR_ADHOC = (WLAN_REASON_CODE_PROFILE_BASE + 15),
		WLAN_REASON_CODE_AUTO_SWITCH_SET_FOR_ADHOC = (WLAN_REASON_CODE_PROFILE_BASE + 16),
		WLAN_REASON_CODE_AUTO_SWITCH_SET_FOR_MANUAL_CONNECTION = (WLAN_REASON_CODE_PROFILE_BASE + 17),
		WLAN_REASON_CODE_IHV_SECURITY_ONEX_MISSING = (WLAN_REASON_CODE_PROFILE_BASE + 18),
		WLAN_REASON_CODE_PROFILE_SSID_INVALID = (WLAN_REASON_CODE_PROFILE_BASE + 19),
		WLAN_REASON_CODE_TOO_MANY_SSID = (WLAN_REASON_CODE_PROFILE_BASE + 20),
		WLAN_REASON_CODE_IHV_CONNECTIVITY_NOT_SUPPORTED = (WLAN_REASON_CODE_PROFILE_BASE + 21),
		WLAN_REASON_CODE_BAD_MAX_NUMBER_OF_CLIENTS_FOR_AP = (WLAN_REASON_CODE_PROFILE_BASE + 22),
		WLAN_REASON_CODE_INVALID_CHANNEL = (WLAN_REASON_CODE_PROFILE_BASE + 23),
		WLAN_REASON_CODE_OPERATION_MODE_NOT_SUPPORTED = (WLAN_REASON_CODE_PROFILE_BASE + 24),
		WLAN_REASON_CODE_AUTO_AP_PROFILE_NOT_ALLOWED = (WLAN_REASON_CODE_PROFILE_BASE + 25),
		WLAN_REASON_CODE_AUTO_CONNECTION_NOT_ALLOWED = (WLAN_REASON_CODE_PROFILE_BASE + 26),

		// WLAN_NOTIFICATION_SOURCE_MSM network incompatible reasons
		//
		WLAN_REASON_CODE_UNSUPPORTED_SECURITY_SET_BY_OS = (WLAN_REASON_CODE_MSM_BASE + 1),
		WLAN_REASON_CODE_UNSUPPORTED_SECURITY_SET = (WLAN_REASON_CODE_MSM_BASE + 2),
		WLAN_REASON_CODE_BSS_TYPE_UNMATCH = (WLAN_REASON_CODE_MSM_BASE + 3),
		WLAN_REASON_CODE_PHY_TYPE_UNMATCH = (WLAN_REASON_CODE_MSM_BASE + 4),
		WLAN_REASON_CODE_DATARATE_UNMATCH = (WLAN_REASON_CODE_MSM_BASE + 5),

		// WLAN_NOTIFICATION_SOURCE_MSM connection failure reasons, to be defined
		// failure reason codes

		// user called to disconnect
		WLAN_REASON_CODE_USER_CANCELLED = (WLAN_REASON_CODE_MSM_CONNECT_BASE + 1),
		// got disconnect while associating
		WLAN_REASON_CODE_ASSOCIATION_FAILURE = (WLAN_REASON_CODE_MSM_CONNECT_BASE + 2),
		// timeout for association
		WLAN_REASON_CODE_ASSOCIATION_TIMEOUT = (WLAN_REASON_CODE_MSM_CONNECT_BASE + 3),
		// pre-association security completed with failure
		WLAN_REASON_CODE_PRE_SECURITY_FAILURE = (WLAN_REASON_CODE_MSM_CONNECT_BASE + 4),
		// fail to start post-association security
		WLAN_REASON_CODE_START_SECURITY_FAILURE = (WLAN_REASON_CODE_MSM_CONNECT_BASE + 5),
		// post-association security completed with failure
		WLAN_REASON_CODE_SECURITY_FAILURE = (WLAN_REASON_CODE_MSM_CONNECT_BASE + 6),
		// security watchdog timeout
		WLAN_REASON_CODE_SECURITY_TIMEOUT = (WLAN_REASON_CODE_MSM_CONNECT_BASE + 7),
		// got disconnect from driver when roaming
		WLAN_REASON_CODE_ROAMING_FAILURE = (WLAN_REASON_CODE_MSM_CONNECT_BASE + 8),
		// failed to start security for roaming
		WLAN_REASON_CODE_ROAMING_SECURITY_FAILURE = (WLAN_REASON_CODE_MSM_CONNECT_BASE + 9),
		// failed to start security for adhoc-join
		WLAN_REASON_CODE_ADHOC_SECURITY_FAILURE = (WLAN_REASON_CODE_MSM_CONNECT_BASE + 10),
		// got disconnection from driver
		WLAN_REASON_CODE_DRIVER_DISCONNECTED = (WLAN_REASON_CODE_MSM_CONNECT_BASE + 11),
		// driver operation failed
		WLAN_REASON_CODE_DRIVER_OPERATION_FAILURE = (WLAN_REASON_CODE_MSM_CONNECT_BASE + 12),
		// Ihv service is not available
		WLAN_REASON_CODE_IHV_NOT_AVAILABLE = (WLAN_REASON_CODE_MSM_CONNECT_BASE + 13),
		// Response from ihv timed out
		WLAN_REASON_CODE_IHV_NOT_RESPONDING = (WLAN_REASON_CODE_MSM_CONNECT_BASE + 14),
		// Timed out waiting for driver to disconnect
		WLAN_REASON_CODE_DISCONNECT_TIMEOUT = (WLAN_REASON_CODE_MSM_CONNECT_BASE + 15),
		// An internal error prevented the operation from being completed.
		WLAN_REASON_CODE_INTERNAL_FAILURE = (WLAN_REASON_CODE_MSM_CONNECT_BASE + 16),
		// UI Request timed out.
		WLAN_REASON_CODE_UI_REQUEST_TIMEOUT = (WLAN_REASON_CODE_MSM_CONNECT_BASE + 17),
		// Roaming too often, post security is not completed after 5 times.
		WLAN_REASON_CODE_TOO_MANY_SECURITY_ATTEMPTS = (WLAN_REASON_CODE_MSM_CONNECT_BASE + 18),
		// Failed to start AP
		WLAN_REASON_CODE_AP_STARTING_FAILURE = (WLAN_REASON_CODE_MSM_CONNECT_BASE + 19),

		// MSMSEC reason codes
		//

		WLAN_REASON_CODE_MSMSEC_MIN = WLAN_REASON_CODE_MSMSEC_BASE,

		// Key index specified is not valid
		WLAN_REASON_CODE_MSMSEC_PROFILE_INVALID_KEY_INDEX = (WLAN_REASON_CODE_MSMSEC_BASE + 1),
		// Key required, PSK present
		WLAN_REASON_CODE_MSMSEC_PROFILE_PSK_PRESENT = (WLAN_REASON_CODE_MSMSEC_BASE + 2),
		// Invalid key length
		WLAN_REASON_CODE_MSMSEC_PROFILE_KEY_LENGTH = (WLAN_REASON_CODE_MSMSEC_BASE + 3),
		// Invalid PSK length
		WLAN_REASON_CODE_MSMSEC_PROFILE_PSK_LENGTH = (WLAN_REASON_CODE_MSMSEC_BASE + 4),
		// No auth/cipher specified
		WLAN_REASON_CODE_MSMSEC_PROFILE_NO_AUTH_CIPHER_SPECIFIED = (WLAN_REASON_CODE_MSMSEC_BASE + 5),
		// Too many auth/cipher specified
		WLAN_REASON_CODE_MSMSEC_PROFILE_TOO_MANY_AUTH_CIPHER_SPECIFIED = (WLAN_REASON_CODE_MSMSEC_BASE + 6),
		// Profile contains duplicate auth/cipher
		WLAN_REASON_CODE_MSMSEC_PROFILE_DUPLICATE_AUTH_CIPHER = (WLAN_REASON_CODE_MSMSEC_BASE + 7),
		// Profile raw data is invalid (1x or key data)
		WLAN_REASON_CODE_MSMSEC_PROFILE_RAWDATA_INVALID = (WLAN_REASON_CODE_MSMSEC_BASE + 8),
		// Invalid auth/cipher combination
		WLAN_REASON_CODE_MSMSEC_PROFILE_INVALID_AUTH_CIPHER = (WLAN_REASON_CODE_MSMSEC_BASE + 9),
		// 802.1x disabled when it's required to be enabled
		WLAN_REASON_CODE_MSMSEC_PROFILE_ONEX_DISABLED = (WLAN_REASON_CODE_MSMSEC_BASE + 10),
		// 802.1x enabled when it's required to be disabled
		WLAN_REASON_CODE_MSMSEC_PROFILE_ONEX_ENABLED = (WLAN_REASON_CODE_MSMSEC_BASE + 11),
		WLAN_REASON_CODE_MSMSEC_PROFILE_INVALID_PMKCACHE_MODE = (WLAN_REASON_CODE_MSMSEC_BASE + 12),
		WLAN_REASON_CODE_MSMSEC_PROFILE_INVALID_PMKCACHE_SIZE = (WLAN_REASON_CODE_MSMSEC_BASE + 13),
		WLAN_REASON_CODE_MSMSEC_PROFILE_INVALID_PMKCACHE_TTL = (WLAN_REASON_CODE_MSMSEC_BASE + 14),
		WLAN_REASON_CODE_MSMSEC_PROFILE_INVALID_PREAUTH_MODE = (WLAN_REASON_CODE_MSMSEC_BASE + 15),
		WLAN_REASON_CODE_MSMSEC_PROFILE_INVALID_PREAUTH_THROTTLE = (WLAN_REASON_CODE_MSMSEC_BASE + 16),
		// PreAuth enabled when PMK cache is disabled
		WLAN_REASON_CODE_MSMSEC_PROFILE_PREAUTH_ONLY_ENABLED = (WLAN_REASON_CODE_MSMSEC_BASE + 17),
		// Capability matching failed at network
		WLAN_REASON_CODE_MSMSEC_CAPABILITY_NETWORK = (WLAN_REASON_CODE_MSMSEC_BASE + 18),
		// Capability matching failed at NIC
		WLAN_REASON_CODE_MSMSEC_CAPABILITY_NIC = (WLAN_REASON_CODE_MSMSEC_BASE + 19),
		// Capability matching failed at profile
		WLAN_REASON_CODE_MSMSEC_CAPABILITY_PROFILE = (WLAN_REASON_CODE_MSMSEC_BASE + 20),
		// Network does not support specified discovery type
		WLAN_REASON_CODE_MSMSEC_CAPABILITY_DISCOVERY = (WLAN_REASON_CODE_MSMSEC_BASE + 21),
		// Passphrase contains invalid character
		WLAN_REASON_CODE_MSMSEC_PROFILE_PASSPHRASE_CHAR = (WLAN_REASON_CODE_MSMSEC_BASE + 22),
		// Key material contains invalid character
		WLAN_REASON_CODE_MSMSEC_PROFILE_KEYMATERIAL_CHAR = (WLAN_REASON_CODE_MSMSEC_BASE + 23),
		// Wrong key type specified for the auth/cipher pair
		WLAN_REASON_CODE_MSMSEC_PROFILE_WRONG_KEYTYPE = (WLAN_REASON_CODE_MSMSEC_BASE + 24),
		// "Mixed cell" suspected (AP not beaconing privacy, we have privacy enabled profile)
		WLAN_REASON_CODE_MSMSEC_MIXED_CELL = (WLAN_REASON_CODE_MSMSEC_BASE + 25),
		// Auth timers or number of timeouts in profile is incorrect
		WLAN_REASON_CODE_MSMSEC_PROFILE_AUTH_TIMERS_INVALID = (WLAN_REASON_CODE_MSMSEC_BASE + 26),
		// Group key update interval in profile is incorrect
		WLAN_REASON_CODE_MSMSEC_PROFILE_INVALID_GKEY_INTV = (WLAN_REASON_CODE_MSMSEC_BASE + 27),
		// "Transition network" suspected, trying legacy 802.11 security
		WLAN_REASON_CODE_MSMSEC_TRANSITION_NETWORK = (WLAN_REASON_CODE_MSMSEC_BASE + 28),
		// Key contains characters which do not map to ASCII
		WLAN_REASON_CODE_MSMSEC_PROFILE_KEY_UNMAPPED_CHAR = (WLAN_REASON_CODE_MSMSEC_BASE + 29),
		// Capability matching failed at profile (auth not found)
		WLAN_REASON_CODE_MSMSEC_CAPABILITY_PROFILE_AUTH = (WLAN_REASON_CODE_MSMSEC_BASE + 30),
		// Capability matching failed at profile (cipher not found)
		WLAN_REASON_CODE_MSMSEC_CAPABILITY_PROFILE_CIPHER = (WLAN_REASON_CODE_MSMSEC_BASE + 31),
		// Safe mode value is invalid
		WLAN_REASON_CODE_MSMSEC_PROFILE_SAFE_MODE = (WLAN_REASON_CODE_MSMSEC_BASE + 32),
		// Profile requires safe mode, not supported by NIC
		WLAN_REASON_CODE_MSMSEC_CAPABILITY_PROFILE_SAFE_MODE_NIC = (WLAN_REASON_CODE_MSMSEC_BASE + 33),
		// Profile requires safe mode, not supported by network
		WLAN_REASON_CODE_MSMSEC_CAPABILITY_PROFILE_SAFE_MODE_NW = (WLAN_REASON_CODE_MSMSEC_BASE + 34),
		// Profile has unsupported auth
		WLAN_REASON_CODE_MSMSEC_PROFILE_UNSUPPORTED_AUTH = (WLAN_REASON_CODE_MSMSEC_BASE + 35),
		// Profile has unsupported cipher
		WLAN_REASON_CODE_MSMSEC_PROFILE_UNSUPPORTED_CIPHER = (WLAN_REASON_CODE_MSMSEC_BASE + 36),

		// Failed to queue UI request
		WLAN_REASON_CODE_MSMSEC_UI_REQUEST_FAILURE = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 1),
		// 802.1x authentication did not start within configured time 
		WLAN_REASON_CODE_MSMSEC_AUTH_START_TIMEOUT = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 2),
		// 802.1x authentication did not complete within configured time
		WLAN_REASON_CODE_MSMSEC_AUTH_SUCCESS_TIMEOUT = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 3),
		// Dynamic key exchange did not start within configured time
		WLAN_REASON_CODE_MSMSEC_KEY_START_TIMEOUT = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 4),
		// Dynamic key exchange did not succeed within configured time
		WLAN_REASON_CODE_MSMSEC_KEY_SUCCESS_TIMEOUT = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 5),
		// Message 3 of 4 way handshake has no key data (RSN/WPA)
		WLAN_REASON_CODE_MSMSEC_M3_MISSING_KEY_DATA = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 6),
		// Message 3 of 4 way handshake has no IE (RSN/WPA)
		WLAN_REASON_CODE_MSMSEC_M3_MISSING_IE = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 7),
		// Message 3 of 4 way handshake has no Group Key (RSN)
		WLAN_REASON_CODE_MSMSEC_M3_MISSING_GRP_KEY = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 8),
		// Matching security capabilities of IE in M3 failed (RSN/WPA)
		WLAN_REASON_CODE_MSMSEC_PR_IE_MATCHING = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 9),
		// Matching security capabilities of Secondary IE in M3 failed (RSN)
		WLAN_REASON_CODE_MSMSEC_SEC_IE_MATCHING = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 10),
		// Required a pairwise key but AP configured only group keys
		WLAN_REASON_CODE_MSMSEC_NO_PAIRWISE_KEY = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 11),
		// Message 1 of group key handshake has no key data (RSN/WPA)
		WLAN_REASON_CODE_MSMSEC_G1_MISSING_KEY_DATA = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 12),
		// Message 1 of group key handshake has no group key
		WLAN_REASON_CODE_MSMSEC_G1_MISSING_GRP_KEY = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 13),
		// AP reset secure bit after connection was secured
		WLAN_REASON_CODE_MSMSEC_PEER_INDICATED_INSECURE = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 14),
		// 802.1x indicated there is no authenticator but profile requires 802.1x
		WLAN_REASON_CODE_MSMSEC_NO_AUTHENTICATOR = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 15),
		// Plumbing settings to NIC failed
		WLAN_REASON_CODE_MSMSEC_NIC_FAILURE = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 16),
		// Operation was cancelled by caller
		WLAN_REASON_CODE_MSMSEC_CANCELLED = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 17),
		// Key was in incorrect format 
		WLAN_REASON_CODE_MSMSEC_KEY_FORMAT = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 18),
		// WLAN_NOTIFICATION_SOURCE_SECURITY downgrade detected
		WLAN_REASON_CODE_MSMSEC_DOWNGRADE_DETECTED = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 19),
		// PSK mismatch suspected
		WLAN_REASON_CODE_MSMSEC_PSK_MISMATCH_SUSPECTED = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 20),
		// Forced failure because connection method was not secure
		WLAN_REASON_CODE_MSMSEC_FORCED_FAILURE = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 21),
		// Message 3 of 4 way handshake contains too many RSN IE (RSN)
		WLAN_REASON_CODE_MSMSEC_M3_TOO_MANY_RSNIE = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 22),
		// Message 2 of 4 way handshake has no key data (RSN Adhoc)
		WLAN_REASON_CODE_MSMSEC_M2_MISSING_KEY_DATA = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 23),
		// Message 2 of 4 way handshake has no IE (RSN Adhoc)
		WLAN_REASON_CODE_MSMSEC_M2_MISSING_IE = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 24),
		WLAN_REASON_CODE_MSMSEC_AUTH_WCN_COMPLETED = (WLAN_REASON_CODE_MSMSEC_CONNECT_BASE + 25),

		WLAN_REASON_CODE_MSMSEC_MAX = WLAN_REASON_CODE_MSMSEC_END
	}

	/// <summary>
	/// Indicates the state of an interface.
	/// </summary>
	/// <remarks>
	/// Corresponds to the native <c>WLAN_INTERFACE_STATE</c> type.
	/// </remarks>
	public enum WLAN_INTERFACE_STATE {
		/// <summary>
		/// The interface is not ready to operate.
		/// </summary>
		wlan_interface_state_not_ready = 0,
		/// <summary>
		/// The interface is connected to a network.
		/// </summary>
		wlan_interface_state_connected = 1,
		/// <summary>
		/// The interface is the first node in an ad hoc network. No peer has connected.
		/// </summary>
		wlan_interface_state_ad_hoc_network_formed = 2,
		/// <summary>
		/// The interface is disconnecting from the current network.
		/// </summary>
		wlan_interface_state_disconnecting = 3,
		/// <summary>
		/// The interface is not connected to any network.
		/// </summary>
		wlan_interface_state_disconnected = 4,
		/// <summary>
		/// The interface is attempting to associate with a network.
		/// </summary>
		wlan_interface_state_associating = 5,
		/// <summary>
		/// Auto configuration is discovering the settings for the network.
		/// </summary>
		wlan_interface_state_discovering = 6,
		/// <summary>
		/// The interface is in the process of authenticating.
		/// </summary>
		wlan_interface_state_authenticating = 7
	}

	/// <summary>
	/// http://msdn.microsoft.com/en-us/library/dd439506%28VS.85%29.aspx
	/// </summary>
	public enum WLAN_HOSTED_NETWORK_REASON {
		wlan_hosted_network_reason_success = 0,
		wlan_hosted_network_reason_unspecified,
		wlan_hosted_network_reason_bad_parameters,
		wlan_hosted_network_reason_service_shutting_down,
		wlan_hosted_network_reason_insufficient_resources,
		wlan_hosted_network_reason_elevation_required,
		wlan_hosted_network_reason_read_only,
		wlan_hosted_network_reason_persistence_failed,
		wlan_hosted_network_reason_crypt_error,
		wlan_hosted_network_reason_impersonation,
		wlan_hosted_network_reason_stop_before_start,
		wlan_hosted_network_reason_interface_available,
		wlan_hosted_network_reason_interface_unavailable,
		wlan_hosted_network_reason_miniport_stopped,
		wlan_hosted_network_reason_miniport_started,
		wlan_hosted_network_reason_incompatible_connection_started,
		wlan_hosted_network_reason_incompatible_connection_stopped,
		wlan_hosted_network_reason_user_action,
		wlan_hosted_network_reason_client_abort,
		wlan_hosted_network_reason_ap_start_failed,
		wlan_hosted_network_reason_peer_arrived,
		wlan_hosted_network_reason_peer_departed,
		wlan_hosted_network_reason_peer_timeout,
		wlan_hosted_network_reason_gp_denied,
		wlan_hosted_network_reason_service_unavailable,
		wlan_hosted_network_reason_device_change,
		wlan_hosted_network_reason_properties_change,
		wlan_hosted_network_reason_virtual_station_blocking_use,
		wlan_hosted_network_reason_service_available_on_virtual_station
	}

	/// <summary>
	/// http://msdn.microsoft.com/en-us/library/dd439502%28VS.85%29.aspx
	/// </summary>
	public enum WLAN_HOSTED_NETWORK_OPCODE {
		wlan_hosted_network_opcode_connection_settings,
		wlan_hosted_network_opcode_security_settings,
		wlan_hosted_network_opcode_station_profile,
		wlan_hosted_network_opcode_enable,
	}

	/// <summary>
	/// http://msdn.microsoft.com/en-us/library/dd439508%28VS.85%29.aspx
	/// </summary>
	public enum WLAN_HOSTED_NETWORK_STATE {
		wlan_hosted_network_unavailable,
		wlan_hosted_network_idle,
		wlan_hosted_network_active,
	}

	/// <summary>
	/// http://msdn.microsoft.com/en-us/library/ms706027%28VS.85%29.aspx
	/// </summary>
	public enum DOT11_RADIO_STATE {
		dot11_radio_state_unknown = 0,
		dot11_radio_state_on,
		dot11_radio_state_off
	}

	public enum WLAN_HOSTED_NETWORK_NOTIFICATION_CODE {
		wlan_hosted_network_state_change = 0x00001000,	//4096
		wlan_hosted_network_peer_state_change,			//4097
		wlan_hosted_network_radio_state_change			//4098
	}

	public enum WLAN_HOSTED_NETWORK_PEER_AUTH_STATE {
		wlan_hosted_network_peer_state_invalid,
		wlan_hosted_network_peer_state_authenticated,
	}
}
