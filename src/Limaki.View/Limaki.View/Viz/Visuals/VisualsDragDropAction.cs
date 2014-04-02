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
 * 
 */

using System;
using System.Linq;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Iconerias;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz.UI;
using Xwt;
using Xwt.Backends;
using Limaki.View.Properties;
using DragEventArgs = Limaki.View.DragDrop.DragEventArgs;
using DragOverEventArgs = Limaki.View.DragDrop.DragOverEventArgs;

namespace Limaki.View.Viz.Visuals {
    /// <summary>
    /// DragDrop support
    /// </summary>
    public class VisualsDragDropAction : DragDropActionBase, ICopyPasteAction, IKeyAction {

        public VisualsDragDropAction (Func<IGraphScene<IVisual, IVisualEdge>> sceneHandler, IVidgetBackend backend, ICamera camera, IGraphSceneLayout<IVisual, IVisualEdge> layout)
            : base(backend, camera) {

            this.SceneHandler = sceneHandler;
            this.Layout = layout;
        }

        protected Func<IGraphScene<IVisual, IVisualEdge>> SceneHandler { get; set; }
        public IGraphScene<IVisual, IVisualEdge> Scene { get { return SceneHandler(); } }
        public virtual IGraphSceneLayout<IVisual, IVisualEdge> Layout { get; set; }

        public IVisual Source { get; set; }

        /// <summary>
        /// gives back scene.Focused if it is hit by p
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        protected virtual IVisual HitTestFocused (Point p) {
            IVisual result = null;
            var sp = Camera.ToSource(p);
            var scene = this.Scene;
            if (scene.Focused != null && scene.Focused.Shape.IsHit(sp, HitSize)) {
                result = scene.Focused;
            }

            return result;
        }

        public override void OnMouseDown (MouseActionEventArgs e) {
            if (Scene == null) return;
            base.OnMouseDown(e);
            Resolved = false;
            Dragging = false;
            if (e.Button == MouseActionButtons.Left) {
                Source = HitTestFocused(e.Location);
            }
        }

        public override void OnMouseMove (MouseActionEventArgs e) {

            base.OnMouseMove(e);
            Resolved = Resolved && Source != null;

            if (Resolved && (e.Button != MouseActionButtons.Left)) {
                EndAction();
            }
            if (Resolved && !Dragging) {
                Dragging = true;
                InprocDragDrop.Dragging = true;
                InprocDragDrop.Data = new GraphCursor<IVisual, IVisualEdge>(Scene.Graph, Source);

                try {
                    var startData =
                        new DragStartData(
                            DragDropViz.TransferDataOfVisual(Scene.Graph, Source),
                            DragDropAction.All,
                            GetDragImageBackend(Scene.Graph, Source),
                            e.Location.X, e.Location.Y);

                    DragDropHandler.DragStart(startData);
                } catch {} finally {
                    EndAction();
                }
            }
        }

        protected virtual object GetDragImageBackend (IGraph<IVisual, IVisualEdge> graph, IVisual Current) { return Iconery.NewSheet; }

        public override TransferDataSource GetTransferData () { return DragDropViz.TransferDataOfVisual(this.Scene.Graph, this.Scene.Focused); }

        public override void DragOver (DragOverEventArgs e) {
            base.DragOver(e);
            if (InprocDragDrop.Dragging && InprocDragDrop.Data is GraphCursor<IVisual, IVisualEdge>)
                e.AllowedAction = DragDropAction.All;
        }

        public override void Dropped (DragEventArgs e) {
            var pt = Camera.ToSource(e.Position);
            var scene = this.Scene;
            var target = scene.Hovered ?? HitTestFocused (e.Position);
            
            IVisual item = null;

            if (Dragging && Dropping) {
                // the current Drop has started in this instance
                // so we make a link
                if (target != null && Source != target) {
                    SceneExtensions.CreateEdge(scene, Source, target);
                }
                e.Success = true;
                return;
            }

            if (InprocDragDrop.Dragging) {
                var source = InprocDragDrop.Data as GraphCursor<IVisual, IVisualEdge>;
                if (source != null && source.Cursor != target) {
                    item = GraphMapping.Mapping.LookUp(source.Graph, scene.Graph, source.Cursor);
                    if (item == null) {
                        //TODO: error here
                        //return;
                    }
                }
            }

            if (item == null) {
                item = DragDropViz.VisualOfTransferData(scene.Graph, e.Data);
                if (item != null)
                    item.Location = pt;
            }

            if (item != null) {
                SceneExtensions.AddItem (scene, item, Layout, pt);
                if (target != null && !scene.Graph.Edges (target).Any (edge => edge.Leaf == item || edge.Root == item))
                    SceneExtensions.CreateEdge (scene, target, item);
            } else {
                // no known type found to import
                string dt = "not found:\t";
            }

            InprocDragDrop.Data = null;
            InprocDragDrop.Dragging = false;
            Dragging = false;
        }


        public virtual void Paste () {

            var scene = this.Scene;
            IVisual item = null;

            if (InprocDragDrop.ClipboardData != null) {
                // TODO: refactor to use same code as above
                var source = InprocDragDrop.ClipboardData as GraphCursor<IVisual, IVisualEdge>;
                if (source != null && source.Cursor != item) {
                    item = GraphMapping.Mapping.LookUp(source.Graph, scene.Graph, source.Cursor);
                    if (item == null) {
                        //TODO: error here
                        //return;
                    }
                }
            }

            if (item == null) {
                item = DragDropViz.VisualOfTransferData(scene.Graph, Clipboard.GetTransferData(DragDropViz.DataManager.DataTypes));
            }
            if (item != null) {
                SceneExtensions.PlaceVisual(scene, scene.Focused, item, Layout);
            }

        }

        public virtual void Copy () {
            if (false) {
                // TODO: it has to be cleared, if the next stuff is copied into clipboard
                // a clipboard Pasted-Event is needed
                InprocDragDrop.ClipboardData = new GraphCursor<IVisual, IVisualEdge>(Scene.Graph, Scene.Focused);
            }
            Clipboard.SetTransferData(DragDropViz.TransferDataOfVisual(Scene.Graph, Scene.Focused));
        }


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

    }
}