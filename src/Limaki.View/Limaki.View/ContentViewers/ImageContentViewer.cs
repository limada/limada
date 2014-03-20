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

using Limaki.Common.Collections;
using Limaki.Contents;
using System;
using System.Collections.Generic;
using System.IO;
using Limaki.Drawing;
using Limaki.View.Viz;
using Limaki.View.Viz.Visualizers;
using Xwt.Drawing;

namespace Limaki.View.ContentViewers {

    /// <summary>
    /// ContentViewer working with Xwt.Drawing.Image
    /// </summary>
    public class ImageContentViewer : ContentStreamViewer {

        public override IVidget Frontend {
            get { return ImageDisplay; }
        }

        public override IVidgetBackend Backend {
            get { return ImageDisplay.Backend;}
        }

        IDisplay<Image> _display = null;
        public virtual IDisplay<Image> ImageDisplay {
            get {
                if (_display == null) {
                    _display = new ImageDisplay();
                    OnAttachBackend(_display.Backend);
                    _display.ZoomState = ZoomState.FitToScreen;
                }
                return _display;
            }
        }

        private ICollection<Int64> _imageStreamTypes = null;
        public ICollection<Int64> ImageStreamTypes {
            get {
                if (_imageStreamTypes == null) {
                    _imageStreamTypes = new Set<Int64>();
                    _imageStreamTypes.Add(ContentTypes.JPG);
                    _imageStreamTypes.Add(ContentTypes.GIF);
                    _imageStreamTypes.Add(ContentTypes.PNG);
                    _imageStreamTypes.Add(ContentTypes.BMP);
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
            if (ImageDisplay != null) {
                var display = ImageDisplay;
                display.BackColor = this.BackColor;
                display.Data = Image.FromStream(content.Data);
            }
            if (IsStreamOwner) {
                content.Data.Close ();
                content.Data = null;
            }
        }

        public override void Save(Content<Stream> content) { }

        public override void Dispose() {
            if (_display != null) {
                _display.Dispose();
                _display = null;
            }
        }

        public override void Clear() {
            base.Clear();
            if (_display != null) {
                _display.Data = null;
            }
        }
    }
}