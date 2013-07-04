using System;
using System.Diagnostics;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.UI;
using Limaki.View.Visuals.UI;
using Limaki.Visuals;
using Xwt;
using Xwt.Backends;

namespace Limaki.View.Ui.DragDrop1 {

    public interface IDropAction:IAction, IDropHandler {
        bool Dragging { get; set; }
    }

    /// <summary>
    /// DragDrop support
    /// </summary>
    public class VisualsDragDropAction : MouseDragActionBase, IDropAction {
        public VisualsDragDropAction(): base() {
            this.Priority = ActionPriorities.SelectionPriority + 30;
            this.HitSize = 5;
        }

        public VisualsDragDropAction(Func<IGraphScene<IVisual, IVisualEdge>> sceneHandler, IVidgetBackend backend, ICamera camera, IGraphSceneLayout<IVisual, IVisualEdge> layout)
            : this() {
            this.backend = backend;
            this.camera = camera;
            this.SceneHandler = sceneHandler;
            this.Layout = layout;
        }

        protected virtual ICamera camera { get; set; }
        protected virtual IVidgetBackend backend { get; set; }

        IDragDropBackendHandler _dragDropHandler = null;
        protected virtual IDragDropBackendHandler DragDropHandler {
            get {
                if(_dragDropHandler==null) {
                    _dragDropHandler = Registry.Factory.Create<IDragDropBackendHandler>(backend);
                    _dragDropHandler.Dropped = this.Dropped;
                    _dragDropHandler.DragDataSource = () => this.GetTransferData(Scene.Graph, Source);
                    _dragDropHandler.DragFinished = e => this.DragFinished(e);

                }
                return _dragDropHandler;
            }
        }

        protected Func<IGraphScene<IVisual, IVisualEdge>> SceneHandler { get; set; }
        public IGraphScene<IVisual, IVisualEdge> Scene { get { return SceneHandler(); } }

        public virtual IGraphSceneLayout<IVisual, IVisualEdge> Layout { get; set; }
        public int HitSize { get; set; }

        public IVisual Source { get; set; }

        protected virtual IVisual HitTest(Point p) {
            IVisual result = null;
            var sp = camera.ToSource(p);
            var scene = this.Scene;
            if (scene.Focused != null && scene.Focused.Shape.IsHit(sp, HitSize)) {
                result = scene.Focused;
            }

            return result;
        }

        public override bool Exclusive { get; protected set; }

        public virtual bool Dragging { get; set; }

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
            if (Scene == null) return;
            if (Source == null) return;
            base.OnMouseMove(e);
            Resolved = Resolved && Source != null;
            if (Resolved && (e.Button != MouseActionButtons.Left)) {
                EndAction();
            }
            if (Resolved && !Dragging) {
                Dragging = true;
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

        private object GetDragImageBackend (IGraph<IVisual, IVisualEdge> graph, IVisual Current) {
            return null;
        }

        protected override void EndAction () {
            base.EndAction();
            Resolved = false;
            //Dragging = false;
        }

        protected virtual void DragFinished (DragFinishedEventArgs e) {
            Dragging = false;
        }

        DragDropViz _dragDropViz = null;
        DragDropViz DragDropViz { get { return _dragDropViz ?? (_dragDropViz = new DragDropViz()); } }
        public virtual TransferDataSource GetTransferData (IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            //TODO
            var result = new TransferDataSource();
            result.AddValue<string>(visual.Data.ToString());
            return result;
        }

        public virtual  void Dropped (DragEventArgs e) {
            if (Dragging && Dropping) {
                // the current Drop has started in this instance
                // so we make a link
                var pt = this.backend.PointToClient(e.Position);
                var target = Scene.Hovered;
               
                if (target !=null && Source != target) {
                    SceneExtensions.CreateEdge(Scene, Source, target);
                }

            } else {
                //TODO
                Trace.WriteLine(e.Data.GetValue(TransferDataType.Text));
            }
        }

        public bool Dropping { get; set; }
        public void DragOver (DragOverEventArgs e) {
            Dropping = true;
            //TODO:
            DragDropHandler.SetDragTarget(DragDropAction.All, TransferDataType.Text);
            DragDropHandler.DragOver(e);
        }

        public void OnDrop (DragEventArgs e) {
            DragDropHandler.OnDrop(e);
            Dropping = false;
        }

        public void DragLeave (EventArgs e) {
            DragDropHandler.DragLeave(e);
            Dropping = false;
        }
    }
}