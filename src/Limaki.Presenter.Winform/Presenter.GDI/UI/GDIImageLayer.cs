using System.Drawing;
using Limaki.Presenter.UI;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System;
using Limaki.Drawing.GDI;
using Limaki.Drawing;
using Limaki.Common;

namespace Limaki.Presenter.GDI.UI {
    public class GDIImageLayer : Layer<Image> {

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

        public override void DataChanged() {
            var data = this.Data;
            if (data != null) {
                this.Size = GDIConverter.Convert(data.Size);
            } else {
                this.Size = SizeI.Empty;
            }
            hadError = false;
        }

        /// <summary>
        /// returns an optimized image for fast drawing
        /// to have a fast Graphis.DrawImage, a Bitmap should be in PixelFormat.Format32bppPArgb
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Image OptimizedImage(Image source) {
            if (source.PixelFormat == PixelFormat.Format32bppPArgb) {
                return source;
            } else {
                try {
                    Image result = new Bitmap(source.Width, source.Height, PixelFormat.Format32bppPArgb);

                    using (Graphics g = Graphics.FromImage(result)) {
                        g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                        g.CompositingMode = CompositingMode.SourceCopy;
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        // draws image too small:
                        // g.DrawImageUnscaled (source, 0, 0);
                        g.DrawImage(source, 0, 0, source.Width, source.Height);
                        g.Flush();
                    }
                    return result;

                } catch (Exception ex) {
                    ExceptionHandler.Catch(ex, MessageType.OK);
                    return source;
                }
            }
        }

        protected IExceptionHandler ExceptionHandler {
            get { return Registry.Pool.TryGetCreate<IExceptionHandler>(); }
        }

        protected virtual void DisposeCache() {
            if (isCacheOwner && (_cache != null) && (_cache is IDisposable)) {
                ((IDisposable)_cache).Dispose();
            }
            _cache = null;
        }

        bool hadError = false;
        public override void OnPaint(IRenderEventArgs e) {
            var data = this.Data;
            if (data != null && ! hadError) {
                try {
                    Graphics g = ((GDISurface)e.Surface).Graphics;
                    g.Transform = ((GDIMatrice)this.Camera.Matrice).Matrix;
                    g.InterpolationMode = InterpolationMode.Low;
                    g.CompositingMode = CompositingMode.SourceCopy;
                    g.CompositingQuality = CompositingQuality.HighSpeed;

                    RectangleS rc = Camera.ToSource(e.Clipper.Bounds);

                    rc.Intersect(new RectangleS(0, 0, Size.Width, Size.Height));
                    rc.Inflate(new SizeS(1, 1));

                    g.DrawImage(data, rc.Location.X, rc.Location.Y,
                        GDIConverter.Convert(rc),
                        GraphicsUnit.Pixel);


                    g.Transform.Reset();
                } catch (Exception ex) {
                    ExceptionHandler.Catch(ex, MessageType.OK);
                    hadError = true;
                }
            }
        }


    }
}