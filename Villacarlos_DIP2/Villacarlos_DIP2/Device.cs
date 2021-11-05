using System;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace WebCamLib
{
    public class Device
    {
        private const short WM_CAP = 0x400;
        private const int WM_CAP_DRIVER_CONNECT = 0x40a;
        private const int WM_CAP_DRIVER_DISCONNECT = 0x40b;
        private const int WM_CAP_EDIT_COPY = WM_CAP + 30;
        private const int WM_CAP_SET_PREVIEW = 0x432;
        private const int WM_CAP_SET_OVERLAY = 0x433;
        private const int WM_CAP_SET_PREVIEWRATE = 0x434;
        private const int WM_CAP_SET_SCALE = 0x435;
        private const int WS_CHILD = 0x40000000;
        private const int WS_VISIBLE = 0x10000000;
		private const int WM_CAP_SEQUENCE = WM_CAP + 62;
		private const int WM_CAP_FILE_SAVEAS = WM_CAP + 23;
		private const int SWP_NOMOVE = 0x20;
		private const int SWP_NOSIZE = 1;
		private const int SWP_NOZORDER = 0x40;
		private const int HWND_BOTTOM = 1;
        private const int WM_CAP_GRAB_FRAME = WM_CAP + 60;
        private const int WM_CAP_GRAB_FRAME_NOSTOP = WM_CAP + 61;
        private const int WM_CAP_SET_CALLBACK_FRAME = WM_CAP + 5;
        private const int WM_CAP_DLG_VIDEODISPLAY = WM_CAP + 42;
        private const int WM_CAP_SET_VIDEOFORMAT = WM_CAP + 45;
     

        [DllImport("user32", EntryPoint = "SendMessage")]
        static extern int SendBitmapMessage(int hWnd, uint wMsg, int wParam, ref BITMAPINFO lParam);

        [DllImport("user32", EntryPoint = "SendMessage")]
        static extern int SendHeaderMessage(int hWnd, uint wMsg, int wParam, CallBackDelegate lParam);

        //Declare Sub RtlMoveMemory Lib "kernel32" (ByVal hpvDest As Long, ByVal hpvSource As Long, ByVal cbCopy As Long)
		[DllImport("kernel32",EntryPoint="RtlMoveMemory")]
        public static extern void GetVideoData(long hpvDest,long hpvSource,long cbCopy); 
		
        
     	[DllImport("avicap32.dll")]
        protected static extern int capCreateCaptureWindowA([MarshalAs(UnmanagedType.VBByRefStr)] ref string lpszWindowName,
            int dwStyle, int x, int y, int nWidth, int nHeight, int hWndParent, int nID);

        [DllImport("user32", EntryPoint = "SendMessageA")]
        protected static extern int SendMessage(int hwnd, int wMsg, int wParam, [MarshalAs(UnmanagedType.AsAny)] object lParam);

        [DllImport("user32")]
        protected static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        [DllImport("user32")]
        protected static extern bool DestroyWindow(int hwnd);


        delegate void CallBackDelegate(IntPtr hwnd, ref VIDEOHEADER hdr);
        CallBackDelegate delegateFrameCallBack;


        [StructLayout(LayoutKind.Sequential)]
        public struct VIDEOHEADER
        {
            public IntPtr lpData;
            public uint dwBufferLength;
            public uint dwBytesUsed;
            public uint dwTimeCaptured;
            public uint dwUser;
            public uint dwFlags;
            [MarshalAs(System.Runtime.InteropServices.UnmanagedType.SafeArray)]
            byte[] dwReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            public BITMAPINFOHEADER bmiHeader;
            public int bmiColors;
        }

        int framewidth;
        int frameheight;
        int index;
        int deviceHandle;
        Bitmap bitframe;
		public Device()
		{
			//just a simple constructor
		}
        public Device(int index)
        {
            this.index = index;
        }
        public int getdevicehandle()
        {
            return this.deviceHandle;
        }
        private string _name;
		
		public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _version;

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public override string ToString()
        {
            return this.Name;
        }
        /// <summary>
        /// To Initialize the device
        /// </summary>
        /// <param name="windowHeight">Height of the Window</param>
        /// <param name="windowWidth">Width of the Window</param>
        /// <param name="handle">The Control Handle to attach the device</param>
        public void Init(int windowHeight, int windowWidth, int handle)
        {
            string deviceIndex = Convert.ToString(this.index);
            deviceHandle = capCreateCaptureWindowA(ref deviceIndex, WS_VISIBLE | WS_CHILD, 0, 0, windowWidth, windowHeight, handle, 0);

            this.frameheight = windowHeight;
            this.framewidth = windowWidth;
            delegateFrameCallBack = FrameCallBack;

            if (SendMessage(deviceHandle, WM_CAP_DRIVER_CONNECT, this.index, 0) > 0)
            {


                BITMAPINFO bInfo = new BITMAPINFO();
                bInfo.bmiHeader = new BITMAPINFOHEADER();
                bInfo.bmiHeader.biSize = (uint)Marshal.SizeOf(bInfo.bmiHeader);
                bInfo.bmiHeader.biWidth = windowWidth;
                bInfo.bmiHeader.biHeight =windowHeight;
                bInfo.bmiHeader.biPlanes = 1;
                bInfo.bmiHeader.biBitCount = 24; // bits per frame, 24 - RGB

                //Enable preview mode. In preview mode, frames are transferred from the 
                //capture hardware to system memory and then displayed in the capture 
                //window using GDI functions.
                SendBitmapMessage(deviceHandle, WM_CAP_SET_VIDEOFORMAT, Marshal.SizeOf(bInfo), ref bInfo);


                //orig settings
                SendMessage(deviceHandle, WM_CAP_SET_SCALE, -1, 0);
			    SendMessage(deviceHandle, WM_CAP_SET_PREVIEWRATE, 0x42, 0);
                SendMessage(deviceHandle, WM_CAP_SET_PREVIEW, -1, 0);

                SetWindowPos(deviceHandle, 1, 0, 0, windowWidth, windowHeight, 6);
            }
        }

        /// <summary>
        /// Shows the webcam preview in the control
        /// </summary>
        /// <param name="windowsControl">Control to attach the webcam preview</param>
        ///                    global::  
        public void ShowWindow(System.Windows.Forms.Control windowsControl)
        {
            Init(windowsControl.Height, windowsControl.Width, windowsControl.Handle.ToInt32());
        }
        public void ShowWindow(ref int h)
        {
                
                Init(640, 480, h);
            
        }

        public void MyFrameCallback()
        {
        
        }
        /// <summary>
        /// Stop the webcam and destroy the handle
        /// </summary>
        public void Stop()
        {
            SendMessage(deviceHandle, WM_CAP_DRIVER_DISCONNECT, this.index, 0);
            DestroyWindow(deviceHandle);
        }
		public void Sendmessage()
		{
			SendMessage(deviceHandle,WM_CAP_EDIT_COPY,0,0);
		}
        public void Sendmessage(int d)
        {
            SendMessage(d, WM_CAP_EDIT_COPY, 0, 0);
        }
        public void getframe()
        {
            SendMessage(deviceHandle, WM_CAP_GRAB_FRAME, 0, 0);
            SendHeaderMessage(deviceHandle, WM_CAP_SET_CALLBACK_FRAME, 0, delegateFrameCallBack);
        }

        private void  FrameCallBack(IntPtr hwnd, ref VIDEOHEADER hdr)
        {
                Bitmap bmp = new Bitmap(this.framewidth,this.frameheight, 3 * this.framewidth, System.Drawing.Imaging.PixelFormat.Format24bppRgb, hdr.lpData);
                bitframe = bmp;
        }
        private bool getbmap(ref Bitmap b)
        {
            if (bitframe != null)
            {
                b = bitframe;
                return true;
            }
            b = null;
            return false ;
        }
            

    }
}
