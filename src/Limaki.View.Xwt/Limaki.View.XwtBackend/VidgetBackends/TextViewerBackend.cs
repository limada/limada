using System;
using System.Collections.Generic;
using System.IO;
using Limaki.View.Vidgets;
using Limaki.Contents.Text;
using Limaki.Drawing;
using System.Text;
using Limaki.Common;
using Limaki.Contents;
using Limaki.Common.Text;
using Limaki.View.Common;

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
                var text = new StringBuilder();
                var line = reader.ReadLine ();
                while (line != null) {
                    text.Append ($"{System.Net.WebUtility.HtmlEncode (line).ReplaceLeading(' ',"&emsp;")}<br>");
                    line = reader.ReadLine ();
                }
                html = text.ToString();

            }

            if (textType == TextViewerTextType.RichText) {
                var converterPool = Registry.Pooled<ConverterPool<Stream>> ();
                var converter = converterPool.Find (ContentTypes.RTF, ContentTypes.HTML) as IHtmlConverter;
                if (converter != null) {
                    html = converter.WithHmtlHeaderTags (stream);
                }
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