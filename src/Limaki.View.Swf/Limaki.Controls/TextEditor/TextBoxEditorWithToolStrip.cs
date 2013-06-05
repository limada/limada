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

namespace Limaki.Swf.Backends.TextEditor {
    public partial class TextBoxEditorWithToolStrip : ToolStripContainer, IZoomTarget, IVidgetBackend {
        public TextBoxEditorWithToolStrip() {
            InitializeComponent();
        }

        void SetEditor(TextBoxEditor editor) {
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

        public TextBoxEditor TextBoxEditor {
            get {
                if (_textBoxEditor == null) {
                    _textBoxEditor = new TextBoxEditor();
                    SetEditor (_textBoxEditor);
                }
                return _textBoxEditor;
            }
            set {
                _textBoxEditor = value;
                SetEditor(_textBoxEditor);
            }
        }

        void SetToolStrip(TextBoxEditorToolStrip toolstrip) {
            this.SuspendLayout ();
            this.TopToolStripPanel.Controls.Clear ();
            if (toolstrip != null) {
                this.TopToolStripPanel.Controls.Add (toolstrip);
            }
            TextBoxEditor.ToolStrip = toolstrip;
            this.ResumeLayout ();
        }

        //bool _toolStripEnabled = false;
        //public bool ToolStripEnabled {
        //    get { return _textBoxEditor != null && _textBoxEditor.ToolStrip != null; }
        //    set {
        //        if (_textBoxEditor != null) {
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

        private TextBoxEditor _textBoxEditor=null;
        private TextBoxEditorToolStrip _textBoxEditorToolStrip=null;

        #region IZoomTarget Member

        public ZoomState ZoomState {
            get {
                if (_textBoxEditor != null) {
                    return _textBoxEditor.ZoomState;
                } else {
                    return ZoomState.Original;
                }
            }
            set {
                if (_textBoxEditor != null) {
                    _textBoxEditor.ZoomState = value;
                }
            }
        }

        public double ZoomFactor {
            get {
                if (_textBoxEditor != null) {
                    return _textBoxEditor.ZoomFactor;
                } else {
                    return 1d;
                }
            }
            set {
                if (_textBoxEditor != null) {
                    _textBoxEditor.ZoomFactor = value;
                }
            }
        }

        public void UpdateZoom() {
            if (_textBoxEditor != null) {
                _textBoxEditor.UpdateZoom ();
            }
        }

        #endregion

        #region IVidgetBackend-Implementation

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
