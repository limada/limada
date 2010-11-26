/*
 * Limaki 
 * Version 0.07
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
using Limaki.Actions;
using Limaki.Widgets;
using Limaki.Drawing.Shapes;
using System;

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

        private Pen _pen = null;
        Pen pen {
            get {
                if (_pen == null) {
                    _pen = new Pen(new SolidBrush(Color.Red));
                    _pen.DashStyle = DashStyle.Dot;
                }
                return _pen;
            }
        }

        private Pen _datahullpen = null;
        Pen datahullpen {
            get {
                if (_datahullpen == null) {
                    _datahullpen = new Pen(new SolidBrush(Color.Green));
                    _datahullpen.DashStyle = DashStyle.Dot;
                }
                return _datahullpen;
            }
        }

        private Pen _hullpen = null;
        Pen hullpen {
            get {
                if (_hullpen == null) {
                    _hullpen = new Pen(new SolidBrush(Color.Blue));
                    _hullpen.DashStyle = DashStyle.Dot;
                }
                return _hullpen;
            }
        }

        public override void OnPaint(PaintActionEventArgs e) {
            Graphics g = e.Graphics;
            Matrix save = e.Graphics.Transform;
            g.Transform = new Matrix();
            GraphicsPath hullPath = new GraphicsPath ();

            foreach (IWidget widget in Data.Elements) {
                Point[] datahull = null;
                Rectangle r = Camera.FromSource(widget.Shape.BoundsRect);
                g.DrawRectangle(pen, r);

                if (Layout != null) {
                    datahull = Layout.GetDataHull (widget, Camera.Matrice, 0, true);
                }
                //Point[] hull = widget.Shape.Hull(Transformer.Matrice,15, true);

                Point[] hull = null;
                if (widget.Shape is VectorShape && Layout != null) {

                }
                
                if (datahull != null)
                    g.DrawPolygon(datahullpen, datahull);

                if (hull != null)
                    g.DrawPolygon(hullpen, hull);
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