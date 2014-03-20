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
using Limaki.Drawing;
using Limaki.View;
using Limaki.View.Swf.Visualizers;
using Limaki.Viewers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Limaki.Swf.Backends.Viewers.Content {

    /// <summary>
    /// ContentViewer working with System.Drawing.Image
    /// </summary>
    public class SdImageContentViewer : ContentStreamViewer {

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
                    _display = new SdImageDisplay();
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
            if (ImageDisplay != null) {
                var display = ImageDisplay;
                display.BackColor = this.BackColor;
                if (content.Data != null)
                    display.Data = Image.FromStream(content.Data);
                else {
                    display.Data = null;
                    display.Reset();
                    return;
                }
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