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


using Limaki.Common;
using Limaki.Common.Text.RTF;
using Limaki.Contents;
using Limaki.Model.Content;
using Limaki.Viewers.Vidgets;
using Limaki.View;
using Limaki.Viewers;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Xwt;

namespace Limaki.Viewers.StreamViewers {

    public class TextContentViewer : ContentStreamViewer {

        protected TextViewer _textViewer = null;
        public virtual TextViewer TextViewer {
            get {
                if (_textViewer != null) {
                    _textViewer = new TextViewer();
                    var backend = _textViewer.Backend;
                    backend.Multiline = true;
                    backend.BorderStyle = VidgetBorderStyle.None;
                    backend.EnableAutoDragDrop = true;
                    OnAttachBackend(_textViewer.Backend);
                }
                return _textViewer;
            }
        }

        public virtual ITextViewerBackend TextViewerBackend { get { return TextViewer.Backend; } }

        public override IVidget Frontend { get { return TextViewer; } }

        public override IVidgetBackend Backend {
            get { return TextViewerBackend; }
        }

        public override bool Supports (long streamType) {
            return streamType == ContentTypes.RTF || streamType == ContentTypes.ASCII;
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
            var backend = TextViewerBackend;
            if (backend == null)
                return;

            zoom = backend.ZoomFactor;

            var stream = content.Data;

            var rtfStreamType = TextViewerRtfType.PlainText;

            try {
                if (content.ContentType == ContentTypes.RTF) {
                    rtfStreamType = TextViewerRtfType.RichText;

                    stream = PrepareRead(stream);
                }

                backend.Load(stream, rtfStreamType);

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
            var backend = TextViewerBackend;
            return backend != null && !backend.ReadOnly && backend.Modified;
        }


        public virtual Stream DoSave () {
            var stream = new MemoryStream();
            TextViewerBackend.Save(stream, TextViewerRtfType.RichText);
            return stream;
        }

        public override void Save (Content<Stream> content) {
            if (CanSave()) {
                if (content != null) {
                    var stream = DoSave();

                    stream.Position = 0;
                    var filter = new RTFFilter();
                    stream = filter.SetDoccom(stream, "Limada.Notes");

                    stream.Position = 0;


                    content.ContentType = ContentTypes.RTF;
                    content.Compression = CompressionType.bZip2;
                    content.Data = stream;
                }
            }
            TextViewerBackend.Modified = false;
        }

        public override void OnShow () {
            base.OnShow();
            var backend = TextViewerBackend as ITextViewerBackend;
            // this is to bring textControl to show proper scrolloffset and zoom
            // but zoom does not work
            //Application.DoEvents(); // this disturbs VisualsDisplay.MouseTimerAction!
            backend.AutoScrollOffset = new Point();
            backend.ZoomFactor = this.zoom;
            backend.ReadOnly = this.ReadOnly;
            //Application.DoEvents();
            backend.Modified = false;
        }

        public override void Dispose () {
            if (_textViewer != null) {
                _textViewer.Dispose();
            }
        }

        public override void Clear () {
            base.Clear();
            if (_textViewer != null) {
                TextViewerBackend.Clear();
            }
        }
    }
}