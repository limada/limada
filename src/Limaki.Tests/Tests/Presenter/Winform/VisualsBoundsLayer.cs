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


using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Presenter.UI;
using Limaki.Visuals;
using Limaki.Drawing.GDI;

namespace Limaki.Tests.Presenter.Winform {
    public class VisualsBoundsLayer : GraphSceneLayer<IVisual,IVisualEdge> {
        public VisualsBoundsLayer() : base() { }
        

        #region IPaintAction Member

        private System.Drawing.Pen _pen = null;
        System.Drawing.Pen pen {
            get {
                if (_pen == null) {
                    _pen = new System.Drawing.Pen(new System.Drawing.SolidBrush(System.Drawing.Color.Red));
                    _pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                }
                return _pen;
            }
        }

        private System.Drawing.Pen _datahullpen = null;
        System.Drawing.Pen datahullpen {
            get {
                if (_datahullpen == null) {
                    _datahullpen = new System.Drawing.Pen(new System.Drawing.SolidBrush(System.Drawing.Color.Green));
                    _datahullpen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                }
                return _datahullpen;
            }
        }

        private System.Drawing.Pen _hullpen = null;
        System.Drawing.Pen hullpen {
            get {
                if (_hullpen == null) {
                    _hullpen = new System.Drawing.Pen(new System.Drawing.SolidBrush(System.Drawing.Color.Blue));
                    _hullpen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                }
                return _hullpen;
            }
        }

        public override void OnPaint(IRenderEventArgs e) {
            var g = ((GDISurface)e.Surface).Graphics;
            var save = g.Transform;
            g.Transform = new System.Drawing.Drawing2D.Matrix();
            System.Drawing.Drawing2D.GraphicsPath hullPath = 
                new System.Drawing.Drawing2D.GraphicsPath ();

            var layout = this.Layout ();
            foreach (var visual in Data.Elements) {
                PointI[] datahull = null;
                RectangleI r = Camera.FromSource(visual.Shape.BoundsRect);
                g.DrawRectangle(pen, GDIConverter.Convert(r));

                if (Layout != null) {
                    datahull = layout.GetDataHull(visual, Camera.Matrice, 0, true);
                }
                //Point[] hull = visual.Shape.Hull(Transformer.Matrice,15, true);

                PointI[] hull = null;
                if (visual.Shape is VectorShape && Layout != null) {

                }
                
                if (datahull != null)
                    g.DrawPolygon(datahullpen, GDIConverter.Convert( datahull));

                if (hull != null)
                    g.DrawPolygon(hullpen, GDIConverter.Convert(hull));
            }
            g.Transform = save;
        }

        #endregion

        #region IAction Member


        #endregion

        public override void DataChanged() {

        }

    }
}