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

namespace Limaki.Swf.Backends.Viewers.Content {

    public class TextViewerWithToolstrip : ContentStreamViewer {

        TextBoxEditorWithToolStrip _control = null;
        TextViewer _controller=null;
        TextViewer controller {
            get {
                if (_controller == null) {
                    _controller = new TextViewer ();
                }
                return _controller;
            }
        }

        public override object Backend {
            get {
                if (_control == null) {
                    _control = new TextBoxEditorWithToolStrip();
                    _control.TextBoxEditor = controller.Backend as TextBoxEditor;
                    this.OnAttach (_control);
                }
                return _control;
            }
        }

        public override bool Supports(long streamType) {
            return controller.Supports(streamType);
        }

        public override bool CanSave() {
            return controller.CanSave ();
        }

        public override void SetContent(Content<Stream> content) {
            controller.SetContent(content);

            ((TextBoxEditorWithToolStrip)Backend).TextBoxEditorToolStrip.Visible = 
                !_controller.ReadOnly;

            if (IsStreamOwner) {
                content.Data.Close();
                content.Data = null;
            }
        }

        public override void Save(Content<Stream> content) {
            controller.Save (content);
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