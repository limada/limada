// 
// Colors.cs
//  
// Author:
//       Lytico 
// 
// Copyright (c) 2012 Lytico (http://limada.sourceforge.net)
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

using Xwt.Engine;
using SWF = System.Windows.Forms;
using Xwt.Gdi.Backend;

namespace Xwt.WinformBackend {

    public class SwfEngine : GdiEngine {
        public override void InitializeApplication() {

            SWF.Application.EnableVisualStyles();
            SWF.Application.SetCompatibleTextRenderingDefault(false);
            RegisterBackends();
            
        }

        public override void RegisterBackends() {
            base.RegisterBackends();
        }

        public override void RunApplication() {
            SWF.Application.Run();
        }

        //public override void Invoke(System.Action action) {
           
        //    SWF.Form.ActiveForm.Invoke(action);
        //}

        //public override object TimeoutInvoke(System.Func<bool> action, System.TimeSpan timeSpan) {
        //    throw new System.NotImplementedException();
        //}

        //public override void CancelTimeoutInvoke(object id) {
        //    throw new System.NotImplementedException();
        //}

        public override object GetNativeWidget(Widget w) {
            var backend = (IWinformWidgetBackend)WidgetRegistry.GetBackend(w);
            return backend.Control;
        }

        public override Backends.IWindowFrameBackend GetBackendForWindow(object nativeWindow) {
            throw new System.NotImplementedException();
        }
    }

    public interface IWinformWidgetBackend {
        SWF.Control Control { get; }
    }
}