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

using System.Collections.Generic;
using System.Drawing;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;

namespace Limaki.Widgets.Layout {
    public class WidgetLayout<TData, TItem> : Layout<TData, TItem>
        where TData : Scene
        where TItem : class,IWidget  {
        public WidgetLayout(Handler<TData> handler, IStyleSheet stylesheet):
                base(handler, stylesheet) { }

        private IRouter _router = new NearestAnchorRouter();
        public IRouter Router {
            get { return _router; }
            set { _router = value; }
        }


        protected virtual void InvokeLinks() {
            Scene scene = this.Data as Scene;
            if (scene != null) {
                IGraph<IWidget,ILinkWidget> graph = scene.Graph;
                foreach (IWidget widget in graph) {
                    if (!(widget is ILinkWidget)) {
                        foreach (ILinkWidget link in graph.PreorderEdges(widget)) {
                            Invoke((TItem)link);
                            Justify((TItem)link);
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
                IGraph<IWidget,ILinkWidget> graph = scene.Graph;
                foreach (IWidget widget in graph) {
                    Invoke((TItem)widget);
                    if (!(widget is ILinkWidget)) {
                        Justify((TItem)widget);
                    }
                }
                InvokeLinks();
            }
        }

        protected virtual IShape LinkShape() {
            return new VectorShape ();
        }
        protected virtual IShape WidgetShape() {
            return new RoundedRectangleShape();
        }

        public override void Invoke(TItem item) {
            if (item is ILinkWidget) {
                if (item.Shape == null) {
                    item.Shape = LinkShape ();
                }
            } else {
                if (item.Shape == null) {
                    item.Shape = WidgetShape();
                }
            }
        }

        public override IStyle GetStyle(TItem widget) {
            Scene scene = this.Data as Scene;
            IStyle style = (widget.Style == null ? this.styleSheet : widget.Style);
            bool isSelected = scene.Selected.Contains (widget);
            if (widget is ILinkWidget) {
                if (isSelected) {
                    style = styleSheet.LinkSelectedStyle;
                } else if (widget == scene.Hovered) {
                    style = styleSheet.LinkHoveredStyle;
                } else {
                    style = styleSheet.LinkStyle;
                }
            } else {
                if (isSelected) {
                    style = styleSheet.SelectedStyle;
                } else if (widget == scene.Hovered) {
                    style = styleSheet.HoveredStyle;
                } else {
                    style = styleSheet.DefaultStyle;
                }
            }
            return style;
        }

        public virtual void AjustSize(TItem widget) {
            if (!(widget is ILinkWidget)) {
                IStyle style = styleSheet.DefaultStyle;
                Rectangle invalid = widget.Shape.BoundsRect;
                SizeF textSize = ShapeUtils.GetTextDimension(
                    style.Font, widget.Data.ToString(),style.AutoSize);
                widget.Size = Size.Add(Size.Ceiling(textSize), new Size(10, 10));
                Data.UpdateBounds(widget,invalid);
            }
        }

        public override void Justify(TItem target) {
            if ((target is ILinkWidget) && (target.Shape is ILinkShape)) {
                Rectangle invalid = target.Shape.BoundsRect;
                ILinkWidget link = (ILinkWidget)target;
                Router.routeLink(link);
                ILinkShape shape = (ILinkShape)target.Shape;
                shape.Start = link.Root.Shape[link.RootAnchor];
                shape.End = link.Leaf.Shape[link.LeafAnchor];
                Data.UpdateBounds(target, invalid);
            } else {
                AjustSize(target);
            }
        }

        public override void Perform(TItem item) {
            
        }
        


        }
}