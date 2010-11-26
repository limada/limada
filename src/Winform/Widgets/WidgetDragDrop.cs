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


//#define TraceDrop

using System;
using System.Drawing;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Widgets;
using System.Collections.Generic;
using Limaki.Winform.DragDrop;

namespace Limaki.Winform.Widgets {

    /// <summary>
    /// DragDrop support
    /// </summary>
    public class WidgetDragDrop : MouseDragActionBase, IDragDropAction {
        public WidgetDragDrop():base() {
            this.Priority = ActionPriorities.SelectionPriority + 30;
            dataObjectHandlerChain.InitDataObjectHanders();
        }
        ///<directed>True</directed>
        ICamera camera = null;
        IDragDopControl control = null;
        public WidgetDragDrop( Handler<Scene> sceneHandler, IDragDopControl control, ICamera camera )
            : this() {
            this.control = control;
            this.camera = camera;
            this.SceneHandler = sceneHandler;
        }

        ///<directed>True</directed>
        Handler<Scene> SceneHandler;
        public Scene Scene {
            get { return SceneHandler(); }
        }

        private int _hitSize = 5;
        /// <summary>
        /// has to be the same as in WidgetResizer
        /// </summary>
        public int HitSize {
            get { return _hitSize; }
            set { _hitSize = value; }
        }

        private IWidget _current = null;
        public IWidget Current {
            get { return _current; }
            set { _current = value; }
        }

        IWidget HitTest(Point p) {
            IWidget result = null;
            Point sp = camera.ToSource(p);
            if (Scene.Focused!=null && Scene.Focused.Shape.IsHit(sp, HitSize)) {
                result = Scene.Focused;
            }

            return result;
        }

        bool _exclusive = false;
        public override bool Exclusive {
            get { return _exclusive; }
            protected set { _exclusive = value; }
        }

        public override void OnMouseDown(MouseEventArgs e) {
            if (Scene == null) return;
            base.OnMouseDown(e);
            Resolved = false;
            Dragging = false;
            if (e.Button == MouseButtons.Left) {
                Current = HitTest(e.Location);
             }
        }

        public override void OnMouseMove(MouseEventArgs e) {
            if (Scene == null) return;
            if (Current == null) return;
            base.OnMouseMove(e);
            Resolved = Resolved && Current != null;
            if (Resolved &&(e.Button != MouseButtons.Left)) {
                EndAction ();
            }
            if (Resolved && ! Dragging) {
                Dragging = true;
                ControlDataObject myDataObject = new ControlDataObject();
                myDataObject.control = this.control;
                dataObjectHandlerChain.SetData(myDataObject, Current);
                DragDropEffects dropEffect = control.DoDragDrop(myDataObject, DragDropEffects.All | DragDropEffects.Link);
                EndAction ();
            }

        }
        protected override void EndAction() {
            base.EndAction();
            Resolved = false;
            Dragging = false;
        }
        public override void OnMouseUp( MouseEventArgs e ) {
            base.OnMouseUp(e);
        }
        
        #region IDragDropAction Member

        private DataObjectHandlerChain dataObjectHandlerChain = new DataObjectHandlerChain();

        bool _dragging = false;
        public virtual bool Dragging {
            get { return _dragging; }
            set { _dragging = value; }
        }
        public virtual void OnGiveFeedback(GiveFeedbackEventArgs e) {
            if (Dragging) {
                
            }
        }

        public virtual void OnQueryContinueDrag(QueryContinueDragEventArgs e) {
            if (Dragging) {
                //e.Action = DragAction.Continue;
                
            }
        }

        public virtual void OnDragDrop(DragEventArgs e) {
            IWidget item = dataObjectHandlerChain.GetWidget(e.Data);
            if (item == null) {
                e.Effect = DragDropEffects.None;
                return;
            } 
#if TraceDrop            
            else {
                System.Console.WriteLine ("Dropped:" + item.ToString ());
            }
#endif
            if (e.Data is ControlDataObject) {
                ControlDataObject data = (ControlDataObject) e.Data;
                if (data.control == this.control) {
                    if (Scene.Hovered != null && Scene.Hovered != item) {
                        // make a new link:
                        string s = "[" + item.Data.ToString() +
                                   "->" + Scene.Hovered.Data.ToString () + "]";
                        IEdgeWidget edge = new EdgeWidget<string>(s);
                        edge.Root = item;
                        edge.Leaf = Scene.Hovered;
                        Scene.Add(edge);
                        Scene.Commands.Add(new LayoutCommand<IWidget>(edge, LayoutActionType.Invoke));
                        Scene.Commands.Add(new LayoutCommand<IWidget>(edge, LayoutActionType.Justify));
                        control.CommandsExecute ();
                    }
                }
            } else {
                Point pt = camera.ToSource(control.PointToClient(new Point(e.X, e.Y)));
                Scene.Add(item);
                Scene.Commands.Add(new LayoutCommand<IWidget>(item, LayoutActionType.Invoke));
                Scene.Commands.Add(new MoveCommand(item, pt));
                Scene.Commands.Add(new LayoutCommand<IWidget>(item, LayoutActionType.Justify));
                
                control.CommandsExecute();
            }
        }

        public virtual void OnDragOver(DragEventArgs e) {
            if (!dataObjectHandlerChain.IsValidData(e.Data)) {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Copy;

            bool sourceIsWidgetControl = e.Data is ControlDataObject;
            // Set the effect based upon the KeyState.
            if (( e.KeyState & ( 8 + 32 ) ) == ( 8 + 32 ) &&
                ( e.AllowedEffect & DragDropEffects.Link ) == DragDropEffects.Link) {
                // KeyState 8 + 32 = CTL + ALT

                // Link drag and drop effect.
                e.Effect = DragDropEffects.Link;

            } else if (( e.KeyState & 32 ) == 32 &&
                       ( e.AllowedEffect & DragDropEffects.Link ) == DragDropEffects.Link) {

                // ALT KeyState for link.
                e.Effect = DragDropEffects.Link;

            } else if (( e.KeyState & 4 ) == 4 &&
                       ( e.AllowedEffect & DragDropEffects.Move ) == DragDropEffects.Move) {

                // SHIFT KeyState for move.
                e.Effect = DragDropEffects.Move;

            } else if (( e.KeyState & 8 ) == 8 &&
                       ( e.AllowedEffect & DragDropEffects.Copy ) == DragDropEffects.Copy) {

                // CTL KeyState for copy.
                e.Effect = DragDropEffects.Copy;

            } else if (( e.AllowedEffect & DragDropEffects.Move ) == DragDropEffects.Move) {

                // By default, the drop action should be move, if allowed.
                e.Effect =  DragDropEffects.Copy;

            } else
                e.Effect = DragDropEffects.None;

            // get the Item the mouse is below. 

            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.


            Point pt = camera.ToSource(control.PointToClient(new Point(e.X, e.Y)));
            IWidget itemUnderMouse = Scene.Hit(pt,HitSize);

#if TraceDrop
            if (itemUnderMouse != null) {
                System.Console.Out.WriteLine("drag over "+itemUnderMouse.Data.ToString());

            } else {
                System.Console.Out.WriteLine("drag over empty space");
            }
#endif

        }

        public virtual void OnDragLeave(EventArgs e) {
            Dragging = false;
        }

        #endregion
    }
}