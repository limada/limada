using System.Windows.Forms;
using Limada.Model;
using Limaki.Winform.Controls.TextEditor;
using Limaki.Model.Streams;
using System.IO;

namespace Limaki.App.StreamViewers {
    public class TextViewerWithToolstripController : StreamViewerController {
        TextBoxEditorWithToolStrip _control = null;
        TextViewerController _controller=null;
        TextViewerController controller {
            get {
                if (_controller == null) {
                    _controller = new TextViewerController ();
                }
                return _controller;
            }
        }

        public override Control Control {
            get {
                if (_control == null) {
                    _control = new TextBoxEditorWithToolStrip();
                    _control.TextBoxEditor = controller.Control as TextBoxEditor;
                    this.OnAttach (_control);
                }
                return _control;
            }
        }

        public override bool CanView(long streamType) {
            return controller.CanView(streamType);
        }

        public override bool CanSave() {
            return controller.CanSave ();
        }

        public override void SetContent(StreamInfo<Stream> info) {
            controller.SetContent(info);

            ((TextBoxEditorWithToolStrip)Control).TextBoxEditorToolStrip.Visible = 
                !_controller.ReadOnly;

            if (IsStreamOwner) {
                info.Data.Close();
                info.Data = null;
            }
        }

        public override void Save(StreamInfo<Stream> info) {
            controller.Save (info);
        }

        public override void OnShow() {
            controller.OnShow();
        }

        public override void Dispose() {
            controller.Dispose ();
        }
        public override void Clear() {
            base.Clear();
            controller.Clear ();
        }
    }
}