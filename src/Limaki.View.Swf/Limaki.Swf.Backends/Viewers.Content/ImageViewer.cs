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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Limada.Model;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Model.Content;
using System.IO;
using Limaki.View.Swf;
using Limaki.Viewers;
using Limaki.View.Swf.Visualizers;
using Limaki.View;

namespace Limaki.Swf.Backends.Viewers.Content {

    public class ImageViewer : ContentStreamViewer {

        //TODO: replace with factory methods
        SwfImageDisplayBackend _backend = null;
        public override IVidgetBackend Backend {
            get {
                if (_backend == null) {
                    _backend = new SwfImageDisplayBackend();
                    OnAttach (_backend);
                }
                return _backend;
            }
        }

        private ICollection<Int64> _imageStreamTypes = null;
        public ICollection<Int64> ImageStreamTypes {
            get {
                if (_imageStreamTypes == null) {
                    _imageStreamTypes = new Set<Int64>();
                    _imageStreamTypes.Add(ContentTypes.TIF);
                    _imageStreamTypes.Add(ContentTypes.JPG);
                    _imageStreamTypes.Add(ContentTypes.GIF);
                    _imageStreamTypes.Add(ContentTypes.PNG);
                    _imageStreamTypes.Add(ContentTypes.BMP);
                    _imageStreamTypes.Add(ContentTypes.EMF);
                    _imageStreamTypes.Add(ContentTypes.WMF);
                }
                return _imageStreamTypes;
            }
            set { _imageStreamTypes = value; }
        }

        public override bool Supports(long streamType) {
            return ImageStreamTypes.Contains(streamType);
        }

        public override bool CanSave() {
            return false;
        }

        public override void SetContent(Content<Stream> content) {
            var backend = Backend as SwfImageDisplayBackend;

            if (backend != null) {
                var display = backend.Display;
                display.BackColor = this.BackColor;

                display.Data = Image.FromStream(content.Data);

                display.ZoomState = ZoomState.FitToScreen;
            }
            if (IsStreamOwner) {
                content.Data.Close ();
                content.Data = null;
            }
        }

        public override void Save(Content<Stream> content) { }

        public override void Dispose() {
            if (_backend != null) {
                _backend.Dispose();
                _backend = null;
            }
        }

        public override void Clear() {
            base.Clear();
            var backend = _backend as SwfImageDisplayBackend;
            if (backend != null) {
                backend.Display.Data = null;
            }
        }
    }
}