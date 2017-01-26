// 
// WidgetBackend.cs
//  
// Author:
//       Lytico 
// 
// Copyright (c) 2012-2017 Lytico (www.limada.org)
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
using Xwt.GdiBackend;
using SWF=System.Windows.Forms;
using SD=System.Drawing;

namespace Xwt.SwfBackend {

    public class WidgetBackend<T> : IWidgetBackend, ISwfWidgetBackend where T:SWF.Control {


        #region IWidgetBackend Member

        public T Control { get; protected set; }

        SWF.Control ISwfWidgetBackend.Control { get { return Control; } }

        public virtual void Initialize(IWidgetEventSink eventSink) {
        }

        public virtual void Dispose (bool disposing) {
            if (!Control.IsDisposed && !Control.Disposing) {
                Control.Dispose ();
            }
        }

        public virtual bool Visible {
            get { return Control.Visible; }
            set { Control.Visible = value; }
        }

        public virtual bool Sensitive {
            get { return Control.Capture; }
            set { Control.Capture = value; }
        }

        public virtual bool CanGetFocus {
            get { return Control.CanFocus; }
            set { /*Control.CanFocus = value;*/ }
        }

        public virtual bool HasFocus {
            get { return Control.Focused; }
        }

        public virtual Xwt.Size Size {
            get { return Control.Size.ToXwt (); }
        }

        public virtual Xwt.Point ConvertToScreenCoordinates(Xwt.Point widgetCoordinates) {
            throw new System.NotImplementedException();
        }

        public virtual void SetMinSize (double width, double height) {
        }

        public virtual void SetNaturalSize (double width, double height) {
        }

        public virtual void SetFocus () {
            Control.Focus ();
        }

        public virtual void UpdateLayout () {
            Control.Update ();
        }

        public virtual object NativeWidget {
            get { return Control; }
        }

        public virtual void DragStart (DragStartData data) {
        }

        public virtual void SetDragSource (Xwt.TransferDataType[] types, Xwt.DragDropAction dragAction) {
        }

        public virtual void SetDragTarget(Xwt.TransferDataType[] types, Xwt.DragDropAction dragAction) {
        }

        public virtual object Font {
            get { return Control.Font; }
            set { Control.Font = (SD.Font) value; }
        }

        public virtual  Xwt.Drawing.Color BackgroundColor {
            get { return Control.BackColor.ToXwt (); }
            set { Control.BackColor = value.ToGdi (); }
        }

        public virtual string TooltipText {
            get { return string.Empty; }
            set { }
        }

        #endregion

        #region IBackend Member

        public virtual void InitializeBackend (object frontend) {
        }

        public virtual void EnableEvent (object eventId) {

        }

        public virtual void DisableEvent (object eventId) {

        }

        #endregion


        public virtual void Dispose () {
            if(Control==null)
                return;
            Dispose(Control.Disposing);
        }

        public virtual void SetCursor (CursorType cursorType) {
            
        }
        
        public virtual double Opacity {
            get { return 1; }
            set { }
        }

        public virtual string Name {
            get { return Control.Name; }
            set { Control.Name = value; }
        }

        public virtual void SetSizeRequest (double width, double height) {
            Control.Size = new SD.Size ((int) width, (int) height);
        }

        public virtual Size GetPreferredSize (SizeConstraint widthConstraint, SizeConstraint heightConstraint) {
            return Size;
        }

        public virtual void InitializeBackend (object frontend, ApplicationContext context) {
        }
    }

}