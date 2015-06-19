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
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Modelling;
using Xwt;

namespace Limaki.View.Viz.UI.GraphScene {
    /// <summary>
    /// Changes Root or Link of an Edge
    /// </summary>
    public class GraphEdgeChangeAction<TItem,TEdge> : MoveResizeAction 
    where TEdge:TItem, IEdge<TItem> {

        public GraphEdgeChangeAction(): base() {
            this.Priority = ActionPriorities.SelectionPriority - 200;
        }

        public virtual Func<IGraphScene<TItem, TEdge>> SceneHandler { get; set; }
        public IGraphScene<TItem, TEdge> Scene {
            get { return SceneHandler(); }
        }


        public virtual TEdge Edge {
            get {
                if (Scene == null)
                    return default(TEdge);
                if (Scene.Focused is TEdge) {
                    return (TEdge)Scene.Focused;
                }

                return default(TEdge);
            }
            set { }
        }

        private TItem _target = default(TItem);
        public virtual TItem Target {
            get { return _target; }
            set { _target = value; }
        }

        public virtual bool Dragging { get; set; }

        public override bool HitTest(Point p) {
            bool result = false;
            if (Edge != null) {
                var sp = Camera.ToSource(p);
                hitAnchor = Scene.ItemShape(Edge).IsAnchorHit(sp, HitSize);
                CursorHandler.SetEdgeCursor(hitAnchor);
                result = hitAnchor != Anchor.None;
            }
            return result;
        }

        public override IShape Shape {
            get {
                if (Edge != null) {
                    return Scene.ItemShape(Edge);
                } else {
                    return null;
                }
            }
            set {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        bool _exclusive = false;
        public override bool Exclusive {
            get { return _exclusive; }
            protected set { _exclusive = value; }
        }

        protected bool rootMoving = false;
        protected bool CheckResizing() {
            return (this.hitAnchor == Anchor.LeftTop) || (this.hitAnchor == Anchor.RightBottom);
        }

        public override void OnMouseDown(MouseActionEventArgs e) {
            Resolved = false;
            Exclusive = false;
            if (Edge != null) {
                base.OnMouseDown(e);
                this.resizing = this.resizing && CheckResizing();
                Resolved = this.resizing;
                if (Resolved) {
                    var shape = Scene.ItemShape (Edge);
                    rootMoving = shape.Location == shape[hitAnchor];
                    Exclusive = true;

                }
            }
            //Dragging = !Exclusive;
        }


        protected override void OnMouseMoveResolved(MouseActionEventArgs e) {
            if ((Edge != null) && (resizing)) {
                ShowGrips = true;

                var rect = Camera.ToSource(
                    Rectangle.FromLTRB(MouseDownPos.X, MouseDownPos.Y,
                                        LastMousePos.X, LastMousePos.Y));

                Scene.Requests.Add(new ResizeCommand<TItem>(Edge, Scene.ItemShape,rect));
                foreach (var twigEdge in Scene.Graph.Twig(Edge)) {
                    Scene.Requests.Add(new LayoutCommand<TItem>(twigEdge, LayoutActionType.Justify));
                }

                // saving current mouse position to be used for next dragging
                this.LastMousePos = e.Location;

                IsTargetHit(this.LastMousePos);

            } else {
                Resolved = false;
            }


        }

        Rectangle clipRect = Rectangle.Zero;
        protected override void OnMouseMoveNotResolved(MouseActionEventArgs e) {
            if (HitTest(e.Location)) {
                ShowGrips = true;
            } else {
                ShowGrips = false;
            }
        }

        public override void OnMouseMove(MouseActionEventArgs e) {
            Exclusive = false;
            base.OnMouseMove(e);
            if (e.Button != MouseActionButtons.Left) {
                if (Resolved || Dragging) {
                    EndAction();
                    Resolved = false;
                    Dragging = false;
                }
                return;
            }
            Dragging = !Resolved;
        }

        public virtual bool IsTargetHit(Point p) {
            bool result = false;
            var hovered = Scene.Hovered;
            var edge = this.Edge;
            if ((hovered != null) && (edge != null) && (!edge.Equals(hovered))) {
                if (!hovered.Equals(edge.Leaf) && ! hovered.Equals(edge.Root)) {
                    var sp = Camera.ToSource(p);
                    result = Scene.ItemShape(hovered).IsHit(sp, HitSize);
                }
            }
            if (result) {
                this.Target = hovered;
            } else {
                Target = default(TItem);
            }
            return result;
        }

        protected virtual void SetTarget(TEdge edge, TItem target) {
            if (target != null & !target.Equals(edge.Root) && !target.Equals(edge.Leaf)) {
                Scene.ChangeEdge(edge, target, rootMoving);
                Scene.Graph.OnGraphChange(edge, GraphEventType.Update);
            }
        }

        protected override void EndAction() {
            if (Edge != null) {
                if (Target != null) {
                    SetTarget(Edge, Target);
                }

                Scene.Requests.Add(new LayoutCommand<TItem>(Edge, LayoutActionType.Justify));
                foreach (TItem item in Scene.Graph.Twig(Edge)) {
                    Scene.Requests.Add(new LayoutCommand<TItem>(item, LayoutActionType.Justify));
                }
            }

            Target = default(TItem);
            base.EndAction();
        }

        public override void OnMouseUp(MouseActionEventArgs e) {
            Exclusive = Resolved;
            base.OnMouseUp(e);
        }

        public override bool Check() {
            if (this.SceneHandler == null) {
                throw new CheckFailedException(this.GetType(), typeof(IGraphScene<TItem, TEdge>));
            }
            return base.Check();
        }
    }
}