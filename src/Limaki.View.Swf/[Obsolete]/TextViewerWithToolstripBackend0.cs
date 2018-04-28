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
using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.View;
using Limaki.View.Common;
using Limaki.View.Vidgets;
using Xwt.GdiBackend;

namespace Limaki.View.SwfBackend.VidgetBackends {

    [Obsolete]
    public partial class TextViewerWithToolstripBackend0 : VidgetBackend<ToolStripContainer>, IZoomTarget, ITextViewerWithToolstripVidgetBackend0 {

        TextViewerWithToolstrip0 _frontend = null;
        public new TextViewerWithToolstrip0 Frontend {
            get { return _frontend; }
            protected set {
                if(_frontend!=value) {
                    TextViewerBackend = value.TextViewer.Backend as TextViewerBackend;
                }
                _frontend = value;
            }
        }

        protected override void Compose () {
            base.Compose ();
            InitializeComponent ();
        }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (TextViewerWithToolstrip0)frontend;
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
            Control.SuspendLayout ();
            Control.ContentPanel.Controls.Clear();
            if (editor != null) {
                var editorControl = editor.Control;
                Control.ContentPanel.Controls.Add (editorControl);
                editorControl.Dock = DockStyle.Fill;
                // TODO:
                //editorControl.Enter += (sender, args) => { this.OnEnter (args); };
                //editorControl.MouseUp += (sender, args) => { this.OnMouseUp (args); };
                //editorControl.GotFocus += (sender, args) => { this.OnGotFocus (args); };
            }
            Control.ResumeLayout ();
        }

        public bool ToolStripVisible {
            //get { return this.TextBoxEditorToolStrip.Visible; }
            //set { this.TextBoxEditorToolStrip.Visible = value; }
            get;set;
        }

        //void SetToolbar(TextBoxEditorToolStrip0 toolstrip) {
        //    Control.SuspendLayout ();
        //    Control.TopToolStripPanel.Controls.Clear ();
        //    if (toolstrip != null) {
        //        Control.TopToolStripPanel.Controls.Add (toolstrip);
        //    }
        //    TextViewerBackend.Toolbar = toolstrip;
        //    Control.ResumeLayout ();
        //}

        //private TextBoxEditorToolStrip0 _textBoxEditorToolStrip = null;
        //public virtual TextBoxEditorToolStrip0 TextBoxEditorToolStrip {
        //    get {
        //        if (_textBoxEditorToolStrip ==null) {
        //            _textBoxEditorToolStrip = new TextBoxEditorToolStrip0 ();
        //            SetToolbar (_textBoxEditorToolStrip);
        //        }
                
        //        return _textBoxEditorToolStrip;
        //    }
        //    set { _textBoxEditorToolStrip = value;
        //          SetToolbar(_textBoxEditorToolStrip);
        //    }
        //}

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


        private void InitializeComponent () {
            Control.SuspendLayout ();
            Control.BottomToolStripPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            Control.BottomToolStripPanel.Location = new System.Drawing.Point (0, 175);
            Control.BottomToolStripPanel.Name = "";
            Control.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            Control.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding (3, 0, 0, 0);
            Control.BottomToolStripPanel.Size = new System.Drawing.Size (150, 0);

            Control.ContentPanel.Size = new System.Drawing.Size (150, 175);

            Control.LeftToolStripPanel.Dock = System.Windows.Forms.DockStyle.Left;
            Control.LeftToolStripPanel.Location = new System.Drawing.Point (0, 0);
            Control.LeftToolStripPanel.Name = "";
            Control.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Vertical;
            Control.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding (0, 3, 0, 0);
            Control.LeftToolStripPanel.Size = new System.Drawing.Size (0, 175);

            Control.RightToolStripPanel.Dock = System.Windows.Forms.DockStyle.Right;
            Control.RightToolStripPanel.Location = new System.Drawing.Point (150, 0);
            Control.RightToolStripPanel.Name = "";
            Control.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Vertical;
            Control.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding (0, 3, 0, 0);
            Control.RightToolStripPanel.Size = new System.Drawing.Size (0, 175);

            Control.TopToolStripPanel.Dock = System.Windows.Forms.DockStyle.Top;
            Control.TopToolStripPanel.Location = new System.Drawing.Point (0, 0);
            Control.TopToolStripPanel.Name = "";
            Control.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            Control.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding (3, 0, 0, 0);
            Control.TopToolStripPanel.Size = new System.Drawing.Size (150, 30);
            Control.ResumeLayout (false);
            Control.PerformLayout ();

        }
    }
}
