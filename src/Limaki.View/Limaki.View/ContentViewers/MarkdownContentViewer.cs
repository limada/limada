using Limaki.Common;
using Limaki.Contents;
using Limaki.View.Vidgets;
using System.IO;

namespace Limaki.View.ContentViewers {

    public class MarkdownContentViewer : ContentStreamViewer {

        protected IMarkdownEdit _markdownEdit = null;

        public virtual IMarkdownEdit MarkdownEdit {
            get {
                if (_markdownEdit == null) {
                    _markdownEdit = Registry.Create<IMarkdownEdit>();
                    var backend = _markdownEdit.Backend;
                    OnAttachBackend (_markdownEdit.Backend);
                }
                return _markdownEdit;
            }
        }

        public static bool Available () {
            return VidgetToolkit.CurrentEngine.Backend.BackendRegistered<IMarkdownEditBackend> () && Registry.Factory.Contains<IMarkdownEdit> ();
        }

        public override IVidget Frontend { get { return MarkdownEdit; } }

        public override IVidgetBackend Backend { get { return MarkdownEdit.Backend; } }

        public override bool Supports (long streamType) {
            return streamType == ContentTypes.Markdown;
        }

        public override bool CanSave () {
            return true;
        }

        public override void Save (Content<Stream> content) {
            content.Data = new MemoryStream ();
            content.ContentType = ContentTypes.Markdown;
            content.Compression = CompressionType.bZip2;
            MarkdownEdit.Save (content.Data);
        }

        public override void SetContent (Content<Stream> content) {
            MarkdownEdit.Load (content.Data);
        }

        public override void Dispose () {
            if (_markdownEdit != null) {
                _markdownEdit.Dispose ();
            }
        }

        public override void Clear () {
            base.Clear ();
            if (_markdownEdit != null) {
                _markdownEdit.Clear ();
            }
        }
    }
}