using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.View;


namespace Limaki.Winform.Controls.TextEditor {
    public partial class TextBoxEditorWithToolStrip : ToolStripContainer, IZoomTarget {
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
    }
}
