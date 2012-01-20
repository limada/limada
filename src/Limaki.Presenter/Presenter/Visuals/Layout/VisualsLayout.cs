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
using Limaki.Presenter.Layout;
using Limaki.Visuals;

namespace Limaki.Presenter.Visuals.Layout {
    public class VisualsLayout<TItem, TEdge> : GraphLayout<TItem, TEdge>
        where TItem :  IVisual
        where TEdge:  TItem, IEdge<TItem> {
        public VisualsLayout(Get<IGraphScene<TItem, TEdge>> handler, IStyleSheet stylesheet)
            : base(handler, stylesheet) {
            
            this.Router = new NearestAnchorRouter<TItem,TEdge>();
        }


        
        public override IShape GetShape(TItem item) {
            if(item.Shape==null) {
                item.Shape = CreateShape (item);
            }
            return item.Shape;
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

        public virtual void AjustSize(TItem visual, IShape shape) {
            if (!(visual is TEdge)) {
                IStyle style = GetStyle(visual);
                RectangleI invalid = shape.BoundsRect;
                var data = visual.Data;
                if (data == null)
                    data = "<<null>>";
                SizeS textSize = drawingUtils.GetTextDimension(data.ToString(), style);
                SizeI size = SizeI.Add(SizeI.Ceiling(textSize), new SizeI(10, 10));
                if (shape is VectorShape) {
                    size.Height = 0;
                }
                shape.Size = size;
                Data.UpdateBounds(visual, invalid);
            }
        }

        public virtual void AjustSize(TItem visual) {
            if (!(visual is TEdge)) {
                //RectangleI invalid = visual.Shape.BoundsRect;
                AjustSize (visual, visual.Shape);
                //Data.UpdateBounds(visual, invalid);
            }
        }

        public override void Justify(TItem target, IShape tshape) {
            if ((target is IVisualEdge) && (tshape is IEdgeShape)) {
                var edge = (IVisualEdge)target;
                Router.routeEdge((TEdge)edge);

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
            if ((target is TEdge) && (target.Shape is IEdgeShape)) {
                RectangleI invalid = target.Shape.BoundsRect;
                Justify (target, target.Shape);
                Data.UpdateBounds(target, invalid);
            } else {
                AjustSize(target, target.Shape);
            }
        }

        public override void Perform(TItem item) { }


        protected virtual PointI[] GetDataHull(TItem item, IStyle style, Matrice matrix, int delta, bool extend) {
            PointI[] result = null;
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
                    painter.Shape = item.Shape;
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

        public override PointI[] GetDataHull(TItem item, Matrice matrix, int delta, bool extend) {
            return this.GetDataHull(item, GetStyle(item), matrix, delta, extend);
        }
        public override PointI[] GetDataHull(TItem item, UiState uiState, Matrice matrix, int delta, bool extend) {
            return this.GetDataHull(item, GetStyle(item,uiState),matrix,delta,extend);
        }
        public override PointI[] GetDataHull(TItem item, int delta, bool extend) {
            return this.GetDataHull(item, GetStyle(item), null, delta, extend);
        }
        public override PointI[] GetDataHull(TItem item, UiState uiState, int delta, bool extend) {
            return this.GetDataHull(item, GetStyle(item, uiState), null, delta, extend);
        }
 
    }
}