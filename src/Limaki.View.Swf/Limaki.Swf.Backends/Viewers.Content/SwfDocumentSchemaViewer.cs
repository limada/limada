/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */


using System.IO;
using Limada.View;
using Image = System.Drawing.Image;

namespace Limaki.View.Swf.Backends {

    public class SwfDocumentSchemaViewer : DocumentSchemaViewer {

        //TODO: refactor this to Xwt.Drawing.Image and move to DocumentSchemaViewer
        IDisplay<Image> _imageDisplay = null;
        public new IDisplay<Image> ImageDisplay {
            get { return _imageDisplay; }
            set {
                _imageDisplay = value;
                base.ImageDisplay = value;
            }
        }

        protected override Stream ImageStream {
            set {
                if (value != null) {
                    ImageDisplay.Data = Image.FromStream(value);
                } else {
                    ImageDisplay.Data = null;
                }
            }
        }

        DocumentSchemaBackend _backend = null;
        public override object Backend {
            get {
                if (_backend == null) {
                    _backend = new DocumentSchemaBackend(this);
                    OnAttach(_backend);
                }
                return _backend;
            }
        }

        public override void Clear () {
            var control = _backend as DocumentSchemaBackend;
            if (control != null) {
                control.GraphSceneDisplay.Data = null;
                control.ImageDisplay.Data = null;
            }
            base.Clear();
        }
    }

 
}