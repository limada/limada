//
// TextEntryBackendMultiLine.cs
//
// Author:
//       Lytico (http://www.limada.org)
//
// Copyright (c) 2014 http://www.limada.org
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
using Xwt.Drawing;

namespace Xwt.GtkBackend {

    /// <summary>
    /// MultiLine-TextEntryBackend
    /// uses Gtk.TextView instead of Gtk.Entry
    /// </summary>
    public class TextEntryBackendMultiLine : WidgetBackend, ITextEntryBackend {

        protected virtual Gtk.TextView TextView {
            get { return (Gtk.TextView) base.Widget; }
        }

        protected new Gtk.TextView Widget {
            get { return TextView; }
            set { base.Widget = value; }
        }

        public string Text {
            get { return TextView.Buffer.Text; }
            set {
                bufferSizeRequest = true;
                TextView.Buffer.Text = value;
            }
        }

        public bool ShowFrame { get; set; }

        protected virtual void RenderFrame (object o, Gtk.ExposeEventArgs args) {
            var w = TextView.GetWindow (Gtk.TextWindowType.Text);
            if (ShowFrame && args.Event.Window == w) {
                using (var gc = new Gdk.GC (w)) {
                    int wh, ww;
                    w.GetSize (out ww, out wh);

                    w.DrawLines (gc, new Gdk.Point[] {
                        new Gdk.Point (0, 0),
                        new Gdk.Point (--ww, 0),
                        new Gdk.Point (ww, --wh),
                        new Gdk.Point (0, wh),
                        new Gdk.Point (0, 0),
                    });
                }
            }
        }
    
        private string _placeholderText;
        public string PlaceholderText {
            get { return _placeholderText; }
            set {
                if (_placeholderText != value) {
                    if (_placeholderText == null)
                        Widget.ExposeEvent += RenderPlaceholderText;
                    else if (value == null)
                        Widget.ExposeEvent -= RenderPlaceholderText;
                }
                _placeholderText = value;
            }
        }

        Pango.Layout _layout = null;
        protected Pango.Layout Layout {
            get { return _layout ?? (_layout = new Pango.Layout (TextView.PangoContext)); }
        }

        protected virtual void RenderPlaceholderText (object o, Gtk.ExposeEventArgs args) {
            var w = TextView.GetWindow (Gtk.TextWindowType.Text);
            if (!string.IsNullOrEmpty (PlaceholderText) && string.IsNullOrEmpty(Text) && args.Event.Window == w) {
                Util.RenderPlaceholderText (TextView, args, PlaceholderText, ref _layout);
            }
        }

        public override object Font {
            get { return base.Font; }
            set {
                base.Font = value;
                _xLayout = null;
                _layout = null;
            }
        }

        public override void Initialize () {
            Widget = new Gtk.TextView ();
            Widget.Show ();

            ShowFrame = true;
            TextView.ExposeEvent += RenderFrame;
            TextView.Indent = 2;

            InitializeMultiLine ();
            
        }
        
        #region Multiline-Handling

        bool _multiLine = false;
        public bool MultiLine {
            get { return _multiLine; }
            set {
                if (value != _multiLine) {
                    _multiLine = value;
                    if (!value) {
                        TextView.WrapMode = Gtk.WrapMode.None;

                    } else {
                        TextView.WrapMode = Gtk.WrapMode.Word;
                        TextView.PixelsAboveLines = 2;
                    }

                }
            }
        }

        bool bufferSizeRequest = false;
        int lineHeight = -1;

        Pango.Layout _xLayout = null;
        protected Pango.Layout XLayout {
            get { return _xLayout ?? (_xLayout = TextView.CreatePangoLayout ("X")); }
        }

        public virtual void InitializeMultiLine () {
            var w = 0;
            var lastHeight = -1;

            TextView.SizeRequested += (s, args) => {
                if (!MultiLine)
                    args.Requisition = new Gtk.Requisition {
                        Width = -1,
                        Height = lineHeight
                    };
            };

            TextView.SizeAllocated += (s, e) => {
                if (!MultiLine && lastHeight != e.Allocation.Height) {
                    lastHeight = e.Allocation.Height;
                    XLayout.GetPixelSize (out w, out lineHeight);
                    TextView.PixelsAboveLines = (int) ((lastHeight - lineHeight) / 2d) - 1;
                }
            };

            TextView.Buffer.Changed += (s, e) => {
                bufferSizeRequest = true;
            };
        }

        public override Size GetPreferredSize (SizeConstraint widthConstraint, SizeConstraint heightConstraint) {
            if (MultiLine)
                return base.GetPreferredSize (widthConstraint, heightConstraint);

            if (bufferSizeRequest) {
                bufferSizeRequest = !widthConstraint.IsConstrained;
                return new Size (Widget.Allocation.Width, Widget.Allocation.Height);
            }

            return base.GetPreferredSize (widthConstraint, heightConstraint);
        }

        #endregion

        public static Alignment ToXwt (Gtk.Justification value) {
            if (value == Gtk.Justification.Center)
                return Alignment.Center;
            if (value == Gtk.Justification.Left)
                return Alignment.Start;
            //if (value == Gtk.Justification.Right)
            return Alignment.End;
        }

        public static Gtk.Justification ToGtk (Alignment value) {
            if (value == Alignment.Center)
                return Gtk.Justification.Center;
            if (value == Alignment.Start)
                return Gtk.Justification.Left;
            // if (value == Alignment.End)
            return Gtk.Justification.Right;
        }

        public Alignment TextAlignment {
            get { return ToXwt (TextView.Justification); }
            set { TextView.Justification = ToGtk (value); }
        }

        public bool ReadOnly {
            get { return TextView.Editable; }
            set {
                TextView.Editable = value;
                TextView.CursorVisible = !value;
            }
        }

        public int CursorPosition {
            get {
                var iter = TextView.Buffer.GetIterAtMark (TextView.Buffer.InsertMark);
                return iter.Offset;
            }
            set {
                var iter = TextView.Buffer.GetIterAtOffset (value);
                TextView.Buffer.PlaceCursor (iter);
            }
        }

        public int SelectionStart {
            get {
                if (TextView.Buffer.HasSelection) {
                    var iter = TextView.Buffer.GetIterAtMark (TextView.Buffer.SelectionBound);
                    return iter.Offset;
                }
                return -1;

            }
            set {
                throw new NotImplementedException ();
            }
        }

        public int SelectionLength {
            get {
                if (TextView.Buffer.HasSelection) {
                    var iter = TextView.Buffer.GetIterAtMark (TextView.Buffer.SelectionBound);
                    return iter.Char.Length;
                }
                return -1;
            }
            set {
                throw new NotImplementedException ();
            }
        }

        public string SelectedText {
            get {
                if (TextView.Buffer.HasSelection) {
                    var iter = TextView.Buffer.GetIterAtMark (TextView.Buffer.SelectionBound);
                    return iter.Char;
                }
                return "";
            }
            set {
                throw new NotImplementedException ();
            }
        }

        #region Eventhandling

        protected new ITextEntryEventSink EventSink {
            get { return (ITextEntryEventSink) base.EventSink; }
        }

        public override void EnableEvent (object eventId) {
            base.EnableEvent (eventId);
            if (eventId is TextEntryEvent) {
                switch ((TextEntryEvent) eventId) {
                    case TextEntryEvent.Changed: TextView.Buffer.Changed += HandleChanged; break;
                    case TextEntryEvent.Activated: Widget.KeyPressEvent += HandleActivated; break;
                    case TextEntryEvent.SelectionChanged:
                        enableSelectionChangedEvent = true;
                        Widget.MoveCursor += HandleMoveCursor;
                        Widget.ButtonPressEvent += HandleButtonPressEvent;
                        Widget.ButtonReleaseEvent += HandleButtonReleaseEvent;
                        Widget.MotionNotifyEvent += HandleMotionNotifyEvent;
                        break;
                }
            }
        }

        public override void DisableEvent (object eventId) {
            base.DisableEvent (eventId);
            if (eventId is TextEntryEvent) {
                switch ((TextEntryEvent) eventId) {
                    case TextEntryEvent.Changed: TextView.Buffer.Changed -= HandleChanged; break;
                    case TextEntryEvent.Activated: Widget.KeyPressEvent -= HandleActivated; break;
                    case TextEntryEvent.SelectionChanged:
                        enableSelectionChangedEvent = false;
                        Widget.MoveCursor -= HandleMoveCursor;
                        Widget.ButtonPressEvent -= HandleButtonPressEvent;
                        Widget.ButtonReleaseEvent -= HandleButtonReleaseEvent;
                        Widget.MotionNotifyEvent -= HandleMotionNotifyEvent;
                        break;
                }
            }
        }

        void HandleChanged (object sender, EventArgs e) {
            ApplicationContext.InvokeUserCode (delegate {
                EventSink.OnChanged ();
                EventSink.OnSelectionChanged ();
            });
        }

        void HandleActivated (object sender, Gtk.KeyPressEventArgs e) {
            if (e.Event.Key == Gdk.Key.Return || e.Event.Key == Gdk.Key.ISO_Enter)
                ApplicationContext.InvokeUserCode (delegate {
                    EventSink.OnActivated ();
                });
        }

        bool enableSelectionChangedEvent;
        void HandleSelectionChanged () {
            if (enableSelectionChangedEvent)
                ApplicationContext.InvokeUserCode (delegate {
                    EventSink.OnSelectionChanged ();
                });
        }

        void HandleMoveCursor (object sender, EventArgs e) {
            HandleSelectionChanged ();
        }

        int cacheSelectionStart, cacheSelectionLength;
        bool isMouseSelection;

        [GLib.ConnectBefore]
        void HandleButtonPressEvent (object o, Gtk.ButtonPressEventArgs args) {
            if (args.Event.Button == 1) {
                HandleSelectionChanged ();
                cacheSelectionStart = SelectionStart;
                cacheSelectionLength = SelectionLength;
                isMouseSelection = true;
            }
        }

        [GLib.ConnectBefore]
        void HandleMotionNotifyEvent (object o, Gtk.MotionNotifyEventArgs args) {
            if (isMouseSelection)
                if (cacheSelectionStart != SelectionStart || cacheSelectionLength != SelectionLength)
                    HandleSelectionChanged ();
            cacheSelectionStart = SelectionStart;
            cacheSelectionLength = SelectionLength;
        }

        [GLib.ConnectBefore]
        void HandleButtonReleaseEvent (object o, Gtk.ButtonReleaseEventArgs args) {
            if (args.Event.Button == 1) {
                isMouseSelection = false;
                HandleSelectionChanged ();
            }
        }

        #endregion
        
        protected override void Dispose (bool disposing) {
            if (disposing) {
                if (_xLayout != null)
                    _xLayout.Dispose ();
                _xLayout = null;

                if (_layout != null)
                    _layout.Dispose ();
                _layout = null;
            }
            base.Dispose (disposing);
        }
    }

}