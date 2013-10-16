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
using System.Diagnostics;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.UI;
using Limaki.View.Visuals.UI;
using Limaki.Visuals;
using Xwt;
using Xwt.Backends;
using Limaki.Graphs.Extensions;

namespace Limaki.View.DragDrop {
    /// <summary>
    /// DragDrop support
    /// </summary>
    public class VisualsDragDropAction : DragDropActionBase {
        
        public VisualsDragDropAction(Func<IGraphScene<IVisual, IVisualEdge>> sceneHandler, IVidgetBackend backend, ICamera camera, IGraphSceneLayout<IVisual, IVisualEdge> layout)
            : base(backend,camera) {

            this.SceneHandler = sceneHandler;
            this.Layout = layout;
        }

        protected Func<IGraphScene<IVisual, IVisualEdge>> SceneHandler { get; set; }
        public IGraphScene<IVisual, IVisualEdge> Scene { get { return SceneHandler(); } }
        public virtual IGraphSceneLayout<IVisual, IVisualEdge> Layout { get; set; }

        public IVisual Source { get; set; }

        protected virtual IVisual HitTest(Point p) {
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
                Source = HitTest(e.Location);
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
                        new DragStartData(GetTransferData(Scene.Graph, Source), 
                            DragDropAction.All,
                            GetDragImageBackend(Scene.Graph, Source), 
                            e.Location.X, e.Location.Y);
                    
                    DragDropHandler.DragStart(startData);
                } catch {

                } finally {
                    EndAction();
                }
            }
        }

        public virtual TransferDataSource GetTransferData (IGraph<IVisual,IVisualEdge> graph, IVisual visual) {
            if (graph == null || visual == null)
                return null;

            var result = new TransferDataSource();
            result.AddValue<string>(visual.Data.ToString());
            result.AddValue<IVisual>(visual);
            return result;
        }

        public override TransferDataSource GetTransferData () {
            return GetTransferData(this.Scene.Graph, this.Source);
        }

        public override void Dropped (DragEventArgs e) {
            var pt = Camera.ToSource(this.Backend.PointToClient(e.Position));
            var scene = this.Scene;
            var target = scene.Hovered;
            IVisual item = null;

            if (Dragging && Dropping) {
                // the current Drop has started in this instance
                // so we make a link
                if (target != null && Source != target) {
                    SceneExtensions.CreateEdge(scene, Source, target);
                }
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
                item = DragDropViz.VisualOfTransferData(scene.Graph,e.Data);
                if (item != null)
                    item.Location = pt;
            }

            if (item != null) {
                SceneExtensions.AddItem(scene, item, Layout, pt);
                if (target != null)
                    SceneExtensions.CreateEdge(scene, target, item);
            } else {
                // no known type found to import
                string dt = "not found:\t";
                foreach (var d in e.Data.DataTypes) dt += d.Id + " | ";
                Trace.WriteLine(dt);
            }

        }

        
    }
}