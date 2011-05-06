/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Limaki.Common.Text.RTF;
using Limaki.Model.Streams;
using Limaki.UseCases.Viewers;
using Limaki.Winform.Controls.TextEditor;
using System;
using Limaki.Common;

namespace Limaki.UseCases.Winform.Viewers.StreamViewers {
    public class TextViewerController : StreamViewerController {
        TextBoxEditor _control = null;
        public override object Control {
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
            return streamType == StreamTypes.RTF || streamType == StreamTypes.ASCII;
        }

        float zoom = 1;
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

        public override void SetContent(StreamInfo<Stream> info) {
            TextBoxEditor control = Control as TextBoxEditor;
            if (control == null)
                return;
            
            zoom = control.ZoomFactor;

            var stream = info.Data;
            
            var rtfStreamType = RichTextBoxStreamType.PlainText;

            try {
                if (info.StreamType == StreamTypes.RTF) {
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

        public override void Save(StreamInfo<Stream> info) {
            if (CanSave()) {
                if (info != null) {
                    var stream = DoSave ();
                    
                    stream.Position = 0;
                    var filter = new RTFFilter();
                    stream = filter.SetDoccom(stream, "Limada.Notes");

                    stream.Position = 0;


                    info.StreamType = StreamTypes.RTF;
                    info.Compression = CompressionType.bZip2;
                    info.Data = stream;
                }
            }
            _control.Modified = false;
        }

        public override void OnShow() {
            base.OnShow();
            // this is to bring textControl to show proper scrolloffset and zoom
            // but zoom does not work
            //Application.DoEvents(); // this disturbs WidgetDisplay.MouseTimerAction!
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