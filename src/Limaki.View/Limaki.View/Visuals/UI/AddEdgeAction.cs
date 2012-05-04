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
 */

using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Layout;
using Limaki.View.UI;
using Limaki.View.UI.GraphScene;
using Limaki.Visuals;
using Xwt;

namespace Limaki.View.Visuals.UI {
    /// <summary>
    /// Adds a link between two Visuals
    /// </summary>
    public class AddEdgeAction : GraphEdgeChangeAction<IVisual,IVisualEdge> {
        public AddEdgeAction(): base() {
            this.Priority = ActionPriorities.SelectionPriority + 10;
        }

        public virtual Get<IGraphSceneLayout<IVisual, IVisualEdge>> LayoutHandler { get; set; }
        public virtual IGraphSceneLayout<IVisual, IVisualEdge> Layout {
            get { return LayoutHandler(); }
        }

        
        public virtual IVisual Current {get;set;}
        public override IVisualEdge Edge { get; set; }

        public override void OnMouseDown(MouseActionEventArgs e) {
            Edge = null;
            if (Scene == null) return;
            base.OnMouseDown(e);
            if (e.Button == MouseActionButtons.Left) {
                Resolved = Scene.Focused != null && !Scene.Focused.Equals(Current);
                Current = Scene.Focused;
                LastMousePos = e.Location;
                //Exclusive = true;
            }
        }

        protected virtual IVisualEdge NewVisual(Point p) {
            IVisualEdge result = SceneExtensions.CreateEdge(this.Scene);
            newCounter++;
            result.Root = Current;
            result.Leaf = Current;
            return result;
        }

        private int newCounter = 1;

        protected override void OnMouseMoveResolved(MouseActionEventArgs e) {
            var camera = this.Camera;
            Point p = camera.ToSource(e.Location);
            if (Edge == null && Current != null) {
                Edge = NewVisual (p);

                Scene.Add(Edge);
                // see: endAction! Scene.Graph.OnGraphChanged (Edge, GraphChangeType.Add);
                Scene.Requests.Add(new LayoutCommand<IVisual,IShape>(Edge, Layout.CreateShape(Edge),LayoutActionType.Invoke));

                resizing = true;
                Anchor rootAnchor = Anchor.Center;
                if (Current is IVisualEdge) {
                    rootAnchor = Anchor.Center;
                } else {
                    IShape shape = Layout.ShapeFactory.Shape<Vector>(p,new Size());

                    rootAnchor = NearestAnchorRouter<IVisual,IVisualEdge>.nearestAnchors(Current.Shape, shape,Current is IVisualEdge,true).One;
                }
                MouseDownPos = camera.FromSource(Current.Shape[rootAnchor]);
            }

            ShowGrips = true;

            var rect = camera.ToSource(
                Rectangle.FromLTRB(MouseDownPos.X, MouseDownPos.Y,
                                    LastMousePos.X, LastMousePos.Y));

            Scene.Requests.Add(new ResizeCommand<IVisual>(Edge, Scene.ItemShape,rect));
            
            // saving current mouse position to be used for next dragging
            this.LastMousePos = e.Location;
            
            IsTargetHit(this.LastMousePos);


        }

        public override void OnMouseMove(MouseActionEventArgs e) {
            Exclusive = false;
            if (Scene == null) return;
            if (Current == null) return;
            base.BaseMouseMove(e);
            Resolved = Resolved && (Scene.Focused != null);
            if (Resolved) {
                OnMouseMoveResolved(e);
            }
            if (e.Button != MouseActionButtons.Left) {
                EndAction();
                Resolved = false;
                return;
            }
            Dragging = !Resolved;
        }

        protected override void SetTarget(IVisualEdge edge, IVisual target) {
            if ((target != edge.Root) && (target != edge.Leaf)) {
                Scene.ChangeEdge(edge, target, false); 
                Scene.Graph.OnGraphChanged(edge, GraphChangeType.Add);
            }
        }

        protected override void EndAction() {
            if (Edge != null) {
                if ((Target == null) && (Edge.Leaf == Edge.Root)) {
                    Scene.Remove(Edge);
                    newCounter--;
                    Scene.Requests.Add(new Command<IVisual>(Edge));
                    Edge = null;
                } else {
                    base.EndAction(); // calls SetTarget
                }
            } else {
                base.EndAction();
            }
            Edge = null;
            Current = null;            
        }

        public override void OnMouseUp(MouseActionEventArgs e) {
            Exclusive = Resolved;
            base.OnMouseUp (e);
        }

        public override bool Check() {
            if (this.LayoutHandler == null) {
                throw new CheckFailedException(this.GetType(), typeof(ICamera));
            }
            return base.Check();
        }
    }
}