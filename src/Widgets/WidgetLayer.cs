/*
 * Limaki 
 * Version 0.064
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
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Drawing.Painters;

namespace Limaki.Widgets {

    public class WidgetLayer : Layer<Scene> {
        public WidgetLayer(IZoomTarget zoomTarget, IScrollTarget scrollTarget)
            : base(zoomTarget, scrollTarget) {
            Priority = ActionPriorities.LayerPriority;
        }

        public override Scene Data {
            get { return _data; }
            set {
                bool refresh = value != _data;
                if (refresh) {
                    DisposeData();
                    if (value != null) {
                        isDataOwner = _data != value;
                        _data = value;
                    }

                    DataChanged();
                }
            }
        }

        public override Size Size {
            get {
                if (Data != null) {
                    return Data.Shape.Size;
                } else {
                    return Size.Empty;
                }
            }
            set {
                base.Size = value;
            }
        }

        public override void DataChanged() { }

        private PainterFactory _painterFactory = null;
        public PainterFactory PainterFactory {
            get { return _painterFactory; }
            set { _painterFactory = value; }
        }

        private ILayout<Scene, IWidget> _layout = null;
        public ILayout<Scene, IWidget> Layout {
            get { return _layout; }
            set { _layout = value; }
        }



        #region Renderer

        public SceneRenderer sceneRenderer = new SceneRenderer(null);
        #endregion

        #region IPaintAction Member
        public bool AntiAlias = true;
        public override void OnPaint(PaintActionEventArgs e) {
            Graphics g = e.Graphics;

            Matrice transform = this.Transformer.Matrice;
            g.Transform = transform.Matrix;

            if (AntiAlias) {
                // this is hiqh quality mode:
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality; //.AntiAlias;//.HighQuality;//.HighSpeed;
                e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;//.SystemDefault;//.AntiAliasGridFit;//.ClearTypeGridFit:this is slowest on mono;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic; // 
            } else {
                // this is speed - mode, best compromise, not highspeed:
                e.Graphics.SmoothingMode = SmoothingMode.HighSpeed; // .none is fastest
                e.Graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                e.Graphics.InterpolationMode = InterpolationMode.Low;
            }
            //e.Graphics.TextContrast = 12; // 0..12

            e.Graphics.CompositingMode = CompositingMode.SourceOver;


            sceneRenderer.PainterFactory = this.PainterFactory;
            sceneRenderer.Scene = this.Data;
            sceneRenderer.Layout = this.Layout;

            //if (e.ClipPath != null)
            //    sceneRenderer.Render(g,e.ClipPath);
            //else
            Region region = e.ClipRegion;
            if (region != null) {
                Matrice matrix = transform.Clone();
                matrix.Invert ();
                region.Transform(matrix.Matrix);
                
            }
            sceneRenderer.Render(g, region);

            g.Transform.Reset();
        }

        #endregion


    }
}