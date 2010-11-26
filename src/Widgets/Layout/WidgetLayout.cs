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

using System.Collections.Generic;
using System.Drawing;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Drawing.Painters;

namespace Limaki.Widgets.Layout {
    public class WidgetLayout<TData, TItem> : Layout<TData, TItem>
        where TData : Scene
        where TItem : class, IWidget {
        public WidgetLayout(Handler<TData> handler, IStyleSheet stylesheet)
            :
                base(handler, stylesheet) { }

        private IRouter _router = new NearestAnchorRouter();
        public IRouter Router {
            get { return _router; }
            set { _router = value; }
        }


        protected virtual void InvokeEdges() {
            Scene scene = this.Data as Scene;
            if (scene != null) {
                IGraph<IWidget, IEdgeWidget> graph = scene.Graph;
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
                IGraph<IWidget, IEdgeWidget> graph = scene.Graph;
                foreach (IWidget widget in graph) {
                    Invoke((TItem)widget);
                    if (!(widget is IEdgeWidget)) {
                        Justify((TItem)widget);
                    }
                }
                InvokeEdges();
            }
        }

        protected virtual IShape EdgeShape() {
            return new VectorShape();
        }
        protected virtual IShape WidgetShape() {
            return new RoundedRectangleShape();
        }

        public override void Invoke(TItem item) {
            if (item is IEdgeWidget) {
                if (item.Shape == null) {
                    item.Shape = EdgeShape();
                }
            } else {
                if (item.Shape == null) {
                    item.Shape = WidgetShape();
                }
            }
        }

        public override IStyle GetStyle(TItem item) {
            Scene scene = this.Data as Scene;
            IStyle style = (item.Style == null ? this.styleSheet : item.Style);
            bool isSelected = scene.Selected.Contains(item);
            if (item is IEdgeWidget) {
                if (isSelected) {
                    style = styleSheet.EdgeSelectedStyle;
                } else if (item == scene.Hovered) {
                    style = styleSheet.EdgeHoveredStyle;
                } else {
                    style = styleSheet.EdgeStyle;
                }
            } else {
                if (isSelected) {
                    style = styleSheet.SelectedStyle;
                } else if (item == scene.Hovered) {
                    style = styleSheet.HoveredStyle;
                } else {
                    style = styleSheet.DefaultStyle;
                }
            }
            return style;
        }

        public override IStyle GetStyle(TItem item, UiState uiState) {
            IStyle style = (item.Style == null ? this.styleSheet : item.Style);
            if (item is IEdgeWidget) {
                if (uiState==UiState.Selected||uiState==UiState.Focus) {
                    style = styleSheet.EdgeSelectedStyle;
                } else if (uiState == UiState.Hovered) {
                    style = styleSheet.EdgeHoveredStyle;
                } else {
                    style = styleSheet.EdgeStyle;
                }
            } else {
                if (uiState == UiState.Selected) {
                    style = styleSheet.SelectedStyle;
                } else if (uiState == UiState.Hovered) {
                    style = styleSheet.HoveredStyle;
                } else {
                    style = styleSheet.DefaultStyle;
                }
            }
            
            return style;
        }
        public virtual void AjustSize(TItem widget) {
            if (!(widget is IEdgeWidget)) {
                IStyle style = styleSheet.DefaultStyle;
                Rectangle invalid = widget.Shape.BoundsRect;
                SizeF textSize = ShapeUtils.GetTextDimension(
                    style.Font, widget.Data.ToString(), style.AutoSize);
                widget.Size = Size.Add(Size.Ceiling(textSize), new Size(10, 10));
                Data.UpdateBounds(widget, invalid);
            }
        }

        public override void Justify(TItem target) {
            if ((target is IEdgeWidget) && (target.Shape is IEdgeShape)) {
                Rectangle invalid = target.Shape.BoundsRect;
                IEdgeWidget edge = (IEdgeWidget)target;
                Invoke((TItem)edge.Root);
                Invoke((TItem)edge.Leaf);
                Router.routeEdge(edge);

                IEdgeShape shape = (IEdgeShape)target.Shape;
                shape.Start = edge.Root.Shape[edge.RootAnchor];
                shape.End = edge.Leaf.Shape[edge.LeafAnchor];
                Data.UpdateBounds(target, invalid);
            } else {
                AjustSize(target);
            }
        }

        public override void Perform(TItem item) { }

        protected virtual Point[] GetDataHull(TItem item, IStyle style, Matrice matrix, int delta, bool extend) {
            Point[] result = null;
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

        public override Point[] GetDataHull(TItem item, Matrice matrix, int delta, bool extend) {
            return this.GetDataHull(item, GetStyle(item), matrix, delta, extend);
        }
        public override Point[] GetDataHull(TItem item, UiState uiState, Matrice matrix, int delta, bool extend) {
            return this.GetDataHull(item, GetStyle(item,uiState),matrix,delta,extend);
        }
    }
}