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
 * 
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Limaki.Common;
using Limaki.Common.Text.RTF;
using Limaki.Contents;
using Limaki.Contents.IO;
using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View.ContentViewers {

    public class TextContentViewer : ContentStreamViewer {

        protected TextViewer _textViewer = null;
        public virtual TextViewer TextViewer {
            get {
                if (_textViewer == null) {
                    _textViewer = new TextViewer();
                    var backend = _textViewer.Backend;
                    backend.BorderStyle = VidgetBorderStyle.None;
                    OnAttachBackend(_textViewer.Backend);
                }
                return _textViewer;
            }
        }

        public override IVidget Frontend { get { return TextViewer; } }

        public override IVidgetBackend Backend {
            get { return TextViewer.Backend; }
        }

        public override bool Supports (long streamType) {
            return streamType == ContentTypes.RTF || streamType == ContentTypes.ASCII || streamType == ContentTypes.Text;
        }

        double zoom = 1;
        public bool ReadOnly { get; set; }

        public Stream PrepareRead (Stream stream) {
            var filter = new AdobeRTFFilter();
            if (filter.IsAdobeRTF(stream)) {
                stream = filter.RemoveAdobeParagraphTags(stream);
            }

            var doccom = filter.GetDoccom(stream);
            ReadOnly = Regex.Matches(doccom, "limada.note", RegexOptions.IgnoreCase).Count == 0;
            return stream;
        }

        public override void SetContent (Content<Stream> content) {

            if (TextViewer == null)
                return;

            zoom = TextViewer.ZoomFactor;

            var stream = content.Data;

            var textType = TextViewerTextType.PlainText;

            try {
                var info = new RtfContentSpot ().Use (content.Data);
                if (info != null && info.ContentType == ContentTypes.RTF) {
                    if (content.ContentType != info.ContentType)
                        Trace.WriteLine (this.GetType () + ":  wrong contenttype detected");
                    content.ContentType = info.ContentType;
                }

                if (content.ContentType == ContentTypes.RTF) {
                    textType = TextViewerTextType.RichText;
                    stream = PrepareRead(stream);

                } else if (content.ContentType == ContentTypes.Text) {
                    textType = TextViewerTextType.UnicodePlainText;
                }

                TextViewer.Load(stream, textType);

            } catch (Exception ex) {
                ExceptionHandler.Catch(ex, MessageType.OK);
            } finally {
                if (IsStreamOwner) {
                    content.Data.Close();
                    content.Data = null;
                }
            }
        }

        public override bool CanSave () {
            return TextViewer != null && !TextViewer.ReadOnly && TextViewer.Modified;
        }

        public override void Save (Content<Stream> content) {
            if (CanSave()) {
                if (content != null) {

                    Stream stream = new MemoryStream ();
                    TextViewer.Save (stream, TextViewerTextType.RichText);

                    stream.Position = 0;
                    var filter = new RTFFilter();
                    stream = filter.SetDoccom(stream, "Limada.Notes");

                    stream.Position = 0;

                    content.ContentType = ContentTypes.RTF;
                    content.Compression = CompressionType.bZip2;
                    content.Data = stream;
                }
            }
            TextViewer.Modified = false;
        }

        public override void OnShow () {
            base.OnShow();

            TextViewer.AutoScrollOffset = new Point();
            TextViewer.ZoomFactor = this.zoom;
            TextViewer.ReadOnly = this.ReadOnly;
            TextViewer.Modified = false;
        }

        public override void Dispose () {
            if (_textViewer != null) {
                _textViewer.Dispose();
            }
        }

        public override void Clear () {
            base.Clear();
            if (_textViewer != null) {
                TextViewer.Clear();
            }
        }
    }
}