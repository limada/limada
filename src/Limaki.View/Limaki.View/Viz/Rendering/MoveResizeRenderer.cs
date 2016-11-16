/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.ComponentModel;
using Limaki.Actions;
using Limaki.Drawing;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.Viz.Rendering {

    public class MoveResizeRenderer : ISelectionRenderer {
        
        public MoveResizeRenderer() {
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
        Func<ICamera> _camera = null;
        Func<ICamera> ISelectionRenderer.Camera {
            get { return _camera; }
            set { _camera = value; }
        }

        public Func<IClipper> Clipper { get; set; }

        protected GripPainter _gripPainter = null;
        public virtual GripPainter GripPainter {
            get {
                if (_gripPainter == null)
                    _gripPainter = new GripPainter();
                if(_gripPainter!=null) {
                    _gripPainter.GripSize = this.GripSize;
                    _gripPainter.Camera = this.Camera;
                    _gripPainter.Style = this.Style;
                    _gripPainter.TargetShape = this.Shape;
                }
                return _gripPainter;
            }
            set { _gripPainter = value; }
        }
        
        public IVidgetBackend Backend { get; set; }

        public virtual void InvalidateShapeOutline (IShape oldShape, IShape newShape) {
            if (oldShape != null) {
                int halfborder = GripSize + 1;

                var a = oldShape.BoundsRect;
                var b = newShape.BoundsRect;
                if (a.IsEmpty && b.IsEmpty)
                    return;

                var bigger = DrawingExtensions.Union(a, b);
                bigger = Camera.FromSource(bigger);
                bigger = bigger.NormalizedRectangle();

                if (bigger.Width <= halfborder || bigger.Height <= halfborder) {
                    bigger = bigger.Inflate(halfborder, halfborder);
                    Backend.QueueDraw(bigger);
                    Backend.Update();
                } else {
                    bigger = bigger.Inflate(halfborder, halfborder);

                    var smaller = DrawingExtensions.Intersect(a, b);
                    smaller = Camera.FromSource(smaller);
                    smaller = smaller.NormalizedRectangle();
                    smaller = smaller.Inflate(-halfborder, -halfborder);

                    Backend.QueueDraw(
                        Rectangle.FromLTRB(bigger.Left, bigger.Top, bigger.Right, smaller.Top).NormalizedRectangle());

                    Backend.QueueDraw(
                        Rectangle.FromLTRB(bigger.Left, smaller.Bottom, bigger.Right, bigger.Bottom).NormalizedRectangle());

                    Backend.QueueDraw(
                        Rectangle.FromLTRB(bigger.Left, smaller.Top, smaller.Left, smaller.Bottom).NormalizedRectangle());

                    Backend.QueueDraw(
                        Rectangle.FromLTRB(smaller.Right, smaller.Top, bigger.Right, smaller.Bottom).NormalizedRectangle());

                }
            }
        }

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

            var clipper = this.Clipper();
            if (this.Shape != null && clipper != null) {
                var hull = this.Shape.Hull(this.Camera.Matrix, this.GripSize, true);
                clipper.Add(hull);
            }
        }

        public Func<ISurface, object> SaveMatrix { get; set; }
        public Action<ISurface> SetMatrix { get; set; }
        public Action<ISurface, object> RestoreMatrix { get; set; }

        public virtual void OnPaint (IRenderEventArgs e) {
            var shape = this.Shape;
            if ((shape != null) && (ShowGrips)) {

                var save = SaveMatrix(e.Surface);
                SetMatrix(e.Surface);

                GripPainter.Render(e.Surface);
                RestoreMatrix(e.Surface, save);
            }
        }

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