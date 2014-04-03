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
using Limaki.View.Vidgets;
using Xwt.GdiBackend;

namespace Limaki.SwfBackend.VidgetBackends {

    public partial class TextViewerWithToolstripBackend : ToolStripContainer, IZoomTarget, ITextViewerWithToolstripBackend {
        
        public TextViewerWithToolstripBackend() {
            InitializeComponent();
        }

        TextViewerWithToolstrip _frontend = null;
        public TextViewerWithToolstrip Frontend {
            get { return _frontend; }
            protected set {
                if(_frontend!=value) {
                    TextViewerBackend = value.TextViewer.Backend as TextViewerBackend;
                }
                _frontend = value;
            }
        }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (TextViewerWithToolstrip)frontend;
        }

        private TextViewerBackend _textViewerBackend = null;
        public TextViewerBackend TextViewerBackend {
            get {
                if (_textViewerBackend == null) {
                    _textViewerBackend = new TextViewerBackend();
                    SetEditor(_textViewerBackend);
                }
                return _textViewerBackend;
            }
            set {
                _textViewerBackend = value;
                SetEditor(_textViewerBackend);
            }
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

        public bool ToolStripVisible {
            get { return this.TextBoxEditorToolStrip.Visible; }
            set { this.TextBoxEditorToolStrip.Visible = value; }
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

        private TextBoxEditorToolStrip _textBoxEditorToolStrip = null;
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

        Xwt.Size IVidgetBackend.Size {
            get { return this.Size.ToXwt(); }
        }

        void IVidgetBackend.SetFocus () { this.Focus (); }

        void IVidgetBackend.Invalidate (Xwt.Rectangle rect) {
            this.Invalidate(rect.ToGdi());
        }
     
        #endregion

       
    }
}
