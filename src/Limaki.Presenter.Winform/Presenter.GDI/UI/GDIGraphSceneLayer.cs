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
 * http://limada.sourceforge.net
 * 
 */

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Limaki.Drawing.GDI;
using Limaki.Graphs;
using Limaki.Presenter.Rendering;
using Limaki.Presenter.UI.GraphScene;


namespace Limaki.Presenter.GDI.UI {
    public class GDIGraphSceneLayer<TItem, TEdge> : GraphSceneLayer<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {
        
        public bool AntiAlias = true;
        
        public override void OnPaint(IRenderEventArgs e) {
            var g = ((GDISurface)e.Surface).Graphics;

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

            this.Renderer.Render (this.Data, e);

            g.Transform.Reset();
        }

        public override void DataChanged() {}
        }
}