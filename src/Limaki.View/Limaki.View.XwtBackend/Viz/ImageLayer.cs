/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.XwtBackend;
using System;
using Limaki.View.Viz.Rendering;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend {

    public class ImageLayer : Layer<Image> {

        Image _cache = null;
        Image _value = null;
        bool isCacheOwner = false;

        public override Image Data {
            get {
                bool refresh = _value != base.Data;
                _value = base.Data;
                if (refresh) {
                    DisposeCache();
                    if (_value != null) {
                        _cache = OptimizedImage(_value);
                        isCacheOwner = _cache != _value;
                    }
                    DataChanged();
                }
                return _cache;
            }
        }

        public override void DataChanged () {
            var data = this.Data;
            if (data != null) {
                this.Size = data.Size;
            } else {
                this.Size = Size.Zero;
            }
            hadError = false;
        }

        /// <summary>
        /// returns an optimized image for fast drawing
        /// to have a fast Graphis.DrawImage, a Bitmap should be in PixelFormat.Format32bppPArgb
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [TODO]
        public Image OptimizedImage (Image source) {
            var bitmap = source as BitmapImage;
            if (bitmap == null || bitmap.Format == ImageFormat.ARGB32)
                return source;

            // TODO: make a ARGB32-Image here
            return source;

        }

        protected IExceptionHandler ExceptionHandler {
            get { return Registry.Pooled<IExceptionHandler>(); }
        }

        protected virtual void DisposeCache () {
            if (isCacheOwner && (_cache != null) && (_cache is IDisposable)) {
                ((IDisposable) _cache).Dispose();
            }
            _cache = null;
        }

        bool hadError = false;
        public override void OnPaint (IRenderEventArgs e) {
            var image = this.Data;
            if (image != null && !hadError && ! image.Size.IsZero) {
                var ctx = ((ContextSurface) e.Surface).Context;
                ctx.Save();
                try {
                    ctx.ModifyCTM(this.Camera.Matrix);

                    var rc = Camera.ToSource(e.Clipper.Bounds);
                    rc = rc.Intersect(new Xwt.Rectangle(0, 0, Size.Width, Size.Height));
                    rc = rc.Inflate(new Size(1, 1));

                    ctx.DrawImage(image, rc, rc, this.Alpha);

                } catch (Exception ex) {
                    ExceptionHandler.Catch(ex, MessageType.OK);
                    hadError = true;
                } finally {
                    ctx.Restore();
                }
            }
        }


    }
}