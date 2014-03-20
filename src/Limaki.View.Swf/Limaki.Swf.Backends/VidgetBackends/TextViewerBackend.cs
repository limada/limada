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


using Limaki.Drawing;
using Limaki.Swf.Backends.Viewers.Content;
using Limaki.View;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Limaki.View.Vidgets;
using Xwt.Gdi.Backend;
using Limaki.View.Swf;
using Limaki.Common;
using System.Text;
using Limaki.Common.Text;

// this control uses ideas from RicherTextBox by ???

namespace Limaki.Swf.Backends.TextEditor {

    public partial class TextViewerBackend : UserControl, IZoomTarget, ITextViewerBackend, IDragDropControl {

        public TextViewerBackend () {
            InitializeComponent();
            innerTextBox.Enter += (sender, args) => { this.OnEnter(args); };
            innerTextBox.MouseUp += (sender, args) => { this.OnMouseUp(args); };
            innerTextBox.GotFocus += (sender, args) => { this.OnGotFocus(args); };

        }

        TextBoxEditorControler _controller = null;
        [Browsable(false)]
        public TextBoxEditorControler Controller {
            get {
                if (_controller == null) {
                    _controller = new TextBoxEditorControler(this.innerTextBox);
                }
                return _controller;
            }
        }

        [Browsable(true)]
        TextBoxEditorToolStrip _toolStrip = null;
        public TextBoxEditorToolStrip ToolStrip {
            get { return _toolStrip; }
            set {
                if (value != null) {
                    value.Controller = this.Controller;
                } else {
                    if (_toolStrip != null) {
                        _toolStrip.Controller = null;
                    }
                }
                _toolStrip = value;
            }
        }

        [Browsable(true)]
        public virtual bool Multiline {
            get { return this.innerTextBox.Multiline; }
            set { this.innerTextBox.Multiline = value; }
        }

        [Browsable(true)]
        public virtual bool EnableAutoDragDrop {
            get { return this.innerTextBox.EnableAutoDragDrop; }
            set { this.innerTextBox.EnableAutoDragDrop = value; }
        }

        [Browsable(true)]
        public virtual bool ReadOnly {
            get { return this.innerTextBox.ReadOnly; }
            set { this.innerTextBox.ReadOnly = value; }
        }

        [Browsable(false)]
        public virtual bool Modified {
            get { return this.innerTextBox.Modified; }
            set { this.innerTextBox.Modified = value; }
        }

        public void Clear () {
            this.innerTextBox.Clear();
        }

        protected virtual void AfterLoadRTF () {
            if (this._toolStrip != null) {
                _toolStrip.Reset();
            }
        }

        public string Rtf {
            get { return this.innerTextBox.Rtf; }
            set {
                this.innerTextBox.Rtf = value;
                AfterLoadRTF();
            }
        }

        public void Load (Stream stream, TextViewerRtfType streamType) {
            //innerTextBox.Clear ();
            var color = innerTextBox.BackColor;
            if (streamType == TextViewerRtfType.RichText)
                innerTextBox.LoadFile (stream, RichTextBoxStreamType.RichText);
            else if (streamType == TextViewerRtfType.UnicodePlainText) {
                if (TextHelper.IsUnicode (stream.GetBuffer (30)))
                    using (var reader = new StreamReader (stream, Encoding.Unicode))
                        innerTextBox.Text = reader.ReadToEnd ();
                else
                    innerTextBox.LoadFile (stream, RichTextBoxStreamType.PlainText);
            }
            innerTextBox.BackColor = color;
            AfterLoadRTF();
        }

        public void Save (Stream stream, TextViewerRtfType streamType) {
            innerTextBox.SaveFile(stream, (RichTextBoxStreamType)streamType);
        }

        #region IZoomTarget Member

        public ZoomState ZoomState {
            get { return ZoomState.Original; }
            set {
                if (value == ZoomState.Original) {
                    this.innerTextBox.ZoomFactor = 1.0f;
                }
            }
        }

        /// <summary>
        /// remark: works only with truetype-fonts, else next integer
        /// </summary>
        [Browsable(true)]
        public virtual double ZoomFactor {
            get { return this.innerTextBox.ZoomFactor; }
            set { this.innerTextBox.ZoomFactor = (float)value; }
        }

        public void UpdateZoom () {
            //base.Window.TextZoom = ZoomFactor;
        }

        #endregion

        #region IVidgetBackend-Implementation

        public TextViewer Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (TextViewer)frontend;
        }

        Xwt.Size IVidgetBackend.Size {
            get { return this.Size.ToXwt(); }
        }


        void IVidgetBackend.Invalidate (Xwt.Rectangle rect) {
            this.Invalidate(rect.ToGdi());
        }

        Xwt.Point IDragDropControl.PointToClient (Xwt.Point source) { return PointToClient(source.ToGdi()).ToXwt(); }

        void IVidgetBackend.SetFocus () { this.Focus (); }

        #endregion

        VidgetBorderStyle ITextViewerBackend.BorderStyle {
            get { return (VidgetBorderStyle)this.BorderStyle; }
            set { this.BorderStyle = (BorderStyle)value; }
        }

        Xwt.Point ITextViewerBackend.AutoScrollOffset {
            get { return this.AutoScrollOffset.ToXwt(); }
            set { this.AutoScrollOffset = value.ToGdi(); }
        }

    }
}

