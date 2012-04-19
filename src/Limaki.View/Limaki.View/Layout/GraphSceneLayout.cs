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

using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Xwt;

namespace Limaki.View.Layout {

    public abstract class GraphSceneLayout<TItem, TEdge> : Layout<TItem>, IGraphSceneLayout<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public GraphSceneLayout(Get<IGraphScene<TItem, TEdge>> dataHandler, IStyleSheet styleSheet)
            : base(styleSheet) {
            this.DataHandler = dataHandler;
            this.Orientation = Drawing.Orientation.LeftRight;
            this.Centered = true;
        }

        public Get<IGraphScene<TItem, TEdge>> DataHandler { get; set; }

        public virtual IGraphScene<TItem, TEdge> Data {
            get { return DataHandler(); }
        }

        public Drawing.Orientation Orientation { get; set; }

        public bool Centered { get; set; }

        public IRouter<TItem, TEdge> Router { get; set; }

        protected virtual void InvokeEdges() {
            var scene = this.Data;
            if (scene != null) {
                var graph = scene.Graph;
                foreach (TItem item in graph) {
                    if (!(item is TEdge)) {
                        foreach (TEdge edge in graph.Twig(item)) {
                            Invoke(edge);
                            Justify(edge);
                        }

                    } else {
                        Invoke(item);
                    }
                }
            }
        }

        public override void Invoke() {
            var scene = this.Data;
            if (scene != null) {
                // init spatialIndex:
                scene.SpatialIndex.Query(Rectangle.Zero);
                var graph = scene.Graph;
                foreach (TItem item in graph) {
                    Invoke(item);
                    if (!(item is TEdge)) {
                        Justify(item);
                    }
                }
                InvokeEdges();
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
        protected static IDrawingUtils drawingUtils {
            get {
                if (_drawingUtils == null) {
                    _drawingUtils = Registry.Factory.Create<IDrawingUtils>();
                }
                return _drawingUtils;
            }
        }

    }
}