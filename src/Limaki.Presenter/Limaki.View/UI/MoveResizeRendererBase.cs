using System;
using Limaki.Actions;
using Limaki.Common;
using System.ComponentModel;
using Limaki.Drawing;
using Limaki.View.Rendering;

namespace Limaki.View.UI {
    public abstract class MoveResizeRendererBase : ISelectionRenderer {
        
        public MoveResizeRendererBase() {
            Priority = ActionPriorities.ToolsLayerPriority;
        }
        [DefaultValue(ActionPriorities.ToolsLayerPriority)]
        public int Priority { get; set; }

        public virtual IShape Shape { get; set; }
        public virtual IStyle Style { get; set; }

        [DefaultValue(4)]
        public virtual int GripSize { get; set; }

        bool _showGrips = false;
        public virtual bool ShowGrips {
            get { return _showGrips; }
            set {
                if (_showGrips != value) {
                    _showGrips = value;
                    UpdateSelection();
                }
            }
        }
        
        public ICamera Camera {
            get {
                if (_camera != null) {
                    return _camera();
                }
                return null;
            }
        }
        Get<ICamera> _camera = null;
        Get<ICamera> ISelectionRenderer.Camera {
            get { return _camera; }
            set { _camera = value; }
        }

        public Get<IClipper> Clipper { get; set; }
        public Action<IShape> UpdateGrip { get; set; }

        protected GripPainterBase _gripPainter = null;
        public virtual GripPainterBase GripPainter {
            get {
                if(_gripPainter!=null) {
                    _gripPainter.GripSize = this.GripSize;
                    _gripPainter.Camera = this.Camera;
                    _gripPainter.Style = this.Style;
                    _gripPainter.TargetShape = this.Shape;
                    _gripPainter.UpdateGrip = this.UpdateGrip;
                }
                return _gripPainter;
            }
            set { _gripPainter = value; }
        }
        
        public IControl Device { get; set; }
        public abstract void InvalidateShapeOutline ( IShape oldShape, IShape newShape );

        public virtual void Clear() {
            this.Shape = null;
            this.ShowGrips = false;
            if (_gripPainter != null) {
                _gripPainter.Dispose();
            }
            _gripPainter = null;
        }

        public virtual void UpdateSelection() {
            this.Resolved = true;
            if (UpdateGrip != null) {

                GripPainter.UpdateGrips();
            }

            var clipper = this.Clipper();
            if (this.Shape != null && clipper != null) {
                var hull = this.Shape.Hull(this.Camera.Matrice, this.GripSize, true);
                clipper.Add(hull);
            }
        }

        public abstract void OnPaint ( IRenderEventArgs e );

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                Clear();
            }
        }

        public virtual void Dispose() {
            Dispose(true);
        }

        #region IAction Member
        
        [DefaultValue(true)]
        public virtual bool Resolved {get;set;}

        [DefaultValue(false)]
        public bool Exclusive { get; protected set; }
            
        public bool Enabled { get; set; }
        

        #endregion
    }
}