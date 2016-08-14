using System;
using NETCONLib;
using System.Collections.ObjectModel;
using winapi;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.ComponentModel;

namespace ics {

	/// <summary>
	///  Class used to interface between the HostedNetwork application and ICS.
	///  MicrosoftIcsManager static library is used to interface between the 
	///  HostedNetwork application and ICS.
	/// </summary>
	public class IcsManager : IDisposable {

		private bool disposed;
		private readonly int maxTries = 3;
		private readonly int sleepTime = 5000;
		private string virtualAdapterDeviceName = "MICROSOFT VIRTUAL WIFI MINIPORT ADAPTER";

		private INetSharingManager netSharingManager = null;
		private Collection<IcsConnection> icsConnections = null;

		/// <summary>
		/// Creates new instance of IcsManager class
		/// </summary>
		public IcsManager() {
			try {
				netSharingManager = new NetSharingManagerClass();
			} catch(Win32Exception e) {
				Logger.LogException("IcsManager", "IcsManager", "Win32Exception instantiating NetSharingManagerClass", e);
			} catch(COMException e) {
				Logger.LogException("IcsManager", "IcsManager", "COMException instantiating NetSharingManagerClass", e);
			} catch(Exception e) {
				Logger.LogException("IcsManager", "IcsManager", "Exception instantiating NetSharingManagerClass", e);
			}

			icsConnections = new Collection<IcsConnection>();
		}

		/// <summary>
		/// Accessible and sharing-enabled connections
		/// Returns current range of connections, refreshed at each access
		/// NULL if could not access object
		/// </summary>
		public Collection<IcsConnection> IcsConnections {
			get {
				if(netSharingManager == null)
					return null;

				icsConnections.Clear();

				try {
					foreach(INetConnection conn in netSharingManager.EnumEveryConnection)
						icsConnections.Add(new IcsConnection(netSharingManager, conn));
					return icsConnections;
				} catch(Win32Exception e) {
					Logger.LogException("IcsManager", "IcsConnections", "Win32Exception enumerating connections, trying again...", e);
				} catch(COMException e) {
					Logger.LogException("IcsManager", "IcsConnections", "COMException enumerating connections, trying again...", e);
				} catch(Exception e) {
					Logger.LogException("IcsManager", "IcsConnections", "Exception enumerating connections, trying again...", e);
				}

				Logger.LogError("IcsManager", "IcsConnections", "Could not enumerate connections. Try restarting program.");
				return icsConnections;
			}
		}

		/// <summary>
		/// Returns current range of active connections
		/// Collection refreshed at each access
		/// NULL if could not access object
		/// </summary>
		public Collection<IcsConnection> IcsActiveConnections {
			get {
				Collection<IcsConnection> active = new Collection<IcsConnection>();
				foreach(IcsConnection conn in IcsConnections)
					if(conn.IsConnected && conn.IsSupported)
						active.Add(conn);

				return active;
			}
		}

		/// <summary>
		/// Enables ics for given GUIDS
		/// </summary>
		/// <param name="publicGuidValue"></param>
		/// <param name="privateGuidValue"></param>
		public bool EnableIcs(IcsConnection publicConn, IcsConnection privateConn) {
			if(publicConn == null || privateConn == null) {
				Logger.LogError("IcsManager", "EnableIcs", "Wrong connections. Retry.");
				return false;
			}

			if(publicConn.IsSupported == false || privateConn.IsSupported == false) {
				Logger.LogError("IcsManager", "EnableIcs", "ICS for connections not supported. Retry.");
				return false;
			}

			if(publicConn.IsOn == false || privateConn.IsOn == false) {
				Logger.LogError("IcsManager", "EnableIcs", "Connections down. Retry.");
				return false;
			}

			int counter = 0;

			while(counter++ < maxTries) {
				if(publicConn.EnableSharing(true) == true)
					break;
				else {
					Logger.LogError("IcsManager", "EnableIcs", "Could not share public, trying again...");
					Thread.Sleep(sleepTime);
				}
			}

			if(publicConn.IsSharingEnabled == false)
				return false;

			counter = 0;

			while(counter++ < maxTries) {
				if(privateConn.EnableSharing(false) == true)
					break;
				else {
					Logger.LogError("IcsManager", "EnableIcs", "Could not share private, trying again...");
					Thread.Sleep(sleepTime);
				}
			}

			return privateConn.IsSharingEnabled;
		}

		/// <summary>
		/// TRUE if Internet Connection Sharing installed
		/// FALSE if not or could not access object
		/// </summary>
		public bool SharingInstalled {
			get {
				if(netSharingManager != null)
					return netSharingManager.SharingInstalled;
				else
					return false;
			}
		}

		/// <summary>
		/// Gets first available virtual connection
		/// </summary>
		/// <returns>NULL if could not access object or not found</returns>
		public IcsConnection VirtualPrivateConnection() {
			foreach(IcsConnection conn in IcsConnections)
				if(conn.DeviceName.ToUpperInvariant().Contains(virtualAdapterDeviceName))
					return conn;

			return null;
		}

		/// <summary>
		/// Amount of private virtual connections
		/// </summary>
		/// <returns>
		/// -1 if could not acces object
		/// 0 if none found
		/// </returns>
		public int PrivateConnectionsCount {
			get {
				// As for VirtualPrivateConnection, this may not always work (if previously terminated unexpectedly)
				//return netSharingManager.get_EnumPrivateConnections(tagSHARINGCONNECTION_ENUM_FLAGS.ICSSC_ENABLED).Count;
				int counter = 0;

				foreach(IcsConnection conn in IcsConnections)
					if(conn.DeviceName.ToUpperInvariant().Contains(virtualAdapterDeviceName))
						counter++;

				return counter;
			}
		}

		/// <summary>
		/// Disables ics on all icsConnections
		/// </summary>
		/// <returns>
		/// TRUE if attempted for all connections
		/// FALSE if could not access object
		/// </returns>
		public bool DisableIcsOnAll() {
			bool result = true;

			foreach(IcsConnection connection in IcsConnections)
				if(connection.DisableSharing() == false)
					result = false;

			return result;
		}

		#region cleanup

		/// <summary>
		/// Disposes
		/// </summary>
		public void Dispose() {
			Dispose(true);

			try {
				GC.SuppressFinalize(this);
			} catch(ArgumentNullException e) {
				Logger.LogException("IcsManager", "Dispose", "Error disposing", e);
			}
		}

		private void Dispose(bool disposing) {
			if(!disposed) {
				if(disposing) {
					// Dispose managed resources.

					foreach(IcsConnection con in icsConnections)
						con.Dispose();
					icsConnections.Clear();
				}

				// Clean up unmanaged resources here.
				netSharingManager = null;

				disposed = true;
			}
		}

		//Disposable types should declare finalizer
		~IcsManager() {
			Dispose(false);
		}

		#endregion
	}
}
