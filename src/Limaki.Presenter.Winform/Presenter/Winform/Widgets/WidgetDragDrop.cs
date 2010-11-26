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


//#define TraceDrop

using System;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Presenter.UI;
using Limaki.Presenter.Winform.DragDrop;
using Limaki.Widgets;

namespace Limaki.Presenter.Winform.Widgets {

    /// <summary>
    /// DragDrop support
    /// </summary>
    public class WidgetDragDrop : MouseDragActionBase, IDragDropAction, IKeyAction, IDragDopActionPresenter {
        public WidgetDragDrop():base() {
            this.Priority = ActionPriorities.SelectionPriority + 30;
            //dataObjectHandlerChain.InitDataObjectHanders();
        }

        ICamera camera = null;
        IDragDopControl control = null;
        public WidgetDragDrop( Get<IGraphScene<IWidget,IEdgeWidget>> sceneHandler, IDragDopControl control, ICamera camera , IGraphLayout<IWidget,IEdgeWidget> layout)
            : this() {
            this.control = control;
            this.camera = camera;
            this.SceneHandler = sceneHandler;
            this._layout = layout;
        }

        Get<IGraphScene<IWidget, IEdgeWidget>> SceneHandler;
        public IGraphScene<IWidget, IEdgeWidget> Scene {
            get { return SceneHandler(); }
        }

        private IGraphLayout<IWidget,IEdgeWidget> _layout = null;
        public virtual IGraphLayout<IWidget,IEdgeWidget> Layout {
            get { return _layout; }
            set { _layout = value; }
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

        IWidget HitTest(PointI p) {
            IWidget result = null;
            PointI sp = camera.ToSource(p);
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

        public override void OnMouseDown(MouseActionEventArgs e) {
            if (Scene == null) return;
            base.OnMouseDown(e);
            Resolved = false;
            Dragging = false;
            if (e.Button == MouseActionButtons.Left) {
                Current = HitTest(e.Location);
             }
        }

        public override void OnMouseMove(MouseActionEventArgs e) {
            if (Scene == null) return;
            if (Current == null) return;
            base.OnMouseMove(e);
            Resolved = Resolved && Current != null;
            if (Resolved && (e.Button != MouseActionButtons.Left)) {
                EndAction ();
            }
            if (Resolved && ! Dragging) {
                Dragging = true;
                try {
                    IDataObject myDataObject = facade.SetWidget(this.control, Scene.Graph, Current);
                    DragDropEffects dropEffect =
                        control.DoDragDrop(myDataObject, DragDropEffects.All | DragDropEffects.Link);
                } catch {
                    
                } finally {
                    EndAction ();
                }
            }

        }
        protected override void EndAction() {
            base.EndAction();
            Resolved = false;
            Dragging = false;
        }
        public override void OnMouseUp(MouseActionEventArgs e) {
            base.OnMouseUp(e);
        }
        
        #region IDragDropAction Member

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

        private DragDropFacade facade = new DragDropFacade();


        public virtual void OnDragDrop(DragEventArgs e) {
            
            PointI pt = camera.ToSource(
                control.PointToClient(new PointI(e.X, e.Y))
                );

            if (!facade.DoDragDrop (this.Scene, this.control, e.Data, this.Layout, pt, this.HitSize)) {
                e.Effect = DragDropEffects.None;
            } else {
                ( (Control) this.control ).FindForm ().ActiveControl = ( (Control) this.control );
            }

        }

        public virtual void OnDragOver(DragEventArgs e) {
            if (!facade.IsValidData(e.Data)) {
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

            } 
            //else e.Effect = DragDropEffects.None;

            // get the Item the mouse is below. 

            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.


            PointI pt = camera.ToSource(
                control.PointToClient(new PointI(e.X, e.Y))
                );
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

        #region CopyPaste

        public virtual void Copy() {
            var scene = this.Scene;
            if (scene !=null && scene.Focused != null) {
                Clipboard.SetDataObject(facade.SetWidget(scene.Graph,scene.Focused));
            }
        }

        public virtual void Paste() {
            if (this.Scene == null)
                return;
            IWidget widget = facade.PlaceWidget(Clipboard.GetDataObject(), this.Scene, this.Layout);
            //this.control.CommandsExecute();


        }
        #endregion

        #region IKeyAction Member

        void IKeyAction.OnKeyDown( KeyActionEventArgs e ) {
            if (e.Key == Key.C && 
                e.ModifierKeys == ModifierKeys.Control) {
                if (Scene.Focused != null) {
                    this.Copy ();
                    e.Handled = true;
                }
            }

            if (e.Key == Key.V 
                && e.ModifierKeys == ModifierKeys.Control) {
                this.Paste();
                e.Handled = true;
            }
        }

        void IKeyAction.OnKeyPress( KeyActionPressEventArgs e ) {}

        void IKeyAction.OnKeyUp( KeyActionEventArgs e ) {}

        #endregion

        #region IAction Member
        bool keyActionResolved = false;
        bool IAction.Resolved {
            get { return keyActionResolved; }
        }

        bool keyActionExclusive = false;
        bool IAction.Exclusive {
            get { return keyActionExclusive; }
        }



        #endregion

    }
}