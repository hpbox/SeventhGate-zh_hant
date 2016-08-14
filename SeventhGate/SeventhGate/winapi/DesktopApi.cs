using System.Runtime.InteropServices;
using System;

namespace winapi
{
	/// <summary>
	/// Class responsible for getting desktop taskbar
	/// coordinates for proper main window placing
	/// </summary>
	public static class DesktopApi
	{
		[StructLayout(LayoutKind.Sequential)]
		private struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct APPBARDATA
		{
			public int cbSize;
			public IntPtr hWnd;
			public int uCallbackMessage;
			public int uEdge;
			public RECT rc;
			public IntPtr lParam;
		}

		private enum ABMsg :int
		{
			ABM_NEW = 0,
			ABM_REMOVE = 1,
			ABM_QUERYPOS = 2,
			ABM_SETPOS = 3,
			ABM_GETSTATE = 4,
			ABM_GETTASKBARPOS = 5,
			ABM_ACTIVATE = 6,
			ABM_GETAUTOHIDEBAR = 7,
			ABM_SETAUTOHIDEBAR = 8,
			ABM_WINDOWPOSCHANGED = 9,
			ABM_SETSTATE = 10
		}

		/// <summary>
		/// Enumerates taskbar edge placement
		/// </summary>
		private enum ABEdge :int
		{
			ABE_LEFT = 0,
			ABE_TOP,
			ABE_RIGHT,
			ABE_BOTTOM
		}

		/*enum ABState :int
		{
			ABS_MANUAL = 0,
			ABS_AUTOHIDE = 1,
			ABS_ALWAYSONTOP = 2,
			ABS_AUTOHIDEANDONTOP = 3,
		}*/

		/// <summary>
		/// Identifies edge at which taskbar is currently located
		/// </summary>
		public enum TaskBarEdge :int
		{
			Bottom,
			Top,
			Left,
			Right
		}

		[DllImport("SHELL32", CallingConvention = CallingConvention.StdCall)]
		private static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

		/// <summary>
		/// Returns information about Desktop taskbar
		/// </summary>
		/// <param name="taskBarEdge"></param>
		/// <param name="height"></param>
		/// <param name="width"></param>
		public static void GetTaskBarInfo(out TaskBarEdge taskBarEdge, out int height, out int width)
		{
			APPBARDATA abd = new APPBARDATA();

			taskBarEdge = TaskBarEdge.Bottom;
			height = 0;
			width = 0;

			//uint ret = SHAppBarMessage((int)ABMsg.ABM_GETTASKBARPOS, ref abd);
			SHAppBarMessage((int)ABMsg.ABM_GETTASKBARPOS, ref abd);

			switch(abd.uEdge)
			{
				case (int)ABEdge.ABE_BOTTOM:
					taskBarEdge = TaskBarEdge.Bottom;
					height = abd.rc.bottom - abd.rc.top;
					width = abd.rc.right;
					break;
				case (int)ABEdge.ABE_TOP:
					taskBarEdge = TaskBarEdge.Top;
					height = abd.rc.bottom;
					width = abd.rc.right;
					break;
				case (int)ABEdge.ABE_LEFT:
					taskBarEdge = TaskBarEdge.Left;
					height = abd.rc.right - abd.rc.left;
					width = abd.rc.bottom;
					break;
				case (int)ABEdge.ABE_RIGHT:
					taskBarEdge = TaskBarEdge.Right;
					height = abd.rc.right - abd.rc.left;
					width = abd.rc.bottom;
					break;
			}
		}
	}
}
