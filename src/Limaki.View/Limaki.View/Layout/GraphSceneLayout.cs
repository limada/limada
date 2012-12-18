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
using Limaki.Common.Linqish;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Xwt;
using Limaki.Graphs.Extensions;
using System.Linq;

namespace Limaki.View.Layout {

    public abstract class GraphSceneLayout<TItem, TEdge> : Layout<TItem>, IGraphSceneLayout<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public GraphSceneLayout(Get<IGraphScene<TItem, TEdge>> dataHandler, IStyleSheet styleSheet)
            : base(styleSheet) {
            this.DataHandler = dataHandler;
            this.Dimension = Drawing.Dimension.X;
            this.Centered = true;
            this.Align = true;
        }

        public Get<IGraphScene<TItem, TEdge>> DataHandler { get; set; }

        public virtual IGraphScene<TItem, TEdge> Data {
            get { return DataHandler(); }
        }

        public Drawing.Dimension Dimension { get; set; }

        public bool Centered { get; set; }

        public IEdgeRouter<TItem, TEdge> EdgeRouter { get; set; }

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

        public bool Align { get; set; }
        public override void Invoke () {
            var data = Data;
            
            if (data != null) {
                data.SpatialIndex.Query(Rectangle.Zero);
                var graph = data.Graph;
                foreach (TItem item in graph) {
                    Invoke(item);
                    if (!(item is TEdge)) {
                        Justify(item);
                    }
                }
                InvokeEdges();

                if (Align) {
                    var aligner = new Aligner<TItem, TEdge>(data, this);
                    var options = this.Options();

                    var pos = new Point(Border.Width, Border.Height);
                    var walker = new Walker<TItem, TEdge>(data.Graph);
                    var roots = data.Graph.FindRoots(data.Focused);
                    roots.ForEach(root => {
                        var walk = walker.DeepWalk(root, 1).Where(l => !(l.Node is TEdge)).ToArray();
                        var bounds = new Rectangle(pos, Size.Zero);
                        aligner.Columns(walk, ref bounds, options);
                        pos = new Point(pos.X, pos.Y + bounds.Size.Height + options.Distance.Height);
                    });

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
        protected static IDrawingUtils DrawingUtils {
            get {
                if (_drawingUtils == null) {
                    _drawingUtils = Registry.Factory.Create<IDrawingUtils>();
                }
                return _drawingUtils;
            }
        }

    }
}