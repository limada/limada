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
using Limaki.Actions;
using Limaki.Common;
using System.Drawing.Drawing2D;
using Limaki.Drawing.Shapes;
using System;
using Limaki.Drawing;

namespace Limaki.Tests.Drawing {
    public class RegionSandbox : Layer<Empty> {
        public override Empty Data {
            get { return default(Empty); }
            set { }
        }

        public RegionSandbox ( IZoomTarget zoomTarget, IScrollTarget scrollTarget )
            : base(zoomTarget, scrollTarget) {
            this.Priority = ActionPriorities.LayerPriority - 500;
        }

        public override void DataChanged () { }
        private SolidBrush regionBrush = new SolidBrush(Color.FromArgb(100, Color.Fuchsia));
        private SolidBrush clipregionBrush = new SolidBrush(Color.FromArgb(100, Color.Plum));

        private Pen rectPen = new Pen(Color.Green);
        private Pen pathPen = new Pen(Color.Red);
        Region region = new Region();
        private Point start = new Point(200, 100);
        private GraphicsPath invPath = new GraphicsPath();

        Point[] GetHull(VectorShape shape, Matrice matrix, int delta) {
            Vector vector = shape.Data;
            //vector.Transform(matrix);
            //return vector.PolygonHull(delta, true);

            Point[] oldHull = vector.Hull(delta, true);
            matrix.TransformPoints(oldHull);
            return oldHull;
        }

       
        void PaintWidenPolygon ( Graphics g, Point start, Size size, int i ) {
            invPath.Reset();
            VectorShape shape = new VectorShape(new Vector(start, start + size));
            invPath.AddLine(shape.Start, shape.End);
            pathPen.EndCap = LineCap.ArrowAnchor;
            
            g.DrawPath(pathPen, invPath);
            Point count = shape[Anchor.MostLeft];
            g.DrawString(i.ToString(), SystemFonts.StatusFont, SystemBrushes.Control, count);

            invPath.Reset();
            invPath.AddPolygon(GetHull(shape,Transformer.Matrice,2));
            Matrix save = g.Transform;
            g.Transform = new Matrix ();
            g.DrawPath(pathPen, invPath);
            g.Transform = save;

            
        }
        public  void OnPaintHullTest ( PaintActionEventArgs e ) {

            Graphics g = e.Graphics;
            Matrix save = g.Transform;
            g.Transform = Transformer.Matrice.Matrix;

            Point start = new Point(200, 100);
            Size size = new Size(0, -100);
            Size distance = new Size(30, 30);
            PaintWidenPolygon(g, start, size, 1);

            start += distance;
            size = new Size(0, 100);
            PaintWidenPolygon(g, start, size, 2);

            start += distance;
            size = new Size(100, 0);
            PaintWidenPolygon(g, start, size, 3);

            start += distance;
            size = new Size(-100, 0);
            PaintWidenPolygon(g, start, size, 4);

            // second

            start += distance;
            size = new Size(10, -100);
            PaintWidenPolygon(g, start, size, 5);

            start += distance;
            size = new Size(10, 100);
            PaintWidenPolygon(g, start, size, 6);

            start += distance;
            size = new Size(100, 10);
            PaintWidenPolygon(g, start, size, 7);

            start += distance;
            size = new Size(-100, 10);
            PaintWidenPolygon(g, start, size, 8);

            // third

            start += distance;
            size = new Size(-10, -100);
            PaintWidenPolygon(g, start, size, 9);

            start += distance;
            size = new Size(-10, 100);
            PaintWidenPolygon(g, start, size, 10);

            start += distance;
            size = new Size(100, -80);
            PaintWidenPolygon(g, start, size, 11);

            start += distance;
            size = new Size(-100, -10);
            PaintWidenPolygon(g, start, size, 12);
            g.Transform = save;

        }


        public override void OnPaint(PaintActionEventArgs e) {
            Graphics g = e.Graphics;
            Matrix save = g.Transform;
            g.Transform = Transformer.Matrice.Matrix;

            Rectangle smaller = new Rectangle(start, new Size(50, 20));
            Rectangle bigger = new Rectangle(start, new Size(55, 25));

            g.DrawRectangle(rectPen, smaller);
            g.DrawRectangle(rectPen, bigger);

            smaller.Inflate(-5, -5);
            bigger.Inflate(5, 5);
            Rectangle[] pathRects = {
                                        Rectangle.FromLTRB (bigger.Left, bigger.Top, bigger.Right, smaller.Top),
                                        Rectangle.FromLTRB (bigger.Left, smaller.Bottom, bigger.Right, bigger.Bottom),
                                        Rectangle.FromLTRB (bigger.Left, smaller.Top, smaller.Left, smaller.Bottom),
                                        Rectangle.FromLTRB (smaller.Right, smaller.Top, bigger.Right, smaller.Bottom)
                                    };

            Region clipRegion = new Region ();
            clipRegion.MakeInfinite ();
            clipRegion.Intersect(
    Rectangle.FromLTRB(bigger.Left, bigger.Top, bigger.Right, smaller.Top));
            clipRegion.Union(
                Rectangle.FromLTRB(bigger.Left, smaller.Bottom, bigger.Right, bigger.Bottom));
            clipRegion.Union(
                Rectangle.FromLTRB(bigger.Left, smaller.Top, smaller.Left, smaller.Bottom));
            clipRegion.Union(
                Rectangle.FromLTRB(smaller.Right, smaller.Top, bigger.Right, smaller.Bottom));


            bool isVisible = false;
            RectangleF testRect = Rectangle.Inflate (smaller, -1, -1);
            isVisible = clipRegion.IsVisible (testRect);
            if (isVisible) {
                clipRegion.MakeInfinite ();
            }
            invPath.Reset();
            invPath.FillMode = FillMode.Alternate;
            invPath.AddRectangles(pathRects);
            g.DrawPath(pathPen, invPath);
            g.FillPath(regionBrush, invPath);

        }

        public void OnPaintTest2 ( PaintActionEventArgs e ) {
            Graphics g = e.Graphics;
            Matrix save = g.Transform;
            g.Transform = Transformer.Matrice.Matrix;

            Rectangle smaller = new Rectangle(start, new Size(50, 20));
            Rectangle bigger = new Rectangle(start, new Size(55, 25));
            Rectangle rect3 = new Rectangle(start, new Size(10, 10));

            g.DrawRectangle(rectPen, smaller);
            g.DrawRectangle(rectPen, bigger);

            smaller.Inflate(-5, -5);
            bigger.Inflate(5, 5);

            //region.MakeEmpty ();
            region.MakeEmpty();

            invPath.Reset();
            invPath.FillMode = FillMode.Alternate;

            smaller.Intersect(bigger);
            //region.Intersect (smaller);

            //invPath.StartFigure ();
            invPath.AddRectangle(bigger);
            //invPath.CloseFigure();
            //invPath.StartFigure();
            invPath.AddRectangle(smaller);
            //invPath.CloseFigure();

            // this is to simulage g.Clip, as it has the bounds-rectangle:
            region.Union(invPath.GetBounds());
            region.Intersect(invPath);

            Region saveRegion = g.Clip;
            g.Clip = region;
            g.DrawPath(pathPen, invPath);
            //g.FillPath (regionBrush, invPath);
            g.FillRegion(regionBrush, g.Clip);
            if ( !region.IsVisible(rect3) ) {
                int x = 1;
            }
            //g.FillRegion (clipregionBrush, g.Clip);
            g.Transform = save;
            g.Clip = saveRegion;
        }
    }
}