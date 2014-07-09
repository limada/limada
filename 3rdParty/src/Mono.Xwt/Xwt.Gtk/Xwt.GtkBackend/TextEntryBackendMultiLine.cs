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

        #region TODO

        public string PlaceholderText { get { return ""; } set { } }

        public bool ShowFrame { get { return false; } set { } }

        bool _multiLine = false;
        public bool MultiLine {
            get { return _multiLine; }
            set {
                if (value != _multiLine) {
                    _multiLine = value;
                    if (!value) {
                        TextView.WrapMode = Gtk.WrapMode.None;
                        // TODO: calculate this in resize-events:
                        TextView.PixelsAboveLines = TextView.PixelsAboveLines;
                    } else {
                        TextView.WrapMode = Gtk.WrapMode.Word;
                        TextView.PixelsAboveLines = 0;
                    }

                }
            }
        }

        // TODO: set the EventHandlers
        protected new ITextEntryEventSink EventSink {
            get { return (ITextEntryEventSink) base.EventSink; }
        }

        #endregion

        public override void Initialize () {
            Widget = new Gtk.TextView ();
            Widget.Show ();
        }

        protected virtual Gtk.TextView TextView {
            get { return (Gtk.TextView) base.Widget; }
        }

        protected new Gtk.TextView Widget {
            get { return TextView; }
            set { base.Widget = value; }
        }

        public string Text {
            get { return TextView.Buffer.Text; }
            set { TextView.Buffer.Text = value; }
        }

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
                TextView.CursorVisible = value;
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

    }

}