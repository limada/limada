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


using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.View;
using Xwt.Gdi.Backend;
using Limaki.Swf.Backends.Viewers.Content;

namespace Limaki.Swf.Backends.TextEditor {

    public partial class TextViewerWithToolstripBackend : ToolStripContainer, IZoomTarget, IVidgetBackend {
        
        public TextViewerWithToolstripBackend() {
            InitializeComponent();
        }

        void SetEditor(TextViewerBackend editor) {
            this.SuspendLayout ();
            this.ContentPanel.Controls.Clear();
            if (editor != null) {
                this.ContentPanel.Controls.Add (editor);
                editor.Dock = DockStyle.Fill;
                editor.Enter += ( sender, args) => {this.OnEnter (args);};
                editor.MouseUp += ( sender,args) => {this.OnMouseUp (args);};
                editor.GotFocus += ( sender,args) => {this.OnGotFocus (args);};
            }
            this.ResumeLayout ();
        }

        public TextViewerBackend TextViewerBackend {
            get {
                if (_textViewerBackend == null) {
                    _textViewerBackend = new TextViewerBackend();
                    SetEditor (_textViewerBackend);
                }
                return _textViewerBackend;
            }
            set {
                _textViewerBackend = value;
                SetEditor(_textViewerBackend);
            }
        }

        void SetToolStrip(TextBoxEditorToolStrip toolstrip) {
            this.SuspendLayout ();
            this.TopToolStripPanel.Controls.Clear ();
            if (toolstrip != null) {
                this.TopToolStripPanel.Controls.Add (toolstrip);
            }
            TextViewerBackend.ToolStrip = toolstrip;
            this.ResumeLayout ();
        }

        //bool _toolStripEnabled = false;
        //public bool ToolStripEnabled {
        //    get { return _textViewerBackend != null && _textViewerBackend.ToolStrip != null; }
        //    set {
        //        if (_textViewerBackend != null) {
        //            if (value) {
        //                TextBoxEditorToolStrip strip = _textBoxEditorToolStrip;
        //                if (strip == null)
        //                    strip = this.TextBoxEditorToolStrip;
        //                else
        //                    SetToolStrip(strip);
        //            } else {
        //                SetToolStrip(null);
        //            }
        //        }
        //    }
        //}
        
        public virtual TextBoxEditorToolStrip TextBoxEditorToolStrip {
            get {
                if (_textBoxEditorToolStrip ==null) {
                    _textBoxEditorToolStrip = new TextBoxEditorToolStrip ();
                    SetToolStrip (_textBoxEditorToolStrip);
                }
                
                return _textBoxEditorToolStrip;
            }
            set { _textBoxEditorToolStrip = value;
                  SetToolStrip(_textBoxEditorToolStrip);
            }
        }

        private TextViewerBackend _textViewerBackend=null;
        private TextBoxEditorToolStrip _textBoxEditorToolStrip=null;

        #region IZoomTarget Member

        public ZoomState ZoomState {
            get {
                if (_textViewerBackend != null) {
                    return _textViewerBackend.ZoomState;
                } else {
                    return ZoomState.Original;
                }
            }
            set {
                if (_textViewerBackend != null) {
                    _textViewerBackend.ZoomState = value;
                }
            }
        }

        public double ZoomFactor {
            get {
                if (_textViewerBackend != null) {
                    return _textViewerBackend.ZoomFactor;
                } else {
                    return 1d;
                }
            }
            set {
                if (_textViewerBackend != null) {
                    _textViewerBackend.ZoomFactor = value;
                }
            }
        }

        public void UpdateZoom() {
            if (_textViewerBackend != null) {
                _textViewerBackend.UpdateZoom ();
            }
        }

        #endregion

        #region IVidgetBackend-Implementation

        public TextViewerWithToolstrip Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (TextViewerWithToolstrip)frontend;
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
