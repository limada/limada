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

using System;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Drawing.UI;

namespace Limaki.Widgets.Layout {
    public class WidgetLayout<TData, TItem> : Layout<TData, TItem>
        where TData :  Scene
        where TItem :  IWidget {
        public WidgetLayout(Func<TData> handler, IStyleSheet stylesheet)
            : base(handler, stylesheet) { }


        protected Orientation _orientation = Orientation.LeftRight;
        public Orientation Orientation {
            get { return _orientation; }
            set { _orientation = value; }
        }

        protected bool _centered = true;
        public bool Centered {
            get { return _centered; }
            set { _centered = value; }
        }

        private IRouter _router = new NearestAnchorRouter();
        public IRouter Router {
            get { return _router; }
            set { _router = value; }
        }


        protected virtual void InvokeEdges() {
            Scene scene = this.Data as Scene;
            if (scene != null) {
                var graph = scene.Graph;
                foreach (IWidget widget in graph) {
                    if (!(widget is IEdgeWidget)) {
                        foreach (IEdgeWidget edge in graph.Twig(widget)) {
                            Invoke((TItem)edge);
                            Justify((TItem)edge);
                        }

                    } else {
                        Invoke((TItem)widget);
                    }
                }
            }
        }

        public override void Invoke() {
            Scene scene = this.Data as Scene;
            if (scene != null) {
                // init spatialIndex:
                scene.SpatialIndex.Query (RectangleS.Empty);
                var graph = scene.Graph;
                foreach (IWidget widget in graph) {
                    Invoke((TItem)widget);
                    if (!(widget is IEdgeWidget)) {
                        Justify((TItem)widget);
                    }
                }
                InvokeEdges();
            }
        }

        public override IShape CreateShape(TItem item) {
            if (item is IEdgeWidget) {
                return ShapeFactory.One<IVectorShape>();
            } else {
                return ShapeFactory.One<IRoundedRectangleShape>();
            }
        }

        public override bool Invoke(TItem item) {
            if (item.Shape == null) {
                item.Shape = CreateShape(item);
                return true;
            }
            return false;
        }

        public override bool Invoke(TItem item, IShape shape) {
            if (item.Shape != shape) {
                item.Shape = shape;
                return true;
            }
            return false;
        }

        public override IStyle GetStyle(TItem item) {
            Scene scene = this.Data as Scene;
            IStyle style = (item.Style == null ? this.StyleSheet : item.Style);
            bool isSelected = scene.Selected.Contains(item);
            if (item is IEdgeWidget) {
                if (isSelected) {
                    style = StyleSheet.EdgeSelectedStyle;
                } else if (item.Equals(scene.Hovered)) {
                    style = StyleSheet.EdgeHoveredStyle;
                } else {
                    style = StyleSheet.EdgeStyle;
                }
            } else {
                if (isSelected) {
                    style = StyleSheet.SelectedStyle;
                } else if (item.Equals(scene.Hovered)) {
                    style = StyleSheet.HoveredStyle;
                } else {
                    style = StyleSheet.DefaultStyle;
                }
            }
            return style;
        }

        public override IStyle GetStyle(TItem item, UiState uiState) {
            IStyle style = (item.Style == null ? this.StyleSheet : item.Style);
            if (item is IEdgeWidget) {
                if (uiState==UiState.Selected||uiState==UiState.Focus) {
                    style = StyleSheet.EdgeSelectedStyle;
                } else if (uiState == UiState.Hovered) {
                    style = StyleSheet.EdgeHoveredStyle;
                } else {
                    style = StyleSheet.EdgeStyle;
                }
            } else {
                if (uiState == UiState.Selected) {
                    style = StyleSheet.SelectedStyle;
                } else if (uiState == UiState.Hovered) {
                    style = StyleSheet.HoveredStyle;
                } else {
                    style = StyleSheet.DefaultStyle;
                }
            }
            
            return style;
        }

        static IDrawingUtils _drawingUtils = null;
        protected static IDrawingUtils drawingUtils {
            get {
                if (_drawingUtils == null) {
                    _drawingUtils = Registry.Factory.One<IDrawingUtils>();
                }
                return _drawingUtils;
            }
        }

        public virtual void AjustSize(TItem widget, IShape shape) {
            if (!(widget is IEdgeWidget)) {
                IStyle style = GetStyle(widget);
                RectangleI invalid = shape.BoundsRect;
                SizeS textSize = drawingUtils.GetTextDimension(widget.Data.ToString(), style);
                SizeI size = SizeI.Add(SizeI.Ceiling(textSize), new SizeI(10, 10));
                if (shape is VectorShape) {
                    size.Height = 0;
                }
                shape.Size = size;
                Data.UpdateBounds(widget, invalid);
            }
        }

        public virtual void AjustSize(TItem widget) {
            if (!(widget is IEdgeWidget)) {
                RectangleI invalid = widget.Shape.BoundsRect;
                AjustSize (widget, widget.Shape);
                Data.UpdateBounds(widget, invalid);
            }
        }

        public override void Justify(TItem target, IShape tshape) {
            if ((target is IEdgeWidget) && (tshape is IEdgeShape)) {
                IEdgeWidget edge = (IEdgeWidget)target;
                Router.routeEdge(edge);

                IEdgeShape shape = (IEdgeShape)tshape;
                shape.Start = edge.Root.Shape[edge.RootAnchor];
                shape.End = edge.Leaf.Shape[edge.LeafAnchor];
            } else {
                AjustSize (target, tshape);
            }
        }

        public override void AddBounds(TItem target) {
            RectangleI invalid = target.Shape.BoundsRect;

            Justify(target, target.Shape);
            RectangleI valid = target.Shape.BoundsRect;
            if (valid.Equals(invalid)) {
                Data.RemoveBounds(target); // this is a workaround, should not be here!
                Data.AddBounds(target);
            } else {
                Data.UpdateBounds(target, invalid);
            }
        }

        public override void Justify(TItem target) {
            if ((target is IEdgeWidget) && (target.Shape is IEdgeShape)) {
                RectangleI invalid = target.Shape.BoundsRect;
                Justify (target, target.Shape);
                Data.UpdateBounds(target, invalid);
            } else {
                AjustSize(target, target.Shape);
            }
        }

        public override void Perform(TItem item) { }


        public virtual PointI Arrange(TItem root) {
            Justify(root);  
            return root.Location; 
        }

        protected virtual PointI[] GetDataHull(TItem item, IStyle style, Matrice matrix, int delta, bool extend) {
            PointI[] result = null;
            if (style.PaintData && item.Data != null && item.Data.GetType() != typeof(Empty)) {
                IDataPainter painter = this.GetPainter(item.Data.GetType()) as IDataPainter;
                if (painter != null) {
                    painter.Data = item.Data;
                    painter.Style = style;
                    painter.Shape = item.Shape;
                    result = painter.Measure(matrix, delta, extend);
                }
            }
            return result;
        }

        public override PointI[] GetDataHull(TItem item, Matrice matrix, int delta, bool extend) {
            return this.GetDataHull(item, GetStyle(item), matrix, delta, extend);
        }
        public override PointI[] GetDataHull(TItem item, UiState uiState, Matrice matrix, int delta, bool extend) {
            return this.GetDataHull(item, GetStyle(item,uiState),matrix,delta,extend);
        }

        public static IWidget Adjacent(IEdgeWidget edge, IWidget item) {
            if (item.Equals(edge.Root)) {
                return edge.Leaf;
            } else if (item.Equals(edge.Leaf)) {
                return edge.Root;
            }
            return null;
        }


    }
}