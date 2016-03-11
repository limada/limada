// 
// WidgetBackend.cs
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
using Xwt.Backends;

namespace Xwt.SwfBackend {
    public class WidgetBackend : IWidgetBackend, ISwfWidgetBackend {
        #region IWidgetBackend Member

        public void Initialize(IWidgetEventSink eventSink) {
            throw new System.NotImplementedException();
        }

        public void Dispose(bool disposing) {
            throw new System.NotImplementedException();
        }

        public bool Visible {
            get {
                throw new System.NotImplementedException();
            }
            set {
                throw new System.NotImplementedException();
            }
        }

        public bool Sensitive {
            get {
                throw new System.NotImplementedException();
            }
            set {
                throw new System.NotImplementedException();
            }
        }

        public bool CanGetFocus {
            get {
                throw new System.NotImplementedException();
            }
            set {
                throw new System.NotImplementedException();
            }
        }

        public bool HasFocus {
            get { throw new System.NotImplementedException(); }
        }

        public Xwt.Size Size {
            get { throw new System.NotImplementedException(); }
        }

        public Xwt.Point ConvertToScreenCoordinates(Xwt.Point widgetCoordinates) {
            throw new System.NotImplementedException();
        }

        public void SetMinSize(double width, double height) {
            throw new System.NotImplementedException();
        }

        public void SetNaturalSize(double width, double height) {
            throw new System.NotImplementedException();
        }

        public void SetFocus() {
            throw new System.NotImplementedException();
        }

        public void UpdateLayout() {
            throw new System.NotImplementedException();
        }

        public object NativeWidget {
            get { throw new System.NotImplementedException(); }
        }

        public void DragStart(DragStartData data) {
            throw new System.NotImplementedException();
        }

        public void SetDragSource(Xwt.TransferDataType[] types, Xwt.DragDropAction dragAction) {
            throw new System.NotImplementedException();
        }

        public void SetDragTarget(Xwt.TransferDataType[] types, Xwt.DragDropAction dragAction) {
            throw new System.NotImplementedException();
        }

        public object Font {
            get {
                throw new System.NotImplementedException();
            }
            set {
                throw new System.NotImplementedException();
            }
        }

        public Xwt.Drawing.Color BackgroundColor {
            get {
                throw new System.NotImplementedException();
            }
            set {
                throw new System.NotImplementedException();
            }
        }

        public string TooltipText {
            get {
                throw new System.NotImplementedException();
            }
            set {
                throw new System.NotImplementedException();
            }
        }

        #endregion

        #region IBackend Member

        public void InitializeBackend(object frontend) {
            throw new System.NotImplementedException();
        }

        public void EnableEvent(object eventId) {
            throw new System.NotImplementedException();
        }

        public void DisableEvent(object eventId) {
            throw new System.NotImplementedException();
        }

        #endregion


        public void Dispose () {
            throw new System.NotImplementedException ();
        }

        public void SetCursor (CursorType cursorType) {
            throw new System.NotImplementedException ();
        }


        public double Opacity {
            get {
                throw new System.NotImplementedException();
            }
            set {
                throw new System.NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void SetSizeRequest (double width, double height) {
            throw new System.NotImplementedException();
        }

        public Size GetPreferredSize (SizeConstraint widthConstraint, SizeConstraint heightConstraint) {
            throw new System.NotImplementedException();
        }

        public void InitializeBackend (object frontend, ApplicationContext context) {
            throw new System.NotImplementedException();
        }
    }

    class WidgetBackendImpl : WidgetBackend {}
}