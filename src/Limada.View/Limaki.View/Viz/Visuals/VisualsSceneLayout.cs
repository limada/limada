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
using Limaki.View.Visuals;
using Limaki.View.Viz.Modelling;
using Xwt;
using Xwt.Drawing;
using System;
using Limaki.View.Common;

namespace Limaki.View.Viz.Visuals {

    public class VisualsSceneLayout<TItem, TEdge> : GraphSceneLayout<TItem, TEdge>
        where TItem :  IVisual
        where TEdge:  TItem, IEdge<TItem> {
        public VisualsSceneLayout(Func<IGraphScene<TItem, TEdge>> handler, IStyleSheet stylesheet)
            : base(handler, stylesheet) {
            
            this.EdgeRouter = new NearestAnchorRouter<TItem,TEdge>();
        }

        public override IShape GetShape (TItem item) {
            //if (item.Shape == null) {
            //    item.Shape = CreateShape(item);
            //}
            return item.Shape;
        }

        public override bool Perform(TItem item) {
            if (item.Shape == null) {
                item.Shape = CreateShape(item);
                return true;
            }
            return false;
        }

        public override bool Perform(TItem item, IShape shape) {
            if (item.Shape != shape) {
                item.Shape = shape;
                return true;
            }
            return false;
        }

        public override IStyle GetStyle(TItem item) {
            var scene = this.Data;
            var styleGroup = item.Style ?? (item is TEdge ? StyleSheet.EdgeStyle : StyleSheet.ItemStyle);
            if (scene.Selected.Contains(item))
                return styleGroup.SelectedStyle;
            if (item.Equals(scene.Hovered))
                return styleGroup.HoveredStyle;

            return styleGroup.DefaultStyle;
        }

        public override IStyle GetStyle(TItem item, UiState uiState) {
            var styleGroup = item.Style ?? (item is TEdge ? StyleSheet.EdgeStyle : StyleSheet.ItemStyle);

            if (uiState == UiState.Selected || uiState == UiState.Focus)
                return styleGroup.SelectedStyle;
            if (uiState == UiState.Hovered)
                return styleGroup.HoveredStyle;

            return styleGroup.DefaultStyle;
        }

        public override void AdjustSize(TItem visual, IShape shape) {
            if (!(visual is TEdge)) {
                var style = GetStyle(visual);
                var invalid = shape.BoundsRect;
                var size = GetSize (visual.Data, style);
                if (shape is VectorShape) {
                    size.Height = 0;
                }
                shape.DataSize = size;
                Data.UpdateBounds(visual, invalid);
            }
        }

        public override void AdjustSize (TItem visual) {
            if (!(visual is TEdge)) {
                //var invalid = visual.Shape.BoundsRect;
                AdjustSize (visual, visual.Shape);
                //Data.UpdateBounds(visual, invalid);
            }
        }

        public override void Justify(TItem target, IShape tshape) {
            if ((target is IVisualEdge) && (tshape is IEdgeShape)) {
                var edge = (IVisualEdge)target;
                EdgeRouter.RouteEdge((TEdge)edge);

                var shape = (IEdgeShape)tshape;
                var rootShape = edge.Root.Shape;
                var leafShape = edge.Leaf.Shape;
                if (rootShape != null)
                    shape.Start = rootShape[edge.RootAnchor];
                if (leafShape != null)
                    shape.End = leafShape[edge.LeafAnchor];
            } else {
                AdjustSize (target, tshape);
            }
        }

        public override void BoundsChanged(TItem target) {
            var invalid = target.Shape.BoundsRect;

            Justify(target, target.Shape);
            var valid = target.Shape.BoundsRect;
            if (valid.Equals(invalid)) {
                Data.RemoveBounds(target); // this is a workaround, should not be here!
                Data.AddBounds(target);
            } else {
                Data.UpdateBounds(target, invalid);
            }
        }

        public override void Justify(TItem target) {
            if ((target is TEdge) && (target.Shape is IEdgeShape)) {
                var invalid = target.Shape.BoundsRect;
                Justify (target, target.Shape);
                Data.UpdateBounds(target, invalid);
            } else {
                AdjustSize(target, target.Shape);
            }
        }

        public override void Refresh(TItem item) { }


        protected virtual Point[] GetDataHull(TItem item, IStyle style, Matrix matrix, int delta, bool extend) {
            Point[] result = null;
            if (style.PaintData && item.Data != null && item.Data.GetType() != typeof(Empty)) {
                var painter = this.GetPainter(item.Data.GetType()) as IDataPainter;
                if (painter != null) {
                    var autoSize = style.AutoSize;
                    var oldSize = autoSize;
                    if (item.Shape is IVectorShape) {
                        var vector = ((IVectorShape)item.Shape).Data;
                        autoSize.Width = (int)Vector.Length(vector);
                    }
                    style.AutoSize = autoSize;
                    painter.Data = item.Data;
                    painter.Style = style;
                    painter.OuterShape = item.Shape;
                    if (matrix == null) {
                        result = painter.Measure(delta, extend);
                    } else {
                        result = painter.Measure (matrix, delta, extend);
                    }
                    style.AutoSize = oldSize;
                }

            }
            return result;
        }

        public override Point[] GetDataHull(TItem item, Matrix matrix, int delta, bool extend) {
            return this.GetDataHull(item, GetStyle(item), matrix, delta, extend);
        }
        public override Point[] GetDataHull(TItem item, UiState uiState, Matrix matrix, int delta, bool extend) {
            return this.GetDataHull(item, GetStyle(item,uiState),matrix,delta,extend);
        }
        public override Point[] GetDataHull(TItem item, int delta, bool extend) {
            return this.GetDataHull(item, GetStyle(item), null, delta, extend);
        }
        public override Point[] GetDataHull(TItem item, UiState uiState, int delta, bool extend) {
            return this.GetDataHull(item, GetStyle(item, uiState), null, delta, extend);
        }
 
    }
}