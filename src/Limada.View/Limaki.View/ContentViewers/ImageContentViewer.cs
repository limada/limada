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
using Limaki.Common;
using System.Linq;
using Limaki.View.Common;

namespace Limaki.View.ContentViewers {

    /// <summary>
    /// ContentViewer working with Xwt.Drawing.Image
    /// </summary>
    public class ImageContentViewer : ContentStreamViewer {

        public override IVidget Frontend {
            get { return ImageDisplay; }
        }

        public override IVidgetBackend Backend {
            get { return ImageDisplay.Backend; }
        }

        IDisplay<Image> _display = null;
        public virtual IDisplay<Image> ImageDisplay {
            get {
                if (_display == null) {
                    _display = new ImageDisplay ();
                    _display.ZoomState = ZoomState.FitToScreen;
                }
                return _display;
            }
        }

        private ICollection<long> _converterStreamTypes = new HashSet<long>();
        private ICollection<long> _imageStreamTypes = null;
        public ICollection<long> ImageStreamTypes {
            get {
                if (_imageStreamTypes == null) {
                    _imageStreamTypes = new Set<Int64> ();
                    _imageStreamTypes.Add (ContentTypes.JPG);
                    _imageStreamTypes.Add (ContentTypes.GIF);
                    _imageStreamTypes.Add (ContentTypes.PNG);
                    _imageStreamTypes.Add (ContentTypes.BMP);

                    if (Registry.Pooled<IUISystemInformation> ().ImageFormats.Any (f => f.Contains ("tif"))) {
                        _imageStreamTypes.Add (ContentTypes.TIF);
                    }
                    if (!_imageStreamTypes.Contains (ContentTypes.TIF)) {
                        if (Registry.Pooled<ConverterPool<Stream>> ()
                            .Find (ContentTypes.TIF, ContentTypes.PNG) != null) {
                            _imageStreamTypes.Add (ContentTypes.TIF);
                            _converterStreamTypes.Add (ContentTypes.TIF);
                        }
                    }
                }
                return _imageStreamTypes;
            }
            set { _imageStreamTypes = value; }
        }

        public override bool Supports (long streamType) {
            return ImageStreamTypes.Contains (streamType);
        }

        public override bool CanSave () {
            return false;
        }

        public Content<Stream> Convert (Content<Stream> source) {

            var sinkType = ContentTypes.PNG;
            var converter = Registry.Pooled<ConverterPool<Stream>> ()
                                    .Find (source.ContentType, sinkType);

            if (converter != null) {
                var conv = converter.Use (source, sinkType);
                return conv;
            }

            return null;
        }

        protected void CloseContent (Content<Stream> content, bool isStreamOwner) {
            if (isStreamOwner) {
                if (content.Data != null)
                    content.Data.Close ();
                content.Data = null;
            }
        }

        public override void SetContent (Content<Stream> content) {
            if (ImageDisplay != null) {
                var display = ImageDisplay;
                display.BackColor = this.BackColor;
                if (content.Data != null) {
                    content.Data.Position = 0;

                    if (_converterStreamTypes.Contains(content.ContentType)) {
                        var converted = Convert (content);
                        if (converted != null) {
                            CloseContent (content, IsStreamOwner);
                            content = converted;
                            IsStreamOwner = true;
                        }
                    }
                    display.Data = Image.FromStream (content.Data);
                } else {
                    display.Data = null;
                }

            }
            CloseContent (content, IsStreamOwner);
        }

        public override void Save (Content<Stream> content) { }

        public override void Dispose () {
            _display?.Dispose ();
            _display = null;
        }

        public override void Clear () {
            base.Clear ();
            if (_display != null) {
                _display.Data = null;
            }
        }
    }
}