// 
// GdiEngine.cs
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

using System;
using Xwt.Backends;

namespace Xwt.Gdi.Backend {

    public class GdiEngine : Xwt.Backends.ToolkitEngineBackend {
        public override void RunApplication() {
            
        }

        public override void InitializeApplication() {
            base.InitializeApplication();
        }

        public override void InitializeBackends () {

            RegisterBackend<FontBackendHandler, GdiFontBackendHandler>();
            RegisterBackend<TextLayoutBackendHandler, GdiTextLayoutBackendHandler>();
            RegisterBackend<ContextBackendHandler, GdiContextBackendHandler>();
            RegisterBackend<ImageBackendHandler, GdiImageBackendHandler>();
            RegisterBackend<ImageBuilderBackendHandler,GdiImageBuilderBackend>();
            RegisterBackend<ImagePatternBackendHandler, GdiImagePatternBackendHandler>();
            
        }

        public override object GetNativeWidget(Widget w) {
            throw new NotImplementedException();
        }

        public override IWindowFrameBackend GetBackendForWindow(object nativeWindow) {
            throw new NotImplementedException();
        }

        public override void ExitApplication() {
            throw new NotImplementedException();
        }

        public override void InvokeAsync(Action action) {
            throw new NotImplementedException();
        }

        public override object TimerInvoke(Func<bool> action, TimeSpan timeSpan) {
            throw new NotImplementedException();
        }

        public override void CancelTimerInvoke(object id) {
            throw new NotImplementedException();
        }

        public override void DispatchPendingEvents () {
            throw new NotImplementedException();
        }

        public override bool HasNativeParent (Widget w) {
            throw new NotImplementedException();
        }
    }
}