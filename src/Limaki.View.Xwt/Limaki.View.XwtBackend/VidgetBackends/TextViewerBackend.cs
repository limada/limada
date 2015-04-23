using System;
using System.Collections.Generic;
using System.IO;
using Limaki.View.Vidgets;
using Limaki.Contents.Text;
using Limaki.Drawing;

namespace Limaki.View.XwtBackend {

    /// <summary>
    /// a readonly implementation of TextViewer basede on WebView
    /// rtf is converted to html and shown in WevView
    /// </summary>
    public class TextViewerBackend : VidgetBackend<Xwt.WebView>, ITextViewerVidgetBackend {

        public Vidgets.TextViewer Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            Frontend = (Vidgets.TextViewer)frontend;
        }

        public VidgetBorderStyle BorderStyle { get; set; }

        public bool ReadOnly { get { return true; } set { } }

        public bool Modified { get; set; }

        public Xwt.Point AutoScrollOffset { get; set; }

        public void Save (Stream stream, TextViewerTextType textType) {
        }

        public void Load (Stream stream, TextViewerTextType textType) {
            using (var reader = new StreamReader (stream)) {
                string html = null;
                if (textType == TextViewerTextType.PlainText || textType == TextViewerTextType.UnicodePlainText) {
                    var text = reader.ReadToEnd ();
                    html = System.Net.WebUtility.HtmlEncode (text);
                }

                if (textType == TextViewerTextType.RichText) {
                    var doc = new HtmlDocument ();
                    var importer = new RtfImporter (stream, doc);
                    importer.Import ();
                    html = "<html><body>" + doc.Body + "</html></body>";
                }

                Widget.LoadHtml (html, "");
            }
        }

        public void Clear () {
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