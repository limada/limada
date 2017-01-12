using System;
using System.Collections.Generic;
using System.IO;
using Limaki.View.Vidgets;
using Limaki.Contents.Text;
using Limaki.Drawing;
using System.Text;

namespace Limaki.View.XwtBackend {

    /// <summary>
    /// a readonly implementation of TextViewer basede on WebView
    /// rtf is converted to html and shown in WevView
    /// </summary>
    public class TextViewerBackend : VidgetBackend<Xwt.WebView>, ITextViewerVidgetBackend {

        public new Vidgets.TextViewer Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            Frontend = (Vidgets.TextViewer)frontend;
        }

        public VidgetBorderStyle BorderStyle { get; set; }

        public bool ReadOnly { get { return true; } set { } }

        public bool Modified { get; set; }

        public Xwt.Point AutoScrollOffset { get; set; }

        public void Save (Stream stream, TextViewerTextType textType) {
        }

        public void Load (Stream stream, TextViewerTextType textType) {

            string html = null;
            if (textType == TextViewerTextType.PlainText || textType == TextViewerTextType.UnicodePlainText) {
                var reader = new StreamReader (stream, textType == TextViewerTextType.UnicodePlainText ? Encoding.Unicode : Encoding.ASCII);
                var text = reader.ReadToEnd ();
                html = System.Net.WebUtility.HtmlEncode (text);

            }

            if (textType == TextViewerTextType.RichText) {
                var doc = new HtmlDocument ();
                var importer = new RtfImporter (stream, doc);
                importer.Import ();
                html = "<html><body>" + doc.Body + "</body></html>";
            }
            stream.Position = 0;
            Widget.LoadHtml (html, "");

        }

        public void Clear () {
            Widget.LoadHtml ("<html><body></body></html>", "");
        }

        public void SetAttribute (Xwt.Drawing.TextAttribute attribute) {
        }

        public IEnumerable<Xwt.Drawing.TextAttribute> GetAttributes () {
            yield break;
        }

        public event EventHandler SelectionChanged;

        public ZoomState ZoomState { get; set; }

        public double ZoomFactor { get; set; }

        public void UpdateZoom () {
        }
    }
}