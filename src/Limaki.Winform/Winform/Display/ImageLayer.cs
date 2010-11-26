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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Drawing.UI;

namespace Limaki.Winform.Displays {
    public class ImageLayer:Layer<Image> {

        //public ImageLayer(IZoomTarget zoomTarget, IScrollTarget scrollTarget) : base(zoomTarget, scrollTarget) { }

        public ImageLayer(ICamera camera) : base(camera) { }

        public override Image Data {
            get { return _data; }
            set {
                bool refresh = value != _data;
                if (refresh) {
                    DisposeData ();
                    if (value != null) {
                        _data = OptimizedImage (value);
                        isDataOwner = _data != value;
                    } 
                    DataChanged();
                }
            }
        }


        /// <summary>
        /// returns an optimized image for fast drawing
        /// to have a fast Graphis.DrawImage, a Bitmap should be in PixelFormat.Format32bppPArgb
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Image OptimizedImage(Image source) {
            if (source.PixelFormat == PixelFormat.Format32bppPArgb) {
                return source;
            } else {
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
            }
        }


        public override void DataChanged() {
            if (Data != null) {
                _size = GDIConverter.Convert(Data.Size);
            } else {
                _size = SizeI.Empty;
            }
        }

        #region IPaintAction Member
        /// <summary>
        /// this is for perfomance testing, will be removed
        /// </summary>
        public enum DrawMode {
            /// <summary>
            /// with mono with a slow graphic card, this is the fastest.
            /// with ms.net, same as others
            /// </summary>
            ClipRect,
            /// <summary>
            /// slow with mono, as slow as FullImage.
            /// with ms.net, same as others
            /// </summary>
            ImageRect,
            /// <summary>
            /// slow with mono, as slow as ImageRect
            /// with ms.net, same as others
            /// </summary>
            FullImage
        }

        /// <summary>
        /// this is for perfomance testing, will be removed
        /// </summary>
        public DrawMode drawMode = DrawMode.ClipRect;

        public override void OnPaint(IPaintActionEventArgs e) {
            if (Data != null) {
                Graphics g = ((GDISurface) e.Surface).Graphics;
                g.Transform = ((GDIMatrice)this.Camera.Matrice).Matrix;
                g.InterpolationMode = InterpolationMode.Low;
                g.CompositingMode = CompositingMode.SourceCopy;
                g.CompositingQuality = CompositingQuality.HighSpeed;

                if (drawMode == DrawMode.FullImage) {
                    g.DrawImage (Data, new Point (0, 0));
                } else {
                    RectangleS rc = RectangleS.Empty;
                    if (drawMode == DrawMode.ImageRect) {
                        rc = new RectangleS(0, 0, Size.Width, Size.Height);
                    } else if (drawMode == DrawMode.ClipRect) {
                        // g.VisibleClipBounds  is same as ToSource (e.ClipRectangle)
                        // under MS.NET
                        // with mono, it seems to be ToSource(Control.Bounds)

                        // rc = g.VisibleClipBounds; 
                        // lets transform ourself, much faster under mono:
                        rc = Camera.ToSource (e.ClipRectangle);
                        
                        // this is trace code for mono:
                        //System.Console.WriteLine("transformed ClipRectangle" + rc + " , VisibleClipBounds " +
                        //                          g.VisibleClipBounds + " ClipRectangle " + e.ClipRectangle);


                        rc.Intersect(new RectangleS(0, 0, Size.Width, Size.Height));
                        rc.Inflate(new SizeS(1, 1));
                    }
                    g.DrawImage (Data, rc.Location.X, rc.Location.Y,
                        GDIConverter.Convert(rc), 
                        GraphicsUnit.Pixel);
                    //.DrawImage(Image,rc,rc,GraphicsUnit.Pixel);
                }
                g.Transform.Reset ();
            }
        }

        #endregion


    }
}