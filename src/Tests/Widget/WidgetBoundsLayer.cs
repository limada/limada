/*
 * Limaki 
 * Version 0.063
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

namespace Limaki.Tests.Widget {
    public class WidgetBoundsLayer : Layer<Scene> {
        public WidgetBoundsLayer(IZoomTarget zoomTarget, IScrollTarget scrollTarget) : base(zoomTarget, scrollTarget) { }
        
        public override Scene Data {
            get { return _data; }
            set { _data = value; }
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


        public override void OnPaint(PaintActionEventArgs e) {
            Graphics g = e.Graphics;
            Matrix save = e.Graphics.Transform;
            g.Transform = new Matrix();
            foreach (IWidget widget in Data.Widgets) {
                Rectangle r = Transformer.FromSource(widget.Shape.BoundsRect);
                g.DrawRectangle(pen, r);
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