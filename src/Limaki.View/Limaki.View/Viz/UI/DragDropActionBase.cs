using System;
using System.Linq;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.View.DragDrop;
using Limaki.View.Vidgets;
using Xwt;
using DragEventArgs = Limaki.View.DragDrop.DragEventArgs;
using DragOverEventArgs = Limaki.View.DragDrop.DragOverEventArgs;

namespace Limaki.View.Viz.UI {
    /// <summary>
    /// DragDrop support
    /// </summary>
    public abstract class DragDropActionBase : MouseDragActionBase, IDropAction {
        protected DragDropActionBase (): base() {
            this.Priority = ActionPriorities.SelectionPriority + 30;
            this.HitSize = 5;
            
        }
        public DragDropActionBase (IVidgetBackend backend, ICamera camera)
            : this() {
            this.Backend = backend;
            this.Camera = camera;
            SetDragTarget();
        }

        protected virtual ICamera Camera { get; set; }
        protected virtual IVidgetBackend Backend { get; set; }

        IDragDropBackendHandler _dragDropHandler = null;
        protected virtual IDragDropBackendHandler DragDropHandler {
            get {
                if (_dragDropHandler == null) {
                    _dragDropHandler = Registry.Factory.Create<IDragDropBackendHandler>(Backend);
                    _dragDropHandler.Dropped = this.Dropped;
                    _dragDropHandler.DragDataSource = () => this.GetTransferData();
                    _dragDropHandler.DragFinished = e => this.DragFinished(e);
                }
                return _dragDropHandler;
            }
        }

        DragDropContainer _dragDropContainer = null;
        protected virtual DragDropContainer InprocDragDrop {
            get { return _dragDropContainer ?? (_dragDropContainer = Registry.Pooled<DragDropContainer>()); }
        }

        public int HitSize { get; set; }
        public override bool Exclusive { get; protected set; }
        public virtual bool Dragging { get; set; }

        DragDropViz _dragDropViz = null;
        protected virtual DragDropViz DragDropViz { get { return _dragDropViz ?? (_dragDropViz = new DragDropViz()); } }

        public abstract void Dropped (DragEventArgs e);
        public abstract TransferDataSource GetTransferData ();

        protected override void EndAction () {
            base.EndAction();
            Resolved = false;
        }

        protected virtual void DragFinished (DragFinishedEventArgs e) {
            Dragging = false;
            InprocDragDrop.Dragging = false;
        }

        public bool Dropping { get; set; }
        
        public virtual void SetDragTarget() {
            var targets = DragDropViz.DataManager.DataTypes.ToArray();
            DragDropHandler.SetDragTarget(DragDropAction.All, targets);
        }

        public virtual void DragOver (DragOverEventArgs e) {
            Dropping = true;
            DragLeft = false;
            InprocDragDrop.Dropping = true;
            DragDropHandler.DragOver(e);
        }

        public virtual void OnDrop (DragEventArgs e) {
            DragDropHandler.OnDrop(e);
            Dropping = false;
            InprocDragDrop.Dropping = false;
        }

        private bool DragLeft = false;

        public virtual void DragLeave (EventArgs e) {
            DragDropHandler.DragLeave(e);
            // sometimes DragLeave is called before Drop
            DragLeft = true;
        }

        public override void OnMouseMove (MouseActionEventArgs e) {
            if (DragLeft) {
                Dropping = false;
                DragLeft = false;
            }
            base.OnMouseMove(e);
        }

        public override void OnMouseUp (MouseActionEventArgs e) {
            if (DragLeft) {
                Dropping = false;
                DragLeft = false;
            }
            Dragging = false;
            InprocDragDrop.Dragging = false;
            InprocDragDrop.Data = null;
            base.OnMouseUp(e);
        }
    }
}