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


using System.IO;
using System.Windows.Forms;
using Limaki.Model.Content;
using Limaki.Viewers;
using Limaki.Swf.Backends.TextEditor;
using Limaki.View;

namespace Limaki.Swf.Backends.Viewers.Content {

    public class TextViewerWithToolstrip : ContentStreamViewer {

       
        TextViewer _textViewer=null;
        TextViewer TextViewer {
            get {
                if (_textViewer == null) {
                    _textViewer = new TextViewer ();
                }
                return _textViewer;
            }
        }

        //TODO: replace with factory methods
        TextViewerWithToolstripBackend _backend = null;
        public override IVidgetBackend Backend {
            get {
                if (_backend == null) {
                    _backend = new TextViewerWithToolstripBackend();
                    _backend.TextViewerBackend = TextViewer.Backend as TextViewerBackend;
                    this.OnAttachBackend (_backend);
                }
                return _backend;
            }
        }

        public override bool Supports(long streamType) {
            return TextViewer.Supports(streamType);
        }

        public override bool CanSave() {
            return TextViewer.CanSave ();
        }

        public override void SetContent(Content<Stream> content) {
            TextViewer.SetContent(content);

            ((TextViewerWithToolstripBackend)Backend).TextBoxEditorToolStrip.Visible = 
                !_textViewer.ReadOnly;

            if (IsStreamOwner) {
                content.Data.Close();
                content.Data = null;
            }
        }

        public override void Save(Content<Stream> content) {
            TextViewer.Save (content);
        }

        public override void OnShow() {
            TextViewer.OnShow();
        }

        public override void Dispose() {
            TextViewer.Dispose ();
        }
        public override void Clear() {
            base.Clear();
            TextViewer.Clear ();
        }
    }
}