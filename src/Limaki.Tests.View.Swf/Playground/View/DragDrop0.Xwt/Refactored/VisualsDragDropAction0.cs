/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 */


//#define TraceDrop

using System;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.View.Ui.DragDrop1;
using Limaki.View.UI;
using Limaki.Visuals;
using Xwt;
using Clipboard = System.Windows.Forms.Clipboard;
using DragEventArgs = Limaki.View.Ui.DragDrop1.DragEventArgs;
using DragOverEventArgs = Limaki.View.Ui.DragDrop1.DragOverEventArgs;

using Key = Xwt.Key;
using ModifierKeys = Xwt.ModifierKeys;
using SWF = System.Windows.Forms;

namespace Limaki.View.Ui.DragDrop0 {

    public interface IXwtDragDopControl : IVidgetBackend {
        bool AllowDrop { get; set; }
        DragDropAction DoDragDrop (object data, DragDropAction allowedEffects);
    }

    public interface IXwtDragDropAction : IAction {
        bool Dragging { get; set; }
        
        void OnDragOver (DragOverEventArgs e);
        void OnDragDrop (Limaki.View.Ui.DragDrop1.DragEventArgs e);
        void OnDragLeave (EventArgs e);
        void Copy ();
        void Paste ();
    }

    /// <summary>
    /// DragDrop support
    /// </summary>
    public class VisualsDragDropAction0 : MouseDragActionBase, IXwtDragDropAction, IKeyAction {
        public VisualsDragDropAction0 (): base() {
            this.Priority = ActionPriorities.SelectionPriority + 30;
            //dataObjectHandlerChain.InitDataObjectHanders();
        }

        ICamera camera = null;
        IXwtDragDopControl backend = null;

        public VisualsDragDropAction0 (Func<IGraphScene<IVisual, IVisualEdge>> sceneHandler, IXwtDragDopControl backend, ICamera camera, IGraphSceneLayout<IVisual, IVisualEdge> layout)
            : this() {
            this.backend = backend;
            this.camera = camera;
            this.SceneHandler = sceneHandler;
            this._layout = layout;
        }

        Func<IGraphScene<IVisual, IVisualEdge>> SceneHandler;
        public IGraphScene<IVisual, IVisualEdge> Scene {
            get { return SceneHandler(); }
        }

        private IGraphSceneLayout<IVisual, IVisualEdge> _layout = null;
        public virtual IGraphSceneLayout<IVisual, IVisualEdge> Layout {
            get { return _layout; }
            set { _layout = value; }
        }

        private int _hitSize = 5;
        /// <summary>
        /// has to be the same as in GraphItemResizer
        /// </summary>
        public int HitSize {
            get { return _hitSize; }
            set { _hitSize = value; }
        }

        private IVisual _current = null;
        public IVisual Current {
            get { return _current; }
            set { _current = value; }
        }

        IVisual HitTest (Point p) {
            IVisual result = null;
            var sp = camera.ToSource(p);
            if (Scene.Focused != null && Scene.Focused.Shape.IsHit(sp, HitSize)) {
                result = Scene.Focused;
            }

            return result;
        }

        bool _exclusive = false;
        public override bool Exclusive {
            get { return _exclusive; }
            protected set { _exclusive = value; }
        }

        public override void OnMouseDown (MouseActionEventArgs e) {
            if (Scene == null) return;
            base.OnMouseDown(e);
            Resolved = false;
            Dragging = false;
            if (e.Button == MouseActionButtons.Left) {
                Current = HitTest(e.Location);
            }
        }

        public override void OnMouseMove (MouseActionEventArgs e) {
            if (Scene == null) return;
            if (Current == null) return;
            base.OnMouseMove(e);
            Resolved = Resolved && Current != null;
            if (Resolved && (e.Button != MouseActionButtons.Left)) {
                EndAction();
            }
            if (Resolved && !Dragging) {
                Dragging = true;
                try {
                    var myDataObject = dragDropViz.SetVisual(this.backend, Scene.Graph, Current);
                    var dropEffect =
                        backend.DoDragDrop(myDataObject, DragDropAction.All | DragDropAction.Link);
                } catch {

                } finally {
                    EndAction();
                }
            }
        }

        protected override void EndAction () {
            base.EndAction();
            Resolved = false;
            Dragging = false;
        }

        public override void OnMouseUp (MouseActionEventArgs e) {
            base.OnMouseUp(e);
        }

        #region IDragDropAction Member


        public virtual bool Dragging { get; set; }
       

        private DragDropViz0 dragDropViz = new DragDropViz0();


        public virtual void OnDragDrop (DragEventArgs e) {

            var pt = camera.ToSource(backend.PointToClient(e.Position));

            if (dragDropViz.DoDragDrop(this.Scene, this.backend, e.Data, this.Layout, pt, this.HitSize)) {
                ((SWF.Control) this.backend).FindForm().ActiveControl = ((SWF.Control) this.backend);
            }

        }

        public virtual void OnDragOver (DragOverEventArgs e) {
            if (!dragDropViz.IsValidData(e.Data)) {
                e.AllowedAction = DragDropAction.None;
                return;
            }

            var pt = camera.ToSource(backend.PointToClient(e.Position));
            var itemUnderMouse = Scene.Hit(pt, HitSize);

#if TraceDrop
            if (itemUnderMouse != null) {
                Trace.WriteLine("drag over "+itemUnderMouse.Data.ToString());

            } else {
                Trace.WriteLine("drag over empty space");
            }
#endif

        }

        public virtual void OnDragLeave (EventArgs e) {
            Dragging = false;
        }

        #endregion

        #region CopyPaste

        public virtual void Copy () {
            var scene = this.Scene;
            if (scene != null && scene.Focused != null) {
                Clipboard.SetDataObject(dragDropViz.SetVisual(scene.Graph, scene.Focused).ToSwf());
            }
        }

        public virtual void Paste () {
            if (this.Scene == null)
                return;
            var visual = dragDropViz.PlaceVisual(Clipboard.GetDataObject().ToXwt(), this.Scene, this.Layout);
            //this.control.CommandsExecute();


        }
        #endregion

        #region IKeyAction Member

        void IKeyAction.OnKeyPressed (KeyActionEventArgs e) {
            if (e.Key == Key.C &&
                e.Modifiers == ModifierKeys.Control) {
                if (Scene.Focused != null) {
                    this.Copy();
                    e.Handled = true;
                }
            }

            if (e.Key == Key.V
                && e.Modifiers == ModifierKeys.Control) {
                this.Paste();
                e.Handled = true;
            }
        }


        void IKeyAction.OnKeyReleased (KeyActionEventArgs e) { }

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