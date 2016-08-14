using System.Net;
using System;
using System.Net.Sockets;
using winapi;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace wlan
{
	/// <summary>
	/// Class responsible for retrieving additional network information given device mac address
	/// </summary>
	public static class DnsInterface
	{
		/// <summary>
		/// Client's ip not found yet
		/// </summary>
		public static string stringIp = "?.?.?.?";

		/// <summary>
		/// Client's name not found yet
		/// </summary>
		public static string stringHost = "Identifying...";

		private readonly static char[] splitOne = { '\n', '\r' };
		private readonly static char[] splitTwo = { ' ', '\t' };

		/// <summary>
		/// Method retrieves ip address from given mac address
		/// </summary>
		/// <param name="maxAddress"></param>
		/// <returns></returns>
		public static bool GetClientIpFromMac(string macAddress, ref string ipAddress)
		{
			string[] ipWithMac = null;
			ipAddress = stringIp;

			foreach(string entry in GetARPResult().Split(splitOne))
			{
				if(string.IsNullOrEmpty(entry) == true)
					continue;

				try
				{
					ipWithMac = entry.Split(splitTwo);
				}
				catch(ArgumentNullException e)
				{
					Logger.LogException("DnsInterface", "GetClientIpFromMac", "Error parsing arp entry", e);
					continue;
				}

				ipWithMac = RemoveEmptySpaces(ipWithMac);

				try
				{
					if(macAddress.ToUpperInvariant().Equals(ipWithMac[1].ToUpperInvariant()))
					{
						ipAddress = ipWithMac[0];
						break;
					}
				}
				catch(NullReferenceException e)
				{
					Logger.LogException("DnsInterface", "GetClientIpFromMac", "Error comparing arp entries", e);
					continue;
				}
			}

			ipWithMac = null;

			if(ipAddress.Equals(stringIp))
			{
				Logger.LogDebug("DnsInterface", "GetClientIpFromMac", "Could not find IP entry for " + macAddress);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Method getting client name from given ip address
		/// In case name not found returns "Unknown" string
		/// </summary>
		/// <param name="ipAddress"></param>
		/// <returns></returns>
		public static bool GetClientNameFromIp(string ipAddress, ref string name)
		{
			try
			{
				name = Dns.GetHostEntry(ipAddress).HostName;
			}
			catch(ArgumentNullException e)
			{
				Logger.LogException("DnsInterface", "GetClientNameFromIp", "Error retrieving client name", e);
			}
			catch(ArgumentOutOfRangeException e)
			{
				Logger.LogException("DnsInterface", "GetClientNameFromIp", "Error retrieving client name", e);
			}
			catch(SocketException e)
			{
				Logger.LogException("DnsInterface", "GetClientNameFromIp", "Error retrieving client name", e);
			}
			catch(ArgumentException e)
			{
				Logger.LogException("DnsInterface", "GetClientNameFromIp", "Error retrieving client name", e);
			}

			if(string.IsNullOrEmpty(name))
			{
				name = stringHost;
				Logger.LogDebug("DnsInterface", "GetClientNameFromIp", "Could not find hostname entry for " + ipAddress);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Method removes empty entries from arp split entries
		/// </summary>
		/// <param name="pieces"></param>
		/// <returns></returns>
		private static string[] RemoveEmptySpaces(string[] pieces)
		{
			string[] result = new string[2];
			int i = 0;

			foreach(string item in pieces)
			{
				if(string.IsNullOrEmpty(item))
					continue;

				result[i++] = item;

				if(i == 2)
					break;
			}

			return result;
		}

		/// <summary>
		/// Method to execute arp -a query and get the answer as an array
		/// </summary>
		/// <returns></returns>
		private static string GetARPResult()
		{
			Process process = null;
			string output = null;

			try
			{
				process = Process.Start(new ProcessStartInfo("arp", "-a")
				{
					RedirectStandardOutput = true,
					CreateNoWindow = true,
					UseShellExecute = false
				});

				output = process.StandardOutput.ReadToEnd();
			}
			catch(ObjectDisposedException e)
			{
				Logger.LogException("DnsInterface", "GetARPResult", "Error retrieving arp info", e);
			}
			catch(InvalidOperationException e)
			{
				Logger.LogException("DnsInterface", "GetARPResult", "Error retrieving arp info", e);
			}
			catch(ArgumentNullException e)
			{
				Logger.LogException("DnsInterface", "GetARPResult", "Error retrieving arp info", e);
			}
			catch(Win32Exception e)
			{
				Logger.LogException("DnsInterface", "GetARPResult", "Error retrieving arp info", e);
			}
			catch(OutOfMemoryException e)
			{
				Logger.LogException("DnsInterface", "GetARPResult", "Error retrieving arp info", e);
			}
			catch(IOException e)
			{
				Logger.LogException("DnsInterface", "GetARPResult", "Error retrieving arp info", e);
			}
			finally
			{
				if(process != null)
				{
					process.Close();
					process.Dispose();
				}
			}

			return output;
		}

		/// <summary>
		/// Method used to clear arp cache after client disconnection.
		/// In case it is not cleared, after second connection client will be almost
		/// certainly identified as previously, whereas each time client is given
		/// a new ip address. This is due to the fact that arp does not clear it's
		/// cache immediately after device disconnection.
		/// </summary>
		/// <param name="ipAddress"></param>
		public static void ClearArpCache(string ipAddress)
		{
			if(ipAddress.Equals(stringIp))
				return;

			Process process = null;

			try
			{
				process = Process.Start(new ProcessStartInfo("arp", "-d " + ipAddress)
				{
					RedirectStandardOutput = true,
					CreateNoWindow = true,
					UseShellExecute = false
				});

				//output = process.StandardOutput.ReadToEnd();

				Logger.LogDebug("DnsInterface", "ClearArpCache", "Cleared arp cache for " + ipAddress);
			}
			catch(ObjectDisposedException e)
			{
				Logger.LogException("DnsInterface", "ClearArpCache", "Error updating arp cache", e);
			}
			catch(InvalidOperationException e)
			{
				Logger.LogException("DnsInterface", "ClearArpCache", "Error updating arp cache", e);
			}
			catch(ArgumentNullException e)
			{
				Logger.LogException("DnsInterface", "ClearArpCache", "Error updating arp cache", e);
			}
			catch(Win32Exception e)
			{
				Logger.LogException("DnsInterface", "ClearArpCache", "Error updating arp cache", e);
			}
			catch(OutOfMemoryException e)
			{
				Logger.LogException("DnsInterface", "ClearArpCache", "Error updating arp cache", e);
			}
			catch(IOException e)
			{
				Logger.LogException("DnsInterface", "ClearArpCache", "Error updating arp cache", e);
			}
			finally
			{
				if(process != null)
				{
					process.Close();
					process.Dispose();
				}
			}
		}
	}
}
