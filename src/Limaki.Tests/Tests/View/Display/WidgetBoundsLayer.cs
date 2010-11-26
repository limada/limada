/*
 * Limaki 
 * Version 0.08
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
using Limaki.Drawing.GDI;
using Limaki.Drawing.Shapes;
using Limaki.Drawing.UI;
using Limaki.Widgets;

namespace Limaki.Tests.Widget {
    public class WidgetBoundsLayer : Layer<Scene> {
        public WidgetBoundsLayer(IZoomTarget zoomTarget, IScrollTarget scrollTarget) : base(zoomTarget, scrollTarget) { }
        
        public override Scene Data {
            get { return _data; }
            set { _data = value; }
        }

        private ILayout<Scene, IWidget> _layout = null;
        public ILayout<Scene, IWidget> Layout {
            get { return _layout; }
            set { _layout = value; }
        }

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

        public override void OnPaint(IPaintActionEventArgs e) {
            System.Drawing.Graphics g = ((GDISurface)e.Surface).Graphics;
            System.Drawing.Drawing2D.Matrix save = g.Transform;
            g.Transform = new System.Drawing.Drawing2D.Matrix();
            System.Drawing.Drawing2D.GraphicsPath hullPath = 
                new System.Drawing.Drawing2D.GraphicsPath ();

            foreach (IWidget widget in Data.Elements) {
                PointI[] datahull = null;
                RectangleI r = Camera.FromSource(widget.Shape.BoundsRect);
                g.DrawRectangle(pen, GDIExtensions.Native(r));

                if (Layout != null) {
                    datahull = Layout.GetDataHull (widget, Camera.Matrice, 0, true);
                }
                //Point[] hull = widget.Shape.Hull(Transformer.Matrice,15, true);

                PointI[] hull = null;
                if (widget.Shape is VectorShape && Layout != null) {

                }
                
                if (datahull != null)
                    g.DrawPolygon(datahullpen, GDIExtensions.Native( datahull));

                if (hull != null)
                    g.DrawPolygon(hullpen, GDIExtensions.Native(hull));
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