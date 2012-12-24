/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Limaki.Common.Text.RTF;
using Limaki.Model.Streams;
using Limaki.Viewers;
using Limaki.Swf.Backends.TextEditor;
using System;
using Limaki.Common;

namespace Limaki.Swf.Backends.Viewers.Content {
    public class TextViewerController : StreamViewerController {
        TextBoxEditor _control = null;
        public override object Backend {
            get {
                if (_control == null) {
                    _control = new TextBoxEditor();
                    _control.Multiline = true;
                    _control.BorderStyle = BorderStyle.None;
                    _control.EnableAutoDragDrop = true;
                    OnAttach (_control );
                }
                return _control;
            }
        }

        public override bool Supports(long streamType) {
            return streamType == ContentTypes.RTF || streamType == ContentTypes.ASCII;
        }

        double zoom = 1;
        public bool ReadOnly {get;set;}


        public Stream PrepareRead(Stream stream) {
            var filter = new AdobeRTFFilter();
            if (filter.IsAdobeRTF(stream)) {
                stream = filter.RemoveAdobeParagraphTags(stream);
            }

            var doccom = filter.GetDoccom(stream);
            ReadOnly = Regex.Matches(doccom, "limada.note", RegexOptions.IgnoreCase).Count == 0;
            return stream;
        }

        public override void SetContent(Content<Stream> content) {
            TextBoxEditor control = Backend as TextBoxEditor;
            if (control == null)
                return;
            
            zoom = control.ZoomFactor;

            var stream = content.Data;
            
            var rtfStreamType = RichTextBoxStreamType.PlainText;

            try {
                if (content.StreamType == ContentTypes.RTF) {
                    rtfStreamType = RichTextBoxStreamType.RichText;

                    stream = PrepareRead(stream);
                }

                control.Load(stream, rtfStreamType);

            } catch (Exception ex) {
                ExceptionHandler.Catch(ex, MessageType.OK);
            } finally {
                stream.Close();
                stream = null;
            }
        }
        
        public override bool CanSave() {
            return _control != null && !_control.ReadOnly && _control.Modified;
        }


        public virtual Stream DoSave() {
            Stream stream = new MemoryStream();
            _control.Save(stream, RichTextBoxStreamType.RichText);
            return stream;
        }

        public override void Save(Content<Stream> content) {
            if (CanSave()) {
                if (content != null) {
                    var stream = DoSave ();
                    
                    stream.Position = 0;
                    var filter = new RTFFilter();
                    stream = filter.SetDoccom(stream, "Limada.Notes");

                    stream.Position = 0;


                    content.StreamType = ContentTypes.RTF;
                    content.Compression = CompressionType.bZip2;
                    content.Data = stream;
                }
            }
            _control.Modified = false;
        }

        public override void OnShow() {
            base.OnShow();
            // this is to bring textControl to show proper scrolloffset and zoom
            // but zoom does not work
            //Application.DoEvents(); // this disturbs VisualsDisplay.MouseTimerAction!
            _control.AutoScrollOffset = new Point();
            _control.ZoomFactor = this.zoom;
            _control.ReadOnly = this.ReadOnly;
            //Application.DoEvents();
            _control.Modified = false;
        }



        public override void Dispose() {
            if (_control != null) {
                _control.Dispose ();
            }
        }

        public override void Clear() {
            base.Clear();
            if (_control != null) {
                var control = _control as TextBoxEditor;
                control.Clear ();
            }
        }
    }
}