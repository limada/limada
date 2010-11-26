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
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Widgets;
using Limaki.Widgets.Layout;

namespace Limaki.Winform.Widgets {

    /// <summary>
    /// Adds a link between two widgets
    /// </summary>
    public class AddEdgeAction : EdgeWidgetChanger {
        public AddEdgeAction(Handler<Scene> sceneHandler, IWinControl control, ICamera camera)
            : base(sceneHandler, control, camera) {
            this.Priority = ActionPriorities.SelectionPriority - 30;
        }


        private IWidget _current = null;
        public IWidget Current {
            get { return _current; }
            set { _current = value; }
        }

        IEdgeWidget _link = null;
        public override IEdgeWidget Edge {
            get { return _link; }
            set { _link = value; }
        }
        //IWidget override HitTest(Point p) {
        //    IWidget result = null;
        //    Point sp = camera.ToSource(p);

        //    result = Scene.Hit(sp, HitSize);

        //    return result;
        //}

        public override void OnMouseDown(MouseEventArgs e) {
            Edge = null;
            if (Scene == null) return;
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left) {
                Resolved = (Scene.Focused != null) && (Scene.Focused != Current);
                Current = Scene.Focused;
                LastMousePos = e.Location;
                //Exclusive = true;
            }
        }

        protected virtual IEdgeWidget NewWidget(Point p) {
            IEdgeWidget result= new EdgeWidget<string>(newCounter+". Link");
            newCounter++;
            result.Root = Current;
            return result;
        }

        private int newCounter = 1;

        protected override void OnMouseMoveResolved(MouseEventArgs e) {
            Point p = camera.ToSource(e.Location);
            if (Edge == null) {
                Edge = NewWidget (p);
                
                Scene.Add(Edge);
                Scene.Commands.Add(new LayoutCommand<IWidget>(Edge, LayoutActionType.Invoke));
                Scene.Commands.Add(new LayoutCommand<IWidget>(Scene.Focused, LayoutActionType.Perform));

                Scene.Focused = Edge;
                resizing = true;
                Anchor rootAnchor = Anchor.Center;
                if (Current is IEdgeWidget) {
                    rootAnchor = Anchor.Center;
                } else {
                    VectorShape shape = new VectorShape ();
                    
                    shape.Location = p;
                    rootAnchor = NearestAnchorRouter.nearestAnchors(Current.Shape, shape,Current is IEdgeWidget,true).One;
                }
                MouseDownPos = camera.FromSource(Current.Shape[rootAnchor]);
            }

            ShowGrips = true;

            Rectangle rect = camera.ToSource(
                        Rectangle.FromLTRB(MouseDownPos.X, MouseDownPos.Y,
                                           LastMousePos.X, LastMousePos.Y));

            Scene.Commands.Add(new ResizeCommand(Edge, rect));

            control.CommandsExecute();
            
            // saving current mouse position to be used for next dragging
            this.LastMousePos = e.Location;
            
            IsTargetHit(this.LastMousePos);


        }

        public override void OnMouseMove(MouseEventArgs e) {
            Exclusive = false;
            if (Scene == null) return;
            if (Current == null) return;
            base.BaseMouseMove(e);
            Resolved = Resolved && (Scene.Focused != null);
            if (Resolved) {
                OnMouseMoveResolved(e);
            }
            if (e.Button != MouseButtons.Left) {
                EndAction();
                Resolved = false;
                return;
            }
            Dragging = !Resolved;
        }

        protected override void EndAction() {
            if (Edge != null) {
                if ((Current != null) && (Scene.Focused != Current)) {
                    Scene.Focused = Current;
                    Scene.Commands.Add(new LayoutCommand<IWidget>(Scene.Focused, LayoutActionType.Perform));
                }
                if ((Target == null) && (Edge.Leaf == null)) {
                    Scene.Remove(Edge);
                    newCounter--;
                    Scene.Commands.Add(new Command<IWidget>(Edge));
                    Edge = null;
                    control.CommandsExecute();
                } else {
                    base.EndAction();
                }
            } else {
                base.EndAction();
            }
            Edge = null;
            Current = null;            
        }
        public override void OnMouseUp(MouseEventArgs e) {
            Exclusive = Resolved;
            base.OnMouseUp (e);
        }
    }
}