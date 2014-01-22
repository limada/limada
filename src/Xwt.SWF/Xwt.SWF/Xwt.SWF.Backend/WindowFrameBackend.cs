// 
// WindowFrameBackend.cs
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

using Xwt.Backends;
using System;
using SWF=System.Windows.Forms;

namespace Xwt.WinformBackend {

    public class WindowFrameBackend : IWindowFrameBackend {

        SWF.Form form;
        IWindowFrameEventSink eventSink;
        WindowFrame frontend;

        public WindowFrameBackend() {
        }


        void IWindowFrameBackend.Initialize(IWindowFrameEventSink eventSink) {
            this.eventSink = eventSink;
            Initialize();
        }

        public virtual void Initialize() {
        }

        public virtual void Dispose(bool disposing) {
            Form.Close();
        }

        public SWF.Form Form {
            get { return form; }
            set { form = value; }
        }

        protected WindowFrame Frontend {
            get { return frontend; }
        }

        public IWindowFrameEventSink EventSink {
            get { return eventSink; }
        }

        bool IWindowFrameBackend.Decorated {
            get { return form.FormBorderStyle != SWF.FormBorderStyle.Sizable; }
            set { form.FormBorderStyle = value ? SWF.FormBorderStyle.Sizable : SWF.FormBorderStyle.None; }
        }

        bool IWindowFrameBackend.ShowInTaskbar {
            get { return form.ShowInTaskbar; }
            set { form.ShowInTaskbar = value; }
        }

        string IWindowFrameBackend.Title {
            get { return form.Text; }
            set { form.Text = value; }
        }

        bool IWindowFrameBackend.Visible {
            get { return form.Visible; }
            set { form.Visible = value; }
        }

        public Rectangle Bounds {
            get {
                return new Rectangle(form.Left, form.Top, form.Width, form.Height);
            }
            set {
                form.Top = (int)value.Top;
                form.Left = (int)value.Left;
                form.Width = (int)value.Width;
                form.Height = (int)value.Height;
                Toolkit.CurrentEngine.Invoke(delegate {
                                   eventSink.OnBoundsChanged(Bounds);
                               });
            }
        }

        public virtual void EnableEvent(object eventId) {
            if (eventId is WindowFrameEvent) {
                switch ((WindowFrameEvent)eventId) {
                    case WindowFrameEvent.BoundsChanged:
                        form.LocationChanged += BoundsChangedHandler;
                        form.SizeChanged += BoundsChangedHandler;
                        break;
                }
            }
        }

        public virtual void DisableEvent(object eventId) {
            if (eventId is WindowFrameEvent) {
                switch ((WindowFrameEvent)eventId) {
                    case WindowFrameEvent.BoundsChanged:
                        form.LocationChanged -= BoundsChangedHandler;
                        form.SizeChanged -= BoundsChangedHandler;
                        break;
                }
            }
        }

        void BoundsChangedHandler(object o, EventArgs args) {
            Toolkit.CurrentEngine.Invoke(delegate() {
                eventSink.OnBoundsChanged(Bounds);
            });
        }


        public void Dispose () {
            throw new NotImplementedException ();
        }


        public void Move(double x, double y) {
            throw new NotImplementedException();
        }

        public void Resize(double width, double height) {
            throw new NotImplementedException();
        }

        public void Present() {
            throw new NotImplementedException();
        }


        public void SetSize (double width, double height) {
            throw new NotImplementedException();
        }

        public void SetTransientFor (IWindowFrameBackend window) {
            throw new NotImplementedException();
        }

        public bool Resizable {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public double Opacity {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public void SetIcon (ImageDescription image) {
            throw new NotImplementedException();
        }

        public bool FullScreen {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public object Screen {
            get { throw new NotImplementedException(); }
        }

        public void InitializeBackend (object frontend, ApplicationContext context) {
            throw new NotImplementedException();
        }


        public bool Close () {
            throw new NotImplementedException();
        }
    }
}