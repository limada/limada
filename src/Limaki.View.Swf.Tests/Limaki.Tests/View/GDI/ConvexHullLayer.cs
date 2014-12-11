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
 * http://www.limada.org
 * 
 */


using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Limaki.Actions;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.View.GdiBackend;
using Limaki.Drawing.Shapes;
using Limaki.View;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Rendering;
using Xwt;
using Point = System.Drawing.Point;
using Xwt.GdiBackend;
using Xwt.Drawing;
using Matrix = Xwt.Drawing.Matrix;
using Limaki.View.GraphScene;

namespace Limaki.Tests.View.GDI {
    /// <summary>
    /// does not work at the moment
    /// change it to an IReceiver
    /// </summary>
    public class ConvexHullLayer : GraphSceneLayer<IVisual, IVisualEdge>, IReceiver {
        public ConvexHullLayer(): base() {
            this.Priority = ActionPriorities.DataLayerPriority - 500;
        }

        public override void DataChanged() {}

        private SolidBrush regionBrush = 
            new SolidBrush(System.Drawing.Color.FromArgb(100, 
                                                         System.Drawing.Color.Fuchsia));
        private SolidBrush clipregionBrush = 
            new SolidBrush(System.Drawing.Color.FromArgb(100, 
                                                         System.Drawing.Color.Plum));

        private System.Drawing.Pen rectPen = new System.Drawing.Pen(System.Drawing.Color.Green);
        private System.Drawing.Pen pathPen = new System.Drawing.Pen(System.Drawing.Color.Red);
        private System.Drawing.Pen backGroundPen = new System.Drawing.Pen(System.Drawing.Color.White);
        private GraphicsPath invPath = new GraphicsPath();

        Point[] GetHull(IGraphScene<IVisual,IVisualEdge> scene, Matrix matrix, int delta) {
            Point[] result = new Point[0];
            var points = new Set<Xwt.Point> ();
            foreach(var visual in scene.Elements) {
                foreach(var p in visual.Shape.Hull(matrix,0,true)) {
                    if (!points.Contains(p))
                        points.Add (p);
                }
            }
            var resultI =  new GrahamConvexHull ().FindHull (points).ToArray();
            return Array.ConvertAll<Xwt.Point, Point> (resultI, a => a.ToGdi());
        }

        private int tolerance = 1;
        Point[] CommandsHull = new Point[0];

        void IReceiver.Perform() {
            var points = new Set<Xwt.Point>();
            var matrix = new Matrix(this.Camera.Matrix);
            var layout = this.Layout ();
            if (Data != null && Data.Requests.Count != 0) {
                foreach (var command in Data.Requests) {
                    if (command != null && command.Subject != null) {
                        if (command.Subject.Shape != null) {
                            var hull = command.Subject.Shape.Hull (tolerance, true);
                            points.AddRange(hull);
                        }
                        if (command is StateChangeCommand<IVisual>) {
                            var hull = layout.GetDataHull(
                                command.Subject, ( (StateChangeCommand<IVisual>) command ).Parameter.One,
                                tolerance, true);
                            points.AddRange(hull);
                        } else {
                            var hull = layout.GetDataHull(command.Subject, tolerance, true);
                            points.AddRange(hull);
                        }
                    }
                }
            }

            //points = points.Distinct ().ToList();
            if (points.Count > 2) {
                var resultI = new GrahamConvexHull().FindHull(points).ToArray();
                //matrix.Invert ();
                matrix.Transform (resultI);
                CommandsHull = Array.ConvertAll<Xwt.Point, Point>(resultI, a => a.ToGdi() );
            } else {
                CommandsHull = new Point[0] ;
            }
        }

        public virtual Point[] ClipHull(IRenderEventArgs e) {
            var hull = e.Clipper.Hull.ToArray();
            var result = new Point[hull.Length];

            result = Array.ConvertAll<Xwt.Point, Point> (hull, a => a.ToGdi());

            return result;
        }

        Point[] oldHull = null;
        public override void OnPaint(IRenderEventArgs e) {
            var hull = 
            //CommandsHull;
            //hull = ClipHull(e);
            GetHull(this.Data, Camera.Matrix, 5);

            Graphics g = ((GdiSurface)e.Surface).Graphics;
            System.Drawing.Drawing2D.Matrix save = g.Transform;
            g.Transform = new System.Drawing.Drawing2D.Matrix();

            if (oldHull != null && oldHull.Length > 2) {
                invPath.Reset();
                invPath.FillMode = FillMode.Alternate;

                invPath.AddPolygon(oldHull);
                g.DrawPath(backGroundPen, invPath);
                //g.FillPath (regionBrush, invPath);

                oldHull = null;
            }

            if (hull != null && hull.Length > 2) {
                invPath.Reset();
                invPath.FillMode = FillMode.Alternate;

                invPath.AddPolygon(hull);
                g.DrawPath(pathPen, invPath);
                //g.FillPath (regionBrush, invPath);

                g.Transform = save;
                oldHull = hull;
            }
            g.Transform = save;
        }

        void IReceiver.Reset() { }

        void IReceiver.Finish() { }

    }
}