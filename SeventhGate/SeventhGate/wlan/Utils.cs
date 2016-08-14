using System.ComponentModel;
using System.Text;
using winapi;
using System;

namespace wlan
{
	/// <summary>
	/// This is the notificationSource file that contains utility functions used with MicrosoftWlanManager
	/// </summary>
	public static class Utils
	{
		/// <summary>
		/// Throws win 32 exception if retCode is != 0
		/// <exception cref="Win32Exception"></exception>
		/// </summary>
		/// <param name="retCode"></param>
		public static void ThrowOnWin32Error(int retCode)
		{
			if(retCode != 0)
				throw new Win32Exception(retCode);
		}

		/// <summary>
		/// Converts mac address to string
		/// </summary>
		/// <param name="mac"></param>
		/// <returns></returns>
		public static string ConvertToString(this DOT11_MAC_ADDRESS mac)
		{
			return ConvertToHexString(mac.one) + "-" +
				ConvertToHexString(mac.two) + "-" +
				ConvertToHexString(mac.three) + "-" +
				ConvertToHexString(mac.four) + "-" +
				ConvertToHexString(mac.five) + "-" +
				ConvertToHexString(mac.six);
		}

		/// <summary>
		/// Converts bytes to mac address given in characters
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static string ConvertToHexString(this byte value)
		{
			return Convert.ToString(value, 0x10).PadLeft(2, '0').ToUpperInvariant();
		}

		/// <summary>
		/// Creates dot11ssid structure based on human-readable string
		/// Takes only first 32 characters of string
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static DOT11_SSID ConvertStringToSsid(String name)
		{
			if(name.Length > 32)
				name = name.Substring(0, 32);

			DOT11_SSID ssid = new DOT11_SSID();
			ssid.SSID = new byte[32];

			ASCIIEncoding encoding = new ASCIIEncoding();

			Byte[] bytes = encoding.GetBytes(name);
			uint length = (uint)bytes.Length;

			ssid.uSSIDLength = length;
			for(int i = 0; i < ssid.SSID.Length; i++)
			{
				if(i < length)
					ssid.SSID[i] = bytes[i];
				else
					ssid.SSID[i] = 0;
			}

			return ssid;
		}
	}
}
