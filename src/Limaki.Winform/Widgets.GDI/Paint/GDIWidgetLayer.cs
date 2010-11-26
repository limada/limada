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
using System.Drawing.Text;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Drawing.UI;

namespace Limaki.Widgets.Paint {
    public class GDIWidgetLayer : WidgetLayerBase {
        //public GDIWidgetLayer(IZoomTarget zoomTarget, IScrollTarget scrollTarget):base(zoomTarget,scrollTarget){}
        public GDIWidgetLayer(ICamera camera) : base(camera) { }

        #region Renderer
        public GDISceneRenderer sceneRenderer = new GDISceneRenderer(null);
        #endregion

        #region IPaintAction Member
        public bool AntiAlias = true;
        public override void OnPaint(IPaintActionEventArgs e) {
            Graphics g = ((GDISurface)e.Surface).Graphics;

            var transform = (GDIMatrice)this.Camera.Matrice;
            g.Transform = transform.Matrix;

            if (AntiAlias) {
                // this is hiqh quality mode:
                g.SmoothingMode = SmoothingMode.HighQuality; //.AntiAlias;//.HighQuality;//.HighSpeed;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;//.SystemDefault;//.AntiAliasGridFit;//.ClearTypeGridFit:this is slowest on mono;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic; // 
            } else {
                // this is speed - mode, best compromise, not highspeed:
                g.SmoothingMode = SmoothingMode.HighSpeed; // .none is fastest
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                g.InterpolationMode = InterpolationMode.Low;
            }
            //e.Graphics.TextContrast = 12; // 0..12

            g.CompositingMode = CompositingMode.SourceOver;


            sceneRenderer.Scene = this.Data;
            sceneRenderer.Layout = this.Layout;

            //Region region = e.ClipRegion;
            //if (region != null) {
            //    Matrice matrix = transform.Clone();
            //    matrix.Invert ();
            //    region.Transform(matrix.Matrix);
            //}

            sceneRenderer.Render(e.Surface);

            g.Transform.Reset();
        }

        #endregion

    }
}