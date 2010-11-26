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

using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;

namespace Limaki.Presenter.Layout {
    public abstract class GraphLayout<TItem, TEdge> : Layout<TItem>, IGraphLayout<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {
        public GraphLayout(Get<IGraphScene<TItem, TEdge>> dataHandler, IStyleSheet styleSheet)
            : base(styleSheet) {
            this.DataHandler = dataHandler;
            this.Orientation = Orientation.LeftRight;
            this.Centered = true;
        }

        public Get<IGraphScene<TItem, TEdge>> DataHandler { get; set; }

        public virtual IGraphScene<TItem, TEdge> Data {
            get { return DataHandler(); }
        }

        public Orientation Orientation { get; set; }
        public bool Centered { get; set; }

        public IRouter<TItem, TEdge> Router { get; set; }

        protected virtual void InvokeEdges() {
            var scene = this.Data;
            if (scene != null) {
                var graph = scene.Graph;
                foreach (TItem widget in graph) {
                    if (!(widget is TEdge)) {
                        foreach (TEdge edge in graph.Twig(widget)) {
                            Invoke(edge);
                            Justify(edge);
                        }

                    } else {
                        Invoke(widget);
                    }
                }
            }
        }

        public override void Invoke() {
            var scene = this.Data;
            if (scene != null) {
                // init spatialIndex:
                scene.SpatialIndex.Query(RectangleS.Empty);
                var graph = scene.Graph;
                foreach (TItem widget in graph) {
                    Invoke(widget);
                    if (!(widget is TEdge)) {
                        Justify(widget);
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