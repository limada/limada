/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 */


using System.Windows.Forms;
using System.Threading;
using System;
using System.Runtime.InteropServices;
using System.Net;
using Limaki.Drawing;
using Limaki.View;
using Limaki.Viewers;
using Xwt.Gdi.Backend;
using Limaki.Viewers.StreamViewers;
using Limaki.Viewers.Vidgets;

namespace Limaki.Swf.Backends {

    public class WebBrowserBackend : System.Windows.Forms.WebBrowser, IWebBrowserBackend, IHistoryAware, IZoomTarget {

        public void Navigatewithproxy(string uri, string host, int port) {
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Proxy = new WebProxy(host, port);

            var response = (HttpWebResponse)webRequest.GetResponse();
            var receiveStream = response.GetResponseStream();
            
            DocumentStream = receiveStream;
        }

        public void MakeReady() {
            if (base.Document == null) {
                Action navigate = () => base.Navigate("about:blank");
                if (base.InvokeRequired)
                    base.Invoke(navigate);
                else
                    navigate();

                if (!OS.Mono)
                    for (int i = 0; i < 200 && base.IsBusy; i++) {
                        Application.DoEvents();
                        Thread.Sleep(5);
                    }
            }
        }

        public void AfterNavigate (Func<bool> done) {
            if (!OS.Mono) {
                // try to resolve timing problems 
                // does not work so well, but better than nothing
                int i = 0;
                while (!done() && i < 10) {
                    Thread.Sleep(5);
                    i++;
                }
            }
            // fails with IExplorer, not necessry with gecko:
            // control.Refresh();
            Application.DoEvents();
        }

        #region IZoomTarget Member

        public ZoomState ZoomState {
            get { return ZoomState.Original; }
            set {
                if (value == ZoomState.Original) {
                    ZoomFactor = 1.0f;
                }
            }
        }

        private double _zoomFactor = 1f;
        public double ZoomFactor {
            get { return _zoomFactor; }
            set { _zoomFactor = value;
                UpdateZoom();
            }
        }

        public void UpdateZoom() {

            SetZoom((int)(_zoomFactor * 100));
        }

        #endregion

        #region enums
        public enum OLECMDID
        {
            // ...
            OLECMDID_OPTICAL_ZOOM = 63,
            OLECMDID_OPTICAL_GETZOOMRANGE = 64,
            // ...
        }

        public enum OLECMDEXECOPT
        {
            // ...
            OLECMDEXECOPT_DONTPROMPTUSER,
            // ...
        }

        public enum OLECMDF
        {
            // ...
            OLECMDF_SUPPORTED = 1
        }
        #endregion

        #region IWebBrowser2
        [ComImport, /*SuppressUnmanagedCodeSecurity,*/
         TypeLibType(TypeLibTypeFlags.FOleAutomation | 
                     TypeLibTypeFlags.FDual |       
                     TypeLibTypeFlags.FHidden), 
        Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E")]
        public interface IWebBrowser2
        {
            [DispId(100)]  void GoBack();
            [DispId(0x65)] void GoForward();
            [DispId(0x66)] void GoHome();
            [DispId(0x67)] void GoSearch();
            [DispId(0x68)] void Navigate([In] string Url, 
                                         [In] ref object flags, 
                                         [In] ref object targetFrameName, 
                                         [In] ref object postData, 
                                         [In] ref object headers);
            [DispId(-550)] void Refresh();
            [DispId(0x69)] void Refresh2([In] ref object level);
            [DispId(0x6a)] void Stop();
            [DispId(200)]  object Application 
                           { [return: 
                              MarshalAs(UnmanagedType.IDispatch)] get; }
            [DispId(0xc9)] object Parent 
                           { [return: 
                              MarshalAs(UnmanagedType.IDispatch)] get; }
            [DispId(0xca)] object Container 
                           { [return: 
                              MarshalAs(UnmanagedType.IDispatch)] get; }
            [DispId(0xcb)] object Document 
                           { [return: 
                              MarshalAs(UnmanagedType.IDispatch)] get; }
            [DispId(0xcc)] bool TopLevelContainer { get; }
            [DispId(0xcd)] string Type { get; }
            [DispId(0xce)] int Left { get; set; }
            [DispId(0xcf)] int Top { get; set; }
            [DispId(0xd0)] int Width { get; set; }
            [DispId(0xd1)] int Height { get; set; }
            [DispId(210)]  string LocationName { get; }
            [DispId(0xd3)] string LocationURL { get; }
            [DispId(0xd4)] bool Busy { get; }
            [DispId(300)]  void Quit();
            [DispId(0x12d)] void ClientToWindow(out int pcx, out int pcy);
            [DispId(0x12e)] void PutProperty([In] string property, 
                                             [In] object vtValue);
            [DispId(0x12f)] object GetProperty([In] string property);
            [DispId(0)] string Name { get; }
            [DispId(-515)] int HWND { get; }
            [DispId(400)] string FullName { get; }
            [DispId(0x191)] string Path { get; }
            [DispId(0x192)] bool Visible { get; set; }
            [DispId(0x193)] bool StatusBar { get; set; }
            [DispId(0x194)] string StatusText { get; set; }
            [DispId(0x195)] int ToolBar { get; set; }
            [DispId(0x196)] bool MenuBar { get; set; }
            [DispId(0x197)] bool FullScreen { get; set; }
            [DispId(500)] void Navigate2([In] ref object URL, 
                                         [In] ref object flags, 
                                         [In] ref object targetFrameName, 
                                         [In] ref object postData, 
                                         [In] ref object headers);
            [DispId(0x1f5)] OLECMDF QueryStatusWB([In] OLECMDID cmdID);
            [DispId(0x1f6)] void ExecWB([In] OLECMDID cmdID, 
                                        [In] OLECMDEXECOPT cmdexecopt, 
                                        ref object pvaIn, IntPtr pvaOut);
            [DispId(0x1f7)] void ShowBrowserBar([In] ref object pvaClsid, 
                                                [In] ref object pvarShow, 
                                                [In] ref object pvarSize);
            [DispId(-525)] WebBrowserReadyState ReadyState { get; }
            [DispId(550)] bool Offline { get; set; }
            [DispId(0x227)] bool Silent { get; set; }
            [DispId(0x228)] bool RegisterAsBrowser { get; set; }
            [DispId(0x229)] bool RegisterAsDropTarget { get; set; }
            [DispId(0x22a)] bool TheaterMode { get; set; }
            [DispId(0x22b)] bool AddressBar { get; set; }
            [DispId(0x22c)] bool Resizable { get; set; }
        }
        #endregion

        private IWebBrowser2 axIWebBrowser2;

        protected override void AttachInterfaces(object nativeActiveXObject) {
            base.AttachInterfaces(nativeActiveXObject);
            if (!OS.Mono) {
                this.axIWebBrowser2 = (IWebBrowser2) nativeActiveXObject;
            }
        }

        protected override void DetachInterfaces() {
            base.DetachInterfaces();
            if (!OS.Mono) {
                this.axIWebBrowser2 = null;
            }
        }

        public void SetZoom(int factor) {
            if (!OS.Mono) {
                object pvaIn = factor;
                try {
                    this.axIWebBrowser2.ExecWB(OLECMDID.OLECMDID_OPTICAL_ZOOM,
                                               OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER,
                                               ref pvaIn, IntPtr.Zero);
                } catch (Exception) {
                    throw;
                }
            }
        }

        #region IVidgetBackend-Implementation

        public WebBrowserVidget Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (WebBrowserVidget)frontend;
        }

        Xwt.Rectangle IVidgetBackend.ClientRectangle {
            get { return this.ClientRectangle.ToXwt(); }
        }

        Xwt.Size IVidgetBackend.Size {
            get { return this.Size.ToXwt(); }
        }

   
        void IVidgetBackend.Invalidate (Xwt.Rectangle rect) {
            this.Invalidate(rect.ToGdi());
        }

        Xwt.Point IVidgetBackend.PointToClient (Xwt.Point source) { return PointToClient(source.ToGdi()).ToXwt(); }

        #endregion

    }
}
