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


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using Limaki.Common.Text.RTF;
using System.IO;
using System.Text;
using Limaki.Drawing;
using Limaki.View;
using Xwt.Gdi.Backend;
using Limaki.Swf.Backends.Viewers.Content;

// this control uses ideas from RicherTextBox by ???

namespace Limaki.Swf.Backends.TextEditor {

    public partial class TextViewerBackend : UserControl, IZoomTarget, IVidgetBackend {
        public TextViewerBackend() {
            InitializeComponent();
            innerTextBox.Enter += (sender, args) => {this.OnEnter (args); };
            innerTextBox.MouseUp += (sender, args) => { this.OnMouseUp(args); };
            innerTextBox.GotFocus += (sender, args) => { this.OnGotFocus(args); };
            
        }

        TextBoxEditorControler _controller = null;
        [Browsable(false)]
        public TextBoxEditorControler Controller {
            get {
                if (_controller == null) {
                    _controller = new TextBoxEditorControler (this.innerTextBox);
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

        public void Clear() {
            this.innerTextBox.Clear ();
        }

        protected virtual void AfterLoadRTF() {
            if (this._toolStrip!=null) {
                _toolStrip.Reset ();
            }
        }

        public string Rtf {
            get { return this.innerTextBox.Rtf; }
            set { this.innerTextBox.Rtf = value;
                AfterLoadRTF ();  
            }
        }

        public void Load(Stream stream, RichTextBoxStreamType streamType) {
            //innerTextBox.Clear ();
            var color = innerTextBox.BackColor;
            innerTextBox.LoadFile (stream, streamType);
            innerTextBox.BackColor = color;
            AfterLoadRTF ();
        }

        public void Save(Stream stream, RichTextBoxStreamType streamType) {
            innerTextBox.SaveFile(stream, streamType);
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

        public void UpdateZoom() {
            //base.Window.TextZoom = ZoomFactor;
        }

        #endregion

        #region IVidgetBackend-Implementation

        public TextViewer Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (TextViewer)frontend;
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

