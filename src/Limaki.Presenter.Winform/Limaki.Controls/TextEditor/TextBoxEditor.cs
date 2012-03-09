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


// this control uses ideas from RicherTextBox by ???

namespace Limaki.Winform.Controls.TextEditor {
    public partial class TextBoxEditor : UserControl, IZoomTarget {
        public TextBoxEditor() {
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

    }
}

