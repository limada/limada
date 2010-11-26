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

using System;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Drawing.UI;
using Limaki.Graphs;
using Limaki.Widgets.Layout;

namespace Limaki.Widgets.UI {
    /// <summary>
    /// Adds a link between two widgets
    /// </summary>
    public class AddEdgeActionBase : EdgeWidgetChangerBase {
        public AddEdgeActionBase(Func<Scene> sceneHandler, IControl control, ICamera camera, ILayout<Scene, IWidget> layout)
            : base(sceneHandler, control, camera) {
            this.Priority = ActionPriorities.SelectionPriority + 10;
            this.Layout = layout;
        }

        private ILayout<Scene, IWidget> _layout = null;
        public virtual ILayout<Scene, IWidget> Layout {
            get { return _layout; }
            set { _layout = value; }
        }

        private IWidget _current = null;
        public virtual IWidget Current {
            get { return _current; }
            set { _current = value; }
        }

        IEdgeWidget _link = null;
        public override IEdgeWidget Edge {
            get { return _link; }
            set { _link = value; }
        }

        public override void OnMouseDown(MouseActionEventArgs e) {
            Edge = null;
            if (Scene == null) return;
            base.OnMouseDown(e);
            if (e.Button == MouseActionButtons.Left) {
                Resolved = (Scene.Focused != null) && (Scene.Focused != Current);
                Current = Scene.Focused;
                LastMousePos = e.Location;
                //Exclusive = true;
            }
        }

        protected virtual IEdgeWidget NewWidget(PointI p) {
            IEdgeWidget result= SceneTools.CreateEdge(this.Scene);
            newCounter++;
            result.Root = Current;
            result.Leaf = Current;
            return result;
        }

        private int newCounter = 1;

        protected override void OnMouseMoveResolved(MouseActionEventArgs e) {
            PointI p = camera.ToSource(e.Location);
            if (Edge == null && Current != null) {
                Edge = NewWidget (p);

                Scene.Add(Edge);
                // see: endAction! Scene.Graph.OnGraphChanged (Edge, GraphChangeType.Add);
                Scene.Commands.Add(new LayoutCommand<IWidget,IShape>(Edge, Layout.CreateShape(Edge),LayoutActionType.Invoke));

                //Scene.Focused = Edge;
                //Scene.Commands.Add(new LayoutCommand<IWidget>(Scene.Focused, LayoutActionType.Perform));

                resizing = true;
                Anchor rootAnchor = Anchor.Center;
                if (Current is IEdgeWidget) {
                    rootAnchor = Anchor.Center;
                } else {
                    IShape shape = Layout.ShapeFactory.Shape<Vector>(p,new SizeI());

                    rootAnchor = NearestAnchorRouter.nearestAnchors(Current.Shape, shape,Current is IEdgeWidget,true).One;
                }
                MouseDownPos = camera.FromSource(Current.Shape[rootAnchor]);
            }

            ShowGrips = true;

            RectangleI rect = camera.ToSource(
                RectangleI.FromLTRB(MouseDownPos.X, MouseDownPos.Y,
                                    LastMousePos.X, LastMousePos.Y));

            Scene.Commands.Add(new ResizeCommand(Edge, rect));
            
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
                    Scene.Commands.Add(new Command<IWidget>(Edge));
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
    }
}