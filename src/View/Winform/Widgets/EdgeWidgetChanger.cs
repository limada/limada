/*
 * Limaki 
 * Version 0.071
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
using Limaki.Widgets;

namespace Limaki.Winform.Widgets {

    /// <summary>
    /// Changes Root or Link of a LinkWidget
    /// </summary>
    public class EdgeWidgetChanger : ShapeActionBase,IDragDropAction {

        public EdgeWidgetChanger(Handler<Scene> sceneHandler, IWinControl control, ICamera camera)
            : base(control, camera) {
            this.SceneHandler = sceneHandler;
            this.Priority = ActionPriorities.SelectionPriority - 20;
        }

        ///<directed>True</directed>
        Handler<Scene> SceneHandler;
        public Scene Scene {
            get { return SceneHandler(); }
        }


        public virtual IEdgeWidget Edge {
            get {
                if (Scene == null) return null;
                if (Scene.Focused is IEdgeWidget) {
                    return (IEdgeWidget)Scene.Focused;
                }
                
                return null;
            }
            set {}
       }

        private IWidget _target = null;
        public IWidget Target {
            get { return _target; }
            set { _target = value; }
        }

        public void SetCursor(Anchor anchor) {
            if (anchor != Anchor.None) {
                Cursor.Current = Cursors.HSplit;
            } else {
                Cursor.Current = savedCursor;
            }
        }

        public override bool HitTest(Point p) {
            bool result = false;
            if (Edge != null) {
                Point sp = camera.ToSource (p);
                hitAnchor = Edge.Shape.IsAnchorHit (sp, HitSize);
                SetCursor (hitAnchor);
                result = hitAnchor != Anchor.None;
            }
            return result;
        }

        // TOTO: Shape is used to draw the grips; find another solution to get
        // rid of ShapeSelectionBase-Inheritance
        public override IShape Shape {
            get {
                if (Edge != null) {
                    return Edge.Shape;
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
            get { return _exclusive;}
            protected set { _exclusive = value; }
        }

        bool rootMoving = false;
        bool checkResizing() {
            return ( this.hitAnchor == Anchor.LeftTop ) ||
                     ( this.hitAnchor == Anchor.RightBottom );
        }
        public override void OnMouseDown(MouseEventArgs e) {
            Resolved = false;
            Exclusive = false;
            if (Edge != null) {
                base.OnMouseDown(e);
                this.resizing = this.resizing && checkResizing();
                Resolved = this.resizing;
                if (Resolved) {
                    rootMoving = Edge.Shape.Location == Edge.Shape[hitAnchor];
                    Exclusive = true;
                    
                }
            }
            //Dragging = !Exclusive;
        }


        protected override void OnMouseMoveResolved(MouseEventArgs e) {
            if ((Edge !=null) && (resizing)) {
                ShowGrips = true;

                Rectangle rect = camera.ToSource(
                            Rectangle.FromLTRB(MouseDownPos.X, MouseDownPos.Y,
                                               LastMousePos.X, LastMousePos.Y));

                Scene.Commands.Add(new ResizeCommand(Edge,rect));

                foreach (IWidget widget in Scene.Twig(Edge)) {
                    Scene.Commands.Add(new LayoutCommand<IWidget>(widget, LayoutActionType.Justify));
                }
                control.CommandsExecute();
                
                // saving current mouse position to be used for next dragging
                this.LastMousePos = e.Location;

                IsTargetHit(this.LastMousePos);
               
            } else {
                Resolved = false;
            }


        }

        protected override void OnMouseMoveNotResolved(MouseEventArgs e) {
            HitTest (e.Location);
            resizing = checkResizing(); 
        }

        public override void OnMouseMove(MouseEventArgs e) {
            Exclusive = false;
            base.OnMouseMove(e);
            if (e.Button != MouseButtons.Left) {
                if (Resolved || Dragging) {
                    EndAction ();
                    Resolved = false;
                    Dragging = false;
                }
                return;
            }
            Dragging = !Resolved;
        }

        public bool IsTargetHit(Point p) {
            bool result = false;
            IWidget widget = Scene.Hovered;
            IEdgeWidget edge = this.Edge;
            if ((widget != null)&&(edge!=null) &&(edge!=widget)) {
                if (edge.Leaf != widget && edge.Root != widget) {
                    Point sp = camera.ToSource (p);
                    result = widget.Shape.IsHit(sp, HitSize);
                }
            }
            if (result) {
                this.Target = widget;
            } else {
                Target = null;
            }
            return result;
        }

        void SetTarget (IEdgeWidget edge, IWidget target) {
            if ((target != edge.Root)&&(target!=edge.Leaf)) {
                Scene.ChangeLink (edge, target, rootMoving);
            }
        }

        protected override void EndAction() {
            if (Edge != null) {
                if (Target != null) {
                    SetTarget(Edge, Target);
                }

                Scene.Commands.Add(new LayoutCommand<IWidget>(Edge, LayoutActionType.Justify));
                foreach (IWidget widget in Scene.Twig(Edge)) {
                    Scene.Commands.Add(new LayoutCommand<IWidget>(widget, LayoutActionType.Justify));
                }
                control.CommandsExecute();
            }
            
            Target = null;
            base.EndAction();
        }
        public override void OnMouseUp(MouseEventArgs e) {
            Exclusive = Resolved;
            base.OnMouseUp(e);
        }

        #region IDragDropAction Member
        bool _dragging = false;
        public virtual bool Dragging {
            get { return _dragging; }
            set { _dragging = value; }
        }
        public void OnGiveFeedback( GiveFeedbackEventArgs e ) {}

        public void OnQueryContinueDrag( QueryContinueDragEventArgs e ) {
            if (Resolved) {
                e.Action = DragAction.Cancel;
                Dragging = false;
            }
        }

        public void OnDragOver( DragEventArgs e ) {
           //Dragging = !Resolved; 
        }

        public void OnDragDrop( DragEventArgs e ) {}
        public void OnDragLeave( EventArgs e ) { }
        #endregion
    }
}