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

using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Xwt;
using System;

namespace Limaki.View.Viz.Modelling {

    public abstract class GraphSceneLayout<TItem, TEdge> : Layout<TItem>, IGraphSceneLayout<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public GraphSceneLayout(Func<IGraphScene<TItem, TEdge>> dataHandler, IStyleSheet styleSheet): base(styleSheet) {
            this.DataHandler = dataHandler;
            this.Dimension = Dimension.X;
            this.Centered = true;
            this.AlignOnReset = true;
        }

        public Func<IGraphScene<TItem, TEdge>> DataHandler { get; set; }

        public virtual IGraphScene<TItem, TEdge> Data {
            get { return DataHandler(); }
        }

        public Dimension Dimension { get; set; }
        public bool Centered { get; set; }

        public bool AlignOnReset { get; set; }

        public IEdgeRouter<TItem, TEdge> EdgeRouter { get; set; }

        protected virtual void PerformEdges() {
            var scene = this.Data;
            if (scene != null) {
                var graph = scene.Graph;
                foreach (var item in graph) {
                    if (!(item is TEdge)) {
                        foreach (var edge in graph.Twig(item)) {
                            Perform(edge);
                            Justify(edge);
                        }

                    } else {
                        Perform(item);
                    }
                }
            }
        }

        public override void Reset () {
            var scene = Data;
            
            if (scene != null) {
                scene.SpatialIndex.Query(Rectangle.Zero);
                var graph = scene.Graph;

                foreach (var item in graph) {
                    Perform(item);
                    if (!(item is TEdge)) {
                        Justify(item);
                    }
                }

                PerformEdges();

                if (AlignOnReset) {
                    var aligner = new Aligner<TItem, TEdge>(scene, this);
                    aligner.FullLayout(scene.Focused, new Point(Border.Width, Border.Height), this.Options(), this.Comparer);
                    aligner.Commit();
                } 
            }
        }

        public override IShape CreateShape(TItem item) {
            if (item is TEdge) {
                return ShapeFactory.Create<IVectorShape>();
            } else {
                return ShapeFactory.Create<IRoundedRectangleShape>();
            }
        }

        static IDrawingUtils _drawingUtils = null;
        protected static IDrawingUtils DrawingUtils { get { return _drawingUtils ?? (_drawingUtils = Registry.Factory.Create<IDrawingUtils>()); } }

        public virtual Size GetSize (object data, IStyle style) {
            if (data == null)
                data = "<<null>>";
            var size = Size.Zero;
            if (data is string) {
                size = DrawingUtils.GetTextDimension (data.ToString (), style);
            } else {
                size = DrawingUtils.GetObjectDimension (data, style);
            }
            size += new Size (style.Padding.HorizontalSpacing, style.Padding.VerticalSpacing);
            return size;
        }

        public abstract void AdjustSize (TItem item, IShape shape);

        public abstract void AdjustSize (TItem item);

    }
}