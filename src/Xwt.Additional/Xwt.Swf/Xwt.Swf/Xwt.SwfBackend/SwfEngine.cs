// 
// Colors.cs
//  
// Author:
//       Lytico 
// 
// Copyright (c) 2012 Lytico (www.limada.org)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.


using System;
using System.Collections.Generic;
using System.Timers;
using SWF = System.Windows.Forms;
using Xwt.GdiBackend;
using Xwt.Backends;
using Xwt.Swf.Xwt.SwfBackend;
using System.Diagnostics;
using ST = System.Threading;

namespace Xwt.SwfBackend {

    public class SwfEngine : GdiEngine {

        public override void InitializeApplication () {

            SWF.Application.EnableVisualStyles ();
            SWF.Application.SetCompatibleTextRenderingDefault (false);

        }

        public override void InitializeBackends () {
            base.InitializeBackends ();
            RegisterBackend<DesktopBackend, SwfDesktopBackend> ();
            RegisterBackend<ClipboardBackend, SwfClipboardBackend> ();
            RegisterBackend<IMenuItemBackend, MenuItemBackend> ();
            RegisterBackend<IMenuBackend, MenuBackend> ();
        }

        public static SWF.ApplicationContext SwfApplicationContext {get;set;}

        public override void RunApplication () {
            if (SwfApplicationContext == null)
                SwfApplicationContext = new SWF.ApplicationContext();
            SWF.Application.Run (SwfApplicationContext);
        }

        public override void DispatchPendingEvents () {
            SWF.Application.DoEvents();
        }

        public override void InvokeAsync (Action action) {
            
            if (SwfApplicationContext.MainForm != null)
                SwfApplicationContext.MainForm.BeginInvoke (action);
            else
                action.BeginInvoke((ar) => { }, null);

        }

        public override object TimerInvoke (Func<bool> action, TimeSpan timeSpan) {
            
            var contextId = ST.Thread.CurrentContext.ContextID;
            Trace.WriteLine ($"{contextId}");
            SwfApplicationContext.MainForm.Invoke ((SWF.MethodInvoker)delegate { contextId = ST.Thread.CurrentContext.ContextID; Trace.WriteLine ($"{contextId} on mainform"); });
            var name = ST.Thread.CurrentThread.Name;
            Trace.WriteLine ($"{name}");
            SwfApplicationContext.MainForm.Invoke ((SWF.MethodInvoker)delegate { name = ST.Thread.CurrentThread.Name; Trace.WriteLine ($"{name} on mainform"); });


            //TODO:
            return action ();
        }

        public override void CancelTimerInvoke (object id) {
            //TODO:
            // throw new NotImplementedException ();
        }

        public override bool HasNativeParent (Widget w) {
            throw new NotImplementedException ();
        }

        public override object GetNativeWidget(Widget w) {
            var backend = w.GetBackend() as ISwfWidgetBackend;
            return backend.Control;
        }

        public override Backends.IWindowFrameBackend GetBackendForWindow(object nativeWindow) {
            throw new System.NotImplementedException();
        }

        public override void ExitApplication () {
            SWF.Application.Exit();
        }
    }

    public interface IWinformWidgetBackend {
        SWF.Control Control { get; }
    }
    
}