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
 */

using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Presenter.UI;
using Limaki.Widgets;
using Limaki.Presenter.Layout;


namespace Limaki.Presenter.Widgets.UI {
    /// <summary>
    /// Adds a link between two widgets
    /// </summary>
    public class AddEdgeAction : GraphEdgeChangeAction<IWidget,IEdgeWidget> {
        public AddEdgeAction(): base() {
            this.Priority = ActionPriorities.SelectionPriority + 10;
        }

        public virtual Get<IGraphLayout<IWidget, IEdgeWidget>> LayoutHandler { get; set; }
        public virtual IGraphLayout<IWidget, IEdgeWidget> Layout {
            get { return LayoutHandler(); }
        }

        private IWidget _current = default(IWidget);
        public virtual IWidget Current {
            get { return _current; }
            set { _current = value; }
        }

        IEdgeWidget _link = default(IEdgeWidget);
        public override IEdgeWidget Edge {
            get { return _link; }
            set { _link = value; }
        }

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

        protected virtual IEdgeWidget NewWidget(PointI p) {
            IEdgeWidget result = SceneTools.CreateEdge(this.Scene as Scene);
            newCounter++;
            result.Root = Current;
            result.Leaf = Current;
            return result;
        }

        private int newCounter = 1;

        protected override void OnMouseMoveResolved(MouseActionEventArgs e) {
            var camera = this.Camera;
            PointI p = camera.ToSource(e.Location);
            if (Edge == null && Current != null) {
                Edge = NewWidget (p);

                Scene.Add(Edge);
                // see: endAction! Scene.Graph.OnGraphChanged (Edge, GraphChangeType.Add);
                Scene.Requests.Add(new LayoutCommand<IWidget,IShape>(Edge, Layout.CreateShape(Edge),LayoutActionType.Invoke));

                resizing = true;
                Anchor rootAnchor = Anchor.Center;
                if (Current is IEdgeWidget) {
                    rootAnchor = Anchor.Center;
                } else {
                    IShape shape = Layout.ShapeFactory.Shape<Vector>(p,new SizeI());

                    rootAnchor = NearestAnchorRouter<IWidget,IEdgeWidget>.nearestAnchors(Current.Shape, shape,Current is IEdgeWidget,true).One;
                }
                MouseDownPos = camera.FromSource(Current.Shape[rootAnchor]);
            }

            ShowGrips = true;

            RectangleI rect = camera.ToSource(
                RectangleI.FromLTRB(MouseDownPos.X, MouseDownPos.Y,
                                    LastMousePos.X, LastMousePos.Y));

            Scene.Requests.Add(new ResizeCommand<IWidget>(Edge, Scene.ItemShape,rect));
            
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

        protected override void SetTarget(IEdgeWidget edge, IWidget target) {
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
                    Scene.Requests.Add(new Command<IWidget>(Edge));
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